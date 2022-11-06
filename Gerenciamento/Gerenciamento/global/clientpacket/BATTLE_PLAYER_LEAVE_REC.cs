
// Type: Game.global.clientpacket.BATTLE_PLAYER_LEAVE_REC
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.models.enums;
using Core.models.room;
using Core.server;
using Game.data.model;
using Game.data.utils;
using Game.global.serverpacket;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Game.global.clientpacket
{
  public class BATTLE_PLAYER_LEAVE_REC : ReceiveGamePacket
  {
    private bool isFinished;
    private long objId;

    public BATTLE_PLAYER_LEAVE_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read() => this.objId = this.readQ();

    public override void run()
    {
      try
      {
        Account player = this._client._player;
        Room room = player == null ? (Room) null : player._room;
        Core.models.room.Slot slot1;
        if (room == null || room._state < RoomState.Loading || (!room.getSlot(player._slotId, out slot1) || slot1.state < SLOT_STATE.LOAD))
          return;
        bool isBotMode = room.isBotMode();
        this.FreepassEffect(player, slot1, room, isBotMode);
        if (room.vote.Timer != null && room.votekick != null && room.votekick.victimIdx == slot1._id)
        {
          room.vote.Timer = (Timer) null;
          room.votekick = (VoteKick) null;
          using (VOTEKICK_CANCEL_VOTE_PAK votekickCancelVotePak = new VOTEKICK_CANCEL_VOTE_PAK())
            room.SendPacketToPlayers((SendPacket) votekickCancelVotePak, SLOT_STATE.BATTLE, 0, slot1._id);
        }
        AllUtils.ResetSlotInfo(room, slot1, true);
        int red13 = 0;
        int blue13 = 0;
        int num1 = 0;
        int num2 = 0;
        for (int index = 0; index < 16; ++index)
        {
          Core.models.room.Slot slot2 = room._slots[index];
          if (slot2.state >= SLOT_STATE.LOAD)
          {
            if (slot2._team == 0)
              ++num1;
            else
              ++num2;
            if (slot2.state == SLOT_STATE.BATTLE)
            {
              if (slot2._team == 0)
                ++red13;
              else
                ++blue13;
            }
          }
        }
        if (slot1._id == room._leader)
        {
          if (isBotMode)
          {
            if (red13 > 0 || blue13 > 0)
              this.LeaveHostBOT_GiveBattle(room, player);
            else
              this.LeaveHostBOT_EndBattle(room, player);
          }
          else if (room._state == RoomState.Battle && (red13 == 0 || blue13 == 0) || room._state <= RoomState.PreBattle && (num1 == 0 || num2 == 0))
            this.LeaveHostNoBOT_EndBattle(room, player, red13, blue13);
          else
            this.LeaveHostNoBOT_GiveBattle(room, player);
        }
        else if (!isBotMode)
        {
          if (room._state == RoomState.Battle && (red13 == 0 || blue13 == 0) || room._state <= RoomState.PreBattle && (num1 == 0 || num2 == 0))
            this.LeavePlayerNoBOT_EndBattle(room, player, red13, blue13);
          else
            this.LeavePlayer_QuitBattle(room, player);
        }
        else
          this.LeavePlayer_QuitBattle(room, player);
        this._client.SendPacket((SendPacket) new BATTLE_LEAVEP2PSERVER_PAK(player, 0));
        if (this.isFinished || room._state != RoomState.Battle)
          return;
        AllUtils.BattleEndRoundPlayersCount(room);
      }
      catch (Exception ex)
      {
        Logger.warning("[BATTLE_PLAYER_LEAVE_REC] " + ex.ToString());
      }
    }

    private void FreepassEffect(Account p, Core.models.room.Slot slot, Room room, bool isBotMode)
    {
      DBQuery dbQuery = new DBQuery();
      if (p._bonus.freepass == 0 || p._bonus.freepass == 1 && room._channelType == 4)
      {
        if (isBotMode || slot.state < SLOT_STATE.BATTLE_READY)
          return;
        if (p._gp > 0)
        {
          p._gp -= 200;
          if (p._gp < 0)
            p._gp = 0;
          dbQuery.AddQuery("gp", (object) p._gp);
        }
        dbQuery.AddQuery("escapes", (object) ++p._statistic.escapes);
      }
      else
      {
        if (room._state != RoomState.Battle)
          return;
        int num1 = 0;
        int num2 = 0;
        int num3;
        int num4;
        if (isBotMode)
        {
          int num5 = (int) room.IngameAiLevel * (150 + slot.allDeaths);
          if (num5 == 0)
            ++num5;
          int num6 = slot.Score / num5;
          num3 = num2 + num6;
          num4 = num1 + num6;
        }
        else
        {
          int num5 = slot.allKills != 0 || slot.allDeaths != 0 ? (int) slot.inBattleTime(DateTime.Now) : 0;
          if (room.room_type == (byte) 2 || room.room_type == (byte) 4)
          {
            num4 = (int) ((double) slot.Score + (double) num5 / 2.5 + (double) slot.allDeaths * 2.2 + (double) (slot.objetivos * 20));
            num3 = (int) ((double) slot.Score + (double) num5 / 3.0 + (double) slot.allDeaths * 2.2 + (double) (slot.objetivos * 20));
          }
          else
          {
            num4 = (int) ((double) slot.Score + (double) num5 / 2.5 + (double) slot.allDeaths * 1.8 + (double) (slot.objetivos * 20));
            num3 = (int) ((double) slot.Score + (double) num5 / 3.0 + (double) slot.allDeaths * 1.8 + (double) (slot.objetivos * 20));
          }
        }
        p._exp += ConfigGS.maxBattleXP < num4 ? ConfigGS.maxBattleXP : num4;
        p._gp += ConfigGS.maxBattleGP < num3 ? ConfigGS.maxBattleGP : num3;
        if (num3 > 0)
          dbQuery.AddQuery("gp", (object) p._gp);
        if (num4 > 0)
          dbQuery.AddQuery("exp", (object) p._exp);
      }
      ComDiv.updateDB("accounts", "player_id", (object) p.player_id, dbQuery.GetTables(), dbQuery.GetValues());
    }

    private void LeaveHostBOT_GiveBattle(Room room, Account p)
    {
      List<Account> allPlayers = room.getAllPlayers(SLOT_STATE.READY, 1);
      if (allPlayers.Count == 0)
        return;
      int leader = room._leader;
      room.setNewLeader(-1, 12, room._leader, true);
      using (BATTLE_LEAVEP2PSERVER_PAK leaveP2PserverPak = new BATTLE_LEAVEP2PSERVER_PAK(p, 0))
      {
        using (BATTLE_GIVEUPBATTLE_PAK battleGiveupbattlePak = new BATTLE_GIVEUPBATTLE_PAK(room, leader))
        {
          byte[] completeBytes1 = leaveP2PserverPak.GetCompleteBytes("BATTLE_PLAYER_LEAVE_REC-1");
          byte[] completeBytes2 = battleGiveupbattlePak.GetCompleteBytes("BATTLE_PLAYER_LEAVE_REC-2");
          foreach (Account account in allPlayers)
          {
            Core.models.room.Slot slot = room.getSlot(account._slotId);
            if (slot != null)
            {
              if (slot.state >= SLOT_STATE.PRESTART)
                account.SendCompletePacket(completeBytes2);
              account.SendCompletePacket(completeBytes1);
            }
          }
        }
      }
    }

    private void LeaveHostBOT_EndBattle(Room room, Account p)
    {
      List<Account> allPlayers = room.getAllPlayers(SLOT_STATE.READY, 1);
      if (allPlayers.Count != 0)
      {
        using (BATTLE_LEAVEP2PSERVER_PAK leaveP2PserverPak = new BATTLE_LEAVEP2PSERVER_PAK(p, 0))
        {
          byte[] completeBytes = leaveP2PserverPak.GetCompleteBytes("BATTLE_PLAYER_LEAVE_REC-3");
          TeamResultType winnerTeam = AllUtils.GetWinnerTeam(room);
          ushort result1;
          ushort result2;
          byte[] data;
          AllUtils.getBattleResult(room, out result1, out result2, out data);
          foreach (Account p1 in allPlayers)
          {
            p1.SendCompletePacket(completeBytes);
            p1.SendPacket((SendPacket) new BATTLE_ENDBATTLE_PAK(p1, winnerTeam, result2, result1, true, data));
          }
        }
      }
      AllUtils.resetBattleInfo(room);
    }

    private void LeaveHostNoBOT_EndBattle(Room room, Account p, int red13, int blue13)
    {
      this.isFinished = true;
      List<Account> allPlayers = room.getAllPlayers(SLOT_STATE.READY, 1);
      if (allPlayers.Count != 0)
      {
        TeamResultType winnerTeam = AllUtils.GetWinnerTeam(room, red13, blue13);
        if (room._state == RoomState.Battle)
          room.CalculateResult(winnerTeam, false);
        using (BATTLE_LEAVEP2PSERVER_PAK leaveP2PserverPak = new BATTLE_LEAVEP2PSERVER_PAK(p, 0))
        {
          byte[] completeBytes = leaveP2PserverPak.GetCompleteBytes("BATTLE_PLAYER_LEAVE_REC-4");
          ushort result1;
          ushort result2;
          byte[] data;
          AllUtils.getBattleResult(room, out result1, out result2, out data);
          foreach (Account p1 in allPlayers)
          {
            p1.SendCompletePacket(completeBytes);
            p1.SendPacket((SendPacket) new BATTLE_ENDBATTLE_PAK(p1, winnerTeam, result2, result1, false, data));
          }
        }
      }
      AllUtils.resetBattleInfo(room);
    }

    private void LeaveHostNoBOT_GiveBattle(Room room, Account p)
    {
      List<Account> allPlayers = room.getAllPlayers(SLOT_STATE.READY, 1);
      if (allPlayers.Count == 0)
        return;
      int leader = room._leader;
      int state = room._state == RoomState.Battle ? 12 : 8;
      room.setNewLeader(-1, state, room._leader, true);
      using (BATTLE_GIVEUPBATTLE_PAK battleGiveupbattlePak = new BATTLE_GIVEUPBATTLE_PAK(room, leader))
      {
        using (BATTLE_LEAVEP2PSERVER_PAK leaveP2PserverPak = new BATTLE_LEAVEP2PSERVER_PAK(p, 0))
        {
          byte[] completeBytes1 = battleGiveupbattlePak.GetCompleteBytes("BATTLE_PLAYER_LEAVE_REC-6");
          byte[] completeBytes2 = leaveP2PserverPak.GetCompleteBytes("BATTLE_PLAYER_LEAVE_REC-7");
          foreach (Account account in allPlayers)
          {
            if (room._slots[account._slotId].state >= SLOT_STATE.PRESTART)
              account.SendCompletePacket(completeBytes1);
            account.SendCompletePacket(completeBytes2);
          }
        }
      }
    }

    private void LeavePlayerNoBOT_EndBattle(Room room, Account p, int red13, int blue13)
    {
      this.isFinished = true;
      TeamResultType winnerTeam = AllUtils.GetWinnerTeam(room, red13, blue13);
      List<Account> allPlayers = room.getAllPlayers(SLOT_STATE.READY, 1);
      if (allPlayers.Count != 0)
      {
        if (room._state == RoomState.Battle)
          room.CalculateResult(winnerTeam, false);
        using (BATTLE_LEAVEP2PSERVER_PAK leaveP2PserverPak = new BATTLE_LEAVEP2PSERVER_PAK(p, 0))
        {
          byte[] completeBytes = leaveP2PserverPak.GetCompleteBytes("BATTLE_PLAYER_LEAVE_REC-8");
          ushort result1;
          ushort result2;
          byte[] data;
          AllUtils.getBattleResult(room, out result1, out result2, out data);
          foreach (Account p1 in allPlayers)
          {
            p1.SendCompletePacket(completeBytes);
            p1.SendPacket((SendPacket) new BATTLE_ENDBATTLE_PAK(p1, winnerTeam, result2, result1, false, data));
          }
        }
      }
      AllUtils.resetBattleInfo(room);
    }

    private void LeavePlayer_QuitBattle(Room room, Account p)
    {
      using (BATTLE_LEAVEP2PSERVER_PAK leaveP2PserverPak = new BATTLE_LEAVEP2PSERVER_PAK(p, 0))
        room.SendPacketToPlayers((SendPacket) leaveP2PserverPak, SLOT_STATE.READY, 1);
    }
  }
}
