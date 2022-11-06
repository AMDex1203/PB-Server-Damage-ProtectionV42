
// Type: Core.models.enums.flags.CupomEffects
// Assembly: Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 263E2FBF-1098-4552-8FB3-F6A660503737
// Interprise: C:\Users\Cuzin\3,50Core.dll

using System;

namespace Core.models.enums.flags
{
  [Flags]
  public enum CupomEffects
  {
    Colete90 = 1,
    Ketupat = 2,
    Colete20 = 4,
    HollowPointPlus = 8,
    Colete10 = 16, // 0x00000010
    HP5 = 32, // 0x00000020
    HollowPointF = 64, // 0x00000040
    ExtraGrenade = 128, // 0x00000080
    C4SpeedKit = 256, // 0x00000100
    HollowPoint = 512, // 0x00000200
    IronBullet = 1024, // 0x00000400
    Colete5 = 2048, // 0x00000800
    Invulnerability = 4096, // 0x00001000
    HP10 = 8192, // 0x00002000
    FastReload = 16384, // 0x00004000
    FastChange = 32768, // 0x00008000
    FlashProtect = 65536, // 0x00010000
    TakeDrop = 131072, // 0x00020000
    Ammo40 = 262144, // 0x00040000
    Respawn30 = 524288, // 0x00080000
    Respawn50 = 1048576, // 0x00100000
    Respawn100 = 2097152, // 0x00200000
    Ammo10 = 4194304, // 0x00400000
    ExtraThrowGrenade = 8388608, // 0x00800000
  }
}
