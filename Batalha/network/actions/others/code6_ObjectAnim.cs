
// Type: Battle.network.actions.others.code6_ObjectAnim
// Assembly: pbserver_battle, Version=1.0.0.2646, Culture=neutral, PublicKeyToken=null
// MVID: AA2D7911-B687-4238-AB73-D37E54123039
// Interprise: C:\Users\Cuzin\3,50pbserver_battle.exe

namespace Battle.network.actions.others
{
  public class code6_ObjectAnim
  {
    public static byte[] ReadInfo(ReceivePacket p) => p.readB(8);

    public static code6_ObjectAnim.Struct ReadInfo(ReceivePacket p, bool genLog)
    {
      code6_ObjectAnim.Struct @struct = new code6_ObjectAnim.Struct()
      {
        _life = p.readUH(),
        _anim1 = p.readC(),
        _anim2 = p.readC(),
        _syncDate = p.readT()
      };
      if (genLog)
        Logger.warning("[code6_ObjectAnim] u: " + @struct._life.ToString() + "; u2: " + @struct._anim1.ToString() + "; u3: " + @struct._anim2.ToString() + "; u4: " + @struct._syncDate.ToString());
      return @struct;
    }

    public static void writeInfo(SendPacket s, ReceivePacket p) => s.writeB(code6_ObjectAnim.ReadInfo(p));

    public static void writeInfo(SendPacket s, ReceivePacket p, bool genLog)
    {
      code6_ObjectAnim.Struct @struct = code6_ObjectAnim.ReadInfo(p, genLog);
      s.writeH(@struct._life);
      s.writeC(@struct._anim1);
      s.writeC(@struct._anim2);
      s.writeT(@struct._syncDate);
    }

    public class Struct
    {
      public float _syncDate;
      public byte _anim1;
      public byte _anim2;
      public ushort _life;
    }
  }
}
