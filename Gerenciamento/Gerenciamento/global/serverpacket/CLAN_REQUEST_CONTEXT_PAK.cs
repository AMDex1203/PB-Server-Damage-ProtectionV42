
// Type: Game.global.serverpacket.CLAN_REQUEST_CONTEXT_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.managers;
using Core.server;
using System;

namespace Game.global.serverpacket
{
  public class CLAN_REQUEST_CONTEXT_PAK : SendPacket
  {
    private uint _erro;
    private int invites;

    public CLAN_REQUEST_CONTEXT_PAK(int clanId)
    {
      if (clanId > 0)
        this.invites = PlayerManager.getRequestCount(clanId);
      else
        this._erro = uint.MaxValue;
    }

    public override void write()
    {
      this.writeH((short) 1321);
      this.writeD(this._erro);
      if (this._erro != 0U)
        return;
      this.writeC((byte) this.invites);
      this.writeC((byte) 13);
      this.writeC((byte) Math.Ceiling((double) this.invites / 13.0));
      this.writeD(uint.Parse(DateTime.Now.ToString("MMddHHmmss")));
    }
  }
}
