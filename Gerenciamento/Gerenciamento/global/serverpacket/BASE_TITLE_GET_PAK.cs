
// Type: Game.global.serverpacket.BASE_TITLE_GET_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;

namespace Game.global.serverpacket
{
  public class BASE_TITLE_GET_PAK : SendPacket
  {
    private int _slots;
    private uint _erro;

    public BASE_TITLE_GET_PAK(uint erro, int slots)
    {
      this._erro = erro;
      this._slots = slots;
    }

    public override void write()
    {
      this.writeH((short) 2620);
      this.writeD(this._erro);
      this.writeD(this._slots);
    }
  }
}
