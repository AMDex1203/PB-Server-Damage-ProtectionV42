
// Type: Battle.network.RoomsManager
// Assembly: pbserver_battle, Version=1.0.0.2646, Culture=neutral, PublicKeyToken=null
// MVID: AA2D7911-B687-4238-AB73-D37E54123039
// Interprise: C:\Users\Cuzin\3,50pbserver_battle.exe

using Battle.data;
using Battle.data.models;
using System;
using System.Collections.Generic;

namespace Battle.network
{
  public class RoomsManager
  {
    private static List<Room> list = new List<Room>();

    public static int getGenV(int gen, int type)
    {
      if (type == 1)
        return gen >> 4;
      return type == 2 ? gen & 15 : 0;
    }

    public static Room CreateOrGetRoom(uint UniqueRoomId, int gen2)
    {
      lock (RoomsManager.list)
      {
        for (int index = 0; index < RoomsManager.list.Count; ++index)
        {
          Room room = RoomsManager.list[index];
          if ((int) room.UniqueRoomId == (int) UniqueRoomId)
            return room;
        }
        int roomInfo1 = AllUtils.GetRoomInfo(UniqueRoomId, 2);
        int roomInfo2 = AllUtils.GetRoomInfo(UniqueRoomId, 1);
        int roomInfo3 = AllUtils.GetRoomInfo(UniqueRoomId, 0);
        Room room1 = new Room(roomInfo1)
        {
          UniqueRoomId = UniqueRoomId,
          _genId2 = gen2,
          _roomId = roomInfo3,
          _channelId = roomInfo2,
          _mapId = RoomsManager.getGenV(gen2, 1),
          stageType = RoomsManager.getGenV(gen2, 2)
        };
        RoomsManager.list.Add(room1);
        return room1;
      }
    }

    public static Room getRoom(uint UniqueRoomId)
    {
      lock (RoomsManager.list)
      {
        for (int index = 0; index < RoomsManager.list.Count; ++index)
        {
          Room room = RoomsManager.list[index];
          if (room != null && (int) room.UniqueRoomId == (int) UniqueRoomId)
            return room;
        }
        return (Room) null;
      }
    }

    public static Room getRoom(uint UniqueRoomId, int gen2)
    {
      lock (RoomsManager.list)
      {
        for (int index = 0; index < RoomsManager.list.Count; ++index)
        {
          Room room = RoomsManager.list[index];
          if (room != null && (int) room.UniqueRoomId == (int) UniqueRoomId && room._genId2 == gen2)
            return room;
        }
        return (Room) null;
      }
    }

    public static bool getRoom(uint UniqueRoomId, out Room room)
    {
      room = (Room) null;
      lock (RoomsManager.list)
      {
        for (int index = 0; index < RoomsManager.list.Count; ++index)
        {
          Room room1 = RoomsManager.list[index];
          if (room1 != null && (int) room1.UniqueRoomId == (int) UniqueRoomId)
          {
            room = room1;
            return true;
          }
        }
      }
      return false;
    }

    public static void RemoveRoom(uint UniqueRoomId)
    {
      try
      {
        lock (RoomsManager.list)
        {
          for (int index = 0; index < RoomsManager.list.Count; ++index)
          {
            if ((int) RoomsManager.list[index].UniqueRoomId == (int) UniqueRoomId)
            {
              RoomsManager.list.RemoveAt(index);
              break;
            }
          }
        }
      }
      catch (Exception ex)
      {
        Logger.warning(ex.ToString());
      }
    }
  }
}
