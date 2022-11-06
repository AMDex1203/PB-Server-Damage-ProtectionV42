
// Type: Game.data.chat.KickPlayer
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.server;
using Game.data.managers;
using Game.data.model;
using Game.global.serverpacket;

namespace Game.data.chat
{
  public static class KickPlayer
  {
    public static string KickByNick(string str, Account player)
    {
      Account account = AccountManager.getAccount(str.Substring(4), 1, 0);
      return KickPlayer.BaseKick(player, account);
    }

    public static string KickById(string str, Account player)
    {
      Account account = AccountManager.getAccount(long.Parse(str.Substring(4)), 0);
      return KickPlayer.BaseKick(player, account);
    }

    private static string BaseKick(Account player, Account victim)
    {
      if (victim == null)
        return Translation.GetLabel("PlayerKickNotFound");
      if (victim.access > player.access)
        return Translation.GetLabel("PlayerBanAccessInvalid");
      if (victim.player_id == player.player_id)
        return Translation.GetLabel("PlayerKickKickYourself");
      if (victim._connection != null)
      {
        victim.SendPacket((SendPacket) new AUTH_ACCOUNT_KICK_PAK(2), false);
        victim.Close(1000, true);
        return Translation.GetLabel("PlayerKickSuccess", (object) victim.player_name);
      }
      return Translation.GetLabel("PlayerKickOffline", (object) victim.player_name);
    }
  }
}
