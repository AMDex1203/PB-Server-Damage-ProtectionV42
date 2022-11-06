
// Type: Game.global.serverpacket.AUTH_WEB_CASH_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;

namespace Game.global.serverpacket
{
  public class AUTH_WEB_CASH_PAK : SendPacket
  {
    private int erro;
    private int gold;
    private int cash;

    public AUTH_WEB_CASH_PAK(int erro, int gold = 0, int cash = 0)
    {
      this.erro = erro;
      this.gold = gold;
      this.cash = cash;
    }

    public override void write()
    {
      this.writeH((short) 545);
      this.writeD(this.erro);
      if (this.erro < 0)
        return;
      this.writeD(this.gold);
      this.writeD(this.cash);
    }
  }
}
