
// Type: Core.managers.events.EventPlayTimeSyncer
// Assembly: Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 263E2FBF-1098-4552-8FB3-F6A660503737
// Interprise: C:\Users\Cuzin\3,50Core.dll

using Core.models.account.players;
using Core.server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Core.managers.events
{
  public class EventPlayTimeSyncer
  {
    private static readonly List<PlayTimeModel> list = new List<PlayTimeModel>();
    private static readonly string path = "data/events/EventsPlayTime.xml";

    public static void Load()
    {
      if (!File.Exists(EventPlayTimeSyncer.path))
      {
        Logger.warning(" [EventPlayTimeSyncer] " + EventPlayTimeSyncer.path + " no exists.");
      }
      else
      {
        EventPlayTimeSyncer.GenerateList();
        Logger.Informations(string.Format(" [EventPlayTimeSyncer] Loaded {0} events playtime.", (object) EventPlayTimeSyncer.list.Count));
      }
    }

    private static void GenerateList()
    {
      try
      {
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load(EventPlayTimeSyncer.path);
        for (XmlNode xmlNode1 = xmlDocument.FirstChild; xmlNode1 != null; xmlNode1 = xmlNode1.NextSibling)
        {
          if ("list".Equals(xmlNode1.Name))
          {
            for (XmlNode xmlNode2 = xmlNode1.FirstChild; xmlNode2 != null; xmlNode2 = xmlNode2.NextSibling)
            {
              XmlNamedNodeMap attributes = (XmlNamedNodeMap) xmlNode2.Attributes;
              if ("event".Equals(xmlNode2.Name))
                EventPlayTimeSyncer.list.Add(new PlayTimeModel()
                {
                  _startDate = uint.Parse(attributes.GetNamedItem("start_date").Value),
                  _endDate = uint.Parse(attributes.GetNamedItem("end_date").Value),
                  _title = attributes.GetNamedItem("title").Value,
                  _time = long.Parse(attributes.GetNamedItem("time").Value),
                  _goodReward1 = int.Parse(attributes.GetNamedItem("good_reward1").Value),
                  _goodReward2 = int.Parse(attributes.GetNamedItem("good_reward2").Value),
                  _goodCount1 = int.Parse(attributes.GetNamedItem("good_count1").Value),
                  _goodCount2 = int.Parse(attributes.GetNamedItem("good_count2").Value)
                });
            }
          }
        }
      }
      catch (Exception ex)
      {
        Logger.Exception(ex);
      }
    }

    public static void ReGenerateList()
    {
      EventPlayTimeSyncer.list.Clear();
      EventPlayTimeSyncer.Load();
    }

    public static void ResetPlayerEvent(long pId, PlayerEvent pE)
    {
      if (pId == 0L)
        return;
      ComDiv.updateDB("player_events", "player_id", (object) pId, new string[3]
      {
        "last_playtime_value",
        "last_playtime_finish",
        "last_playtime_date"
      }, (object) pE.LastPlaytimeValue, (object) pE.LastPlaytimeFinish, (object) (long) pE.LastPlaytimeDate);
    }

    public static PlayTimeModel GetRunningEvent()
    {
      try
      {
        int num = int.Parse(DateTime.Now.ToString("yyMMddHHmm"));
        for (int index = 0; index < EventPlayTimeSyncer.list.Count; ++index)
        {
          PlayTimeModel playTimeModel = EventPlayTimeSyncer.list[index];
          if ((long) playTimeModel._startDate <= (long) num && (long) num < (long) playTimeModel._endDate)
            return playTimeModel;
          if (playTimeModel != null);
            //Logger.write(string.Format("[EVENTO] Evento PlayTime Ativado."), ConsoleColor.Magenta);
        }
      }
      catch (Exception ex)
      {
        Logger.Exception(ex);
      }
      return (PlayTimeModel) null;
    }
  }
}
