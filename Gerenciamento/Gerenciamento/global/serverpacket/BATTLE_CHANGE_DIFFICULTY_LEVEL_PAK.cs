
// Type: Game.global.serverpacket.BATTLE_CHANGE_DIFFICULTY_LEVEL_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;
using Game.data.model;

namespace Game.global.serverpacket
{
  public class BATTLE_CHANGE_DIFFICULTY_LEVEL_PAK : SendPacket
  {
    private Room room;

    public BATTLE_CHANGE_DIFFICULTY_LEVEL_PAK(Room room) => this.room = room;

    public override void write()
    {
      this.writeH((short) 3377);
      this.writeC(this.room.IngameAiLevel);
      for (int index = 0; index < 16; ++index)
        this.writeD(this.room._slots[index].aiLevel);
    }
  }
}
