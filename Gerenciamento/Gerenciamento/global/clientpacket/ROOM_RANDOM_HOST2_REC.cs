
// Type: Game.global.clientpacket.ROOM_RANDOM_HOST2_REC
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.models.enums;
using Core.server;
using Game.data.model;
using Game.global.serverpacket;
using System;
using System.Collections.Generic;

namespace Game.global.clientpacket
{
  internal class ROOM_RANDOM_HOST2_REC : ReceiveGamePacket
  {
    private List<Core.models.room.Slot> slots = new List<Core.models.room.Slot>();

    public ROOM_RANDOM_HOST2_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read()
    {
    }

    public override void run()
    {
      try
      {
        Account player = this._client._player;
        Room room = player == null ? (Room) null : player._room;
        if (room == null || room._leader != player._slotId || room._state != RoomState.Ready)
          return;
        lock (room._slots)
        {
          for (int index = 0; index < 16; ++index)
          {
            Core.models.room.Slot slot = room._slots[index];
            if (slot._playerId > 0L && index != room._leader)
              this.slots.Add(slot);
          }
        }
        if (this.slots.Count > 0)
        {
          Core.models.room.Slot slot = this.slots[new Random().Next(this.slots.Count)];
          if (room.getPlayerBySlot(slot) != null)
          {
            room.setNewLeader(slot._id, 0, room._leader, false);
            using (ROOM_RANDOM_HOST_PAK roomRandomHostPak = new ROOM_RANDOM_HOST_PAK(slot._id))
              room.SendPacketToPlayers((SendPacket) roomRandomHostPak);
            room.updateSlotsInfo();
          }
          else
            this._client.SendPacket((SendPacket) new ROOM_RANDOM_HOST_PAK(2147483648U));
          this.slots = (List<Core.models.room.Slot>) null;
        }
        else
          this._client.SendPacket((SendPacket) new ROOM_RANDOM_HOST_PAK(2147483648U));
      }
      catch (Exception ex)
      {
        Logger.info("ROOM_RANDOM_HOST2_REC: " + ex.ToString());
      }
    }
  }
}
