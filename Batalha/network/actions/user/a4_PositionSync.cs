
// Type: Battle.network.actions.user.a4_PositionSync
// Assembly: pbserver_battle, Version=1.0.0.2646, Culture=neutral, PublicKeyToken=null
// MVID: AA2D7911-B687-4238-AB73-D37E54123039
// Interprise: C:\Users\Cuzin\3,50pbserver_battle.exe

namespace Battle.network.actions.user
{
  public class a4_PositionSync
  {
    public static a4_PositionSync.Struct ReadInfo(ReceivePacket p, bool genLog)
    {
      a4_PositionSync.Struct @struct = new a4_PositionSync.Struct()
      {
        _rotationX = p.readUH(),
        _rotationY = p.readUH(),
        _rotationZ = p.readUH(),
        _cameraX = p.readUH(),
        _cameraY = p.readUH(),
        _area = p.readUH()
      };
      if (!genLog)
        ;
      return @struct;
    }

    public static void writeInfo(SendPacket s, ReceivePacket p, bool genLog)
    {
      a4_PositionSync.Struct info = a4_PositionSync.ReadInfo(p, genLog);
      a4_PositionSync.writeInfo(s, info);
    }

    public static void writeInfo(SendPacket s, a4_PositionSync.Struct info)
    {
      s.writeH(info._rotationX);
      s.writeH(info._rotationY);
      s.writeH(info._rotationZ);
      s.writeH(info._cameraX);
      s.writeH(info._cameraY);
      s.writeH(info._area);
    }

    public class Struct
    {
      public ushort _rotationX;
      public ushort _rotationY;
      public ushort _rotationZ;
      public ushort _cameraX;
      public ushort _cameraY;
      public ushort _area;
    }
  }
}
