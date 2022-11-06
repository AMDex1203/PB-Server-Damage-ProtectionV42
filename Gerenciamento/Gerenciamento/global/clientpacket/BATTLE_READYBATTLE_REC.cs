
// Type: Game.global.clientpacket.BATTLE_READYBATTLE_REC
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.models.account.players;
using Core.models.enums;
using Core.models.room;
using Core.server;
using Game.data.managers;
using Game.data.model;
using Game.data.utils;
using Game.global.serverpacket;
using System;
using System.Collections.Generic;

namespace Game.global.clientpacket
{
  public class BATTLE_READYBATTLE_REC : ReceiveGamePacket
  {
    private int erro;

    public BATTLE_READYBATTLE_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read() => this.erro = this.readD();

    private bool ClassicModeCheck(Account p, Room room)
    {
      if (!room.name.ToLower().Contains("@camp") && !room.name.ToLower().Contains("camp") && (!room.name.ToLower().Contains("@cnpb") && !room.name.ToLower().Contains("cnpb")) && (!room.name.ToLower().Contains("@cupons") && !room.name.ToLower().Contains("cupons")))
        return false;
      List<string> list = new List<string>();
      PlayerEquipedItems equip = p._equip;
      if (room.name.ToLower().Contains("@camp") || room.name.ToLower().Contains(" @camp") || (room.name.ToLower().Contains("@camp ") || room.name.ToLower().Contains("camp")))
      {
        for (int index = 0; index < ClassicModeManager.itemscamp.Count; ++index)
        {
          int listid = ClassicModeManager.itemscamp[index];
          if (!ClassicModeManager.IsBlocked(listid, equip._primary, ref list, Translation.GetLabel("ClassicCategory1")) && !ClassicModeManager.IsBlocked(listid, equip._secondary, ref list, Translation.GetLabel("ClassicCategory2")) && (!ClassicModeManager.IsBlocked(listid, equip._melee, ref list, Translation.GetLabel("ClassicCategory3")) && !ClassicModeManager.IsBlocked(listid, equip._grenade, ref list, Translation.GetLabel("ClassicCategory4"))) && (!ClassicModeManager.IsBlocked(listid, equip._special, ref list, Translation.GetLabel("ClassicCategory5")) && !ClassicModeManager.IsBlocked(listid, equip._red, ref list, Translation.GetLabel("ClassicCategory6")) && (!ClassicModeManager.IsBlocked(listid, equip._blue, ref list, Translation.GetLabel("ClassicCategory7")) && !ClassicModeManager.IsBlocked(listid, equip._helmet, ref list, Translation.GetLabel("ClassicCategory8")))) && !ClassicModeManager.IsBlocked(listid, equip._dino, ref list, Translation.GetLabel("ClassicCategory9")))
            ClassicModeManager.IsBlocked(listid, equip._beret, ref list, Translation.GetLabel("ClassicCategory10"));
        }
      }
      if (list.Count <= 0)
        return false;
      p.SendPacket((SendPacket) new SERVER_MESSAGE_ANNOUNCE_PAK(Translation.GetLabel("ClassicModeWarn", (object) string.Join(", ", list.ToArray()))));
      return true;
    }

