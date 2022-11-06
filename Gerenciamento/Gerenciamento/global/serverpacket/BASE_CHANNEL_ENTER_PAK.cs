
// Type: Game.global.serverpacket.BASE_CHANNEL_ENTER_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;

namespace Game.global.serverpacket
{
  public class BASE_CHANNEL_ENTER_PAK : SendPacket
  {
    private uint _channelId;
    private string _announce;

    public BASE_CHANNEL_ENTER_PAK(int id, string announce)
    {
      this._channelId = (uint) id;
      this._announce = announce;
    }

    public BASE_CHANNEL_ENTER_PAK(uint erro) => this._channelId = erro;

    public override void write()
    {
      this.writeH((short) 2574);
      this.writeD(this._channelId);
      if (string.IsNullOrEmpty(this._announce))
        return;
      this.writeH((ushort) this._announce.Length);
      this.writeS(this._announce);
    }
  }
}
