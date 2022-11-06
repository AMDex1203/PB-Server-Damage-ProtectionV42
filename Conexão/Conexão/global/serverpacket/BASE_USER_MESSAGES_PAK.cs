
// Type: Auth.global.serverpacket.BASE_USER_MESSAGES_PAK
// Assembly: pbserver_auth, Version=1.0.7796.38604, Culture=neutral, PublicKeyToken=null
// MVID: 1CFDFF97-782C-4EF8-A621-6E4FDDE32436
// Interprise: C:\Users\Cuzin\3,50pbserver_auth.exe

using Core.models.account;
using Core.models.enums;
using Core.server;
using System.Collections.Generic;

namespace Auth.global.serverpacket
{
  public class BASE_USER_MESSAGES_PAK : SendPacket
  {
    private int pageIdx;
    private List<Message> msgs;

    public BASE_USER_MESSAGES_PAK(int pageIdx, List<Message> msgs)
    {
      this.pageIdx = pageIdx;
      this.msgs = new List<Message>();
      int num = 0;
      for (int index = pageIdx * 25; index < msgs.Count; ++index)
      {
        this.msgs.Add(msgs[index]);
        if (++num == 25)
          break;
      }
    }

    public override void write()
    {
      this.writeH((short) 421);
      this.writeC((byte) this.pageIdx);
      this.writeC((byte) this.msgs.Count);
      for (int index = 0; index < this.msgs.Count; ++index)
      {
        Message msg = this.msgs[index];
        this.writeD(msg.object_id);
        this.writeQ(msg.sender_id);
        this.writeC((byte) msg.type);
        this.writeC((byte) msg.state);
        this.writeC((byte) msg.DaysRemaining);
        this.writeD(msg.clanId);
      }
      for (int index = 0; index < this.msgs.Count; ++index)
      {
        Message msg = this.msgs[index];
        this.writeC((byte) (msg.sender_name.Length + 1));
        this.writeC(msg.type == 5 || msg.type == 4 && msg.cB != NoteMessageClan.None ? (byte) 0 : (byte) (msg.text.Length + 1));
        this.writeS(msg.sender_name, msg.sender_name.Length + 1);
        if (msg.type == 5 || msg.type == 4)
        {
          if (msg.cB >= NoteMessageClan.JoinAccept && msg.cB <= NoteMessageClan.Secession)
          {
            this.writeC((byte) (msg.text.Length + 1));
            this.writeC((byte) msg.cB);
            this.writeS(msg.text, msg.text.Length);
          }
          else if (msg.cB == NoteMessageClan.None)
          {
            this.writeS(msg.text, msg.text.Length + 1);
          }
          else
          {
            this.writeC((byte) 2);
            this.writeH((short) msg.cB);
          }
        }
        else
          this.writeS(msg.text, msg.text.Length + 1);
      }
    }
  }
}
