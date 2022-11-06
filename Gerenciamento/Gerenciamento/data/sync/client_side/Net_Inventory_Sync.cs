
// Type: Game.data.sync.client_side.Net_Inventory_Sync
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.models.account.players;
using Core.server;
using Game.data.managers;
using Game.data.model;

namespace Game.data.sync.client_side
{
  public class Net_Inventory_Sync
  {
    public static void Load(ReceiveGPacket p)
    {
      long id = p.readQ();
      long num1 = p.readQ();
      int num2 = p.readD();
      int num3 = (int) p.readC();
      int num4 = (int) p.readC();
      uint num5 = p.readUD();
      Account account = AccountManager.getAccount(id, true);
      if (account == null)
        return;
      ItemsModel itemsModel = account._inventory.getItem(num1);
      if (itemsModel == null)
        account._inventory.AddItem(new ItemsModel()
        {
          _objId = num1,
          _id = num2,
          _equip = num3,
          _count = num5,
          _category = num4,
          _name = ""
        });
      else
        itemsModel._count = num5;
    }
  }
}
