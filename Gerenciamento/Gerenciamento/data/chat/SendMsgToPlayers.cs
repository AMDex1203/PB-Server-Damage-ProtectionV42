
// Type: Game.data.chat.SendMsgToPlayers
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.server;
using Game.data.model;
using Game.global.serverpacket;

namespace Game.data.chat
{
  public static class SendMsgToPlayers
  {
    public static string SendToAll(string str)
    {
      string msg = str.Substring(3);
      int num = 0;
      using (SERVER_MESSAGE_ANNOUNCE_PAK messageAnnouncePak = new SERVER_MESSAGE_ANNOUNCE_PAK(msg))
        num = GameManager.SendPacketToAllClients((SendPacket) messageAnnouncePak);
      return Translation.GetLabel("MsgAllClients", (object) num);
    }

    public static string SendToRoom(string str, Room room)
    {
      string msg = str.Substring(3);
      if (room == null)
        return Translation.GetLabel("GeneralRoomInvalid");
      using (SERVER_MESSAGE_ANNOUNCE_PAK messageAnnouncePak = new SERVER_MESSAGE_ANNOUNCE_PAK(msg))
        room.SendPacketToPlayers((SendPacket) messageAnnouncePak);
      return Translation.GetLabel("MsgRoomPlayers");
    }
  }
}
