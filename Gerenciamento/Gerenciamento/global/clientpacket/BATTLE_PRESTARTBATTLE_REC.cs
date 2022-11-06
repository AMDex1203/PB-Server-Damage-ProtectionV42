
// Type: Game.global.clientpacket.BATTLE_PRESTARTBATTLE_REC
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.models.enums;
using Core.models.enums.errors;
using Core.models.room;
using Core.server;
using Game.data.model;
using Game.data.utils;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
  public class BATTLE_PRESTARTBATTLE_REC : ReceiveGamePacket
  {
    private int mapId;
    private int stage4v4;
    private int room_type;

    public BATTLE_PRESTARTBATTLE_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read()
    {
      this.mapId = (int) this.readH();
      this.stage4v4 = (int) this.readC();
      this.room_type = (int) this.readC();
    }

    private int genValor(int first, int second) => first * 16 + second;

    public override void run()
    {
      try
      {
        if (this._client == null)
          return;
        Account player = this._client._player;
        Room room = player == null ? (Room) null : player._room;
        if (room != null && (int) room.stage4v4 == this.stage4v4 && ((int) room.room_type == this.room_type && room.mapId == this.mapId))
        {
          Slot slot = room._slots[player._slotId];
          if (room.isPreparing() && room.UDPServer != null && slot.state >= SLOT_STATE.LOAD)
          {
            Account leader = room.getLeader();
            if (leader != null)
            {
              if (player.LocalIP == new byte[4] || string.IsNullOrEmpty(player.PublicIP.ToString()))
              {
                this._client.SendPacket((SendPacket) new SERVER_MESSAGE_KICK_BATTLE_PLAYER_PAK(EventErrorEnum.Battle_No_Real_IP));
                this._client.SendPacket((SendPacket) new BATTLE_LEAVEP2PSERVER_PAK(player, 0));
                room.changeSlotState(slot, SLOT_STATE.NORMAL, true);
                AllUtils.BattleEndPlayersCount(room, room.isBotMode());
                slot.StopTiming();
              }
              else
              {
                int gen2 = this.genValor(room.mapId, (int) room.room_type);
                if (slot._id == room._leader)
                {
                  room._state = RoomState.PreBattle;
                  room.updateRoomInfo();
                }
                slot.preStartDate = DateTime.Now;
                room.StartCounter(1, player, slot);
                room.changeSlotState(slot, SLOT_STATE.PRESTART, true);
                this._client.SendPacket((SendPacket) new BATTLE_PRESTARTBATTLE_PAK(player, leader, true, gen2));
                if (slot._id == room._leader)
                  return;
                leader.SendPacket((SendPacket) new BATTLE_PRESTARTBATTLE_PAK(player, leader, false, gen2));
              }
            }
            else
            {
              this._client.SendPacket((SendPacket) new SERVER_MESSAGE_KICK_BATTLE_PLAYER_PAK(EventErrorEnum.Battle_First_Hole));
              this._client.SendPacket((SendPacket) new BATTLE_LEAVEP2PSERVER_PAK(player, 0));
              room.changeSlotState(slot, SLOT_STATE.NORMAL, true);
              AllUtils.BattleEndPlayersCount(room, room.isBotMode());
              slot.StopTiming();
            }
          }
          else
          {
            room.changeSlotState(slot, SLOT_STATE.NORMAL, true);
            this._client.SendPacket((SendPacket) new BATTLE_STARTBATTLE_PAK());
            AllUtils.BattleEndPlayersCount(room, room.isBotMode());
            slot.StopTiming();
          }
        }
        else
        {
          this._client.SendPacket((SendPacket) new SERVER_MESSAGE_KICK_BATTLE_PLAYER_PAK(EventErrorEnum.Battle_First_MainLoad));
          this._client.SendPacket((SendPacket) new BATTLE_PRESTARTBATTLE_PAK());
          if (room != null)
          {
            room.changeSlotState(player._slotId, SLOT_STATE.NORMAL, true);
            AllUtils.BattleEndPlayersCount(room, room.isBotMode());
          }
          else
            this._client.SendPacket((SendPacket) new LOBBY_ENTER_PAK());
        }
      }
      catch (Exception ex)
      {
        Logger.info("BATTLE_PRESTARTBATTLE_REC: " + ex.ToString());
      }
    }
  }
}
