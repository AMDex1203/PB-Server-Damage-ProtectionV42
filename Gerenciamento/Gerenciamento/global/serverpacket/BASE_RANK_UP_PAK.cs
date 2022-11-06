
// Type: Game.global.serverpacket.BASE_RANK_UP_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;

namespace Game.global.serverpacket
{
  public class BASE_RANK_UP_PAK : SendPacket
  {
    private int _rank;
    private int _allExp;

    public BASE_RANK_UP_PAK(int rank, int allExp)
    {
      this._rank = rank;
      this._allExp = allExp;
    }

    public override void write()
    {
      this.writeH((short) 2614);
      this.writeD(this._rank);
      this.writeD(this._rank);
      this.writeD(this._allExp);
    }
  }
}