    public override void run()
    {
      try
      {
        Account player = this._client._player;
        if (player == null)
          return;
        Room room = player._room;
        Channel ch;
        Core.models.room.Slot slot;
        if (room == null || room.getLeader() == null || (!room.getChannel(out ch) || !room.getSlot(player._slotId, out slot)))
          return;
        if (slot._equip == null)
        {
          this._client.SendPacket((SendPacket) new BATTLE_READY_ERROR_PAK(2147487915U));
        }
        else
        {
          bool isBotMode = room.isBotMode();
          slot.specGM = this.erro == 1 && player.IsGM();
          player.DebugPing = false;
          if (ConfigGS.EnableClassicRules && this.ClassicModeCheck(player, room))
            return;
          int TotalEnemys = 0;
          int redPlayers = 0;
          int bluePlayers = 0;
          this.GetReadyPlayers(room, ref redPlayers, ref bluePlayers, ref TotalEnemys);
          if (room._leader == player._slotId)
          {
            if (room._state != RoomState.Ready && room._state != RoomState.CountDown)
              return;
            if (ConfigGS.isTestMode && ConfigGS.udpType == SERVER_UDP_STATE.RELAY)
              TotalEnemys = 1;
            if (room.stage4v4 == (byte) 1 | isBotMode)
              this.Check4vs4(room, -1, true, isBotMode, ref redPlayers, ref bluePlayers, ref TotalEnemys);
            if (this.ClanMatchCheck(room, ch._type, TotalEnemys, redPlayers, bluePlayers, room._leader % 2))
              return;
            if (isBotMode || room.room_type == (byte) 10 || TotalEnemys > 0 && !isBotMode)
            {
              room.changeSlotState(slot, SLOT_STATE.READY, false);
              if (room.autobalans != 0)
                this.TryBalanceTeams(room, (Account) null);
              if (room.thisModeHaveCD())
              {
                if (room._state == RoomState.Ready)
                {
                  room._state = RoomState.CountDown;
                  room.updateRoomInfo();
                  if (room.countdownSequence == -1)
                    room.countdownSequence = 0;
                  ++room.countdownSequence;
                  room.StartCountDown(room.countdownSequence);
                }
                else if (room._state == RoomState.CountDown)
                {
                  room.changeSlotState(room._leader, SLOT_STATE.NORMAL, false);
                  room.StopCountDown(CountDownEnum.StopByHost);
                }
              }
              else
                room.StartBattle(false);
              room.updateSlotsInfo();
            }
            else
            {
              if (TotalEnemys != 0 || isBotMode)
                return;
              this._client.SendPacket((SendPacket) new BATTLE_READY_ERROR_PAK(2147487753U));
            }
          }
          else if (room._slots[room._leader].state >= SLOT_STATE.LOAD)
          {
            if (slot.state != SLOT_STATE.NORMAL)
              return;
            if (this.Check4vs4(room, slot._team, false, isBotMode, ref redPlayers, ref bluePlayers, ref TotalEnemys))
            {
              this._client.SendPacket((SendPacket) new BATTLE_4VS4_ERROR_PAK());
            }
            else
            {
              room.changeSlotState(slot, SLOT_STATE.READY, false);
              if (!isBotMode && room.autobalans != 0)
              {
                this.TryBalanceTeams(room, player);
                slot = room.getSlot(player._slotId);
              }
              room.changeSlotState(slot, SLOT_STATE.LOAD, true);
              slot.SetMissionsClone(player._mission);
              this._client.SendPacket((SendPacket) new BATTLE_READYBATTLE_PAK(room));
              this._client.SendPacket((SendPacket) new BATTLE_READY_ERROR_PAK((uint) slot.state));
              using (BATTLE_READYBATTLE2_PAK battleReadybattlE2Pak = new BATTLE_READYBATTLE2_PAK(slot, player._titles))
                room.SendPacketToPlayers((SendPacket) battleReadybattlE2Pak, SLOT_STATE.READY, 1, slot._id);
            }
          }
          else if (slot.state == SLOT_STATE.NORMAL)
          {
            room.changeSlotState(slot, SLOT_STATE.READY, true);
          }
          else
          {
            if (slot.state != SLOT_STATE.READY)
              return;
            room.changeSlotState(slot, SLOT_STATE.NORMAL, false);
            if (room._state == RoomState.CountDown && room.getPlayingPlayers(room._leader % 2 == 0 ? 1 : 0, SLOT_STATE.READY, 0) == 0)
            {
              room.changeSlotState(room._leader, SLOT_STATE.NORMAL, false);
              room.StopCountDown(CountDownEnum.StopByPlayer);
            }
            room.updateSlotsInfo();
          }
        }
      }
      catch (Exception ex)
      {
        Logger.info("BATTLE_READYBATTLE_REC: " + ex.ToString());
      }
    }

