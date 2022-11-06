
// Type: Game.data.sync.client_side.Net_Room_HitMarker
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.models.enums;
using Core.server;
using Game.data.model;
using Game.data.xml;
using Game.global.serverpacket;
using System;

namespace Game.data.sync.client_side
{
  public static class Net_Room_HitMarker
  {
    public static void Load(ReceiveGPacket p)
    {
      int id1 = (int) p.readH();
      int id2 = (int) p.readH();
      byte num1 = p.readC();
      byte num2 = p.readC();
      byte num3 = p.readC();
      int num4 = (int) p.readH();
      if (p.getBuffer().Length > 11)
        Logger.warning("[Invalid MARKER: " + BitConverter.ToString(p.getBuffer()) + "]");
      Channel channel = ChannelsXML.getChannel(id2);
      if (channel == null)
        return;
      Room room = channel.getRoom(id1);
      if (room == null || room._state != RoomState.Battle)
        return;
      Account playerBySlot = room.getPlayerBySlot((int) num1);
      if (playerBySlot == null || !playerBySlot.DebugHitMarker)
        return;
      string message = "";
      if (num2 == (byte) 10)
      {
        message = Translation.GetLabel("LifeRestored", (object) num4);
      }
      else
      {
        switch (num3)
        {
          case 0:
            message = Translation.GetLabel("HitMarker1", (object) num4);
            break;
          case 1:
            message = Translation.GetLabel("HitMarker2", (object) num4);
            break;
          case 2:
            message = Translation.GetLabel("HitMarker3");
            break;
          case 3:
            message = Translation.GetLabel("HitMarker4");
            break;
        }
      }
      playerBySlot.SendPacket((SendPacket) new LOBBY_CHATTING_PAK(Translation.GetLabel("HitMarkerName"), playerBySlot.getSessionId(), 0, true, message));
    }
  }
}
