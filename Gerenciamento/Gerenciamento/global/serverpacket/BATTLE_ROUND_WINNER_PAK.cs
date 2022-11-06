
// Type: Game.global.serverpacket.BATTLE_ROUND_WINNER_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.models.enums;
using Core.server;
using Game.data.model;

namespace Game.global.serverpacket
{
  public class BATTLE_ROUND_WINNER_PAK : SendPacket
  {
    private Room _room;
    private int _winner;
    private RoundEndType _reason;

    public BATTLE_ROUND_WINNER_PAK(Room room, int winner, RoundEndType reason)
    {
      this._room = room;
      this._winner = winner;
      this._reason = reason;
    }

    public BATTLE_ROUND_WINNER_PAK(Room room, TeamResultType winner, RoundEndType reason)
    {
      this._room = room;
      this._winner = (int) winner;
      this._reason = reason;
    }

    public override void write()
    {
      this.writeH((short) 3353);
      this.writeC((byte) this._winner);
      this.writeC((byte) this._reason);
      if (this._room.room_type == (byte) 7)
      {
        this.writeH((ushort) this._room.red_dino);
        this.writeH((ushort) this._room.blue_dino);
      }
      else if (this._room.room_type == (byte) 1 || this._room.room_type == (byte) 8 || this._room.room_type == (byte) 13)
      {
        this.writeH((ushort) this._room._redKills);
        this.writeH((ushort) this._room._blueKills);
      }
      else
      {
        this.writeH((ushort) this._room.red_rounds);
        this.writeH((ushort) this._room.blue_rounds);
      }
    }
  }
}
