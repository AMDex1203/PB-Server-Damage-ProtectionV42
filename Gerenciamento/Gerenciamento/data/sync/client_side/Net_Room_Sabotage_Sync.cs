
// Type: Game.data.sync.client_side.Net_Room_Sabotage_Sync
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.models.enums;
using Core.models.room;
using Core.server;
using Game.data.model;
using Game.data.utils;
using Game.data.xml;
using Game.global.serverpacket;
using System;

namespace Game.data.sync.client_side
{
  public class Net_Room_Sabotage_Sync
  {
    public static void Load(ReceiveGPacket p)
    {
      int id1 = (int) p.readH();
      int id2 = (int) p.readH();
      byte num1 = p.readC();
      ushort num2 = p.readUH();
      ushort num3 = p.readUH();
      int num4 = (int) p.readC();
      ushort num5 = p.readUH();
      if (p.getBuffer().Length > 14)
        Logger.warning("[Invalid SABOTAGE: " + BitConverter.ToString(p.getBuffer()) + "]");
      Channel channel = ChannelsXML.getChannel(id2);
      if (channel == null)
        return;
      Room room = channel.getRoom(id1);
      Slot slot;
      if (room == null || room.round.Timer != null || (room._state != RoomState.Battle || room.swapRound) || !room.getSlot((int) num1, out slot))
        return;
      room.Bar1 = (int) num2;
      room.Bar2 = (int) num3;
      RoomType roomType = (RoomType) room.room_type;
      int num6 = 0;
      switch (num4)
      {
        case 1:
          slot.damageBar1 += num5;
          num6 += (int) slot.damageBar1 / 600;
          break;
        case 2:
          slot.damageBar2 += num5;
          num6 += (int) slot.damageBar2 / 600;
          break;
      }
      slot.earnedXP = num6;
      switch (roomType)
      {
        case RoomType.Destroy:
          using (BATTLE_MISSION_GENERATOR_INFO_PAK generatorInfoPak = new BATTLE_MISSION_GENERATOR_INFO_PAK(room))
            room.SendPacketToPlayers((SendPacket) generatorInfoPak, SLOT_STATE.BATTLE, 0);
          if (room.Bar1 == 0)
          {
            Net_Room_Sabotage_Sync.EndRound(room, (byte) 1);
            break;
          }
          if (room.Bar2 != 0)
            break;
          Net_Room_Sabotage_Sync.EndRound(room, (byte) 0);
          break;
        case RoomType.Defense:
          using (BATTLE_MISSION_DEFENCE_INFO_PAK missionDefenceInfoPak = new BATTLE_MISSION_DEFENCE_INFO_PAK(room))
            room.SendPacketToPlayers((SendPacket) missionDefenceInfoPak, SLOT_STATE.BATTLE, 0);
          if (room.Bar1 != 0 || room.Bar2 != 0)
            break;
          Net_Room_Sabotage_Sync.EndRound(room, (byte) 0);
          break;
      }
    }

    public static void EndRound(Room room, byte winner)
    {
      room.swapRound = true;
      switch (winner)
      {
        case 0:
          ++room.red_rounds;
          break;
        case 1:
          ++room.blue_rounds;
          break;
      }
      AllUtils.BattleEndRound(room, (int) winner, RoundEndType.Normal);
    }
  }
}
