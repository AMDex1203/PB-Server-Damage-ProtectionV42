
// Type: Game.global.clientpacket.BOX_MESSAGE_CREATE_REC
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.managers;
using Core.models.account;
using Core.server;
using Game.data.managers;
using Game.data.model;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
  public class BOX_MESSAGE_CREATE_REC : ReceiveGamePacket
  {
    private int nameLength;
    private int textLength;
    private string name;
    private string text;
    private uint erro;

    public BOX_MESSAGE_CREATE_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read()
    {
      this.nameLength = (int) this.readC();
      this.textLength = (int) this.readC();
      this.name = this.readS(this.nameLength);
      this.text = this.readS(this.textLength);
    }

    public override void run()
    {
      try
      {
        if (this.text.Length > 120)
          return;
        Account player = this._client._player;
        if (player == null || player.player_name.Length == 0 || player.player_name == this.name)
          return;
        Account account = AccountManager.getAccount(this.name, 1, 0);
        if (account != null)
        {
          if (MessageManager.getMsgsCount(account.player_id) >= 100)
          {
            this.erro = 2147487871U;
          }
          else
          {
            Message message = this.CreateMessage(player.player_name, account.player_id, this._client.player_id);
            if (message != null)
              account.SendPacket((SendPacket) new BOX_MESSAGE_RECEIVE_PAK(message), false);
          }
        }
        else
          this.erro = 2147487870U;
        this._client.SendPacket((SendPacket) new BOX_MESSAGE_CREATE_PAK(this.erro));
      }
      catch (Exception ex)
      {
        Logger.info("[BOX_MESSAGE_CREATE_REC] " + ex.ToString());
      }
    }

    private Message CreateMessage(string senderName, long owner, long senderId)
    {
      Message msg = new Message(15.0)
      {
        sender_name = senderName,
        sender_id = senderId,
        text = this.text,
        state = 1
      };
      if (MessageManager.CreateMessage(owner, msg))
        return msg;
      this.erro = 2147483648U;
      return (Message) null;
    }
  }
}
