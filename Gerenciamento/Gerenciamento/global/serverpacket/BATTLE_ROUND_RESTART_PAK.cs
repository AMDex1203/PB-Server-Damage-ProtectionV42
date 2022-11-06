
// Type: Game.global.serverpacket.BATTLE_ROUND_RESTART_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;
using Game.data.model;
using Game.data.utils;
using System.Collections.Generic;

namespace Game.global.serverpacket
{
  public class BATTLE_ROUND_RESTART_PAK : SendPacket
  {
    private Room _r;
    private List<int> _dinos;
    private bool isBotMode;

    public BATTLE_ROUND_RESTART_PAK(Room r, List<int> dinos, bool isBotMode)
    {
      this._r = r;
      this._dinos = dinos;
      this.isBotMode = isBotMode;
    }

    public BATTLE_ROUND_RESTART_PAK(Room r)
    {
      this._r = r;
      this._dinos = AllUtils.getDinossaurs(r, false, -1);
      this.isBotMode = this._r.isBotMode();
    }

    public override void write()
    {
      this.writeH((short) 3351);
      this.writeH(AllUtils.getSlotsFlag(this._r, false, false));
      if (this._r.room_type == (byte) 8)
        this.writeB(new byte[10]
        {
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue
        });
      else if (this.isBotMode)
        this.writeB(new byte[10]);
      else if (this._r.room_type == (byte) 7 || this._r.room_type == (byte) 12)
      {
        int num1 = this._dinos.Count == 1 || this._r.room_type == (byte) 12 ? (int) byte.MaxValue : this._r.TRex;
        this.writeC((byte) num1);
        foreach (int dino in this._dinos)
        {
          if (dino != this._r.TRex && this._r.room_type == (byte) 7 || this._r.room_type == (byte) 12)
            this.writeC((byte) dino);
        }
        int num2 = 8 - this._dinos.Count - (num1 == (int) byte.MaxValue ? 1 : 0);
        for (int index = 0; index < num2; ++index)
          this.writeC(byte.MaxValue);
        this.writeC(byte.MaxValue);
        this.writeC(byte.MaxValue);
      }
      else
        this.writeB(new byte[10]
        {
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue
        });
    }
  }
}
