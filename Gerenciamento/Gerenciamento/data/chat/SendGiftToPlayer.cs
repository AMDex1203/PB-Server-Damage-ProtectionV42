
// Type: Game.data.chat.SendGiftToPlayer
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.managers;
using Core.models.account;
using Core.models.shop;
using Core.server;
using Game.data.managers;
using Game.data.model;
using Game.global.serverpacket;
using System;

namespace Game.data.chat
{
  public static class SendGiftToPlayer
  {
    public static string SendGiftById(string str)
    {
      if (!GameManager.Config.GiftSystem)
        return Translation.GetLabel("SendGift_SystemOffline");
      string[] strArray = str.Substring(str.IndexOf(" ") + 1).Split(' ');
      long int64 = Convert.ToInt64(strArray[0]);
      int int32 = Convert.ToInt32(strArray[1]);
      Account account = AccountManager.getAccount(int64, 0);
      if (account == null)
        return Translation.GetLabel("SendGift_Fail4");
      GoodItem good = ShopManager.getGood(int32);
      if (good != null && (good.visibility == 0 || good.visibility == 4))
      {
        Message message = new Message(30.0)
        {
          sender_id = (long) int32,
          state = 0,
          type = 2
        };
        if (!MessageManager.CreateMessage(int64, message))
          return Translation.GetLabel("SendGift_Fail1");
        account.SendPacket((SendPacket) new BOX_MESSAGE_GIFT_RECEIVE_PAK(message), false);
        return Translation.GetLabel("SendGift_Success", (object) good._item._name, (object) account.player_name);
      }
      if (good == null)
        return Translation.GetLabel("SendGift_Fail2");
      return Translation.GetLabel("SendGift_Fail3", (object) good._item._name);
    }
  }
}
