
// Type: Game.global.serverpacket.CLAN_GET_CLAN_MEMBERS_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;
using Game.data.model;
using System.Collections.Generic;

namespace Game.global.serverpacket
{
  public class CLAN_GET_CLAN_MEMBERS_PAK : SendPacket
  {
    private List<Account> _players;

    public CLAN_GET_CLAN_MEMBERS_PAK(List<Account> players) => this._players = players;

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
