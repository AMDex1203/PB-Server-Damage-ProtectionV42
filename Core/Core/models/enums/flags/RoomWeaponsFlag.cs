
// Type: Core.models.enums.flags.RoomWeaponsFlag
// Assembly: Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 263E2FBF-1098-4552-8FB3-F6A660503737
// Interprise: C:\Users\Cuzin\3,50Core.dll

using System;

namespace Core.models.enums.flags
{
  [Flags]
  public enum RoomWeaponsFlag
  {
    Mode_Barefist = 128, // 0x00000080
    Mode_870MCS = 64, // 0x00000040
    Mode_SSG69 = 32, // 0x00000020
    RPG7 = 16, // 0x00000010
    Primary = 8,
    Secondary = 4,
    Melee = 2,
    Grenade = 1,
  }
}
