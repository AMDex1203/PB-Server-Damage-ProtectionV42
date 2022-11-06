
// Type: Game.global.clientpacket.CLAN_MEMBER_CONTEXT_REC
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
  public class CLAN_MEMBER_CONTEXT_REC : ReceiveGamePacket
  {
    public CLAN_MEMBER_CONTEXT_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read()
    {
    }

    public override void run()
    {
      try
      {
        Account player = this._client._player;
        if (player == null)
          return;
        int clanId = player.clanId;
        if (clanId == 0)
          this._client.SendPacket((SendPacket) new CLAN_MEMBER_CONTEXT_PAK(-1));
        else
          this._client.SendPacket((SendPacket) new CLAN_MEMBER_CONTEXT_PAK(0, PlayerManager.getClanPlayers(clanId)));
      }
      catch (Exception ex)
      {
        Logger.info(ex.ToString());
      }
    }
  }
}
