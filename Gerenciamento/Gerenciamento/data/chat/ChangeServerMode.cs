
// Type: Game.data.chat.ChangeServerMode
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;

namespace Game.data.chat
{
  public static class ChangeServerMode
  {
    public static string EnableTestMode()
    {
      if (ConfigGS.isTestMode)
        return Translation.GetLabel("AlreadyTestModeOn");
      ConfigGS.isTestMode = true;
      return Translation.GetLabel("TestModeOn");
    }

    public static string EnablePublicMode()
    {
      if (!ConfigGS.isTestMode)
        return Translation.GetLabel("AlreadyTestModeOff");
      ConfigGS.isTestMode = false;
      return Translation.GetLabel("TestModeOff");
    }
  }
}
