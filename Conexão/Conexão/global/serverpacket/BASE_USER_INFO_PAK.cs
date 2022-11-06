
// Type: Auth.global.serverpacket.BASE_USER_INFO_PAK
// Assembly: pbserver_auth, Version=1.0.7796.38604, Culture=neutral, PublicKeyToken=null
// MVID: 1CFDFF97-782C-4EF8-A621-6E4FDDE32436
// Interprise: C:\Users\Cuzin\3,50pbserver_auth.exe

using Auth.data.configs;
using Auth.data.managers;
using Auth.data.model;
using Core.managers;
using Core.managers.events;
using Core.managers.events.EventModels;
using Core.managers.server;
using Core.models.account;
using Core.models.account.clan;
using Core.models.account.players;
using Core.server;
using Core.xml;
using System;
using System.Collections.Generic;

namespace Auth.global.serverpacket
{
  public class BASE_USER_INFO_PAK : SendPacket
  {
    private Account c;
    private Clan clan;
    private uint _erro;
    private bool _xmas;
    private List<ItemsModel> charas = new List<ItemsModel>();
    private List<ItemsModel> weapons = new List<ItemsModel>();
    private List<ItemsModel> cupons = new List<ItemsModel>();

    public BASE_USER_INFO_PAK(Account p)
    {
      this.c = p;
      if (this.c != null && this.c._inventory._items.Count == 0)
      {
        this.clan = ClanManager.getClanDB((object) this.c.clan_id, 1);
        this.c.LoadInventory();
        this.c.LoadMissionList();
        this.c.DiscountPlayerItems();
        if (this.c._rank >= 46 && this.c._inventory.getItem(1103003016) == null)
          this.c._inventory.AddItem(new ItemsModel(1103003016, "Boina General", 3, 1U));
        if (this.c._rank >= 53 && this.c._inventory.getItem(1103003010) == null)
          this.c._inventory.AddItem(new ItemsModel(1103003010, "Boina PBTN (Boina Azul)", 3, 1U));
        if (this.c._rank >= 53 && this.c._inventory.getItem(100003160) == null)
          this.c._inventory.AddItem(new ItemsModel(100003160, "AUG A3 PBBR4", 3, 1U));
        if (this.c._rank >= 53 && this.c._inventory.getItem(300005082) == null)
          this.c._inventory.AddItem(new ItemsModel(300005082, "Barrett M82A1 D", 3, 1U));
        if (this.c._rank >= 53 && this.c._inventory.getItem(1001002047) == null)
          this.c._inventory.AddItem(new ItemsModel(1001002047, "Keen Ayes Garena", 3, 1U));
        if (this.c._rank >= 53 && this.c._inventory.getItem(1001001049) == null)
          this.c._inventory.AddItem(new ItemsModel(1001001049, "Tarantula Garena", 3, 1U));
        this.GetInventoryInfo();
      }
      else
        this._erro = 2147483648U;
    }

    public void GetInventoryInfo()
    {
      lock (this.c._inventory._items)
      {
        for (int index = 0; index < this.c._inventory._items.Count; ++index)
        {
          ItemsModel itemsModel = this.c._inventory._items[index];
          if (itemsModel._category == 1)
            this.weapons.Add(itemsModel);
          else if (itemsModel._category == 2)
            this.charas.Add(itemsModel);
          else if (itemsModel._category == 3)
            this.cupons.Add(itemsModel);
        }
      }
    }

