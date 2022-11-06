
// Type: Auth.global.serverpacket.BASE_EXIT_URL_PAK
// Assembly: pbserver_auth, Version=1.0.7796.38604, Culture=neutral, PublicKeyToken=null
// MVID: 1CFDFF97-782C-4EF8-A621-6E4FDDE32436
// Interprise: C:\Users\Cuzin\3,50pbserver_auth.exe

using Core.server;

namespace Auth.global.serverpacket
{
  public class BASE_EXIT_URL_PAK : SendPacket
  {
    private int count;
    private string link;

    public BASE_EXIT_URL_PAK(string link)
    {
      this.count = link.Length > 0 ? 1 : 0;
      this.link = link;
    }

    public override void write()
    {
      this.writeH((short) 2694);
      this.writeC((byte) this.count);
      if (this.count <= 0)
        return;
      this.writeD(1);
      this.writeD(9);
      this.writeS(this.link, 256);
    }
  }
}
