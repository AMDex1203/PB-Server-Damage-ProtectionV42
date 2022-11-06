
// Type: Game.GameManager
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.managers.server;
using Core.server;
using Core.xml;
using Game.data.model;
using Game.global.serverpacket;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Game
{
  public static class GameManager
  {
    public static ServerConfig Config;
    public static Socket mainSocket;
    public static bool ServerIsClosed;
    public static ConcurrentDictionary<uint, GameClient> _socketList = new ConcurrentDictionary<uint, GameClient>();

    public static bool Start()
    {
      try
      {
        GameManager.Config = ServerConfigSyncer.GenerateConfig(ConfigGS.configId);
        GameManager.mainSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        GameManager.mainSocket.DontFragment = false;
        GameManager.mainSocket.NoDelay = true;
        GameManager.mainSocket.Bind((EndPoint) new IPEndPoint(IPAddress.Parse(ConfigGS.gameIp), ConfigGS.gamePort));
        GameManager.mainSocket.Listen(3);
        GameManager.mainSocket.BeginAccept(new AsyncCallback(GameManager.AcceptCallback), (object) GameManager.mainSocket);
        return true;
      }
      catch (Exception ex)
      {
        Logger.error(ex.ToString());
        return false;
      }
    }

    private static void AcceptCallback(IAsyncResult result)
    {
      if (GameManager.ServerIsClosed)
        return;
      Socket asyncState = (Socket) result.AsyncState;
      try
      {
        Socket client = asyncState.EndAccept(result);
        if (client != null)
        {
          GameClient sck = new GameClient(client);
          GameManager.AddSocket(sck);
          if (sck == null)
            Console.WriteLine("GameClient destruído após falha ao adicionar na lista.");
          Thread.Sleep(5);
        }
      }
      catch
      {
        Logger.warning("[Failed a GC connection] " + DateTime.Now.ToString("dd/MM/yy HH:mm"));
      }
      GameManager.mainSocket.BeginAccept(new AsyncCallback(GameManager.AcceptCallback), (object) GameManager.mainSocket);
    }

    public static void SendUpToAll(Account player)
    {
      if (!GameManager.RankMessages(player._rank))
        return;
      using (SERVER_MESSAGE_ANNOUNCE_PAK messageAnnouncePak = new SERVER_MESSAGE_ANNOUNCE_PAK(string.Format("O jogador {0} upou para o rank {1}.", (object) player.player_name, (object) RankXML.getRank(player._rank)._name)))
        GameManager.SendPacketToAllClients((SendPacket) messageAnnouncePak);
    }

    public static void AddSocket(GameClient sck)
    {
      if (sck == null)
        return;
      uint num = 0;
      while (num < 100000U)
      {
        uint key = ++num;
        if (!GameManager._socketList.ContainsKey(key) && GameManager._socketList.TryAdd(key, sck))
        {
          sck.SessionId = key;
          sck.Start();
          return;
        }
      }
      sck.Close(500);
    }

    public static bool RemoveSocket(GameClient sck) => sck != null && sck.SessionId != 0U && (GameManager._socketList.ContainsKey(sck.SessionId) && GameManager._socketList.TryGetValue(sck.SessionId, out sck)) && GameManager._socketList.TryRemove(sck.SessionId, out sck);

    public static int SendPacketToAllClients(SendPacket packet)
    {
      int num = 0;
      if (GameManager._socketList.Count == 0)
        return num;
      byte[] completeBytes = packet.GetCompleteBytes("GameManager.SendPacketToAllClients");
      foreach (GameClient gameClient in (IEnumerable<GameClient>) GameManager._socketList.Values)
      {
        Account player = gameClient._player;
        if (player != null && player._isOnline)
        {
          player.SendCompletePacket(completeBytes);
          ++num;
        }
      }
      return num;
    }

    public static Account SearchActiveClient(long accountId)
    {
      if (GameManager._socketList.Count == 0)
        return (Account) null;
      foreach (GameClient gameClient in (IEnumerable<GameClient>) GameManager._socketList.Values)
      {
        Account player = gameClient._player;
        if (player != null && player.player_id == accountId)
          return player;
      }
      return (Account) null;
    }

    public static Account SearchActiveClient(uint sessionId)
    {
      if (GameManager._socketList.Count == 0)
        return (Account) null;
      foreach (GameClient gameClient in (IEnumerable<GameClient>) GameManager._socketList.Values)
      {
        if (gameClient._player != null && (int) gameClient.SessionId == (int) sessionId)
          return gameClient._player;
      }
      return (Account) null;
    }

    public static int KickActiveClient(double Hours)
    {
      int num = 0;
      DateTime now = DateTime.Now;
      foreach (GameClient gameClient in (IEnumerable<GameClient>) GameManager._socketList.Values)
      {
        Account player = gameClient._player;
        if (player != null && player._room == null && (player.channelId > -1 && !player.IsGM()) && (now - player.LastLobbyEnter).TotalHours >= Hours)
        {
          ++num;
          player.Close(5000);
        }
      }
      return num;
    }

    public static int KickCountActiveClient(double Hours)
    {
      int num = 0;
      DateTime now = DateTime.Now;
      foreach (GameClient gameClient in (IEnumerable<GameClient>) GameManager._socketList.Values)
      {
        Account player = gameClient._player;
        if (player != null && player._room == null && (player.channelId > -1 && !player.IsGM()) && (now - player.LastLobbyEnter).TotalHours >= Hours)
          ++num;
      }
      return num;
    }

    public static bool RankMessages(int rank) => rank == 31 || rank == 36 || (rank == 41 || rank == 46) || (rank == 47 || rank == 48 || (rank == 49 || rank == 50)) || rank == 51;
  }
}
