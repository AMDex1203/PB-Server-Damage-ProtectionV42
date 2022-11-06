
// Type: Game.global.clientpacket.CLAN_REPLACE_NOTICE_REC
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.models.account.clan;
using Core.server;
using Game.data.managers;
using Game.data.model;
using Game.global.serverpacket;

namespace Game.global.clientpacket
{
  public class CLAN_REPLACE_NOTICE_REC : ReceiveGamePacket
  {
    private string clan_news;
    private uint erro;

    public CLAN_REPLACE_NOTICE_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read() => this.clan_news = this.readS((int) this.readC());

    public override void run()
    {
      try
      {
        Account player = this._client._player;
        if (player != null)
        {
          Clan clan = ClanManager.getClan(player.clanId);
          if (clan.id > 0 && clan.notice != this.clan_news && (clan.ownerId == this._client.player_id || player.clanAccess >= 1 && player.clanAccess <= 2))
          {
            if (ComDiv.updateDB("clan_data", "clan_news", (object) this.clan_news, "clan_id", (object) clan.id))
              clan.notice = this.clan_news;
            else
              this.erro = 2147487859U;
          }
          else
            this.erro = 2147487835U;
        }
        else
          this.erro = 2147487835U;
      }
      catch
      {
        this.erro = 2147487859U;
      }
      this._client.SendPacket((SendPacket) new CLAN_REPLACE_NOTICE_PAK(this.erro));
    }
  }
}
