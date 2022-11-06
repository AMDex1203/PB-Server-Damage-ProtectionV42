
// Type: Game.global.serverpacket.CLAN_WAR_CREATED_ROOM_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;
using Game.data.model;

namespace Game.global.serverpacket
{
  public class CLAN_WAR_CREATED_ROOM_PAK : SendPacket
  {
    public Match _mt;

    public CLAN_WAR_CREATED_ROOM_PAK(Match match) => this._mt = match;

    public override void write()
    {
      this.writeH((short) 1564);
      this.writeH((short) this._mt._matchId);
      this.writeD(this._mt.getServerInfo());
      this.writeH((short) this._mt.getServerInfo());
      this.writeC((byte) 10);
    }
  }
}
