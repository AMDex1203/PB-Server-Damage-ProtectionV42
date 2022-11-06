
// Type: Battle.network.actions.damage.DamageManager
// Assembly: pbserver_battle, Version=1.0.0.2646, Culture=neutral, PublicKeyToken=null
// MVID: AA2D7911-B687-4238-AB73-D37E54123039
// Interprise: C:\Users\Cuzin\3,50pbserver_battle.exe

using Battle.data.enums;
using Battle.data.models;
using Battle.data.sync;
using Battle.data.xml;
using SharpDX;
using System;
using System.Collections.Generic;

namespace Battle.network.actions.damage
{
  public class DamageManager
  {
    public static void SabotageDestroy(
      Room room,
      Player pl,
      ObjModel objM,
      ObjectInfo obj,
      int damage)
    {
      if (objM._ultraSYNC <= 0 || room.stageType != 3 && room.stageType != 5)
        return;
      if (objM._ultraSYNC == 1 || objM._ultraSYNC == 3)
        room._bar1 = obj._life;
      else if (objM._ultraSYNC == 2 || objM._ultraSYNC == 4)
        room._bar2 = obj._life;
      Battle_SyncNet.SendSabotageSync(room, pl, damage, objM._ultraSYNC == 4 ? 2 : 1);
    }

    public static void SetDeath(List<DeathServerData> deaths, Player player, CHARA_DEATH deathType)
    {
      player._life = 0;
      player.isDead = true;
      player.LastDie = DateTime.Now;
      deaths.Add(new DeathServerData()
      {
        _player = player,
        _deathType = deathType
      });
    }

    public static void SetHitEffect(
      List<ObjectHitInfo> objs,
      Player player,
      Player killer,
      CHARA_DEATH deathType,
      int hitPart)
    {
      objs.Add(new ObjectHitInfo(5)
      {
        objId = player._slot,
        killerId = killer._slot,
        deathType = deathType,
        objLife = hitPart
      });
    }

    public static void SetHitEffect(
      List<ObjectHitInfo> objs,
      Player player,
      CHARA_DEATH deathType,
      int hitPart)
    {
      objs.Add(new ObjectHitInfo(5)
      {
        objId = player._slot,
        killerId = player._slot,
        deathType = deathType,
        objLife = hitPart
      });
    }

    public static void BoomDeath(
      Room room,
      Player pl,
      int weaponId,
      List<DeathServerData> deaths,
      List<ObjectHitInfo> objs,
      List<int> BoomPlayers)
    {
      if (BoomPlayers == null || BoomPlayers.Count == 0)
        return;
      for (int index = 0; index < BoomPlayers.Count; ++index)
      {
        int boomPlayer = BoomPlayers[index];
        Player p;
        if (room.getPlayer(boomPlayer, out p) && !p.isDead)
        {
          DamageManager.SetDeath(deaths, p, CHARA_DEATH.OBJECT_EXPLOSION);
          objs.Add(new ObjectHitInfo(2)
          {
            hitPart = 1,
            deathType = CHARA_DEATH.OBJECT_EXPLOSION,
            objId = boomPlayer,
            killerId = pl._slot,
            weaponId = weaponId
          });
        }
      }
    }

    public static void SimpleDeath(
      List<DeathServerData> deaths,
      List<ObjectHitInfo> objs,
      Player killer,
      Player victim,
      int damage,
      int weapon,
      int hitPart,
      CHARA_DEATH deathType)
    {
      victim._life -= damage;
      if (victim._life <= 0)
        DamageManager.SetDeath(deaths, victim, deathType);
      else
        DamageManager.SetHitEffect(objs, victim, killer, deathType, hitPart);
      objs.Add(new ObjectHitInfo(2)
      {
        objId = victim._slot,
        objLife = victim._life,
        hitPart = hitPart,
        killerId = killer._slot,
        Position = (Half3) ((Vector3) victim.Position - (Vector3) killer.Position),
        deathType = deathType,
        weaponId = weapon
      });
    }
  }
}
