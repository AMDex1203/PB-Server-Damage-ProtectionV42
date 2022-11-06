
// Type: Game.global.clientpacket.A_3094_REC
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Game.data.managers;
using Game.data.model;

namespace Game.global.clientpacket
{
  public class A_3094_REC : ReceiveGamePacket
  {
    private uint sessionId;
    private string name;

    public A_3094_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read()
    {
      this.sessionId = this.readUD();
      this.name = this.readS((int) this.readC());
    }

    public override void run()
    {
      if (this._client == null || this._client._player == null)
        return;
      Account player1 = this._client._player;
      Channel channel = player1.getChannel();
      if (channel == null || player1._room != null)
        return;
      if (this.sessionId == uint.MaxValue)
        return;
      try
      {
        PlayerSession player2 = channel.getPlayer(this.sessionId);
        if (player2 == null || AccountManager.getAccount(player2._playerId, true) == null)
          return;
        Logger.warning("[3094] SessionId: " + this.sessionId.ToString() + "; Name: " + this.name);
      }
      catch
      {
      }
    }
  }
}
