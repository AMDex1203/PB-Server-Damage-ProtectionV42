
// Type: Game.data.utils.AllUtils
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.managers;
using Core.managers.events;
using Core.managers.events.EventModels;
using Core.models.account;
using Core.models.account.mission;
using Core.models.account.players;
using Core.models.enums;
using Core.models.enums.flags;
using Core.models.enums.friends;
using Core.models.enums.item;
using Core.models.enums.missions;
using Core.models.enums.room;
using Core.models.room;
using Core.server;
using Core.sql;
using Core.xml;
using Game.data.managers;
using Game.data.model;
using Game.data.sync;
using Game.data.sync.server_side;
using Game.data.xml;
using Game.global.serverpacket;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;

namespace Game.data.utils
{
  public static class AllUtils
  {
    public static int getKillScore(KillingMessage msg)
    {
      int num = 0;
      switch (msg)
      {
        case KillingMessage.PiercingShot:
        case KillingMessage.MassKill:
          num += 6;
          goto case KillingMessage.Suicide;
        case KillingMessage.ChainStopper:
          num += 8;
          goto case KillingMessage.Suicide;
        case KillingMessage.Headshot:
          num += 10;
          goto case KillingMessage.Suicide;
        case KillingMessage.ChainHeadshot:
          num += 14;
          goto case KillingMessage.Suicide;
        case KillingMessage.ChainSlugger:
          num += 6;
          goto case KillingMessage.Suicide;
        case KillingMessage.Suicide:
          return num;
        case KillingMessage.ObjectDefense:
          num += 7;
          goto case KillingMessage.Suicide;
        default:
          num += 5;
          goto case KillingMessage.Suicide;
      }
    }

    public static void CompleteMission(
      Room room,
      Core.models.room.Slot slot,
      FragInfos kills,
      MISSION_TYPE autoComplete,
      int moreInfo)
    {
      try
      {
        Account playerBySlot = room.getPlayerBySlot(slot);
        if (playerBySlot == null)
          return;
        AllUtils.MissionCompleteBase(room, playerBySlot, slot, kills, autoComplete, moreInfo);
      }
      catch (Exception ex)
      {
        Logger.error("[AllUtils.CompleteMission1] " + ex.ToString());
      }
    }

    public static void CompleteMission(
      Room room,
      Core.models.room.Slot slot,
      MISSION_TYPE autoComplete,
      int moreInfo)
    {
      try
      {
        Account playerBySlot = room.getPlayerBySlot(slot);
        if (playerBySlot == null)
          return;
        AllUtils.MissionCompleteBase(room, playerBySlot, slot, autoComplete, moreInfo);
      }
      catch (Exception ex)
      {
        Logger.error("[AllUtils.CompleteMission2] " + ex.ToString());
      }
    }

    public static void CompleteMission(
      Room room,
      Account player,
      Core.models.room.Slot slot,
      FragInfos kills,
      MISSION_TYPE autoComplete,
      int moreInfo)
    {
      AllUtils.MissionCompleteBase(room, player, slot, kills, autoComplete, moreInfo);
    }

    public static void CompleteMission(
      Room room,
      Account player,
      Core.models.room.Slot slot,
      MISSION_TYPE autoComplete,
      int moreInfo)
    {
      AllUtils.MissionCompleteBase(room, player, slot, autoComplete, moreInfo);
    }

    public static int GetItemIdClass(int id)
    {
      try
      {
        return id / 10000;
      }
      catch (Exception ex)
      {
        Logger.Exception(ex);
      }
      return 0;
    }

