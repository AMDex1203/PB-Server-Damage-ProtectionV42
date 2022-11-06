
// Type: Auth.global.serverpacket.SERVER_MESSAGE_ANNOUNCE_PAK
// Assembly: pbserver_auth, Version=1.0.7796.38604, Culture=neutral, PublicKeyToken=null
// MVID: 1CFDFF97-782C-4EF8-A621-6E4FDDE32436
// Interprise: C:\Users\Cuzin\3,50pbserver_auth.exe

using Core.server;

namespace Auth.global.serverpacket
{
  public class SERVER_MESSAGE_ANNOUNCE_PAK : SendPacket
  {
    private string _message;

    public SERVER_MESSAGE_ANNOUNCE_PAK(string msg) => this._message = msg;

    public override void write()
    {
      this.writeH((short) 2055);
      this.writeD(2);
      this.writeH((ushort) this._message.Length);
      this.writeS(this._message);
    }
  }
}
