
// Type: Game.global.clientpacket.CLAN_CREATE_REC
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.managers;
using Core.models.account.clan;
using Core.server;
using Game.data.managers;
using Game.data.model;
using Game.data.sync.server_side;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
  public class CLAN_CREATE_REC : ReceiveGamePacket
  {
    private int NameLength;
    private int InfoLength;
    private int AzitLength;
    private uint erro;
    private string clanName;
    private string clanInfo;
    private string clanAzit;

    public CLAN_CREATE_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read()
    {
      this.NameLength = (int) this.readC();
      this.InfoLength = (int) this.readC();
      this.AzitLength = (int) this.readC();
      this.clanName = this.readS(this.NameLength);
      this.clanInfo = this.readS(this.InfoLength);
      this.clanAzit = this.readS(this.AzitLength);
      this.readD();
    }

    public override void run()
    {
      try
      {
        Account player = this._client._player;
        if (player == null)
          return;
        Clan clan = new Clan()
        {
          name = this.clanName,
          informations = this.clanInfo,
          logo = 0,
          ownerId = player.player_id,
          creationDate = int.Parse(DateTime.Now.ToString("yyyyMMdd"))
        };
        if (player.clanId > 0 || PlayerManager.getRequestClanId(player.player_id) > 0)
          this.erro = 2147487836U;
        else if (0 > player._gp - ConfigGS.minCreateGold || ConfigGS.minCreateRank > player._rank)
        {
          this.erro = 2147487818U;
        }
        else
        {
          if (ClanManager.isClanNameExist(clan.name))
          {
            this.erro = 2147487834U;
            return;
          }
          if (ClanManager._clans.Count > ConfigGS.maxActiveClans)
            this.erro = 2147487829U;
          else if (PlayerManager.CreateClan(out clan.id, clan.name, clan.ownerId, clan.informations, clan.creationDate) && PlayerManager.updateAccountGold(player.player_id, player._gp - ConfigGS.minCreateGold))
          {
            clan.BestPlayers.SetDefault();
            player.clanDate = clan.creationDate;
            if (ComDiv.updateDB("accounts", "player_id", (object) player.player_id, new string[3]
            {
              "clanaccess",
              "clandate",
              "clan_id"
            }, (object) 1, (object) clan.creationDate, (object) clan.id))
            {
              if (clan.id > 0)
              {
                player.clanId = clan.id;
                player.clanAccess = 1;
                ClanManager.AddClan(clan);
                SEND_CLAN_INFOS.Load(clan, 0);
                player._gp -= ConfigGS.minCreateGold;
              }
              else
                this.erro = 2147487819U;
            }
            else
              this.erro = 2147487816U;
          }
          else
            this.erro = 2147487816U;
        }
        this._client.SendPacket((SendPacket) new CLAN_CREATE_PAK(this.erro, clan, player));
      }
      catch (Exception ex)
      {
        Logger.warning("[CLAN_CREATE_REC] " + ex.ToString());
      }
    }
  }
}
