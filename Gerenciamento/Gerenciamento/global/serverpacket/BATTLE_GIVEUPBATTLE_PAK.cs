
// Type: Game.global.serverpacket.BATTLE_GIVEUPBATTLE_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;
using Game.data.model;

namespace Game.global.serverpacket
{
  public class BATTLE_GIVEUPBATTLE_PAK : SendPacket
  {
    private Room _r;
    private int _oldLeader;

    public BATTLE_GIVEUPBATTLE_PAK(Room room, int oldLeader)
    {
      this._r = room;
      this._oldLeader = oldLeader;
    }

    public override void write()
    {
      this.writeH((short) 3347);
      this.writeD(this._r._leader);
      for (int slotId = 0; slotId < 16; ++slotId)
      {
        if (this._oldLeader == slotId)
        {
          this.writeB(new byte[13]);
        }
        else
        {
          Account playerBySlot = this._r.getPlayerBySlot(slotId);
          if (playerBySlot != null)
          {
            this.writeIP(playerBySlot.PublicIP);
            this.writeH((short) 29890);
            this.writeB(playerBySlot.LocalIP);
            this.writeH((short) 29890);
            this.writeC((byte) 0);
          }
          else
            this.writeB(new byte[13]);
        }
      }
    }
  }
}
