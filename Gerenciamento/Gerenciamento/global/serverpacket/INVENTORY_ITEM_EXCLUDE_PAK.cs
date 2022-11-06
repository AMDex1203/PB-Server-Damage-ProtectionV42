
// Type: Game.global.serverpacket.INVENTORY_ITEM_EXCLUDE_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;

namespace Game.global.serverpacket
{
  public class INVENTORY_ITEM_EXCLUDE_PAK : SendPacket
  {
    private long _objId;
    private uint _erro;

    public INVENTORY_ITEM_EXCLUDE_PAK(uint erro, long objId = 0)
    {
      this._erro = erro;
      if (erro != 1U)
        return;
      this._objId = objId;
    }

    public override void write()
    {
      this.writeH((short) 543);
      this.writeD(this._erro);
      if (this._erro != 1U)
        return;
      this.writeQ(this._objId);
    }
  }
}
