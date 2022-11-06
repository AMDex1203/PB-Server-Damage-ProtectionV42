
// Type: Game.global.clientpacket.BASE_HEARTBEAT_REC
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

namespace Game.global.clientpacket
{
  public class BASE_HEARTBEAT_REC : ReceiveGamePacket
  {
    public BASE_HEARTBEAT_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read()
    {
    }

    public override void run()
    {
    }
  }
}
