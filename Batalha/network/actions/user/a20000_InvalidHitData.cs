
// Type: Battle.network.actions.user.a20000_InvalidHitData
// Assembly: pbserver_battle, Version=1.0.0.2646, Culture=neutral, PublicKeyToken=null
// MVID: AA2D7911-B687-4238-AB73-D37E54123039
// Interprise: C:\Users\Cuzin\3,50pbserver_battle.exe

using Battle.data;
using Battle.data.enums;
using SharpDX;
using System.Collections.Generic;

namespace Battle.network.actions.user
{
  public class a20000_InvalidHitData
  {
    public static List<a20000_InvalidHitData.HitData> ReadInfo(
      ReceivePacket p,
      bool genLog,
      bool OnlyBytes = false)
    {
      return a20000_InvalidHitData.BaseReadInfo(p, OnlyBytes, genLog);
    }

    public static void ReadInfo(ReceivePacket p)
    {
      int num = (int) p.readC();
      p.Advance(14 * num);
    }

    private static List<a20000_InvalidHitData.HitData> BaseReadInfo(
      ReceivePacket p,
      bool OnlyBytes,
      bool genLog)
    {
      List<a20000_InvalidHitData.HitData> hitDataList = new List<a20000_InvalidHitData.HitData>();
      int num = (int) p.readC();
      for (int index = 0; index < num; ++index)
      {
        a20000_InvalidHitData.HitData hitData = new a20000_InvalidHitData.HitData()
        {
          _hitInfo = p.readUH(),
          FirePos = p.readUHVector(),
          HitPos = p.readUHVector()
        };
        if (!OnlyBytes)
          hitData.HitEnum = (HitType) AllUtils.getHitHelmet((uint) hitData._hitInfo);
        if (!genLog)
          ;
        hitDataList.Add(hitData);
      }
      return hitDataList;
    }

    public static void writeInfo(SendPacket s, ReceivePacket p, bool genLog)
    {
      List<a20000_InvalidHitData.HitData> hits = a20000_InvalidHitData.ReadInfo(p, genLog, true);
      a20000_InvalidHitData.writeInfo(s, hits);
    }

    public static void writeInfo(SendPacket s, List<a20000_InvalidHitData.HitData> hits)
    {
      s.writeC((byte) hits.Count);
      for (int index = 0; index < hits.Count; ++index)
      {
        a20000_InvalidHitData.HitData hit = hits[index];
        s.writeH(hit._hitInfo);
        s.writeHVector(hit.FirePos);
        s.writeHVector(hit.HitPos);
      }
    }

    public class HitData
    {
      public ushort _hitInfo;
      public Half3 FirePos;
      public Half3 HitPos;
      public HitType HitEnum;
    }
  }
}
