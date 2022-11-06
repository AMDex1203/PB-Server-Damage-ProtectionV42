
// Type: Game.global.clientpacket.BATTLE_MISSION_DEFENCE_INFO_REC
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
  public class BATTLE_MISSION_DEFENCE_INFO_REC : ReceiveGamePacket
  {
    private ushort tanqueA;
    private ushort tanqueB;
    private List<ushort> _damag1 = new List<ushort>();
    private List<ushort> _damag2 = new List<ushort>();

    public BATTLE_MISSION_DEFENCE_INFO_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read()
    {
      this.tanqueA = this.readUH();
      this.tanqueB = this.readUH();
      for (int index = 0; index < 16; ++index)
        this._damag1.Add(this.readUH());
      for (int index = 0; index < 16; ++index)
        this._damag2.Add(this.readUH());
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
        room.Bar1 = (int) this.tanqueA;
        room.Bar2 = (int) this.tanqueB;
        for (int index = 0; index < 16; ++index)
        {
          Core.models.room.Slot slot2 = room._slots[index];
          if (slot2._playerId > 0L && slot2.state == SLOT_STATE.BATTLE)
          {
            slot2.damageBar1 = this._damag1[index];
            slot2.damageBar2 = this._damag2[index];
          }
        }
        using (BATTLE_MISSION_DEFENCE_INFO_PAK missionDefenceInfoPak = new BATTLE_MISSION_DEFENCE_INFO_PAK(room))
          room.SendPacketToPlayers((SendPacket) missionDefenceInfoPak, SLOT_STATE.BATTLE, 0);
        if (this.tanqueA != (ushort) 0 || this.tanqueB != (ushort) 0)
          return;
        Net_Room_Sabotage_Sync.EndRound(room, (byte) 0);
      }
      catch (Exception ex)
      {
        Logger.info(ex.ToString());
      }
    }
  }
}
