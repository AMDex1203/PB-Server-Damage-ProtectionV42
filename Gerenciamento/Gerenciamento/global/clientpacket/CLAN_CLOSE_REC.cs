
// Type: Game.global.clientpacket.CLAN_CLOSE_REC
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.models.account.clan;
using Core.server;
using Game.data.managers;
using Game.data.model;
using Game.data.sync.server_side;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
  public class CLAN_CLOSE_REC : ReceiveGamePacket
  {
    private uint erro;

    public CLAN_CLOSE_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read()
    {
    }

    public override void run()
    {
      try
      {
        Account player = this._client._player;
        if (player != null)
        {
          Clan clan = ClanManager.getClan(player.clanId);
          if (clan.id > 0 && clan.ownerId == this._client.player_id && ComDiv.deleteDB("clan_data", "clan_id", (object) clan.id))
          {
            if (ComDiv.updateDB("accounts", "player_id", (object) player.player_id, new string[4]
            {
              "clan_id",
              "clanaccess",
              "clan_game_pt",
              "clan_wins_pt"
            }, (object) 0, (object) 0, (object) 0, (object) 0) && ClanManager.RemoveClan(clan))
            {
              player.clanId = 0;
              player.clanAccess = 0;
              SEND_CLAN_INFOS.Load(clan, 1);
              goto label_6;
            }
          }
          this.erro = 2147487850U;
        }
        else
          this.erro = 2147487850U;
label_6:
        this._client.SendPacket((SendPacket) new CLAN_CLOSE_PAK(this.erro));
      }
      catch (Exception ex)
      {
        Logger.info("[CLAN_CLOSE_REC] " + ex.ToString());
      }
    }
  }
}
