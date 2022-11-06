
// Type: Auth.global.serverpacket.A_2678_PAK
// Assembly: pbserver_auth, Version=1.0.7796.38604, Culture=neutral, PublicKeyToken=null
// MVID: 1CFDFF97-782C-4EF8-A621-6E4FDDE32436
// Interprise: C:\Users\Cuzin\3,50pbserver_auth.exe

using Core.server;

namespace Auth.global.serverpacket
{
  public class A_2678_PAK : SendPacket
  {
    public override void write()
    {
      this.writeH((short) 2679);
      this.writeD(8);
      this.writeC((byte) 1);
      this.writeC((byte) 5);
      this.writeH((short) 1);
      this.writeH((short) 15);
      this.writeH((short) 37);
      this.writeH((short) 21);
      this.writeH((short) 1012);
      this.writeH((short) 12);
      this.writeC((byte) 5);
      this.writeS("Mar  2 2017", 11);
      this.writeD(0);
      this.writeS("11:10:23", 8);
      this.writeB(new byte[7]);
      this.writeS("DIST", 4);
      this.writeH((short) 0);
    }
  }
}
