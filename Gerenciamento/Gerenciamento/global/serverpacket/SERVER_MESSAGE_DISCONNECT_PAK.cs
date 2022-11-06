
// Type: Game.global.serverpacket.SERVER_MESSAGE_DISCONNECT_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;
using System;

namespace Game.global.serverpacket
{
  public class SERVER_MESSAGE_DISCONNECT_PAK : SendPacket
  {
    private uint _erro;
    private bool type;

    public SERVER_MESSAGE_DISCONNECT_PAK(uint erro, bool HackUse)
    {
      this._erro = erro;
      this.type = HackUse;
    }

    public override void write()
    {
      this.writeH((short) 2062);
      this.writeD(uint.Parse(DateTime.Now.ToString("MMddHHmmss")));
      this.writeD(this._erro);
      this.writeD(this.type);
      if (!this.type)
        return;
      this.writeD(0);
    }
  }
}
