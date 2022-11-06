
// Type: Game.global.serverpacket.CLAN_WAR_TEAM_CHATTING_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;

namespace Game.global.serverpacket
{
  public class CLAN_WAR_TEAM_CHATTING_PAK : SendPacket
  {
    private int type;
    private int bantime;
    private string message;
    private string sender;

    public CLAN_WAR_TEAM_CHATTING_PAK(string sender, string text)
    {
      this.sender = sender;
      this.message = text;
    }

    public CLAN_WAR_TEAM_CHATTING_PAK(int type, int bantime)
    {
      this.type = type;
      this.bantime = bantime;
    }

    public override void write()
    {
      this.writeH((short) 1577);
      this.writeC((byte) this.type);
      if (this.type == 0)
      {
        this.writeC((byte) (this.sender.Length + 1));
        this.writeS(this.sender, this.sender.Length + 1);
        this.writeC((byte) this.message.Length);
        this.writeS(this.message, this.message.Length);
      }
      else
        this.writeD(this.bantime);
    }
  }
}
