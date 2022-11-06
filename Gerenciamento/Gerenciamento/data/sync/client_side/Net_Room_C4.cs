
// Type: Game.data.sync.client_side.Net_Room_C4
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
  public static class Net_Room_C4
  {
    public static void Load(ReceiveGPacket p)
    {
      int id1 = (int) p.readH();
      int id2 = (int) p.readH();
      int num = (int) p.readC();
      int slotIdx = (int) p.readC();
      int areaId = 0;
      float x = 0.0f;
      float y = 0.0f;
      float z = 0.0f;
      switch (num)
      {
        case 0:
          areaId = (int) p.readC();
          x = p.readT();
          y = p.readT();
          z = p.readT();
          if (p.getBuffer().Length > 21)
          {
            Logger.warning("[Invalid BOMB0: " + BitConverter.ToString(p.getBuffer()) + "]");
            break;
          }
          break;
        case 1:
          if (p.getBuffer().Length > 8)
          {
            Logger.warning("[Invalid BOMB1: " + BitConverter.ToString(p.getBuffer()) + "]");
            break;
          }
          break;
      }
      Channel channel = ChannelsXML.getChannel(id2);
      if (channel == null)
        return;
      Room room = channel.getRoom(id1);
      if (room == null || room.round.Timer != null || room._state != RoomState.Battle)
        return;
      Slot slot = room.getSlot(slotIdx);
      if (slot == null || slot.state != SLOT_STATE.BATTLE)
        return;
      if (num == 0)
      {
        Net_Room_C4.InstallBomb(room, slot, areaId, x, y, z);
      }
      else
      {
        if (num != 1)
          return;
        Net_Room_C4.UninstallBomb(room, slot);
      }
    }

    public static void InstallBomb(Room room, Slot slot, int areaId, float x, float y, float z)
    {
      if (room.C4_actived)
        return;
      using (BATTLE_MISSION_BOMB_INSTALL_PAK missionBombInstallPak = new BATTLE_MISSION_BOMB_INSTALL_PAK(slot._id, (byte) areaId, x, y, z))
        room.SendPacketToPlayers((SendPacket) missionBombInstallPak, SLOT_STATE.BATTLE, 0);
      if (room.room_type == (byte) 10)
        return;
      room.C4_actived = true;
      ++slot.objetivos;
      AllUtils.CompleteMission(room, slot, MISSION_TYPE.C4_PLANT, 0);
      room.StartBomb(room.rodada);
    }

    public static void UninstallBomb(Room room, Slot slot)
    {
      if (!room.C4_actived)
        return;
      using (BATTLE_MISSION_BOMB_UNINSTALL_PAK bombUninstallPak = new BATTLE_MISSION_BOMB_UNINSTALL_PAK(slot._id))
        room.SendPacketToPlayers((SendPacket) bombUninstallPak, SLOT_STATE.BATTLE, 0);
      if (room.room_type == (byte) 10)
        return;
      ++slot.objetivos;
      ++room.blue_rounds;
      AllUtils.CompleteMission(room, slot, MISSION_TYPE.C4_DEFUSE, 0);
      AllUtils.BattleEndRound(room, 1, RoundEndType.Uninstall);
    }
  }
}
