
// Type: Auth.data.sync.client_side.Net_Server_Warning
// Assembly: pbserver_auth, Version=1.0.7796.38604, Culture=neutral, PublicKeyToken=null
// MVID: 1CFDFF97-782C-4EF8-A621-6E4FDDE32436
// Interprise: C:\Users\Cuzin\3,50pbserver_auth.exe

using Auth.data.managers;
using Auth.data.model;
using Auth.global.serverpacket;
using Core;
using Core.managers;
using Core.server;
using Core.xml;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;

namespace Auth.data.sync.client_side
{
  public static class Net_Server_Warning
  {
    public static void LoadGMWarning(ReceiveGPacket p)
    {
      string str1 = p.readS((int) p.readC());
      string text = p.readS((int) p.readC());
      string msg = p.readS((int) p.readH());
      string str2 = ComDiv.gen5(text);
      Account accountDb = AccountManager.getInstance().getAccountDB((object) str1, (object) str2, 2, 0);
      if (accountDb == null || accountDb.access <= 3)
        return;
      int num = 0;
      using (SERVER_MESSAGE_ANNOUNCE_PAK messageAnnouncePak = new SERVER_MESSAGE_ANNOUNCE_PAK(msg))
        num = LoginManager.SendPacketToAllClients((SendPacket) messageAnnouncePak);
      string[] strArray1 = new string[9]
      {
        "[SM] Mensagem enviada a ",
        num.ToString(),
        " jogadores: ",
        msg,
        "; by Login: '",
        str1,
        "'; Date: '",
        null,
        null
      };
      DateTime now = DateTime.Now;
      strArray1[7] = now.ToString("dd/MM/yy HH:mm");
      strArray1[8] = "'";
      Logger.warning(string.Concat(strArray1));
      string[] strArray2 = new string[9]
      {
        "[Via SM] Mensagem enviada a ",
        num.ToString(),
        " jogadores: ",
        msg,
        "; by Login: '",
        str1,
        "'; Date: '",
        null,
        null
      };
      now = DateTime.Now;
      strArray2[7] = now.ToString("dd/MM/yy HH:mm");
      strArray2[8] = "'";
      Logger.LogCMD(string.Concat(strArray2));
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
      string str1 = p.readS((int) p.readC());
      string str2 = ComDiv.gen5(p.readS((int) p.readC()));
      Account accountDb = AccountManager.getInstance().getAccountDB((object) str1, (object) str2, 2, 0);
      if (accountDb == null || !(accountDb.password == str2) || accountDb.access < 4)
        return;
      int num = 0;
      foreach (LoginClient loginClient in (IEnumerable<LoginClient>) LoginManager._socketList.Values)
      {
        loginClient._client.Shutdown(SocketShutdown.Both);
        loginClient._client.Close(10000);
        ++num;
      }
      Logger.warning("[SM] Jogadores Desconectados: " + num.ToString() + ". (By: " + str1 + ")");
      LoginManager.ServerIsClosed = true;
      LoginManager.mainSocket.Close(5000);
      Logger.warning("[SM] 1/2 Step");
      Thread.Sleep(5000);
      Auth_SyncNet.udp.Close();
      Logger.warning("[SM] 2/2 Step.");
      foreach (LoginClient loginClient in (IEnumerable<LoginClient>) LoginManager._socketList.Values)
        loginClient.Close(0, true);
      Logger.warning("[SM] Servidor foi completamente desligado.");
      Logger.LogCMD("[Via SM] Shutdowned Server: " + num.ToString() + " jogadores desconectados; by Login: '" + str1 + "'; Date: '" + DateTime.Now.ToString("dd/MM/yy HH:mm") + "'");
    }
  }
}
