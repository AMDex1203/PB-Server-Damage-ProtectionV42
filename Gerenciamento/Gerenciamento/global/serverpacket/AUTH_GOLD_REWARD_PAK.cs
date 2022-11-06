
// Type: Game.global.serverpacket.AUTH_GOLD_REWARD_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;

namespace Game.global.serverpacket
{
  public class AUTH_GOLD_REWARD_PAK : SendPacket
  {
    private int gp;
    private int _gpIncrease;
    private int type;

    public AUTH_GOLD_REWARD_PAK(int increase, int gold, int type)
    {
      this._gpIncrease = increase;
      this.gp = gold;
      this.type = type;
    }

    public override void write()
    {
      this.writeH((short) 561);
      this.writeD(this._gpIncrease);
      this.writeD(this.gp);
      this.writeD(this.type);
    }
  }
}
