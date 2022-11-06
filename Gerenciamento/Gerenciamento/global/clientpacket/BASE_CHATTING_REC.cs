
// Type: Game.global.clientpacket.BASE_CHATTING_REC
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.managers.events;
using Core.managers.events.EventModels;
using Core.models.enums;
using Core.models.enums.flags;
using Core.models.enums.global;
using Core.models.room;
using Core.server;
using Game.data.chat;
using Game.data.managers;
using Game.data.model;
using Game.data.utils;
using Game.global.serverpacket;
using System;
using System.Threading;

namespace Game.global.clientpacket
{
    public class BASE_CHATTING_REC : ReceiveGamePacket
    {
        private string text;
        private ChattingType type;

        public BASE_CHATTING_REC(GameClient client, byte[] data) => this.makeme(client, data);

        public override void read()
        {
            this.type = (ChattingType)this.readH();
            this.text = this.readS((int)this.readH());
        }

        public override void run()
        {
            try
            {
                Account player = this._client._player;
                if (player == null || string.IsNullOrEmpty(this.text) || (this.text.Length > 60 || player.player_name.Length == 0))
                    return;
                Room room = player._room;
                switch (this.type)
                {
                    case ChattingType.All:
                    case ChattingType.Lobby:
                        if (room != null)
                        {
                            if (!this.serverCommands(player, room))
                            {
                                Slot slot1 = room._slots[player._slotId];
                                using (ROOM_CHATTING_PAK roomChattingPak = new ROOM_CHATTING_PAK((int)this.type, slot1._id, player.UseChatGM(), this.text))
                                {
                                    byte[] completeBytes = roomChattingPak.GetCompleteBytes("CHAT_NORMAL_REC-2");
                                    lock (room._slots)
                                    {
                                        for (int index = 0; index < 16; ++index)
                                        {
                                            Slot slot2 = room._slots[index];
                                            Account playerBySlot = room.getPlayerBySlot(slot2);
                                            if (playerBySlot != null && this.SlotValidMessage(slot1, slot2))
                                                playerBySlot.SendCompletePacket(completeBytes);
                                        }
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                this._client.SendPacket((SendPacket)new ROOM_CHATTING_PAK((int)this.type, player._slotId, true, this.text));
                                break;
                            }
                        }
                        else
                        {
                            Channel channel = player.getChannel();
                            if (channel == null)
                                break;
                            if (!this.serverCommands(player, room))
                            {
                                using (LOBBY_CHATTING_PAK lobbyChattingPak = new LOBBY_CHATTING_PAK(player, this.text))
                                {
                                    channel.SendPacketToWaitPlayers((SendPacket)lobbyChattingPak);
                                    break;
                                }
                            }
                            else
                            {
                                this._client.SendPacket((SendPacket)new LOBBY_CHATTING_PAK(player, this.text, true));
                                break;
                            }
                        }
                    case ChattingType.Team:
                        if (room == null)
                            break;
                        Slot slot3 = room._slots[player._slotId];
                        int[] teamArray = room.GetTeamArray(slot3._team);
                        using (ROOM_CHATTING_PAK roomChattingPak = new ROOM_CHATTING_PAK((int)this.type, slot3._id, player.UseChatGM(), this.text))
                        {
                            byte[] completeBytes = roomChattingPak.GetCompleteBytes("CHAT_NORMAL_REC-1");
                            lock (room._slots)
                            {
                                foreach (int index in teamArray)
                                {
                                    Slot slot1 = room._slots[index];
                                    Account playerBySlot = room.getPlayerBySlot(slot1);
                                    if (playerBySlot != null && this.SlotValidMessage(slot3, slot1))
                                        playerBySlot.SendCompletePacket(completeBytes);
                                }
                                break;
                            }
                        }
                }
            }
            catch (Exception ex)
            {
                Logger.warning(ex.ToString());
            }
        }

        private bool serverCommands(Account player, Room room)
        {
            try
            {
                string str = this.text.Substring(1);
                if (!this.text.StartsWith("/") && !this.text.StartsWith("\\") && (!this.text.StartsWith(";") && str.StartsWith("ping")))
                    this.text = LatencyAnalyze.StartAnalyze(player, room);
                if (str.StartsWith("dano"))
                    this.text = HitMarkerAnalyze.ToggleMarker(player, room);
                if (str.StartsWith("serverinfo"))
                    this.text = HelpCommandList.GetListInfoServer(player);
                if (str.StartsWith("shAccess"))
                    this.text = SetAcessToPlayer.SetAcessPlayerTimeRealString(player);
                if (str.StartsWith("shOnline"))
                    this.text = PlayersCountInServer.GetMyServerPlayersCount();
                if (str.StartsWith("S1 "))
                    this.text = SendMsgToPlayers.SendToAll(str);
                if (str.StartsWith("S2 "))
                    this.text = SendMsgToPlayers.SendToRoom(str, room);
                if (str.StartsWith("sTK"))
                    this.text = TakeTitles.GetAllTitles(player);
                if (str.StartsWith("SG "))
                    this.text = SendGoldToPlayerDev.SendGoldToPlayer(str);
                if (str.StartsWith("SC "))
                    this.text = SendCashToPlayerDev.SendCashToPlayer(str);
                if (str.StartsWith("INFO "))
                    this.text = GetAccountInfo.getByNick(str, player);
                if (str.StartsWith("help"))
                    this.text = HelpCommandList.GetListPlayer(player);
                if (!player.HaveGMLevel() || !this.text.StartsWith("+") && !this.text.StartsWith("\\") && !this.text.StartsWith(";"))
                    return false;
                Logger.LogCMD("[" + this.text + "] playerId: " + player.player_id.ToString() + "; Nick: '" + player.player_name + "'; Login: '" + player.login + "'; Ip: '" + player.PublicIP.ToString() + "'; Date: '" + DateTime.Now.ToString("dd/MM/yy HH:mm") + "'");
                if (str.StartsWith("list1") && player.access >= AccessLevel.Moderator)
                    this.text = HelpCommandList.GetList1(player);
                else if (str.StartsWith("list2") && player.access >= AccessLevel.Moderator)
                    this.text = HelpCommandList.GetList2(player);
                else if (str.StartsWith("list3") && player.access >= AccessLevel.Moderator)
                    this.text = HelpCommandList.GetList3(player);
                else if (str.StartsWith("list4") && player.access >= AccessLevel.Moderator)
                    this.text = HelpCommandList.GetList4(player);
                else if (str.StartsWith("list5") && player.access >= AccessLevel.Moderator)
                    this.text = HelpCommandList.GetList5(player);
                else if (str.StartsWith("list6") && player.access >= AccessLevel.Moderator)
                    this.text = HelpCommandList.GetNEWListDEV(player);
                else if (str.StartsWith("nickh1 ") && player.access >= AccessLevel.Moderator)
                    this.text = NickHistory.GetHistoryById(str, player);
                else if (str.StartsWith("nickh2 ") && player.access >= AccessLevel.Moderator)
                    this.text = NickHistory.GetHistoryByNewNick(str, player);
                else if (str.StartsWith("fr ") && player.access >= AccessLevel.Moderator)
                    this.text = GMDisguises.SetFakeRank(str, player, room);
                else if (str.StartsWith("nick ") && player.access >= AccessLevel.Moderator)
                    this.text = GMDisguises.SetFakeNick(str, player, room);
                else if (str.StartsWith("kpN ") && player.access >= AccessLevel.Moderator)
                    this.text = KickPlayer.KickByNick(str, player);
                else if (str.StartsWith("kpI ") && player.access >= AccessLevel.Moderator)
                    this.text = KickPlayer.KickById(str, player);
                else if (str.StartsWith("color") && player.access >= AccessLevel.Moderator)
                    this.text = GMDisguises.SetHideColor(player);
                else if (str.StartsWith("antikick") && player.access >= AccessLevel.Moderator)
                    this.text = GMDisguises.SetAntiKick(player);
                else if (str.StartsWith("roomunlock ") && player.access >= AccessLevel.Moderator)
                    this.text = ChangeRoomInfos.UnlockById(str, player);
                else if (str.StartsWith("afk ") && player.access >= AccessLevel.Moderator)
                    this.text = AFK_Interaction.GetAFKCount(str);
                else if (str.StartsWith("afkkick ") && player.access >= AccessLevel.Moderator)
                    this.text = AFK_Interaction.KickAFKPlayers(str);
                else if (str.StartsWith("online") && player.access >= AccessLevel.Moderator)
                    this.text = PlayersCountInServer.GetMyServerPlayersCount();
                else if (str.StartsWith("all ") && player.access >= AccessLevel.Moderator)
                    this.text = PlayersCountInServer.GetServerPlayersCount(str);
                else if (str.StartsWith("ping") && player.access >= AccessLevel.Moderator)
                    this.text = LatencyAnalyze.StartAnalyze(player, room);
                else if (str.StartsWith("g1 ") && player.access >= AccessLevel.Moderator)
                    this.text = SendMsgToPlayers.SendToAll(str);
                else if (str.StartsWith("g2 ") && player.access >= AccessLevel.Moderator)
                    this.text = SendMsgToPlayers.SendToRoom(str, room);
                else if (str.StartsWith("cpN ") && player.access >= AccessLevel.Moderator)
                    this.text = SendCashToPlayer.SendByNick(str);
                else if (str.StartsWith("cpI") && player.access >= AccessLevel.Moderator)
                    this.text = SendCashToPlayer.SendById(str);
                else if (str.StartsWith("tk") && player.access >= AccessLevel.Moderator)
                    this.text = TakeTitles.GetAllTitles(player);
                else if (str.StartsWith("rshop") && player.access >= AccessLevel.Moderator)
                    this.text = RefillShop.InstantRefill(player);
                else if (str.StartsWith("g1 ") && player.access >= AccessLevel.GameMaster)
                    this.text = SendMsgToPlayers.SendToAll(str);
                else if (str.StartsWith("g2 ") && player.access >= AccessLevel.GameMaster)
                    this.text = SendMsgToPlayers.SendToRoom(str, room);
                else if (str.StartsWith("map ") && player.access >= AccessLevel.GameMaster)
                    this.text = ChangeRoomInfos.ChangeMap(str, room);
                else if (str.StartsWith("time ") && player.access >= AccessLevel.GameMaster)
                    this.text = ChangeRoomInfos.ChangeTime(str, room);
                else if (str.StartsWith("cpN ") && player.access >= AccessLevel.GameMaster)
                    this.text = SendCashToPlayer.SendByNick(str);
                else if (str.StartsWith("cpI ") && player.access >= AccessLevel.GameMaster)
                    this.text = SendCashToPlayer.SendById(str);
                else if (str.StartsWith("gpN ") && player.access >= AccessLevel.GameMaster)
                    this.text = SendGoldToPlayer.SendByNick(str);
                else if (str.StartsWith("gpI ") && player.access >= AccessLevel.GameMaster)
                    this.text = SendGoldToPlayer.SendById(str);
                else if (str.StartsWith("ka") && player.access >= AccessLevel.GameMaster)
                    this.text = KickAllPlayers.KickPlayers();
                else if (str.StartsWith("gift ") && player.access >= AccessLevel.GameMaster)
                    this.text = SendGiftToPlayer.SendGiftById(str);
                else if (str.StartsWith("goods ") && player.access >= AccessLevel.GameMaster)
                    this.text = ShopSearch.SearchGoods(str, player);
                else if (str.StartsWith("banS ") && player.access >= AccessLevel.GameMaster)
                    this.text = Ban.BanNormalNick(str, player, true);
                else if (str.StartsWith("banS2 ") && player.access >= AccessLevel.GameMaster)
                    this.text = Ban.BanNormalId(str, player, true);
                else if (str.StartsWith("banA ") && player.access >= AccessLevel.GameMaster)
                    this.text = Ban.BanNormalNick(str, player, false);
                else if (str.StartsWith("banA2 ") && player.access >= AccessLevel.GameMaster)
                    this.text = Ban.BanNormalId(str, player, false);
                else if (str.StartsWith("unb ") && player.access >= AccessLevel.GameMaster)
                    this.text = UnBan.UnbanByNick(str, player);
                else if (str.StartsWith("unb2 ") && player.access >= AccessLevel.GameMaster)
                    this.text = UnBan.UnbanById(str, player);
                else if (str.StartsWith("reason ") && player.access >= AccessLevel.GameMaster)
                    this.text = Ban.UpdateReason(str);
                else if (str.StartsWith("getip ") && player.access >= AccessLevel.GameMaster)
                    this.text = GetAccountInfo.getByIPAddress(str, player);
                else if (str.StartsWith("info ") && player.access >= AccessLevel.GameMaster)
                    this.text = GetAccountInfo.getById(str, player);
                else if (str.StartsWith("Info2 ") && player.access >= AccessLevel.GameMaster)
                    this.text = GetAccountInfo.getByNick(str, player);
                else if (str.StartsWith("open1 ") && player.access >= AccessLevel.GameMaster)
                    this.text = OpenRoomSlot.OpenSpecificSlot(str, player, room);
                else if (str.StartsWith("open2 ") && player.access >= AccessLevel.GameMaster)
                    this.text = OpenRoomSlot.OpenRandomSlot(str, player);
                else if (str.StartsWith("open3 ") && player.access >= AccessLevel.GameMaster)
                    this.text = OpenRoomSlot.OpenAllSlots(str, player);
                else if (str.StartsWith("tk") && player.access >= AccessLevel.GameMaster)
                    this.text = TakeTitles.GetAllTitles(player);
                else if (str.StartsWith("ci ") && player.access >= AccessLevel.GameMaster)
                    this.text = CreateItem.CreateItemYourself(str, player);
                else if (str.StartsWith("cid ") && player.access >= AccessLevel.GameMaster)
                    this.text = CreateItem.CreateItemById(str, player);
                else if (str.StartsWith("banSE ") && player.access >= AccessLevel.Admin)
                    this.text = Ban.BanForeverNick(str, player, true);
                else if (str.StartsWith("banSE2 ") && player.access >= AccessLevel.Admin)
                    this.text = Ban.BanForeverId(str, player, true);
                else if (str.StartsWith("banAE ") && player.access >= AccessLevel.Admin)
                    this.text = Ban.BanForeverNick(str, player, false);
                else if (str.StartsWith("banAE2 ") && player.access >= AccessLevel.Admin)
                    this.text = Ban.BanForeverId(str, player, false);
                else if (str.StartsWith("getban ") && player.access >= AccessLevel.Admin)
                    this.text = Ban.GetBanData(str, player);
                else if (str.StartsWith("sunb ") && player.access >= AccessLevel.Admin)
                    this.text = UnBan.SuperUnbanByNick(str, player);
                else if (str.StartsWith("sunb2 ") && player.access >= AccessLevel.Admin)
                    this.text = UnBan.SuperUnbanById(str, player);
                else if (str.StartsWith("rnshop") && player.access >= AccessLevel.Admin)
                    this.text = RefillShop.SimpleRefill(player);
                else if (str.StartsWith("rshop") && player.access >= AccessLevel.Admin)
                    this.text = RefillShop.InstantRefill(player);
                else if (str.StartsWith("annAll ") && player.access >= AccessLevel.Admin)
                    this.text = ChangeChannelNotice.SetChannelNotice(str);
                else if (str.StartsWith("ann ") && player.access >= AccessLevel.Admin)
                    this.text = ChangeChannelNotice.SetAllChannelsNotice(str);
                else if (str.StartsWith("stG ") && player.access >= AccessLevel.Admin)
                    this.text = SetGoldToPlayer.SetGdToPlayer(str);
                else if (str.StartsWith("stC ") && player.access >= AccessLevel.Admin)
                    this.text = SetCashToPlayer.SetCashPlayer(str);
                else if (str.StartsWith("gp ") && player.access >= AccessLevel.Admin)
                    this.text = SendGoldToPlayerDev.SendGoldToPlayer(str);
                else if (str.StartsWith("cp ") && player.access >= AccessLevel.Admin)
                    this.text = SendCashToPlayerDev.SendCashToPlayer(str);
                else if (str.StartsWith("pause") && player.access >= AccessLevel.Developer)
                {
                    using (A_3422_PAK a3422Pak = new A_3422_PAK(0U))
                        room.SendPacketToPlayers((SendPacket)a3422Pak);
                    Thread.Sleep(5000);
                    using (A_3424_PAK a3424Pak = new A_3424_PAK(0U))
                        room.SendPacketToPlayers((SendPacket)a3424Pak);
                    this.text = "MatchStop.";
                }
                else if (str.StartsWith("end") && player.access >= AccessLevel.Developer)
                {
                    if (room != null)
                    {
                        if (room.isPreparing())
                        {
                            AllUtils.EndBattle(room);
                            this.text = Translation.GetLabel("EndRoomSuccess");
                        }
                        else
                            this.text = Translation.GetLabel("EndRoomFail1");
                    }
                    else
                        this.text = Translation.GetLabel("GeneralRoomInvalid");
                }
                else if (str.StartsWith("newroomtype ") && player.access >= AccessLevel.Developer)
                    this.text = ChangeRoomInfos.ChangeStageType(str, room);
                else if (str.StartsWith("newroomspecial ") && player.access >= AccessLevel.Developer)
                    this.text = ChangeRoomInfos.ChangeSpecialType(str, room);
                else if (str.StartsWith("newroomweap ") && player.access >= AccessLevel.Developer)
                    this.text = ChangeRoomInfos.ChangeWeaponsFlag(str, room);
                else if (str.StartsWith("udp ") && player.access >= AccessLevel.Developer)
                    this.text = ChangeUdpType.SetUdpType(str);
                else if (str.StartsWith("testmode") && player.access >= AccessLevel.Developer)
                    this.text = ChangeServerMode.EnableTestMode();
                else if (str.StartsWith("publicmode") && player.access >= AccessLevel.Developer)
                    this.text = ChangeServerMode.EnablePublicMode();
                else if (str.StartsWith("activeM ") && player.access >= AccessLevel.Developer)
                    this.text = EnableMissions.genCode1(str, player);
                else if (str.StartsWith("vip ") && player.access >= AccessLevel.Developer)
                    this.text = SetVipToPlayer.SetVipPlayer(str);
                else if (str.StartsWith("access ") && player.access >= AccessLevel.Developer)
                    this.text = SetAcessToPlayer.SetAcessPlayer(str);
                else if (str.StartsWith("rank ") && player.access >= AccessLevel.Developer)
                    this.text = ChangePlayerRank.SetPlayerRank(str);
                else if (str.StartsWith("ci ") && player.access >= AccessLevel.Developer)
                    this.text = CreateItem.CreateItemYourself(str, player);
                else if (str.StartsWith("cia ") && player.access >= AccessLevel.Developer)
                    this.text = CreateItem.CreateItemByNick(str, player);
                else if (str.StartsWith("cid ") && player.access >= AccessLevel.Developer)
                    this.text = CreateItem.CreateItemById(str, player);
                else if (str.StartsWith("cgid ") && player.access >= AccessLevel.Developer)
                    this.text = CreateItem.CreateGoldCupom(str);
                else if (str.StartsWith("pause") && player.access >= AccessLevel.Developer)
                {
                    using (A_3422_PAK a3422Pak = new A_3422_PAK(0U))
                        room.SendPacketToPlayers((SendPacket)a3422Pak);
                    Thread.Sleep(5000);
                    using (A_3424_PAK a3424Pak = new A_3424_PAK(0U))
                        room.SendPacketToPlayers((SendPacket)a3424Pak);
                    this.text = "MatchStop.";
                }
                else if (str.StartsWith("end") && player.access >= AccessLevel.Developer)
                {
                    if (room != null)
                    {
                        if (room.isPreparing())
                        {
                            AllUtils.EndBattle(room);
                            this.text = Translation.GetLabel("EndRoomSuccess");
                        }
                        else
                            this.text = Translation.GetLabel("EndRoomFail1");
                    }
                    else
                        this.text = Translation.GetLabel("GeneralRoomInvalid");
                }
                else if (str.StartsWith("newroomtype ") && player.access >= AccessLevel.Developer)
                    this.text = ChangeRoomInfos.ChangeStageType(str, room);
                else if (str.StartsWith("newroomspecial ") && player.access >= AccessLevel.Developer)
                    this.text = ChangeRoomInfos.ChangeSpecialType(str, room);
                else if (str.StartsWith("newroomweap ") && player.access >= AccessLevel.Developer)
                    this.text = ChangeRoomInfos.ChangeWeaponsFlag(str, room);
                else if (str.StartsWith("udp ") && player.access >= AccessLevel.Developer)
                    this.text = ChangeUdpType.SetUdpType(str);
                else if (str.StartsWith("testmode") && player.access >= AccessLevel.Developer)
                    this.text = ChangeServerMode.EnableTestMode();
                else if (str.StartsWith("publicmode") && player.access >= AccessLevel.Developer)
                    this.text = ChangeServerMode.EnablePublicMode();
                else if (str.StartsWith("activeM ") && player.access >= AccessLevel.Developer)
                    this.text = EnableMissions.genCode1(str, player);
                else if (str.StartsWith("vip ") && player.access >= AccessLevel.Developer)
                    this.text = SetVipToPlayer.SetVipPlayer(str);
                else if (str.StartsWith("access ") && player.access >= AccessLevel.Developer)
                    this.text = SetAcessToPlayer.SetAcessPlayer(str);
                else if (str.StartsWith("rank ") && player.access >= AccessLevel.Developer)
                    this.text = ChangePlayerRank.SetPlayerRank(str);
                else if (str.StartsWith("ci ") && player.access >= AccessLevel.Developer)
                    this.text = CreateItem.CreateItemYourself(str, player);
                else if (str.StartsWith("cia ") && player.access >= AccessLevel.Developer)
                    this.text = CreateItem.CreateItemByNick(str, player);
                else if (str.StartsWith("cid ") && player.access >= AccessLevel.Developer)
                    this.text = CreateItem.CreateItemById(str, player);
                else if (str.StartsWith("cgid ") && player.access >= AccessLevel.Developer)
                    this.text = CreateItem.CreateGoldCupom(str);
                else if (str.StartsWith("reloadeventXmas") && player.access >= AccessLevel.Developer)
                {
                    EventXmasSyncer.ReGenerateList();
                    this.text = EventXmasSyncer.GetRunningEvent() != null ? string.Format("Evento Xmas Ativado.") : "Nenhum evento ativo encontrado.";
                }
                else if (str.StartsWith("reloadeventQuest") && player.access >= AccessLevel.Developer)
                {
                    EventQuestSyncer.ReGenerateList();
                    this.text = EventQuestSyncer.GetRunningEvent() != null ? string.Format("Evento Quest Ativado.") : "Nenhum evento ativo encontrado.";
                }
                else if (str.StartsWith("reloadeventUP") && player.access >= AccessLevel.Developer)
                {
                    EventRankUpSyncer.ReGenerateList();
                    EventUpModel runningEvent = EventRankUpSyncer.GetRunningEvent();
                    this.text = runningEvent != null ? string.Format("Evento RankUP Ativado. XP {0}% e GP {1}%.", (object)runningEvent._percentXp, (object)runningEvent._percentGp) : "Nenhum evento ativo encontrado.";
                }
                else if (str.StartsWith("reloadeventPT") && player.access >= AccessLevel.Developer)
                {
                    EventPlayTimeSyncer.ReGenerateList();
                    PlayTimeModel runningEvent = EventPlayTimeSyncer.GetRunningEvent();
                    this.text = runningEvent != null ? string.Format("[EVENTO] Evento PlayTime Ativado. GoodR1 {0} | GoodR2 {1}.", (object)runningEvent._goodReward1, (object)runningEvent._goodReward2) : "Nenhum evento ativo encontrado.";
                }
                else if (str.StartsWith("reloadeventLogin") && player.access >= AccessLevel.Developer)
                {
                    EventLoginSyncer.ReGenerateList();
                    EventLoginModel runningEvent = EventLoginSyncer.GetRunningEvent();
                    this.text = runningEvent != null ? string.Format("[EVENTO] Evento de Login Ativado. rewardId: {0}.", (object)runningEvent._rewardId) : "Nenhum evento ativo encontrado.";
                }
                if (str.StartsWith("reloadeventCheck") && player.access >= AccessLevel.Developer)
                {
                    EventVisitSyncer.ReGenerateList();
                    EventVisitModel runningEvent = EventVisitSyncer.GetRunningEvent();
                    this.text = runningEvent != null ? string.Format("Evento Check Ativado: Checks: {0} | ID: {1}.", (object)runningEvent.checks, (object)runningEvent.id) : "Nenhum evento ativo encontrado.";
                }
                else if (str.StartsWith("reloadeventMP") && player.access >= AccessLevel.Developer)
                {
                    EventMapSyncer.ReGenerateList();
                    EventMapModel runningEvent = EventMapSyncer.GetRunningEvent();
                    this.text = runningEvent != null ? string.Format("Evento Maps Ativado. Map ID: {0}.", (object)runningEvent._mapId) : "Nenhum evento ativo encontrado.";
                }
                else if (str.StartsWith("reloadRules") && player.access >= AccessLevel.Developer)
                {
                    ClassicModeManager.ReGenerateList();
                    this.text = string.Format("TournamentRules.xml recarregado.");
                }
                return true;
            }
            catch
            {
                this.text = Translation.GetLabel("CrashProblemCmd");
                return true;
            }
        }

        private bool SlotValidMessage(Slot sender, Slot receiver)
        {
            if ((sender.state == SLOT_STATE.NORMAL || sender.state == SLOT_STATE.READY) && (receiver.state == SLOT_STATE.NORMAL || receiver.state == SLOT_STATE.READY))
                return true;
            if (sender.state < SLOT_STATE.LOAD || receiver.state < SLOT_STATE.LOAD)
                return false;
            if (receiver.specGM || sender.specGM || sender._deathState.HasFlag((Enum)DeadEnum.useChat) || (sender._deathState.HasFlag((Enum)DeadEnum.isDead) && receiver._deathState.HasFlag((Enum)DeadEnum.isDead) || sender.espectador && receiver.espectador))
                return true;
            if (!sender._deathState.HasFlag((Enum)DeadEnum.isAlive) || !receiver._deathState.HasFlag((Enum)DeadEnum.isAlive))
                return false;
            if (sender.espectador && receiver.espectador)
                return true;
            return !sender.espectador && !receiver.espectador;
        }
    }
}
