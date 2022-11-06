
// Type: Game.global.clientpacket.CLAN_CREATE_REQUIREMENTS_REC
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;
using Game.global.serverpacket;

namespace Game.global.clientpacket
{
  public class CLAN_CREATE_REQUIREMENTS_REC : ReceiveGamePacket
  {
    public CLAN_CREATE_REQUIREMENTS_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read()
    {
    }

    public override void run()
    {
      try
      {
        if (this._client == null)
          return;
        this._client.SendPacket((SendPacket) new CLAN_CREATE_REQUIREMENTS_PAK());
      }
      catch
      {
      }
    }
  }
}
