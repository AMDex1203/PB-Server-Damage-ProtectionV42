
// Type: Game.global.serverpacket.SHOP_GET_GOODS_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.managers;
using Core.server;

namespace Game.global.serverpacket
{
  public class SHOP_GET_GOODS_PAK : SendPacket
  {
    private int _tudo;
    private ShopData data;

    public SHOP_GET_GOODS_PAK(ShopData data, int tudo)
    {
      this.data = data;
      this._tudo = tudo;
    }

    public override void write()
    {
      this.writeH((short) 523);
      this.writeD(this._tudo);
      this.writeD(this.data.ItemsCount);
      this.writeD(this.data.Offset);
      this.writeB(this.data.Buffer);
      this.writeD(44);
    }
  }
}