    private void CheckGameEvents(EventVisitModel evVisit)
    {
      long num = long.Parse(DateTime.Now.ToString("yyMMddHHmm"));
      PlayerEvent pE = this.c._event;
      if (pE != null)
      {
        QuestModel runningEvent1 = EventQuestSyncer.GetRunningEvent();
        if (runningEvent1 != null)
        {
          long lastQuestDate = (long) pE.LastQuestDate;
          long lastQuestFinish = (long) pE.LastQuestFinish;
          if (pE.LastQuestDate < runningEvent1.startDate)
          {
            pE.LastQuestDate = 0U;
            pE.LastQuestFinish = 0;
            this.c.SendPacket((SendPacket) new SERVER_MESSAGE_EVENT_QUEST_PAK());
          }
          if (pE.LastQuestFinish == 0)
          {
            this.c._mission.mission4 = 13;
            if (pE.LastQuestDate == 0U)
              pE.LastQuestDate = (uint) num;
          }
          if ((long) pE.LastQuestDate != lastQuestDate || (long) pE.LastQuestFinish != lastQuestFinish)
            EventQuestSyncer.ResetPlayerEvent(this.c.player_id, pE);
        }
        EventLoginModel runningEvent2 = EventLoginSyncer.GetRunningEvent();
        if (runningEvent2 != null && (runningEvent2.startDate >= pE.LastLoginDate || pE.LastLoginDate >= runningEvent2.endDate))
        {
          ItemsModel modelo = new ItemsModel(runningEvent2._rewardId, runningEvent2._category, "Login event", 1, (uint) runningEvent2._count);
          PlayerManager.tryCreateItem(modelo, this.c._inventory, this.c.player_id);
          this.c.SendPacket((SendPacket) new SERVER_MESSAGE_ITEM_RECEIVE_PAK(0U));
          if (modelo._category == 1)
            this.weapons.Add(modelo);
          else if (modelo._category == 2)
            this.charas.Add(modelo);
          else if (modelo._category == 3)
            this.cupons.Add(modelo);
          ComDiv.updateDB("player_events", "last_login_date", (object) num, "player_id", (object) this.c.player_id);
        }
        if (evVisit != null && pE.LastVisitEventId != evVisit.id)
        {
          pE.LastVisitEventId = evVisit.id;
          pE.LastVisitSequence1 = 0;
          pE.LastVisitSequence2 = 0;
          pE.NextVisitDate = 0;
          EventVisitSyncer.ResetPlayerEvent(this.c.player_id, evVisit.id);
        }
        EventXmasModel runningEvent3 = EventXmasSyncer.GetRunningEvent();
        if (runningEvent3 != null)
        {
          if (pE.LastXmasRewardDate < runningEvent3.startDate)
          {
            pE.LastXmasRewardDate = 0U;
            ComDiv.updateDB("player_events", "last_xmas_reward_date", (object) 0, "player_id", (object) this.c.player_id);
          }
          if (pE.LastXmasRewardDate <= runningEvent3.startDate || pE.LastXmasRewardDate > runningEvent3.endDate)
            this._xmas = true;
        }
      }
      ComDiv.updateDB("accounts", "last_login", (object) num, "player_id", (object) this.c.player_id);
    }

