
// Type: Game.data.chat.ChangeChannelNotice
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Game.data.xml;

namespace Game.data.chat
{
  public static class ChangeChannelNotice
  {
    public static string SetChannelNotice(string str)
    {
      int length = str.IndexOf(" ");
      if (length == -1)
        return Translation.GetLabel("ChangeChAnnounceFail");
      int num = int.Parse(str.Substring(7, length));
      if (num < 1)
        return Translation.GetLabel("ChangeChAnnounceFail2");
      int channelId = num - 1;
      string text = str.Substring(length + 1);
      if (!ChannelsXML.updateNotice(ConfigGS.serverId, channelId, text))
        return Translation.GetLabel("ChangeChAnnounceFail");
      Logger.warning(Translation.GetLabel("ChangeChAnnounceWarn", (object) (channelId + 1), (object) (ConfigGS.serverId + 1), (object) text));
      return Translation.GetLabel("ChangeChAnnounceSucc");
    }

    public static string SetAllChannelsNotice(string str)
    {
      string text = str.Substring(4);
      if (!ChannelsXML.updateNotice(text))
        return Translation.GetLabel("ChangeChsAnnounceFail");
      Logger.warning(Translation.GetLabel("ChangeChsAnnounceWarn", (object) text));
      return Translation.GetLabel("ChangeChsAnnounceSucc");
    }
  }
}
