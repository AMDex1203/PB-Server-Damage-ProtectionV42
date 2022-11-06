
// Type: Game.data.sync.client_side.Net_Room_Death
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.models.enums;
using Core.models.enums.flags;
using Core.models.enums.item;
using Core.models.enums.missions;
using Core.models.enums.room;
using Core.models.room;
using Core.server;
using Game.data.model;
using Game.data.utils;
using Game.data.xml;
using System;

namespace Game.data.sync.client_side
{
  public static class Net_Room_Death
  {
    public static void Load(ReceiveGPacket p)
    {
      int id1 = (int) p.readH();
      int id2 = (int) p.readH();
      byte num1 = p.readC();
      byte num2 = p.readC();
      int num3 = p.readD();
      float num4 = p.readT();
      float num5 = p.readT();
      float num6 = p.readT();
      byte num7 = p.readC();
      int num8 = (int) num7 * 15;
      if (p.getBuffer().Length > 25 + num8)
        Logger.warning("[Invalid DEATH: " + BitConverter.ToString(p.getBuffer()) + "]");
      Channel channel = ChannelsXML.getChannel(id2);
      if (channel == null)
        return;
      Room room = channel.getRoom(id1);
      if (room == null || room.round.Timer != null || room._state != RoomState.Battle)
        return;
      Slot slot1 = room.getSlot((int) num1);
      if (slot1 == null || slot1.state != SLOT_STATE.BATTLE)
        return;
      FragInfos kills = new FragInfos()
      {
        killerIdx = num1,
        killingType = CharaKillType.DEFAULT,
        weapon = num3,
        x = num4,
        y = num5,
        z = num6,
        flag = num2
      };
      bool isSuicide = false;
      for (int index = 0; index < (int) num7; ++index)
      {
        byte num9 = p.readC();
        byte hitspotInfo = p.readC();
        float num10 = p.readT();
        float num11 = p.readT();
        float num12 = p.readT();
        int num13 = (int) p.readC();
        int slotIdx = (int) hitspotInfo & 15;
        Slot slot2 = room.getSlot(slotIdx);
        if (slot2 != null && slot2.state == SLOT_STATE.BATTLE)
        {
          Frag frag = new Frag(hitspotInfo)
          {
            flag = (byte) num13,
            victimWeaponClass = num9,
            x = num10,
            y = num11,
            z = num12
          };
          if ((int) kills.killerIdx == slotIdx)
            isSuicide = true;
          kills.frags.Add(frag);
        }
      }
      kills.killsCount = (byte) kills.frags.Count;
      Game_SyncNet.genDeath(room, slot1, kills, isSuicide);
    }

