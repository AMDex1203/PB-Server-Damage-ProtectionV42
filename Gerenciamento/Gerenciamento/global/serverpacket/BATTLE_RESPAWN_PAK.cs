
// Type: Game.global.serverpacket.BATTLE_RESPAWN_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;
using Game.data.model;
using Game.data.utils;
using System.Collections.Generic;

namespace Game.global.serverpacket
{
  public class BATTLE_RESPAWN_PAK : SendPacket
  {
    private Core.models.room.Slot slot;
    private Room room;

    public BATTLE_RESPAWN_PAK(Room r, Core.models.room.Slot slot)
    {
      this.slot = slot;
      this.room = r;
    }

    public override void write()
    {
      this.writeH((short) 3338);
      this.writeD(this.slot._id);
      this.writeD(this.room.spawnsCount++);
      this.writeD(++this.slot.spawnsCount);
      this.writeD(this.slot._equip._primary);
      this.writeD(this.slot._equip._secondary);
      this.writeD(this.slot._equip._melee);
      this.writeD(this.slot._equip._grenade);
      this.writeD(this.slot._equip._special);
      this.writeD(0);
      this.writeB(new byte[6]
      {
        (byte) 100,
        (byte) 100,
        (byte) 100,
        (byte) 100,
        (byte) 100,
        (byte) 1
      });
      this.writeD(this.slot._equip._red);
      this.writeD(this.slot._equip._blue);
      this.writeD(this.slot._equip._helmet);
      this.writeD(this.slot._equip._beret);
      this.writeD(this.slot._equip._dino);
      if (this.room.room_type != (byte) 7 && this.room.room_type != (byte) 12)
        return;
      List<int> dinossaurs = AllUtils.getDinossaurs(this.room, false, this.slot._id);
      int num1 = dinossaurs.Count == 1 || this.room.room_type == (byte) 12 ? (int) byte.MaxValue : this.room.TRex;
      this.writeC((byte) num1);
      foreach (int num2 in dinossaurs)
      {
        if (num2 != this.room.TRex && this.room.room_type == (byte) 7 || this.room.room_type == (byte) 12)
          this.writeC((byte) num2);
      }
      int num3 = 8 - dinossaurs.Count - (num1 == (int) byte.MaxValue ? 1 : 0);
      for (int index = 0; index < num3; ++index)
        this.writeC(byte.MaxValue);
      this.writeC(byte.MaxValue);
      this.writeC(byte.MaxValue);
    }
  }
}
