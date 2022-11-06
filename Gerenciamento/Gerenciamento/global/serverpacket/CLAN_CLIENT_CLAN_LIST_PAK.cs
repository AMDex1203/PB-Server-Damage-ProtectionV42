
// Type: Game.global.serverpacket.CLAN_CLIENT_CLAN_LIST_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;

namespace Game.global.serverpacket
{
  public class CLAN_CLIENT_CLAN_LIST_PAK : SendPacket
  {
    private uint _page;
    private int _count;
    private byte[] _array;

    public CLAN_CLIENT_CLAN_LIST_PAK(uint page, int count, byte[] array)
    {
      this._page = page;
      this._count = count;
      this._array = array;
    }

    public override void write()
    {
      this.writeH((short) 1446);
      this.writeD(this._page);
      this.writeC((byte) this._count);
      this.writeB(this._array);
    }
  }
}
