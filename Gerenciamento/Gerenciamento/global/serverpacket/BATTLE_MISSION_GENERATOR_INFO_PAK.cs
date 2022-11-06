
// Type: Game.global.serverpacket.BATTLE_MISSION_GENERATOR_INFO_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;
using Game.data.model;

namespace Game.global.serverpacket
{
  public class BATTLE_MISSION_GENERATOR_INFO_PAK : SendPacket
  {
    private Room _room;

    public BATTLE_MISSION_GENERATOR_INFO_PAK(Room room) => this._room = room;

    public override void write()
    {
      this.writeH((short) 3369);
      this.writeH((ushort) this._room.Bar1);
      this.writeH((ushort) this._room.Bar2);
      for (int index = 0; index < 16; ++index)
        this.writeH(this._room._slots[index].damageBar1);
    }
  }
}
