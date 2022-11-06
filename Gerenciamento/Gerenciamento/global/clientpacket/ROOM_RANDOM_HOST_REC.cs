
// Type: Game.global.clientpacket.ROOM_RANDOM_HOST_REC
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
  internal class ROOM_RANDOM_HOST_REC : ReceiveGamePacket
  {
    private List<Core.models.room.Slot> slots = new List<Core.models.room.Slot>();
    private uint erro;

    public ROOM_RANDOM_HOST_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read()
    {
    }

    public override void run()
    {
      try
      {
        Account player = this._client._player;
        Room room = player == null ? (Room) null : player._room;
        if (room != null && room._leader == player._slotId && room._state == RoomState.Ready)
        {
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
            this.erro = room.getPlayerBySlot(slot) != null ? (uint) slot._id : 2147483648U;
            this.slots = (List<Core.models.room.Slot>) null;
          }
          else
            this.erro = 2147483648U;
        }
        else
          this.erro = 2147483648U;
        this._client.SendPacket((SendPacket) new ROOM_NEW_HOST_PAK(this.erro));
      }
      catch (Exception ex)
      {
        Logger.info("ROOM_RANDOM_HOST_REC: " + ex.ToString());
      }
    }
  }
}
