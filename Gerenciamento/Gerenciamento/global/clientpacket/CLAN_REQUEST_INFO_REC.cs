
// Type: Game.global.clientpacket.CLAN_REQUEST_INFO_REC
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.managers;
using Core.server;
using Game.data.model;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
  public class CLAN_REQUEST_INFO_REC : ReceiveGamePacket
  {
    private long pId;

    public CLAN_REQUEST_INFO_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read() => this.pId = this.readQ();

    public override void run()
    {
      try
      {
        Account player = this._client._player;
        if (player == null)
          return;
        this._client.SendPacket((SendPacket) new CLAN_REQUEST_INFO_PAK(this.pId, PlayerManager.getRequestText(player.clanId, this.pId)));
      }
      catch (Exception ex)
      {
        Logger.info(ex.ToString());
      }
    }
  }
}
