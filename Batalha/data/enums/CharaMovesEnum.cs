
// Type: Battle.data.enums.CharaMovesEnum
// Assembly: pbserver_battle, Version=1.0.0.2646, Culture=neutral, PublicKeyToken=null
// MVID: AA2D7911-B687-4238-AB73-D37E54123039
// Interprise: C:\Users\Cuzin\3,50pbserver_battle.exe

using System;

namespace Battle.data.enums
{
  [Flags]
  public enum CharaMovesEnum
  {
    STOPPED = 0,
    LEFT = 1,
    BACK = 2,
    RIGHT = 4,
    FRONT = 8,
    HELI_IN_MOVE = 16, // 0x00000010
    HELI_UNKNOWN = 32, // 0x00000020
    HELI_LEAVE = 64, // 0x00000040
    HELI_STOPPED = 128, // 0x00000080
  }
}
