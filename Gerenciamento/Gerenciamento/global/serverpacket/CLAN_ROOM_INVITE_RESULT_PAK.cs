
// Type: Game.global.serverpacket.CLAN_ROOM_INVITE_RESULT_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;

namespace Game.global.serverpacket
{
  public class CLAN_ROOM_INVITE_RESULT_PAK : SendPacket
  {
    private long _pId;

    public CLAN_ROOM_INVITE_RESULT_PAK(long pId) => this._pId = pId;

    public override void write()
    {
      this.writeH((short) 1383);
      this.writeQ(this._pId);
    }
  }
}
