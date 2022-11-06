
// Type: Game.global.clientpacket.BASE_USER_EXIT_REC
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.server;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
  public class BASE_USER_EXIT_REC : ReceiveGamePacket
  {
    public BASE_USER_EXIT_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read()
    {
    }

    public override void run()
    {
      try
      {
        this._client.SendPacket((SendPacket) new BASE_USER_EXIT_PAK());
        this._client.Close(1000);
      }
      catch (Exception ex)
      {
        Logger.info(ex.ToString());
        this._client.Close(0);
      }
    }
  }
}
