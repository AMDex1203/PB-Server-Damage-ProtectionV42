
// Type: Game.data.chat.SendCashToPlayer
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

namespace Game.data.chat
{
  public static class SendCashToPlayer
  {
    public static string SendByNick(string str) => SendCashToPlayer.BaseGiveCash(AccountManager.getAccount(str.Substring(4), 1, 0));

    public static string SendById(string str) => SendCashToPlayer.BaseGiveCash(AccountManager.getAccount(long.Parse(str.Substring(4)), 0));

    private static string BaseGiveCash(Account pR)
    {
      if (pR == null)
        return Translation.GetLabel("GiveCashFail");
      if (!PlayerManager.updateAccountCash(pR.player_id, pR._money + 500))
        return Translation.GetLabel("GiveCashFail2");
      pR._money += 500;
      pR.SendPacket((SendPacket) new AUTH_WEB_CASH_PAK(0, pR._gp, pR._money), false);
      SEND_ITEM_INFO.LoadGoldCash(pR);
      return Translation.GetLabel("GiveCashSuccess", (object) pR.player_name);
    }

    public static string SendById(int id, int cash) => SendCashToPlayer.BaseGiveCashToPlayer(AccountManager.getAccount((long) id, 0), cash);

    private static string BaseGiveCashToPlayer(Account player, int cash)
    {
      if (player == null)
        return Translation.GetLabel("GiveCashFail");
      if (!PlayerManager.updateAccountCash(player.player_id, player._money + cash))
        return Translation.GetLabel("GiveCashFail2");
      player._money += cash;
      player.SendPacket((SendPacket) new AUTH_WEB_CASH_PAK(0, player._gp, player._money), false);
      SEND_ITEM_INFO.LoadGoldCash(player);
      return Translation.GetLabel("GiveCashSuccessD", (object) cash, (object) player.player_name);
    }
  }
}
