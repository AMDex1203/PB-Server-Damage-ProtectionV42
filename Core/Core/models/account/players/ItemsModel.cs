
// Type: Core.models.account.players.ItemsModel
// Assembly: Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 263E2FBF-1098-4552-8FB3-F6A660503737
// Interprise: C:\Users\Cuzin\3,50Core.dll

using Core.server;

namespace Core.models.account.players
{
  public class ItemsModel
  {
    public int _id;
    public int _category;
    public int _equip;
    public string _name;
    public long _objId;
    public uint _count;

    public ItemsModel DeepCopy() => (ItemsModel) this.MemberwiseClone();

    public ItemsModel()
    {
    }

    public ItemsModel(int id) => this.SetItemId(id);

    public ItemsModel(int id, string name, int equip, uint count, long objId = 0)
    {
      this._objId = objId;
      this.SetItemId(id);
      this._name = name;
      this._equip = equip;
      this._count = count;
    }

    public ItemsModel(int id, int category, string name, int equip, uint count, long objId = 0)
    {
      this._objId = objId;
      this._id = id;
      this._category = category;
      this._name = name;
      this._equip = equip;
      this._count = count;
    }

    public ItemsModel(ItemsModel item)
    {
      this._id = item._id;
      this._category = item._category;
      this._name = item._name;
      this._count = item._count;
      this._equip = item._equip;
    }

    public void SetItemId(int id)
    {
      this._id = id;
      this._category = ComDiv.GetItemCategory(id);
    }
  }
}
