
// Type: Game.global.serverpacket.BATTLE_DEATH_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.models.room;
using Core.server;
using Game.data.model;

namespace Game.global.serverpacket
{
  public class BATTLE_DEATH_PAK : SendPacket
  {
    private Room room;
    private FragInfos kills;
    private Slot killer;
    private bool isBotMode;

    public BATTLE_DEATH_PAK(Room r, FragInfos kills, Slot killer, bool isBotMode)
    {
      this.room = r;
      this.kills = kills;
      this.killer = killer;
      this.isBotMode = isBotMode;
    }

    public override void write()
    {
      this.writeH((short) 3355);
      this.writeC((byte) this.kills.killingType);
      this.writeC(this.kills.killsCount);
      this.writeC(this.kills.killerIdx);
      this.writeD(this.kills.weapon);
      this.writeT(this.kills.x);
      this.writeT(this.kills.y);
      this.writeT(this.kills.z);
      this.writeC(this.kills.flag);
      for (int index = 0; index < this.kills.frags.Count; ++index)
      {
        Frag frag = this.kills.frags[index];
        this.writeC(frag.victimWeaponClass);
        this.writeC(frag.hitspotInfo);
        this.writeH((short) frag.killFlag);
        this.writeC(frag.flag);
        this.writeT(frag.x);
        this.writeT(frag.y);
        this.writeT(frag.z);
      }
      this.writeH((short) this.room._redKills);
      this.writeH((short) this.room._redDeaths);
      this.writeH((short) this.room._blueKills);
      this.writeH((short) this.room._blueDeaths);
      for (int index = 0; index < 16; ++index)
      {
        Slot slot = this.room._slots[index];
        this.writeH((short) slot.allKills);
        this.writeH((short) slot.allDeaths);
      }
      if (this.killer.killsOnLife == 2)
        this.writeC((byte) 1);
      else if (this.killer.killsOnLife == 3)
        this.writeC((byte) 2);
      else if (this.killer.killsOnLife > 3)
        this.writeC((byte) 3);
      else
        this.writeC((byte) 0);
      this.writeH((ushort) this.kills.Score);
      if (this.room.room_type != (byte) 7)
        return;
      this.writeH((ushort) this.room.red_dino);
      this.writeH((ushort) this.room.blue_dino);
    }
  }
}
