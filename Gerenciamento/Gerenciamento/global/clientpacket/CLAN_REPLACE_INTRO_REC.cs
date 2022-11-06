
// Type: Game.global.clientpacket.CLAN_REPLACE_INTRO_REC
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
  public class CLAN_REPLACE_INTRO_REC : ReceiveGamePacket
  {
    private string clan_info;
    private uint erro;

    public CLAN_REPLACE_INTRO_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read() => this.clan_info = this.readS((int) this.readC());

    public override void run()
    {
      try
      {
        Account player = this._client._player;
        if (player != null)
        {
          Clan clan = ClanManager.getClan(player.clanId);
          if (clan.id > 0 && clan.informations != this.clan_info && (clan.ownerId == this._client.player_id || player.clanAccess >= 1 && player.clanAccess <= 2))
          {
            if (ComDiv.updateDB("clan_data", "clan_info", (object) this.clan_info, "clan_id", (object) clan.id))
              clan.informations = this.clan_info;
            else
              this.erro = 2147487860U;
          }
          else
            this.erro = 2147487835U;
        }
        else
          this.erro = 2147487835U;
      }
      catch
      {
        this.erro = 2147487860U;
      }
      this._client.SendPacket((SendPacket) new CLAN_REPLACE_INTRO_PAK(this.erro));
    }
  }
}
