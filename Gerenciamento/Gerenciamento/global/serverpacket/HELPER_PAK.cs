
// Type: Game.global.serverpacket.HELPER_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;

namespace Game.global.serverpacket
{
  public class HELPER_PAK : SendPacket
  {
    private ushort _packet;

    public HELPER_PAK(ushort packet) => this._packet = packet;

    public override void write()
    {
      this.writeH(this._packet);
      this.writeD(0);
      this.writeC((byte) 1);
    }
  }
}
