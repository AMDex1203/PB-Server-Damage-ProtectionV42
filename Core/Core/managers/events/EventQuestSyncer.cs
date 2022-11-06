
// Type: Core.managers.events.EventQuestSyncer
// Assembly: Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 263E2FBF-1098-4552-8FB3-F6A660503737
// Interprise: C:\Users\Cuzin\3,50Core.dll

using Core.managers.events.EventModels;
using Core.models.account.players;
using Core.server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Core.managers.events
{
  public class EventQuestSyncer
  {
    private static readonly List<QuestModel> list = new List<QuestModel>();
    private static readonly string path = "data/events/EventsQuest.xml";

    public static void Load()
    {
      if (!File.Exists(EventQuestSyncer.path))
      {
        Logger.warning(" [EventQuestSyncer] " + EventQuestSyncer.path + " no exists.");
      }
      else
      {
        EventQuestSyncer.GenerateList();
        Logger.Informations(string.Format(" [EventQuestSyncer] Loaded {0} events quests.", (object) EventQuestSyncer.list.Count));
      }
    }

    private static void GenerateList()
    {
      try
      {
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load(EventQuestSyncer.path);
        for (XmlNode xmlNode1 = xmlDocument.FirstChild; xmlNode1 != null; xmlNode1 = xmlNode1.NextSibling)
        {
          if ("list".Equals(xmlNode1.Name))
          {
            for (XmlNode xmlNode2 = xmlNode1.FirstChild; xmlNode2 != null; xmlNode2 = xmlNode2.NextSibling)
            {
              XmlNamedNodeMap attributes = (XmlNamedNodeMap) xmlNode2.Attributes;
              if ("event".Equals(xmlNode2.Name))
                EventQuestSyncer.list.Add(new QuestModel()
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
      EventQuestSyncer.list.Clear();
      EventQuestSyncer.Load();
    }

    public static void ResetPlayerEvent(long pId, PlayerEvent pE)
    {
      if (pId == 0L)
        return;
      ComDiv.updateDB("player_events", "player_id", (object) pId, new string[2]
      {
        "last_quest_date",
        "last_quest_finish"
      }, (object) (long) pE.LastQuestDate, (object) pE.LastQuestFinish);
    }

    public static QuestModel GetRunningEvent()
    {
      try
      {
        int num = int.Parse(DateTime.Now.ToString("yyMMddHHmm"));
        for (int index = 0; index < EventQuestSyncer.list.Count; ++index)
        {
          QuestModel questModel = EventQuestSyncer.list[index];
          if ((long) questModel.startDate <= (long) num && (long) num < (long) questModel.endDate)
            return questModel;
          if (questModel != null);
            //Logger.write(string.Format("[EVENTO] Evento Quest Ativado."), ConsoleColor.Magenta);
        }
      }
      catch (Exception ex)
      {
        Logger.Exception(ex);
      }
      return (QuestModel) null;
    }
  }
}
