
// Type: Game.global.serverpacket.AUTH_ACCOUNT_KICK_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;

namespace Game.global.serverpacket
{
  public class AUTH_ACCOUNT_KICK_PAK : SendPacket
  {
    private int _type;

    public AUTH_ACCOUNT_KICK_PAK(int type) => this._type = type;

    public override void write()
    {
      this.writeH((short) 513);
      this.writeC((byte) this._type);
    }
  }
}
