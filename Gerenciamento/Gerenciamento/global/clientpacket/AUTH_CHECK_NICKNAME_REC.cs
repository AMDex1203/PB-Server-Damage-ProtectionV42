
// Type: Game.global.clientpacket.AUTH_CHECK_NICKNAME_REC
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.managers;
using Core.server;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
  public class AUTH_CHECK_NICKNAME_REC : ReceiveGamePacket
  {
    private string name;

    public AUTH_CHECK_NICKNAME_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read() => this.name = this.readS(33);

    public override void run()
    {
      try
      {
        if (this._client == null || this._client._player == null)
          return;
        this._client.SendPacket((SendPacket) new AUTH_CHECK_NICKNAME_PAK(!PlayerManager.isPlayerNameExist(this.name) ? 0U : 2147483923U));
      }
      catch (Exception ex)
      {
        Logger.info(ex.ToString());
      }
    }
  }
}
