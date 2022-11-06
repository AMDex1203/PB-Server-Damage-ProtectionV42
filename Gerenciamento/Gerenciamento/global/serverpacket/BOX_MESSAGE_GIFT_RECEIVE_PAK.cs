
// Type: Game.global.serverpacket.BOX_MESSAGE_GIFT_RECEIVE_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.models.account;
using Core.server;

namespace Game.global.serverpacket
{
  public class BOX_MESSAGE_GIFT_RECEIVE_PAK : SendPacket
  {
    private Message gift;

    public BOX_MESSAGE_GIFT_RECEIVE_PAK(Message gift) => this.gift = gift;

    public override void write()
    {
      this.writeH((short) 553);
      this.writeD(this.gift.object_id);
      this.writeD((uint) this.gift.sender_id);
      this.writeD(this.gift.state);
      this.writeD((uint) this.gift.expireDate);
      this.writeC((byte) (this.gift.sender_name.Length + 1));
      this.writeS(this.gift.sender_name, this.gift.sender_name.Length + 1);
      this.writeC((byte) 6);
      this.writeS("EVENT", 6);
    }
  }
}
