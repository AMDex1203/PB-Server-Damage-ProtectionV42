
// Type: Game.global.serverpacket.LOBBY_GET_ROOMINFO_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.server;
using Game.data.model;
using System;

namespace Game.global.serverpacket
{
  public class LOBBY_GET_ROOMINFO_PAK : SendPacket
  {
    private Room room;
    private Account leader;

    public LOBBY_GET_ROOMINFO_PAK(Room room, Account leader)
    {
      this.room = room;
      this.leader = leader;
    }

    public override void write()
    {
      if (this.room == null || this.leader == null)
        return;
      this.writeH((short) 3088);
      try
      {
        this.writeS(this.leader.player_name, 33);
        this.writeC((byte) this.room.killtime);
        this.writeC((byte) (this.room.rodada - 1));
        this.writeH((ushort) this.room.getInBattleTime());
        this.writeC(this.room.limit);
        this.writeC(this.room.seeConf);
        this.writeH((ushort) this.room.autobalans);
      }
      catch (Exception ex)
      {
        this.writeS("", 33);
        this.writeB(new byte[8]);
        Logger.warning("[LOBBY_GET_ROOMINFO_PAK] " + ex.ToString());
      }
    }
  }
}
