
// Type: Battle.network.actions.user.a40_WeaponSync
// Assembly: pbserver_battle, Version=1.0.0.2646, Culture=neutral, PublicKeyToken=null
// MVID: AA2D7911-B687-4238-AB73-D37E54123039
// Interprise: C:\Users\Cuzin\3,50pbserver_battle.exe

using Battle.data.models;

namespace Battle.network.actions.user
{
  public class a40_WeaponSync
  {
    public static a40_WeaponSync.Struct ReadInfo(
      ActionModel ac,
      ReceivePacket p,
      bool genLog,
      bool OnlyBytes = false)
    {
      return a40_WeaponSync.BaseReadInfo(ac, p, OnlyBytes, genLog);
    }

    private static a40_WeaponSync.Struct BaseReadInfo(
      ActionModel ac,
      ReceivePacket p,
      bool OnlyBytes,
      bool genLog)
    {
      a40_WeaponSync.Struct @struct = new a40_WeaponSync.Struct()
      {
        _weaponInfo = p.readUH(),
        _weaponSlotInfo = p.readC(),
        _charaModelId = p.readC()
      };
      if (!OnlyBytes)
      {
        @struct.WeaponSecondMelee = (int) @struct._weaponSlotInfo >> 4;
        @struct.WeaponSlot = (int) @struct._weaponSlotInfo & 15;
        @struct.WeaponId = (int) @struct._weaponInfo >> 6 & 1023;
        @struct.WeaponClass = (int) @struct._weaponInfo & 63;
      }
      if (!genLog)
        ;
      return @struct;
    }

    public static void writeInfo(SendPacket s, ActionModel ac, ReceivePacket p, bool genLog)
    {
      a40_WeaponSync.Struct info = a40_WeaponSync.ReadInfo(ac, p, genLog, true);
      a40_WeaponSync.writeInfo(s, info);
    }

    public static void writeInfo(SendPacket s, a40_WeaponSync.Struct info)
    {
      s.writeH(info._weaponInfo);
      s.writeC(info._weaponSlotInfo);
      s.writeC(info._charaModelId);
    }

    public class Struct
    {
      public ushort _weaponInfo;
      public byte _weaponSlotInfo;
      public byte _charaModelId;
      public int WeaponClass;
      public int WeaponId;
      public int WeaponSlot;
      public int WeaponSecondMelee;
    }
  }
}
