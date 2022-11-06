
// Type: Game.data.chat.PlayersCountInServer
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.models.servers;
using Core.xml;

namespace Game.data.chat
{
  public static class PlayersCountInServer
  {
    public static string GetMyServerPlayersCount() => Translation.GetLabel("UsersCount", (object) GameManager._socketList.Count, (object) ConfigGS.serverId);

    public static string GetServerPlayersCount(string str)
    {
      int id = int.Parse(str.Substring(4));
      GameServerModel server = ServersXML.getServer(id);
      if (server == null)
        return Translation.GetLabel("UsersInvalid");
      return Translation.GetLabel("UsersCount2", (object) server._LastCount, (object) server._maxPlayers, (object) id);
    }
  }
}
