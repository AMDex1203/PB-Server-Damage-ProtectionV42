
// Type: Game.global.serverpacket.SHOP_BUY_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.managers;
using Core.models.account.players;
using Core.models.shop;
using Core.server;
using Game.data.model;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Game.global.serverpacket
{
  public class SHOP_BUY_PAK : SendPacket
  {
    private List<ItemsModel> weapons = new List<ItemsModel>();
    private List<ItemsModel> charas = new List<ItemsModel>();
    private List<ItemsModel> cupons = new List<ItemsModel>();
    private Account p;
    private uint erro;

    public SHOP_BUY_PAK(uint erro, List<GoodItem> item = null, Account player = null)
    {
      this.erro = erro;
      if (this.erro != 1U)
        return;
      this.p = player;
      this.AddItems(item);
    }

    public override void write()
    {
      this.writeH((short) 531);
      this.writeD(this.erro);
      if (this.erro != 1U)
        return;
      this.writeD(uint.Parse(DateTime.Now.ToString("yyMMddHHmm")));
      this.writeD(this.charas.Count);
      this.writeD(this.weapons.Count);
      this.writeD(this.cupons.Count);
      foreach (ItemsModel chara in this.charas)
      {
        this.writeQ(chara._objId);
        this.writeD(chara._id);
        this.writeC((byte) chara._equip);
        this.writeD(chara._count);
      }
      foreach (ItemsModel weapon in this.weapons)
      {
        this.writeQ(weapon._objId);
        this.writeD(weapon._id);
        this.writeC((byte) weapon._equip);
        this.writeD(weapon._count);
      }
      foreach (ItemsModel cupon in this.cupons)
      {
        this.writeQ(cupon._objId);
        this.writeD(cupon._id);
        this.writeC((byte) cupon._equip);
        this.writeD(cupon._count);
      }
      this.writeD(this.p._gp);
      this.writeD(this.p._money);
    }

    private void AddItems(List<GoodItem> items)
    {
      GoodItem goodItem1 = (GoodItem) null;
      try
      {
        foreach (GoodItem goodItem2 in items)
        {
          goodItem1 = goodItem2;
          ItemsModel itemsModel1 = this.p._inventory.getItem(goodItem2._item._id);
          ItemsModel itemsModel2 = new ItemsModel(goodItem2._item);
          if (itemsModel1 == null)
          {
            if (PlayerManager.CreateItem(itemsModel2, this.p.player_id))
              this.p._inventory.AddItem(itemsModel2);
          }
          else
          {
            itemsModel2._count = itemsModel1._count;
            itemsModel2._objId = itemsModel1._objId;
            if (itemsModel1._equip == 1)
            {
              itemsModel2._count += goodItem2._item._count;
              ComDiv.updateDB("player_items", "count", (object) (long) itemsModel2._count, "owner_id", (object) this.p.player_id, "item_id", (object) itemsModel2._id);
            }
            else if (itemsModel1._equip == 2 && itemsModel2._category != 3)
            {
              DateTime exact = DateTime.ParseExact(itemsModel1._count.ToString(), "yyMMddHHmm", (IFormatProvider) CultureInfo.InvariantCulture);
              itemsModel2._count = uint.Parse(exact.AddSeconds((double) goodItem2._item._count).ToString("yyMMddHHmm"));
              ComDiv.updateDB("player_items", "count", (object) (long) itemsModel2._count, "owner_id", (object) this.p.player_id, "item_id", (object) itemsModel2._id);
            }
            itemsModel2._equip = itemsModel1._equip;
            itemsModel1._count = itemsModel2._count;
          }
          if (itemsModel2._category == 1)
            this.weapons.Add(itemsModel2);
          else if (itemsModel2._category == 2)
            this.charas.Add(itemsModel2);
          else if (itemsModel2._category == 3)
            this.cupons.Add(itemsModel2);
        }
      }
      catch (Exception ex)
      {
        this.erro = 2147487767U;
        Logger.warning("[SHOP_BUY_PAK] " + ex.ToString());
        if (goodItem1 == null)
          return;
        Logger.warning("[SHOP_BUY_PAK] Good: " + goodItem1.id.ToString());
      }
    }
  }
}
