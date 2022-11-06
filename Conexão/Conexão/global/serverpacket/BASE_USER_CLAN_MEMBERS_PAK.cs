
// Type: Auth.global.serverpacket.BASE_USER_CLAN_MEMBERS_PAK
// Assembly: pbserver_auth, Version=1.0.7796.38604, Culture=neutral, PublicKeyToken=null
// MVID: 1CFDFF97-782C-4EF8-A621-6E4FDDE32436
// Interprise: C:\Users\Cuzin\3,50pbserver_auth.exe

using Auth.data.model;
using Core.server;
using System.Collections.Generic;

namespace Auth.global.serverpacket
{
  public class BASE_USER_CLAN_MEMBERS_PAK : SendPacket
  {
    private List<Account> _players;

    public BASE_USER_CLAN_MEMBERS_PAK(List<Account> players) => this._players = players;

    public override void write()
    {
      this.writeH((short) 1349);
      this.writeC((byte) this._players.Count);
      for (int index = 0; index < this._players.Count; ++index)
      {
        Account player = this._players[index];
        this.writeC((byte) (player.player_name.Length + 1));
        this.writeS(player.player_name, player.player_name.Length + 1);
        this.writeQ(player.player_id);
        this.writeQ(ComDiv.GetClanStatus(player._status, player._isOnline));
        this.writeC((byte) player._rank);
      }
    }
  }
}