    public override void write()
    {
      this.writeH((short) 2566);
      this.writeD(this._erro);
      if (this._erro != 0U)
        return;
      ServerConfig config = LoginManager.Config;
      EventVisitModel runningEvent = EventVisitSyncer.GetRunningEvent();
      this.writeC((byte) 18);
      this.writeS(this.c.player_name, 33);
      this.writeD(this.c._exp);
      this.writeD(this.c._rank);
      this.writeD(this.c._rank);
      this.writeD(this.c._gp);
      this.writeD(this.c._money);
      this.writeD(this.clan.id);
      this.writeD(this.c.clanAccess);
      this.writeQ(0L);
      this.writeC((byte) this.c.pc_cafe);
      this.writeC((byte) this.c.tourneyLevel);
      this.writeC((byte) this.c.name_color);
      this.writeS(this.clan.name, 17);
      this.writeC(this.clan.rank);
      this.writeC((byte) this.clan.getClanUnit());
      this.writeD(this.clan.logo);
      this.writeC(this.clan.nameColor);
      this.writeD(0);
      this.writeC((byte) 0);
      this.writeD(0);
      this.writeD(this.c.LastRankUpDate);
      this.writeD(this.c._statistic.fights);
      this.writeD(this.c._statistic.fights_win);
      this.writeD(this.c._statistic.fights_lost);
      this.writeD(this.c._statistic.fights_draw);
      this.writeD(this.c._statistic.kills_count);
      this.writeD(this.c._statistic.headshots_count);
      this.writeD(this.c._statistic.deaths_count);
      this.writeD(this.c._statistic.totalfights_count);
      this.writeD(this.c._statistic.totalkills_count);
      this.writeD(this.c._statistic.escapes);
      this.writeD(this.c._statistic.fights);
      this.writeD(this.c._statistic.fights_win);
      this.writeD(this.c._statistic.fights_lost);
      this.writeD(this.c._statistic.fights_draw);
      this.writeD(this.c._statistic.kills_count);
      this.writeD(this.c._statistic.headshots_count);
      this.writeD(this.c._statistic.deaths_count);
      this.writeD(this.c._statistic.totalfights_count);
      this.writeD(this.c._statistic.totalkills_count);
      this.writeD(this.c._statistic.escapes);
      this.writeD(this.c._equip._red);
      this.writeD(this.c._equip._blue);
      this.writeD(this.c._equip._helmet);
      this.writeD(this.c._equip._beret);
      this.writeD(this.c._equip._dino);
      this.writeD(this.c._equip._primary);
      this.writeD(this.c._equip._secondary);
      this.writeD(this.c._equip._melee);
      this.writeD(this.c._equip._grenade);
      this.writeD(this.c._equip._special);
      this.writeH((short) 0);
      this.writeD(this.c._bonus.fakeRank);
      this.writeD(this.c._bonus.fakeRank);
      this.writeS(this.c._bonus.fakeNick, 33);
      this.writeH((short) this.c._bonus.sightColor);
      this.writeC((byte) 31);
      this.CheckGameEvents(runningEvent);
      if (config.ClientVersion == "1.15.37")
      {
        this.writeC((byte) 1);
        this.writeD(this.charas.Count);
        this.writeD(this.weapons.Count);
        this.writeD(this.cupons.Count);
        this.writeD(0);
        for (int index = 0; index < this.charas.Count; ++index)
        {
          ItemsModel chara = this.charas[index];
          this.writeQ(chara._objId);
          this.writeD(chara._id);
          this.writeC((byte) chara._equip);
          this.writeD(chara._count);
        }
        for (int index = 0; index < this.weapons.Count; ++index)
        {
          ItemsModel weapon = this.weapons[index];
          this.writeQ(weapon._objId);
          this.writeD(weapon._id);
          this.writeC((byte) weapon._equip);
          this.writeD(weapon._count);
        }
        for (int index = 0; index < this.cupons.Count; ++index)
        {
          ItemsModel cupon = this.cupons[index];
          this.writeQ(cupon._objId);
          this.writeD(cupon._id);
          this.writeC((byte) cupon._equip);
          this.writeD(cupon._count);
        }
      }
      this.writeC(ConfigGA.Outpost);
      this.writeD(this.c.brooch);
      this.writeD(this.c.insignia);
      this.writeD(this.c.medal);
      this.writeD(this.c.blue_order);
      this.writeC((byte) this.c._mission.actualMission);
      this.writeC((byte) this.c._mission.card1);
      this.writeC((byte) this.c._mission.card2);
      this.writeC((byte) this.c._mission.card3);
      this.writeC((byte) this.c._mission.card4);
      this.writeB(ComDiv.getCardFlags(this.c._mission.mission1, this.c._mission.list1));
      this.writeB(ComDiv.getCardFlags(this.c._mission.mission2, this.c._mission.list2));
      this.writeB(ComDiv.getCardFlags(this.c._mission.mission3, this.c._mission.list3));
      this.writeB(ComDiv.getCardFlags(this.c._mission.mission4, this.c._mission.list4));
      this.writeC((byte) this.c._mission.mission1);
      this.writeC((byte) this.c._mission.mission2);
      this.writeC((byte) this.c._mission.mission3);
      this.writeC((byte) this.c._mission.mission4);
      this.writeB(this.c._mission.list1);
      this.writeB(this.c._mission.list2);
      this.writeB(this.c._mission.list3);
      this.writeB(this.c._mission.list4);
      this.writeQ(this.c._titles.Flags);
      this.writeC((byte) this.c._titles.Equiped1);
      this.writeC((byte) this.c._titles.Equiped2);
      this.writeC((byte) this.c._titles.Equiped3);
      this.writeD(this.c._titles.Slots);
      this.writeD(ConfigMaps.Tutorial);
      this.writeD(ConfigMaps.Deathmatch);
      this.writeD(ConfigMaps.Destruction);
      this.writeD(ConfigMaps.Sabotage);
      this.writeD(ConfigMaps.Supression);
      this.writeD(ConfigMaps.Defense);
      this.writeD(ConfigMaps.Challenge);
      this.writeD(ConfigMaps.Dinosaur);
      this.writeD(ConfigMaps.Sniper);
      this.writeD(ConfigMaps.Shotgun);
      this.writeD(ConfigMaps.HeadHunter);
      this.writeD(ConfigMaps.Knuckle);
      this.writeD(ConfigMaps.CrossCounter);
      this.writeD(ConfigMaps.Chaos);
      if (config.ClientVersion == "1.15.38" || config.ClientVersion == "1.15.39" || (config.ClientVersion == "1.15.41" || config.ClientVersion == "1.15.42"))
        this.writeD(ConfigMaps.TheifMode);
      this.writeC((byte) MapsXML.ModeList.Count);
      this.writeC((byte) 4);
      this.writeD(MapsXML.maps1);
      this.writeD(MapsXML.maps2);
      this.writeD(MapsXML.maps3);
      this.writeD(MapsXML.maps4);
      foreach (ushort mode in MapsXML.ModeList)
        this.writeH(mode);
      this.writeB(MapsXML.TagList.ToArray());
      this.writeC(config.missions);
      this.writeD(MissionsXML._missionPage1);
      this.writeD(0);
      this.writeD(0);
      this.writeC((byte) 0);
      this.writeH((short) 20);
      this.writeB(new byte[20]);
      this.writeD(this.c.IsGM() || this.c.HaveAcessLevel());
      this.writeD(this._xmas);
      this.writeC((byte) 1);
      this.WriteVisitEvent(runningEvent);
      if (config.ClientVersion == "1.15.39" || config.ClientVersion == "1.15.41" || config.ClientVersion == "1.15.42")
        this.writeB(new byte[9]);
      this.writeD(uint.Parse(DateTime.Now.ToString("yyMMddHHmm")));
      this.writeS("10.120.1.44", 256);
      this.writeH((short) 8085);
      this.writeC((byte) 1);
      this.writeH((short) 0);
      this.writeH((short) 1);
      this.writeC((byte) 0);
      this.writeH((short) 1);
      this.writeC((byte) 1);
      this.writeC((byte) 6);
      this.writeH((short) 4);
      this.writeC((byte) 2);
      this.writeC((byte) 1);
      this.writeC((byte) 5);
      this.writeC((byte) 3);
    }

