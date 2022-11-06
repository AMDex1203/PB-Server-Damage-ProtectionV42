
// Type: Game.global.serverpacket.ROOM_CHANGE_SLOTS_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.models.room;
using Core.server;
using System.Collections.Generic;

namespace Game.global.serverpacket
{
  public class ROOM_CHANGE_SLOTS_PAK : SendPacket
  {
    private int _type;
    private int _leader;
    private List<SLOT_CHANGE> _slots;

    public ROOM_CHANGE_SLOTS_PAK(List<SLOT_CHANGE> slots, int leader, int type)
    {
      this._slots = slots;
      this._leader = leader;
      this._type = type;
    }

    public override void write()
    {
      this.writeH((short) 3877);
      this.writeC((byte) this._type);
      this.writeC((byte) this._leader);
      this.writeC((byte) this._slots.Count);
      for (int index = 0; index < this._slots.Count; ++index)
      {
        SLOT_CHANGE slot = this._slots[index];
        this.writeC((byte) slot.oldSlot._id);
        this.writeC((byte) slot.newSlot._id);
        this.writeC((byte) slot.oldSlot.state);
        this.writeC((byte) slot.newSlot.state);
      }
    }
  }
}
