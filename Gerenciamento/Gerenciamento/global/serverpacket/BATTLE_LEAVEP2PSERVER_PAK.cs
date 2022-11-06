
// Type: Game.global.serverpacket.BATTLE_LEAVEP2PSERVER_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;
using Game.data.model;

namespace Game.global.serverpacket
{
  public class BATTLE_LEAVEP2PSERVER_PAK : SendPacket
  {
    private Account p;
    private int type;

    public BATTLE_LEAVEP2PSERVER_PAK(Account p, int type)
    {
      this.p = p;
      this.type = type;
    }

    public override void write()
    {
      if (this.p == null)
        return;
      this.writeH((short) 3385);
      this.writeD(this.p._slotId);
      this.writeC((byte) this.type);
      this.writeD(this.p._exp);
      this.writeD(this.p._rank);
      this.writeD(this.p._gp);
      this.writeD(this.p._statistic.escapes);
      this.writeD(this.p._statistic.escapes);
    }
  }
}
