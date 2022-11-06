
// Type: Game.data.chat.AFK_Interaction
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;

namespace Game.data.chat
{
  public static class AFK_Interaction
  {
    public static string GetAFKCount(string str) => Translation.GetLabel("AFK_Count_Success", (object) GameManager.KickCountActiveClient(double.Parse(str.Substring(4))));

    public static string KickAFKPlayers(string str) => Translation.GetLabel("AFK_Kick_Success", (object) GameManager.KickActiveClient(double.Parse(str.Substring(8))));
  }
}
