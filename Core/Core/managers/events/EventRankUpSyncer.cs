
// Type: Core.managers.events.EventRankUpSyncer
// Assembly: Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 263E2FBF-1098-4552-8FB3-F6A660503737
// Interprise: C:\Users\Cuzin\3,50Core.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Core.managers.events
{
  public class EventRankUpSyncer
  {
    private static readonly List<EventUpModel> list = new List<EventUpModel>();
    private static readonly string path = "data/events/EventsRankUp.xml";

    public static void Load()
    {
      if (!File.Exists(EventRankUpSyncer.path))
      {
        Logger.warning(" [EventRankUpSyncer] " + EventRankUpSyncer.path + " no exists.");
      }
      else
      {
        EventRankUpSyncer.GenerateList();
        Logger.Informations(string.Format(" [EventRankUpSyncer] Loaded {0} events rankup.", (object) EventRankUpSyncer.list.Count));
      }
    }

    private static void GenerateList()
    {
      try
      {
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load(EventRankUpSyncer.path);
        for (XmlNode xmlNode1 = xmlDocument.FirstChild; xmlNode1 != null; xmlNode1 = xmlNode1.NextSibling)
        {
          if ("list".Equals(xmlNode1.Name))
          {
            for (XmlNode xmlNode2 = xmlNode1.FirstChild; xmlNode2 != null; xmlNode2 = xmlNode2.NextSibling)
            {
              XmlNamedNodeMap attributes = (XmlNamedNodeMap) xmlNode2.Attributes;
              if ("event".Equals(xmlNode2.Name))
                EventRankUpSyncer.list.Add(new EventUpModel()
                {
                  _startDate = uint.Parse(attributes.GetNamedItem("start_date").Value),
                  _endDate = uint.Parse(attributes.GetNamedItem("end_date").Value),
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
      EventRankUpSyncer.list.Clear();
      EventRankUpSyncer.Load();
    }

    public static EventUpModel GetRunningEvent()
    {
      try
      {
        int num = int.Parse(DateTime.Now.ToString("yyMMddHHmm"));
        for (int index = 0; index < EventRankUpSyncer.list.Count; ++index)
        {
          EventUpModel eventUpModel = EventRankUpSyncer.list[index];
          if ((long) eventUpModel._startDate <= (long) num && (long) num < (long) eventUpModel._endDate)
            return eventUpModel;
                    if (eventUpModel != null) ;
            //Logger.write(string.Format("[EVENTO] Evento RankUP Ativado: EXP {0}% e GP {1}%", (object) eventUpModel._percentXp, (object) eventUpModel._percentGp), ConsoleColor.Magenta);
        }
      }
      catch (Exception ex)
      {
        Logger.Exception(ex);
      }
      return (EventUpModel) null;
    }
  }
}
