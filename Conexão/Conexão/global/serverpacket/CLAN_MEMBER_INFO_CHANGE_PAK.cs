
// Type: Auth.global.serverpacket.CLAN_MEMBER_INFO_CHANGE_PAK
// Assembly: pbserver_auth, Version=1.0.7796.38604, Culture=neutral, PublicKeyToken=null
// MVID: 1CFDFF97-782C-4EF8-A621-6E4FDDE32436
// Interprise: C:\Users\Cuzin\3,50pbserver_auth.exe

using Auth.data.model;
using Core.models.enums.friends;
using Core.server;

namespace Auth.global.serverpacket
{
  public class CLAN_MEMBER_INFO_CHANGE_PAK : SendPacket
  {
    private ulong status;
    private Account member;

    public CLAN_MEMBER_INFO_CHANGE_PAK(Account player)
    {
      this.member = player;
      this.status = ComDiv.GetClanStatus(player._status, player._isOnline);
    }

    public CLAN_MEMBER_INFO_CHANGE_PAK(Account player, FriendState st)
    {
      this.member = player;
      if (st == FriendState.None)
        this.status = ComDiv.GetClanStatus(player._status, player._isOnline);
      else
        this.status = ComDiv.GetClanStatus(st);
    }

    public override void write()
    {
      this.writeH((short) 1355);
      this.writeQ(this.member.player_id);
      this.writeQ(this.status);
    }
  }
}
