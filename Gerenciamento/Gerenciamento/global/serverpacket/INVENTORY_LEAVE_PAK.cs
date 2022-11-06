
// Type: Game.global.serverpacket.INVENTORY_LEAVE_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;

namespace Game.global.serverpacket
{
  public class INVENTORY_LEAVE_PAK : SendPacket
  {
    private int erro;
    private int type;

    public INVENTORY_LEAVE_PAK(int erro, int type = 0)
    {
      this.erro = erro;
      this.type = type;
    }

    public override void write()
    {
      this.writeH((short) 3590);
      this.writeD(this.erro);
      if (this.erro >= 0)
        return;
      this.writeD(this.type);
    }
  }
}
