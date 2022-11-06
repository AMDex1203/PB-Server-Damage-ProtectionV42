
// Type: Game.global.serverpacket.CLAN_CLIENT_CLAN_CONTEXT_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;
using System;

namespace Game.global.serverpacket
{
  public class CLAN_CLIENT_CLAN_CONTEXT_PAK : SendPacket
  {
    private int clansCount;

    public CLAN_CLIENT_CLAN_CONTEXT_PAK(int count) => this.clansCount = count;

    public override void write()
    {
      this.writeH((short) 1452);
      this.writeD(this.clansCount);
      this.writeC((byte) 170);
      this.writeH((ushort) Math.Ceiling((double) this.clansCount / 170.0));
      this.writeD(uint.Parse(DateTime.Now.ToString("MMddHHmmss")));
    }
  }
}
