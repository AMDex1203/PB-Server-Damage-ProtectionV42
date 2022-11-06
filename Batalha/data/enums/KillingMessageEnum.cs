
// Type: Battle.data.enums.KillingMessageEnum
// Assembly: pbserver_battle, Version=1.0.0.2646, Culture=neutral, PublicKeyToken=null
// MVID: AA2D7911-B687-4238-AB73-D37E54123039
// Interprise: C:\Users\Cuzin\3,50pbserver_battle.exe

namespace Battle.data.enums
{
  public enum KillingMessageEnum
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
