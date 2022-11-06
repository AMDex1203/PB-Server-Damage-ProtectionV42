
// Type: Game.global.serverpacket.BASE_SERVER_CHANGE_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;

namespace Game.global.serverpacket
{
  public class BASE_SERVER_CHANGE_PAK : SendPacket
  {
    private int error;

    public BASE_SERVER_CHANGE_PAK(int error) => this.error = error;

    public override void write()
    {
      this.writeH((short) 2578);
      this.writeD(this.error);
    }
  }
}
