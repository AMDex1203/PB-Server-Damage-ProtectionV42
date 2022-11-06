
// Type: Game.global.serverpacket.ROOM_INSPECTPLAYER_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.models.account.players;
using Core.server;
using Game.data.model;
using System.Collections.Generic;

namespace Game.global.serverpacket
{
  public class ROOM_INSPECTPLAYER_PAK : SendPacket
  {
    private Account p;

    public ROOM_INSPECTPLAYER_PAK(Account p) => this.p = p;

    public override void write()
    {
      this.writeH((short) 3893);
      this.writeD(this.p._equip._primary);
      this.writeD(this.p._equip._secondary);
      this.writeD(this.p._equip._melee);
      this.writeD(this.p._equip._grenade);
      this.writeD(this.p._equip._special);
      this.writeD(this.p._equip._red);
      this.writeD(this.p._equip._blue);
      this.writeD(this.p._equip._helmet);
      this.writeD(this.p._equip._beret);
      this.writeD(this.p._equip._dino);
      List<ItemsModel> itemsByType = this.p._inventory.getItemsByType(4);
      this.writeD(itemsByType.Count);
      foreach (ItemsModel itemsModel in itemsByType)
        this.writeD(itemsModel._id);
    }
  }
}
