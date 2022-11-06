
// Type: Game.global.clientpacket.BATTLE_RESPAWN_FOR_AI_REC
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
  public class BATTLE_RESPAWN_FOR_AI_REC : ReceiveGamePacket
  {
    private int slotIdx;

    public BATTLE_RESPAWN_FOR_AI_REC(GameClient gc, byte[] data) => this.makeme(gc, data);

    public override void read() => this.slotIdx = this.readD();

    public override void run()
    {
      try
      {
        Account player = this._client._player;
        if (player == null)
          return;
        Room room = player._room;
        if (room == null || room._state != RoomState.Battle || player._slotId != room._leader)
          return;
        room.getSlot(this.slotIdx).aiLevel = (int) room.IngameAiLevel;
        ++room.spawnsCount;
        using (BATTLE_RESPAWN_FOR_AI_PAK battleRespawnForAiPak = new BATTLE_RESPAWN_FOR_AI_PAK(this.slotIdx))
          room.SendPacketToPlayers((SendPacket) battleRespawnForAiPak, SLOT_STATE.BATTLE, 0);
      }
      catch (Exception ex)
      {
        Logger.info("BATTLE_RESPAWN_FOR_AI_REC: " + ex.ToString());
      }
    }
  }
}
