
// Type: Game.data.chat.LatencyAnalyze
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.models.enums;
using Game.data.model;

namespace Game.data.chat
{
  public static class LatencyAnalyze
  {
    public static string StartAnalyze(Account player, Room room)
    {
      if (room == null)
        return Translation.GetLabel("GeneralRoomInvalid");
      if (room.getSlot(player._slotId).state != SLOT_STATE.BATTLE)
        return Translation.GetLabel("LatencyInfoError");
      player.DebugPing = !player.DebugPing;
      return player.DebugPing ? Translation.GetLabel("LatencyInfoOn") : Translation.GetLabel("LatencyInfoOff");
    }
  }
}
