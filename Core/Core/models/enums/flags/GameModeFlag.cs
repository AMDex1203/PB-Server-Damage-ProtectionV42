
// Type: Core.models.enums.flags.GameModeFlag
// Assembly: Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 263E2FBF-1098-4552-8FB3-F6A660503737
// Interprise: C:\Users\Cuzin\3,50Core.dll

using System;

namespace Core.models.enums.flags
{
  [Flags]
  public enum GameModeFlag
  {
    TDM_Challenge = 1,
    SniperMode = 4,
    TDM_Supression = 8,
    HeadHunter = 32, // 0x00000020
    ShotgunMode = 128, // 0x00000080
    CrossCounter = 256, // 0x00000100
    Tutorial = 512, // 0x00000200
    Zombie = 1024, // 0x00000400
    DURO_DE_MATAR = 2048, // 0x00000800
    Chaos = 4096, // 0x00001000
  }
}
