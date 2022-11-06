
// Type: Game.global.serverpacket.VOTEKICK_UPDATE_RESULT_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;

namespace Game.global.serverpacket
{
  public class VOTEKICK_UPDATE_RESULT_PAK : SendPacket
  {
    private uint erro;

    public VOTEKICK_UPDATE_RESULT_PAK(uint erro) => this.erro = erro;

    public override void write()
    {
      this.writeH((short) 3401);
      this.writeD(this.erro);
    }
  }
}
