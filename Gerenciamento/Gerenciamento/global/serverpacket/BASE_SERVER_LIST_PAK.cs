
// Type: Game.global.serverpacket.BASE_SERVER_LIST_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.models.servers;
using Core.server;
using Core.xml;
using Game.data.xml;
using System.Net;

namespace Game.global.serverpacket
{
  public class BASE_SERVER_LIST_PAK : SendPacket
  {
    private IPAddress _ip;
    private uint _sessionId;

    public BASE_SERVER_LIST_PAK(GameClient gc)
    {
      this._sessionId = gc.SessionId;
      this._ip = gc.GetAddress();
    }

    public override void write()
    {
      this.writeH((short) 2049);
      this.writeD(this._sessionId);
      this.writeIP(this._ip);
      this.writeH((short) 29890);
      this.writeH((short) 0);
      for (int index = 0; index < 10; ++index)
        this.writeC((byte) ChannelsXML._channels[index]._type);
      this.writeC((byte) 1);
      this.writeD(ServersXML._servers.Count);
      for (int index = 0; index < ServersXML._servers.Count; ++index)
      {
        GameServerModel server = ServersXML._servers[index];
        this.writeD(server._state);
        this.writeIP(server.Connection.Address);
        this.writeH(server._port);
        this.writeC((byte) server._type);
        this.writeH((ushort) server._maxPlayers);
        this.writeD(server._LastCount);
      }
      this.writeH((short) 1);
      this.writeH((short) 300);
      this.writeD(200);
      this.writeD(100);
      this.writeC((byte) 1);
      this.writeD(3);
      this.writeD(100);
      this.writeD(150);
    }
  }
}
