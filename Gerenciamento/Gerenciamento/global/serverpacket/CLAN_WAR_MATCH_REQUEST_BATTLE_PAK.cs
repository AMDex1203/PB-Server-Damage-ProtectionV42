
// Type: Game.global.serverpacket.CLAN_WAR_MATCH_REQUEST_BATTLE_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;
using Game.data.model;

namespace Game.global.serverpacket
{
  public class CLAN_WAR_MATCH_REQUEST_BATTLE_PAK : SendPacket
  {
    public Match mt;
    public Account p;

    public CLAN_WAR_MATCH_REQUEST_BATTLE_PAK(Match match, Account p)
    {
      this.mt = match;
      this.p = p;
    }

    public override void write()
    {
      this.writeH((short) 1555);
      this.writeH((short) this.mt._matchId);
      this.writeH((ushort) this.mt.getServerInfo());
      this.writeH((ushort) this.mt.getServerInfo());
      this.writeC((byte) this.mt._state);
      this.writeC((byte) this.mt.friendId);
      this.writeC((byte) this.mt.formação);
      this.writeC((byte) this.mt.getCountPlayers());
      this.writeD(this.mt._leader);
      this.writeC((byte) 0);
      this.writeD(this.mt.clan.id);
      this.writeC(this.mt.clan.rank);
      this.writeD(this.mt.clan.logo);
      this.writeS(this.mt.clan.name, 17);
      this.writeT(this.mt.clan.pontos);
      this.writeC(this.mt.clan.nameColor);
      if (this.p != null)
      {
        this.writeC((byte) this.p._rank);
        this.writeS(this.p.player_name, 33);
        this.writeQ(this.p.player_id);
        this.writeC((byte) this.mt._slots[this.mt._leader].state);
      }
      else
        this.writeB(new byte[43]);
    }
  }
}
