
// Type: Game.global.serverpacket.ROOM_CHANGE_INFO_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;
using Game.data.model;

namespace Game.global.serverpacket
{
  public class ROOM_CHANGE_INFO_PAK : SendPacket
  {
    private string _leader;
    private Room _room;

    public ROOM_CHANGE_INFO_PAK(Room room, string leader)
    {
      this._room = room;
      this._leader = leader;
    }

    public override void write()
    {
      this.writeH((short) 3859);
      this.writeS(this._leader, 33);
      this.writeD(this._room.killtime);
      this.writeC(this._room.limit);
      this.writeC(this._room.seeConf);
      this.writeH((short) this._room.autobalans);
    }
  }
}
