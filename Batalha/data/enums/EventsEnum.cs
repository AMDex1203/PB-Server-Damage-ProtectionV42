
// Type: Battle.data.enums.EventsEnum
// Assembly: pbserver_battle, Version=1.0.0.2646, Culture=neutral, PublicKeyToken=null
// MVID: AA2D7911-B687-4238-AB73-D37E54123039
// Interprise: C:\Users\Cuzin\3,50pbserver_battle.exe

using System;

namespace Battle.data.enums
{
  [Flags]
  public enum EventsEnum : uint
  {
    ActionState = 1,
    Animation = 2,
    PosRotation = 4,
    OnLoadObject = 8,
    Unk1 = 16, // 0x00000010
    RadioChat = 32, // 0x00000020
    WeaponSync = 64, // 0x00000040
    WeaponRecoil = 128, // 0x00000080
    LifeSync = 256, // 0x00000100
    Suicide = 512, // 0x00000200
    BombMission = 1024, // 0x00000400
    TakeWeapon = 2048, // 0x00000800
    DropWeapon = 4096, // 0x00001000
    FireSync = 8192, // 0x00002000
    AIDamage = 16384, // 0x00004000
    NormalDamage = 32768, // 0x00008000
    BoomDamage = 65536, // 0x00010000
    Unk3 = 131072, // 0x00020000
    Death = 262144, // 0x00040000
    SufferingDamage = 524288, // 0x00080000
    PassPortal = 1048576, // 0x00100000
  }
}
