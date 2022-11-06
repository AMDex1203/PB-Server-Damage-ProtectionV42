
// Type: Game.global.clientpacket.SHOP_BUY_ITEM_REC
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.managers;
using Core.models.shop;
using Core.server;
using Game.data.model;
using Game.global.serverpacket;
using System;
using System.Collections.Generic;

namespace Game.global.clientpacket
{
  public class SHOP_BUY_ITEM_REC : ReceiveGamePacket
  {
    private List<CartGoods> ShopCart = new List<CartGoods>();

    public SHOP_BUY_ITEM_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read()
    {
      int num = (int) this.readC();
      for (int index = 0; index < num; ++index)
        this.ShopCart.Add(new CartGoods()
        {
          GoodId = this.readD(),
          BuyType = (int) this.readC()
        });
    }

    public override void run()
    {
      try
      {
        Account player = this._client._player;
        if (player == null || player.player_name.Length == 0)
          this._client.SendPacket((SendPacket) new SHOP_BUY_PAK(2147487767U));
        else if (player._inventory._items.Count >= 500)
        {
          this._client.SendPacket((SendPacket) new SHOP_BUY_PAK(2147487929U));
        }
        else
        {
          int GoldPrice;
          int CashPrice;
          List<GoodItem> goods = ShopManager.getGoods(this.ShopCart, out GoldPrice, out CashPrice);
          if (goods.Count == 0)
            this._client.SendPacket((SendPacket) new SHOP_BUY_PAK(2147487767U));
          else if (0 > player._gp - GoldPrice || 0 > player._money - CashPrice)
            this._client.SendPacket((SendPacket) new SHOP_BUY_PAK(2147487768U));
          else if (PlayerManager.updateAccountCashing(player.player_id, player._gp - GoldPrice, player._money - CashPrice))
          {
            player._gp -= GoldPrice;
            player._money -= CashPrice;
            this._client.SendPacket((SendPacket) new SHOP_BUY_PAK(1U, goods, player));
          }
          else
            this._client.SendPacket((SendPacket) new SHOP_BUY_PAK(2147487769U));
        }
      }
      catch (Exception ex)
      {
        Logger.info("SHOP_BUY_ITEM_REC: " + ex.ToString());
      }
    }
  }
}
