
// Type: Auth.global.serverpacket.BASE_SERVER_LIST_REFRESH_PAK
// Assembly: pbserver_auth, Version=1.0.7796.38604, Culture=neutral, PublicKeyToken=null
// MVID: 1CFDFF97-782C-4EF8-A621-6E4FDDE32436
// Interprise: C:\Users\Cuzin\3,50pbserver_auth.exe

using Core.models.servers;
using Core.server;
using Core.xml;

namespace Auth.global.serverpacket
{
  public class BASE_SERVER_LIST_REFRESH_PAK : SendPacket
  {
    public override void write()
    {
      this.writeH((short) 2643);
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
    }
  }
}
