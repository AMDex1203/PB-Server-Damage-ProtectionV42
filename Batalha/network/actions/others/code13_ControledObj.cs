
// Type: Battle.network.actions.others.code13_ControledObj
// Assembly: pbserver_battle, Version=1.0.0.2646, Culture=neutral, PublicKeyToken=null
// MVID: AA2D7911-B687-4238-AB73-D37E54123039
// Interprise: C:\Users\Cuzin\3,50pbserver_battle.exe

using System;

namespace Battle.network.actions.others
{
  public class code13_ControledObj
  {
    public static code13_ControledObj.Struct readSyncInfo(
      ReceivePacket p,
      bool genLog)
    {
      code13_ControledObj.Struct @struct = new code13_ControledObj.Struct()
      {
        _unk = p.readB(9)
      };
      if (genLog)
        Logger.warning("[code13_ControledObj] " + BitConverter.ToString(@struct._unk));
      return @struct;
    }

    public static void writeInfo(SendPacket s, ReceivePacket p, bool genLog)
    {
      code13_ControledObj.Struct @struct = code13_ControledObj.readSyncInfo(p, genLog);
      s.writeB(@struct._unk);
    }

    public class Struct
    {
      public byte[] _unk;
    }
  }
}
