
// Type: Game.data.chat.RefillShop
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.managers;
using Core.server;
using Game.data.model;
using Game.global.serverpacket;

namespace Game.data.chat
{
  public static class RefillShop
  {
    public static string SimpleRefill(Account player)
    {
      ShopManager.Reset();
      ShopManager.Load(1);
      Logger.warning(Translation.GetLabel("RefillShopWarn", (object) player.player_name));
      return Translation.GetLabel("RefillShopMsg");
    }

    public static string InstantRefill(Account player)
    {
      ShopManager.Reset();
      ShopManager.Load(1);
      for (int index = 0; index < ShopManager.ShopDataItems.Count; ++index)
      {
        ShopData shopDataItem = ShopManager.ShopDataItems[index];
        player.SendPacket((SendPacket) new SHOP_GET_ITEMS_PAK(shopDataItem, ShopManager.TotalItems));
      }
      for (int index = 0; index < ShopManager.ShopDataGoods.Count; ++index)
      {
        ShopData shopDataGood = ShopManager.ShopDataGoods[index];
        player.SendPacket((SendPacket) new SHOP_GET_GOODS_PAK(shopDataGood, ShopManager.TotalGoods));
      }
      player.SendPacket((SendPacket) new SHOP_GET_REPAIR_PAK());
      player.SendPacket((SendPacket) new SHOP_TEST2_PAK());
      if (player.pc_cafe == 0)
      {
        for (int index = 0; index < ShopManager.ShopDataMt1.Count; ++index)
        {
          ShopData data = ShopManager.ShopDataMt1[index];
          player.SendPacket((SendPacket) new SHOP_GET_MATCHING_PAK(data, ShopManager.TotalMatching1));
        }
      }
      else
      {
        for (int index = 0; index < ShopManager.ShopDataMt2.Count; ++index)
        {
          ShopData data = ShopManager.ShopDataMt2[index];
          player.SendPacket((SendPacket) new SHOP_GET_MATCHING_PAK(data, ShopManager.TotalMatching2));
        }
      }
      player.SendPacket((SendPacket) new SHOP_LIST_PAK());
      Logger.warning(Translation.GetLabel("RefillShopWarn", (object) player.player_name));
      return Translation.GetLabel("RefillShopMsg");
    }
  }
}
