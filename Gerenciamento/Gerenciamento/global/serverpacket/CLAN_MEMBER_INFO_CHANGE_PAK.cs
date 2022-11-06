
// Type: Game.global.serverpacket.CLAN_MEMBER_INFO_CHANGE_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.models.enums.friends;
using Core.server;
using Game.data.model;

namespace Game.global.serverpacket
{
  public class CLAN_MEMBER_INFO_CHANGE_PAK : SendPacket
  {
    private Account p;
    private ulong status;

    public CLAN_MEMBER_INFO_CHANGE_PAK(Account player)
    {
      this.p = player;
      this.status = ComDiv.GetClanStatus(player._status, player._isOnline);
    }

    public CLAN_MEMBER_INFO_CHANGE_PAK(Account player, FriendState st)
    {
      this.p = player;
      if (st == FriendState.None)
        this.status = ComDiv.GetClanStatus(player._status, player._isOnline);
      else
        this.status = ComDiv.GetClanStatus(st);
    }

    public override void write()
    {
      this.writeH((short) 1355);
      this.writeQ(this.p.player_id);
      this.writeQ(this.status);
    }
  }
}
