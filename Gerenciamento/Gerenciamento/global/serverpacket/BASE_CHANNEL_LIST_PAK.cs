
// Type: Game.global.serverpacket.BASE_CHANNEL_LIST_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;
using Game.data.model;
using Game.data.xml;

namespace Game.global.serverpacket
{
  public class BASE_CHANNEL_LIST_PAK : SendPacket
  {
    public override void write()
    {
      this.writeH((short) 2572);
      this.writeD(ChannelsXML._channels.Count);
      this.writeD(ConfigGS.maxChannelPlayers);
      foreach (Channel channel in ChannelsXML._channels)
        this.writeD(channel._players.Count);
    }
  }
}
