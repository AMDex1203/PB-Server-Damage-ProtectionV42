
// Type: Battle.data.sync.client_side.RespawnSync
// Assembly: pbserver_battle, Version=1.0.0.2646, Culture=neutral, PublicKeyToken=null
// MVID: AA2D7911-B687-4238-AB73-D37E54123039
// Interprise: C:\Users\Cuzin\3,50pbserver_battle.exe

using Battle.config;
using Battle.data.models;
using Battle.data.xml;
using Battle.network;
using Battle.network.packets;
using System;
using System.Collections.Generic;

namespace Battle.data.sync.client_side
{
  public static class RespawnSync
  {
    public static void Load(ReceivePacket p)
    {
      uint UniqueRoomId = p.readUD();
      int gen2 = p.readD();
      long startTick = p.readQ();
      int num1 = (int) p.readC();
      int round = (int) p.readC();
      int slot = (int) p.readC();
      int num2 = (int) p.readC();
      byte num3 = p.readC();
      int type = 0;
      int charaId = 0;
      int percent = 0;
      bool flag = false;
      if (num1 == 0 || num1 == 2)
      {
        type = (int) p.readC();
        charaId = (int) p.readH();
        percent = (int) p.readC();
        flag = p.readC() == (byte) 1;
        if (28 < p.getBuffer().Length)
          Logger.warning("[ALTO]: " + BitConverter.ToString(p.getBuffer()));
      }
      else if (23 < p.getBuffer().Length)
        Logger.warning("[ALTO]: " + BitConverter.ToString(p.getBuffer()));
      Room room = RoomsManager.getRoom(UniqueRoomId);
      if (room == null)
        return;
      room.ResyncTick(startTick, gen2);
      Player player1 = room.getPlayer(slot, true);
      if (player1 != null && player1._playerIdByUser != (int) num3)
        Logger.warning("Invalid User Ids: [By user: " + player1._playerIdByUser.ToString() + "/ Server: " + num3.ToString() + "]");
      if (player1 == null || player1._playerIdByUser != (int) num3)
        return;
      player1._playerIdByServer = (int) num3;
      player1._respawnByServer = num2;
      player1.Integrity = false;
      if (round > room._serverRound)
        room._serverRound = round;
      if (num1 == 0 || num1 == 2)
      {
        ++player1._respawnByLogic;
        player1.isDead = false;
        player1._plantDuration = Config.plantDuration;
        player1._defuseDuration = Config.defuseDuration;
        if (flag)
        {
          player1._plantDuration -= AllUtils.percentage(Config.plantDuration, 50);
          player1._defuseDuration -= AllUtils.percentage(Config.defuseDuration, 25);
        }
        if (!room._isBotMode)
        {
          if (room._sourceToMap == -1)
            room.RoundResetRoomF1(round);
          else
            room.RoundResetRoomS1(round);
        }
        if (type == (int) byte.MaxValue)
        {
          player1.Immortal = true;
        }
        else
        {
          player1.Immortal = false;
          int lifeById = CharaXML.getLifeById(charaId, type);
          int num4 = lifeById + AllUtils.percentage(lifeById, percent);
          player1._maxLife = num4;
          player1.ResetLife();
        }
      }
      if (room._isBotMode || num1 == 2 || !room.ObjectsIsValid())
        return;
      List<ObjectHitInfo> objs = new List<ObjectHitInfo>();
      for (int index = 0; index < room._objects.Length; ++index)
      {
        ObjectInfo objectInfo = room._objects[index];
        ObjModel model = objectInfo._model;
        if (model != null && (num1 != 2 && model.isDestroyable && objectInfo._life != model._life || model._needSync))
          objs.Add(new ObjectHitInfo(3)
          {
            objSyncId = model._needSync ? 1 : 0,
            _animId1 = model._anim1,
            _animId2 = objectInfo._anim != null ? objectInfo._anim._id : (int) byte.MaxValue,
            _destroyState = objectInfo.DestroyState,
            objId = model._id,
            objLife = objectInfo._life,
            _specialUse = AllUtils.GetDuration(objectInfo._useDate)
          });
      }
      for (int index = 0; index < room._players.Length; ++index)
      {
        Player player2 = room._players[index];
        if (player2._slot != slot && player2.AccountIdIsValid() && !player2.Immortal && player2._date != new DateTime() && (player2._maxLife != player2._life || player2.isDead))
          objs.Add(new ObjectHitInfo(4)
          {
            objId = player2._slot,
            objLife = player2._life
          });
      }
      if (objs.Count > 0)
        BattleManager.Send(Packet4Creator.getCode4(Packet4Creator.getCode4SyncData(objs), room._startTime, round, (int) byte.MaxValue), player1._client);
    }
  }
}
