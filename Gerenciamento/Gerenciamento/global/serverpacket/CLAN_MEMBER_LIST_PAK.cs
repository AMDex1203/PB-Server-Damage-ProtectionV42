
// Type: Game.global.serverpacket.CLAN_MEMBER_LIST_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;

namespace Game.global.serverpacket
{
  public class CLAN_MEMBER_LIST_PAK : SendPacket
  {
    private byte[] array;
    private int erro;
    private int page;
    private int count;

    public CLAN_MEMBER_LIST_PAK(int page, int count, byte[] array)
    {
      this.page = page;
      this.count = count;
      this.array = array;
    }

    public CLAN_MEMBER_LIST_PAK(int erro) => this.erro = erro;

    public override void write()
    {
      this.writeH((short) 1309);
      this.writeD(this.erro);
      if (this.erro < 0)
        return;
      this.writeC((byte) this.page);
      this.writeC((byte) this.count);
      this.writeB(this.array);
    }
  }
}
