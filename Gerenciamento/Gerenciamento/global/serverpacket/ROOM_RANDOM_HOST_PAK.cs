
// Type: Game.global.serverpacket.ROOM_RANDOM_HOST_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;

namespace Game.global.serverpacket
{
  public class ROOM_RANDOM_HOST_PAK : SendPacket
  {
    private uint _slot;

    public ROOM_RANDOM_HOST_PAK(uint slot) => this._slot = slot;

    public ROOM_RANDOM_HOST_PAK(int slot) => this._slot = (uint) slot;

    public override void write()
    {
      this.writeH((short) 3869);
      this.writeD(this._slot);
    }
  }
}
