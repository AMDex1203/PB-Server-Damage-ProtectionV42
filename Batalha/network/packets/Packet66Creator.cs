
// Type: Battle.network.packets.Packet66Creator
// Assembly: pbserver_battle, Version=1.0.0.2646, Culture=neutral, PublicKeyToken=null
// MVID: AA2D7911-B687-4238-AB73-D37E54123039
// Interprise: C:\Users\Cuzin\3,50pbserver_battle.exe

namespace Battle.network.packets
{
  public class Packet66Creator
  {
    public static byte[] getCode66()
    {
      using (SendPacket sendPacket = new SendPacket())
      {
        sendPacket.writeC((byte) 66);
        sendPacket.writeC((byte) 0);
        sendPacket.writeT(0.0f);
        sendPacket.writeC((byte) 0);
        sendPacket.writeH((short) 13);
        sendPacket.writeD(0);
        return sendPacket.mstream.ToArray();
      }
    }
  }
}
