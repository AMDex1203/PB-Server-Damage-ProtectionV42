
// Type: Game.global.serverpacket.BASE_2612_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.models.account.clan;
using Core.server;
using Game.data.managers;
using Game.data.model;

namespace Game.global.serverpacket
{
  public class BASE_2612_PAK : SendPacket
  {
    private Account p;
    private Clan clan;

    public BASE_2612_PAK(Account player)
    {
      this.p = player;
      this.clan = ClanManager.getClan(this.p.clanId);
    }

    public override void write()
    {
      this.writeH((short) 2612);
      this.writeS(this.p.player_name, 33);
      this.writeD(this.p._exp);
      this.writeD(this.p._rank);
      this.writeD(this.p._rank);
      this.writeD(this.p._gp);
      this.writeD(this.p._money);
      this.writeD(this.clan.id);
      this.writeD(this.p.clanAccess);
      this.writeQ(0L);
      this.writeC((byte) this.p.pc_cafe);
      this.writeC((byte) this.p.tourneyLevel);
      this.writeC((byte) this.p.name_color);
      this.writeS(this.clan.name, 17);
      this.writeC(this.clan.rank);
      this.writeC((byte) this.clan.getClanUnit());
      this.writeD(this.clan.logo);
      this.writeC(this.clan.nameColor);
      this.writeD(0);
      this.writeC((byte) 0);
      this.writeD(0);
      this.writeD(this.p.LastRankUpDate);
    }
  }
}
