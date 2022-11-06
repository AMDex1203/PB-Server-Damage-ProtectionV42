
// Type: Game.global.serverpacket.CLAN_CHAT_1390_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;
using Game.data.model;

namespace Game.global.serverpacket
{
  public class CLAN_CHAT_1390_PAK : SendPacket
  {
    private string sender;
    private string message;
    private int type;
    private int bantime;
    private bool isGM;

    public CLAN_CHAT_1390_PAK(Account p, string msg)
    {
      this.sender = p.player_name;
      this.message = msg;
      this.isGM = p.UseChatGM();
    }

    public CLAN_CHAT_1390_PAK(int type, int bantime)
    {
      this.type = type;
      this.bantime = bantime;
    }

    public override void write()
    {
      this.writeH((short) 1391);
      this.writeC((byte) this.type);
      if (this.type == 0)
      {
        this.writeC((byte) (this.sender.Length + 1));
        this.writeS(this.sender, this.sender.Length + 1);
        this.writeC(this.isGM);
        this.writeC((byte) (this.message.Length + 1));
        this.writeS(this.message, this.message.Length + 1);
      }
      else
        this.writeD(this.bantime);
    }
  }
}
