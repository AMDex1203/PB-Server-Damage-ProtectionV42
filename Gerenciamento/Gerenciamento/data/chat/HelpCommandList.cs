
// Type: Game.data.chat.HelpCommandList
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.models.enums;
using Core.models.room;
using Core.server;
using Game.data.model;
using Game.global.serverpacket;

namespace Game.data.chat
{
  public static class HelpCommandList
  {
    public static string GetList1(Account player)
    {
      if (player.access < AccessLevel.Moderator)
        return Translation.GetLabel("HelpListNoLevel");
      if (HelpCommandList.InGame(player))
        return Translation.GetLabel("InGameBlock");
      string msg = Translation.GetLabel("HelpListTitle1") + "\n" + Translation.GetLabel("NickHistoryByID") + "\n" + Translation.GetLabel("IDHistoryByNick") + "\n" + Translation.GetLabel("FakeRank") + "\n" + Translation.GetLabel("ChangeNick") + "\n" + Translation.GetLabel("KickPlayer") + "\n" + Translation.GetLabel("EnableDisableGMColor") + "\n" + Translation.GetLabel("AntiKickActive") + "\n" + Translation.GetLabel("RoomUnlock") + "\n" + Translation.GetLabel("AFKCounter") + "\n" + Translation.GetLabel("AFKKick") + "\n" + Translation.GetLabel("PlayersCountInServer") + "\n" + Translation.GetLabel("PlayersCountInServer2") + "\n" + Translation.GetLabel("Ping");
      player.SendPacket((SendPacket) new SERVER_MESSAGE_ANNOUNCE_PAK(msg));
      return Translation.GetLabel("HelpListList1");
    }

    public static string GetList2(Account player)
    {
      if (player.access < AccessLevel.Moderator)
        return Translation.GetLabel("HelpListNoLevel");
      if (HelpCommandList.InGame(player))
        return Translation.GetLabel("InGameBlock");
      string msg = Translation.GetLabel("HelpListTitle2") + "\n" + Translation.GetLabel("MsgMOD") + "\n" + Translation.GetLabel("Msg2MOD") + "\n" + Translation.GetLabel("Give10CashMOD") + "\n" + Translation.GetLabel("Give10GoldMOD") + "\n" + Translation.GetLabel("TakeTitlesMOD") + "\n" + Translation.GetLabel("V2ReloadShopMOD");
      player.SendPacket((SendPacket) new SERVER_MESSAGE_ANNOUNCE_PAK(msg));
      return Translation.GetLabel("HelpListList2");
    }

    public static string GetList3(Account player)
    {
      if (player.access < AccessLevel.GameMaster)
        return Translation.GetLabel("HelpListNoLevel");
      if (HelpCommandList.InGame(player))
        return Translation.GetLabel("InGameBlock");
      string msg = Translation.GetLabel("HelpListTitle3") + "\n\n" + Translation.GetLabel("MsgToAllServer") + "\n" + Translation.GetLabel("MsgToAllRoom") + "\n" + Translation.GetLabel("ChangeMapId") + "\n" + Translation.GetLabel("ChangeRoomTime") + "\n" + Translation.GetLabel("Give10Cash") + "\n" + Translation.GetLabel("Give10Gold") + "\n" + Translation.GetLabel("KickAll") + "\n" + Translation.GetLabel("SendGift") + "\n" + Translation.GetLabel("GoodsFound") + "\n" + Translation.GetLabel("SimpleBanNormal") + "\n" + Translation.GetLabel("AdvancedBanNormal") + "\n" + Translation.GetLabel("UnbanNormal") + "\n" + Translation.GetLabel("GetPlayersByIP") + "\n" + Translation.GetLabel("BanReason") + "\n" + Translation.GetLabel("GetPlayerInfos") + "\n" + Translation.GetLabel("OpenRoomSlot") + "\n" + Translation.GetLabel("OpenRandomRoomSlot") + "\n" + Translation.GetLabel("OpenAllClosedRoomSlots") + "\n" + Translation.GetLabel("TakeTitles");
      player.SendPacket((SendPacket) new SERVER_MESSAGE_ANNOUNCE_PAK(msg));
      return Translation.GetLabel("HelpListList3");
    }

