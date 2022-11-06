
// Type: Game.data.sync.server_side.SEND_FRIENDS_INFOS
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.models.account;
using Core.models.servers;
using Core.server;
using Game.data.model;

namespace Game.data.sync.server_side
{
  public class SEND_FRIENDS_INFOS
  {
    public static void Load(Account player, Friend friend, int type)
    {
      if (player == null)
        return;
      GameServerModel server = Game_SyncNet.GetServer(player._status);
      if (server == null)
        return;
      using (SendGPacket sendGpacket = new SendGPacket())
      {
        sendGpacket.writeH((short) 17);
        sendGpacket.writeQ(player.player_id);
        sendGpacket.writeC((byte) type);
        sendGpacket.writeQ(friend.player_id);
        if (type != 2)
        {
          sendGpacket.writeC((byte) friend.state);
          sendGpacket.writeC(friend.removed);
        }
        Game_SyncNet.SendPacket(sendGpacket.mstream.ToArray(), server.Connection);
      }
    }
  }
}
