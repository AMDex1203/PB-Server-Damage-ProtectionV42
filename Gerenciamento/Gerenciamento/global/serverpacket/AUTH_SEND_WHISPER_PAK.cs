
// Type: Game.global.serverpacket.AUTH_SEND_WHISPER_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;

namespace Game.global.serverpacket
{
  public class AUTH_SEND_WHISPER_PAK : SendPacket
  {
    private string name;
    private string msg;
    private uint erro;
    private int type;
    private int bantime;

    public AUTH_SEND_WHISPER_PAK(string name, string msg, uint erro)
    {
      this.name = name;
      this.msg = msg;
      this.erro = erro;
    }

    public AUTH_SEND_WHISPER_PAK(int type, int bantime)
    {
      this.type = type;
      this.bantime = bantime;
    }

    public override void write()
    {
      this.writeH((short) 291);
      this.writeC((byte) this.type);
      if (this.type == 0)
      {
        this.writeD(this.erro);
        this.writeS(this.name, 33);
        if (this.erro != 0U)
          return;
        this.writeH((ushort) (this.msg.Length + 1));
        this.writeS(this.msg, this.msg.Length + 1);
      }
      else
        this.writeD(this.bantime);
    }
  }
}
