
// Type: Game.global.serverpacket.A_3096_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;

namespace Game.global.serverpacket
{
  public class A_3096_PAK : SendPacket
  {
    private int XPEarned;
    private int GPEarned;

    public A_3096_PAK(int xp_earned, int gp_earned)
    {
      this.XPEarned = xp_earned;
      this.GPEarned = gp_earned;
    }

    public override void write()
    {
      this.writeH((short) 3097);
      this.writeD(this.XPEarned);
      this.writeD(this.GPEarned);
    }
  }
}
