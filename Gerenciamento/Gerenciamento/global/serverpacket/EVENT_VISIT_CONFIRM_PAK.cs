
// Type: Game.global.serverpacket.EVENT_VISIT_CONFIRM_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.managers.events.EventModels;
using Core.models.account.players;
using Core.models.enums.errors;
using Core.server;

namespace Game.global.serverpacket
{
  public class EVENT_VISIT_CONFIRM_PAK : SendPacket
  {
    private EventVisitModel _event;
    private PlayerEvent _pev;
    private uint _erro;

    public EVENT_VISIT_CONFIRM_PAK(EventErrorEnum erro, EventVisitModel ev, PlayerEvent pev)
    {
      this._erro = (uint) erro;
      this._event = ev;
      this._pev = pev;
    }

    public override void write()
    {
      this.writeH((short) 2662);
      this.writeD(this._erro);
      if (this._erro != 2147489028U)
        return;
      this.writeD(this._event.id);
      this.writeC((byte) this._pev.LastVisitSequence1);
      this.writeC((byte) this._pev.LastVisitSequence2);
      this.writeH(ushort.MaxValue);
      this.writeD(this._event.startDate);
    }
  }
}
