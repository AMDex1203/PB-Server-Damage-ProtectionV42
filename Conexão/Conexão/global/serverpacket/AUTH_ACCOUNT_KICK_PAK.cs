
// Type: Auth.global.serverpacket.AUTH_ACCOUNT_KICK_PAK
// Assembly: pbserver_auth, Version=1.0.7796.38604, Culture=neutral, PublicKeyToken=null
// MVID: 1CFDFF97-782C-4EF8-A621-6E4FDDE32436
// Interprise: C:\Users\Cuzin\3,50pbserver_auth.exe

using Core.server;

namespace Auth.global.serverpacket
{
  public class AUTH_ACCOUNT_KICK_PAK : SendPacket
  {
    private int _type;

    public AUTH_ACCOUNT_KICK_PAK(int type) => this._type = type;

    public override void write()
    {
      this.writeH((short) 513);
      this.writeC((byte) this._type);
    }
  }
}
