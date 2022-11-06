
// Type: Game.data.chat.HitMarkerAnalyze
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Game.data.model;

namespace Game.data.chat
{
  public static class HitMarkerAnalyze
  {
    public static string ToggleMarker(Account player, Room room)
    {
      if (room == null)
        return "Não foi possivel ativar o hitmarker";
      player.DebugHitMarker = !player.DebugHitMarker;
      return player.DebugHitMarker ? "HitMarker ativado." : "HitMarker desativado.";
    }
  }
}
