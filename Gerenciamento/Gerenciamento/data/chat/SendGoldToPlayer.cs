
// Type: Game.data.chat.SendGoldToPlayer
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.managers;
using Core.models.account.players;
using Core.server;
using Game.data.managers;
using Game.data.model;
using Game.data.sync.server_side;
using Game.global.serverpacket;
using System.Collections.Generic;

namespace Game.data.chat
{
  public static class SendGoldToPlayer
  {
    public static string SendByNick(string str) => SendGoldToPlayer.BaseGiveGold(AccountManager.getAccount(str.Substring(4), 1, 0));

    public static string SendById(string str) => SendGoldToPlayer.BaseGiveGold(AccountManager.getAccount(long.Parse(str.Substring(4)), 0));

    private static string BaseGiveGold(Account pR)
    {
      if (pR == null)
        return Translation.GetLabel("GiveGoldFail");
      if (!PlayerManager.updateAccountGold(pR.player_id, pR._gp + 10000))
        return Translation.GetLabel("GiveGoldFail2");
      pR._gp += 10000;
      pR.SendPacket((SendPacket) new AUTH_WEB_CASH_PAK(0, pR._gp, pR._money), false);
      SEND_ITEM_INFO.LoadGoldCash(pR);
      return Translation.GetLabel("GiveGoldSuccess", (object) pR.player_name);
    }

    public static string SendVipAwards(
      int playerId,
      int gold,
      int pcCafe,
      int money,
      List<ItemsModel> items)
    {
      Account account = AccountManager.getAccount((long) playerId, 0);
      if (account == null || account._gp + gold > 999999999 || account._money + money > 999999999)
        return Translation.GetLabel("[*]SendCash_Fail4");
      if (!PlayerManager.updateAccountCashing(account.player_id, account._gp + gold, account._money + money))
        return Translation.GetLabel("GiveCashFail2");
      account._gp += gold;
      account._money += money;
      account.SendPacket((SendPacket) new AUTH_WEB_CASH_PAK(0, account._gp, account._money), false);
      SEND_ITEM_INFO.LoadGoldCash(account);
      if (items.Count > 0)
        account.SendPacket((SendPacket) new INVENTORY_ITEM_CREATE_PAK(1, account, items));
      if (pcCafe < 0 || pcCafe > 2)
        return Translation.GetLabel("[*]SetVip_Fail4");
      if (PlayerManager.updateAccountVip(account.player_id, pcCafe))
      {
        try
        {
          account.SendPacket((SendPacket) new AUTH_ACCOUNT_KICK_PAK(2), false);
          account.Close(1000, true);
          return Translation.GetLabel("ReceiveVipAwards", (object) account.player_name, (object) money, (object) gold, (object) items.Count, (object) pcCafe);
        }
        catch
        {
          return Translation.GetLabel("SetVipF");
        }
      }
      else
        return Translation.GetLabel("ReceiveVipAwards", (object) account.player_name, (object) money, (object) gold, (object) items.Count, (object) pcCafe);
    }
  }
}
