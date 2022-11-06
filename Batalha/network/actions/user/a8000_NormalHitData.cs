
// Type: Battle.network.actions.user.a8000_NormalHitData
// Assembly: pbserver_battle, Version=1.0.0.2646, Culture=neutral, PublicKeyToken=null
// MVID: AA2D7911-B687-4238-AB73-D37E54123039
// Interprise: C:\Users\Cuzin\3,50pbserver_battle.exe

using Battle.data;
using Battle.data.enums;
using Battle.data.enums.weapon;
using SharpDX;
using System.Collections.Generic;

namespace Battle.network.actions.user
{
  public class a8000_NormalHitData
  {
    public static List<a8000_NormalHitData.HitData> ReadInfo(
      ReceivePacket p,
      bool genLog,
      bool OnlyBytes = false)
    {
      return a8000_NormalHitData.BaseReadInfo(p, OnlyBytes, genLog);
    }

    public static void ReadInfo(ReceivePacket p)
    {
      int num = (int) p.readC();
      p.Advance(33 * num);
    }

    private static List<a8000_NormalHitData.HitData> BaseReadInfo(
      ReceivePacket p,
      bool OnlyBytes,
      bool genLog)
    {
      List<a8000_NormalHitData.HitData> hitDataList = new List<a8000_NormalHitData.HitData>();
      int num1 = (int) p.readC();
      for (int index1 = 0; index1 < num1; ++index1)
      {
        a8000_NormalHitData.HitData hitData = new a8000_NormalHitData.HitData()
        {
          _hitInfo = p.readUD(),
          _boomInfo = p.readUH(),
          _weaponInfo = p.readUH(),
          _weaponSlot = p.readC(),
          StartBullet = p.readTVector(),
          EndBullet = p.readTVector()
        };
        if (!OnlyBytes)
        {
          hitData.HitEnum = (HitType) AllUtils.getHitHelmet(hitData._hitInfo);
          if (hitData._boomInfo > (ushort) 0)
          {
            hitData.BoomPlayers = new List<int>();
            for (int index2 = 0; index2 < 16; ++index2)
            {
              int num2 = 1 << index2;
              if (((int) hitData._boomInfo & num2) == num2)
                hitData.BoomPlayers.Add(index2);
            }
          }
          hitData.WeaponClass = (ClassType) ((int) hitData._weaponInfo & 63);
          hitData.WeaponId = (int) hitData._weaponInfo >> 6;
        }
        if (!genLog)
          ;
        hitDataList.Add(hitData);
      }
      return hitDataList;
    }

    public static void writeInfo(SendPacket s, ReceivePacket p, bool genLog)
    {
      List<a8000_NormalHitData.HitData> hits = a8000_NormalHitData.ReadInfo(p, genLog, true);
      a8000_NormalHitData.writeInfo(s, hits);
    }

    public static void writeInfo(SendPacket s, List<a8000_NormalHitData.HitData> hits)
    {
      s.writeC((byte) hits.Count);
      for (int index = 0; index < hits.Count; ++index)
      {
        a8000_NormalHitData.HitData hit = hits[index];
        s.writeD(hit._hitInfo);
        s.writeH(hit._boomInfo);
        s.writeH(hit._weaponInfo);
        s.writeC(hit._weaponSlot);
        s.writeTVector(hit.StartBullet);
        s.writeTVector(hit.EndBullet);
      }
    }

    public class HitData
    {
      public byte _weaponSlot;
      public ushort _boomInfo;
      public ushort _weaponInfo;
      public uint _hitInfo;
      public int WeaponId;
      public Half3 StartBullet;
      public Half3 EndBullet;
      public List<int> BoomPlayers;
      public HitType HitEnum;
      public ClassType WeaponClass;
    }
  }
}
