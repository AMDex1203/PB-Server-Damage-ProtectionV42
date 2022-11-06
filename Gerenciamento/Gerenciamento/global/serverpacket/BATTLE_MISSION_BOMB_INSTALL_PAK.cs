
// Type: Game.global.serverpacket.BATTLE_MISSION_BOMB_INSTALL_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;

namespace Game.global.serverpacket
{
  public class BATTLE_MISSION_BOMB_INSTALL_PAK : SendPacket
  {
    private int _slot;
    private float _x;
    private float _y;
    private float _z;
    private byte _zone;

    public BATTLE_MISSION_BOMB_INSTALL_PAK(int slot, byte zone, float x, float y, float z)
    {
      this._zone = zone;
      this._slot = slot;
      this._x = x;
      this._y = y;
      this._z = z;
    }

    public override void write()
    {
      this.writeH((short) 3357);
      this.writeD(this._slot);
      this.writeC(this._zone);
      this.writeH((short) 42);
      this.writeT(this._x);
      this.writeT(this._y);
      this.writeT(this._z);
    }
  }
}
