
// Type: Game.global.serverpacket.BASE_USER_CHANGE_STATS_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.models.account.players;
using Core.server;

namespace Game.global.serverpacket
{
  public class BASE_USER_CHANGE_STATS_PAK : SendPacket
  {
    private PlayerStats s;

    public BASE_USER_CHANGE_STATS_PAK(PlayerStats s) => this.s = s;

    public override void write()
    {
      this.writeH((short) 2610);
      this.writeD(this.s.fights);
      this.writeD(this.s.fights_win);
      this.writeD(this.s.fights_lost);
      this.writeD(this.s.fights_draw);
      this.writeD(this.s.kills_count);
      this.writeD(this.s.headshots_count);
      this.writeD(this.s.deaths_count);
      this.writeD(this.s.totalfights_count);
      this.writeD(this.s.totalkills_count);
      this.writeD(this.s.escapes);
      this.writeD(this.s.fights);
      this.writeD(this.s.fights_win);
      this.writeD(this.s.fights_lost);
      this.writeD(this.s.fights_draw);
      this.writeD(this.s.kills_count);
      this.writeD(this.s.headshots_count);
      this.writeD(this.s.deaths_count);
      this.writeD(this.s.totalfights_count);
      this.writeD(this.s.totalkills_count);
      this.writeD(this.s.escapes);
    }
  }
}
