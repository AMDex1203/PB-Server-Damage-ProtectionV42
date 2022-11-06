
// Type: Game.global.serverpacket.BASE_CHAT_ERROR_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;

namespace Game.global.serverpacket
{
  public class BASE_CHAT_ERROR_PAK : SendPacket
  {
    private int erro;
    private int banTime;

    public BASE_CHAT_ERROR_PAK(int erro, int time = 0)
    {
      this.erro = erro;
      this.banTime = time;
    }

    public override void write()
    {
      this.writeH((short) 2628);
      this.writeC((byte) this.erro);
      if (this.erro <= 0)
        return;
      this.writeD(this.banTime);
    }
  }
}
