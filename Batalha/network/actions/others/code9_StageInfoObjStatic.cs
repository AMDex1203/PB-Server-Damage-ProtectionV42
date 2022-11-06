
// Type: Battle.network.actions.others.code9_StageInfoObjStatic
// Assembly: pbserver_battle, Version=1.0.0.2646, Culture=neutral, PublicKeyToken=null
// MVID: AA2D7911-B687-4238-AB73-D37E54123039
// Interprise: C:\Users\Cuzin\3,50pbserver_battle.exe

namespace Battle.network.actions.others
{
  public class code9_StageInfoObjStatic
  {
    public static code9_StageInfoObjStatic.Struct readSyncInfo(
      ReceivePacket p,
      bool genLog)
    {
      code9_StageInfoObjStatic.Struct @struct = new code9_StageInfoObjStatic.Struct()
      {
        _isDestroyed = p.readC()
      };
      if (genLog)
        Logger.warning("[code9_StageInfoObjStatic] u: " + @struct._isDestroyed.ToString());
      return @struct;
    }

    public static void writeInfo(SendPacket s, ReceivePacket p, bool genLog)
    {
      code9_StageInfoObjStatic.Struct @struct = code9_StageInfoObjStatic.readSyncInfo(p, genLog);
      s.writeC(@struct._isDestroyed);
    }

    public class Struct
    {
      public byte _isDestroyed;
    }
  }
}
