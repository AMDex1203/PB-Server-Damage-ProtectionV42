
// Type: Game.global.serverpacket.CLAN_MEMBER_INFO_UPDATE_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;
using Game.data.model;

namespace Game.global.serverpacket
{
  public class CLAN_MEMBER_INFO_UPDATE_PAK : SendPacket
  {
    private Account p;
    private ulong status;

    public CLAN_MEMBER_INFO_UPDATE_PAK(Account pl)
    {
      this.p = pl;
      this.status = ComDiv.GetClanStatus(pl._status, pl._isOnline);
    }

    public override void write()
    {
      this.writeH((short) 1380);
      this.writeQ(this.p.player_id);
      this.writeS(this.p.player_name, 33);
      this.writeC((byte) this.p._rank);
      this.writeC((byte) this.p.clanAccess);
      this.writeQ(this.status);
      this.writeD(this.p.clanDate);
      this.writeC((byte) this.p.name_color);
    }
  }
}
