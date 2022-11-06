
// Type: Core.models.shop.GoodItem
// Assembly: Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 263E2FBF-1098-4552-8FB3-F6A660503737
// Interprise: C:\Users\Cuzin\3,50Core.dll

using Core.models.account.players;

namespace Core.models.shop
{
  public class GoodItem
  {
    public int price_gold;
    public int price_cash;
    public int auth_type;
    public int buy_type2;
    public int buy_type3;
    public int id;
    public int tag;
    public int title;
    public int visibility;
    public ItemsModel _item = new ItemsModel()
    {
      _equip = 1
    };
  }
}
