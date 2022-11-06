
// Type: Game.global.clientpacket.CM_2584
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.server;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
  public class CM_2584 : ReceiveGamePacket
  {
    public byte[] unk;

    public CM_2584(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read() => this.unk = this.readB(59);

    public override void run()
    {
      try
      {
        this._client.SendPacket((SendPacket) new BASE_HACK_PAK(this.unk));
      }
      catch (Exception ex)
      {
        Logger.info(ex.ToString());
      }
    }
  }
}
