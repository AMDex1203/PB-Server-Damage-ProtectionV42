
// Type: Game.global.serverpacket.BATTLE_ROOM_INFO_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;
using Game.data.model;

namespace Game.global.serverpacket
{
  public class BATTLE_ROOM_INFO_PAK : SendPacket
  {
    private Room room;
    private bool isBotMode;

    public BATTLE_ROOM_INFO_PAK(Room r)
    {
      this.room = r;
      if (this.room == null)
        return;
      this.isBotMode = this.room.isBotMode();
    }

    public BATTLE_ROOM_INFO_PAK(Room r, bool isBotMode)
    {
      this.room = r;
      this.isBotMode = isBotMode;
    }

    public override void write()
    {
      if (this.room == null)
        return;
      this.writeH((short) 3848);
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
      Account leader = this.room.getLeader();
      this.writeS(leader != null ? leader.player_name : "", 33);
      this.writeD(this.room.killtime);
      this.writeC(this.room.limit);
      this.writeC(this.room.seeConf);
      this.writeH((short) this.room.autobalans);
      if (!this.isBotMode)
        return;
      this.writeC(this.room.aiCount);
      this.writeC(this.room.aiLevel);
    }
  }
}
