
// Type: Game.global.serverpacket.ROOM_GET_SLOTONEINFO_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.models.account.clan;
using Core.server;
using Game.data.managers;
using Game.data.model;

namespace Game.global.serverpacket
{
  public class ROOM_GET_SLOTONEINFO_PAK : SendPacket
  {
    private Account p;
    private Clan clan;

    public ROOM_GET_SLOTONEINFO_PAK(Account player)
    {
      this.p = player;
      if (this.p == null)
        return;
      this.clan = ClanManager.getClan(this.p.clanId);
    }

    public ROOM_GET_SLOTONEINFO_PAK(Account player, Clan c)
    {
      this.p = player;
      this.clan = c;
    }

    public override void write()
    {
      if (this.p._room == null || this.p._slotId == -1)
        return;
      this.writeH((short) 3909);
      this.writeD(this.p._slotId);
      this.writeC((byte) this.p._room._slots[this.p._slotId].state);
      this.writeC((byte) this.p.getRank());
      this.writeD(this.clan.id);
      this.writeD(this.p.clanAccess);
      this.writeC(this.clan.rank);
      this.writeD(this.clan.logo);
      this.writeC((byte) this.p.pc_cafe);
      this.writeC((byte) this.p.tourneyLevel);
      this.writeD((uint) this.p.effects);
      this.writeS(this.clan.name, 17);
      this.writeD(0);
      this.writeC((byte) 31);
      this.writeS(this.p.player_name, 33);
      this.writeC((byte) this.p.name_color);
    }
  }
}
