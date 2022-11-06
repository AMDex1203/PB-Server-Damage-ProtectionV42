
// Type: Core.managers.events.EventXmasSyncer
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
  public class EventXmasSyncer
  {
    private static readonly List<EventXmasModel> list = new List<EventXmasModel>();
    private static readonly string path = "data/events/EventsXmas.xml";

    public static void Load()
    {
      if (!File.Exists(EventXmasSyncer.path))
      {
        Logger.warning(" [EventXmasSyncer] " + EventXmasSyncer.path + " no exists.");
      }
      else
      {
        EventXmasSyncer.GenerateList();
        Logger.Informations(string.Format(" [EventXmasSyncer] Loaded {0} events Xmas.", (object) EventXmasSyncer.list.Count));
      }
    }

    private static void GenerateList()
    {
      try
      {
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load(EventXmasSyncer.path);
        for (XmlNode xmlNode1 = xmlDocument.FirstChild; xmlNode1 != null; xmlNode1 = xmlNode1.NextSibling)
        {
          if ("list".Equals(xmlNode1.Name))
          {
            for (XmlNode xmlNode2 = xmlNode1.FirstChild; xmlNode2 != null; xmlNode2 = xmlNode2.NextSibling)
            {
              XmlNamedNodeMap attributes = (XmlNamedNodeMap) xmlNode2.Attributes;
              if ("event".Equals(xmlNode2.Name))
                EventXmasSyncer.list.Add(new EventXmasModel()
                {
                  startDate = uint.Parse(attributes.GetNamedItem("start_date").Value),
                  endDate = uint.Parse(attributes.GetNamedItem("end_date").Value)
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
      EventXmasSyncer.list.Clear();
      EventXmasSyncer.Load();
    }

    public static EventXmasModel GetRunningEvent()
    {
      try
      {
        int num = int.Parse(DateTime.Now.ToString("yyMMddHHmm"));
        for (int index = 0; index < EventXmasSyncer.list.Count; ++index)
        {
          EventXmasModel eventXmasModel = EventXmasSyncer.list[index];
          if ((long) eventXmasModel.startDate <= (long) num && (long) num < (long) eventXmasModel.endDate)
            return eventXmasModel;
                    if (eventXmasModel != null) ;
            //Logger.write(string.Format("[EVENTO] Evento Xmas Ativado."), ConsoleColor.Magenta);
        }
      }
      catch (Exception ex)
      {
        Logger.Exception(ex);
      }
      return (EventXmasModel) null;
    }
  }
}
