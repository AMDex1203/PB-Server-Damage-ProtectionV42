﻿
// Type: Core.models.servers.GameServerModel
// Assembly: Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 263E2FBF-1098-4552-8FB3-F6A660503737
// Interprise: C:\Users\Cuzin\3,50Core.dll

using System.Net;

namespace Core.models.servers
{
  public class GameServerModel
  {
    public int _state;
    public int _id;
    public int _type;
    public int _LastCount;
    public int _maxPlayers;
    public string _ip;
    public ushort _port;
    public ushort _syncPort;
    public IPEndPoint Connection;

    public GameServerModel(string ip, ushort syncPort)
    {
      this._ip = ip;
      this._syncPort = syncPort;
      this.Connection = new IPEndPoint(IPAddress.Parse(ip), (int) syncPort);
    }
  }
}
