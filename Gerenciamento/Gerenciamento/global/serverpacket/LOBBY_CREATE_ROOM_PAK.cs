
// Type: Game.global.serverpacket.LOBBY_CREATE_ROOM_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;
using Game.data.model;

namespace Game.global.serverpacket
{
  public class LOBBY_CREATE_ROOM_PAK : SendPacket
  {
    private Account leader;
    private Room room;
    private uint erro;

    public LOBBY_CREATE_ROOM_PAK(uint err, Room r, Account p)
    {
      this.erro = err;
      this.room = r;
      this.leader = p;
    }

    public override void write()
    {
      this.writeH((short) 3090);
      this.writeD(this.erro == 0U ? (uint) this.room._roomId : this.erro);
      if (this.erro != 0U)
        return;
      this.writeD(this.room._roomId);
      this.writeS(this.room.name, 23);
      this.writeH((short) this.room.mapId);
      this.writeC(this.room.stage4v4);
      this.writeC(this.room.room_type);
      this.writeC((byte) this.room._state);
      this.writeC((byte) this.room.getAllPlayers().Count);
      this.writeC((byte) this.room.getSlotCount());
      this.writeC((byte) this.room._ping);
      this.writeC(this.room.weaponsFlag);
      this.writeC(this.room.random_map);
      this.writeC(this.room.special);
      this.writeS(this.leader.player_name, 33);
      this.writeD(this.room.killtime);
      this.writeC(this.room.limit);
      this.writeC(this.room.seeConf);
      this.writeH((short) this.room.autobalans);
    }
  }
}
