
// Type: Game.global.clientpacket.CLAN_CHECK_CREATE_INVITE_REC
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.models.account.clan;
using Core.server;
using Game.data.managers;
using Game.data.model;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
  public class CLAN_CHECK_CREATE_INVITE_REC : ReceiveGamePacket
  {
    private int clanId;
    private uint erro;

    public CLAN_CHECK_CREATE_INVITE_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read() => this.clanId = this.readD();

    public override void run()
    {
      try
      {
        Account player = this._client._player;
        if (player == null)
          return;
        Clan clan = ClanManager.getClan(this.clanId);
        if (clan.id == 0)
          this.erro = 2147483648U;
        else if (clan.limitRankId > player._rank)
          this.erro = 2147487867U;
        this._client.SendPacket((SendPacket) new CLAN_CHECK_CREATE_INVITE_PAK(this.erro));
      }
      catch (Exception ex)
      {
        Logger.info("CLAN_CHECK_CREATE_INVITE_REC: " + ex.ToString());
      }
    }
  }
}
