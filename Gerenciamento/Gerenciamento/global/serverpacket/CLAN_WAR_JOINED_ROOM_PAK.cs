
// Type: Game.global.serverpacket.CLAN_WAR_JOINED_ROOM_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;
using Game.data.model;

namespace Game.global.serverpacket
{
  public class CLAN_WAR_JOINED_ROOM_PAK : SendPacket
  {
    private Match _mt;
    private int _roomId;
    private int _team;

    public CLAN_WAR_JOINED_ROOM_PAK(Match match, int roomId, int team)
    {
      this._mt = match;
      this._roomId = roomId;
      this._team = team;
    }

    public override void write()
    {
      this.writeH((short) 1566);
      this.writeD(this._roomId);
      this.writeH((ushort) this._team);
      this.writeH((ushort) this._mt.getServerInfo());
    }
  }
}
