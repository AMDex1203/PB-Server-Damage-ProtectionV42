
// Type: Battle.network.actions.others.code12_StageObjAnim
// Assembly: pbserver_battle, Version=1.0.0.2646, Culture=neutral, PublicKeyToken=null
// MVID: AA2D7911-B687-4238-AB73-D37E54123039
// Interprise: C:\Users\Cuzin\3,50pbserver_battle.exe

namespace Battle.network.actions.others
{
  public class code12_StageObjAnim
  {
    public static byte[] ReadInfo(ReceivePacket p) => p.readB(9);

    public static code12_StageObjAnim.Struct ReadInfo(
      ReceivePacket p,
      bool genLog)
    {
      code12_StageObjAnim.Struct @struct = new code12_StageObjAnim.Struct()
      {
        _unk = p.readC(),
        _life = p.readUH(),
        _syncDate = p.readT(),
        _anim1 = p.readC(),
        _anim2 = p.readC()
      };
      if (genLog)
        Logger.warning("[code12_StageObjAnim] " + @struct._unk.ToString() + ";" + @struct._life.ToString() + ";" + @struct._syncDate.ToString() + ";" + @struct._anim1.ToString() + ";" + @struct._anim2.ToString());
      return @struct;
    }

    public static void writeInfo(SendPacket s, ReceivePacket p) => s.writeB(code12_StageObjAnim.ReadInfo(p));

    public static void writeInfo(SendPacket s, ReceivePacket p, bool genLog)
    {
      code12_StageObjAnim.Struct @struct = code12_StageObjAnim.ReadInfo(p, genLog);
      s.writeC(@struct._unk);
      s.writeH(@struct._life);
      s.writeT(@struct._syncDate);
      s.writeC(@struct._anim1);
      s.writeC(@struct._anim2);
    }

    public class Struct
    {
      public byte _unk;
      public byte _anim1;
      public byte _anim2;
      public float _syncDate;
      public ushort _life;
    }
  }
}
