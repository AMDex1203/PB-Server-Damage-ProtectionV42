
// Type: Game.global.clientpacket.LOBBY_GET_PLAYERINFO2_REC
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;
using Game.data.model;
using Game.global.serverpacket;

namespace Game.global.clientpacket
{
  public class LOBBY_GET_PLAYERINFO2_REC : ReceiveGamePacket
  {
    private uint sessionId;

    public LOBBY_GET_PLAYERINFO2_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read() => this.sessionId = this.readUD();

    public override void run()
    {
      Account player1 = this._client._player;
      if (player1 == null)
        return;
      long player2 = 0;
      try
      {
        player2 = player1.getChannel().getPlayer(this.sessionId)._playerId;
      }
      catch
      {
      }
      this._client.SendPacket((SendPacket) new LOBBY_GET_PLAYERINFO2_PAK(player2));
    }
  }
}
