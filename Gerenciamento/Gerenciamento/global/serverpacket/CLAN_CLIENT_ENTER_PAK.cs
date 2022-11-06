
// Type: Game.global.serverpacket.CLAN_CLIENT_ENTER_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;
using Game.data.managers;
using System;

namespace Game.global.serverpacket
{
  public class CLAN_CLIENT_ENTER_PAK : SendPacket
  {
    private int _type;
    private int _clanId;

    public CLAN_CLIENT_ENTER_PAK(int id, int access)
    {
      this._clanId = id;
      this._type = access;
    }

    public override void write()
    {
      this.writeH((short) 1442);
      this.writeD(this._clanId);
      this.writeD(this._type);
      if (this._clanId != 0 && this._type != 0)
        return;
      this.writeD(ClanManager._clans.Count);
      this.writeC((byte) 170);
      this.writeH((ushort) Math.Ceiling((double) ClanManager._clans.Count / 170.0));
      this.writeD(uint.Parse(DateTime.Now.ToString("MMddHHmmss")));
    }
  }
}
