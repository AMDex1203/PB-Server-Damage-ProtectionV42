
// Type: Game.global.clientpacket.CLAN_DEMOTE_KICK_REC
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
using Game.data.sync.server_side;
using Game.global.serverpacket;
using System;
using System.Collections.Generic;

namespace Game.global.clientpacket
{
  public class CLAN_DEMOTE_KICK_REC : ReceiveGamePacket
  {
    private uint result;

    public CLAN_DEMOTE_KICK_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read()
    {
      Account player = this._client._player;
      if (player == null)
        return;
      Clan clan = ClanManager.getClan(player.clanId);
      if (clan.id == 0 || (player.clanAccess < 1 || player.clanAccess > 2) && clan.ownerId != this._client.player_id)
      {
        this.result = 2147487833U;
      }
      else
      {
        List<Account> clanPlayers = ClanManager.getClanPlayers(clan.id, -1L, true);
        int num = (int) this.readC();
        for (int index = 0; index < num; ++index)
        {
          Account account = AccountManager.getAccount(this.readQ(), 0);
          if (account != null && account.clanId == clan.id && account._match == null)
          {
            if (ComDiv.updateDB("accounts", "player_id", (object) account.player_id, new string[4]
            {
              "clan_id",
              "clanaccess",
              "clan_game_pt",
              "clan_wins_pt"
            }, (object) 0, (object) 0, (object) 0, (object) 0))
            {
              using (CLAN_MEMBER_INFO_DELETE_PAK memberInfoDeletePak = new CLAN_MEMBER_INFO_DELETE_PAK(account.player_id))
                ClanManager.SendPacket((SendPacket) memberInfoDeletePak, clanPlayers, account.player_id);
              account.clanId = 0;
              account.clanAccess = 0;
              SEND_CLAN_INFOS.Load(account, (Account) null, 0);
              if (MessageManager.getMsgsCount(account.player_id) < 100)
              {
                Message message = this.CreateMessage(clan, account.player_id, this._client.player_id);
                if (message != null && account._isOnline)
                  account.SendPacket((SendPacket) new BOX_MESSAGE_RECEIVE_PAK(message), false);
              }
              if (account._isOnline)
                account.SendPacket((SendPacket) new CLAN_PRIVILEGES_KICK_PAK(), false);
              ++this.result;
              clanPlayers.Remove(account);
              continue;
            }
          }
          this.result = 2147487833U;
          break;
        }
      }
    }

    public override void run()
    {
      try
      {
        if (this._client == null)
          return;
        this._client.SendPacket((SendPacket) new CLAN_DEPORTATION_PAK(this.result));
      }
      catch (Exception ex)
      {
        Logger.info("[CLAN_DEMOTE_KICK_REC] " + ex.ToString());
      }
    }

    private Message CreateMessage(Clan clan, long owner, long senderId)
    {
      Message msg = new Message(15.0)
      {
        sender_name = clan.name,
        sender_id = senderId,
        clanId = clan.id,
        type = 4,
        state = 1,
        cB = NoteMessageClan.Deportation
      };
      return !MessageManager.CreateMessage(owner, msg) ? (Message) null : msg;
    }
  }
}
