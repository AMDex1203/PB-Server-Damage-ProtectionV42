
// Type: Game.global.serverpacket.INVENTORY_ITEM_CREATE_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.managers;
using Core.models.account.players;
using Core.server;
using Game.data.model;
using Game.data.sync.server_side;
using System;
using System.Collections.Generic;

namespace Game.global.serverpacket
{
  public class INVENTORY_ITEM_CREATE_PAK : SendPacket
  {
    private int _type;
    private List<ItemsModel> weapons = new List<ItemsModel>();
    private List<ItemsModel> charas = new List<ItemsModel>();
    private List<ItemsModel> cupons = new List<ItemsModel>();

    public INVENTORY_ITEM_CREATE_PAK(int type, Account player, List<ItemsModel> items)
    {
      this._type = type;
      this.AddItems(player, items);
    }

    public INVENTORY_ITEM_CREATE_PAK(int type, Account player, ItemsModel item)
    {
      this._type = type;
      this.AddItems(player, item);
    }

    public override void write()
    {
      this.writeH((short) 3588);
      this.writeC((byte) this._type);
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
    }

    private void AddItems(Account p, List<ItemsModel> items)
    {
      try
      {
        for (int index = 0; index < items.Count; ++index)
        {
          ItemsModel itemsModel = items[index];
          ItemsModel modelo = new ItemsModel(itemsModel)
          {
            _objId = itemsModel._objId
          };
          if (this._type == 1)
            PlayerManager.tryCreateItem(modelo, p._inventory, p.player_id);
          SEND_ITEM_INFO.LoadItem(p, modelo);
          if (modelo._category == 1)
            this.weapons.Add(modelo);
          else if (modelo._category == 2)
            this.charas.Add(modelo);
          else if (modelo._category == 3)
            this.cupons.Add(modelo);
        }
      }
      catch (Exception ex)
      {
        p.Close(0);
        Logger.warning("[INVENTORY_ITEM_CREATE_PAK1] " + ex.ToString());
      }
    }

    private void AddItems(Account p, ItemsModel item)
    {
      try
      {
        ItemsModel modelo = new ItemsModel(item)
        {
          _objId = item._objId
        };
        if (this._type == 1)
          PlayerManager.tryCreateItem(modelo, p._inventory, p.player_id);
        SEND_ITEM_INFO.LoadItem(p, modelo);
        if (modelo._category == 1)
          this.weapons.Add(modelo);
        else if (modelo._category == 2)
        {
          this.charas.Add(modelo);
        }
        else
        {
          if (modelo._category != 3)
            return;
          this.cupons.Add(modelo);
        }
      }
      catch (Exception ex)
      {
        p.Close(0);
        Logger.warning("[INVENTORY_ITEM_CREATE_PAK2] " + ex.ToString());
      }
    }
  }
}
