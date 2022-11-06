
// Type: Game.global.serverpacket.PROTOCOL_BASE_COPYRIGHT_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;

namespace Game.global.serverpacket
{
  public class PROTOCOL_BASE_COPYRIGHT_PAK : SendPacket
  {
    public static byte[] BASE_COPYRIGTH_PAK;

    public override void write()
    {
      string name1 = "Shark Server";
      string name2 = "Server Protection";
      string name3 = "Asymmetric Encryption RSA";
      this.writeH((short) 7771);
      this.writeD(20190505);
      this.writeQ(348768394698436L);
      this.writeS(name1, name1.Length + 1);
      this.writeS(name2, name2.Length + 1);
      this.writeS(name3, name3.Length + 1);
      using (PROTOCOL_BASE_COPYRIGHT_PAK baseCopyrightPak = new PROTOCOL_BASE_COPYRIGHT_PAK())
        PROTOCOL_BASE_COPYRIGHT_PAK.BASE_COPYRIGTH_PAK = baseCopyrightPak.GetCompleteBytes((string) null);
    }
  }
}
