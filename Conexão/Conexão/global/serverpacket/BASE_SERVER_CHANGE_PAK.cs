
// Type: Auth.global.serverpacket.BASE_SERVER_CHANGE_PAK
// Assembly: pbserver_auth, Version=1.0.7796.38604, Culture=neutral, PublicKeyToken=null
// MVID: 1CFDFF97-782C-4EF8-A621-6E4FDDE32436
// Interprise: C:\Users\Cuzin\3,50pbserver_auth.exe

using Core.server;

namespace Auth.global.serverpacket
{
  public class BASE_SERVER_CHANGE_PAK : SendPacket
  {
    private int erro;

    public BASE_SERVER_CHANGE_PAK(int erro) => this.erro = erro;

    public override void write()
    {
      this.writeH((short) 2578);
      this.writeD(this.erro);
    }
  }
}