    public static string GetList4(Account player)
    {
      if (player.access < AccessLevel.Admin)
        return Translation.GetLabel("HelpListNoLevel");
      if (HelpCommandList.InGame(player))
        return Translation.GetLabel("InGameBlock");
      string msg = Translation.GetLabel("HelpListTitle4") + "\n" + Translation.GetLabel("SimpleBanEtern") + "\n" + Translation.GetLabel("AdvancedBanEtern") + "\n" + Translation.GetLabel("GetBanInfo") + "\n" + Translation.GetLabel("UnbanEtern") + "\n" + Translation.GetLabel("CreateItemPt1") + "\n" + Translation.GetLabel("CreateItemPt2") + "\n" + Translation.GetLabel("CreateGoldItem") + "\n" + Translation.GetLabel("ReloadShop") + "\n" + Translation.GetLabel("V2ReloadShop") + "\n" + Translation.GetLabel("ChangeAnnounce") + "\n" + Translation.GetLabel("SetCashD") + "\n" + Translation.GetLabel("SetGoldD") + "\n" + Translation.GetLabel("CashPlayerD") + "\n" + Translation.GetLabel("GoldPlayerD");
      player.SendPacket((SendPacket) new SERVER_MESSAGE_ANNOUNCE_PAK(msg));
      return Translation.GetLabel("HelpListList4");
    }

    public static string GetList5(Account player)
    {
      if (player.access < AccessLevel.Developer)
        return Translation.GetLabel("HelpListNoLevel");
      if (HelpCommandList.InGame(player))
        return Translation.GetLabel("InGameBlock");
      string msg = Translation.GetLabel("HelpListTitle5") + "\n\n" + Translation.GetLabel("Pausar") + "\n" + Translation.GetLabel("EndRoom") + "\n" + Translation.GetLabel("ChangeRoomType") + "\n" + Translation.GetLabel("ChangeRoomSpecial") + "\n" + Translation.GetLabel("ChangeRoomWeapons") + "\n" + Translation.GetLabel("ChangeUDP") + "\n" + Translation.GetLabel("EnableTestMode") + "\n" + Translation.GetLabel("EnablePublicMode") + "\n" + Translation.GetLabel("EnableMissions") + "\n" + Translation.GetLabel("SetVip") + "\n" + Translation.GetLabel("SetAcess") + "\n" + Translation.GetLabel("ChangeRank") + "\n" + Translation.GetLabel("CreateItemPt1") + "\n" + Translation.GetLabel("CreateItemPt2") + "\nColocar o Shop todo em Promoção: ;saleShop";
      player.SendPacket((SendPacket) new SERVER_MESSAGE_ANNOUNCE_PAK(msg));
      return Translation.GetLabel("HelpListList5");
    }

    public static string GetListPlayer(Account player)
    {
      if (HelpCommandList.InGame(player))
        return Translation.GetLabel("InGameBlock");
      string msg = Translation.GetLabel("HelpListTitlePlayer") + "\nMostra sua Latência de conexão com o servidor na partida: !ping" + "\nExibir LOGs de dano causado: !dano" + "\nInformações da versão atual do servidor: !serverinfo";
      player.SendPacket((SendPacket) new SERVER_MESSAGE_ANNOUNCE_PAK(msg));
      return Translation.GetLabel("HelpListListPlayer");
    }

    public static string GetNEWListDEV(Account player)
    {
      if (HelpCommandList.InGame(player))
        return Translation.GetLabel("InGameBlock");
      string msg = "</> NEW List DEV </>" + "\nRecarregar Regras: ;reloadRules" + "\nRecarregar Evento de XMAS: ;reloadeventXmas" + "\nRecarregar Evento de RankUP: ;reloadeventUP" + "\nRecarregar Evento de PlayTime: ;reloadeventPT" + "\nRecarregar Evento de Mapas: ;reloadeventMP" + "\nRecarregar Evento de Missões: ;reloadeventQuest" + "\nRecarregar Evento de Login: ;reloadeventLogin" + "\nRecarregar Evento de Verificação: ;reloadeventCheck";
      player.SendPacket((SendPacket) new SERVER_MESSAGE_ANNOUNCE_PAK(msg));
      return "Você recebeu a lista 2 de comandos de Developer.";
    }

    public static string GetListInfoServer(Account player)
    {
      if (HelpCommandList.InGame(player))
        return Translation.GetLabel("InGameBlock");
      string msg = "</> Point Blank Sword </>" + "\nVersão 1.15.37" + "\nPing Fixo" + "\nNovo sistema de UP sem acumular EXP" + "\nBalanceamento Fixo" + "\nNovos comandos para MODs, ADMs e DEVS" + "\nNovos comandos para Players" + "\nEfeitos de itens inapropriados em @CAMP são desativados nas partidas" + "\nSnipers com 28% de dano ++" + "\nProteção contra Packs de Dano" + "\n</> Dúvidas, contate um membro da Staff </>";
      player.SendPacket((SendPacket) new SERVER_MESSAGE_ANNOUNCE_PAK(msg));
      return "Você recebeu as informações da versão atual do servidor.";
    }

    private static bool InGame(Account player)
    {
      Room room = player._room;
      Slot slot;
      return room != null && room.getSlot(player._slotId, out slot) && slot.state >= SLOT_STATE.LOAD;
    }
  }
}
