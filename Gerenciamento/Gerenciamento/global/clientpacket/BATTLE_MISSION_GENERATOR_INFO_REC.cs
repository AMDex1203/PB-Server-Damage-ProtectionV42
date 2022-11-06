
// Type: Game.global.clientpacket.BATTLE_MISSION_GENERATOR_INFO_REC
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.models.enums;
using Core.server;
using Game.data.model;
using Game.data.sync.client_side;
using Game.global.serverpacket;
using System;
using System.Collections.Generic;

namespace Game.global.clientpacket
{
  public class BATTLE_MISSION_GENERATOR_INFO_REC : ReceiveGamePacket
  {
    private ushort barRed;
    private ushort barBlue;
    private List<ushort> damages = new List<ushort>();

    public BATTLE_MISSION_GENERATOR_INFO_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read()
    {
      this.barRed = this.readUH();
      this.barBlue = this.readUH();
      for (int index = 0; index < 16; ++index)
        this.damages.Add(this.readUH());
    }

    public override void run()
    {
      try
      {
        Account player = this._client._player;
        Room room = player == null ? (Room) null : player._room;
        if (room == null || room.round.Timer != null || (room._state != RoomState.Battle || room.swapRound))
          return;
        Core.models.room.Slot slot1 = room.getSlot(player._slotId);
        if (slot1 == null || slot1.state != SLOT_STATE.BATTLE)
          return;
        room.Bar1 = (int) this.barRed;
        room.Bar2 = (int) this.barBlue;
        for (int index = 0; index < 16; ++index)
        {
          Core.models.room.Slot slot2 = room._slots[index];
          if (slot2._playerId > 0L && slot2.state == SLOT_STATE.BATTLE)
          {
            slot2.damageBar1 = this.damages[index];
            slot2.earnedXP = (int) this.damages[index] / 600;
          }
        }
        using (BATTLE_MISSION_GENERATOR_INFO_PAK generatorInfoPak = new BATTLE_MISSION_GENERATOR_INFO_PAK(room))
          room.SendPacketToPlayers((SendPacket) generatorInfoPak, SLOT_STATE.BATTLE, 0);
        if (this.barRed == (ushort) 0)
        {
          Net_Room_Sabotage_Sync.EndRound(room, (byte) 1);
        }
        else
        {
          if (this.barBlue != (ushort) 0)
            return;
          Net_Room_Sabotage_Sync.EndRound(room, (byte) 0);
        }
      }
      catch (Exception ex)
      {
        Logger.info(ex.ToString());
      }
    }
  }
}
