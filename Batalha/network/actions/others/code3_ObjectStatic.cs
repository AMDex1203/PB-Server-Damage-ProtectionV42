
// Type: Battle.network.actions.others.code3_ObjectStatic
// Assembly: pbserver_battle, Version=1.0.0.2646, Culture=neutral, PublicKeyToken=null
// MVID: AA2D7911-B687-4238-AB73-D37E54123039
// Interprise: C:\Users\Cuzin\3,50pbserver_battle.exe

namespace Battle.network.actions.others
{
  public class code3_ObjectStatic
  {
    public static byte[] ReadInfo(ReceivePacket p) => p.readB(3);

    public static code3_ObjectStatic.Struct ReadInfo(ReceivePacket p, bool genLog)
    {
      code3_ObjectStatic.Struct @struct = new code3_ObjectStatic.Struct()
      {
        Life = p.readUH(),
        DestroyedBySlot = p.readC()
      };
      if (genLog)
        Logger.warning("[code3_ObjectStatic] Life: " + @struct.Life.ToString() + "; DestroyedBy: " + @struct.DestroyedBySlot.ToString());
      return @struct;
    }

    public static void writeInfo(SendPacket s, ReceivePacket p) => s.writeB(code3_ObjectStatic.ReadInfo(p));

    public static void writeInfo(SendPacket s, ReceivePacket p, bool genLog)
    {
      code3_ObjectStatic.Struct @struct = code3_ObjectStatic.ReadInfo(p, genLog);
      s.writeH(@struct.Life);
      s.writeC(@struct.DestroyedBySlot);
    }

    public class Struct
    {
      public byte DestroyedBySlot;
      public ushort Life;
    }
  }
}
