
// Type: Game.global.serverpacket.BATTLE_ENDBATTLE_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.models.account.clan;
using Core.models.enums;
using Core.server;
using Game.data.managers;
using Game.data.model;
using Game.data.utils;

namespace Game.global.serverpacket
{
  public class BATTLE_ENDBATTLE_PAK : SendPacket
  {
    private Room r;
    private Account p;
    private TeamResultType winner = TeamResultType.TeamDraw;
    private ushort playersFlag;
    private ushort missionsFlag;
    private bool isBotMode;
    private byte[] array1;

    public BATTLE_ENDBATTLE_PAK(Account p)
    {
      this.p = p;
      if (p == null)
        return;
      this.r = p._room;
      this.winner = AllUtils.GetWinnerTeam(this.r);
      this.isBotMode = this.r.isBotMode();
      AllUtils.getBattleResult(this.r, out this.missionsFlag, out this.playersFlag, out this.array1);
    }

    public BATTLE_ENDBATTLE_PAK(
      Account p,
      TeamResultType winner,
      ushort playersFlag,
      ushort missionsFlag,
      bool isBotMode,
      byte[] a1)
    {
      this.p = p;
      this.winner = winner;
      this.playersFlag = playersFlag;
      this.missionsFlag = missionsFlag;
      this.isBotMode = isBotMode;
      this.array1 = a1;
      if (p == null)
        return;
      this.r = p._room;
    }

    public override void write()
    {
      if (this.p == null || this.r == null)
        return;
      this.writeH((short) 3336);
      this.writeC((byte) this.winner);
      this.writeH(this.playersFlag);
      this.writeH(this.missionsFlag);
      this.writeB(this.array1);
      Clan clan = ClanManager.getClan(this.p.clanId);
      this.writeS(this.p.player_name, 33);
      this.writeD(this.p._exp);
      this.writeD(this.p._rank);
      this.writeD(this.p._rank);
      this.writeD(this.p._gp);
      this.writeD(this.p._money);
      this.writeD(clan.id);
      this.writeD(this.p.clanAccess);
      this.writeD(0);
      this.writeD(0);
      this.writeC((byte) this.p.pc_cafe);
      this.writeC((byte) this.p.tourneyLevel);
      this.writeC((byte) this.p.name_color);
      this.writeS(clan.name, 17);
      this.writeC(clan.rank);
      this.writeC((byte) clan.getClanUnit());
      this.writeD(clan.logo);
      this.writeC(clan.nameColor);
      this.writeD(0);
      this.writeC((byte) 0);
      this.writeD(0);
      this.writeD(this.p.LastRankUpDate);
      this.writeD(this.p._statistic.fights);
      this.writeD(this.p._statistic.fights_win);
      this.writeD(this.p._statistic.fights_lost);
      this.writeD(this.p._statistic.fights_draw);
      this.writeD(this.p._statistic.kills_count);
      this.writeD(this.p._statistic.headshots_count);
      this.writeD(this.p._statistic.deaths_count);
      this.writeD(this.p._statistic.totalfights_count);
      this.writeD(this.p._statistic.totalkills_count);
      this.writeD(this.p._statistic.escapes);
      this.writeD(this.p._statistic.fights);
      this.writeD(this.p._statistic.fights_win);
      this.writeD(this.p._statistic.fights_lost);
      this.writeD(this.p._statistic.fights_draw);
      this.writeD(this.p._statistic.kills_count);
      this.writeD(this.p._statistic.headshots_count);
      this.writeD(this.p._statistic.deaths_count);
      this.writeD(this.p._statistic.totalfights_count);
      this.writeD(this.p._statistic.totalkills_count);
      this.writeD(this.p._statistic.escapes);
      if (this.isBotMode)
      {
        for (int index = 0; index < 16; ++index)
          this.writeH((ushort) this.r._slots[index].Score);
      }
      else if (this.r.room_type == (byte) 2 || this.r.room_type == (byte) 4 || (this.r.room_type == (byte) 7 || this.r.room_type == (byte) 12))
      {
        this.writeH(this.r.room_type == (byte) 7 ? (ushort) this.r.red_dino : (this.r.room_type == (byte) 12 ? (ushort) this.r._redKills : (ushort) this.r.red_rounds));
        this.writeH(this.r.room_type == (byte) 7 ? (ushort) this.r.blue_dino : (this.r.room_type == (byte) 12 ? (ushort) this.r._blueKills : (ushort) this.r.blue_rounds));
        for (int index = 0; index < 16; ++index)
          this.writeC((byte) this.r._slots[index].objetivos);
      }
      this.writeC((byte) 0);
      this.writeD(0);
      this.writeB(new byte[16]);
    }
  }
}
