
// Type: Battle.network.actions.user.a2_unk
// Assembly: pbserver_battle, Version=1.0.0.2646, Culture=neutral, PublicKeyToken=null
// MVID: AA2D7911-B687-4238-AB73-D37E54123039
// Interprise: C:\Users\Cuzin\3,50pbserver_battle.exe

using Battle.data.models;

namespace Battle.network.actions.user
{
  public class a2_unk
  {
    public static a2_unk.Struct ReadInfo(ActionModel ac, ReceivePacket p, bool genLog)
    {
      a2_unk.Struct @struct = new a2_unk.Struct()
      {
        _unkV = p.readUH()
      };
      if (genLog)
        Logger.warning("Slot " + ac._slot.ToString() + " is moving the crosshair position: posV (" + @struct._unkV.ToString() + ")");
      return @struct;
    }

    public static void writeInfo(SendPacket s, ActionModel ac, ReceivePacket p, bool genLog)
    {
      a2_unk.Struct @struct = a2_unk.ReadInfo(ac, p, genLog);
      s.writeH(@struct._unkV);
    }

    public class Struct
    {
      public ushort _unkV;
    }
  }
}
