
// Type: Battle.network.actions.user.a80_WeaponRecoil
// Assembly: pbserver_battle, Version=1.0.0.2646, Culture=neutral, PublicKeyToken=null
// MVID: AA2D7911-B687-4238-AB73-D37E54123039
// Interprise: C:\Users\Cuzin\3,50pbserver_battle.exe

using Battle.data.models;

namespace Battle.network.actions.user
{
  public class a80_WeaponRecoil
  {
    public static a80_WeaponRecoil.Struct ReadInfo(
      ActionModel ac,
      ReceivePacket p,
      bool genLog)
    {
      a80_WeaponRecoil.Struct @struct = new a80_WeaponRecoil.Struct()
      {
        _RecoilHorzAngle = p.readT(),
        _RecoilHorzMax = p.readT(),
        _RecoilVertAngle = p.readT(),
        _RecoilVertMax = p.readT(),
        _Deviation = p.readT(),
        _weaponId = p.readUH(),
        _weaponSlot = p.readC(),
        _unkV = p.readC(),
        _RecoilHorzCount = p.readC()
      };
      if (genLog)
        Logger.warning("Slot " + ac._slot.ToString() + " weapon info: (" + @struct._RecoilHorzAngle.ToString() + ";" + @struct._RecoilHorzMax.ToString() + ";" + @struct._RecoilVertAngle.ToString() + ";" + @struct._RecoilVertMax.ToString() + ";" + @struct._Deviation.ToString() + ";" + @struct._weaponId.ToString() + ";" + @struct._weaponSlot.ToString() + ";" + @struct._unkV.ToString() + ";" + @struct._RecoilHorzCount.ToString() + ")");
      return @struct;
    }

    public static void ReadInfo(ReceivePacket p) => p.Advance(25);

    public static void writeInfo(SendPacket s, ActionModel ac, ReceivePacket p, bool genLog)
    {
      a80_WeaponRecoil.Struct @struct = a80_WeaponRecoil.ReadInfo(ac, p, genLog);
      s.writeT(@struct._RecoilHorzAngle);
      s.writeT(@struct._RecoilHorzMax);
      s.writeT(@struct._RecoilVertAngle);
      s.writeT(@struct._RecoilVertMax);
      s.writeT(@struct._Deviation);
      s.writeH(@struct._weaponId);
      s.writeC(@struct._weaponSlot);
      s.writeC(@struct._unkV);
      s.writeC(@struct._RecoilHorzCount);
    }

    public class Struct
    {
      public float _RecoilHorzAngle;
      public float _RecoilHorzMax;
      public float _RecoilVertAngle;
      public float _RecoilVertMax;
      public float _Deviation;
      public ushort _weaponId;
      public byte _weaponSlot;
      public byte _unkV;
      public byte _RecoilHorzCount;
    }
  }
}
