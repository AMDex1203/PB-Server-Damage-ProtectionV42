
// Type: Game.global.serverpacket.BATTLE_STARTBATTLE_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.models.enums.missions;
using Core.server;
using Game.data.model;
using Game.data.utils;
using System.Collections.Generic;

namespace Game.global.serverpacket
{
  public class BATTLE_STARTBATTLE_PAK : SendPacket
  {
    private Room room;
    private Core.models.room.Slot slot;
    private int isBattle;
    private int type;
    private List<int> dinos;

    public BATTLE_STARTBATTLE_PAK(
      Core.models.room.Slot slot,
      Account pR,
      List<int> dinos,
      bool isBotMode,
      bool type)
    {
      this.slot = slot;
      this.room = pR._room;
      this.type = type ? 0 : 1;
      this.dinos = dinos;
      if (this.room == null)
        return;
      this.isBattle = 1;
      if (isBotMode)
        return;
      AllUtils.CompleteMission(this.room, pR, slot, type ? MISSION_TYPE.STAGE_ENTER : MISSION_TYPE.STAGE_INTERCEPT, 0);
    }

    public BATTLE_STARTBATTLE_PAK()
    {
    }

    public override void write()
    {
      this.writeH((short) 3334);
      this.writeD(this.isBattle);
      if (this.isBattle != 1)
        return;
      this.writeD(this.slot._id);
      this.writeC((byte) this.type);
      this.writeH(AllUtils.getSlotsFlag(this.room, false, false));
      if (this.room.room_type == (byte) 2 || this.room.room_type == (byte) 3 || (this.room.room_type == (byte) 4 || this.room.room_type == (byte) 5))
      {
        this.writeH((ushort) this.room.red_rounds);
        this.writeH((ushort) this.room.blue_rounds);
        if (this.room.room_type != (byte) 3 && this.room.room_type != (byte) 5)
        {
          this.writeH(AllUtils.getSlotsFlag(this.room, true, false));
        }
        else
        {
          this.writeH((ushort) this.room.Bar1);
          this.writeH((ushort) this.room.Bar2);
          for (int index = 0; index < 16; ++index)
            this.writeH(this.room._slots[index].damageBar1);
          if (this.room.room_type != (byte) 5)
            return;
          for (int index = 0; index < 16; ++index)
            this.writeH(this.room._slots[index].damageBar2);
        }
      }
      else
      {
        if (this.room.room_type != (byte) 7 && this.room.room_type != (byte) 12)
          return;
        this.writeH(this.room.room_type == (byte) 12 ? (ushort) this.room._redKills : (ushort) this.room.red_dino);
        this.writeH(this.room.room_type == (byte) 12 ? (ushort) this.room._blueKills : (ushort) this.room.blue_dino);
        this.writeC((byte) this.room.rodada);
        this.writeH(AllUtils.getSlotsFlag(this.room, false, false));
        int num1 = this.dinos.Count == 1 || this.room.room_type == (byte) 12 ? (int) byte.MaxValue : this.room.TRex;
        this.writeC((byte) num1);
        foreach (int dino in this.dinos)
        {
          if (dino != this.room.TRex && this.room.room_type == (byte) 7 || this.room.room_type == (byte) 12)
            this.writeC((byte) dino);
        }
        int num2 = 8 - this.dinos.Count - (num1 == (int) byte.MaxValue ? 1 : 0);
        for (int index = 0; index < num2; ++index)
          this.writeC(byte.MaxValue);
        this.writeC(byte.MaxValue);
        this.writeC(byte.MaxValue);
        this.writeC((byte) 37);
      }
    }
  }
}
