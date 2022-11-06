
// Type: Game.global.clientpacket.BATTLE_ROOM_INFO_REC
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
  public class BATTLE_ROOM_INFO_REC : ReceiveGamePacket
  {
    public BATTLE_ROOM_INFO_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read()
    {
      try
      {
        Account player = this._client._player;
        Room room = player == null ? (Room) null : player._room;
        if (room == null || room._state != RoomState.Ready || room._leader != player._slotId)
          return;
        this.readD();
        room.name = this.readS(23);
        room.mapId = (int) this.readH();
        room.stage4v4 = this.readC();
        room.room_type = this.readC();
        int num1 = (int) this.readC();
        int num2 = (int) this.readC();
        int num3 = (int) this.readC();
        room._ping = (int) this.readC();
        byte num4 = this.readC();
        if ((int) num4 != (int) room.weaponsFlag)
        {
          room.weaponsFlag = num4;
          for (int index = 0; index < 16; ++index)
          {
            Slot slot = room._slots[index];
            if (slot.state == SLOT_STATE.READY)
              slot.state = SLOT_STATE.NORMAL;
          }
        }
        room.random_map = this.readC();
        room.special = this.readC();
        room.aiCount = this.readC();
        room.aiLevel = this.readC();
        room.updateRoomInfo();
      }
      catch (Exception ex)
      {
        Logger.info("BATTLE_ROOM_INFO_REC: " + ex.ToString());
      }
    }

    public override void run()
    {
    }
  }
}
