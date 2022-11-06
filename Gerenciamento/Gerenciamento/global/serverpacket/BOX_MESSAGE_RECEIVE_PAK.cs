
// Type: Game.global.serverpacket.BOX_MESSAGE_RECEIVE_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.models.account;
using Core.models.enums;
using Core.server;

namespace Game.global.serverpacket
{
  public class BOX_MESSAGE_RECEIVE_PAK : SendPacket
  {
    private Message msg;

    public BOX_MESSAGE_RECEIVE_PAK(Message msg) => this.msg = msg;

    public override void write()
    {
      this.writeH((short) 427);
      this.writeD(this.msg.object_id);
      this.writeQ(this.msg.sender_id);
      this.writeC((byte) this.msg.type);
      this.writeC((byte) this.msg.state);
      this.writeC((byte) this.msg.DaysRemaining);
      this.writeD(this.msg.clanId);
      this.writeC((byte) (this.msg.sender_name.Length + 1));
      this.writeC(this.msg.type == 5 || this.msg.type == 4 && this.msg.cB != NoteMessageClan.None ? (byte) 0 : (byte) (this.msg.text.Length + 1));
      this.writeS(this.msg.sender_name, this.msg.sender_name.Length + 1);
      if (this.msg.type == 5 || this.msg.type == 4)
      {
        if (this.msg.cB >= NoteMessageClan.JoinAccept && this.msg.cB <= NoteMessageClan.Secession)
        {
          this.writeC((byte) (this.msg.text.Length + 1));
          this.writeC((byte) this.msg.cB);
          this.writeS(this.msg.text, this.msg.text.Length + 1);
        }
        else if (this.msg.cB == NoteMessageClan.None)
        {
          this.writeS(this.msg.text, this.msg.text.Length + 1);
        }
        else
        {
          this.writeC((byte) 2);
          this.writeH((short) this.msg.cB);
        }
      }
      else
        this.writeS(this.msg.text, this.msg.text.Length + 1);
    }
  }
}
