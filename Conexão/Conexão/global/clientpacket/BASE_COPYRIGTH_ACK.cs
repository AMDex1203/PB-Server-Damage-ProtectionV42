
// Type: Auth.global.clientpacket.BASE_COPYRIGTH_ACK
// Assembly: pbserver_auth, Version=1.0.7796.38604, Culture=neutral, PublicKeyToken=null
// MVID: 1CFDFF97-782C-4EF8-A621-6E4FDDE32436
// Interprise: C:\Users\Cuzin\3,50pbserver_auth.exe

using Core.server;

namespace Auth.global.clientpacket
{
  public class BASE_COPYRIGTH_ACK : SendPacket
  {
    public override void write()
    {
      string name1 = "BR.UZ Server";
      string name2 = "Server Protection";
      string name3 = "Asymmetric Encryption RSA";
      this.writeH((short) 7771);
      this.writeD(20190505);
      this.writeQ(348768394698436L);
      this.writeS(name1, name1.Length + 1);
      this.writeS(name2, name2.Length + 1);
      this.writeS(name3, name3.Length + 1);
    }
  }
}
