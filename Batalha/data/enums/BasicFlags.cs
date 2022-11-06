
// Type: Battle.data.enums.BasicFlags
// Assembly: pbserver_battle, Version=1.0.0.2646, Culture=neutral, PublicKeyToken=null
// MVID: AA2D7911-B687-4238-AB73-D37E54123039
// Interprise: C:\Users\Cuzin\3,50pbserver_battle.exe

using System;

namespace Battle.data.enums
{
  [Flags]
  public enum BasicFlags
  {
    flag0 = 0,
    flag1 = 1,
    flag2 = 2,
    flag3 = 4,
    flag4 = 8,
    flag5 = 16, // 0x00000010
    flag6 = 32, // 0x00000020
    flag7 = 64, // 0x00000040
    flag8 = 128, // 0x00000080
  }
}
