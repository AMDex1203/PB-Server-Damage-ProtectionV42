
// Type: Auth.global.serverpacket.BASE_SERVER_LIST_PAK
// Assembly: pbserver_auth, Version=1.0.7796.38604, Culture=neutral, PublicKeyToken=null
// MVID: 1CFDFF97-782C-4EF8-A621-6E4FDDE32436
// Interprise: C:\Users\Cuzin\3,50pbserver_auth.exe

using Core.models.servers;
using Core.server;
using Core.xml;
using System.Net;

namespace Auth.global.serverpacket
{
  public class BASE_SERVER_LIST_PAK : SendPacket
  {
    private IPAddress _ip;
    private uint _sessionId;
    private ushort _sessionSeed;

    public BASE_SERVER_LIST_PAK(LoginClient lc)
    {
      this._sessionId = lc.SessionId;
      this._sessionSeed = lc.Seed;
      this._ip = lc.GetAddress();
    }

    public override void write()
    {
      this.writeH((short) 2049);
      this.writeD(this._sessionId);
      this.writeIP(this._ip);
      this.writeH((ushort) 45546);
      this.writeH(this._sessionSeed);
      for (int index = 0; index < 10; ++index)
        this.writeC((byte) 1);
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
      this.writeD(1);
      this.writeD(100);
      this.writeD(150);
    }
  }
}
