
// Type: Auth.global.serverpacket.BASE_USER_GIFT_LIST_PAK
// Assembly: pbserver_auth, Version=1.0.7796.38604, Culture=neutral, PublicKeyToken=null
// MVID: 1CFDFF97-782C-4EF8-A621-6E4FDDE32436
// Interprise: C:\Users\Cuzin\3,50pbserver_auth.exe

using Core.models.account;
using Core.server;
using System;
using System.Collections.Generic;

namespace Auth.global.serverpacket
{
  public class BASE_USER_GIFT_LIST_PAK : SendPacket
  {
    private int erro;
    private List<Message> gifts;

    public BASE_USER_GIFT_LIST_PAK(int erro, List<Message> gifts)
    {
      this.erro = erro;
      this.gifts = gifts;
    }

    public override void write()
    {
      this.writeH((short) 529);
      this.writeD(uint.Parse(DateTime.Now.ToString("yyMMddHHmm")));
      this.writeQ(0L);
      this.writeD(this.erro);
      this.writeC((byte) 0);
      this.writeC((byte) this.gifts.Count);
      this.writeC((byte) 0);
      foreach (Message gift in this.gifts)
      {
        this.writeD(gift.object_id);
        this.writeD((uint) gift.sender_id);
        this.writeD(gift.state);
        this.writeD((uint) gift.expireDate);
        this.writeC((byte) (gift.sender_name.Length + 1));
        this.writeS(gift.sender_name, gift.sender_name.Length + 1);
        this.writeC((byte) 1);
        this.writeS("", 1);
      }
    }
  }
}
