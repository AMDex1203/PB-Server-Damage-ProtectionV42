
// Type: Game.data.chat.GetRoomInfo
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.models.room;
using Core.server;
using Game.data.model;
using Game.global.serverpacket;

namespace Game.data.chat
{
  public static class GetRoomInfo
  {
    public static string GetSlotStats(string str, Account player, Room room)
    {
      int slotIdx = int.Parse(str.Substring(5)) - 1;
      string str1 = "Informações:";
      if (room == null)
        return "Sala inválida. [Servidor]";
      Slot slot = room.getSlot(slotIdx);
      if (slot == null)
        return "Slot inválido. [Servidor]";
      string msg = str1 + "\nIndex: " + slot._id.ToString() + "\nTeam: " + slot._team.ToString() + "\nFlag: " + slot._flag.ToString() + "\nAccountId: " + slot._playerId.ToString() + "\nState: " + slot.state.ToString() + "\nMissions: " + (slot.Missions != null ? "Valido" : "Null");
      player.SendPacket((SendPacket) new SERVER_MESSAGE_ANNOUNCE_PAK(msg));
      return "Logs do slot geradas com sucesso. [Servidor]";
    }
  }
}
