
// Type: Game.global.serverpacket.BATTLE_HACK_DETECTED_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;

namespace Game.global.serverpacket
{
  public class BATTLE_HACK_DETECTED_PAK : SendPacket
  {
    private int slotId;

    public BATTLE_HACK_DETECTED_PAK(int slot) => this.slotId = slot;

    public override void write()
    {
      this.writeH((short) 3413);
      this.writeC((byte) this.slotId);
      this.writeC((byte) 1);
      this.writeD(1);
    }
  }
}
