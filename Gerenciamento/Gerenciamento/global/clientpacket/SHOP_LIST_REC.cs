
// Type: Game.global.clientpacket.SHOP_LIST_REC
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.managers;
using Core.server;
using Game.data.model;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
  public class SHOP_LIST_REC : ReceiveGamePacket
  {
    private int erro;

    public SHOP_LIST_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read() => this.erro = this.readD();

    public override void run()
    {
      try
      {
        Account player = this._client._player;
        if (player == null)
          return;
        if (!player.LoadedShop && this.erro <= 0)
        {
          player.LoadedShop = true;
          for (int index = 0; index < ShopManager.ShopDataItems.Count; ++index)
            this._client.SendPacket((SendPacket) new SHOP_GET_ITEMS_PAK(ShopManager.ShopDataItems[index], ShopManager.TotalItems));
          for (int index = 0; index < ShopManager.ShopDataGoods.Count; ++index)
            this._client.SendPacket((SendPacket) new SHOP_GET_GOODS_PAK(ShopManager.ShopDataGoods[index], ShopManager.TotalGoods));
          this._client.SendPacket((SendPacket) new SHOP_GET_REPAIR_PAK());
          this._client.SendPacket((SendPacket) new SHOP_TEST2_PAK());
          if (player.pc_cafe == 0)
          {
            for (int index = 0; index < ShopManager.ShopDataMt1.Count; ++index)
              this._client.SendPacket((SendPacket) new SHOP_GET_MATCHING_PAK(ShopManager.ShopDataMt1[index], ShopManager.TotalMatching1));
          }
          else
          {
            for (int index = 0; index < ShopManager.ShopDataMt2.Count; ++index)
              this._client.SendPacket((SendPacket) new SHOP_GET_MATCHING_PAK(ShopManager.ShopDataMt2[index], ShopManager.TotalMatching2));
          }
        }
        this._client.SendPacket((SendPacket) new SHOP_LIST_PAK());
      }
      catch (Exception ex)
      {
        Logger.info("SHOP_LIST_REC: " + ex.ToString());
      }
    }
  }
}
