
// Type: Game.data.chat.GMDisguises
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.managers;
using Core.server;
using Game.data.managers;
using Game.data.model;
using Game.data.utils;
using Game.global.serverpacket;

namespace Game.data.chat
{
  public static class GMDisguises
  {
    public static string SetHideColor(Account player)
    {
      if (player == null)
        return Translation.GetLabel("HideGMColorFail");
      if (player._rank != 53 && player._rank != 54)
        return Translation.GetLabel("HideGMColorNoRank");
      player.HideGMcolor = !player.HideGMcolor;
      return player.HideGMcolor ? Translation.GetLabel("HideGMColorSuccess1") : Translation.GetLabel("HideGMColorSuccess2");
    }

    public static string SetAntiKick(Account player)
    {
      if (player == null)
        return Translation.GetLabel("AntiKickGMFail");
      player.AntiKickGM = !player.AntiKickGM;
      return player.AntiKickGM ? Translation.GetLabel("AntiKickGMSuccess1") : Translation.GetLabel("AntiKickGMSuccess2");
    }

    public static string SetFakeRank(string str, Account player, Room room)
    {
      int num = int.Parse(str.Substring(3));
      if (num > 55 || num < 0)
        return Translation.GetLabel("FakeRankWrongValue");
      if (player._bonus.fakeRank == num)
        return Translation.GetLabel("FakeRankAlreadyFaked");
      if (!ComDiv.updateDB("player_bonus", "fakerank", (object) num, "player_id", (object) player.player_id))
        return Translation.GetLabel("FakeRankFail");
      player._bonus.fakeRank = num;
      player.SendPacket((SendPacket) new BASE_USER_EFFECTS_PAK(0, player._bonus));
      room?.updateSlotsInfo();
      if (num == 55)
        return Translation.GetLabel("FakeRankSuccess1");
      return Translation.GetLabel("FakeRankSuccess2", (object) num);
    }

    public static string SetFakeNick(string str, Account player, Room room)
    {
      string name = str.Substring(5);
      if (name.Length > ConfigGS.maxNickSize || name.Length < ConfigGS.minNickSize)
        return Translation.GetLabel("FakeNickWrongLength");
      if (PlayerManager.isPlayerNameExist(name))
        return Translation.GetLabel("FakeNickAlreadyExist");
      if (!ComDiv.updateDB("accounts", "player_name", (object) name, "player_id", (object) player.player_id))
        return Translation.GetLabel("FakeNickFail");
      player.player_name = name;
      player.SendPacket((SendPacket) new AUTH_CHANGE_NICKNAME_PAK(name));
      if (room != null)
      {
        using (ROOM_GET_NICKNAME_PAK roomGetNicknamePak = new ROOM_GET_NICKNAME_PAK(player._slotId, player.player_name, player.name_color))
          room.SendPacketToPlayers((SendPacket) roomGetNicknamePak);
      }
      if (player.clanId > 0)
      {
        using (CLAN_MEMBER_INFO_UPDATE_PAK memberInfoUpdatePak = new CLAN_MEMBER_INFO_UPDATE_PAK(player))
          ClanManager.SendPacket((SendPacket) memberInfoUpdatePak, player.clanId, -1L, true, true);
      }
      AllUtils.syncPlayerToFriends(player, true);
      return Translation.GetLabel("FakeNickSuccess", (object) name);
    }
  }
}
