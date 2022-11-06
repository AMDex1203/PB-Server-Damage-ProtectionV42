
// Type: Game.global.clientpacket.CLAN_REQUEST_DENIAL_REC
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
  public class CLAN_REQUEST_DENIAL_REC : ReceiveGamePacket
  {
    private int result;

    public CLAN_REQUEST_DENIAL_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read()
    {
      Account player = this._client._player;
      if (player == null)
        return;
      Clan clan = ClanManager.getClan(player.clanId);
      if (clan.id <= 0 || (player.clanAccess < 1 || player.clanAccess > 2) && clan.ownerId != player.player_id)
        return;
      int num1 = (int) this.readC();
      for (int index = 0; index < num1; ++index)
      {
        long num2 = this.readQ();
        if (PlayerManager.DeleteInviteDb(clan.id, num2))
        {
          if (MessageManager.getMsgsCount(num2) < 100)
          {
            Message message = this.CreateMessage(clan, num2, player.player_id);
            if (message != null)
            {
              Account account = AccountManager.getAccount(num2, 0);
              if (account != null && account._isOnline)
                account.SendPacket((SendPacket) new BOX_MESSAGE_RECEIVE_PAK(message), false);
            }
          }
          ++this.result;
        }
      }
    }

    public override void run()
    {
      try
      {
        this._client.SendPacket((SendPacket) new CLAN_REQUEST_DENIAL_PAK(this.result));
      }
      catch (Exception ex)
      {
        Logger.info("[CLAN_REQUEST_DENIAL_REC] " + ex.ToString());
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
        cB = NoteMessageClan.InviteDenial
      };
      return !MessageManager.CreateMessage(owner, msg) ? (Message) null : msg;
    }
  }
}
