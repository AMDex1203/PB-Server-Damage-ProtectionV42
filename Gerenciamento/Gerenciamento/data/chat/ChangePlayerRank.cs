
// Type: Game.data.chat.ChangePlayerRank
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.managers;
using Core.models.account.players;
using Core.models.account.rank;
using Core.server;
using Core.xml;
using Game.data.managers;
using Game.data.model;
using Game.data.sync.server_side;
using Game.data.utils;
using Game.global.serverpacket;
using System;
using System.Collections.Generic;

namespace Game.data.chat
{
  public static class ChangePlayerRank
  {
    public static string SetPlayerRank(string str)
    {
      string[] strArray = str.Substring(str.IndexOf(" ") + 1).Split(' ');
      long int64 = Convert.ToInt64(strArray[0]);
      int int32 = Convert.ToInt32(strArray[1]);
      if (int32 > 60 || int32 == 56 || (int32 < 0 || int64 <= 0L))
        return Translation.GetLabel("ChangePlyRankWrongValue");
      Account account = AccountManager.getAccount(int64, 0);
      if (account == null)
        return Translation.GetLabel("ChangePlyRankFailPlayer");
      if (!ComDiv.updateDB("accounts", "rank", (object) int32, "player_id", (object) account.player_id))
        return Translation.GetLabel("ChangePlyRankFailUnk");
      RankModel rank = RankXML.getRank(int32);
      List<ItemsModel> awards = RankXML.getAwards(int32 - 1);
      account._rank = int32;
      SEND_ITEM_INFO.LoadGoldCash(account);
      if (awards.Count > 0)
        account.SendPacket((SendPacket) new INVENTORY_ITEM_CREATE_PAK(1, account, awards));
      ItemsModel itemsModel1 = account._inventory.getItem(1103003016);
      ItemsModel itemsModel2 = account._inventory.getItem(1103003010);
      ItemsModel itemsModel3 = account._inventory.getItem(100003160);
      ItemsModel itemsModel4 = account._inventory.getItem(300005082);
      ItemsModel itemsModel5 = account._inventory.getItem(1001002047);
      ItemsModel itemsModel6 = account._inventory.getItem(1001001049);
      if (account._rank >= 53 && itemsModel6 == null)
        account.SendPacket((SendPacket) new INVENTORY_ITEM_CREATE_PAK(1, account, new ItemsModel(1001001049, "Tarantula Garena", 3, 1U)));
      if (account._rank >= 53 && itemsModel5 == null)
        account.SendPacket((SendPacket) new INVENTORY_ITEM_CREATE_PAK(1, account, new ItemsModel(1001002047, "Keen Ayes Garena", 3, 1U)));
      if (account._rank >= 53 && itemsModel4 == null)
        account.SendPacket((SendPacket) new INVENTORY_ITEM_CREATE_PAK(1, account, new ItemsModel(300005082, "Barrett M82A1", 3, 1U)));
      if (account._rank >= 53 && itemsModel3 == null)
        account.SendPacket((SendPacket) new INVENTORY_ITEM_CREATE_PAK(1, account, new ItemsModel(100003160, "AUG A3 PBBR4", 3, 1U)));
      if (account._rank >= 46 && itemsModel1 == null)
        account.SendPacket((SendPacket) new INVENTORY_ITEM_CREATE_PAK(1, account, new ItemsModel(1103003016, "Boina General", 3, 1U)));
      if (account._rank >= 53 && itemsModel2 == null)
        account.SendPacket((SendPacket) new INVENTORY_ITEM_CREATE_PAK(1, account, new ItemsModel(1103003010, "Boina PBTN (Boina Azul)", 3, 1U)));
      if (account._rank < 46 && itemsModel1 != null)
      {
        PlayerManager.DeleteItem(itemsModel1._objId, account.player_id);
        account._inventory.RemoveItem(itemsModel1);
        account.SendPacket((SendPacket) new INVENTORY_ITEM_EXCLUDE_PAK(1U, itemsModel1._objId));
        int type = PlayerManager.CheckEquipedItems(account._equip, account._inventory._items);
        if (type > 0)
        {
          account._equip._beret = 0;
          account.SendPacket((SendPacket) new INVENTORY_EQUIPED_ITEMS_PAK(account, type));
          Room room = account._room;
          if (room != null)
            AllUtils.updateSlotEquips(account, room);
        }
      }
      if (account._rank < 53 && itemsModel2 != null)
      {
        PlayerManager.DeleteItem(itemsModel2._objId, account.player_id);
        account._inventory.RemoveItem(itemsModel2);
        account.SendPacket((SendPacket) new INVENTORY_ITEM_EXCLUDE_PAK(1U, itemsModel2._objId));
        int type = PlayerManager.CheckEquipedItems(account._equip, account._inventory._items);
        if (type > 0)
        {
          account._equip._beret = 0;
          account.SendPacket((SendPacket) new INVENTORY_EQUIPED_ITEMS_PAK(account, type));
          Room room = account._room;
          if (room != null)
            AllUtils.updateSlotEquips(account, room);
        }
      }
      if (account._rank < 53 && itemsModel3 != null)
      {
        PlayerManager.DeleteItem(itemsModel3._objId, account.player_id);
        account._inventory.RemoveItem(itemsModel3);
        account.SendPacket((SendPacket) new INVENTORY_ITEM_EXCLUDE_PAK(1U, itemsModel3._objId));
        int type = PlayerManager.CheckEquipedItems(account._equip, account._inventory._items);
        if (type > 0)
        {
          account._equip._beret = 0;
          account.SendPacket((SendPacket) new INVENTORY_EQUIPED_ITEMS_PAK(account, type));
          Room room = account._room;
          if (room != null)
            AllUtils.updateSlotEquips(account, room);
        }
      }
      if (account._rank < 53 && itemsModel4 != null)
      {
        PlayerManager.DeleteItem(itemsModel4._objId, account.player_id);
        account._inventory.RemoveItem(itemsModel4);
        account.SendPacket((SendPacket) new INVENTORY_ITEM_EXCLUDE_PAK(1U, itemsModel4._objId));
        int type = PlayerManager.CheckEquipedItems(account._equip, account._inventory._items);
        if (type > 0)
        {
          account._equip._beret = 0;
          account.SendPacket((SendPacket) new INVENTORY_EQUIPED_ITEMS_PAK(account, type));
          Room room = account._room;
          if (room != null)
            AllUtils.updateSlotEquips(account, room);
        }
      }
      if (account._rank < 53 && itemsModel5 != null)
      {
        PlayerManager.DeleteItem(itemsModel5._objId, account.player_id);
        account._inventory.RemoveItem(itemsModel5);
        account.SendPacket((SendPacket) new INVENTORY_ITEM_EXCLUDE_PAK(1U, itemsModel5._objId));
        int type = PlayerManager.CheckEquipedItems(account._equip, account._inventory._items);
        if (type > 0)
        {
          account._equip._beret = 0;
          account.SendPacket((SendPacket) new INVENTORY_EQUIPED_ITEMS_PAK(account, type));
          Room room = account._room;
          if (room != null)
            AllUtils.updateSlotEquips(account, room);
        }
      }
      if (account._rank < 53 && itemsModel6 != null)
      {
        PlayerManager.DeleteItem(itemsModel6._objId, account.player_id);
        account._inventory.RemoveItem(itemsModel6);
        account.SendPacket((SendPacket) new INVENTORY_ITEM_EXCLUDE_PAK(1U, itemsModel6._objId));
        int type = PlayerManager.CheckEquipedItems(account._equip, account._inventory._items);
        if (type > 0)
        {
          account._equip._beret = 0;
          account.SendPacket((SendPacket) new INVENTORY_EQUIPED_ITEMS_PAK(account, type));
          Room room = account._room;
          if (room != null)
            AllUtils.updateSlotEquips(account, room);
        }
      }
      account._exp = rank._onAllExp;
      DBQuery dbQuery = new DBQuery();
      dbQuery.AddQuery("exp", (object) account._exp);
      ComDiv.updateDB("accounts", "player_id", (object) account.player_id, dbQuery.GetTables(), dbQuery.GetValues());
      account.SendPacket((SendPacket) new BASE_RANK_UP_PAK(account._rank, rank != null ? rank._onNextLevel : 0), false);
      GameManager.SendUpToAll(account);
      if (account._room != null)
        account._room.updateSlotsInfo();
      return Translation.GetLabel("ChangePlyRankSuccess", (object) int32);
    }
  }
}
