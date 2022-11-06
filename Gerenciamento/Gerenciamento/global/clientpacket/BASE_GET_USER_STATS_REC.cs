
// Type: Game.global.clientpacket.BASE_GET_USER_STATS_REC
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.server;
using Game.data.managers;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
  public class BASE_GET_USER_STATS_REC : ReceiveGamePacket
  {
    private long objId;

    public BASE_GET_USER_STATS_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read() => this.objId = this.readQ();

    public override void run()
    {
      if (this._client._player == null)
        return;
      try
      {
        this._client.SendPacket((SendPacket) new BASE_GET_USER_STATS_PAK(AccountManager.getAccount(this.objId, 0)?._statistic));
      }
      catch (Exception ex)
      {
        Logger.info(ex.ToString());
      }
    }
  }
}
