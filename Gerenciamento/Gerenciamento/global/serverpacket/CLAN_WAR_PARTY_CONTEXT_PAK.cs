
// Type: Game.global.serverpacket.CLAN_WAR_PARTY_CONTEXT_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;
using System;

namespace Game.global.serverpacket
{
  public class CLAN_WAR_PARTY_CONTEXT_PAK : SendPacket
  {
    private int matchCount;

    public CLAN_WAR_PARTY_CONTEXT_PAK(int count) => this.matchCount = count;

    public override void write()
    {
      this.writeH((short) 1539);
      this.writeC((byte) this.matchCount);
      this.writeC((byte) 13);
      this.writeC((byte) Math.Ceiling((double) this.matchCount / 13.0));
    }
  }
}
