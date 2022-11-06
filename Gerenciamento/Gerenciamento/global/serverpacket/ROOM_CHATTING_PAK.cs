
// Type: Game.global.serverpacket.ROOM_CHATTING_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;

namespace Game.global.serverpacket
{
  public class ROOM_CHATTING_PAK : SendPacket
  {
    private string msg;
    private int type;
    private int slotId;
    private bool GMColor;

    public ROOM_CHATTING_PAK(int chat_type, int slotId, bool GM, string message)
    {
      this.type = chat_type;
      this.slotId = slotId;
      this.GMColor = GM;
      this.msg = message;
    }

    public override void write()
    {
      this.writeH((short) 3851);
      this.writeH((short) this.type);
      this.writeD(this.slotId);
      this.writeC(this.GMColor);
      this.writeD(this.msg.Length + 1);
      this.writeS(this.msg, this.msg.Length + 1);
    }
  }
}
