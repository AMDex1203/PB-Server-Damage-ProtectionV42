
// Type: Game.global.serverpacket.LOBBY_CHATTING_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;
using Game.data.model;

namespace Game.global.serverpacket
{
  public class LOBBY_CHATTING_PAK : SendPacket
  {
    private string sender;
    private string msg;
    private uint sessionId;
    private int nameColor;
    private bool GMColor;

    public LOBBY_CHATTING_PAK(Account player, string message, bool GMCmd = false)
    {
      if (!GMCmd)
      {
        this.nameColor = player.name_color;
        this.GMColor = player.UseChatGM();
      }
      else
        this.GMColor = true;
      this.sender = player.player_name;
      this.sessionId = player.getSessionId();
      this.msg = message;
    }

    public LOBBY_CHATTING_PAK(
      string snd,
      uint session,
      int name_color,
      bool chatGm,
      string message)
    {
      this.sender = snd;
      this.sessionId = session;
      this.nameColor = name_color;
      this.GMColor = chatGm;
      this.msg = message;
    }

    public override void write()
    {
      this.writeH((short) 3093);
      this.writeD(this.sessionId);
      this.writeC((byte) (this.sender.Length + 1));
      this.writeS(this.sender, this.sender.Length + 1);
      this.writeC((byte) this.nameColor);
      this.writeC(this.GMColor);
      this.writeH((ushort) (this.msg.Length + 1));
      this.writeS(this.msg, this.msg.Length + 1);
    }
  }
}
