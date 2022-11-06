
// Type: Game.global.serverpacket.SERVER_MESSAGE_ANNOUNCE_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.server;

namespace Game.global.serverpacket
{
  public class SERVER_MESSAGE_ANNOUNCE_PAK : SendPacket
  {
    private string _message;

    public SERVER_MESSAGE_ANNOUNCE_PAK(string msg)
    {
      this._message = msg;
      if (msg.Length < 1024)
        return;
      Logger.error("[GM] Mensagem com tamanho maior a 1024 enviada!!");
    }

    public override void write()
    {
      this.writeH((short) 2055);
      this.writeD(2);
      this.writeH((ushort) this._message.Length);
      this.writeS(this._message);
    }
  }
}
