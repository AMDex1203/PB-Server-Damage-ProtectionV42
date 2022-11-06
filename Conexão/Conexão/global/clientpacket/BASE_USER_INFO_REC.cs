
// Type: Auth.global.clientpacket.BASE_USER_INFO_REC
// Assembly: pbserver_auth, Version=1.0.7796.38604, Culture=neutral, PublicKeyToken=null
// MVID: 1CFDFF97-782C-4EF8-A621-6E4FDDE32436
// Interprise: C:\Users\Cuzin\3,50pbserver_auth.exe

using Auth.global.serverpacket;
using Core;
using Core.server;
using System;

namespace Auth.global.clientpacket
{
  public class BASE_USER_INFO_REC : ReceiveLoginPacket
  {
    public BASE_USER_INFO_REC(LoginClient client, byte[] data) => this.makeme(client, data);

    public override void read()
    {
    }

    public override void run()
    {
      try
      {
        this._client.SendPacket((SendPacket) new BASE_USER_INFO_PAK(this._client._player));
        this._client.SendPacket((SendPacket) new BASE_RANK_AWARDS_PAK());
      }
      catch (Exception ex)
      {
        Logger.warning("[BASE_USER_INFO_REC] " + ex.ToString());
      }
    }
  }
}
