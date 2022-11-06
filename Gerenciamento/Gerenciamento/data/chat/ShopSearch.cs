
// Type: Game.data.chat.ShopSearch
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.managers;
using Core.models.shop;
using Core.server;
using Game.data.model;
using Game.global.serverpacket;

namespace Game.data.chat
{
  public static class ShopSearch
  {
    public static string SearchGoods(string str, Account player)
    {
      string str1 = str.Substring(6);
      int num = 0;
      string msg = Translation.GetLabel("SearchGoodTitle");
      foreach (GoodItem shopBuyable in ShopManager.ShopBuyableList)
      {
        if (shopBuyable._item._name.Contains(str1))
        {
          msg = msg + "\n" + Translation.GetLabel("SearchGoodInfo", (object) shopBuyable.id, (object) shopBuyable._item._name);
          if (++num >= 15)
            break;
        }
      }
      player.SendPacket((SendPacket) new SERVER_MESSAGE_ANNOUNCE_PAK(msg));
      return Translation.GetLabel("SearchGoodSuccess", (object) num);
    }
  }
}
