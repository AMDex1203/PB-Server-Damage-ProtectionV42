
// Type: Battle.network.actions.others.code2_WeaponSync
// Assembly: pbserver_battle, Version=1.0.0.2646, Culture=neutral, PublicKeyToken=null
// MVID: AA2D7911-B687-4238-AB73-D37E54123039
// Interprise: C:\Users\Cuzin\3,50pbserver_battle.exe

using System;

namespace Battle.network.actions.others
{
  public class code2_WeaponSync
  {
    public static byte[] ReadInfo(ReceivePacket p) => p.readB(15);

    public static code2_WeaponSync.Struct ReadInfo(ReceivePacket p, bool genLog)
    {
      code2_WeaponSync.Struct @struct = new code2_WeaponSync.Struct()
      {
        _weaponFlag = p.readC(),
        _posX = p.readUH(),
        _posY = p.readUH(),
        _posZ = p.readUH(),
        _unk4 = p.readUH(),
        _unk5 = p.readUH(),
        _unk6 = p.readUH(),
        _unk7 = p.readUH()
      };
      if (genLog)
      {
        Logger.warning("[code2_WeaponSync] " + BitConverter.ToString(p.getBuffer()));
        Logger.warning("[code2_WeaponSync] Flag: " + @struct._weaponFlag.ToString() + "; u4: " + @struct._unk4.ToString() + "; u5: " + @struct._unk5.ToString() + "; u6: " + @struct._unk6.ToString() + "; u7: " + @struct._unk7.ToString());
      }
      return @struct;
    }

    public static void writeInfo(SendPacket s, ReceivePacket p) => s.writeB(code2_WeaponSync.ReadInfo(p));

    public static void writeInfo(SendPacket s, ReceivePacket p, bool genLog)
    {
      code2_WeaponSync.Struct @struct = code2_WeaponSync.ReadInfo(p, genLog);
      s.writeC(@struct._weaponFlag);
      s.writeH(@struct._posX);
      s.writeH(@struct._posY);
      s.writeH(@struct._posZ);
      s.writeH(@struct._unk4);
      s.writeH(@struct._unk5);
      s.writeH(@struct._unk6);
      s.writeH(@struct._unk7);
    }

    public class Struct
    {
      public byte _weaponFlag;
      public ushort _posX;
      public ushort _posY;
      public ushort _posZ;
      public ushort _unk4;
      public ushort _unk5;
      public ushort _unk6;
      public ushort _unk7;
    }
  }
}
