
// Type: Game.global.serverpacket.INVENTORY_ITEM_EQUIP_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.managers;
using Core.models.account.players;
using Core.server;
using Game.data.model;
using System;

namespace Game.global.serverpacket
{
  public class INVENTORY_ITEM_EQUIP_PAK : SendPacket
  {
    private uint erro;
    private ItemsModel item;

    public INVENTORY_ITEM_EQUIP_PAK(uint erro, ItemsModel item = null, Account p = null)
    {
      this.erro = erro;
      if (erro != 1U)
        return;
      if (item != null)
      {
        switch (ComDiv.getIdStatics(item._id, 1))
        {
          case 13:
          case 15:
            if (item._count > 1U && item._equip == 1)
            {
              ComDiv.updateDB("player_items", "count", (object) (long) --item._count, "object_id", (object) item._objId, "owner_id", (object) p.player_id);
              break;
            }
            PlayerManager.DeleteItem(item._objId, p.player_id);
            p._inventory.RemoveItem(item);
            item._id = 0;
            item._count = 0U;
            break;
          default:
            item._equip = 2;
            break;
        }
        this.item = item;
      }
      else
        this.erro = 2147483648U;
    }

    public override void write()
    {
      this.writeH((short) 535);
      this.writeD(this.erro);
      if (this.erro != 1U)
        return;
      this.writeD(uint.Parse(DateTime.Now.ToString("yyMMddHHmm")));
      this.writeQ(this.item._objId);
      this.writeD(this.item._id);
      this.writeC((byte) this.item._equip);
      this.writeD(this.item._count);
    }
  }
}
