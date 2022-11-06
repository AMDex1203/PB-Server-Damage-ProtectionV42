
// Type: Battle.data.sync.client_side.RemovePlayerSync
// Assembly: pbserver_battle, Version=1.0.0.2646, Culture=neutral, PublicKeyToken=null
// MVID: AA2D7911-B687-4238-AB73-D37E54123039
// Interprise: C:\Users\Cuzin\3,50pbserver_battle.exe

using Battle.data.models;
using Battle.network;

namespace Battle.data.sync.client_side
{
  public static class RemovePlayerSync
  {
    public static void Load(ReceivePacket p)
    {
      uint UniqueRoomId = p.readUD();
      int gen2 = p.readD();
      int slot = (int) p.readC();
      int num = (int) p.readC();
      Room room = RoomsManager.getRoom(UniqueRoomId, gen2);
      if (room == null)
        return;
      if (num == 0)
        RoomsManager.RemoveRoom(UniqueRoomId);
      else
        room.getPlayer(slot, false)?.ResetAllInfos();
    }
  }
}
