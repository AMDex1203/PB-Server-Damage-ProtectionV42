
// Type: Core.managers.events.EventLoginSyncer
// Assembly: Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 263E2FBF-1098-4552-8FB3-F6A660503737
// Interprise: C:\Users\Cuzin\3,50Core.dll

using Core.server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Core.managers.events
{
  public class EventLoginSyncer
  {
    private static readonly List<EventLoginModel> list = new List<EventLoginModel>();
    private static readonly string path = "data/events/EventsLogin.xml";

    public static void Load()
    {
      if (!File.Exists(EventLoginSyncer.path))
      {
        Logger.warning(" [EventLoginSyncer] " + EventLoginSyncer.path + " no exists.");
      }
      else
      {
        EventLoginSyncer.GenerateList();
        Logger.Informations(string.Format(" [EventLoginSyncer] Loaded {0} events login.", (object) EventLoginSyncer.list.Count));
      }
    }

    private static void GenerateList()
    {
      try
      {
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load(EventLoginSyncer.path);
        for (XmlNode xmlNode1 = xmlDocument.FirstChild; xmlNode1 != null; xmlNode1 = xmlNode1.NextSibling)
        {
          if ("list".Equals(xmlNode1.Name))
          {
            for (XmlNode xmlNode2 = xmlNode1.FirstChild; xmlNode2 != null; xmlNode2 = xmlNode2.NextSibling)
            {
              XmlNamedNodeMap attributes = (XmlNamedNodeMap) xmlNode2.Attributes;
              if ("event".Equals(xmlNode2.Name))
              {
                EventLoginModel eventLoginModel = new EventLoginModel()
                {
                  startDate = uint.Parse(attributes.GetNamedItem("start_date").Value),
                  endDate = uint.Parse(attributes.GetNamedItem("end_date").Value),
                  _rewardId = int.Parse(attributes.GetNamedItem("reward_id").Value),
                  _count = int.Parse(attributes.GetNamedItem("count").Value)
                };
                eventLoginModel._category = ComDiv.GetItemCategory(eventLoginModel._rewardId);
                if (eventLoginModel._rewardId < 100000000)
                  Logger.error(" [EventLoginSyncer] Evento com premiação incorreta! Id: " + eventLoginModel._rewardId.ToString());
                else
                  EventLoginSyncer.list.Add(eventLoginModel);
              }
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
      EventLoginSyncer.list.Clear();
      EventLoginSyncer.Load();
    }

    public static EventLoginModel GetRunningEvent()
    {
      try
      {
        int num = int.Parse(DateTime.Now.ToString("yyMMddHHmm"));
        for (int index = 0; index < EventLoginSyncer.list.Count; ++index)
        {
          EventLoginModel eventLoginModel = EventLoginSyncer.list[index];
          if ((long) eventLoginModel.startDate <= (long) num && (long) num < (long) eventLoginModel.endDate)
            return eventLoginModel;
          if (eventLoginModel != null) ;
            //Logger.write(string.Format("[EVENTO] Evento de Login Ativado."), ConsoleColor.Magenta);
        }
      }
      catch (Exception ex)
      {
        Logger.Exception(ex);
      }
      return (EventLoginModel) null;
    }
  }
}