    private void GetReadyPlayers(
      Room room,
      ref int redPlayers,
      ref int bluePlayers,
      ref int TotalEnemys)
    {
      for (int index = 0; index < 16; ++index)
      {
        Core.models.room.Slot slot = room._slots[index];
        if (slot.state == SLOT_STATE.READY)
        {
          if (slot._team == 0)
            ++redPlayers;
          else
            ++bluePlayers;
        }
      }
      if (room._leader % 2 == 0)
        TotalEnemys = bluePlayers;
      else
        TotalEnemys = redPlayers;
    }

    private bool ClanMatchCheck(
      Room room,
      int type,
      int TotalEnemys,
      int redPlayers,
      int bluePlayers,
      int leader)
    {
      if (type != 4)
        return false;
      if (!AllUtils.Have2ClansToClanMatch(room))
      {
        this._client.SendPacket((SendPacket) new BATTLE_READY_ERROR_PAK(2147487857U));
        return true;
      }
      if (redPlayers < (leader == 0 ? 3 : 4) || bluePlayers < (leader == 1 ? 3 : 4))
      {
        this._client.SendPacket((SendPacket) new BATTLE_READY_ERROR_PAK(2147487858U));
        return true;
      }
      if (TotalEnemys <= 0 || AllUtils.HavePlayersToClanMatch(room))
        return false;
      this._client.SendPacket((SendPacket) new BATTLE_READY_ERROR_PAK(2147487858U));
      return true;
    }

    private void TryBalanceTeams(Room room, Account accountPlayer)
    {
      try
      {
        lock (room._slots)
        {
          if (room._state == RoomState.CountDown)
            return;
          List<SLOT_CHANGE> slotChangeList = new List<SLOT_CHANGE>();
          bool inBattle = room._state >= RoomState.Loading;
          int balanceTeamIdx = room.getBalanceTeamIdx(inBattle);
          if (inBattle)
          {
            if (accountPlayer == null)
              return;
            Core.models.room.Slot slot1 = room.getSlot(accountPlayer._slotId);
            if (slot1 == null)
              return;
            if (room.SwitchNewSlot(slotChangeList, ref accountPlayer, ref slot1, slot1._team, true, SLOT_STATE.READY) && slotChangeList.Count > 0)
            {
              using (ROOM_CHANGE_SLOTS_PAK roomChangeSlotsPak = new ROOM_CHANGE_SLOTS_PAK(slotChangeList, room._leader, balanceTeamIdx == -1 || balanceTeamIdx == slot1._team ? 1 : 0))
                room.SendPacketToPlayers((SendPacket) roomChangeSlotsPak);
            }
            if (balanceTeamIdx == -1 || balanceTeamIdx == accountPlayer._slotId)
              return;
            slotChangeList.Clear();
            Core.models.room.Slot slot2 = room.getSlot(accountPlayer._slotId);
            if (slot2 == null)
              return;
            room.SwitchNewSlot(slotChangeList, ref accountPlayer, ref slot2, balanceTeamIdx, false, SLOT_STATE.READY);
          }
          else
          {
            foreach (int slotIdx in room.RED_TEAM)
            {
              Core.models.room.Slot slot = room.getSlot(slotIdx);
              Account player;
              if (slot != null && slot._playerId > 0L && (slot.state == SLOT_STATE.READY && room.getPlayerBySlot(slot, out player)))
                room.SwitchNewSlot(slotChangeList, ref player, ref slot, 0, true, SLOT_STATE.READY);
            }
            foreach (int slotIdx in room.BLUE_TEAM)
            {
              Core.models.room.Slot slot = room.getSlot(slotIdx);
              Account player;
              if (slot != null && slot._playerId > 0L && (slot.state == SLOT_STATE.READY && room.getPlayerBySlot(slot, out player)))
                room.SwitchNewSlot(slotChangeList, ref player, ref slot, 1, true, SLOT_STATE.READY);
            }
            if (slotChangeList.Count > 0)
            {
              using (ROOM_CHANGE_SLOTS_PAK roomChangeSlotsPak = new ROOM_CHANGE_SLOTS_PAK(slotChangeList, room._leader, balanceTeamIdx == -1 ? 1 : 0))
                room.SendPacketToPlayers((SendPacket) roomChangeSlotsPak);
            }
            if (balanceTeamIdx == -1)
              return;
            slotChangeList.Clear();
            foreach (int slotIdx in balanceTeamIdx == 1 ? room.INVERT_RED_TEAM : room.INVERT_BLUE_TEAM)
            {
              Core.models.room.Slot slot = room.getSlot(slotIdx);
              Account player;
              if (slot != null && slot._playerId > 0L && (slot.state == SLOT_STATE.READY && room._leader != slotIdx) && (room.getPlayerBySlot(slot, out player) && room.SwitchNewSlot(slotChangeList, ref player, ref slot, balanceTeamIdx, false, SLOT_STATE.READY)))
              {
                int num = balanceTeamIdx;
                balanceTeamIdx = room.getBalanceTeamIdx(inBattle);
                if (balanceTeamIdx == -1 || num != balanceTeamIdx)
                  break;
              }
            }
          }
          if (slotChangeList.Count > 0)
          {
            Logger.info("PACOTE DE BALANCEAMENTO FOI ENVIADO.");
            using (ROOM_CHANGE_SLOTS_PAK roomChangeSlotsPak = new ROOM_CHANGE_SLOTS_PAK(slotChangeList, room._leader, 1))
              room.SendPacketToPlayers((SendPacket) roomChangeSlotsPak);
          }
        }
      }
      catch
      {
      }
    }

