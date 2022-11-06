
// Type: Game.global.serverpacket.ROOM_INVITE_RETURN_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;

namespace Game.global.serverpacket
{
  public class ROOM_INVITE_RETURN_PAK : SendPacket
  {
    private uint _erro;

    public ROOM_INVITE_RETURN_PAK(uint erro) => this._erro = erro;

    public override void write()
    {
      this.writeH((short) 3885);
      this.writeD(this._erro);
    }
  }
}
