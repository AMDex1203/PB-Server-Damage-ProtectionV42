
// Type: Battle.data.models.ActionModel
// Assembly: pbserver_battle, Version=1.0.0.2646, Culture=neutral, PublicKeyToken=null
// MVID: AA2D7911-B687-4238-AB73-D37E54123039
// Interprise: C:\Users\Cuzin\3,50pbserver_battle.exe

using Battle.data.enums;

namespace Battle.data.models
{
  public class ActionModel
  {
    public ushort _slot;
    public ushort _lengthData;
    public Events _flags;
    public P2P_SUB_HEAD _type;
    public byte[] _data;
  }
}
