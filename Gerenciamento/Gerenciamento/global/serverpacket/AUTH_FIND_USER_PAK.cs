
// Type: Game.global.serverpacket.AUTH_FIND_USER_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;
using Game.data.managers;
using Game.data.model;

namespace Game.global.serverpacket
{
  public class AUTH_FIND_USER_PAK : SendPacket
  {
    private uint _erro;
    private Account player;

    public AUTH_FIND_USER_PAK(uint erro, Account player)
    {
      this._erro = erro;
      this.player = player;
    }

    public override void write()
    {
      this.writeH((short) 298);
      this.writeD(this._erro);
      if (this._erro != 0U)
        return;
      this.writeD(this.player._rank);
      this.writeD(ComDiv.GetPlayerStatus(this.player._status, this.player._isOnline));
      this.writeS(ClanManager.getClan(this.player.clanId).name, 17);
      this.writeD(this.player._statistic.fights);
      this.writeD(this.player._statistic.fights_win);
      this.writeD(this.player._statistic.fights_lost);
      this.writeD(this.player._statistic.fights_draw);
      this.writeD(this.player._statistic.kills_count);
      this.writeD(this.player._statistic.headshots_count);
      this.writeD(this.player._statistic.deaths_count);
      this.writeD(this.player._statistic.totalfights_count);
      this.writeD(this.player._statistic.totalkills_count);
      this.writeD(this.player._statistic.escapes);
      this.writeD(this.player._statistic.fights);
      this.writeD(this.player._statistic.fights_win);
      this.writeD(this.player._statistic.fights_lost);
      this.writeD(this.player._statistic.fights_draw);
      this.writeD(this.player._statistic.kills_count);
      this.writeD(this.player._statistic.headshots_count);
      this.writeD(this.player._statistic.deaths_count);
      this.writeD(this.player._statistic.totalfights_count);
      this.writeD(this.player._statistic.totalkills_count);
      this.writeD(this.player._statistic.escapes);
      this.writeD(this.player._equip._primary);
      this.writeD(this.player._equip._secondary);
      this.writeD(this.player._equip._melee);
      this.writeD(this.player._equip._grenade);
      this.writeD(this.player._equip._special);
      this.writeD(this.player._equip._red);
      this.writeD(this.player._equip._blue);
      this.writeD(this.player._equip._helmet);
      this.writeD(this.player._equip._beret);
      this.writeD(this.player._equip._dino);
      this.writeH((short) 0);
      this.writeC((byte) 0);
    }
  }
}
