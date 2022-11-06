
// Type: Game.global.serverpacket.BATTLE_MISSION_ESCAPE_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.models.room;
using Core.server;
using Game.data.model;

namespace Game.global.serverpacket
{
  public class BATTLE_MISSION_ESCAPE_PAK : SendPacket
  {
    private Room r;
    private Slot slot;

    public BATTLE_MISSION_ESCAPE_PAK(Room room, Slot slot)
    {
      this.r = room;
      this.slot = slot;
    }

    public override void write()
    {
      this.writeH((short) 3383);
      this.writeH((ushort) this.r.red_dino);
      this.writeH((ushort) this.r.blue_dino);
      this.writeD(this.slot._id);
      this.writeH((short) this.slot.passSequence);
    }
  }
}
