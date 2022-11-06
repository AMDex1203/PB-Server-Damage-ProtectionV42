
// Type: Game.global.serverpacket.VOTEKICK_UPDATE_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.models.room;
using Core.server;

namespace Game.global.serverpacket
{
  public class VOTEKICK_UPDATE_PAK : SendPacket
  {
    private VoteKick _k;

    public VOTEKICK_UPDATE_PAK(VoteKick vote) => this._k = vote;

    public override void write()
    {
      this.writeH((short) 3407);
      this.writeC((byte) this._k.kikar);
      this.writeC((byte) this._k.deixar);
    }
  }
}
