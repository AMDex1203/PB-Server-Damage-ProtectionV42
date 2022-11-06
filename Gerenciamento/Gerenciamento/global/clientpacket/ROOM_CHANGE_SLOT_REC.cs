
// Type: Game.global.clientpacket.ROOM_CHANGE_SLOT_REC
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.models.enums;
using Core.models.room;
using Core.server;
using Game.data.model;
using Game.global.serverpacket;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Game.global.clientpacket
{
  public class ROOM_CHANGE_SLOT_REC : ReceiveGamePacket
  {
    private int teamIdx;

    public ROOM_CHANGE_SLOT_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read() => this.teamIdx = this.readD();

    public override void run()
    {
      try
      {
        Account player = this._client._player;
        Room room = player == null ? (Room) null : player._room;
        if (this.teamIdx >= 2 || room == null || !(player.LastSlotChange == new DateTime()) && (DateTime.Now - player.LastSlotChange).TotalSeconds < 1.5 || room.changingSlots)
          return;
        Core.models.room.Slot slot = room.getSlot(player._slotId);
        if (slot == null || this.teamIdx == slot._team || slot.state != SLOT_STATE.NORMAL)
          return;
        player.LastSlotChange = DateTime.Now;
        Monitor.Enter((object) room._slots);
        room.changingSlots = true;
        List<SLOT_CHANGE> slotChangeList = new List<SLOT_CHANGE>();
        room.SwitchNewSlot(slotChangeList, ref player, ref slot, this.teamIdx, false);
        if (slotChangeList.Count > 0)
        {
          using (ROOM_CHANGE_SLOTS_PAK roomChangeSlotsPak = new ROOM_CHANGE_SLOTS_PAK(slotChangeList, room._leader, 0))
            room.SendPacketToPlayers((SendPacket) roomChangeSlotsPak);
        }
        room.changingSlots = false;
        Monitor.Exit((object) room._slots);
      }
      catch (Exception ex)
      {
        Logger.warning("[ROOM_CHANGE_SLOT_REC] " + ex.ToString());
      }
    }
  }
}
