
// Type: Game.global.clientpacket.AUTH_FIND_USER_REC
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.server;
using Game.data.managers;
using Game.data.model;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
  public class AUTH_FIND_USER_REC : ReceiveGamePacket
  {
    private string name;

    public AUTH_FIND_USER_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read() => this.name = this.readS(33);

    public override void run()
    {
      try
      {
        Account player = this._client._player;
        if (player == null || player.player_name.Length == 0 || player.player_name == this.name)
          return;
        Account account = AccountManager.getAccount(this.name, 1, 0);
        this._client.SendPacket((SendPacket) new AUTH_FIND_USER_PAK(account == null ? 2147489795U : (!account._isOnline ? 2147489796U : 0U), account));
      }
      catch (Exception ex)
      {
        Logger.info(ex.ToString());
      }
    }
  }
}
