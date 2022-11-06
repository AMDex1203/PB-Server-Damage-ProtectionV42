
// Type: Game.global.clientpacket.CLAN_PLAYER_LEAVE_REC
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.managers;
using Core.models.account;
using Core.models.account.clan;
using Core.models.enums;
using Core.server;
using Game.data.managers;
using Game.data.model;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
  public class CLAN_PLAYER_LEAVE_REC : ReceiveGamePacket
  {
    private uint erro;

    public CLAN_PLAYER_LEAVE_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read()
    {
    }

    public override void run()
    {
      try
      {
        if (this._client == null)
          return;
        Account player = this._client._player;
        try
        {
          if (player != null && player.clanId > 0)
          {
            Clan clan = ClanManager.getClan(player.clanId);
            if (clan.id > 0 && clan.ownerId != player.player_id)
            {
              if (ComDiv.updateDB("accounts", "player_id", (object) player.player_id, new string[4]
              {
                "clan_id",
                "clanaccess",
                "clan_game_pt",
                "clan_wins_pt"
              }, (object) 0, (object) 0, (object) 0, (object) 0))
              {
                using (CLAN_MEMBER_INFO_DELETE_PAK memberInfoDeletePak = new CLAN_MEMBER_INFO_DELETE_PAK(player.player_id))
                  ClanManager.SendPacket((SendPacket) memberInfoDeletePak, player.clanId, player.player_id, true, true);
                long ownerId = clan.ownerId;
                if (MessageManager.getMsgsCount(ownerId) < 100)
                {
                  Message message = this.CreateMessage(clan, player);
                  if (message != null)
                  {
                    Account account = AccountManager.getAccount(ownerId, 0);
                    if (account != null && account._isOnline)
                      account.SendPacket((SendPacket) new BOX_MESSAGE_RECEIVE_PAK(message), false);
                  }
                }
                player.clanId = 0;
                player.clanAccess = 0;
              }
              else
                this.erro = 2147487851U;
            }
            else
              this.erro = 2147487838U;
          }
          else
            this.erro = 2147487835U;
        }
        catch
        {
          this.erro = 2147487851U;
        }
        this._client.SendPacket((SendPacket) new CLAN_PLAYER_LEAVE_PAK(this.erro));
      }
      catch (Exception ex)
      {
        Logger.info("[CLAN_PLAYER_LEAVE_REC] " + ex.ToString());
      }
    }

    private Message CreateMessage(Clan clan, Account sender)
    {
      Message msg = new Message(15.0)
      {
        sender_name = clan.name,
        sender_id = sender.player_id,
        clanId = clan.id,
        type = 4,
        text = sender.player_name,
        state = 1,
        cB = NoteMessageClan.Secession
      };
      return !MessageManager.CreateMessage(clan.ownerId, msg) ? (Message) null : msg;
    }
  }
}
