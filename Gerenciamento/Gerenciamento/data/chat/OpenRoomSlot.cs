
// Type: Game.data.chat.OpenRoomSlot
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.models.enums;
using Core.models.room;
using Game.data.model;

namespace Game.data.chat
{
  public static class OpenRoomSlot
  {
    public static string OpenSpecificSlot(string str, Account player, Room room)
    {
      int num = int.Parse(str.Substring(6));
      if (num < 1 || num > 16)
        return Translation.GetLabel("OpenRoomSlot_WrongValue");
      int slotIdx = num - 1;
      if (player == null || room == null)
        return Translation.GetLabel("OpenRoomSlot_Fail2");
      Slot slot = room.getSlot(slotIdx);
      if (slot == null || slot.state != SLOT_STATE.CLOSE)
        return Translation.GetLabel("OpenRoomSlot_Fail1");
      slot.state = SLOT_STATE.EMPTY;
      room.updateSlotsInfo();
      return Translation.GetLabel("OpenRoomSlot_Success1", (object) slotIdx);
    }

    public static string OpenRandomSlot(string str, Account player)
    {
      int num = int.Parse(str.Substring(6));
      int id = num - 1;
      if (num <= 0)
        return Translation.GetLabel("OpenRoomSlot_WrongValue2");
      if (player == null)
        return Translation.GetLabel("OpenRoomSlot_Fail6");
      Channel channel = player.getChannel();
      if (channel == null)
        return Translation.GetLabel("GeneralChannelInvalid");
      Room room = channel.getRoom(id);
      if (room == null)
        return Translation.GetLabel("GeneralRoomNotFounded");
      bool flag = false;
      for (int index = 0; index < 16; ++index)
      {
        Slot slot = room._slots[index];
        if (slot.state == SLOT_STATE.CLOSE)
        {
          slot.state = SLOT_STATE.EMPTY;
          flag = true;
          break;
        }
      }
      if (flag)
        room.updateSlotsInfo();
      return flag ? Translation.GetLabel("OpenRoomSlot_Success2") : Translation.GetLabel("OpenRoomSlot_Fail3");
    }

    public static string OpenAllSlots(string str, Account player)
    {
      int num = int.Parse(str.Substring(6));
      int id = num - 1;
      if (num <= 0)
        return Translation.GetLabel("OpenRoomSlot_WrongValue2");
      if (player == null)
        return Translation.GetLabel("OpenRoomSlot_Fail6");
      Channel channel = player.getChannel();
      if (channel == null)
        return Translation.GetLabel("OpenRoomSlot_Fail5");
      Room room = channel.getRoom(id);
      if (room == null)
        return Translation.GetLabel("GeneralRoomNotFounded");
      for (int index = 0; index < 16; ++index)
      {
        Slot slot = room._slots[index];
        if (slot.state == SLOT_STATE.CLOSE)
          slot.state = SLOT_STATE.EMPTY;
      }
      room.updateSlotsInfo();
      return Translation.GetLabel("OpenRoomSlot_Success3");
    }
  }
}