    public static void RegistryFragInfos(
      Room room,
      Slot killer,
      out int score,
      bool isBotMode,
      bool isSuicide,
      FragInfos kills)
    {
      score = 0;
      ITEM_CLASS idStatics = (ITEM_CLASS) ComDiv.getIdStatics(kills.weapon, 1);
      for (int index = 0; index < kills.frags.Count; ++index)
      {
        Frag frag = kills.frags[index];
        CharaDeath charaDeath = (CharaDeath) ((int) frag.hitspotInfo >> 4);
        if ((int) kills.killsCount - (isSuicide ? 1 : 0) > 1)
        {
          frag.killFlag |= charaDeath == CharaDeath.BOOM || charaDeath == CharaDeath.OBJECT_EXPLOSION || (charaDeath == CharaDeath.POISON || charaDeath == CharaDeath.HOWL) || (charaDeath == CharaDeath.TRAMPLED || idStatics == ITEM_CLASS.SHOTGUN) ? KillingMessage.MassKill : KillingMessage.PiercingShot;
        }
        else
        {
          int num1 = 0;
          switch (charaDeath)
          {
            case CharaDeath.DEFAULT:
              if (idStatics == ITEM_CLASS.KNIFE)
              {
                num1 = 6;
                break;
              }
              break;
            case CharaDeath.HEADSHOT:
              num1 = 4;
              break;
          }
          if (num1 > 0)
          {
            int num2 = killer.lastKillState >> 12;
            switch (num1)
            {
              case 4:
                if (num2 != 4)
                  killer.repeatLastState = false;
                killer.lastKillState = num1 << 12 | killer.killsOnLife + 1;
                if (killer.repeatLastState)
                {
                  frag.killFlag |= (killer.lastKillState & 16383) <= 1 ? KillingMessage.Headshot : KillingMessage.ChainHeadshot;
                  break;
                }
                frag.killFlag |= KillingMessage.Headshot;
                killer.repeatLastState = true;
                break;
              case 6:
                if (num2 != 6)
                  killer.repeatLastState = false;
                killer.lastKillState = num1 << 12 | killer.killsOnLife + 1;
                if (killer.repeatLastState && (killer.lastKillState & 16383) > 1)
                {
                  frag.killFlag |= KillingMessage.ChainSlugger;
                  break;
                }
                killer.repeatLastState = true;
                break;
            }
          }
          else
          {
            killer.lastKillState = 0;
            killer.repeatLastState = false;
          }
        }
        int victimSlot = frag.VictimSlot;
        Slot slot = room._slots[victimSlot];
        if (slot.killsOnLife > 3)
          frag.killFlag |= KillingMessage.ChainStopper;
        if ((kills.weapon != 19016 && kills.weapon != 19022 || (int) kills.killerIdx != victimSlot) && !slot.specGM)
          ++slot.allDeaths;
        if (killer._team != slot._team)
        {
          score += AllUtils.getKillScore(frag.killFlag);
          ++killer.allKills;
          if (killer._deathState == DeadEnum.isAlive)
            ++killer.killsOnLife;
          if (slot._team == 0)
          {
            ++room._redDeaths;
            ++room._blueKills;
          }
          else
          {
            ++room._blueDeaths;
            ++room._redKills;
          }
          if (room.room_type == (byte) 7)
          {
            if (killer._team == 0)
              room.red_dino += 4;
            else
              room.blue_dino += 4;
          }
        }
        slot.lastKillState = 0;
        slot.killsOnLife = 0;
        slot.repeatLastState = false;
        slot.passSequence = 0;
        slot._deathState = DeadEnum.isDead;
        if (!isBotMode)
          AllUtils.CompleteMission(room, slot, MISSION_TYPE.DEATH, 0);
        if (charaDeath == CharaDeath.HEADSHOT)
          ++killer.headshots;
      }
    }

    public static void EndBattleByDeath(Room room, Slot killer, bool isBotMode, bool isSuicide)
    {
      if ((room.room_type == (byte) 1 || room.room_type == (byte) 8 || room.room_type == (byte) 13) && !isBotMode)
      {
        AllUtils.BattleEndKills(room, isBotMode);
      }
      else
      {
        if (killer.specGM || room.room_type != (byte) 2 && room.room_type != (byte) 4)
          return;
        int winner1 = 0;
        int RedPlayers;
        int BluePlayers;
        int RedDeaths;
        int BlueDeaths;
        room.getPlayingPlayers(true, out RedPlayers, out BluePlayers, out RedDeaths, out BlueDeaths);
        if (((RedDeaths != RedPlayers ? 0 : (killer._team == 0 ? 1 : 0)) & (isSuicide ? 1 : 0)) != 0 && !room.C4_actived)
        {
          int winner2 = 1;
          ++room.blue_rounds;
          AllUtils.BattleEndRound(room, winner2, true);
        }
        else if (BlueDeaths == BluePlayers && killer._team == 1)
        {
          ++room.red_rounds;
          AllUtils.BattleEndRound(room, winner1, true);
        }
        else if (RedDeaths == RedPlayers && killer._team == 1)
        {
          if (!room.C4_actived)
          {
            winner1 = 1;
            ++room.blue_rounds;
          }
          else if (isSuicide)
            ++room.red_rounds;
          AllUtils.BattleEndRound(room, winner1, false);
        }
        else
        {
          if (BlueDeaths != BluePlayers || killer._team != 0)
            return;
          if (!isSuicide || !room.C4_actived)
          {
            ++room.red_rounds;
          }
          else
          {
            winner1 = 1;
            ++room.blue_rounds;
          }
          AllUtils.BattleEndRound(room, winner1, true);
        }
      }
    }
  }
}
