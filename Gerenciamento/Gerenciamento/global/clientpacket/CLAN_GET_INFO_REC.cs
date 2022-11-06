
// Type: Game.global.clientpacket.CLAN_GET_INFO_REC
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.models.account.clan;
using Core.server;
using Game.data.managers;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
  public class CLAN_GET_INFO_REC : ReceiveGamePacket
  {
    private int clanId;
    private int unk;

    public CLAN_GET_INFO_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read()
    {
      this.clanId = this.readD();
      this.unk = (int) this.readC();
    }

    public override void run()
    {
      try
      {
        if (this._client._player == null)
          return;
        Clan clan = ClanManager.getClan(this.clanId);
        if (clan.id <= 0)
          return;
        this._client.SendPacket((SendPacket) new CLAN_DETAIL_INFO_PAK(1, clan));
      }
      catch (Exception ex)
      {
        Logger.info("CLAN_GET_INFO_REC: " + ex.ToString());
      }
    }
  }
}
