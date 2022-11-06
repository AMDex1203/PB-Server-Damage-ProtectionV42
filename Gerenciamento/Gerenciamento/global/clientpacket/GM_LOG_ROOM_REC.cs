
// Type: Game.global.clientpacket.GM_LOG_ROOM_REC
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;
using Game.data.model;
using Game.global.serverpacket;

namespace Game.global.clientpacket
{
  public class GM_LOG_ROOM_REC : ReceiveGamePacket
  {
    private int slot;

    public GM_LOG_ROOM_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read() => this.slot = (int) this.readC();

    public override void run()
    {
      Account player1 = this._client._player;
      if (player1 == null || !player1.IsGM())
        return;
      Room room = player1._room;
      Account player2;
      if (room == null || !room.getPlayerBySlot(this.slot, out player2))
        return;
      this._client.SendPacket((SendPacket) new GM_LOG_ROOM_PAK(player2));
    }
  }
}
