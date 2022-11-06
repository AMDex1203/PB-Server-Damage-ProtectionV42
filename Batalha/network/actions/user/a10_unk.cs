
// Type: Battle.network.actions.user.a10_unk
// Assembly: pbserver_battle, Version=1.0.0.2646, Culture=neutral, PublicKeyToken=null
// MVID: AA2D7911-B687-4238-AB73-D37E54123039
// Interprise: C:\Users\Cuzin\3,50pbserver_battle.exe

using Battle.data.models;

namespace Battle.network.actions.user
{
  public class a10_unk
  {
    public static a10_unk.Struct readSyncInfo(ActionModel ac, ReceivePacket p, bool genLog)
    {
      a10_unk.Struct @struct = new a10_unk.Struct()
      {
        _unkV = p.readC(),
        _unkV2 = p.readC()
      };
      if (genLog)
        Logger.warning("Slot " + ac._slot.ToString() + " action 16: unk (" + @struct._unkV.ToString() + ";" + @struct._unkV2.ToString() + ")");
      return @struct;
    }

    public static void writeInfo(SendPacket s, ActionModel ac, ReceivePacket p, bool genLog)
    {
      a10_unk.Struct @struct = a10_unk.readSyncInfo(ac, p, genLog);
      s.writeC(@struct._unkV);
      s.writeC(@struct._unkV2);
    }

    public class Struct
    {
      public byte _unkV;
      public byte _unkV2;
    }
  }
}
