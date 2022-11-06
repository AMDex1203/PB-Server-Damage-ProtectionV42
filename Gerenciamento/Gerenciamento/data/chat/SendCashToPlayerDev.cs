
// Type: Game.data.chat.SendCashToPlayerDev
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.managers;
using Core.server;
using Game.data.managers;
using Game.data.model;
using Game.data.sync.server_side;
using Game.global.serverpacket;
using System;

namespace Game.data.chat
{
  public static class SendCashToPlayerDev
  {
    public static string SendCashToPlayer(string str)
    {
      string[] strArray = str.Substring(str.IndexOf(" ") + 1).Split(' ');
      long int64 = Convert.ToInt64(strArray[0]);
      int int32 = Convert.ToInt32(strArray[1]);
      Account account = AccountManager.getAccount(int64, 0);
      if (account == null || account._money + int32 > 999999999)
        return Translation.GetLabel("[*]SendCash_Fail4");
      if (!PlayerManager.updateAccountCash(account.player_id, account._money + int32))
        return Translation.GetLabel("GiveCashFail2");
      account._money += int32;
      account.SendPacket((SendPacket) new AUTH_WEB_CASH_PAK(0, account._gp, account._money), false);
      SEND_ITEM_INFO.LoadGoldCash(account);
      return Translation.GetLabel("GiveCashSuccessD", (object) account._money, (object) account.player_name);
    }
  }
}
