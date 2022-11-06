
// Type: Game.global.serverpacket.BOX_MESSAGE_CHECK_READED_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;
using System.Collections.Generic;

namespace Game.global.serverpacket
{
  public class BOX_MESSAGE_CHECK_READED_PAK : SendPacket
  {
    private List<int> msgs;

    public BOX_MESSAGE_CHECK_READED_PAK(List<int> msgs) => this.msgs = msgs;

    public override void write()
    {
      this.writeH((short) 423);
      this.writeC((byte) this.msgs.Count);
      for (int index = 0; index < this.msgs.Count; ++index)
        this.writeD(this.msgs[index]);
    }
  }
}
