
// Type: Game.global.serverpacket.FRIEND_UPDATE_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.models.account;
using Core.models.account.players;
using Core.models.enums.friends;
using Core.server;

namespace Game.global.serverpacket
{
  public class FRIEND_UPDATE_PAK : SendPacket
  {
    private Friend _f;
    private int _index;
    private FriendState _state;
    private FriendChangeState _type;

    public FRIEND_UPDATE_PAK(FriendChangeState type, Friend friend, int idx)
    {
      this._type = type;
      this._state = (FriendState) friend.state;
      this._f = friend;
      this._index = idx;
    }

    public FRIEND_UPDATE_PAK(FriendChangeState type, Friend friend, int state, int idx)
    {
      this._type = type;
      this._state = (FriendState) state;
      this._f = friend;
      this._index = idx;
    }

    public FRIEND_UPDATE_PAK(FriendChangeState type, Friend friend, FriendState state, int idx)
    {
      this._type = type;
      this._state = state;
      this._f = friend;
      this._index = idx;
    }

    public override void write()
    {
      this.writeH((short) 279);
      this.writeC((byte) this._type);
      this.writeC((byte) this._index);
      if (this._type == FriendChangeState.Insert || this._type == FriendChangeState.Update)
      {
        PlayerInfo player = this._f.player;
        if (player == null)
        {
          this.writeB(new byte[17]);
        }
        else
        {
          this.writeC((byte) (player.player_name.Length + 1));
          this.writeS(player.player_name, player.player_name.Length + 1);
          this.writeQ(this._f.player_id);
          this.writeD(ComDiv.GetFriendStatus(this._f, this._state));
          this.writeC((byte) player._rank);
          this.writeC((byte) 0);
          this.writeH((short) 0);
        }
      }
      else
        this.writeB(new byte[17]);
    }
  }
}
