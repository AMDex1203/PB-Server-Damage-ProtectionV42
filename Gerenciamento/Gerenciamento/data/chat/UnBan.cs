
// Type: Game.data.chat.UnBan
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.models.enums;
using Core.server;
using Game.data.managers;
using Game.data.model;
using System;

namespace Game.data.chat
{
  public static class UnBan
  {
    public static string UnbanByNick(string str, Account player)
    {
      Account account = AccountManager.getAccount(str.Substring(4), 1, 0);
      return UnBan.BaseUnbanNormal(player, account);
    }

    public static string UnbanById(string str, Account player)
    {
      Account account = AccountManager.getAccount(long.Parse(str.Substring(5)), 0);
      return UnBan.BaseUnbanNormal(player, account);
    }

    public static string SuperUnbanByNick(string str, Account player)
    {
      Account account = AccountManager.getAccount(str.Substring(5), 1, 0);
      return UnBan.BaseUnbanSuper(player, account);
    }

    public static string SuperUnbanById(string str, Account player)
    {
      Account account = AccountManager.getAccount(long.Parse(str.Substring(6)), 0);
      return UnBan.BaseUnbanSuper(player, account);
    }

    private static string BaseUnbanNormal(Account player, Account victim)
    {
      if (victim == null)
        return Translation.GetLabel("PlayerBanUserInvalid");
      if (victim.access == AccessLevel.Banned)
        return Translation.GetLabel("PlayerUnbanAccessInvalid");
      if (victim.ban_obj_id == 0L)
        return Translation.GetLabel("PlayerUnbanNoBan");
      if (victim.player_id == player.player_id)
        return Translation.GetLabel("PlayerUnbanSimilarId");
      return ComDiv.updateDB("ban_history", "expire_date", (object) DateTime.Now, "object_id", (object) victim.ban_obj_id) ? Translation.GetLabel("PlayerUnbanSuccess") : Translation.GetLabel("PlayerUnbanFail");
    }

    private static string BaseUnbanSuper(Account player, Account victim)
    {
      if (victim == null)
        return Translation.GetLabel("PlayerBanUserInvalid");
      if (victim.access != AccessLevel.Banned)
        return Translation.GetLabel("PlayerUnbanAccessInvalid");
      if (victim.player_id == player.player_id)
        return Translation.GetLabel("PlayerUnbanSimilarId");
      if (!ComDiv.updateDB("accounts", "access_level", (object) 0, "player_id", (object) victim.player_id))
        return Translation.GetLabel("PlayerUnbanFail");
      victim.access = AccessLevel.Normal;
      return Translation.GetLabel("PlayerUnbanSuccess");
    }
  }
}
