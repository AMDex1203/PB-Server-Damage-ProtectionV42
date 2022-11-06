
// Type: Game.global.serverpacket.BATTLE_PLAYED_TIME_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.managers.events;
using Core.server;

namespace Game.global.serverpacket
{
  public class BATTLE_PLAYED_TIME_PAK : SendPacket
  {
    private int _type;
    private PlayTimeModel ev;

    public BATTLE_PLAYED_TIME_PAK(int type, PlayTimeModel eventPt)
    {
      this._type = type;
      this.ev = eventPt;
    }

    public override void write()
    {
      this.writeH((short) 3911);
      this.writeC((byte) this._type);
      this.writeS(this.ev._title, 30);
      this.writeD(this.ev._goodReward1);
      this.writeD(this.ev._goodReward2);
      this.writeQ(this.ev._time);
    }
  }
}
