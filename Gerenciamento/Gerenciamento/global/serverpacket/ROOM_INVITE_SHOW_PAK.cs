
// Type: Game.global.serverpacket.ROOM_INVITE_SHOW_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;
using Game.data.model;

namespace Game.global.serverpacket
{
  public class ROOM_INVITE_SHOW_PAK : SendPacket
  {
    private Account sender;
    private Room room;

    public ROOM_INVITE_SHOW_PAK(Account sender, Room room)
    {
      this.sender = sender;
      this.room = room;
    }

    public override void write()
    {
      this.writeH((short) 2053);
      this.writeS(this.sender.player_name, 33);
      this.writeD(this.room._roomId);
      this.writeQ(this.sender.player_id);
      this.writeS(this.room.password, 4);
    }
  }
}
