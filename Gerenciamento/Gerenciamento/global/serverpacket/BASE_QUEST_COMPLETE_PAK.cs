
// Type: Game.global.serverpacket.BASE_QUEST_COMPLETE_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;
using Core.xml;

namespace Game.global.serverpacket
{
  public class BASE_QUEST_COMPLETE_PAK : SendPacket
  {
    private int missionId;
    private int value;

    public BASE_QUEST_COMPLETE_PAK(int progress, Card card)
    {
      this.missionId = card._missionBasicId;
      if (card._missionLimit == progress)
        this.missionId += 240;
      this.value = progress;
    }

    public override void write()
    {
      this.writeH((short) 2600);
      this.writeC((byte) this.missionId);
      this.writeC((byte) this.value);
    }
  }
}
