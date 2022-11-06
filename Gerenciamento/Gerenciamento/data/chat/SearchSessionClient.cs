
// Type: Game.data.chat.SearchSessionClient
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

namespace Game.data.chat
{
  public static class SearchSessionClient
  {
    public static string genCode1(string str)
    {
      GameManager.SearchActiveClient(uint.Parse(str.Substring(13)));
      return "";
    }
  }
}
