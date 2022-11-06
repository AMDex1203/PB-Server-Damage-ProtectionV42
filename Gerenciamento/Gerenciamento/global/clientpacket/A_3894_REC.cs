
// Type: Game.global.clientpacket.A_3894_REC
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;

namespace Game.global.clientpacket
{
  public class A_3894_REC : ReceiveGamePacket
  {
    private int Slot;

    public A_3894_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read() => this.Slot = (int) this.readC();

    public override void run()
    {
      if (this._client == null)
        return;
      if (this._client._player == null)
        return;
      try
      {
        Logger.warning("[3894] Slot: " + this.Slot.ToString());
      }
      catch
      {
      }
    }
  }
}
