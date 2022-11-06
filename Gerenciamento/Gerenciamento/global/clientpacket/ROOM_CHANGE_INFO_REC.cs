
// Type: Game.global.clientpacket.ROOM_CHANGE_INFO_REC
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.models.enums;
using Core.models.room;
using Game.data.model;
using System;

namespace Game.global.clientpacket
{
  public class ROOM_CHANGE_INFO_REC : ReceiveGamePacket
  {
    public ROOM_CHANGE_INFO_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read()
    {
      try
      {
        Account player = this._client._player;
        Room room = player == null ? (Room) null : player._room;
        if (room == null || room._leader != player._slotId || room._state != RoomState.Ready)
          return;
        this.readD();
        room.name = this.readS(23);
        room.mapId = (int) this.readH();
        room.stage4v4 = this.readC();
        byte num1 = this.readC();
        if ((int) num1 != (int) room.room_type)
        {
          room.room_type = num1;
          int num2 = 0;
          for (int index = 0; index < 16; ++index)
          {
            Slot slot = room._slots[index];
            if (slot.state == SLOT_STATE.READY)
            {
              slot.state = SLOT_STATE.NORMAL;
              ++num2;
            }
          }
          if (num2 > 0)
            room.updateSlotsInfo();
        }
        int num3 = (int) this.readC();
        int num4 = (int) this.readC();
        int num5 = (int) this.readC();
        room._ping = (int) this.readC();
        room.weaponsFlag = this.readC();
        room.random_map = this.readC();
        room.special = this.readC();
        this.readS(33);
        room.killtime = (int) this.readC();
        int num6 = (int) this.readC();
        int num7 = (int) this.readC();
        int num8 = (int) this.readC();
        room.limit = this.readC();
        room.seeConf = this.readC();
        room.autobalans = (int) this.readH();
        room.aiCount = this.readC();
        room.aiLevel = this.readC();
        room.updateRoomInfo();
      }
      catch (Exception ex)
      {
        Logger.info("ROOM_CHANGE_INFO_REC: " + ex.ToString());
      }
    }

    public override void run()
    {
    }
  }
}
