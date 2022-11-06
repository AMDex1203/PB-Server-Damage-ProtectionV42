
// Type: Game.global.clientpacket.BOX_MESSAGE_GIFT_TAKE_REC
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.managers;
using Core.models.account;
using Core.models.shop;
using Core.server;
using Game.data.model;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
  public class BOX_MESSAGE_GIFT_TAKE_REC : ReceiveGamePacket
  {
    private int msgId;

    public BOX_MESSAGE_GIFT_TAKE_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read() => this.msgId = this.readD();

    public override void run()
    {
      try
      {
        if (this._client == null)
          return;
        Account player = this._client._player;
        if (player == null)
          return;
        if (player._inventory._items.Count >= 500)
        {
          this._client.SendPacket((SendPacket) new INVENTORY_ITEM_EQUIP_PAK(2147487785U));
          this._client.SendPacket((SendPacket) new BOX_MESSAGE_GIFT_TAKE_PAK(2147483648U));
        }
        else
        {
          Message message = MessageManager.getMessage(this.msgId, player.player_id);
          if (message != null && message.type == 2)
          {
            GoodItem good = ShopManager.getGood((int) message.sender_id);
            if (good == null)
              return;
            Logger.warning("Presente Recebido. [Good: " + good.id.ToString() + "; Item: " + good._item._id.ToString() + "; Name: " + good._item._name + "]");
            this._client.SendPacket((SendPacket) new BOX_MESSAGE_GIFT_TAKE_PAK(1U, good._item, player));
            MessageManager.DeleteMessage(this.msgId, player.player_id);
          }
          else
            this._client.SendPacket((SendPacket) new BOX_MESSAGE_GIFT_TAKE_PAK(2147483648U));
        }
      }
      catch (Exception ex)
      {
        Logger.info("[BOX_MESSAGE_GIFT_TAKE_REC] " + ex.ToString());
      }
    }
  }
}
