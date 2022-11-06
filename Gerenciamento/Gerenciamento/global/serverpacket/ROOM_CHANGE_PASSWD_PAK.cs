
// Type: Game.global.serverpacket.ROOM_CHANGE_PASSWD_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;

namespace Game.global.serverpacket
{
  public class ROOM_CHANGE_PASSWD_PAK : SendPacket
  {
    private string _pass;

    public ROOM_CHANGE_PASSWD_PAK(string pass) => this._pass = pass;

    public override void write()
    {
      this.writeH((short) 3907);
      this.writeS(this._pass, 4);
    }
  }
}
