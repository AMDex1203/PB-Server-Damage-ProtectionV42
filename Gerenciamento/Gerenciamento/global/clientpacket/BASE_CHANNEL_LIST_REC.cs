
// Type: Game.global.clientpacket.BASE_CHANNEL_LIST_REC
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;
using Game.global.serverpacket;

namespace Game.global.clientpacket
{
  public class BASE_CHANNEL_LIST_REC : ReceiveGamePacket
  {
    public BASE_CHANNEL_LIST_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read()
    {
    }

    public override void run()
    {
      if (this._client._player == null)
        return;
      this._client.SendPacket((SendPacket) new BASE_CHANNEL_LIST_PAK());
    }
  }
}
