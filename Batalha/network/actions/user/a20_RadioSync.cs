
// Type: Battle.network.actions.user.a20_RadioSync
// Assembly: pbserver_battle, Version=1.0.0.2646, Culture=neutral, PublicKeyToken=null
// MVID: AA2D7911-B687-4238-AB73-D37E54123039
// Interprise: C:\Users\Cuzin\3,50pbserver_battle.exe

using Battle.data.models;

namespace Battle.network.actions.user
{
  public class a20_RadioSync
  {
    public static a20_RadioSync.Struct readSyncInfo(
      ActionModel ac,
      ReceivePacket p,
      bool genLog)
    {
      a20_RadioSync.Struct @struct = new a20_RadioSync.Struct()
      {
        _radioId = p.readC(),
        _areaId = p.readC()
      };
      if (genLog)
        Logger.warning("Slot " + ac._slot.ToString() + " released a radio chat: radId, areaId [" + @struct._radioId.ToString() + ";" + @struct._areaId.ToString() + "]");
      return @struct;
    }

    public static void writeInfo(SendPacket s, ActionModel ac, ReceivePacket p, bool genLog)
    {
      a20_RadioSync.Struct @struct = a20_RadioSync.readSyncInfo(ac, p, genLog);
      s.writeC(@struct._radioId);
      s.writeC(@struct._areaId);
    }

    public class Struct
    {
      public byte _radioId;
      public byte _areaId;
    }
  }
}
