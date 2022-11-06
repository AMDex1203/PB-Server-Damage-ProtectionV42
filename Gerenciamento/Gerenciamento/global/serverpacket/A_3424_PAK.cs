
// Type: Game.global.serverpacket.A_3424_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;

namespace Game.global.serverpacket
{
  public class A_3424_PAK : SendPacket
  {
    private uint erro;

    public A_3424_PAK(uint erro) => this.erro = erro;

    public override void write()
    {
      this.writeH((short) 3424);
      this.writeD(this.erro);
    }
  }
}
