
// Type: Game.data.model.Room
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.managers;
using Core.managers.events;
using Core.managers.events.EventModels;
using Core.managers.server;
using Core.models.account.clan;
using Core.models.account.players;
using Core.models.account.rank;
using Core.models.enums;
using Core.models.enums.errors;
using Core.models.enums.flags;
using Core.models.enums.match;
using Core.models.room;
using Core.server;
using Core.xml;
using Game.data.managers;
using Game.data.sync;
using Game.data.sync.server_side;
using Game.data.utils;
using Game.data.xml;
using Game.global.serverpacket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Game.data.model
{
  public class Room
  {
    public Core.models.room.Slot[] _slots = new Core.models.room.Slot[16];
    public int _channelType;
    public int rodada = 1;
    public int TRex = -1;
    public int blue_rounds;
    public int blue_dino;
    public int red_rounds;
    public int red_dino;
    public int Bar1;
    public int Bar2;
    public int _ping = 5;
    public int _redKills;
    public int _redDeaths;
    public int _blueKills;
    public int _blueDeaths;
    public int spawnsCount;
    public int mapId;
    public int autobalans;
    public int killtime;
    public int _roomId;
    public int _channelId;
    public int _leader;
    public byte weaponsFlag;
    public byte random_map;
    public byte special;
    public byte limit;
    public byte seeConf;
    public byte aiCount = 1;
    public byte IngameAiLevel;
    public byte aiLevel;
    public byte stage4v4;
    public byte room_type;
    public readonly int[] TIMES = new int[9]
    {
      3,
      5,
      7,
      5,
      10,
      15,
      20,
      25,
      30
    };
    public readonly int[] KILLS = new int[6]
    {
      60,
      80,
      100,
      120,
      140,
      160
    };
    public readonly int[] ROUNDS = new int[6]
    {
      1,
      2,
      3,
      5,
      7,
      9
    };
    public readonly int[] RED_TEAM = new int[8]
    {
      0,
      2,
      4,
      6,
      8,
      10,
      12,
      14
    };
    public readonly int[] BLUE_TEAM = new int[8]
    {
      1,
      3,
      5,
      7,
      9,
      11,
      13,
      15
    };
    public byte[] HitParts = new byte[35];
    public byte[] DefaultParts = new byte[35]
    {
      (byte) 0,
      (byte) 1,
      (byte) 2,
      (byte) 3,
      (byte) 4,
      (byte) 5,
      (byte) 6,
      (byte) 7,
      (byte) 8,
      (byte) 9,
      (byte) 10,
      (byte) 11,
      (byte) 12,
      (byte) 13,
      (byte) 14,
      (byte) 15,
      (byte) 16,
      (byte) 17,
      (byte) 18,
      (byte) 19,
      (byte) 20,
      (byte) 21,
      (byte) 22,
      (byte) 23,
      (byte) 24,
      (byte) 25,
      (byte) 26,
      (byte) 27,
      (byte) 28,
      (byte) 29,
      (byte) 30,
      (byte) 31,
      (byte) 32,
      (byte) 33,
      (byte) 34
    };
    public readonly int[] INVERT_RED_TEAM = new int[8]
    {
      14,
      12,
      10,
      8,
      6,
      4,
      2,
      0
    };
    public readonly int[] INVERT_BLUE_TEAM = new int[8]
    {
      15,
      13,
      11,
      9,
      7,
      5,
      3,
      1
    };
    public uint _timeRoom;
    public uint StartDate;
    public uint UniqueRoomId;
    public long StartTick;
    public string name;
    public string password;
    public string _mapName;
    public VoteKick votekick;
    public RoomState _state;
    public bool C4_actived;
    public bool swapRound;
    public bool changingSlots;
    public bool blockedClan;
    public BattleServer UDPServer;
    public DateTime BattleStart;
    public DateTime LastPingSync = DateTime.Now;
    public TimerState bomba = new TimerState();
    public TimerState countdown = new TimerState();
    public TimerState round = new TimerState();
    public TimerState vote = new TimerState();
    public SafeList<long> kickedPlayers = new SafeList<long>();
    public SafeList<long> requestHost = new SafeList<long>();
    public int countdownSequence = -1;

    public Room(int roomId, Channel ch)
    {
      this._roomId = roomId;
      for (int slotIdx = 0; slotIdx < this._slots.Length; ++slotIdx)
        this._slots[slotIdx] = new Core.models.room.Slot(slotIdx);
      this._channelId = ch._id;
      this._channelType = ch._type;
      this.SetUniqueId();
    }

    public bool thisModeHaveCD()
    {
      RoomType roomType = (RoomType) this.room_type;
      switch (roomType)
      {
        case RoomType.Explosion:
        case RoomType.Annihilation:
        case RoomType.Boss:
        case RoomType.Cross_Counter:
          return true;
        default:
          return roomType == RoomType.Escort;
      }
    }

    public bool thisModeHaveRounds()
    {
      RoomType roomType = (RoomType) this.room_type;
      switch (roomType)
      {
        case RoomType.Explosion:
        case RoomType.Destroy:
        case RoomType.Annihilation:
          return true;
        default:
          return roomType == RoomType.Defense;
      }
    }

    public void LoadHitParts()
    {
      int next = new Random().Next(34);
      byte[] array = ((IEnumerable<byte>) this.DefaultParts).OrderBy<byte, bool>((Func<byte, bool>) (x => (int) x <= next)).ToArray<byte>();
      Logger.warning("By: " + next.ToString() + "/ Hits: " + BitConverter.ToString(array));
      this.HitParts = array;
      byte[] numArray = new byte[35];
      for (int index = 0; index < 35; ++index)
      {
        byte hitPart = this.HitParts[index];
        numArray[(index + 8) % 35] = hitPart;
      }
      Logger.warning("P: " + BitConverter.ToString(numArray));
    }

    private void SetUniqueId() => this.UniqueRoomId = (uint) ((ConfigGS.serverId & (int) byte.MaxValue) << 20 | (this._channelId & (int) byte.MaxValue) << 12 | this._roomId & 4095);

    public void SetBotLevel()
    {
      if (!this.isBotMode())
        return;
      this.IngameAiLevel = this.aiLevel;
      for (int index = 0; index < 16; ++index)
        this._slots[index].aiLevel = (int) this.IngameAiLevel;
    }

    public bool isBotMode() => this.special == (byte) 6 || this.special == (byte) 8 || this.special == (byte) 9;

    private void SetSpecialStage()
    {
      if (this.room_type == (byte) 5)
      {
        if (this.mapId != 39)
          return;
        this.Bar1 = 6000;
        this.Bar2 = 9000;
      }
      else
      {
        if (this.room_type != (byte) 3)
          return;
        if (this.mapId == 38)
        {
          this.Bar1 = 12000;
          this.Bar2 = 12000;
        }
        else
        {
          if (this.mapId != 35)
            return;
          this.Bar1 = 6000;
          this.Bar2 = 6000;
        }
      }
    }

    public int getInBattleTime()
    {
      int num = 0;
      if (this.BattleStart != new DateTime() && (this._state == RoomState.Battle || this._state == RoomState.PreBattle))
      {
        num = (int) (DateTime.Now - this.BattleStart).TotalSeconds;
        if (num < 0)
          num = 0;
      }
      return num;
    }

    public int getInBattleTimeLeft()
    {
      int inBattleTime = this.getInBattleTime();
      return this.getTimeByMask() * 60 - inBattleTime;
    }

    public Channel getChannel() => ChannelsXML.getChannel(this._channelId);

    public bool getChannel(out Channel ch)
    {
      ch = ChannelsXML.getChannel(this._channelId);
      return ch != null;
    }

    public bool getSlot(int slotIdx, out Core.models.room.Slot slot)
    {
      slot = (Core.models.room.Slot) null;
      lock (this._slots)
      {
        if (slotIdx >= 0 && slotIdx <= 15)
          slot = this._slots[slotIdx];
        return slot != null;
      }
    }

    public Core.models.room.Slot getSlot(int slotIdx)
    {
      lock (this._slots)
        return slotIdx >= 0 && slotIdx <= 15 ? this._slots[slotIdx] : (Core.models.room.Slot) null;
    }

    public void StartCounter(int type, Account player, Core.models.room.Slot slot)
    {
      EventErrorEnum error = EventErrorEnum.Success;
      int period;
      switch (type)
      {
        case 0:
          error = EventErrorEnum.Battle_First_MainLoad;
          period = 90000;
          break;
        case 1:
          error = EventErrorEnum.Battle_First_Hole;
          period = 30000;
          break;
        default:
          return;
      }
      slot.timing.StartJob(period, (TimerCallback) (callbackState =>
      {
        this.BaseCounter(error, player, slot);
        lock (callbackState)
          slot?.StopTiming();
      }));
    }

    private void BaseCounter(EventErrorEnum error, Account player, Core.models.room.Slot slot)
    {
      player.SendPacket((SendPacket) new SERVER_MESSAGE_KICK_BATTLE_PLAYER_PAK(error));
      player.SendPacket((SendPacket) new BATTLE_LEAVEP2PSERVER_PAK(player, 0));
      slot.state = SLOT_STATE.NORMAL;
      AllUtils.BattleEndPlayersCount(this, this.isBotMode());
      if (slot._id == this._leader)
      {
        this.setNewLeader(-1, this._state == RoomState.Battle ? 12 : 8, this._leader, true);
        Logger.info("Dono da sala foi alterado por tempo para iniciar a partida");
      }
      this.updateSlotsInfo();
    }

    public void StartBomb(int currentRound)
    {
      try
      {
        this.bomba.StartJob(42250, (TimerCallback) (callbackState =>
        {
          int num = currentRound;
          if (this.bomba.Timer == null || num != this.rodada)
            return;
          if (this != null && this.C4_actived && num == this.rodada)
          {
            ++this.red_rounds;
            this.C4_actived = false;
            AllUtils.BattleEndRound(this, 0, RoundEndType.BombFire);
          }
          lock (callbackState)
            this.bomba.Timer = (Timer) null;
        }));
      }
      catch (Exception ex)
      {
        Logger.warning("StartBomb: " + ex.ToString());
      }
    }

    public void StartVote()
    {
      try
      {
        if (this.votekick == null)
          return;
        this.vote.StartJob(20000, (TimerCallback) (callbackState =>
        {
          AllUtils.votekickResult(this);
          lock (callbackState)
            this.vote.Timer = (Timer) null;
        }));
      }
      catch (Exception ex)
      {
        Logger.warning("[Room.StartVote] " + ex.ToString());
        if (this.vote.Timer != null)
          this.vote.Timer = (Timer) null;
        this.votekick = (VoteKick) null;
      }
    }

    public void RoundRestart()
    {
      try
      {
        this.StopBomb();
        for (int index = 0; index < 16; ++index)
        {
          Core.models.room.Slot slot = this._slots[index];
          if (slot._playerId > 0L && slot.state == SLOT_STATE.BATTLE)
          {
            if (!slot._deathState.HasFlag((Enum) DeadEnum.useChat))
              slot._deathState |= DeadEnum.useChat;
            if (slot.espectador)
              slot.espectador = false;
            if (slot.killsOnLife >= 3 && this.room_type == (byte) 4)
              ++slot.objetivos;
            slot.killsOnLife = 0;
            slot.lastKillState = 0;
            slot.repeatLastState = false;
            slot.damageBar1 = (ushort) 0;
            slot.damageBar2 = (ushort) 0;
          }
        }
        this.round.StartJob(8000, (TimerCallback) (callbackState =>
        {
          for (int index = 0; index < 16; ++index)
          {
            Core.models.room.Slot slot = this._slots[index];
            if (slot._playerId > 0L)
            {
              if (!slot._deathState.HasFlag((Enum) DeadEnum.useChat))
                slot._deathState |= DeadEnum.useChat;
              if (slot.espectador)
                slot.espectador = false;
            }
          }
          this.StopBomb();
          DateTime now = DateTime.Now;
          if (this._state == RoomState.Battle)
            this.BattleStart = this.room_type == (byte) 7 || this.room_type == (byte) 12 ? now.AddSeconds(5.0) : now;
          using (BATTLE_ROUND_RESTART_PAK battleRoundRestartPak = new BATTLE_ROUND_RESTART_PAK(this))
          {
            using (BATTLE_TIMERSYNC_PAK battleTimersyncPak = new BATTLE_TIMERSYNC_PAK(this))
              this.SendPacketToPlayers((SendPacket) battleRoundRestartPak, (SendPacket) battleTimersyncPak, SLOT_STATE.BATTLE, 0);
          }
          this.StopBomb();
          this.swapRound = false;
          AllUtils.LogRoomRoundRestart(this);
          lock (callbackState)
            this.round.Timer = (Timer) null;
        }));
      }
      catch (Exception ex)
      {
        Logger.warning("[Room.RoundRestart] " + ex.ToString());
      }
    }

    public void StopBomb()
    {
      if (!this.C4_actived)
        return;
      this.C4_actived = false;
      if (this.bomba == null)
        return;
      this.bomba.Timer = (Timer) null;
    }

    public void StartBattle(bool updateInfo)
    {
      Monitor.Enter((object) this._slots);
      this._state = RoomState.Loading;
      this.countdownSequence = -1;
      this.requestHost.Clear();
      this.UDPServer = BattleServerXML.GetRandomServer();
      this.StartTick = DateTime.Now.Ticks;
      this.StartDate = uint.Parse(DateTime.Now.ToString("yyMMddHHmm"));
      this.SetBotLevel();
      AllUtils.CheckClanMatchRestrict(this);
      using (BATTLE_READYBATTLE_PAK battleReadybattlePak = new BATTLE_READYBATTLE_PAK(this))
      {
        byte[] completeBytes = battleReadybattlePak.GetCompleteBytes("Room.StartBattle");
        List<Account> allPlayers = this.getAllPlayers(SLOT_STATE.READY, 0);
        for (int index = 0; index < allPlayers.Count; ++index)
        {
          Account account = allPlayers[index];
          Core.models.room.Slot slot = this.getSlot(account._slotId);
          if (slot != null)
          {
            slot.withHost = true;
            slot.state = SLOT_STATE.LOAD;
            slot.SetMissionsClone(account._mission);
            account.SendCompletePacket(completeBytes);
            account.SetCuponsFlags();
          }
        }
      }
      if (updateInfo)
        this.updateSlotsInfo();
      Monitor.Exit((object) this._slots);
    }

    public void StartCountDown(int countdownSeq)
    {
      using (BATTLE_COUNTDOWN_PAK battleCountdownPak = new BATTLE_COUNTDOWN_PAK(CountDownEnum.Start))
        this.SendPacketToPlayers((SendPacket) battleCountdownPak);
      this.countdown.StartJob(5250, (TimerCallback) (callbackState =>
      {
        try
        {
          int num = countdownSeq;
          if (this.countdown.Timer == null || this._state != RoomState.CountDown || (num != this.countdownSequence || countdownSeq == -1) || this.countdownSequence == -1)
            return;
          if (this._slots[this._leader].state == SLOT_STATE.READY)
          {
            if (this._state == RoomState.CountDown)
              this.StartBattle(true);
          }
        }
        catch (Exception ex)
        {
          Logger.warning("[Room.StartCountDown] " + ex.ToString());
        }
        lock (callbackState)
          this.countdown.Timer = (Timer) null;
      }));
    }

    public void StopCountDown(CountDownEnum motive, bool refreshRoom = true)
    {
      this._state = RoomState.Ready;
      if (refreshRoom)
        this.updateRoomInfo();
      this.countdown.Timer = (Timer) null;
      using (BATTLE_COUNTDOWN_PAK battleCountdownPak = new BATTLE_COUNTDOWN_PAK(motive))
        this.SendPacketToPlayers((SendPacket) battleCountdownPak);
    }

    public void StopCountDown(int slotId)
    {
      if (this._state != RoomState.CountDown)
        return;
      if (slotId == this._leader)
      {
        this.StopCountDown(CountDownEnum.StopByHost);
      }
      else
      {
        if (this.getPlayingPlayers(this._leader % 2 == 0 ? 1 : 0, SLOT_STATE.READY, 0) != 0)
          return;
        this.changeSlotState(this._leader, SLOT_STATE.NORMAL, false);
        this.StopCountDown(CountDownEnum.StopByPlayer);
      }
    }

    public void CalculateResult()
    {
      lock (this._slots)
        this.BaseResultGame(AllUtils.GetWinnerTeam(this), this.isBotMode());
    }

    public void CalculateResult(TeamResultType resultType)
    {
      lock (this._slots)
        this.BaseResultGame(resultType, this.isBotMode());
    }

    public void CalculateResult(TeamResultType resultType, bool isBotMode)
    {
      lock (this._slots)
        this.BaseResultGame(resultType, isBotMode);
    }

    private void BaseResultGame(TeamResultType winnerTeam, bool isBotMode)
    {
      ServerConfig config = GameManager.Config;
      EventUpModel runningEvent1 = EventRankUpSyncer.GetRunningEvent();
      EventMapModel runningEvent2 = EventMapSyncer.GetRunningEvent();
      bool flag = EventMapSyncer.EventIsValid(runningEvent2, this.mapId, (int) this.room_type);
      PlayTimeModel runningEvent3 = EventPlayTimeSyncer.GetRunningEvent();
      DateTime now = DateTime.Now;
      if (config == null)
      {
        Logger.error("Server Config NULL. RoomResult canceled.");
      }
      else
      {
        for (int index = 0; index < 16; ++index)
        {
          Core.models.room.Slot slot = this._slots[index];
          Account player;
          if (!slot.check && slot.state == SLOT_STATE.BATTLE && this.getPlayerBySlot(slot, out player))
          {
            DBQuery query = new DBQuery();
            slot.check = true;
            double num1 = slot.inBattleTime(now);
            double num2 = slot.inBattleTime(now);
            int gp = player._gp;
            int exp = player._exp;
            int money = player._money;
            if (!isBotMode)
            {
              if (config.missions)
              {
                AllUtils.endMatchMission(this, player, slot, winnerTeam);
                if (slot.MissionsCompleted)
                {
                  player._mission = slot.Missions;
                  MissionManager.getInstance().updateCurrentMissionList(player.player_id, player._mission);
                }
                AllUtils.GenerateMissionAwards(player, query);
              }
              int num3 = slot.allKills != 0 || slot.allDeaths != 0 ? (int) num2 : (int) (num2 / 3.0);
              if (this.room_type == (byte) 2 || this.room_type == (byte) 4)
              {
                slot.exp = (int) ((double) slot.Score + (double) num3 / 2.5 + (double) slot.allDeaths * 2.2 + (double) (slot.objetivos * 20));
                slot.gp = (int) ((double) slot.Score + (double) num3 / 3.0 + (double) slot.allDeaths * 2.2 + (double) (slot.objetivos * 20));
                slot.money = (int) ((double) (slot.Score / 2) + (double) num3 / 6.5 + (double) slot.allDeaths * 1.5 + (double) (slot.objetivos * 10));
              }
              else
              {
                slot.exp = (int) ((double) slot.Score + (double) num3 / 2.5 + (double) slot.allDeaths * 1.8 + (double) (slot.objetivos * 20));
                slot.gp = (int) ((double) slot.Score + (double) num3 / 3.0 + (double) slot.allDeaths * 1.8 + (double) (slot.objetivos * 20));
                slot.money = (int) ((double) slot.Score / 1.5 + (double) num3 / 4.5 + (double) slot.allDeaths * 1.1 + (double) (slot.objetivos * 20));
              }
              bool WonTheMatch = (TeamResultType) slot._team == winnerTeam;
              if (this.room_type != (byte) 13 && this.room_type != (byte) 8)
              {
                player._statistic.headshots_count += slot.headshots;
                player._statistic.kills_count += slot.allKills;
                player._statistic.totalkills_count += slot.allKills;
                player._statistic.deaths_count += slot.allDeaths;
                this.AddKDInfosToQuery(slot, player._statistic, query);
                AllUtils.updateMatchCount(WonTheMatch, player, (int) winnerTeam, query);
              }
              if (WonTheMatch)
              {
                slot.gp += AllUtils.percentage(slot.gp, 15);
                slot.exp += AllUtils.percentage(slot.exp, 20);
              }
              if (slot.earnedXP > 0)
                slot.exp += slot.earnedXP * 5;
            }
            else
            {
              int num3 = (int) this.IngameAiLevel * (150 + slot.allDeaths);
              if (num3 == 0)
                ++num3;
              int num4 = slot.Score / num3;
              slot.gp += num4;
              slot.exp += num4;
            }
            slot.exp = slot.exp > ConfigGS.maxBattleXP ? ConfigGS.maxBattleXP : slot.exp;
            slot.gp = slot.gp > ConfigGS.maxBattleGP ? ConfigGS.maxBattleGP : slot.gp;
            slot.money = slot.money > ConfigGS.maxBattleMY ? ConfigGS.maxBattleMY : slot.money;
            if (slot.exp < 0 || slot.gp < 0 || slot.money < 1500)
            {
              slot.gp = ConfigGS.ReceiveGold;
              slot.money = ConfigGS.ReceiveCash;
            }
            int percent1 = 0;
            int percent2 = 0;
            if (runningEvent1 != null | flag)
            {
              if (runningEvent1 != null)
              {
                percent1 += runningEvent1._percentXp;
                percent2 += runningEvent1._percentGp;
              }
              if (flag)
              {
                percent1 += runningEvent2._percentXp;
                percent2 += runningEvent2._percentGp;
              }
              if (!slot.bonusFlags.HasFlag((Enum) ResultIcon.Event))
                slot.bonusFlags |= ResultIcon.Event;
            }
            PlayerBonus bonus = player._bonus;
            if (bonus != null && bonus.bonuses > 0)
            {
              if ((bonus.bonuses & 8) == 8)
                percent1 += 100;
              if ((bonus.bonuses & 128) == 128)
                percent2 += 100;
              if ((bonus.bonuses & 4) == 4)
                percent1 += 50;
              if ((bonus.bonuses & 64) == 64)
                percent2 += 50;
              if ((bonus.bonuses & 2) == 2)
                percent1 += 30;
              if ((bonus.bonuses & 32) == 32)
                percent2 += 30;
              if ((bonus.bonuses & 1) == 1)
                percent1 += 10;
              if ((bonus.bonuses & 16) == 16)
                percent2 += 10;
              if (!slot.bonusFlags.HasFlag((Enum) ResultIcon.Item))
                slot.bonusFlags |= ResultIcon.Item;
            }
            if (player.pc_cafe == 2 || player.pc_cafe == 1)
            {
              percent1 += player.pc_cafe == 2 ? 120 : 60;
              percent2 += player.pc_cafe == 2 ? 100 : 40;
              if (player.pc_cafe == 1 && !slot.bonusFlags.HasFlag((Enum) ResultIcon.Pc))
                slot.bonusFlags |= ResultIcon.Pc;
              else if (player.pc_cafe == 2 && !slot.bonusFlags.HasFlag((Enum) ResultIcon.PcPlus))
                slot.bonusFlags |= ResultIcon.PcPlus;
            }
            slot.BonusXP = AllUtils.percentage(slot.exp, percent1);
            slot.BonusGP = AllUtils.percentage(slot.gp, percent2);
            player._gp += slot.gp + slot.BonusGP;
            player._exp += slot.exp + slot.BonusXP;
            if (ConfigGS.winCashPerBattle)
              player._money += slot.money;
            RankModel rank = RankXML.getRank(player._rank);
            if (rank != null && player._exp >= rank._onNextLevel + rank._onAllExp && player._rank <= 50)
            {
              List<ItemsModel> awards = RankXML.getAwards(player._rank);
              while (player._exp >= rank._onNextLevel + rank._onAllExp)
              {
                if (player._exp < rank._onNextLevel + rank._onAllExp)
                  return;
                if (awards.Count > 0)
                  player.SendPacket((SendPacket) new INVENTORY_ITEM_CREATE_PAK(1, player, awards));
                player._gp += rank._onGPUp;
                player.SendPacket((SendPacket) new BASE_RANK_UP_PAK(++player._rank, rank._onNextLevel));
                player.LastRankUpDate = uint.Parse(now.ToString("yyMMddHHmm"));
                GameManager.SendUpToAll(player);
                rank = RankXML.getRank(player._rank);
                awards = RankXML.getAwards(player._rank);
              }
              query.AddQuery("rank", (object) player._rank);
              query.AddQuery("last_rankup_date", (object) (long) player.LastRankUpDate);
            }
            if (runningEvent3 != null)
              AllUtils.playTimeEvent((long) num2, player, runningEvent3, isBotMode);
            //player.DailyCashEvent((long) num1, isBotMode);
            AllUtils.DiscountPlayerItems(slot, player);
            if (gp != player._gp)
              query.AddQuery("gp", (object) player._gp);
            if (exp != player._exp)
              query.AddQuery("exp", (object) player._exp);
            if (money != player._money)
              query.AddQuery("money", (object) player._money);
            ComDiv.updateDB("accounts", "player_id", (object) player.player_id, query.GetTables(), query.GetValues());
            if (ConfigGS.winCashPerBattle && ConfigGS.showCashReceiveWarn)
              player.SendPacket((SendPacket) new SERVER_MESSAGE_ANNOUNCE_PAK(Translation.GetLabel("CashReceived", (object) slot.money)));
          }
        }
        this.updateSlotsInfo();
        this.CalculateClanMatchResult((int) winnerTeam);
      }
    }

    private void AddKDInfosToQuery(Core.models.room.Slot slot, PlayerStats stats, DBQuery query)
    {
      if (slot.allKills > 0)
      {
        query.AddQuery("kills_count", (object) stats.kills_count);
        query.AddQuery("totalkills_count", (object) stats.totalkills_count);
      }
      if (slot.allDeaths > 0)
        query.AddQuery("deaths_count", (object) stats.deaths_count);
      if (slot.headshots <= 0)
        return;
      query.AddQuery("headshots_count", (object) stats.headshots_count);
    }

    private void CalculateClanMatchResult(int winnerTeam)
    {
      if (this._channelType != 4 || this.blockedClan)
        return;
      SortedList<int, Clan> sortedList = new SortedList<int, Clan>();
      for (int index = 0; index < 16; ++index)
      {
        Core.models.room.Slot slot = this._slots[index];
        Account player;
        if (slot.state == SLOT_STATE.BATTLE && this.getPlayerBySlot(slot, out player))
        {
          Clan clan = ClanManager.getClan(player.clanId);
          if (clan.id != 0)
          {
            bool WonTheMatch = slot._team == winnerTeam;
            clan.exp += slot.exp;
            clan.BestPlayers.SetBestExp(slot);
            clan.BestPlayers.SetBestKills(slot);
            clan.BestPlayers.SetBestHeadshot(slot);
            clan.BestPlayers.SetBestWins(player._statistic, slot, WonTheMatch);
            clan.BestPlayers.SetBestParticipation(player._statistic, slot);
            if (!sortedList.ContainsKey(player.clanId))
            {
              sortedList.Add(player.clanId, clan);
              if (winnerTeam != 2)
              {
                this.CalculateSpecialCM(clan, winnerTeam, slot._team);
                if (WonTheMatch)
                  ++clan.vitorias;
                else
                  ++clan.derrotas;
              }
              PlayerManager.updateClanBattles(clan.id, ++clan.partidas, clan.vitorias, clan.derrotas);
            }
          }
        }
      }
      foreach (Clan clan in (IEnumerable<Clan>) sortedList.Values)
      {
        PlayerManager.updateClanExp(clan.id, clan.exp);
        if (this.special == (byte) 5)
          PlayerManager.updateClanPoints(clan.id, clan.pontos);
        PlayerManager.updateBestPlayers(clan);
        RankModel rank = ClanRankXML.getRank((int) clan.rank);
        if (rank != null && clan.exp >= rank._onNextLevel + rank._onAllExp)
          PlayerManager.updateClanRank(clan.id, (int) ++clan.rank);
      }
    }

    private void CalculateSpecialCM(Clan clan, int winnerTeam, int teamIdx)
    {
      if (this.special != (byte) 5 || winnerTeam == 2)
        return;
      if (winnerTeam == teamIdx)
      {
        float num = 25f + (this.room_type != (byte) 1 ? (teamIdx == 0 ? (float) this.red_rounds : (float) this.blue_rounds) : (float) ((teamIdx == 0 ? this._redKills : this._blueKills) / 20));
        Logger.warning("Clan: " + clan.id.ToString() + "; Earned Points: " + num.ToString());
        clan.pontos += num;
        Logger.warning("Clan: " + clan.id.ToString() + "; Final Points: " + clan.pontos.ToString());
      }
      else if ((double) clan.pontos == 0.0)
      {
        Logger.warning("Clã não perdeu Pontos devido a baixa pontuação.");
      }
      else
      {
        float num = 40f - (this.room_type != (byte) 1 ? (teamIdx == 0 ? (float) this.red_rounds : (float) this.blue_rounds) : (float) ((teamIdx == 0 ? this._redKills : this._blueKills) / 20));
        Logger.warning("Clan: " + clan.id.ToString() + "; Losed Points: " + num.ToString());
        clan.pontos -= num;
        Logger.warning("Clan: " + clan.id.ToString() + "; Final Points: " + clan.pontos.ToString());
      }
    }

    public bool isStartingMatch() => this._state > RoomState.Ready;

    public bool isPreparing() => this._state >= RoomState.Loading;

    public void updateRoomInfo()
    {
      using (BATTLE_ROOM_INFO_PAK battleRoomInfoPak = new BATTLE_ROOM_INFO_PAK(this))
        this.SendPacketToPlayers((SendPacket) battleRoomInfoPak);
    }

    public void initSlotCount(int count)
    {
      if (this.stage4v4 == (byte) 1)
        count = 8;
      if (count <= 0)
        count = 1;
      for (int index = 0; index < this._slots.Length; ++index)
      {
        if (index >= count)
          this._slots[index].state = SLOT_STATE.CLOSE;
      }
    }

    public int getSlotCount()
    {
      lock (this._slots)
      {
        int num = 0;
        for (int index = 0; index < this._slots.Length; ++index)
        {
          if (this._slots[index].state != SLOT_STATE.CLOSE)
            ++num;
        }
        return num;
      }
    }

    public bool SwitchNewSlot(
      List<SLOT_CHANGE> slotsList,
      ref Account p,
      ref Core.models.room.Slot old,
      int teamIdx,
      bool smallSlot,
      SLOT_STATE newState = SLOT_STATE.NORMAL)
    {
      lock (this._slots)
      {
        foreach (int team in this.GetTeamArray(teamIdx))
        {
          Core.models.room.Slot slot = this._slots[team];
          if (p != null && old != null && (slot != null && slot._playerId == 0L) && slot.state == SLOT_STATE.EMPTY)
          {
            if (smallSlot && old._id <= slot._id)
              return false;
            slot.ResetSlot();
            slot.state = newState;
            slot._playerId = p.player_id;
            slot._equip = p._equip;
            slot.ResetSlot();
            old.state = SLOT_STATE.EMPTY;
            old._playerId = 0L;
            old._equip = (PlayerEquipedItems) null;
            if (p._slotId == this._leader)
              this._leader = team;
            p._slotId = team;
            p.updateCacheInfo();
            slotsList.Add(new SLOT_CHANGE()
            {
              oldSlot = old,
              newSlot = slot
            });
            return true;
          }
        }
      }
      return false;
    }

    public void SwitchSlots(
      List<SLOT_CHANGE> slots,
      int newSlotId,
      int oldSlotId,
      bool changeReady)
    {
      Core.models.room.Slot slot1 = this._slots[newSlotId];
      Core.models.room.Slot slot2 = this._slots[oldSlotId];
      if (changeReady)
      {
        if (slot1.state == SLOT_STATE.READY)
          slot1.state = SLOT_STATE.NORMAL;
        if (slot2.state == SLOT_STATE.READY)
          slot2.state = SLOT_STATE.NORMAL;
      }
      slot1.SetSlotId(oldSlotId);
      slot2.SetSlotId(newSlotId);
      this._slots[newSlotId] = slot2;
      this._slots[oldSlotId] = slot1;
      slots.Add(new SLOT_CHANGE()
      {
        oldSlot = slot1,
        newSlot = slot2
      });
    }

    public void changeSlotState(int slotId, SLOT_STATE state, bool sendInfo) => this.changeSlotState(this.getSlot(slotId), state, sendInfo);

    public void changeSlotState(Core.models.room.Slot slot, SLOT_STATE state, bool sendInfo)
    {
      if (slot == null || slot.state == state)
        return;
      slot.state = state;
      if (state == SLOT_STATE.EMPTY || state == SLOT_STATE.CLOSE)
      {
        AllUtils.ResetSlotInfo(this, slot, false);
        slot._playerId = 0L;
      }
      if (!sendInfo)
        return;
      this.updateSlotsInfo();
    }

    public Account getPlayerBySlot(Core.models.room.Slot slot)
    {
      try
      {
        long playerId = slot._playerId;
        return playerId > 0L ? AccountManager.getAccount(playerId, true) : (Account) null;
      }
      catch
      {
        return (Account) null;
      }
    }

    public Account getPlayerBySlot(int slotId)
    {
      try
      {
        long playerId = this._slots[slotId]._playerId;
        return playerId > 0L ? AccountManager.getAccount(playerId, true) : (Account) null;
      }
      catch
      {
        return (Account) null;
      }
    }

    public bool getPlayerBySlot(int slotId, out Account player)
    {
      try
      {
        long playerId = this._slots[slotId]._playerId;
        player = playerId > 0L ? AccountManager.getAccount(playerId, true) : (Account) null;
        return player != null;
      }
      catch
      {
        player = (Account) null;
        return false;
      }
    }

    public bool getPlayerBySlot(Core.models.room.Slot slot, out Account player)
    {
      try
      {
        long playerId = slot._playerId;
        player = playerId > 0L ? AccountManager.getAccount(playerId, true) : (Account) null;
        return player != null;
      }
      catch
      {
        player = (Account) null;
        return false;
      }
    }

    public int getTimeByMask() => this.TIMES[this.killtime >> 4];

    public int getRoundsByMask() => this.ROUNDS[this.killtime & 15];

    public int getKillsByMask() => this.KILLS[this.killtime & 15];

    public void updateSlotsInfo()
    {
      using (ROOM_GET_SLOTINFO_PAK roomGetSlotinfoPak = new ROOM_GET_SLOTINFO_PAK(this))
        this.SendPacketToPlayers((SendPacket) roomGetSlotinfoPak);
    }

    public bool getLeader(out Account p)
    {
      p = (Account) null;
      if (this.getAllPlayers().Count <= 0)
        return false;
      if (this._leader == -1)
        this.setNewLeader(-1, 0, -1, false);
      if (this._leader >= 0)
        p = AccountManager.getAccount(this._slots[this._leader]._playerId, true);
      return p != null;
    }

    public Account getLeader()
    {
      if (this.getAllPlayers().Count <= 0)
        return (Account) null;
      if (this._leader == -1)
        this.setNewLeader(-1, 0, -1, false);
      return this._leader != -1 ? AccountManager.getAccount(this._slots[this._leader]._playerId, true) : (Account) null;
    }

    public void setNewLeader(int leader, int state, int oldLeader, bool updateInfo)
    {
      Monitor.Enter((object) this._slots);
      if (leader == -1)
      {
        for (int index = 0; index < 16; ++index)
        {
          Core.models.room.Slot slot = this._slots[index];
          if (index != oldLeader && slot._playerId > 0L && slot.state > (SLOT_STATE) state)
          {
            this._leader = index;
            break;
          }
        }
      }
      else
        this._leader = leader;
      if (this._leader != -1)
      {
        Core.models.room.Slot slot = this._slots[this._leader];
        if (slot.state == SLOT_STATE.READY)
          slot.state = SLOT_STATE.NORMAL;
        if (updateInfo)
          this.updateSlotsInfo();
      }
      Monitor.Exit((object) this._slots);
    }

    public void SendPacketToPlayers(SendPacket packet)
    {
      List<Account> allPlayers = this.getAllPlayers();
      if (allPlayers.Count == 0)
        return;
      byte[] completeBytes = packet.GetCompleteBytes("Room.SendPacketToPlayers(SendPacket)");
      for (int index = 0; index < allPlayers.Count; ++index)
        allPlayers[index].SendCompletePacket(completeBytes);
    }

    public void SendPacketToPlayers(SendPacket packet, long player_id)
    {
      List<Account> allPlayers = this.getAllPlayers(player_id);
      if (allPlayers.Count == 0)
        return;
      byte[] completeBytes = packet.GetCompleteBytes("Room.SendPacketToPlayers(SendPacket,long)");
      for (int index = 0; index < allPlayers.Count; ++index)
        allPlayers[index].SendCompletePacket(completeBytes);
    }

    public void SendPacketToPlayers(SendPacket packet, SLOT_STATE state, int type)
    {
      List<Account> allPlayers = this.getAllPlayers(state, type);
      if (allPlayers.Count == 0)
        return;
      byte[] completeBytes = packet.GetCompleteBytes("Room.SendPacketToPlayers(SendPacket,SLOT_STATE,int)");
      for (int index = 0; index < allPlayers.Count; ++index)
        allPlayers[index].SendCompletePacket(completeBytes);
    }

    public void SendPacketToPlayers(
      SendPacket packet,
      SendPacket packet2,
      SLOT_STATE state,
      int type)
    {
      List<Account> allPlayers = this.getAllPlayers(state, type);
      if (allPlayers.Count == 0)
        return;
      byte[] completeBytes1 = packet.GetCompleteBytes("Room.SendPacketToPlayers(SendPacket,SendPacket,SLOT_STATE,int)-1");
      byte[] completeBytes2 = packet2.GetCompleteBytes("Room.SendPacketToPlayers(SendPacket,SendPacket,SLOT_STATE,int)-2");
      for (int index = 0; index < allPlayers.Count; ++index)
      {
        Account account = allPlayers[index];
        account.SendCompletePacket(completeBytes1);
        account.SendCompletePacket(completeBytes2);
      }
    }

    public void SendPacketToPlayers(SendPacket packet, SLOT_STATE state, int type, int exception)
    {
      List<Account> allPlayers = this.getAllPlayers(state, type, exception);
      if (allPlayers.Count == 0)
        return;
      byte[] completeBytes = packet.GetCompleteBytes("Room.SendPacketToPlayers(SendPacket,SLOT_STATE,int,int)");
      for (int index = 0; index < allPlayers.Count; ++index)
        allPlayers[index].SendCompletePacket(completeBytes);
    }

    public void SendPacketToPlayers(
      SendPacket packet,
      SLOT_STATE state,
      int type,
      int exception,
      int exception2)
    {
      List<Account> allPlayers = this.getAllPlayers(state, type, exception, exception2);
      if (allPlayers.Count == 0)
        return;
      byte[] completeBytes = packet.GetCompleteBytes("Room.SendPacketToPlayers(SendPacket,SLOT_STATE,int,int,int)");
      for (int index = 0; index < allPlayers.Count; ++index)
        allPlayers[index].SendCompletePacket(completeBytes);
    }

    public void RemovePlayer(Account player, bool WarnAllPlayers, int quitMotive = 0)
    {
      Core.models.room.Slot slot;
      if (player == null || !this.getSlot(player._slotId, out slot))
        return;
      this.BaseRemovePlayer(player, slot, WarnAllPlayers, quitMotive);
    }

    public void RemovePlayer(Account player, Core.models.room.Slot slot, bool WarnAllPlayers, int quitMotive = 0)
    {
      if (player == null || slot == null)
        return;
      this.BaseRemovePlayer(player, slot, WarnAllPlayers, quitMotive);
    }

    private void BaseRemovePlayer(Account player, Core.models.room.Slot slot, bool WarnAllPlayers, int quitMotive)
    {
      Monitor.Enter((object) this._slots);
      bool flag = false;
      bool host = false;
      if (player != null && slot != null)
      {
        if (slot.state >= SLOT_STATE.LOAD)
        {
          if (this._leader == slot._id)
          {
            int leader = this._leader;
            int state = 1;
            if (this._state == RoomState.Battle)
              state = 12;
            else if (this._state >= RoomState.Loading)
              state = 8;
            if (this.getAllPlayers(slot._id).Count >= 1)
              this.setNewLeader(-1, state, this._leader, false);
            if (this.getPlayingPlayers(2, SLOT_STATE.READY, 1) >= 2)
            {
              using (BATTLE_GIVEUPBATTLE_PAK battleGiveupbattlePak = new BATTLE_GIVEUPBATTLE_PAK(this, leader))
                this.SendPacketToPlayers((SendPacket) battleGiveupbattlePak, SLOT_STATE.RENDEZVOUS, 1, slot._id);
            }
            host = true;
          }
          using (BATTLE_LEAVEP2PSERVER_PAK leaveP2PserverPak = new BATTLE_LEAVEP2PSERVER_PAK(player, quitMotive))
            this.SendPacketToPlayers((SendPacket) leaveP2PserverPak, SLOT_STATE.READY, 1, !WarnAllPlayers ? slot._id : -1);
          BATTLE_LEAVE_SYNC.SendUDPPlayerLeave(this, slot._id);
          slot.ResetSlot();
          if (this.votekick != null)
            this.votekick.TotalArray[slot._id] = false;
        }
        slot._playerId = 0L;
        slot._equip = (PlayerEquipedItems) null;
        slot.state = SLOT_STATE.EMPTY;
        if (this._state == RoomState.CountDown)
        {
          if (slot._id == this._leader)
          {
            this._state = RoomState.Ready;
            flag = true;
            this.countdown.Timer = (Timer) null;
            using (BATTLE_COUNTDOWN_PAK battleCountdownPak = new BATTLE_COUNTDOWN_PAK(CountDownEnum.StopByHost))
              this.SendPacketToPlayers((SendPacket) battleCountdownPak);
          }
          else if (this.getPlayingPlayers(slot._team, SLOT_STATE.READY, 0) == 0)
          {
            if (slot._id != this._leader)
              this.changeSlotState(this._leader, SLOT_STATE.NORMAL, false);
            this.StopCountDown(CountDownEnum.StopByPlayer, false);
            flag = true;
          }
        }
        else if (this.isPreparing())
        {
          AllUtils.BattleEndPlayersCount(this, this.isBotMode());
          if (this._state == RoomState.Battle)
            AllUtils.BattleEndRoundPlayersCount(this);
        }
        this.CheckToEndWaitingBattle(host);
        this.requestHost.Remove(player.player_id);
        if (this.vote.Timer != null && this.votekick != null && (this.votekick.victimIdx == player._slotId && quitMotive != 2))
        {
          this.vote.Timer = (Timer) null;
          this.votekick = (VoteKick) null;
          using (VOTEKICK_CANCEL_VOTE_PAK votekickCancelVotePak = new VOTEKICK_CANCEL_VOTE_PAK())
            this.SendPacketToPlayers((SendPacket) votekickCancelVotePak, SLOT_STATE.BATTLE, 0);
        }
        Match match = player._match;
        if (match != null && player.matchSlot >= 0)
        {
          match._slots[player.matchSlot].state = SlotMatchState.Normal;
          using (CLAN_WAR_REGIST_MERCENARY_PAK registMercenaryPak = new CLAN_WAR_REGIST_MERCENARY_PAK(match))
            match.SendPacketToPlayers((SendPacket) registMercenaryPak);
        }
        player._room = (Room) null;
        Logger.LogProblems("[Room.RemovePlayer] Jogador '" + player.player_id.ToString() + "' '" + player.player_name + "'; OldSlot: " + player._slotId.ToString() + "; NewSlot: -1", "errorC");
        player._slotId = -1;
        player._status.updateRoom(byte.MaxValue);
        AllUtils.syncPlayerToClanMembers(player);
        AllUtils.syncPlayerToFriends(player, false);
        player.updateCacheInfo();
      }
      this.updateSlotsInfo();
      if (flag)
        this.updateRoomInfo();
      Monitor.Exit((object) this._slots);
    }

    public int addPlayer(Account p)
    {
      lock (this._slots)
      {
        for (int index = 0; index < 16; ++index)
        {
          Core.models.room.Slot slot = this._slots[index];
          if (slot._playerId == 0L && slot.state == SLOT_STATE.EMPTY)
          {
            slot._playerId = p.player_id;
            slot.state = SLOT_STATE.NORMAL;
            p._room = this;
            Logger.LogProblems("[Room.addPlayer] Jogador '" + p.player_id.ToString() + "' '" + p.player_name + "'; OldSlot: " + p._slotId.ToString() + "; NewSlot: " + index.ToString(), "errorC");
            p._slotId = index;
            slot._equip = p._equip;
            p._status.updateRoom((byte) this._roomId);
            AllUtils.syncPlayerToClanMembers(p);
            AllUtils.syncPlayerToFriends(p, false);
            p.SetCuponsFlags();
            p.updateCacheInfo();
            return index;
          }
        }
      }
      return -1;
    }

    public int addPlayer(Account p, int teamIdx)
    {
      int[] teamArray = this.GetTeamArray(teamIdx);
      lock (this._slots)
      {
        for (int index1 = 0; index1 < teamArray.Length; ++index1)
        {
          int index2 = teamArray[index1];
          Core.models.room.Slot slot = this._slots[index2];
          if (slot._playerId == 0L && slot.state == SLOT_STATE.EMPTY)
          {
            slot._playerId = p.player_id;
            slot.state = SLOT_STATE.NORMAL;
            p._room = this;
            Logger.LogProblems("[Room.addPlayer2] Jogador '" + p.player_id.ToString() + "' '" + p.player_name + "'; OldSlot: " + p._slotId.ToString() + "; NewSlot: " + index1.ToString(), "errorC");
            p._slotId = index2;
            slot._equip = p._equip;
            p._status.updateRoom((byte) this._roomId);
            AllUtils.syncPlayerToClanMembers(p);
            AllUtils.syncPlayerToFriends(p, false);
            p.SetCuponsFlags();
            p.updateCacheInfo();
            return index2;
          }
        }
      }
      return -1;
    }

    public int[] GetTeamArray(int index) => index != 0 ? this.BLUE_TEAM : this.RED_TEAM;

    public List<Account> getAllPlayers(SLOT_STATE state, int type)
    {
      List<Account> accountList = new List<Account>();
      lock (this._slots)
      {
        for (int index = 0; index < this._slots.Length; ++index)
        {
          Core.models.room.Slot slot = this._slots[index];
          long playerId = slot._playerId;
          if (playerId > 0L && (type == 0 && slot.state == state || type == 1 && slot.state > state))
          {
            Account account = AccountManager.getAccount(playerId, true);
            if (account != null && account._slotId != -1)
              accountList.Add(account);
          }
        }
      }
      return accountList;
    }

    public List<Account> getAllPlayers(SLOT_STATE state, int type, int exception)
    {
      List<Account> accountList = new List<Account>();
      lock (this._slots)
      {
        for (int index = 0; index < this._slots.Length; ++index)
        {
          Core.models.room.Slot slot = this._slots[index];
          long playerId = slot._playerId;
          if (playerId > 0L && index != exception && (type == 0 && slot.state == state || type == 1 && slot.state > state))
          {
            Account account = AccountManager.getAccount(playerId, true);
            if (account != null && account._slotId != -1)
              accountList.Add(account);
          }
        }
      }
      return accountList;
    }

    public List<Account> getAllPlayers(
      SLOT_STATE state,
      int type,
      int exception,
      int exception2)
    {
      List<Account> accountList = new List<Account>();
      lock (this._slots)
      {
        for (int index = 0; index < this._slots.Length; ++index)
        {
          Core.models.room.Slot slot = this._slots[index];
          long playerId = slot._playerId;
          if (playerId > 0L && index != exception && index != exception2 && (type == 0 && slot.state == state || type == 1 && slot.state > state))
          {
            Account account = AccountManager.getAccount(playerId, true);
            if (account != null && account._slotId != -1)
              accountList.Add(account);
          }
        }
      }
      return accountList;
    }

    public List<Account> getAllPlayers(int exception)
    {
      List<Account> accountList = new List<Account>();
      lock (this._slots)
      {
        for (int index = 0; index < this._slots.Length; ++index)
        {
          long playerId = this._slots[index]._playerId;
          if (playerId > 0L && index != exception)
          {
            Account account = AccountManager.getAccount(playerId, true);
            if (account != null && account._slotId != -1)
              accountList.Add(account);
          }
        }
      }
      return accountList;
    }

    public List<Account> getAllPlayers(long exception)
    {
      List<Account> accountList = new List<Account>();
      lock (this._slots)
      {
        for (int index = 0; index < this._slots.Length; ++index)
        {
          long playerId = this._slots[index]._playerId;
          if (playerId > 0L && playerId != exception)
          {
            Account account = AccountManager.getAccount(playerId, true);
            if (account != null && account._slotId != -1)
              accountList.Add(account);
          }
        }
      }
      return accountList;
    }

    public List<Account> getAllPlayers()
    {
      List<Account> accountList = new List<Account>();
      lock (this._slots)
      {
        for (int index = 0; index < this._slots.Length; ++index)
        {
          long playerId = this._slots[index]._playerId;
          if (playerId > 0L)
          {
            Account account = AccountManager.getAccount(playerId, true);
            if (account != null && account._slotId != -1)
              accountList.Add(account);
          }
        }
      }
      return accountList;
    }

    public int getPlayingPlayers(int team, bool inBattle)
    {
      int num = 0;
      lock (this._slots)
      {
        for (int index = 0; index < 16; ++index)
        {
          Core.models.room.Slot slot = this._slots[index];
          if (slot._playerId > 0L && (slot._team == team || team == 2) && (inBattle && slot.state == SLOT_STATE.BATTLE && !slot.espectador || !inBattle && slot.state >= SLOT_STATE.RENDEZVOUS))
            ++num;
        }
      }
      return num;
    }

    public int getPlayingPlayers(int team, SLOT_STATE state, int type)
    {
      int num = 0;
      lock (this._slots)
      {
        for (int index = 0; index < 16; ++index)
        {
          Core.models.room.Slot slot = this._slots[index];
          if (slot._playerId > 0L && (type == 0 && slot.state == state || type == 1 && slot.state > state) && (team == 2 || slot._team == team))
            ++num;
        }
      }
      return num;
    }

    public int getPlayingPlayers(int team, SLOT_STATE state, int type, int exception)
    {
      int num = 0;
      lock (this._slots)
      {
        for (int index = 0; index < 16; ++index)
        {
          Core.models.room.Slot slot = this._slots[index];
          if (index != exception && slot._playerId > 0L && (type == 0 && slot.state == state || type == 1 && slot.state > state) && (team == 2 || slot._team == team))
            ++num;
        }
      }
      return num;
    }

    public void getPlayingPlayers(bool inBattle, out int RedPlayers, out int BluePlayers)
    {
      RedPlayers = 0;
      BluePlayers = 0;
      lock (this._slots)
      {
        for (int index = 0; index < 16; ++index)
        {
          Core.models.room.Slot slot = this._slots[index];
          if (slot._playerId > 0L && (inBattle && slot.state == SLOT_STATE.BATTLE && !slot.espectador || !inBattle && slot.state >= SLOT_STATE.RENDEZVOUS))
          {
            if (slot._team == 0)
              ++RedPlayers;
            else
              ++BluePlayers;
          }
        }
      }
    }

    public void getPlayingPlayers(
      bool inBattle,
      out int RedPlayers,
      out int BluePlayers,
      out int RedDeaths,
      out int BlueDeaths)
    {
      RedPlayers = 0;
      BluePlayers = 0;
      RedDeaths = 0;
      BlueDeaths = 0;
      lock (this._slots)
      {
        for (int index = 0; index < 16; ++index)
        {
          Core.models.room.Slot slot = this._slots[index];
          if (slot._deathState.HasFlag((Enum) DeadEnum.isDead))
          {
            if (slot._team == 0)
              ++RedDeaths;
            else
              ++BlueDeaths;
          }
          if (slot._playerId > 0L && (inBattle && slot.state == SLOT_STATE.BATTLE && !slot.espectador || !inBattle && slot.state >= SLOT_STATE.RENDEZVOUS))
          {
            if (slot._team == 0)
              ++RedPlayers;
            else
              ++BluePlayers;
          }
        }
      }
    }

    public void CheckToEndWaitingBattle(bool host)
    {
      if (this._state != RoomState.CountDown && this._state != RoomState.Loading && this._state != RoomState.Rendezvous || !host && this._slots[this._leader].state != SLOT_STATE.BATTLE_READY)
        return;
      AllUtils.EndBattleNoPoints(this);
    }

    public void SpawnReadyPlayers()
    {
      lock (this._slots)
        this.BaseSpawnReadyPlayers(this.isBotMode());
    }

    public void SpawnReadyPlayers(bool isBotMode)
    {
      lock (this._slots)
        this.BaseSpawnReadyPlayers(isBotMode);
    }

    private void BaseSpawnReadyPlayers(bool isBotMode)
    {
      DateTime now = DateTime.Now;
      for (int index = 0; index < 16; ++index)
      {
        Core.models.room.Slot slot = this._slots[index];
        if (slot.state == SLOT_STATE.BATTLE_READY && slot.isPlaying == 0 && slot._playerId > 0L)
        {
          slot.isPlaying = 1;
          slot.startTime = now;
          slot.state = SLOT_STATE.BATTLE;
          if (this._state == RoomState.Battle && (this.room_type == (byte) 2 || this.room_type == (byte) 4))
            slot.espectador = true;
        }
      }
      this.updateSlotsInfo();
      List<int> dinossaurs = AllUtils.getDinossaurs(this, false, -1);
      if (this._state == RoomState.PreBattle)
      {
        this.BattleStart = this.room_type == (byte) 7 || this.room_type == (byte) 12 ? now.AddMinutes(5.0) : now;
        this.SetSpecialStage();
      }
      bool flag = false;
      using (BATTLE_ROUND_RESTART_PAK battleRoundRestartPak = new BATTLE_ROUND_RESTART_PAK(this, dinossaurs, isBotMode))
      {
        using (BATTLE_TIMERSYNC_PAK battleTimersyncPak = new BATTLE_TIMERSYNC_PAK(this))
        {
          using (BATTLE_RECORD_PAK battleRecordPak = new BATTLE_RECORD_PAK(this))
          {
            byte[] completeBytes1 = battleRoundRestartPak.GetCompleteBytes("Room.BaseSpawnReadyPlayers-1");
            byte[] completeBytes2 = battleTimersyncPak.GetCompleteBytes("Room.BaseSpawnReadyPlayers-2");
            byte[] completeBytes3 = battleRecordPak.GetCompleteBytes("Room.BaseSpawnReadyPlayers-3");
            for (int index = 0; index < 16; ++index)
            {
              Core.models.room.Slot slot = this._slots[index];
              Account player;
              if (slot.state == SLOT_STATE.BATTLE && slot.isPlaying == 1 && this.getPlayerBySlot(slot, out player))
              {
                slot.isPlaying = 2;
                if (this._state == RoomState.PreBattle)
                {
                  using (BATTLE_STARTBATTLE_PAK battleStartbattlePak = new BATTLE_STARTBATTLE_PAK(slot, player, dinossaurs, isBotMode, true))
                    this.SendPacketToPlayers((SendPacket) battleStartbattlePak, SLOT_STATE.READY, 1);
                  player.SendCompletePacket(completeBytes1);
                  if (this.room_type == (byte) 7 || this.room_type == (byte) 12)
                    flag = true;
                  else
                    player.SendCompletePacket(completeBytes2);
                }
                else if (this._state == RoomState.Battle)
                {
                  using (BATTLE_STARTBATTLE_PAK battleStartbattlePak = new BATTLE_STARTBATTLE_PAK(slot, player, dinossaurs, isBotMode, false))
                    this.SendPacketToPlayers((SendPacket) battleStartbattlePak, SLOT_STATE.READY, 1);
                  if (this.room_type == (byte) 2 || this.room_type == (byte) 4)
                    Game_SyncNet.SendUDPPlayerSync(this, slot, (CupomEffects) 0, 1);
                  else
                    player.SendCompletePacket(completeBytes1);
                  player.SendCompletePacket(completeBytes2);
                  player.SendCompletePacket(completeBytes3);
                }
              }
            }
          }
        }
      }
      if (this._state == RoomState.PreBattle)
      {
        this._state = RoomState.Battle;
        this.updateRoomInfo();
      }
      if (!flag)
        return;
      this.StartDinoRound();
    }

    private void StartDinoRound() => this.round.StartJob(5250, (TimerCallback) (callbackState =>
    {
      if (this._state == RoomState.Battle)
      {
        using (BATTLE_TIMERSYNC_PAK battleTimersyncPak = new BATTLE_TIMERSYNC_PAK(this))
          this.SendPacketToPlayers((SendPacket) battleTimersyncPak, SLOT_STATE.BATTLE, 0);
        this.swapRound = false;
      }
      lock (callbackState)
        this.round.Timer = (Timer) null;
    }));

    public int getBalanceTeamIdx(bool inBattle)
    {
      int num1 = 0;
      int num2 = 0;
      for (int index = 0; index < 16; ++index)
      {
        Core.models.room.Slot slot = this._slots[index];
        if (slot.state == SLOT_STATE.READY && !inBattle || slot.state >= SLOT_STATE.READY & inBattle)
        {
          if (slot._team == 0)
            ++num1;
          else
            ++num2;
        }
      }
      if (num1 + 1 < num2)
        return 0;
      return num2 + 1 >= num1 ? -1 : 1;
    }
  }
}
