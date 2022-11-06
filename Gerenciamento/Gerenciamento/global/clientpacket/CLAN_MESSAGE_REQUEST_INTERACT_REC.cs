
// Type: Game.global.clientpacket.CLAN_MESSAGE_REQUEST_INTERACT_REC
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
using System.Collections.Generic;

namespace Game.global.clientpacket
{
  public class CLAN_MESSAGE_REQUEST_INTERACT_REC : ReceiveGamePacket
  {
    private int clanId;
    private int type;

    public CLAN_MESSAGE_REQUEST_INTERACT_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read()
    {
      this.clanId = this.readD();
      this.readD();
      this.type = (int) this.readC();
    }

    public override void run()
    {
      Account player = this._client._player;
      if (player == null || player.player_name.Length == 0)
        return;
      Clan clan = ClanManager.getClan(this.clanId);
      List<Account> clanPlayers = ClanManager.getClanPlayers(this.clanId, -1L, true);
      if (clan.id == 0)
        this._client.SendPacket((SendPacket) new CLAN_MESSAGE_REQUEST_ACCEPT_PAK(2147487835U));
      else if (player.clanId > 0)
        this._client.SendPacket((SendPacket) new CLAN_MESSAGE_REQUEST_ACCEPT_PAK(2147487832U));
      else if (clan.maxPlayers <= clanPlayers.Count)
      {
        this._client.SendPacket((SendPacket) new CLAN_MESSAGE_REQUEST_ACCEPT_PAK(2147487830U));
      }
      else
      {
        if (this.type != 0 && this.type != 1)
          return;
        try
        {
          uint erro = 0;
          Account account = AccountManager.getAccount(clan.ownerId, 0);
          if (account != null)
          {
            if (MessageManager.getMsgsCount(clan.ownerId) < 100)
            {
              Message message = this.CreateMessage(clan, player.player_name, this._client.player_id);
              if (message != null && account._isOnline)
                account.SendPacket((SendPacket) new BOX_MESSAGE_RECEIVE_PAK(message), false);
            }
            if (this.type == 1)
            {
              int num = int.Parse(DateTime.Now.ToString("yyyyMMdd"));
              if (ComDiv.updateDB("accounts", "player_id", (object) player.player_id, new string[3]
              {
                "clan_id",
                "clanaccess",
                "clandate"
              }, (object) clan.id, (object) 3, (object) num))
              {
                using (CLAN_MEMBER_INFO_INSERT_PAK memberInfoInsertPak = new CLAN_MEMBER_INFO_INSERT_PAK(player))
                  ClanManager.SendPacket((SendPacket) memberInfoInsertPak, clanPlayers);
                player.clanId = clan.id;
                player.clanDate = num;
                player.clanAccess = 3;
                this._client.SendPacket((SendPacket) new CLAN_GET_CLAN_MEMBERS_PAK(clanPlayers));
                player._room?.SendPacketToPlayers((SendPacket) new ROOM_GET_SLOTONEINFO_PAK(player, clan));
                this._client.SendPacket((SendPacket) new CLAN_NEW_INFOS_PAK(clan, account, clanPlayers.Count + 1));
              }
              else
                erro = 2147483648U;
            }
          }
          else
            erro = 2147483648U;
          this._client.SendPacket((SendPacket) new BOX_MESSAGE_SEND_PAK(erro));
        }
        catch (Exception ex)
        {
          Logger.error(ex.ToString());
        }
      }
    }

    private Message CreateMessage(Clan clan, string player, long senderId)
    {
      Message msg = new Message(15.0)
      {
        sender_name = clan.name,
        sender_id = senderId,
        clanId = clan.id,
        type = 4,
        text = player,
        state = 1,
        cB = this.type == 0 ? NoteMessageClan.JoinDenial : NoteMessageClan.JoinAccept
      };
      return !MessageManager.CreateMessage(clan.ownerId, msg) ? (Message) null : msg;
    }
  }
}
