
// Type: Game.global.clientpacket.CLAN_MESSAGE_INVITE_REC
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.managers;
using Core.models.account;
using Core.models.enums;
using Core.server;
using Game.data.managers;
using Game.data.model;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
  public class CLAN_MESSAGE_INVITE_REC : ReceiveGamePacket
  {
    private uint erro;
    private int type;
    private long objId;

    public CLAN_MESSAGE_INVITE_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read()
    {
      this.type = (int) this.readC();
      if (this.type == 0)
        this.objId = this.readQ();
      else if (this.type == 1)
        this.objId = (long) this.readD();
      else if (this.type == 2)
        this.objId = (long) this.readUD();
      else
        this.readD();
    }

    public override void run()
    {
      Account player1 = this._client._player;
      if (player1 == null)
        return;
      if (player1.clanId == 0)
        return;
      try
      {
        if (this.type == 0)
        {
          Account account = AccountManager.getAccount(this.objId, true);
          if (account != null)
            this.SendBoxMessage(account, player1.clanId);
          else
            this.erro = 2147483648U;
        }
        else if (this.type == 1)
        {
          Room room = player1._room;
          if (room != null)
          {
            Account playerBySlot = room.getPlayerBySlot((int) this.objId);
            if (playerBySlot != null)
              this.SendBoxMessage(playerBySlot, player1.clanId);
            else
              this.erro = 2147483648U;
          }
          else
            this.erro = 2147483648U;
        }
        else if (this.type == 2)
        {
          Channel channel = player1.getChannel();
          if (channel != null)
          {
            PlayerSession player2 = channel.getPlayer((uint) this.objId);
            long id = player2 != null ? player2._playerId : -1L;
            if (id != -1L && id != this._client.player_id)
            {
              Account account = AccountManager.getAccount(id, true);
              if (account != null)
                this.SendBoxMessage(account, player1.clanId);
              else
                this.erro = 2147483648U;
            }
            else
              this.erro = 2147483648U;
          }
          else
            this.erro = 2147483648U;
        }
        this._client.SendPacket((SendPacket) new CLAN_MESSAGE_INVITE_PAK(this.erro));
      }
      catch (Exception ex)
      {
        Logger.error(ex.ToString());
      }
    }

    private void SendBoxMessage(Account player, int clanId)
    {
      if (MessageManager.getMsgsCount(player.player_id) >= 100)
      {
        this.erro = 2147483648U;
      }
      else
      {
        Message message = this.CreateMessage(clanId, player.player_id, this._client.player_id);
        if (message == null || !player._isOnline)
          return;
        player.SendPacket((SendPacket) new BOX_MESSAGE_RECEIVE_PAK(message), false);
      }
    }

    private Message CreateMessage(int clanId, long owner, long senderId)
    {
      Message msg = new Message(15.0)
      {
        sender_name = ClanManager.getClan(clanId).name,
        clanId = clanId,
        sender_id = senderId,
        type = 5,
        state = 1,
        cB = NoteMessageClan.Invite
      };
      return !MessageManager.CreateMessage(owner, msg) ? (Message) null : msg;
    }
  }
}
