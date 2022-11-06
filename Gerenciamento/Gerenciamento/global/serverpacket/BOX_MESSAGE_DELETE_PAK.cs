
// Type: Game.global.serverpacket.BOX_MESSAGE_DELETE_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;
using System.Collections.Generic;

namespace Game.global.serverpacket
{
  public class BOX_MESSAGE_DELETE_PAK : SendPacket
  {
    private uint _erro;
    private List<object> _objs;

    public BOX_MESSAGE_DELETE_PAK(uint erro, List<object> objs)
    {
      this._erro = erro;
      this._objs = objs;
    }

    public override void write()
    {
      this.writeH((short) 425);
      this.writeD(this._erro);
      this.writeC((byte) this._objs.Count);
      foreach (int num in this._objs)
        this.writeD(num);
    }
  }
}
