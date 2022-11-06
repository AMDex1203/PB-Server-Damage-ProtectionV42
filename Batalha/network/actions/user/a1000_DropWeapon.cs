
// Type: Battle.network.actions.user.a1000_DropWeapon
// Assembly: pbserver_battle, Version=1.0.0.2646, Culture=neutral, PublicKeyToken=null
// MVID: AA2D7911-B687-4238-AB73-D37E54123039
// Interprise: C:\Users\Cuzin\3,50pbserver_battle.exe

using Battle.config;
using Battle.data.enums.weapon;
using System;

namespace Battle.network.actions.user
{
  public class a1000_DropWeapon
  {
    public static a1000_DropWeapon.Struct ReadInfo(ReceivePacket p, bool genLog)
    {
      a1000_DropWeapon.Struct @struct = new a1000_DropWeapon.Struct()
      {
        _weaponFlag = p.readC(),
        _weaponClass = p.readC(),
        _weaponId = p.readUH(),
        _ammoPrin = p.readC(),
        _ammoDual = p.readC(),
        _ammoTotal = p.readUH()
      };
      if (genLog)
      {
        Logger.warning("[ActionBuffer]: " + BitConverter.ToString(p.getBuffer()));
        Logger.warning("[DropWeapon] Flag: " + @struct._weaponFlag.ToString() + "; Class: " + ((ClassType) @struct._weaponClass).ToString() + "; Id: " + @struct._weaponId.ToString());
      }
      return @struct;
    }

    public static void ReadInfo(ReceivePacket p) => p.Advance(8);

    public static void writeInfo(SendPacket s, ReceivePacket p, bool genLog, int count)
    {
      a1000_DropWeapon.Struct @struct = a1000_DropWeapon.ReadInfo(p, genLog);
      s.writeC((byte) ((uint) @struct._weaponFlag + (uint) count));
      s.writeC(@struct._weaponClass);
      s.writeH(@struct._weaponId);
      if (Config.useMaxAmmoInDrop)
      {
        s.writeC(byte.MaxValue);
        s.writeC(@struct._ammoDual);
        s.writeH((short) 10000);
      }
      else
      {
        s.writeC(@struct._ammoPrin);
        s.writeC(@struct._ammoDual);
        s.writeH(@struct._ammoTotal);
      }
    }

    public class Struct
    {
      public byte _weaponFlag;
      public byte _weaponClass;
      public byte _ammoPrin;
      public byte _ammoDual;
      public ushort _weaponId;
      public ushort _ammoTotal;
    }
  }
}
