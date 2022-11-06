
// Type: Auth.global.serverpacket.BASE_LOGIN_PAK
// Assembly: pbserver_auth, Version=1.0.7796.38604, Culture=neutral, PublicKeyToken=null
// MVID: 1CFDFF97-782C-4EF8-A621-6E4FDDE32436
// Interprise: C:\Users\Cuzin\3,50pbserver_auth.exe

using Core.models.enums.errors;
using Core.server;

namespace Auth.global.serverpacket
{
  public class BASE_LOGIN_PAK : SendPacket
  {
    private uint _result;
    private string _login;
    private long _pId;

    public BASE_LOGIN_PAK(EventErrorEnum result, string login, long pId)
    {
      this._result = (uint) result;
      this._login = login;
      this._pId = pId;
    }

    public BASE_LOGIN_PAK(uint result, string login, long pId)
    {
      this._result = result;
      this._login = login;
      this._pId = pId;
    }

    public BASE_LOGIN_PAK(int result, string login, long pId)
    {
      this._result = (uint) result;
      this._login = login;
      this._pId = pId;
    }

    public override void write()
    {
      this.writeH((short) 2564);
      this.writeD(this._result);
      this.writeC((byte) 0);
      this.writeQ(this._pId);
      this.writeC((byte) this._login.Length);
      this.writeS(this._login, this._login.Length);
      this.writeC((byte) 0);
      this.writeC((byte) 0);
    }
  }
}
