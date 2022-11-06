
// Type: Game.global.serverpacket.GM_LOG_LOBBY_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;
using Game.data.model;

namespace Game.global.serverpacket
{
  public class GM_LOG_LOBBY_PAK : SendPacket
  {
    private Account player;

    public GM_LOG_LOBBY_PAK(Account p) => this.player = p;

    public override void write()
    {
      this.writeH((short) 2685);
      this.writeD(0);
      this.writeQ(this.player.player_id);
    }
  }
}
