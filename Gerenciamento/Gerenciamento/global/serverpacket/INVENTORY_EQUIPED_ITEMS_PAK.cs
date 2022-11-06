
// Type: Game.global.serverpacket.INVENTORY_EQUIPED_ITEMS_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.managers;
using Core.models.account.players;
using Core.models.enums.flags;
using Core.server;
using Game.data.model;
using System;

namespace Game.global.serverpacket
{
  public class INVENTORY_EQUIPED_ITEMS_PAK : SendPacket
  {
    private InventoryFlag type;
    private PlayerEquipedItems equip;

    public INVENTORY_EQUIPED_ITEMS_PAK(Account p)
    {
      this.type = (InventoryFlag) PlayerManager.CheckEquipedItems(p._equip, p._inventory._items);
      this.equip = p._equip;
    }

    public INVENTORY_EQUIPED_ITEMS_PAK(Account p, int type)
    {
      this.type = (InventoryFlag) type;
      this.equip = p._equip;
    }

    public override void write()
    {
      this.writeH((short) 2058);
      this.writeD((int) this.type);
      if (this.type.HasFlag((Enum) InventoryFlag.Character))
      {
        this.writeD(this.equip._red);
        this.writeD(this.equip._blue);
        this.writeD(this.equip._helmet);
        this.writeD(this.equip._beret);
        this.writeD(this.equip._dino);
      }
      if (!this.type.HasFlag((Enum) InventoryFlag.Weapon))
        return;
      this.writeD(this.equip._primary);
      this.writeD(this.equip._secondary);
      this.writeD(this.equip._melee);
      this.writeD(this.equip._grenade);
      this.writeD(this.equip._special);
    }
  }
}