    private void WriteVisitEvent(EventVisitModel ev)
    {
      PlayerEvent playerEvent = this.c._event;
      if (ev != null && (playerEvent.LastVisitSequence1 < (int) ev.checks && playerEvent.NextVisitDate <= int.Parse(DateTime.Now.ToString("yyMMdd")) || playerEvent.LastVisitSequence2 < (int) ev.checks && playerEvent.LastVisitSequence2 != playerEvent.LastVisitSequence1))
      {
        this.writeD(ev.id);
        this.writeC((byte) playerEvent.LastVisitSequence1);
        this.writeC((byte) playerEvent.LastVisitSequence2);
        this.writeH((short) 0);
        this.writeD(ev.startDate);
        this.writeS(ev.title, 60);
        this.writeC((byte) 2);
        this.writeC(ev.checks);
        this.writeH((short) 0);
        this.writeD(ev.id);
        this.writeD(ev.startDate);
        this.writeD(ev.endDate);
        for (int index = 0; index < 7; ++index)
        {
          VisitBox visitBox = ev.box[index];
          this.writeD(visitBox.RewardCount);
          this.writeD(visitBox.reward1.goodId);
          this.writeD(visitBox.reward2.goodId);
        }
      }
      else
        this.writeB(new byte[172]);
    }
  }
}
