
// Type: Game.global.serverpacket.FRIEND_ACCEPT_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;

namespace Game.global.serverpacket
{
  public class FRIEND_ACCEPT_PAK : SendPacket
  {
    private uint _erro;

    public FRIEND_ACCEPT_PAK(uint erro) => this._erro = erro;

    public override void write()
    {
      this.writeH((short) 281);
      this.writeD(this._erro);
    }
  }
}
