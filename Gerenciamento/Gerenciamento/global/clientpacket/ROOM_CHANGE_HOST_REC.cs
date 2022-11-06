
// Type: Game.global.clientpacket.ROOM_CHANGE_HOST_REC
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.models.enums;
using Core.server;
using Game.data.model;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
  public class ROOM_CHANGE_HOST_REC : ReceiveGamePacket
  {
    private int slotId;

    public ROOM_CHANGE_HOST_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read() => this.slotId = this.readD();

    public override void run()
    {
      Account player = this._client._player;
      Room room = player == null ? (Room) null : player._room;
      try
      {
        if (room == null || room._leader == this.slotId || room._slots[this.slotId]._playerId == 0L)
        {
          this._client.SendPacket((SendPacket) new ROOM_CHANGE_HOST_PAK(2147483648U));
        }
        else
        {
          if (room._state != RoomState.Ready || room._leader != player._slotId)
            return;
          room.setNewLeader(this.slotId, 0, room._leader, false);
          using (ROOM_CHANGE_HOST_PAK roomChangeHostPak = new ROOM_CHANGE_HOST_PAK(this.slotId))
            room.SendPacketToPlayers((SendPacket) roomChangeHostPak);
          room.updateSlotsInfo();
        }
      }
      catch (Exception ex)
      {
        Logger.info("ROOM_CHANGE_HOST_REC: " + ex.ToString());
      }
    }
  }
}
