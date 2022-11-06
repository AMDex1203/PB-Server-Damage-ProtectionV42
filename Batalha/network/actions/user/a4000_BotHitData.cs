
// Type: Battle.network.actions.user.a4000_BotHitData
// Assembly: pbserver_battle, Version=1.0.0.2646, Culture=neutral, PublicKeyToken=null
// MVID: AA2D7911-B687-4238-AB73-D37E54123039
// Interprise: C:\Users\Cuzin\3,50pbserver_battle.exe

using System;
using System.Collections.Generic;

namespace Battle.network.actions.user
{
  public class a4000_BotHitData
  {
    public static void ReadInfo(ReceivePacket p)
    {
      int num = (int) p.readC();
      p.Advance(15 * num);
    }

    public static List<a4000_BotHitData.HitData> ReadInfo(
      ReceivePacket p,
      bool genLog)
    {
      List<a4000_BotHitData.HitData> hitDataList = new List<a4000_BotHitData.HitData>();
      int num = (int) p.readC();
      for (int index = 0; index < num; ++index)
      {
        a4000_BotHitData.HitData hitData = new a4000_BotHitData.HitData()
        {
          _hitInfo = p.readUD(),
          _weaponInfo = p.readUH(),
          _weaponSlot = p.readC(),
          _unk = p.readUH(),
          _eixoX = p.readUH(),
          _eixoY = p.readUH(),
          _eixoZ = p.readUH()
        };
        if (genLog)
        {
          Logger.warning("P: " + hitData._eixoX.ToString() + ";" + hitData._eixoY.ToString() + ";" + hitData._eixoZ.ToString());
          Logger.warning("[" + index.ToString() + "] 16384: " + BitConverter.ToString(p.getBuffer()));
        }
        hitDataList.Add(hitData);
      }
      return hitDataList;
    }

    public static void writeInfo(SendPacket s, ReceivePacket p, bool genLog)
    {
      List<a4000_BotHitData.HitData> hitDataList = a4000_BotHitData.ReadInfo(p, genLog);
      s.writeC((byte) hitDataList.Count);
      for (int index = 0; index < hitDataList.Count; ++index)
      {
        a4000_BotHitData.HitData hitData = hitDataList[index];
        s.writeD(hitData._hitInfo);
        s.writeH(hitData._weaponInfo);
        s.writeC(hitData._weaponSlot);
        s.writeH(hitData._unk);
        s.writeH(hitData._eixoX);
        s.writeH(hitData._eixoY);
        s.writeH(hitData._eixoZ);
      }
    }

    public class HitData
    {
      public byte _weaponSlot;
      public ushort _weaponInfo;
      public ushort _eixoX;
      public ushort _eixoY;
      public ushort _eixoZ;
      public ushort _unk;
      public uint _hitInfo;
    }
  }
}
