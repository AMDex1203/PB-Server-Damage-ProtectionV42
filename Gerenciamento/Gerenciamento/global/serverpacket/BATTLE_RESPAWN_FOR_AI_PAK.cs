
// Type: Game.global.serverpacket.BATTLE_RESPAWN_FOR_AI_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;

namespace Game.global.serverpacket
{
  public class BATTLE_RESPAWN_FOR_AI_PAK : SendPacket
  {
    private int _slot;

    public BATTLE_RESPAWN_FOR_AI_PAK(int slot) => this._slot = slot;

    public override void write()
    {
      this.writeH((short) 3379);
      this.writeD(this._slot);
    }
  }
}
