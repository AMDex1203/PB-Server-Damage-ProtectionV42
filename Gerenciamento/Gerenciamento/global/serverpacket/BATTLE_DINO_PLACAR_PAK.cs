
// Type: Game.global.serverpacket.BATTLE_DINO_PLACAR_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;
using Game.data.model;

namespace Game.global.serverpacket
{
  public class BATTLE_DINO_PLACAR_PAK : SendPacket
  {
    private Room _r;

    public BATTLE_DINO_PLACAR_PAK(Room r) => this._r = r;

    public override void write()
    {
      this.writeH((short) 3389);
      this.writeH((ushort) this._r.red_dino);
      this.writeH((ushort) this._r.blue_dino);
    }
  }
}