    private static void MissionCompleteBase(
      Room room,
      Account pR,
      Core.models.room.Slot slot,
      FragInfos kills,
      MISSION_TYPE autoComplete,
      int moreInfo)
    {
      try
      {
        PlayerMissions missions = slot.Missions;
        if (missions == null)
        {
          Logger.error(DateTime.Now.ToString("HH:mm:ss") + ": Missions null1! by accountId: " + slot._playerId.ToString());
        }
        else
        {
          int currentMissionId = missions.getCurrentMissionId();
          int currentCard = missions.getCurrentCard();
          if (currentMissionId <= 0 || missions.selectedCard)
            return;
          List<Card> cards = MissionCardXML.getCards(currentMissionId, currentCard);
          if (cards.Count == 0)
            return;
          KillingMessage allKillFlags = kills.GetAllKillFlags();
          byte[] currentMissionList = missions.getCurrentMissionList();
          ClassType idClassType = ComDiv.getIdClassType(kills.weapon);
          ClassType convertedClass1 = AllUtils.ConvertWeaponClass(idClassType);
          int idStatics = ComDiv.getIdStatics(kills.weapon, 4);
          ClassType weaponClass = moreInfo > 0 ? ComDiv.getIdClassType(moreInfo) : ClassType.Unknown;
          ClassType convertedClass2 = moreInfo > 0 ? AllUtils.ConvertWeaponClass(weaponClass) : ClassType.Unknown;
          int weaponId = moreInfo > 0 ? ComDiv.getIdStatics(moreInfo, 4) : 0;
          for (int index = 0; index < cards.Count; ++index)
          {
            Card card = cards[index];
            int num = 0;
            if (card._mapId == 0 || card._mapId == room.mapId)
            {
              if (kills.frags.Count > 0)
              {
                if (card._missionType == MISSION_TYPE.KILL || card._missionType == MISSION_TYPE.CHAINSTOPPER && allKillFlags.HasFlag((Enum) KillingMessage.ChainStopper) || (card._missionType == MISSION_TYPE.CHAINSLUGGER && allKillFlags.HasFlag((Enum) KillingMessage.ChainSlugger) || card._missionType == MISSION_TYPE.CHAINKILLER && slot.killsOnLife >= 4) || (card._missionType == MISSION_TYPE.TRIPLE_KILL && slot.killsOnLife == 3 || card._missionType == MISSION_TYPE.DOUBLE_KILL && slot.killsOnLife == 2 || card._missionType == MISSION_TYPE.HEADSHOT && (allKillFlags.HasFlag((Enum) KillingMessage.Headshot) || allKillFlags.HasFlag((Enum) KillingMessage.ChainHeadshot))) || (card._missionType == MISSION_TYPE.CHAINHEADSHOT && allKillFlags.HasFlag((Enum) KillingMessage.ChainHeadshot) || card._missionType == MISSION_TYPE.PIERCING && allKillFlags.HasFlag((Enum) KillingMessage.PiercingShot) || card._missionType == MISSION_TYPE.MASS_KILL && allKillFlags.HasFlag((Enum) KillingMessage.MassKill) || card._missionType == MISSION_TYPE.KILL_MAN && (room.room_type == (byte) 7 || room.room_type == (byte) 12) && (slot._team == 1 && room.rodada == 2 || slot._team == 0 && room.rodada == 1)))
                  num = AllUtils.CheckPlayersClass1(card, idClassType, convertedClass1, idStatics, kills);
                else if (card._missionType == MISSION_TYPE.KILL_WEAPONCLASS || card._missionType == MISSION_TYPE.DOUBLE_KILL_WEAPONCLASS && slot.killsOnLife == 2 || card._missionType == MISSION_TYPE.TRIPLE_KILL_WEAPONCLASS && slot.killsOnLife == 3)
                  num = AllUtils.CheckPlayersClass2(card, kills);
              }
              else if (card._missionType == MISSION_TYPE.DEATHBLOW && autoComplete == MISSION_TYPE.DEATHBLOW)
                num = AllUtils.CheckPlayerClass(card, weaponClass, convertedClass2, weaponId);
              else if (card._missionType == autoComplete)
                num = 1;
            }
            if (num != 0)
            {
              int arrayIdx = card._arrayIdx;
              if ((int) currentMissionList[arrayIdx] + 1 <= card._missionLimit)
              {
                slot.MissionsCompleted = true;
                currentMissionList[arrayIdx] += (byte) num;
                if ((int) currentMissionList[arrayIdx] > card._missionLimit)
                  currentMissionList[arrayIdx] = (byte) card._missionLimit;
                int progress = (int) currentMissionList[arrayIdx];
                pR.SendPacket((SendPacket) new BASE_QUEST_COMPLETE_PAK(progress, card));
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        Logger.error(ex.ToString());
      }
    }

    private static void MissionCompleteBase(
      Room room,
      Account pR,
      Core.models.room.Slot slot,
      MISSION_TYPE autoComplete,
      int moreInfo)
    {
      try
      {
        PlayerMissions missions = slot.Missions;
        if (missions == null)
        {
          Logger.error(DateTime.Now.ToString("HH:mm:ss") + ": Missions null2! by accountId: " + slot._playerId.ToString());
        }
        else
        {
          int currentMissionId = missions.getCurrentMissionId();
          int currentCard = missions.getCurrentCard();
          if (currentMissionId <= 0 || missions.selectedCard)
            return;
          List<Card> cards = MissionCardXML.getCards(currentMissionId, currentCard);
          if (cards.Count == 0)
            return;
          byte[] currentMissionList = missions.getCurrentMissionList();
          ClassType weaponClass = moreInfo > 0 ? ComDiv.getIdClassType(moreInfo) : ClassType.Unknown;
          ClassType convertedClass = moreInfo > 0 ? AllUtils.ConvertWeaponClass(weaponClass) : ClassType.Unknown;
          int weaponId = moreInfo > 0 ? ComDiv.getIdStatics(moreInfo, 4) : 0;
          for (int index = 0; index < cards.Count; ++index)
          {
            Card card = cards[index];
            int num = 0;
            if (card._mapId == 0 || card._mapId == room.mapId)
            {
              if (card._missionType == MISSION_TYPE.DEATHBLOW && autoComplete == MISSION_TYPE.DEATHBLOW)
                num = AllUtils.CheckPlayerClass(card, weaponClass, convertedClass, weaponId);
              else if (card._missionType == autoComplete)
                num = 1;
            }
            if (num != 0)
            {
              int arrayIdx = card._arrayIdx;
              if ((int) currentMissionList[arrayIdx] + 1 <= card._missionLimit)
              {
                slot.MissionsCompleted = true;
                currentMissionList[arrayIdx] += (byte) num;
                if ((int) currentMissionList[arrayIdx] > card._missionLimit)
                  currentMissionList[arrayIdx] = (byte) card._missionLimit;
                int progress = (int) currentMissionList[arrayIdx];
                pR.SendPacket((SendPacket) new BASE_QUEST_COMPLETE_PAK(progress, card));
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        Logger.error(ex.ToString());
      }
    }

    private static int CheckPlayersClass1(
      Card card,
      ClassType weaponClass,
      ClassType convertedClass,
      int weaponId,
      FragInfos infos)
    {
      int num = 0;
      if ((card._weaponReqId == 0 || card._weaponReqId == weaponId) && (card._weaponReq == ClassType.Unknown || card._weaponReq == weaponClass || card._weaponReq == convertedClass))
      {
        for (int index = 0; index < infos.frags.Count; ++index)
        {
          if (infos.frags[index].VictimSlot % 2 != (int) infos.killerIdx % 2)
            ++num;
        }
      }
      return num;
    }

    private static int CheckPlayersClass2(Card card, FragInfos infos)
    {
      int num = 0;
      for (int index = 0; index < infos.frags.Count; ++index)
      {
        Frag frag = infos.frags[index];
        if (frag.VictimSlot % 2 != (int) infos.killerIdx % 2 && (card._weaponReq == ClassType.Unknown || card._weaponReq == (ClassType) frag.victimWeaponClass || card._weaponReq == AllUtils.ConvertWeaponClass((ClassType) frag.victimWeaponClass)))
          ++num;
      }
      return num;
    }

    private static int CheckPlayerClass(
      Card card,
      ClassType weaponClass,
      ClassType convertedClass,
      int weaponId,
      int killerId,
      Frag frag)
    {
      return (card._weaponReqId == 0 || card._weaponReqId == weaponId) && (card._weaponReq == ClassType.Unknown || card._weaponReq == weaponClass || card._weaponReq == convertedClass) && frag.VictimSlot % 2 != killerId % 2 ? 1 : 0;
    }

    private static int CheckPlayerClass(
      Card card,
      ClassType weaponClass,
      ClassType convertedClass,
      int weaponId)
    {
      return (card._weaponReqId == 0 || card._weaponReqId == weaponId) && (card._weaponReq == ClassType.Unknown || card._weaponReq == weaponClass || card._weaponReq == convertedClass) ? 1 : 0;
    }

    private static ClassType ConvertWeaponClass(ClassType weaponClass)
    {
      switch (weaponClass)
      {
        case ClassType.DualHandGun:
          return ClassType.HandGun;
        case ClassType.DualKnife:
        case ClassType.Knuckle:
          return ClassType.Knife;
        case ClassType.DualSMG:
          return ClassType.SMG;
        case ClassType.DualShotgun:
          return ClassType.Shotgun;
        default:
          return weaponClass;
      }
    }

    public static TeamResultType GetWinnerTeam(Room room)
    {
      if (room == null)
        return TeamResultType.TeamDraw;
      byte num = 0;
      if (room.room_type == (byte) 2 || room.room_type == (byte) 3 || (room.room_type == (byte) 4 || room.room_type == (byte) 5))
      {
        if (room.blue_rounds == room.red_rounds)
          num = (byte) 2;
        else if (room.blue_rounds > room.red_rounds)
          num = (byte) 1;
        else if (room.blue_rounds < room.red_rounds)
          num = (byte) 0;
      }
      else if (room.room_type == (byte) 7)
      {
        if (room.blue_dino == room.red_dino)
          num = (byte) 2;
        else if (room.blue_dino > room.red_dino)
          num = (byte) 1;
        else if (room.blue_dino < room.red_dino)
          num = (byte) 0;
      }
      else if (room._blueKills == room._redKills)
        num = (byte) 2;
      else if (room._blueKills > room._redKills)
        num = (byte) 1;
      else if (room._blueKills < room._redKills)
        num = (byte) 0;
      return (TeamResultType) num;
    }

    public static TeamResultType GetWinnerTeam(
      Room room,
      int RedPlayers,
      int BluePlayers)
    {
      if (room == null)
        return TeamResultType.TeamDraw;
      byte num = 2;
      if (RedPlayers == 0)
        num = (byte) 1;
      else if (BluePlayers == 0)
        num = (byte) 0;
      return (TeamResultType) num;
    }

    public static void endMatchMission(
      Room room,
      Account player,
      Core.models.room.Slot slot,
      TeamResultType winnerTeam)
    {
      if (winnerTeam == TeamResultType.TeamDraw)
        return;
      AllUtils.CompleteMission(room, player, slot, (TeamResultType) slot._team == winnerTeam ? MISSION_TYPE.WIN : MISSION_TYPE.DEFEAT, 0);
    }

    public static void updateMatchCount(
      bool WonTheMatch,
      Account p,
      int winnerTeam,
      DBQuery query)
    {
      if (winnerTeam == 2)
        query.AddQuery("fights_draw", (object) ++p._statistic.fights_draw);
      else if (WonTheMatch)
        query.AddQuery("fights_win", (object) ++p._statistic.fights_win);
      else
        query.AddQuery("fights_lost", (object) ++p._statistic.fights_lost);
      query.AddQuery("fights", (object) ++p._statistic.fights);
      query.AddQuery("totalfights_count", (object) ++p._statistic.totalfights_count);
    }

    public static void GenerateMissionAwards(Account player, DBQuery query)
    {
      PlayerMissions mission = player._mission;
      int actualMission = mission.actualMission;
      int currentMissionId = mission.getCurrentMissionId();
      int currentCard = mission.getCurrentCard();
      if (currentMissionId <= 0 || mission.selectedCard)
        return;
      int num1 = 0;
      int num2 = 0;
      byte[] currentMissionList = mission.getCurrentMissionList();
      List<Card> cards = MissionCardXML.getCards(currentMissionId, -1);
      for (int index = 0; index < cards.Count; ++index)
      {
        Card card = cards[index];
        if ((int) currentMissionList[card._arrayIdx] >= card._missionLimit)
        {
          ++num2;
          if (card._cardBasicId == currentCard)
            ++num1;
        }
      }
      if (num2 >= 40)
      {
        int blueOrder = player.blue_order;
        int brooch = player.brooch;
        int medal = player.medal;
        int insignia = player.insignia;
        CardAwards award1 = MissionCardXML.getAward(currentMissionId, currentCard);
        if (award1 != null)
        {
          player.brooch += award1._brooch;
          player.medal += award1._medal;
          player.insignia += award1._insignia;
          player._gp += award1._gp;
          player._exp += award1._exp;
        }
        MisAwards award2 = MissionAwards.getAward(currentMissionId);
        if (award2 != null)
        {
          player.blue_order += award2._blueOrder;
          player._exp += award2._exp;
          player._gp += award2._gp;
        }
        List<ItemsModel> missionAwards = MissionCardXML.getMissionAwards(currentMissionId);
        if (missionAwards.Count > 0)
          player.SendPacket((SendPacket) new INVENTORY_ITEM_CREATE_PAK(1, player, missionAwards));
        player.SendPacket((SendPacket) new BASE_QUEST_ALERT_PAK(273U, 4, player));
        if (player.brooch != brooch)
          query.AddQuery("brooch", (object) player.brooch);
        if (player.insignia != insignia)
          query.AddQuery("insignia", (object) player.insignia);
        if (player.medal != medal)
          query.AddQuery("medal", (object) player.medal);
        if (player.blue_order != blueOrder)
          query.AddQuery("blue_order", (object) player.blue_order);
        query.AddQuery("mission_id" + (actualMission + 1).ToString(), (object) 0);
        ComDiv.updateDB("player_missions", "owner_id", (object) player.player_id, new string[2]
        {
          "card" + (actualMission + 1).ToString(),
          "mission" + (actualMission + 1).ToString()
        }, (object) 0, (object) new byte[0]);
        switch (actualMission)
        {
          case 0:
            mission.mission1 = 0;
            mission.card1 = 0;
            mission.list1 = new byte[40];
            break;
          case 1:
            mission.mission2 = 0;
            mission.card2 = 0;
            mission.list2 = new byte[40];
            break;
          case 2:
            mission.mission3 = 0;
            mission.card3 = 0;
            mission.list3 = new byte[40];
            break;
          case 3:
            mission.mission4 = 0;
            mission.card4 = 0;
            mission.list4 = new byte[40];
            if (player._event == null)
              break;
            player._event.LastQuestFinish = 1;
            ComDiv.updateDB("player_events", "last_quest_finish", (object) 1, "player_id", (object) player.player_id);
            break;
        }
      }
      else
      {
        if (num1 != 4 || mission.selectedCard)
          return;
        CardAwards award = MissionCardXML.getAward(currentMissionId, currentCard);
        if (award != null)
        {
          int brooch = player.brooch;
          int medal = player.medal;
          int insignia = player.insignia;
          player.brooch += award._brooch;
          player.medal += award._medal;
          player.insignia += award._insignia;
          player._gp += award._gp;
          player._exp += award._exp;
          if (player.brooch != brooch)
            query.AddQuery("brooch", (object) player.brooch);
          if (player.insignia != insignia)
            query.AddQuery("insignia", (object) player.insignia);
          if (player.medal != medal)
            query.AddQuery("medal", (object) player.medal);
        }
        mission.selectedCard = true;
        player.SendPacket((SendPacket) new BASE_QUEST_ALERT_PAK(1U, 1, player));
      }
    }

    public static void ResetSlotInfo(Room room, Core.models.room.Slot slot, bool updateInfo)
    {
      if (slot.state < SLOT_STATE.LOAD)
        return;
      room.changeSlotState(slot, SLOT_STATE.NORMAL, updateInfo);
      slot.ResetSlot();
    }

    public static void votekickResult(Room room)
    {
      VoteKick votekick = room.votekick;
      if (votekick != null)
      {
        int inGamePlayers = votekick.GetInGamePlayers();
        if (votekick.kikar > votekick.deixar && votekick.enemys > 0 && (votekick.allies > 0 && votekick._votes.Count >= inGamePlayers / 2))
        {
          Account playerBySlot = room.getPlayerBySlot(votekick.victimIdx);
          if (playerBySlot != null)
          {
            playerBySlot.SendPacket((SendPacket) new VOTEKICK_KICK_WARNING_PAK());
            room.kickedPlayers.Add(playerBySlot.player_id);
            room.RemovePlayer(playerBySlot, true, 2);
          }
        }
        uint erro = 0;
        if (votekick.allies == 0)
          erro = 2147488001U;
        else if (votekick.enemys == 0)
          erro = 2147488002U;
        else if (votekick.deixar < votekick.kikar || votekick._votes.Count < inGamePlayers / 2)
          erro = 2147488000U;
        using (VOTEKICK_RESULT_PAK votekickResultPak = new VOTEKICK_RESULT_PAK(erro, votekick))
          room.SendPacketToPlayers((SendPacket) votekickResultPak, SLOT_STATE.BATTLE, 0);
        AllUtils.LogVotekickResult(room);
      }
      room.votekick = (VoteKick) null;
    }

    public static void resetBattleInfo(Room room)
    {
      AllUtils.LogRoomResult(room);
      for (int index = 0; index < 16; ++index)
      {
        Core.models.room.Slot slot = room._slots[index];
        if (slot.state >= SLOT_STATE.LOAD)
          slot.state = SLOT_STATE.NORMAL;
        slot.ResetSlot();
      }
      room.countdownSequence = -1;
      room.blockedClan = false;
      room.rodada = 1;
      room.spawnsCount = 0;
      room._redKills = 0;
      room._redDeaths = 0;
      room._blueKills = 0;
      room._blueDeaths = 0;
      room.red_dino = 0;
      room.blue_dino = 0;
      room.red_rounds = 0;
      room.blue_rounds = 0;
      room.BattleStart = new DateTime();
      room._timeRoom = 0U;
      room.Bar1 = 0;
      room.Bar2 = 0;
      room.swapRound = false;
      room.IngameAiLevel = (byte) 0;
      room._state = RoomState.Ready;
      room.updateRoomInfo();
      room.votekick = (VoteKick) null;
      room.UDPServer = (BattleServer) null;
      if (room.round.Timer != null)
        room.round.Timer = (Timer) null;
      if (room.vote.Timer != null)
        room.vote.Timer = (Timer) null;
      if (room.bomba.Timer != null)
        room.bomba.Timer = (Timer) null;
      room.updateSlotsInfo();
    }

    public static List<int> getDinossaurs(Room room, bool forceNewTRex, int forceRexIdx)
    {
      List<int> intList = new List<int>();
      if (room.room_type == (byte) 7 || room.room_type == (byte) 12)
      {
        int index = room.rodada == 1 ? 0 : 1;
        foreach (int team in room.GetTeamArray(index))
        {
          Core.models.room.Slot slot = room._slots[team];
          if (slot.state == SLOT_STATE.BATTLE && !slot.specGM)
            intList.Add(team);
        }
        if (((room.TRex == -1 ? 1 : (room._slots[room.TRex].state <= SLOT_STATE.BATTLE_READY ? 1 : 0)) | (forceNewTRex ? 1 : 0)) != 0 && intList.Count > 1 && room.room_type == (byte) 7)
        {
          Logger.warning("[" + DateTime.Now.ToString("HH:mm") + "] trex: " + room.TRex.ToString());
          if (forceRexIdx >= 0 && intList.Contains(forceRexIdx))
            room.TRex = forceRexIdx;
          else if (forceRexIdx == -2)
            room.TRex = intList[new Random().Next(0, intList.Count)];
          Logger.warning("[" + DateTime.Now.ToString("HH:mm") + "] forceRexIdx: " + forceRexIdx.ToString() + "; force: " + forceNewTRex.ToString() + "; teamIdx: " + index.ToString() + "; trex: " + room.TRex.ToString());
        }
      }
      return intList;
    }

    public static void BattleEndPlayersCount(Room room, bool isBotMode)
    {
      if (room == null | isBotMode || !room.isPreparing())
        return;
      int num1 = 0;
      int num2 = 0;
      int num3 = 0;
      int num4 = 0;
      for (int index = 0; index < 16; ++index)
      {
        Core.models.room.Slot slot = room._slots[index];
        if (slot.state == SLOT_STATE.BATTLE)
        {
          if (slot._team == 0)
            ++num2;
          else
            ++num1;
        }
        else if (slot.state >= SLOT_STATE.LOAD)
        {
          if (slot._team == 0)
            ++num4;
          else
            ++num3;
        }
      }
      if ((num2 != 0 && num1 != 0 || room._state != RoomState.Battle) && (num4 != 0 && num3 != 0 || room._state > RoomState.PreBattle))
        return;
      AllUtils.EndBattle(room, isBotMode);
    }

    public static void EndBattle(Room room) => AllUtils.EndBattle(room, room.isBotMode());

    public static void EndBattle(Room room, bool isBotMode)
    {
      TeamResultType winnerTeam = AllUtils.GetWinnerTeam(room);
      AllUtils.EndBattle(room, isBotMode, winnerTeam);
    }

    public static void EndBattleNoPoints(Room room)
    {
      List<Account> allPlayers = room.getAllPlayers(SLOT_STATE.READY, 1);
      if (allPlayers.Count != 0)
      {
        ushort result1;
        ushort result2;
        byte[] data;
        AllUtils.getBattleResult(room, out result1, out result2, out data);
        bool isBotMode = room.isBotMode();
        foreach (Account p in allPlayers)
          p.SendPacket((SendPacket) new BATTLE_ENDBATTLE_PAK(p, TeamResultType.TeamDraw, result2, result1, isBotMode, data));
      }
      AllUtils.resetBattleInfo(room);
    }

    public static void EndBattle(Room room, bool isBotMode, TeamResultType winnerTeam)
    {
      List<Account> allPlayers = room.getAllPlayers(SLOT_STATE.READY, 1);
      if (allPlayers.Count != 0)
      {
        room.CalculateResult(winnerTeam, isBotMode);
        ushort result1;
        ushort result2;
        byte[] data;
        AllUtils.getBattleResult(room, out result1, out result2, out data);
        foreach (Account p in allPlayers)
          p.SendPacket((SendPacket) new BATTLE_ENDBATTLE_PAK(p, winnerTeam, result2, result1, isBotMode, data));
      }
      AllUtils.resetBattleInfo(room);
    }

    public static int percentage(int total, int percent) => total * percent / 100;

    public static void BattleEndRound(Room room, int winner, bool forceRestart)
    {
      int roundsByMask = room.getRoundsByMask();
      if (room.red_rounds >= roundsByMask || room.blue_rounds >= roundsByMask)
      {
        room.StopBomb();
        using (BATTLE_ROUND_WINNER_PAK battleRoundWinnerPak = new BATTLE_ROUND_WINNER_PAK(room, winner, RoundEndType.AllDeath))
          room.SendPacketToPlayers((SendPacket) battleRoundWinnerPak, SLOT_STATE.BATTLE, 0);
        AllUtils.EndBattle(room, room.isBotMode(), (TeamResultType) winner);
      }
      else
      {
        if (!(!room.C4_actived | forceRestart))
          return;
        room.StopBomb();
        ++room.rodada;
        Game_SyncNet.SendUDPRoundSync(room);
        using (BATTLE_ROUND_WINNER_PAK battleRoundWinnerPak = new BATTLE_ROUND_WINNER_PAK(room, winner, RoundEndType.AllDeath))
          room.SendPacketToPlayers((SendPacket) battleRoundWinnerPak, SLOT_STATE.BATTLE, 0);
        room.RoundRestart();
      }
    }

    public static void BattleEndRound(Room room, int winner, RoundEndType motive)
    {
      using (BATTLE_ROUND_WINNER_PAK battleRoundWinnerPak = new BATTLE_ROUND_WINNER_PAK(room, winner, motive))
        room.SendPacketToPlayers((SendPacket) battleRoundWinnerPak, SLOT_STATE.BATTLE, 0);
      room.StopBomb();
      int roundsByMask = room.getRoundsByMask();
      if (room.red_rounds >= roundsByMask || room.blue_rounds >= roundsByMask)
      {
        AllUtils.EndBattle(room, room.isBotMode(), (TeamResultType) winner);
      }
      else
      {
        ++room.rodada;
        Game_SyncNet.SendUDPRoundSync(room);
        room.RoundRestart();
      }
    }

    public static int AddFriend(Account owner, Account friend, int state)
    {
      if (owner != null)
      {
        if (friend != null)
        {
          try
          {
            Friend friend1 = owner.FriendSystem.GetFriend(friend.player_id);
            if (friend1 == null)
            {
              using (NpgsqlConnection npgsqlConnection = SQLjec.getInstance().conn())
              {
                NpgsqlCommand command = npgsqlConnection.CreateCommand();
                npgsqlConnection.Open();
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@friend", (object) friend.player_id);
                command.Parameters.AddWithValue("@owner", (object) owner.player_id);
                command.Parameters.AddWithValue("@state", (object) state);
                command.CommandText = "INSERT INTO friends(friend_id,owner_id,state)VALUES(@friend,@owner,@state)";
                command.ExecuteNonQuery();
                command.Dispose();
                npgsqlConnection.Dispose();
                npgsqlConnection.Close();
              }
              lock (owner.FriendSystem._friends)
              {
                Friend friend2 = new Friend(friend.player_id, friend._rank, friend.player_name, friend._isOnline, friend._status)
                {
                  state = state,
                  removed = false
                };
                owner.FriendSystem._friends.Add(friend2);
                SEND_FRIENDS_INFOS.Load(owner, friend2, 0);
              }
              return 0;
            }
            if (friend1.removed)
            {
              friend1.removed = false;
              PlayerManager.UpdateFriendBlock(owner.player_id, friend1);
              SEND_FRIENDS_INFOS.Load(owner, friend1, 1);
            }
            else
              Logger.warning("Other.");
            return 1;
          }
          catch (Exception ex)
          {
            Logger.info("[AllUtils.AddFriend] " + ex.ToString());
            return -1;
          }
        }
      }
      return -1;
    }

    public static void syncPlayerToFriends(Account p, bool all)
    {
      if (p == null || p.FriendSystem._friends.Count == 0)
        return;
      PlayerInfo playerInfo = new PlayerInfo(p.player_id, p._rank, p.player_name, p._isOnline, p._status);
      for (int index1 = 0; index1 < p.FriendSystem._friends.Count; ++index1)
      {
        Friend friend1 = p.FriendSystem._friends[index1];
        if (all || friend1.state == 0 && !friend1.removed)
        {
          Account account = AccountManager.getAccount(friend1.player_id, 32);
          if (account != null)
          {
            int index2 = -1;
            Friend friend2 = account.FriendSystem.GetFriend(p.player_id, out index2);
            if (friend2 != null)
            {
              friend2.player = playerInfo;
              account.SendPacket((SendPacket) new FRIEND_UPDATE_PAK(FriendChangeState.Update, friend2, index2), false);
            }
          }
        }
      }
    }

    public static void syncPlayerToClanMembers(Account player)
    {
      if (player == null || player.clanId <= 0)
        return;
      using (CLAN_MEMBER_INFO_CHANGE_PAK memberInfoChangePak = new CLAN_MEMBER_INFO_CHANGE_PAK(player))
        ClanManager.SendPacket((SendPacket) memberInfoChangePak, player.clanId, player.player_id, true, true);
    }

    public static void updateSlotEquips(Account p)
    {
      Room room = p._room;
      if (room == null)
        return;
      AllUtils.updateSlotEquips(p, room);
    }

    public static void updateSlotEquips(Account p, Room room)
    {
      Core.models.room.Slot slot;
      if (!room.getSlot(p._slotId, out slot))
        return;
      slot._equip = p._equip;
    }

    public static ushort getSlotsFlag(Room room, bool onlyNoSpectators, bool missionSuccess)
    {
      if (room == null)
        return 0;
      int num = 0;
      for (int index = 0; index < 16; ++index)
      {
        Core.models.room.Slot slot = room._slots[index];
        if (slot.state >= SLOT_STATE.LOAD && (missionSuccess && slot.MissionsCompleted || !missionSuccess && (!onlyNoSpectators || !slot.espectador)))
          num += slot._flag;
      }
      return (ushort) num;
    }

    public static void getBattleResult(
      Room room,
      out ushort result1,
      out ushort result2,
      out byte[] data)
    {
      result1 = (ushort) 0;
      result2 = (ushort) 0;
      data = new byte[144];
      if (room == null)
        return;
      using (SendGPacket sendGpacket = new SendGPacket())
      {
        for (int index = 0; index < 16; ++index)
        {
          Core.models.room.Slot slot = room._slots[index];
          if (slot.state >= SLOT_STATE.LOAD)
          {
            ushort flag = (ushort) slot._flag;
            if (slot.MissionsCompleted)
              result1 += flag;
            result2 += flag;
          }
          sendGpacket.writeH(index * 2, (ushort) slot.exp);
          sendGpacket.writeH(32 + index * 2, (ushort) slot.gp);
          sendGpacket.writeH(64 + index * 2, (ushort) slot.BonusXP);
          sendGpacket.writeH(96 + index * 2, (ushort) slot.BonusGP);
          sendGpacket.writeC(128 + index, (byte) slot.bonusFlags);
        }
        data = sendGpacket.mstream.ToArray();
      }
    }

    public static void DiscountPlayerItems(Core.models.room.Slot slot, Account p)
    {
      uint num1 = uint.Parse(DateTime.Now.ToString("yyMMddHHmm"));
      bool flag1 = false;
      bool flag2 = false;
      List<ItemsModel> items = new List<ItemsModel>();
      List<object> objectList = new List<object>();
      PlayerBonus bonus = p._bonus;
      int num2 = bonus != null ? bonus.bonuses : 0;
      int num3 = bonus != null ? bonus.freepass : 0;
      lock (p._inventory._items)
      {
        for (int index = 0; index < p._inventory._items.Count; ++index)
        {
          ItemsModel itemsModel = p._inventory._items[index];
          if (itemsModel._equip == 1 && slot.armas_usadas.Contains(itemsModel._id) && !slot.specGM)
          {
            if (--itemsModel._count < 1U)
            {
              objectList.Add((object) itemsModel._objId);
              p._inventory._items.RemoveAt(index--);
            }
            else
            {
              items.Add(itemsModel);
              ComDiv.updateDB("player_items", "count", (object) (long) itemsModel._count, "object_id", (object) itemsModel._objId, "owner_id", (object) p.player_id);
            }
          }
          else if (itemsModel._count <= num1 & itemsModel._equip == 2)
          {
            if (itemsModel._category == 3 && ComDiv.getIdStatics(itemsModel._id, 1) == 12)
            {
              if (bonus != null)
              {
                if (!bonus.RemoveBonuses(itemsModel._id))
                {
                  if (itemsModel._id == 1200014000)
                  {
                    ComDiv.updateDB("player_bonus", "sightcolor", (object) 4, "player_id", (object) p.player_id);
                    bonus.sightColor = 4;
                    flag1 = true;
                  }
                  else if (itemsModel._id == 1200009000)
                  {
                    ComDiv.updateDB("player_bonus", "fakerank", (object) 55, "player_id", (object) p.player_id);
                    bonus.fakeRank = 55;
                    flag1 = true;
                  }
                }
                else if (itemsModel._id == 1200006000)
                {
                  ComDiv.updateDB("accounts", "name_color", (object) 0, "player_id", (object) p.player_id);
                  p.name_color = 0;
                  if (p._room != null)
                  {
                    using (ROOM_GET_NICKNAME_PAK roomGetNicknamePak = new ROOM_GET_NICKNAME_PAK(slot._id, p.player_name, p.name_color))
                      p._room.SendPacketToPlayers((SendPacket) roomGetNicknamePak);
                  }
                }
                CupomFlag cupomEffect = CupomEffectManager.getCupomEffect(itemsModel._id);
                if (cupomEffect != null && cupomEffect.EffectFlag > (CupomEffects) 0 && p.effects.HasFlag((Enum) cupomEffect.EffectFlag))
                {
                  p.effects -= cupomEffect.EffectFlag;
                  flag2 = true;
                }
              }
              else
                continue;
            }
            objectList.Add((object) itemsModel._objId);
            p._inventory._items.RemoveAt(index--);
          }
        }
      }
      if (bonus != null && (bonus.bonuses != num2 || bonus.freepass != num3))
        PlayerManager.updatePlayerBonus(p.player_id, bonus.bonuses, bonus.freepass);
      if (p.effects < (CupomEffects) 0)
        p.effects = (CupomEffects) 0;
      if (flag2)
        PlayerManager.updateCupomEffects(p.player_id, p.effects);
      if (items.Count > 0)
        p.SendPacket((SendPacket) new INVENTORY_ITEM_CREATE_PAK(2, p, items));
      for (int index = 0; index < objectList.Count; ++index)
      {
        long objId = (long) objectList[index];
        p.SendPacket((SendPacket) new INVENTORY_ITEM_EXCLUDE_PAK(1U, objId));
      }
      ComDiv.deleteDB("player_items", "object_id", objectList.ToArray(), "owner_id", (object) p.player_id);
      if (flag1)
        p.SendPacket((SendPacket) new BASE_USER_EFFECTS_PAK(0, p._bonus));
      if (p._equip == null)
        Logger.warning("É NULO essa poha.");
      int type = PlayerManager.CheckEquipedItems(p._equip, p._inventory._items);
      if (type <= 0)
        return;
      p.SendPacket((SendPacket) new INVENTORY_EQUIPED_ITEMS_PAK(p, type));
      slot._equip = p._equip;
    }

    public static int getNewSlotId(int slotIdx) => slotIdx % 2 != 0 ? slotIdx - 1 : slotIdx + 1;

    public static void GetXmasReward(Account p)
    {
      EventXmasModel runningEvent = EventXmasSyncer.GetRunningEvent();
      if (runningEvent == null)
        return;
      PlayerEvent playerEvent = p._event;
      uint num = uint.Parse(DateTime.Now.ToString("yyMMddHHmm"));
      if (playerEvent == null || playerEvent.LastXmasRewardDate > runningEvent.startDate && playerEvent.LastXmasRewardDate <= runningEvent.endDate || !ComDiv.updateDB("player_events", "last_xmas_reward_date", (object) (long) num, "player_id", (object) p.player_id))
        return;
      playerEvent.LastXmasRewardDate = num;
      p.SendPacket((SendPacket) new INVENTORY_ITEM_CREATE_PAK(1, p, new ItemsModel(702001024, 1, "Alcaçuz (Evento)", 1, 100U)));
    }

    public static void BattleEndRoundPlayersCount(Room room)
    {
      if (room.round.Timer != null || room.room_type != (byte) 2 && room.room_type != (byte) 4)
        return;
      int RedPlayers;
      int BluePlayers;
      int RedDeaths;
      int BlueDeaths;
      room.getPlayingPlayers(true, out RedPlayers, out BluePlayers, out RedDeaths, out BlueDeaths);
      if (RedDeaths == RedPlayers)
      {
        if (!room.C4_actived)
          ++room.blue_rounds;
        AllUtils.BattleEndRound(room, 1, false);
      }
      else
      {
        if (BlueDeaths != BluePlayers)
          return;
        ++room.red_rounds;
        AllUtils.BattleEndRound(room, 0, true);
      }
    }

    public static void BattleEndKills(Room room) => AllUtils.BaseEndByKills(room, room.isBotMode());

    public static void BattleEndKills(Room room, bool isBotMode) => AllUtils.BaseEndByKills(room, isBotMode);

    private static void BaseEndByKills(Room room, bool isBotMode)
    {
      int killsByMask = room.getKillsByMask();
      if (room._redKills < killsByMask && room._blueKills < killsByMask)
        return;
      List<Account> allPlayers = room.getAllPlayers(SLOT_STATE.READY, 1);
      if (allPlayers.Count != 0)
      {
        TeamResultType winnerTeam = AllUtils.GetWinnerTeam(room);
        room.CalculateResult(winnerTeam, isBotMode);
        ushort result1;
        ushort result2;
        byte[] data;
        AllUtils.getBattleResult(room, out result1, out result2, out data);
        using (BATTLE_ROUND_WINNER_PAK battleRoundWinnerPak = new BATTLE_ROUND_WINNER_PAK(room, winnerTeam, RoundEndType.TimeOut))
        {
          byte[] completeBytes = battleRoundWinnerPak.GetCompleteBytes("AllUtils.BaseEndByKills");
          foreach (Account p in allPlayers)
          {
            Core.models.room.Slot slot = room.getSlot(p._slotId);
            if (slot != null)
            {
              if (slot.state == SLOT_STATE.BATTLE)
                p.SendCompletePacket(completeBytes);
              p.SendPacket((SendPacket) new BATTLE_ENDBATTLE_PAK(p, winnerTeam, result2, result1, isBotMode, data));
            }
          }
        }
      }
      AllUtils.resetBattleInfo(room);
    }

    public static bool CheckClanMatchRestrict(Room room)
    {
      if (room._channelType == 4)
      {
        foreach (ClanModel clanModel in (IEnumerable<ClanModel>) AllUtils.GetClanListMatchPlayers(room).Values)
        {
          if (clanModel.RedPlayers >= 1 && clanModel.BluePlayers >= 1)
          {
            room.blockedClan = true;
            Logger.warning("XP cancelado em clãfronto [Room: " + room._roomId.ToString() + "; Canal: " + room._channelId.ToString() + "; Clã: " + clanModel.clanId.ToString() + "]");
            return true;
          }
        }
      }
      return false;
    }

    public static bool Have2ClansToClanMatch(Room room) => AllUtils.GetClanListMatchPlayers(room).Count == 2;

    public static bool HavePlayersToClanMatch(Room room)
    {
      SortedList<int, ClanModel> listMatchPlayers = AllUtils.GetClanListMatchPlayers(room);
      bool flag1 = false;
      bool flag2 = false;
      foreach (ClanModel clanModel in (IEnumerable<ClanModel>) listMatchPlayers.Values)
      {
        if (clanModel.RedPlayers >= 4)
          flag1 = true;
        else if (clanModel.BluePlayers >= 4)
          flag2 = true;
      }
      return flag1 & flag2;
    }

    private static SortedList<int, ClanModel> GetClanListMatchPlayers(Room room)
    {
      SortedList<int, ClanModel> sortedList = new SortedList<int, ClanModel>();
      foreach (Account allPlayer in room.getAllPlayers())
      {
        if (allPlayer.clanId != 0)
        {
          ClanModel clanModel;
          if (sortedList.TryGetValue(allPlayer.clanId, out clanModel) && clanModel != null)
          {
            if (allPlayer._slotId % 2 == 0)
              ++clanModel.RedPlayers;
            else
              ++clanModel.BluePlayers;
          }
          else
          {
            clanModel = new ClanModel();
            clanModel.clanId = allPlayer.clanId;
            if (allPlayer._slotId % 2 == 0)
              ++clanModel.RedPlayers;
            else
              ++clanModel.BluePlayers;
            sortedList.Add(allPlayer.clanId, clanModel);
          }
        }
      }
      return sortedList;
    }

    public static void playTimeEvent(
      long playedTime,
      Account p,
      PlayTimeModel ptEvent,
      bool isBotMode)
    {
      Room room = p._room;
      PlayerEvent pE = p._event;
      if (room == null | isBotMode || pE == null)
        return;
      long lastPlaytimeValue = pE.LastPlaytimeValue;
      long lastPlaytimeFinish = (long) pE.LastPlaytimeFinish;
      long lastPlaytimeDate = (long) pE.LastPlaytimeDate;
      if (pE.LastPlaytimeDate < ptEvent._startDate)
      {
        pE.LastPlaytimeFinish = 0;
        pE.LastPlaytimeValue = 0L;
      }
      if (pE.LastPlaytimeFinish == 0)
      {
        pE.LastPlaytimeValue += playedTime;
        if (pE.LastPlaytimeValue >= ptEvent._time)
          pE.LastPlaytimeFinish = 1;
        pE.LastPlaytimeDate = uint.Parse(DateTime.Now.ToString("yyMMddHHmm"));
        if (pE.LastPlaytimeValue >= ptEvent._time)
          p.SendPacket((SendPacket) new BATTLE_PLAYED_TIME_PAK(0, ptEvent));
        else
          p.SendPacket((SendPacket) new BATTLE_PLAYED_TIME_PAK(1, new PlayTimeModel()
          {
            _time = ptEvent._time - pE.LastPlaytimeValue
          }));
      }
      else if (pE.LastPlaytimeFinish == 1)
        p.SendPacket((SendPacket) new BATTLE_PLAYED_TIME_PAK(0, ptEvent));
      if (pE.LastPlaytimeValue == lastPlaytimeValue && (long) pE.LastPlaytimeFinish == lastPlaytimeFinish && (long) pE.LastPlaytimeDate == lastPlaytimeDate)
        return;
      EventPlayTimeSyncer.ResetPlayerEvent(p.player_id, pE);
    }

    public static void LogRoomBattleStart(Room room)
    {
      uint startDate = room.StartDate;
      uint uniqueRoomId = room.UniqueRoomId;
      StringUtil stringUtil = new StringUtil();
      stringUtil.AppendLine("[Room] Tech id: " + uniqueRoomId.ToString());
      stringUtil.AppendLine("[Room] Room id: " + string.Format("{0:0##}", (object) (room._roomId + 1)));
      stringUtil.AppendLine("[Room] Channel id: " + string.Format("{0:0##}", (object) (room._channelId + 1)));
      stringUtil.AppendLine("[Room] Room name: '" + room.name + "'");
      stringUtil.AppendLine("[Room] Room type: " + ((RoomType) room.room_type).ToString());
      stringUtil.AppendLine("[Room] Room special: " + ((RoomSpecial) room.special).ToString());
      stringUtil.AppendLine("[Room] Room weapons: " + ((RoomWeaponsFlag) room.weaponsFlag).ToString());
      stringUtil.AppendLine("[Room] Map name: '" + room._mapName + "'");
      stringUtil.AppendLine("[Room] Actual players count: " + room.getPlayingPlayers(2, true).ToString());
      stringUtil.AppendLine("[Room] Started battle.");
      Logger.LogRoom(stringUtil.getString(), startDate, uniqueRoomId);
    }

    public static void LogVotekickStart(Room room, Account p, Core.models.room.Slot slot)
    {
      VoteKick votekick = room.votekick;
      if (votekick == null)
        return;
      uint startDate = room.StartDate;
      uint uniqueRoomId = room.UniqueRoomId;
      Logger.LogRoom("[Room] Votekick has been started by " + slot._id.ToString() + " (Id: " + p.player_id.ToString() + "; Nick: " + p.player_name + "), (To: " + votekick.victimIdx.ToString() + "; Motive: " + ((VoteKickMotive) votekick.motive).ToString() + ")", startDate, uniqueRoomId);
    }

    public static void LogVotekickResult(Room room)
    {
      VoteKick votekick = room.votekick;
      if (votekick == null)
        return;
      uint startDate = room.StartDate;
      uint uniqueRoomId = room.UniqueRoomId;
      Logger.LogRoom("[Room] Votekick result: (Allies: " + votekick.allies.ToString() + "; Enemys: " + votekick.enemys.ToString() + "); Total: (" + votekick.kikar.ToString() + "/" + votekick.deixar.ToString() + ")", startDate, uniqueRoomId);
    }

    public static void LogRoomRoundRestart(Room room)
    {
      uint startDate = room.StartDate;
      uint uniqueRoomId = room.UniqueRoomId;
      Logger.LogRoom("[Room] Round " + room.rodada.ToString() + " has been started.", startDate, uniqueRoomId);
    }

    public static void LogRoomResult(Room room)
    {
      uint startDate = room.StartDate;
      uint uniqueRoomId = room.UniqueRoomId;
      StringUtil stringUtil = new StringUtil();
      stringUtil.AppendLine("[Room] ;------------------;");
      stringUtil.AppendLine("[Room] Battle is finished.");
      stringUtil.AppendLine("[Room] Tech id: " + uniqueRoomId.ToString());
      stringUtil.AppendLine("[Room] Used spawn count: " + room.spawnsCount.ToString());
      stringUtil.AppendLine("[Room] End Players count: " + room.getPlayingPlayers(2, true).ToString());
      stringUtil.AppendLine("[Room] ;------------------;");
      stringUtil.AppendLine("[Room] Room result info.");
      for (int index = 0; index < 16; ++index)
      {
        Core.models.room.Slot slot = room._slots[index];
        if (slot.state == SLOT_STATE.BATTLE && slot._playerId > 0L)
          stringUtil.AppendLine("[Room] Player (Id: " + slot._playerId.ToString() + "; Slot: " + index.ToString() + ") Exp: " + slot.exp.ToString() + "; Gp: " + slot.gp.ToString() + "; KD: " + slot.allKills.ToString() + "/" + slot.allDeaths.ToString() + "; Score: " + slot.Score.ToString());
      }
      Logger.LogRoom(stringUtil.getString(), startDate, uniqueRoomId);
    }
  }
}
