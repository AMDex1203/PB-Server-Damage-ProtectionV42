
// Type: Core.models.enums.room.KillingMessage
// Assembly: Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 263E2FBF-1098-4552-8FB3-F6A660503737
// Interprise: C:\Users\Cuzin\3,50Core.dll

namespace Core.models.enums.room
{
  public enum KillingMessage
  {
    PiercingShot = 1,
    MassKill = 2,
    ChainStopper = 4,
    Headshot = 8,
    ChainHeadshot = 16, // 0x00000010
    ChainSlugger = 32, // 0x00000020
    Suicide = 64, // 0x00000040
    ObjectDefense = 128, // 0x00000080
  }
}
