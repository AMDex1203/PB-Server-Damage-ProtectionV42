
// Type: Game.data.xml.BattleServer
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using System.Net;

namespace Game.data.xml
{
  public class BattleServer
  {
    public string IP;
    public int Port;
    public int SyncPort;
    public IPEndPoint Connection;

    public BattleServer(string ip, int syncPort)
    {
      this.IP = ip;
      this.SyncPort = syncPort;
      this.Connection = new IPEndPoint(IPAddress.Parse(ip), syncPort);
    }
  }
}
