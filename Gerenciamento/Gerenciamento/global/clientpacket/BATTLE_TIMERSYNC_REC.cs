
// Type: Game.global.clientpacket.BATTLE_TIMERSYNC_REC
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.models.enums;
using Core.models.enums.errors;
using Core.server;
using Game.data.model;
using Game.data.utils;
using Game.global.serverpacket;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Game.global.clientpacket
{
  public class BATTLE_TIMERSYNC_REC : ReceiveGamePacket
  {
    private float unk0;
    private uint TimeRemaining;
    private int Ping;
    private int unk5;
    private int Latency;
    private int Round;

    public BATTLE_TIMERSYNC_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read()
    {
      this.TimeRemaining = this.readUD();
      this.unk0 = this.readT();
      this.Round = (int) this.readC();
      this.Ping = (int) this.readC();
      this.unk5 = (int) this.readC();
      this.Latency = (int) this.readH();
    }

    public override void run()
    {
      try
      {
        Account player = this._client._player;
        if (player == null)
          return;
        Room room = player._room;
        if (room == null)
          return;
        bool isBotMode = room.isBotMode();
        Core.models.room.Slot slot = room.getSlot(player._slotId);
        if (slot == null || slot.state != SLOT_STATE.BATTLE)
          return;
        if ((double) this.unk0 != 1.0 || this.unk5 != 0)
          Logger.LogHack("[" + this.unk0.ToString() + "; " + this.unk5.ToString() + " (" + ((HackType) this.unk5).ToString() + ")] Player: " + player.player_name + "; Id: " + player.player_id.ToString());
        room._timeRoom = this.TimeRemaining;
        this.SyncPlayerPings(player, room, slot, isBotMode);
        if (this.TimeRemaining <= 2147483648U || room.swapRound || (!this.CompareRounds(room, this.Round) || room._state != RoomState.Battle))
          return;
        this.EndRound(room, isBotMode);
      }
      catch (Exception ex)
      {
        Logger.warning("[BATTLE_TIMERSYNC_REC] " + ex.ToString());
      }
    }

    private void SyncPlayerPings(Account p, Room room, Core.models.room.Slot slot, bool isBotMode)
    {
      if (isBotMode)
        return;
      slot.latency = this.Latency;
      slot.ping = this.Ping;
      if (slot.latency >= ConfigGS.maxBattleLatency)
        ++slot.failLatencyTimes;
      else
        slot.failLatencyTimes = 0;
      if (p.DebugPing && (DateTime.Now - p.LastPingDebug).TotalSeconds >= 5.0)
      {
        p.LastPingDebug = DateTime.Now;
        p.SendPacket((SendPacket) new AUTH_RECV_WHISPER_PAK("Latência", this.Latency.ToString() + "ms (" + this.Ping.ToString() + " barras)", true));
      }
      if (slot.failLatencyTimes >= ConfigGS.maxRepeatLatency)
      {
        Logger.error("[" + DateTime.Now.ToString("MM/dd HH:mm:ss") + "] Player '" + p.player_name + "' (Id: " + slot._playerId.ToString() + ") kicked due to high latency. (" + slot.latency.ToString() + "/" + ConfigGS.maxBattleLatency.ToString() + "ms)");
      }
      else
      {
        if ((DateTime.Now - room.LastPingSync).TotalSeconds < 5.0)
          return;
        byte[] slots = new byte[16];
        for (int index = 0; index < 16; ++index)
          slots[index] = (byte) room._slots[index].ping;
        using (BATTLE_SENDPING_PAK battleSendpingPak = new BATTLE_SENDPING_PAK(slots))
          room.SendPacketToPlayers((SendPacket) battleSendpingPak, SLOT_STATE.BATTLE, 0);
        room.LastPingSync = DateTime.Now;
      }
    }

    private bool CompareRounds(Room room, int externValue) => room.room_type == (byte) 7 || room.room_type == (byte) 12 ? room.rodada == externValue : room.rodada == externValue + 1;

    private void EndRound(Room room, bool isBotMode)
    {
      try
      {
        room.swapRound = true;
        if (room.room_type == (byte) 7 || room.room_type == (byte) 12)
        {
          if (room.rodada == 1)
          {
            room.rodada = 2;
            for (int index = 0; index < 16; ++index)
            {
              Core.models.room.Slot slot = room._slots[index];
              if (slot.state == SLOT_STATE.BATTLE)
              {
                slot.killsOnLife = 0;
                slot.lastKillState = 0;
                slot.repeatLastState = false;
              }
            }
            List<int> dinossaurs = AllUtils.getDinossaurs(room, true, -2);
            using (BATTLE_ROUND_WINNER_PAK battleRoundWinnerPak = new BATTLE_ROUND_WINNER_PAK(room, 2, RoundEndType.TimeOut))
            {
              using (BATTLE_ROUND_RESTART_PAK battleRoundRestartPak = new BATTLE_ROUND_RESTART_PAK(room, dinossaurs, isBotMode))
                room.SendPacketToPlayers((SendPacket) battleRoundWinnerPak, (SendPacket) battleRoundRestartPak, SLOT_STATE.BATTLE, 0);
            }
            room.round.StartJob(5250, (TimerCallback) (callbackState =>
            {
              if (room._state == RoomState.Battle)
              {
                room.BattleStart = DateTime.Now.AddSeconds(5.0);
                using (BATTLE_TIMERSYNC_PAK battleTimersyncPak = new BATTLE_TIMERSYNC_PAK(room))
                  room.SendPacketToPlayers((SendPacket) battleTimersyncPak, SLOT_STATE.BATTLE, 0);
              }
              room.swapRound = false;
              lock (callbackState)
                room.round.Timer = (Timer) null;
            }));
          }
          else
          {
            if (room.rodada != 2)
              return;
            AllUtils.EndBattle(room, isBotMode);
          }
        }
        else if (room.thisModeHaveRounds())
        {
          int winner = 1;
          if (room.room_type != (byte) 3)
            ++room.blue_rounds;
          else if (room.Bar1 > room.Bar2)
          {
            ++room.red_rounds;
            winner = 0;
          }
          else if (room.Bar1 < room.Bar2)
            ++room.blue_rounds;
          else
            winner = 2;
          AllUtils.BattleEndRound(room, winner, RoundEndType.TimeOut);
        }
        else
        {
          List<Account> allPlayers = room.getAllPlayers(SLOT_STATE.READY, 1);
          if (allPlayers.Count != 0)
          {
            TeamResultType winnerTeam = AllUtils.GetWinnerTeam(room);
            room.CalculateResult(winnerTeam, isBotMode);
            using (BATTLE_ROUND_WINNER_PAK battleRoundWinnerPak = new BATTLE_ROUND_WINNER_PAK(room, winnerTeam, RoundEndType.TimeOut))
            {
              ushort result1;
              ushort result2;
              byte[] data;
              AllUtils.getBattleResult(room, out result1, out result2, out data);
              byte[] completeBytes = battleRoundWinnerPak.GetCompleteBytes(nameof (BATTLE_TIMERSYNC_REC));
              foreach (Account p in allPlayers)
              {
                if (room._slots[p._slotId].state == SLOT_STATE.BATTLE)
                  p.SendCompletePacket(completeBytes);
                p.SendPacket((SendPacket) new BATTLE_ENDBATTLE_PAK(p, winnerTeam, result2, result1, isBotMode, data));
              }
            }
          }
          AllUtils.resetBattleInfo(room);
        }
      }
      catch (Exception ex)
      {
        if (room != null)
          Logger.error("[!] Crash no BATTLE_TIMERSYNC_REC, Sala: " + room._roomId.ToString() + ";" + room._channelId.ToString() + ";" + room.room_type.ToString());
        Logger.error("[BATTLE_TIMERSYNC_REC2] " + ex.ToString());
      }
    }
  }
}
