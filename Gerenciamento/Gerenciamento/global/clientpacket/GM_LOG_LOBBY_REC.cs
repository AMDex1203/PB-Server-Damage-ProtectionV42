
// Type: Game.global.clientpacket.GM_LOG_LOBBY_REC
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;
using Game.data.managers;
using Game.data.model;
using Game.global.serverpacket;

namespace Game.global.clientpacket
{
  public class GM_LOG_LOBBY_REC : ReceiveGamePacket
  {
    private uint sessionId;

    public GM_LOG_LOBBY_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read() => this.sessionId = this.readUD();

    public override void run()
    {
      Account player = this._client._player;
      if (player == null || !player.IsGM())
        return;
      Account p = (Account) null;
      try
      {
        p = AccountManager.getAccount(player.getChannel().getPlayer(this.sessionId)._playerId, true);
      }
      catch
      {
      }
      if (p == null)
        return;
      this._client.SendPacket((SendPacket) new GM_LOG_LOBBY_PAK(p));
    }
  }
}
