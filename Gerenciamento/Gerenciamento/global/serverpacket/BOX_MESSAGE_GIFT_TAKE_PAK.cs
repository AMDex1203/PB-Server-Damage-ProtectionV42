
// Type: Game.global.serverpacket.BOX_MESSAGE_GIFT_TAKE_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.managers;
using Core.models.account.players;
using Core.server;
using Game.data.model;
using System.Collections.Generic;

namespace Game.global.serverpacket
{
  public class BOX_MESSAGE_GIFT_TAKE_PAK : SendPacket
  {
    private uint _erro;
    private List<ItemsModel> charas = new List<ItemsModel>();
    private List<ItemsModel> weapons = new List<ItemsModel>();
    private List<ItemsModel> cupons = new List<ItemsModel>();

    public BOX_MESSAGE_GIFT_TAKE_PAK(uint erro, ItemsModel item = null, Account p = null)
    {
      this._erro = erro;
      if (this._erro != 1U)
        return;
      this.get(item, p);
    }

    public override void write()
    {
      this.writeH((short) 541);
      this.writeD(this._erro);
      if (this._erro != 1U)
        return;
      this.writeD(this.charas.Count);
      this.writeD(this.weapons.Count);
      this.writeD(this.cupons.Count);
      this.writeD(0);
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

    private void get(ItemsModel item, Account p)
    {
      try
      {
        ItemsModel modelo = new ItemsModel(item)
        {
          _objId = item._objId
        };
        PlayerManager.tryCreateItem(modelo, p._inventory, p.player_id);
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
      catch
      {
        p.Close(0);
      }
    }
  }
}
