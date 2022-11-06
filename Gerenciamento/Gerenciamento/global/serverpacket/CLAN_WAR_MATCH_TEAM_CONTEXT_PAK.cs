
// Type: Game.global.serverpacket.CLAN_WAR_MATCH_TEAM_CONTEXT_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;
using System;

namespace Game.global.serverpacket
{
  public class CLAN_WAR_MATCH_TEAM_CONTEXT_PAK : SendPacket
  {
    private int count;

    public CLAN_WAR_MATCH_TEAM_CONTEXT_PAK(int count) => this.count = count;

    public override void write()
    {
      this.writeH((short) 1543);
      this.writeH((short) this.count);
      this.writeC((byte) 13);
      this.writeH((short) Math.Ceiling((double) this.count / 13.0));
    }
  }
}
