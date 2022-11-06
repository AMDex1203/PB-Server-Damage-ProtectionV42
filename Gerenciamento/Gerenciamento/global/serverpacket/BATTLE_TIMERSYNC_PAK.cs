
// Type: Game.global.serverpacket.BATTLE_TIMERSYNC_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;
using Game.data.model;
using Game.data.utils;

namespace Game.global.serverpacket
{
  public class BATTLE_TIMERSYNC_PAK : SendPacket
  {
    private Room _r;

    public BATTLE_TIMERSYNC_PAK(Room r) => this._r = r;

    public override void write()
    {
      this.writeH((short) 3371);
      this.writeC((byte) this._r.rodada);
      this.writeD(this._r.getInBattleTimeLeft());
      this.writeH(AllUtils.getSlotsFlag(this._r, true, false));
    }
  }
}
