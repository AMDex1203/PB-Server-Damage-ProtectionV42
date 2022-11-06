
// Type: Game.data.sync.client_side.Net_Room_Pass_Portal
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.models.enums;
using Core.models.enums.missions;
using Core.models.room;
using Core.server;
using Game.data.model;
using Game.data.utils;
using Game.data.xml;
using Game.global.serverpacket;
using System;

namespace Game.data.sync.client_side
{
  public static class Net_Room_Pass_Portal
  {
    public static void Load(ReceiveGPacket p)
    {
      int id1 = (int) p.readH();
      int id2 = (int) p.readH();
      int slotIdx = (int) p.readC();
      int num = (int) p.readC();
      Channel channel = ChannelsXML.getChannel(id2);
      if (channel == null)
        return;
      Room room = channel.getRoom(id1);
      if (room != null && room.round.Timer == null && (room._state == RoomState.Battle && room.room_type == (byte) 7))
      {
        Slot slot = room.getSlot(slotIdx);
        if (slot != null && slot.state == SLOT_STATE.BATTLE)
        {
          ++slot.passSequence;
          if (slot._team == 0)
            room.red_dino += 5;
          else
            room.blue_dino += 5;
          Net_Room_Pass_Portal.CompleteMission(room, slot);
          using (BATTLE_MISSION_ESCAPE_PAK missionEscapePak = new BATTLE_MISSION_ESCAPE_PAK(room, slot))
          {
            using (BATTLE_DINO_PLACAR_PAK battleDinoPlacarPak = new BATTLE_DINO_PLACAR_PAK(room))
              room.SendPacketToPlayers((SendPacket) missionEscapePak, (SendPacket) battleDinoPlacarPak, SLOT_STATE.BATTLE, 0);
          }
        }
      }
      if (p.getBuffer().Length <= 8)
        return;
      Logger.warning("[Invalid PORTAL: " + BitConverter.ToString(p.getBuffer()) + "]");
    }

    private static void CompleteMission(Room room, Slot slot)
    {
      MISSION_TYPE autoComplete = MISSION_TYPE.NA;
      if (slot.passSequence == 1)
        autoComplete = MISSION_TYPE.TOUCHDOWN;
      else if (slot.passSequence == 2)
        autoComplete = MISSION_TYPE.TOUCHDOWN_ACE_ATTACKER;
      else if (slot.passSequence == 3)
        autoComplete = MISSION_TYPE.TOUCHDOWN_HATTRICK;
      else if (slot.passSequence >= 4)
        autoComplete = MISSION_TYPE.TOUCHDOWN_GAME_MAKER;
      if (autoComplete == MISSION_TYPE.NA)
        return;
      AllUtils.CompleteMission(room, slot, autoComplete, 0);
    }
  }
}
