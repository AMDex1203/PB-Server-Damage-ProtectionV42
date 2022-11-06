
// Type: Battle.network.actions.user.a10000_BoomHitData
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
  public class a10000_BoomHitData
  {
    public static List<a10000_BoomHitData.HitData> ReadInfo(
      ReceivePacket p,
      bool genLog,
      bool OnlyBytes = false)
    {
      return a10000_BoomHitData.BaseReadInfo(p, OnlyBytes, genLog);
    }

    public static void ReadInfo(ReceivePacket p)
    {
      int num = (int) p.readC();
      p.Advance(24 * num);
    }

    private static List<a10000_BoomHitData.HitData> BaseReadInfo(
      ReceivePacket p,
      bool OnlyBytes,
      bool genLog)
    {
      List<a10000_BoomHitData.HitData> hitDataList = new List<a10000_BoomHitData.HitData>();
      int num1 = (int) p.readC();
      for (int index1 = 0; index1 < num1; ++index1)
      {
        a10000_BoomHitData.HitData hitData = new a10000_BoomHitData.HitData()
        {
          _hitInfo = p.readUD(),
          _boomInfo = p.readUH(),
          _weaponInfo = p.readUH(),
          _weaponSlot = p.readC(),
          _deathType = p.readC(),
          FirePos = p.readUHVector(),
          HitPos = p.readUHVector(),
          _grenadesCount = p.readUH()
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
        if (genLog)
        {
          string[] strArray1 = new string[6];
          strArray1[0] = "[Player pos] X: ";
          Half half = hitData.FirePos.X;
          strArray1[1] = half.ToString();
          strArray1[2] = "; Y: ";
          half = hitData.FirePos.Y;
          strArray1[3] = half.ToString();
          strArray1[4] = "; Z: ";
          half = hitData.FirePos.Z;
          strArray1[5] = half.ToString();
          Logger.warning(string.Concat(strArray1));
          string[] strArray2 = new string[6];
          strArray2[0] = "[Object pos] X: ";
          half = hitData.HitPos.X;
          strArray2[1] = half.ToString();
          strArray2[2] = "; Y: ";
          half = hitData.HitPos.Y;
          strArray2[3] = half.ToString();
          strArray2[4] = "; Z: ";
          half = hitData.HitPos.Z;
          strArray2[5] = half.ToString();
          Logger.warning(string.Concat(strArray2));
        }
        hitDataList.Add(hitData);
      }
      return hitDataList;
    }

    public static void writeInfo(SendPacket s, ReceivePacket p, bool genLog)
    {
      List<a10000_BoomHitData.HitData> hits = a10000_BoomHitData.ReadInfo(p, genLog, true);
      a10000_BoomHitData.writeInfo(s, hits);
    }

    public static void writeInfo(SendPacket s, List<a10000_BoomHitData.HitData> hits)
    {
      s.writeC((byte) hits.Count);
      for (int index = 0; index < hits.Count; ++index)
      {
        a10000_BoomHitData.HitData hit = hits[index];
        s.writeD(hit._hitInfo);
        s.writeH(hit._boomInfo);
        s.writeH(hit._weaponInfo);
        s.writeC(hit._weaponSlot);
        s.writeC(hit._deathType);
        s.writeHVector(hit.FirePos);
        s.writeHVector(hit.HitPos);
        s.writeH(hit._grenadesCount);
      }
    }

    public class HitData
    {
      public byte _weaponSlot;
      public byte _deathType;
      public ushort _boomInfo;
      public ushort _grenadesCount;
      public ushort _weaponInfo;
      public uint _hitInfo;
      public int WeaponId;
      public List<int> BoomPlayers;
      public Half3 FirePos;
      public Half3 HitPos;
      public HitType HitEnum;
      public ClassType WeaponClass;
    }
  }
}
