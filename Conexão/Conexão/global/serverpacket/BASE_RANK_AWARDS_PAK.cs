
// Type: Auth.global.serverpacket.BASE_RANK_AWARDS_PAK
// Assembly: pbserver_auth, Version=1.0.7796.38604, Culture=neutral, PublicKeyToken=null
// MVID: 1CFDFF97-782C-4EF8-A621-6E4FDDE32436
// Interprise: C:\Users\Cuzin\3,50pbserver_auth.exe

using Core.server;

namespace Auth.global.serverpacket
{
  public class BASE_RANK_AWARDS_PAK : SendPacket
  {
    public override void write()
    {
      this.writeH((short) 2667);
      for (byte index = 1; index < (byte) 51; ++index)
      {
        this.writeC(index);
        this.writeD(0);
        this.writeD(0);
        this.writeD(0);
        this.writeD(0);
      }
    }
  }
}
