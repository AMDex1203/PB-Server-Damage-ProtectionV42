
// Type: Core.models.account.players.PlayerInventory
// Assembly: Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 263E2FBF-1098-4552-8FB3-F6A660503737
// Interprise: C:\Users\Cuzin\3,50Core.dll

using Core.xml;
using System.Collections.Generic;

namespace Core.models.account.players
{
  public class PlayerInventory
  {
    public List<ItemsModel> _items = new List<ItemsModel>();

    public ItemsModel getItem(int id)
    {
      lock (this._items)
      {
        for (int index = 0; index < this._items.Count; ++index)
        {
          ItemsModel itemsModel = this._items[index];
          if (itemsModel._id == id)
            return itemsModel;
        }
      }
      return (ItemsModel) null;
    }

    public ItemsModel getItem(long obj)
    {
      lock (this._items)
      {
        for (int index = 0; index < this._items.Count; ++index)
        {
          ItemsModel itemsModel = this._items[index];
          if (itemsModel._objId == obj)
            return itemsModel;
        }
      }
      return (ItemsModel) null;
    }

    public void LoadBasicItems()
    {
      lock (this._items)
        this._items.AddRange((IEnumerable<ItemsModel>) BasicInventoryXML.basic);
    }

    public List<ItemsModel> getItemsByType(int type)
    {
      List<ItemsModel> itemsModelList = new List<ItemsModel>();
      lock (this._items)
      {
        for (int index = 0; index < this._items.Count; ++index)
        {
          ItemsModel itemsModel = this._items[index];
          if (itemsModel._category == type || itemsModel._id > 1200000000 && itemsModel._id < 1300000000 && type == 4)
            itemsModelList.Add(itemsModel);
        }
      }
      return itemsModelList;
    }

    public void RemoveItem(long obj)
    {
      lock (this._items)
      {
        for (int index = 0; index < this._items.Count; ++index)
        {
          if (this._items[index]._objId == obj)
          {
            this._items.RemoveAt(index);
            break;
          }
        }
      }
    }

    public bool RemoveItem(ItemsModel item)
    {
      lock (this._items)
        return this._items.Remove(item);
    }

    public void AddItem(ItemsModel item)
    {
      lock (this._items)
        this._items.Add(item);
    }
  }
}
