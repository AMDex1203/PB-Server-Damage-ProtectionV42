
// Type: Game.global.clientpacket.INVENTORY_ITEM_EQUIP_REC
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.managers;
using Core.models.account.clan;
using Core.models.account.players;
using Core.models.enums.flags;
using Core.models.randombox;
using Core.server;
using Core.xml;
using Game.data.managers;
using Game.data.model;
using Game.global.serverpacket;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Game.global.clientpacket
{
  public class INVENTORY_ITEM_EQUIP_REC : ReceiveGamePacket
  {
    private long objId;
    private int itemId;
    private uint erro = 1;
    private uint oldCOUNT;
    private static readonly Random getrandom = new Random();
    private static readonly object syncLock = new object();

    public INVENTORY_ITEM_EQUIP_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read() => this.objId = this.readQ();

    public override void run()
    {
      if (this._client == null)
        return;
      if (this._client._player == null)
        return;
      try
      {
        Account player = this._client._player;
        ItemsModel itemsModel1 = player._inventory.getItem(this.objId);
        if (itemsModel1 != null)
        {
          this.itemId = itemsModel1._id;
          this.oldCOUNT = itemsModel1._count;
          if (itemsModel1._category == 3 && player._inventory._items.Count >= 500)
          {
            this._client.SendPacket((SendPacket) new INVENTORY_ITEM_EQUIP_PAK(2147487785U));
            Logger.warning("O inventário do jogador: '" + player.player_name + "' está cheio demais!");
            return;
          }
          if (this.itemId == 1301049000)
          {
            if (PlayerManager.updateKD(player.player_id, 0, 0, player._statistic.headshots_count, player._statistic.totalkills_count))
            {
              player._statistic.kills_count = 0;
              player._statistic.deaths_count = 0;
              this._client.SendPacket((SendPacket) new BASE_USER_CHANGE_STATS_PAK(player._statistic));
            }
            else
              this.erro = 2147483648U;
          }
          else if (this.itemId == 1301048000)
          {
            if (PlayerManager.updateFights(0, 0, 0, 0, player._statistic.totalfights_count, player.player_id))
            {
              player._statistic.fights = 0;
              player._statistic.fights_win = 0;
              player._statistic.fights_lost = 0;
              player._statistic.fights_draw = 0;
              this._client.SendPacket((SendPacket) new BASE_USER_CHANGE_STATS_PAK(player._statistic));
            }
            else
              this.erro = 2147483648U;
          }
          else if (this.itemId == 1301050000)
          {
            if (ComDiv.updateDB("accounts", "escapes", (object) 0, "player_id", (object) player.player_id))
            {
              player._statistic.escapes = 0;
              this._client.SendPacket((SendPacket) new BASE_USER_CHANGE_STATS_PAK(player._statistic));
            }
            else
              this.erro = 2147483648U;
          }
          else if (this.itemId == 1301053000)
          {
            if (PlayerManager.updateClanBattles(player.clanId, 0, 0, 0))
            {
              Clan clan = ClanManager.getClan(player.clanId);
              if (clan.id > 0 && clan.ownerId == this._client.player_id)
              {
                clan.partidas = 0;
                clan.vitorias = 0;
                clan.derrotas = 0;
                this._client.SendPacket((SendPacket) new CLAN_CHANGE_FIGHTS_PAK());
              }
              else
                this.erro = 2147483648U;
            }
            else
              this.erro = 2147483648U;
          }
          else if (this.itemId == 1301055000)
          {
            Clan clan = ClanManager.getClan(player.clanId);
            if (clan.id > 0 && clan.ownerId == this._client.player_id)
            {
              if (clan.maxPlayers + 50 <= 250 && ComDiv.updateDB("clan_data", "max_players", (object) (clan.maxPlayers + 50), "clan_id", (object) player.clanId))
              {
                clan.maxPlayers += 50;
                this._client.SendPacket((SendPacket) new CLAN_CHANGE_MAX_PLAYERS_PAK(clan.maxPlayers));
              }
              else
                this.erro = 2147487830U;
            }
            else
              this.erro = 2147487830U;
          }
          else if (this.itemId == 1301056000)
          {
            Clan clan = ClanManager.getClan(player.clanId);
            if (clan.id > 0 && (double) clan.pontos != 1000.0)
            {
              if (ComDiv.updateDB("clan_data", "pontos", (object) 1000f, "clan_id", (object) player.clanId))
              {
                clan.pontos = 1000f;
                this._client.SendPacket((SendPacket) new CLAN_CHANGE_POINTS_PAK());
              }
              else
                this.erro = 2147487830U;
            }
            else
              this.erro = 2147487830U;
          }
          else if (this.itemId > 1301113000 && this.itemId < 1301119000)
          {
            int increase = this.itemId == 1301114000 ? 500 : (this.itemId == 1301115000 ? 1000 : (this.itemId == 1301116000 ? 5000 : (this.itemId == 1301117000 ? 10000 : 30000)));
            if (ComDiv.updateDB("accounts", "gp", (object) (player._gp + increase), "player_id", (object) player.player_id))
            {
              player._gp += increase;
              this._client.SendPacket((SendPacket) new AUTH_GOLD_REWARD_PAK(increase, player._gp, 0));
            }
            else
              this.erro = 2147483648U;
          }
          else if (this.itemId == 1301999000)
          {
            if (ComDiv.updateDB("accounts", "exp", (object) (player._exp + 515999), "player_id", (object) player.player_id))
            {
              player._exp += 515999;
              this._client.SendPacket((SendPacket) new A_3096_PAK(515999, 0));
            }
            else
              this.erro = 2147483648U;
          }
          else if (itemsModel1._category == 3 && RandomBoxXML.ContainsBox(this.itemId))
          {
            RandomBoxModel box = RandomBoxXML.getBox(this.itemId);
            if (box != null)
            {
              List<RandomBoxItem> sortedList = box.getSortedList(INVENTORY_ITEM_EQUIP_REC.GetRandomNumber(1, 100));
              List<RandomBoxItem> rewardList = box.getRewardList(sortedList, INVENTORY_ITEM_EQUIP_REC.GetRandomNumber(0, sortedList.Count));
              if (rewardList.Count > 0)
              {
                int index1 = rewardList[0].index;
                this._client.SendPacket((SendPacket) new AUTH_RANDOM_BOX_REWARD_PAK(this.itemId, index1));
                List<ItemsModel> items = new List<ItemsModel>();
                for (int index2 = 0; index2 < rewardList.Count; ++index2)
                {
                  RandomBoxItem randomBoxItem = rewardList[index2];
                  if (randomBoxItem.item != null)
                    items.Add(randomBoxItem.item);
                  else if (PlayerManager.updateAccountGold(player.player_id, player._gp + (int) randomBoxItem.count))
                  {
                    player._gp += (int) randomBoxItem.count;
                    this._client.SendPacket((SendPacket) new AUTH_GOLD_REWARD_PAK((int) randomBoxItem.count, player._gp, 0));
                  }
                  else
                  {
                    this.erro = 2147483648U;
                    break;
                  }
                  if (randomBoxItem.special)
                  {
                    using (AUTH_JACKPOT_NOTICE_PAK jackpotNoticePak = new AUTH_JACKPOT_NOTICE_PAK(player.player_name, this.itemId, index1))
                      GameManager.SendPacketToAllClients((SendPacket) jackpotNoticePak);
                  }
                }
                if (items.Count > 0)
                  this._client.SendPacket((SendPacket) new INVENTORY_ITEM_CREATE_PAK(1, player, items));
              }
              else
                this.erro = 2147483648U;
            }
            else
              this.erro = 2147483648U;
          }
          else
          {
            int idStatics = ComDiv.getIdStatics(itemsModel1._id, 1);
            if (idStatics <= 11)
            {
              if (itemsModel1._equip == 1)
              {
                itemsModel1._equip = 2;
                ItemsModel itemsModel2 = itemsModel1;
                DateTime dateTime = DateTime.Now;
                dateTime = dateTime.AddSeconds((double) itemsModel1._count);
                int num = (int) uint.Parse(dateTime.ToString("yyMMddHHmm"));
                itemsModel2._count = (uint) num;
                ComDiv.updateDB("player_items", "object_id", (object) this.objId, "owner_id", (object) player.player_id, new string[2]
                {
                  "count",
                  "equip"
                }, (object) (long) itemsModel1._count, (object) itemsModel1._equip);
              }
              else
                this.erro = 2147483648U;
            }
            else if (idStatics == 13)
              this.CupomIncreaseDays(player, itemsModel1._name);
            else if (idStatics == 15)
            {
              this.cupomIncreaseGold(player, itemsModel1._id);
            }
            else
            {
              this._client.SendCompletePacket(PackageDataManager.INVENTORY_ITEM_EQUIP_0x80000000_PAK);
              return;
            }
          }
        }
        else
          this.erro = 2147483648U;
        this._client.SendPacket((SendPacket) new INVENTORY_ITEM_EQUIP_PAK(this.erro, itemsModel1, player));
      }
      catch (OverflowException ex)
      {
        Logger.error("[Data do erro: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm") + "] Obj: " + this.objId.ToString() + "; Id do item: " + this.itemId.ToString() + "; Count na Db: " + this.oldCOUNT.ToString() + "; Id do cara: " + this._client.player_id.ToString() + "; Nick: '" + this._client._player.player_name + "'\r\n" + ex.ToString());
        this._client.SendPacket((SendPacket) new INVENTORY_ITEM_EQUIP_PAK(2147483648U));
      }
      catch (Exception ex)
      {
        Logger.info("INVENTORY_ITEM_EQUIP_REC: " + ex.ToString());
        this._client.SendPacket((SendPacket) new INVENTORY_ITEM_EQUIP_PAK(2147483648U));
      }
    }

    private static int GetRandomNumber(int min, int max)
    {
      lock (INVENTORY_ITEM_EQUIP_REC.syncLock)
        return INVENTORY_ITEM_EQUIP_REC.getrandom.Next(min, max);
    }

    private void CupomIncreaseDays(Account player, string originalName)
    {
      int itemId = this.CreateItemId(12, this.GetIdStatics(this.itemId, 2), this.GetIdStatics(this.itemId, 3), 0);
      int idStatics = this.GetIdStatics(this.itemId, 4);
      player.SetCuponsFlags();
      CupomEffects effects = player.effects;
      if (itemId == 1200065000 && (effects & (CupomEffects.Colete20 | CupomEffects.Colete10 | CupomEffects.Colete5)) > (CupomEffects) 0 || itemId == 1200079000 && (effects & (CupomEffects.Colete90 | CupomEffects.Colete10 | CupomEffects.Colete5)) > (CupomEffects) 0 || (itemId == 1200044000 && (effects & (CupomEffects.Colete90 | CupomEffects.Colete20 | CupomEffects.Colete5)) > (CupomEffects) 0 || itemId == 1200030000 && (effects & (CupomEffects.Colete90 | CupomEffects.Colete20 | CupomEffects.Colete10)) > (CupomEffects) 0) || (itemId == 1200078000 && (effects & (CupomEffects.HollowPointF | CupomEffects.HollowPoint | CupomEffects.IronBullet)) > (CupomEffects) 0 || itemId == 1200032000 && (effects & (CupomEffects.HollowPointPlus | CupomEffects.HollowPointF | CupomEffects.IronBullet)) > (CupomEffects) 0 || (itemId == 1200031000 && (effects & (CupomEffects.HollowPointPlus | CupomEffects.HollowPointF | CupomEffects.HollowPoint)) > (CupomEffects) 0 || itemId == 1200036000 && (effects & (CupomEffects.HollowPointPlus | CupomEffects.HollowPoint | CupomEffects.IronBullet)) > (CupomEffects) 0)) || (itemId == 1200028000 && effects.HasFlag((Enum) CupomEffects.HP5) || itemId == 1200040000 && effects.HasFlag((Enum) CupomEffects.HP10)))
      {
        this._client.SendCompletePacket(PackageDataManager.INVENTORY_ITEM_EQUIP_0x80000000_PAK);
      }
      else
      {
        ItemsModel itemsModel = player._inventory.getItem(itemId);
        if (itemsModel == null)
        {
          bool flag = player._bonus.AddBonuses(itemId);
          CupomFlag cupomEffect = CupomEffectManager.getCupomEffect(itemId);
          if (cupomEffect != null && cupomEffect.EffectFlag > (CupomEffects) 0 && !player.effects.HasFlag((Enum) cupomEffect.EffectFlag))
          {
            player.effects |= cupomEffect.EffectFlag;
            PlayerManager.updateCupomEffects(player.player_id, player.effects);
          }
          if (flag)
            PlayerManager.updatePlayerBonus(player.player_id, player._bonus.bonuses, player._bonus.freepass);
          this._client.SendPacket((SendPacket) new INVENTORY_ITEM_CREATE_PAK(1, player, new ItemsModel(itemId, 3, originalName + " [Active]", 2, uint.Parse(DateTime.Now.AddDays((double) idStatics).ToString("yyMMddHHmm")))));
        }
        else
        {
          DateTime dateTime = DateTime.ParseExact(itemsModel._count.ToString(), "yyMMddHHmm", (IFormatProvider) CultureInfo.InvariantCulture).AddDays((double) idStatics);
          itemsModel._count = uint.Parse(dateTime.ToString("yyMMddHHmm"));
          ComDiv.updateDB("player_items", "count", (object) (long) itemsModel._count, "object_id", (object) itemsModel._objId, "owner_id", (object) player.player_id);
          this._client.SendPacket((SendPacket) new INVENTORY_ITEM_CREATE_PAK(2, player, itemsModel));
        }
      }
    }

    public int CreateItemId(int class1, int usage, int classtype, int number)
    {
      try
      {
        return class1 * 100000000 + usage * 1000000 + classtype * 1000 + number;
      }
      catch (Exception ex)
      {
        Logger.Exception(ex);
        return 0;
      }
    }

    public int GetIdStatics(int weaponId, int type)
    {
      switch (type)
      {
        case 1:
          return weaponId / 100000000;
        case 2:
          return weaponId % 100000000 / 1000000;
        case 3:
          return weaponId % 1000000 / 1000;
        case 4:
          return weaponId % 1000;
        default:
          return 0;
      }
    }

    private void cupomIncreaseGold(Account p, int cupomId)
    {
      int increase = ComDiv.getIdStatics(cupomId, 4) * 1000 + ComDiv.getIdStatics(cupomId, 3) * 100 + ComDiv.getIdStatics(cupomId, 2) * 1000000;
      if (PlayerManager.updateAccountGold(p.player_id, p._gp + increase))
      {
        p._gp += increase;
        this._client.SendPacket((SendPacket) new AUTH_GOLD_REWARD_PAK(increase, p._gp, 0));
      }
      else
        this.erro = 2147483648U;
    }
  }
}
