
// Type: Auth.data.sync.server_side.SEND_REFRESH_ACC
// Assembly: pbserver_auth, Version=1.0.7796.38604, Culture=neutral, PublicKeyToken=null
// MVID: 1CFDFF97-782C-4EF8-A621-6E4FDDE32436
// Interprise: C:\Users\Cuzin\3,50pbserver_auth.exe

using Auth.data.managers;
using Auth.data.model;
using Core.models.account;
using Core.models.account.players;
using Core.models.servers;
using Core.server;
using Core.xml;

namespace Auth.data.sync.server_side
{
  public static class SEND_REFRESH_ACC
  {
    public static void RefreshAccount(Account player, bool isConnect)
    {
      Auth_SyncNet.UpdateGSCount(0);
      AccountManager.getInstance().getFriendlyAccounts(player.FriendSystem);
      for (int index = 0; index < player.FriendSystem._friends.Count; ++index)
      {
        Friend friend = player.FriendSystem._friends[index];
        PlayerInfo player1 = friend.player;
        if (player1 != null)
        {
          GameServerModel server = ServersXML.getServer((int) player1._status.serverId);
          if (server != null)
            SEND_REFRESH_ACC.SendRefreshPacket(0, player.player_id, friend.player_id, isConnect, server);
        }
      }
      if (player.clan_id <= 0)
        return;
      for (int index = 0; index < player._clanPlayers.Count; ++index)
      {
        Account clanPlayer = player._clanPlayers[index];
        if (clanPlayer != null && clanPlayer._isOnline)
        {
          GameServerModel server = ServersXML.getServer((int) clanPlayer._status.serverId);
          if (server != null)
            SEND_REFRESH_ACC.SendRefreshPacket(1, player.player_id, clanPlayer.player_id, isConnect, server);
        }
      }
    }

    public static void SendRefreshPacket(
      int type,
      long playerId,
      long memberId,
      bool isConnect,
      GameServerModel gs)
    {
      using (SendGPacket sendGpacket = new SendGPacket())
      {
        sendGpacket.writeH((short) 11);
        sendGpacket.writeC((byte) type);
        sendGpacket.writeC(isConnect);
        sendGpacket.writeQ(playerId);
        sendGpacket.writeQ(memberId);
        Auth_SyncNet.SendPacket(sendGpacket.mstream.ToArray(), gs.Connection);
      }
    }
  }
}
