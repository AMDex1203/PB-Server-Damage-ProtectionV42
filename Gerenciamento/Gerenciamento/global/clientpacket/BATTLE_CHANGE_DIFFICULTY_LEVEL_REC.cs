
// Type: Game.global.clientpacket.BATTLE_CHANGE_DIFFICULTY_LEVEL_REC
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

namespace Game.global.clientpacket
{
  public class BATTLE_CHANGE_DIFFICULTY_LEVEL_REC : ReceiveGamePacket
  {
    public BATTLE_CHANGE_DIFFICULTY_LEVEL_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read()
    {
    }

    public override void run()
    {
      try
      {
        Account player = this._client._player;
        Room room = player == null ? (Room) null : player._room;
        if (room == null || room._state != RoomState.Battle || room.IngameAiLevel >= (byte) 10)
          return;
        Slot slot = room.getSlot(player._slotId);
        if (slot == null || slot.state != SLOT_STATE.BATTLE)
          return;
        if (room.IngameAiLevel <= (byte) 9)
          ++room.IngameAiLevel;
        using (BATTLE_CHANGE_DIFFICULTY_LEVEL_PAK difficultyLevelPak = new BATTLE_CHANGE_DIFFICULTY_LEVEL_PAK(room))
          room.SendPacketToPlayers((SendPacket) difficultyLevelPak, SLOT_STATE.READY, 1);
      }
      catch (Exception ex)
      {
        Logger.info("BATTLE_CHANGE_DIFFICULTY_LEVEL_REC: " + ex.ToString());
      }
    }
  }
}
