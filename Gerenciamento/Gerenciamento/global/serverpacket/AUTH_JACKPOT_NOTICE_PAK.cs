
// Type: Game.global.serverpacket.AUTH_JACKPOT_NOTICE_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;

namespace Game.global.serverpacket
{
  public class AUTH_JACKPOT_NOTICE_PAK : SendPacket
  {
    private string _w;
    private int cupomId;
    private int _random;

    public AUTH_JACKPOT_NOTICE_PAK(string winner, int cupom, int rnd)
    {
      this._w = winner;
      this.cupomId = cupom;
      this._random = rnd;
    }

    public override void write()
    {
      this.writeH((short) 557);
      this.writeC((byte) (this._w.Length + 1));
      this.writeS(this._w, this._w.Length + 1);
      this.writeD(this.cupomId);
      this.writeC((byte) this._random);
    }
  }
}
