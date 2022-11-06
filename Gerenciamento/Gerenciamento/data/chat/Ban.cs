
// Type: Game.data.chat.Ban
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.managers;
using Core.models.enums;
using Core.server;
using Game.data.managers;
using Game.data.model;
using Game.global.serverpacket;
using System;

namespace Game.data.chat
{
  public static class Ban
  {
    public static string UpdateReason(string str)
    {
      string str1 = str.Substring(7);
      int num = str1.IndexOf(" ");
      if (num < 0)
        return "Comando inválido. [Servidor]";
      return BanManager.SaveBanReason(long.Parse(str1.Split(' ')[0]), str1.Substring(num + 1)) ? Translation.GetLabel("PlayerBanReasonSuccess") : Translation.GetLabel("PlayerBanReasonFail");
    }

    public static string BanForeverNick(string str, Account player, bool warn)
    {
      Account account = AccountManager.getAccount(str.Substring(6), 1, 0);
      return Ban.BaseBanForever(player, account, warn);
    }

    public static string BanForeverId(string str, Account player, bool warn)
    {
      Account account = AccountManager.getAccount(long.Parse(str.Substring(7)), 0);
      return Ban.BaseBanForever(player, account, warn);
    }

    public static string ApiBanForeverId(long toBan, long Admin, bool warn)
    {
      Account account = AccountManager.getAccount(toBan, 0);
      return Ban.BaseBanForever(AccountManager.getAccount(Admin, 0), account, warn);
    }

    public static string BanNormalNick(string str, Account player, bool warn)
    {
      string[] strArray = str.Substring(5).Split(' ');
      string text = strArray[0];
      DateTime endDate = DateTime.Now.AddDays(Convert.ToDouble(strArray[1]));
      Account account = AccountManager.getAccount(text, 1, 0);
      return Ban.BaseBanNormal(player, account, warn, endDate);
    }

    public static string BanNormalId(string str, Account player, bool warn)
    {
      string[] strArray = str.Substring(6).Split(' ');
      long int64 = Convert.ToInt64(strArray[0]);
      DateTime endDate = DateTime.Now.AddDays(Convert.ToDouble(strArray[1]));
      Account account = AccountManager.getAccount(int64, 0);
      return Ban.BaseBanNormal(player, account, warn, endDate);
    }

    private static string BaseBanNormal(
      Account player,
      Account victim,
      bool warn,
      DateTime endDate)
    {
      if (victim == null)
        return Translation.GetLabel("PlayerBanUserInvalid");
      if (victim.access > player.access)
        return Translation.GetLabel("PlayerBanAccessInvalid");
      if (player.player_id == victim.player_id)
        return Translation.GetLabel("PlayerBanSimilarID");
      BanHistory banHistory = BanManager.SaveHistory(player.player_id, "DURATION", victim.player_id.ToString(), endDate);
      if (banHistory == null)
        return Translation.GetLabel("PlayerBanFail");
      if (warn)
      {
        using (SERVER_MESSAGE_ANNOUNCE_PAK messageAnnouncePak = new SERVER_MESSAGE_ANNOUNCE_PAK(Translation.GetLabel("PlayerBannedWarning2", (object) victim.player_name)))
          GameManager.SendPacketToAllClients((SendPacket) messageAnnouncePak);
      }
      victim.ban_obj_id = banHistory.object_id;
      victim.SendPacket((SendPacket) new AUTH_ACCOUNT_KICK_PAK(2), false);
      victim.Close(1000, true);
      return Translation.GetLabel("PlayerBanSuccess", (object) banHistory.object_id);
    }

    private static string BaseBanForever(Account player, Account victim, bool warn)
    {
      if (victim == null)
        return Translation.GetLabel("PlayerBanUserInvalid");
      if (victim.access > player.access)
        return Translation.GetLabel("PlayerBanAccessInvalid");
      if (player.player_id == victim.player_id)
        return Translation.GetLabel("PlayerBanSimilarID");
      if (victim.access == AccessLevel.Banned)
        return ComDiv.updateDB("accounts", "access_level", (object) 0, "player_id", (object) victim.player_id) ? "Jogador desbanido com sucesso." : "Erro";
      if (!ComDiv.updateDB("accounts", "access_level", (object) -1, "player_id", (object) victim.player_id))
        return Translation.GetLabel("PlayerBanFail");
      if (warn)
      {
        using (SERVER_MESSAGE_ANNOUNCE_PAK messageAnnouncePak = new SERVER_MESSAGE_ANNOUNCE_PAK(Translation.GetLabel("PlayerBannedWarning", (object) victim.player_name)))
          GameManager.SendPacketToAllClients((SendPacket) messageAnnouncePak);
      }
      victim.access = AccessLevel.Banned;
      victim.SendPacket((SendPacket) new AUTH_ACCOUNT_KICK_PAK(2), false);
      victim.Close(1000, true);
      return Translation.GetLabel("PlayerBanSuccess", (object) -1);
    }

    public static string GetBanData(string str, Account player)
    {
      BanHistory accountBan = BanManager.GetAccountBan(long.Parse(str.Substring(7)));
      if (accountBan == null)
        return Translation.GetLabel("GetBanInfoError");
      string msg = Translation.GetLabel("GetBanInfoTitle") + "\n" + Translation.GetLabel("GetBanInfoProvider", (object) accountBan.provider_id) + "\n" + Translation.GetLabel("GetBanInfoType", (object) accountBan.type) + "\n" + Translation.GetLabel("GetBanInfoValue", (object) accountBan.value) + "\n" + Translation.GetLabel("GetBanInfoReason", (object) accountBan.reason) + "\n" + Translation.GetLabel("GetBanInfoStart", (object) accountBan.startDate) + "\n" + Translation.GetLabel("GetBanInfoEnd", (object) accountBan.endDate);
      player.SendPacket((SendPacket) new SERVER_MESSAGE_ANNOUNCE_PAK(msg));
      return Translation.GetLabel("GetBanInfoSuccess");
    }
  }
}
