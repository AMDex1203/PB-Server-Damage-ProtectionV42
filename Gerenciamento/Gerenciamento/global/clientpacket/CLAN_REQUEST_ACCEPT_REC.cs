
// Type: Game.global.clientpacket.CLAN_REQUEST_ACCEPT_REC
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
  public class CLAN_REQUEST_ACCEPT_REC : ReceiveGamePacket
  {
    private int result;

    public CLAN_REQUEST_ACCEPT_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read()
    {
      Account player = this._client._player;
      if (player == null)
        return;
      Clan clan = ClanManager.getClan(player.clanId);
      if (clan.id > 0 && (player.clanAccess >= 1 && player.clanAccess <= 2 || player.player_id == clan.ownerId))
      {
        List<Account> clanPlayers = ClanManager.getClanPlayers(clan.id, -1L, true);
        if (clanPlayers.Count >= clan.maxPlayers)
        {
          this.result = -1;
        }
        else
        {
          int num = (int) this.readC();
          for (int index = 0; index < num; ++index)
          {
            Account account = AccountManager.getAccount(this.readQ(), 0);
            if (account != null && clanPlayers.Count < clan.maxPlayers && (account.clanId == 0 && PlayerManager.getRequestClanId(account.player_id) > 0))
            {
              using (CLAN_MEMBER_INFO_INSERT_PAK memberInfoInsertPak = new CLAN_MEMBER_INFO_INSERT_PAK(account))
                ClanManager.SendPacket((SendPacket) memberInfoInsertPak, clanPlayers);
              account.clanId = player.clanId;
              account.clanDate = int.Parse(DateTime.Now.ToString("yyyyMMdd"));
              account.clanAccess = 3;
              SEND_CLAN_INFOS.Load(account, (Account) null, 3);
              ComDiv.updateDB("accounts", "player_id", (object) account.player_id, new string[3]
              {
                "clanaccess",
                "clan_id",
                "clandate"
              }, (object) account.clanAccess, (object) account.clanId, (object) account.clanDate);
              PlayerManager.DeleteInviteDb(player.clanId, account.player_id);
              if (account._isOnline)
              {
                account.SendPacket((SendPacket) new CLAN_GET_CLAN_MEMBERS_PAK(clanPlayers), false);
                account._room?.SendPacketToPlayers((SendPacket) new ROOM_GET_SLOTONEINFO_PAK(account, clan));
                account.SendPacket((SendPacket) new CLAN_NEW_INFOS_PAK(clan, clanPlayers.Count + 1), false);
              }
              if (MessageManager.getMsgsCount(account.player_id) < 100)
              {
                Message message = this.CreateMessage(clan, account.player_id, this._client.player_id);
                if (message != null && account._isOnline)
                  account.SendPacket((SendPacket) new BOX_MESSAGE_RECEIVE_PAK(message), false);
              }
              ++this.result;
              clanPlayers.Add(account);
            }
          }
        }
      }
      else
        this.result = -1;
    }

    public override void run()
    {
      try
      {
        this._client.SendPacket((SendPacket) new CLAN_REQUEST_ACCEPT_PAK((uint) this.result));
      }
      catch (Exception ex)
      {
        Logger.info("CLAN_INVITE_ACCEPT_REC " + ex.ToString());
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
        cB = NoteMessageClan.InviteAccept
      };
      return !MessageManager.CreateMessage(owner, msg) ? (Message) null : msg;
    }
  }
}
