
// Type: Game.global.serverpacket.CLAN_REQUEST_ACCEPT_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;

namespace Game.global.serverpacket
{
  public class CLAN_REQUEST_ACCEPT_PAK : SendPacket
  {
    private uint result;

    public CLAN_REQUEST_ACCEPT_PAK(uint result) => this.result = result;

    public override void write()
    {
      this.writeH((short) 1327);
      this.writeD(this.result);
    }
  }
}
