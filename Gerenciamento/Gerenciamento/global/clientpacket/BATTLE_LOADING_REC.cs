
// Type: Game.global.clientpacket.BATTLE_LOADING_REC
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.models.enums;
using Core.models.room;
using Game.data.model;
using Game.data.utils;
using System;

namespace Game.global.clientpacket
{
  public class BATTLE_LOADING_REC : ReceiveGamePacket
  {
    private string name;

    public BATTLE_LOADING_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read() => this.name = this.readS((int) this.readC());

    public override void run()
    {
      try
      {
        Account player = this._client._player;
        if (player == null)
          return;
        Room room = player._room;
        Slot slot;
        if (room == null || !room.isPreparing() || (!room.getSlot(player._slotId, out slot) || slot.state != SLOT_STATE.LOAD))
          return;
        slot.preLoadDate = DateTime.Now;
        room.StartCounter(0, player, slot);
        room.changeSlotState(slot, SLOT_STATE.RENDEZVOUS, true);
        room._mapName = this.name;
        if (slot._id != room._leader)
          return;
        AllUtils.LogRoomBattleStart(room);
        room._state = RoomState.Rendezvous;
        room.updateRoomInfo();
      }
      catch (Exception ex)
      {
        Logger.info("BATTLE_LOADING_REC: " + ex.ToString());
      }
    }
  }
}
