
// Type: Game.data.chat.EnableMissions
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.managers.server;
using Game.data.model;

namespace Game.data.chat
{
  public static class EnableMissions
  {
    public static string genCode1(string str, Account player)
    {
      bool mission = bool.Parse(str.Substring(8));
      if (!ServerConfigSyncer.updateMission(GameManager.Config, mission))
        return Translation.GetLabel("ActivateMissionsMsg2");
      Logger.warning(Translation.GetLabel("ActivateMissionsWarn", (object) mission, (object) player.player_name));
      return Translation.GetLabel("ActivateMissionsMsg1");
    }
  }
}
