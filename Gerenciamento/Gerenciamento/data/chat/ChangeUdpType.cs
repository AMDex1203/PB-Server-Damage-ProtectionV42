
// Type: Game.data.chat.ChangeUdpType
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.models.enums;

namespace Game.data.chat
{
  public static class ChangeUdpType
  {
    public static string SetUdpType(string str)
    {
      int num = int.Parse(str.Substring(4));
      if ((SERVER_UDP_STATE) num == ConfigGS.udpType)
        return Translation.GetLabel("ChangeUDPAlready");
      if (num < 1 || num > 4)
        return Translation.GetLabel("ChangeUDPWrongValue");
      ConfigGS.udpType = (SERVER_UDP_STATE) num;
      return Translation.GetLabel("ChangeUDPSuccess", (object) ConfigGS.udpType);
    }
  }
}
