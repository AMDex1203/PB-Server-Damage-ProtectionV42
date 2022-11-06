
// Type: Battle.network.actions.user.a100_LifeSync
// Assembly: pbserver_battle, Version=1.0.0.2646, Culture=neutral, PublicKeyToken=null
// MVID: AA2D7911-B687-4238-AB73-D37E54123039
// Interprise: C:\Users\Cuzin\3,50pbserver_battle.exe

namespace Battle.network.actions.user
{
  public class a100_LifeSync
  {
    public static ushort ReadInfo(ReceivePacket p, bool genLog) => p.readUH();

    public static void ReadInfo(ReceivePacket p) => p.Advance(2);

    public static void writeInfo(SendPacket s, ReceivePacket p, bool genLog) => s.writeH(a100_LifeSync.ReadInfo(p, genLog));
  }
}
