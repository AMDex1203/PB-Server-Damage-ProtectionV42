
// Type: Game.global.serverpacket.AUTH_RECV_WHISPER_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;

namespace Game.global.serverpacket
{
  public class AUTH_RECV_WHISPER_PAK : SendPacket
  {
    private string _sender;
    private string _msg;
    private bool chatGM;

    public AUTH_RECV_WHISPER_PAK(string sender, string msg, bool chatGM)
    {
      this._sender = sender;
      this._msg = msg;
      this.chatGM = chatGM;
    }

    public override void write()
    {
      this.writeH((short) 294);
      this.writeS(this._sender, 33);
      this.writeC(this.chatGM);
      this.writeH((ushort) (this._msg.Length + 1));
      this.writeS(this._msg, this._msg.Length + 1);
    }
  }
}
