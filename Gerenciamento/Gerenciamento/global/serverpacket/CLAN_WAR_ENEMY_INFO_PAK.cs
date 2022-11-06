
// Type: Game.global.serverpacket.CLAN_WAR_ENEMY_INFO_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;
using Game.data.model;

namespace Game.global.serverpacket
{
  public class CLAN_WAR_ENEMY_INFO_PAK : SendPacket
  {
    public Match mt;

    public CLAN_WAR_ENEMY_INFO_PAK(Match match) => this.mt = match;

    public override void write()
    {
      this.writeH((short) 1574);
      this.writeH((short) this.mt.getServerInfo());
      this.writeC((byte) this.mt._matchId);
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
    }
  }
}
