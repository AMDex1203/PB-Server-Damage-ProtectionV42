
// Type: Game.global.serverpacket.ROOM_GET_NICKNAME_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;

namespace Game.global.serverpacket
{
  public class ROOM_GET_NICKNAME_PAK : SendPacket
  {
    private int slotIdx;
    private int color;
    private string name;

    public ROOM_GET_NICKNAME_PAK(int slot, string name, int color)
    {
      this.slotIdx = slot;
      this.name = name;
      this.color = color;
    }

    public override void write()
    {
      this.writeH((short) 3844);
      this.writeD(this.slotIdx);
      if (this.slotIdx < 0)
        return;
      this.writeS(this.name, 33);
      this.writeC((byte) this.color);
    }
  }
}
