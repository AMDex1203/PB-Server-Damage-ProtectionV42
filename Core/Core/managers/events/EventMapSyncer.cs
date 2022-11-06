
// Type: Core.managers.events.EventMapSyncer
// Assembly: Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 263E2FBF-1098-4552-8FB3-F6A660503737
// Interprise: C:\Users\Cuzin\3,50Core.dll

using Core.managers.events.EventModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Core.managers.events
{
  public class EventMapSyncer
  {
    private static readonly List<EventMapModel> list = new List<EventMapModel>();
    private static readonly string path = "data/events/EventsMap.xml";

    public static void Load()
    {
      if (!File.Exists(EventMapSyncer.path))
      {
        Logger.warning(" [EventMapSyncer] " + EventMapSyncer.path + " no exists.");
      }
      else
      {
        EventMapSyncer.GenerateList();
        Logger.Informations(string.Format(" [EventMapSyncer] Loaded {0} events maps.", (object) EventMapSyncer.list.Count));
      }
    }

    private static void GenerateList()
    {
      try
      {
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load(EventMapSyncer.path);
        for (XmlNode xmlNode1 = xmlDocument.FirstChild; xmlNode1 != null; xmlNode1 = xmlNode1.NextSibling)
        {
          if ("list".Equals(xmlNode1.Name))
          {
            for (XmlNode xmlNode2 = xmlNode1.FirstChild; xmlNode2 != null; xmlNode2 = xmlNode2.NextSibling)
            {
              XmlNamedNodeMap attributes = (XmlNamedNodeMap) xmlNode2.Attributes;
              if ("event".Equals(xmlNode2.Name))
                EventMapSyncer.list.Add(new EventMapModel()
                {
                  _startDate = uint.Parse(attributes.GetNamedItem("start_date").Value),
                  _endDate = uint.Parse(attributes.GetNamedItem("end_date").Value),
                  _mapId = int.Parse(attributes.GetNamedItem("map_id").Value),
                  _stageType = int.Parse(attributes.GetNamedItem("stage_type").Value),
                  _percentXp = int.Parse(attributes.GetNamedItem("percent_exp").Value),
                  _percentGp = int.Parse(attributes.GetNamedItem("percent_gold").Value)
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
      EventMapSyncer.list.Clear();
      EventMapSyncer.Load();
    }

    public static EventMapModel GetRunningEvent()
    {
      try
      {
        int num = int.Parse(DateTime.Now.ToString("yyMMddHHmm"));
        for (int index = 0; index < EventMapSyncer.list.Count; ++index)
        {
          EventMapModel eventMapModel = EventMapSyncer.list[index];
          if ((long) eventMapModel._startDate <= (long) num && (long) num < (long) eventMapModel._endDate)
            return eventMapModel;
                    if (eventMapModel != null);
            //Logger.write(string.Format("[EVENTO] Evento RankUP MAPS Ativado."), ConsoleColor.Magenta);
        }
      }
      catch (Exception ex)
      {
        Logger.Exception(ex);
      }
      return (EventMapModel) null;
    }

    public static bool EventIsValid(EventMapModel eventMap, int mapId, int stageType) => eventMap != null && (eventMap._mapId == mapId || eventMap._stageType == stageType);
  }
}