    private bool Check4vs4(
      Room room,
      int myTeam,
      bool isLeader,
      bool isBotMode,
      ref int redPlayers,
      ref int bluePlayers,
      ref int TotalEnemys)
    {
      if (isBotMode & isLeader)
      {
        int num1 = room._leader % 2;
        int num2 = 1;
        int num3 = num1 % 2 != 0 ? num2 + bluePlayers : num2 + redPlayers;
        if (num3 > 4)
        {
          int num4 = num3 - 4;
          if (num4 > 0)
          {
            lock (room._slots)
            {
              for (int slotIdx = 15; slotIdx >= 0; --slotIdx)
              {
                if (slotIdx != room._leader && slotIdx % 2 == num1)
                {
                  Core.models.room.Slot slot = room.getSlot(slotIdx);
                  Account player;
                  if (slot != null && slot._playerId > 0L && (slot.state == SLOT_STATE.READY && room.getPlayerBySlot(slot, out player)))
                  {
                    room.changeSlotState(slot, SLOT_STATE.NORMAL, false);
                    player.SendPacket((SendPacket) new BATTLE_4VS4_ERROR_PAK());
                    if (slotIdx % 2 == 0)
                      --redPlayers;
                    else
                      --bluePlayers;
                    if (--num4 <= 0)
                      break;
                  }
                }
              }
              room.updateSlotsInfo();
              TotalEnemys = num1 != 0 ? redPlayers : bluePlayers;
              return true;
            }
          }
        }
      }
      else
      {
        if (!isLeader && (redPlayers + bluePlayers >= 8 || myTeam == 0 && redPlayers + 1 > 4 || myTeam == 1 && bluePlayers + 1 > 4))
          return true;
        int num1 = redPlayers + bluePlayers + 1;
        if (num1 > 8 && room._state <= RoomState.CountDown)
        {
          int num2 = num1 - 8;
          if (num2 > 0)
          {
            for (int index = 15; index >= 0; --index)
            {
              if (index != room._leader)
              {
                Core.models.room.Slot slot = room.getSlot(index);
                if (slot != null && slot._playerId > 0L && slot.state == SLOT_STATE.READY)
                {
                  Account playerBySlot = room.getPlayerBySlot(slot);
                  if (playerBySlot != null)
                  {
                    room.changeSlotState(index, SLOT_STATE.NORMAL, false);
                    playerBySlot.SendPacket((SendPacket) new BATTLE_4VS4_ERROR_PAK());
                    if (index % 2 == 0)
                      --redPlayers;
                    else
                      --bluePlayers;
                    if (--num2 == 0)
                      break;
                  }
                }
              }
            }
            room.updateSlotsInfo();
            TotalEnemys = room._leader % 2 != 0 ? redPlayers : bluePlayers;
            return true;
          }
        }
      }
      return false;
    }
  }
}
