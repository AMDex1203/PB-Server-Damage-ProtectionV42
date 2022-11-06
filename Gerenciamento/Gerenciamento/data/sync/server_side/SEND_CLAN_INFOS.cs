
// Type: Game.data.sync.server_side.SEND_CLAN_INFOS
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.models.account.clan;
using Core.models.servers;
using Core.server;
using Core.xml;
using Game.data.model;

namespace Game.data.sync.server_side
{
  public class SEND_CLAN_INFOS
  {
    public static void Load(Account pl, Account member, int type)
    {
      if (pl == null)
        return;
      GameServerModel server = Game_SyncNet.GetServer(pl._status);
      if (server == null)
        return;
      using (SendGPacket sendGpacket = new SendGPacket())
      {
        sendGpacket.writeH((short) 16);
        sendGpacket.writeQ(pl.player_id);
        sendGpacket.writeC((byte) type);
        switch (type)
        {
          case 1:
            sendGpacket.writeQ(member.player_id);
            sendGpacket.writeC((byte) (member.player_name.Length + 1));
            sendGpacket.writeS(member.player_name, member.player_name.Length + 1);
            sendGpacket.writeB(member._status.buffer);
            sendGpacket.writeC((byte) member._rank);
            break;
          case 2:
            sendGpacket.writeQ(member.player_id);
            break;
          case 3:
            sendGpacket.writeD(pl.clanId);
            sendGpacket.writeC((byte) pl.clanAccess);
            break;
        }
        Game_SyncNet.SendPacket(sendGpacket.mstream.ToArray(), server.Connection);
      }
    }

    public static void Update(Clan clan, int type)
    {
      foreach (GameServerModel server in ServersXML._servers)
      {
        if (server._id != 0 && server._id != ConfigGS.serverId)
        {
          using (SendGPacket sendGpacket = new SendGPacket())
          {
            sendGpacket.writeH((short) 22);
            sendGpacket.writeC((byte) type);
            switch (type)
            {
              case 0:
                sendGpacket.writeQ(clan.ownerId);
                break;
              case 1:
                sendGpacket.writeC((byte) (clan.name.Length + 1));
                sendGpacket.writeS(clan.name, clan.name.Length + 1);
                break;
              case 2:
                sendGpacket.writeC(clan.nameColor);
                break;
            }
            Game_SyncNet.SendPacket(sendGpacket.mstream.ToArray(), server.Connection);
          }
        }
      }
    }

    public static void Load(Clan clan, int type)
    {
      foreach (GameServerModel server in ServersXML._servers)
      {
        if (server._id != 0 && server._id != ConfigGS.serverId)
        {
          using (SendGPacket sendGpacket = new SendGPacket())
          {
            sendGpacket.writeH((short) 21);
            sendGpacket.writeC((byte) type);
            sendGpacket.writeD(clan.id);
            if (type == 0)
            {
              sendGpacket.writeQ(clan.ownerId);
              sendGpacket.writeD(clan.creationDate);
              sendGpacket.writeC((byte) (clan.name.Length + 1));
              sendGpacket.writeS(clan.name, clan.name.Length + 1);
              sendGpacket.writeC((byte) (clan.informations.Length + 1));
              sendGpacket.writeS(clan.informations, clan.informations.Length + 1);
            }
            Game_SyncNet.SendPacket(sendGpacket.mstream.ToArray(), server.Connection);
          }
        }
      }
    }
  }
}
