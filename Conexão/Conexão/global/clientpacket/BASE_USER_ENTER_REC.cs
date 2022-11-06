
// Type: Auth.global.clientpacket.BASE_USER_ENTER_REC
// Assembly: pbserver_auth, Version=1.0.7796.38604, Culture=neutral, PublicKeyToken=null
// MVID: 1CFDFF97-782C-4EF8-A621-6E4FDDE32436
// Interprise: C:\Users\Cuzin\3,50pbserver_auth.exe

using Core;
using Core.server;
//using Game.global.serverpacket;
using System;

namespace Auth.global.clientpacket
{
  public class BASE_USER_ENTER_REC : ReceiveLoginPacket
  {
    private string login;
    private byte[] _IPLocal;
    private long pId;

    public BASE_USER_ENTER_REC(LoginClient client, byte[] data) => this.makeme(client, data);

    public override void read()
    {
      this.login = this.readS((int) this.readC());
      this.pId = this.readQ();
      int num = (int) this.readC();
      this._IPLocal = this.readB(4);
    }

    public override void run()
    {
      if (this._client == null)
        return;
      try
      {
        Logger.warning("2579 received. [Now: " + DateTime.Now.ToString("yyMMddHHmmss") + "]");
        //this._client.SendPacket((SendPacket) new BASE_USER_ENTER_PAK(2147483648U)); //VER DEPOIS
      }
      catch
      {
        this._client.Close(0, true);
      }
    }
  }
}
