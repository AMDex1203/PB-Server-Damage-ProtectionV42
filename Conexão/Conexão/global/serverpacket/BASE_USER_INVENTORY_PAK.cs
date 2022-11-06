
// Type: Auth.global.serverpacket.BASE_USER_INVENTORY_PAK
// Assembly: pbserver_auth, Version=1.0.7796.38604, Culture=neutral, PublicKeyToken=null
// MVID: 1CFDFF97-782C-4EF8-A621-6E4FDDE32436
// Interprise: C:\Users\Cuzin\3,50pbserver_auth.exe

using Core.models.account.players;
using Core.server;
using System.Collections.Generic;

namespace Auth.global.serverpacket
{
  public class BASE_USER_INVENTORY_PAK : SendPacket
  {
    private List<ItemsModel> charas = new List<ItemsModel>();
    private List<ItemsModel> weapons = new List<ItemsModel>();
    private List<ItemsModel> cupons = new List<ItemsModel>();

    public BASE_USER_INVENTORY_PAK(List<ItemsModel> items) => this.InventoryLoad(items);

    private void InventoryLoad(List<ItemsModel> items)
    {
      for (int index = 0; index < items.Count; ++index)
      {
        ItemsModel itemsModel = items[index];
        if (itemsModel._category == 1)
          this.weapons.Add(itemsModel);
        else if (itemsModel._category == 2)
          this.charas.Add(itemsModel);
        else if (itemsModel._category == 3)
          this.cupons.Add(itemsModel);
      }
    }

    public override void write()
    {
      this.writeH((short) 2699);
      this.writeD(this.charas.Count);
      foreach (ItemsModel chara in this.charas)
      {
        this.writeQ(chara._objId);
        this.writeD(chara._id);
        this.writeC((byte) chara._equip);
        this.writeD(chara._count);
      }
      this.writeD(this.weapons.Count);
      foreach (ItemsModel weapon in this.weapons)
      {
        this.writeQ(weapon._objId);
        this.writeD(weapon._id);
        this.writeC((byte) weapon._equip);
        this.writeD(weapon._count);
      }
      this.writeD(this.cupons.Count);
      foreach (ItemsModel cupon in this.cupons)
      {
        this.writeQ(cupon._objId);
        this.writeD(cupon._id);
        this.writeC((byte) cupon._equip);
        this.writeD(cupon._count);
      }
      this.writeD(0);
    }
  }
}
