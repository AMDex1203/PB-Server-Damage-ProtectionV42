
// Type: Game.global.serverpacket.EVENT_VISIT_REWARD_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.models.enums.errors;
using Core.server;

namespace Game.global.serverpacket
{
  public class EVENT_VISIT_REWARD_PAK : SendPacket
  {
    private uint _erro;

    public EVENT_VISIT_REWARD_PAK(EventErrorEnum erro) => this._erro = (uint) erro;

    public override void write()
    {
      this.writeH((short) 2664);
      this.writeD(this._erro);
    }
  }
}
