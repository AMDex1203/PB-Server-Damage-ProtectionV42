
// Type: Game.data.sync.client_side.Net_Server_Warning
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.managers;
using Core.models.enums;
using Core.server;
using Core.xml;
using Game.data.managers;
using Game.data.model;
using Game.global.serverpacket;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;

namespace Game.data.sync.client_side
{
  public class Net_Server_Warning
  {
    public static void LoadGMWarning(ReceiveGPacket p)
    {
      string text1 = p.readS((int) p.readC());
      string text2 = p.readS((int) p.readC());
      string msg = p.readS((int) p.readH());
      Account account = AccountManager.getAccount(text1, 0, 0);
      if (account == null || !(account.password == ComDiv.gen5(text2)) || account.access < AccessLevel.GameMaster)
        return;
      int num = 0;
      using (SERVER_MESSAGE_ANNOUNCE_PAK messageAnnouncePak = new SERVER_MESSAGE_ANNOUNCE_PAK(msg))
        num = GameManager.SendPacketToAllClients((SendPacket) messageAnnouncePak);
      Logger.warning("[SM] Mensagem enviada a " + num.ToString() + " jogadores: " + msg + "; by Login: '" + text1 + "'; Date: '" + DateTime.Now.ToString("dd/MM/yy HH:mm") + "'");
      Logger.LogCMD("[Via SM] Mensagem enviada a " + num.ToString() + " jogadores: " + msg + "; by Login: '" + text1 + "'; Date: '" + DateTime.Now.ToString("dd/MM/yy HH:mm") + "'");
    }

    public static void LoadShopRestart(ReceiveGPacket p)
    {
      int type = (int) p.readC();
      ShopManager.Reset();
      ShopManager.Load(type);
      Logger.warning("[SM] Shop reiniciada. (Type: " + type.ToString() + ")");
      Logger.LogCMD("[Via SM] Shop reiniciada. (Type: " + type.ToString() + "); Date: '" + DateTime.Now.ToString("dd/MM/yy HH:mm") + "'");
    }

    public static void LoadServerUpdate(ReceiveGPacket p)
    {
      int serverId = (int) p.readC();
      ServersXML.UpdateServer(serverId);
      Logger.warning("[SM] Servidor " + serverId.ToString() + " atualizado.");
      Logger.LogCMD("[Via SM] Servidor " + serverId.ToString() + " atualizado.; Date: '" + DateTime.Now.ToString("dd/MM/yy HH:mm") + "'");
    }

    public static void LoadShutdown(ReceiveGPacket p)
    {
      string text1 = p.readS((int) p.readC());
      string text2 = p.readS((int) p.readC());
      Account account = AccountManager.getAccount(text1, 0, 0);
      if (account == null || !(account.password == ComDiv.gen5(text2)) || account.access < AccessLevel.Admin)
        return;
      int num = 0;
      foreach (GameClient gameClient in (IEnumerable<GameClient>) GameManager._socketList.Values)
      {
        gameClient._client.Shutdown(SocketShutdown.Both);
        gameClient.Close(5000);
        ++num;
      }
      Logger.warning("[SM] Jogadores Desconectados: " + num.ToString() + ". (By: " + text1 + ")");
      GameManager.ServerIsClosed = true;
      GameManager.mainSocket.Close(5000);
      Logger.warning("[SM] 1/2 Step");
      Thread.Sleep(5000);
      Game_SyncNet.udp.Close();
      Logger.warning("[SM] 2/2 Step.");
      foreach (GameClient gameClient in (IEnumerable<GameClient>) GameManager._socketList.Values)
        gameClient.Close(0);
      Logger.warning("[SM] Servidor foi completamente desligado.");
      Logger.LogCMD("[Via SM] Shutdowned Server: " + num.ToString() + " jogadores desconectados; by Login: '" + text1 + "'; Date: '" + DateTime.Now.ToString("dd/MM/yy HH:mm") + "'");
    }
  }
}
