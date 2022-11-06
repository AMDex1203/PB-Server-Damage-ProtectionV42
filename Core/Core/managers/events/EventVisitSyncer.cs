
// Type: Core.managers.events.EventVisitSyncer
// Assembly: Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 263E2FBF-1098-4552-8FB3-F6A660503737
// Interprise: C:\Users\Cuzin\3,50Core.dll

using Core.managers.events.EventModels;
using Core.models.account;
using Core.server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Core.managers.events
{
  public class EventVisitSyncer
  {
    private static readonly List<EventVisitModel> list = new List<EventVisitModel>();
    private static readonly string path = "Data/Events/EventsVisit.xml";
    public static byte[] MyinfoBytes;

    public static void Load()
    {
      if (!File.Exists(EventVisitSyncer.path))
      {
        Logger.warning(" [EventVisitSyncer] " + EventVisitSyncer.path + " no exists.");
      }
      else
      {
        EventVisitSyncer.GenerateList();
        Logger.Informations(string.Format(" [EventVisitSyncer] Loaded {0} events checks.", (object) EventVisitSyncer.list.Count));
      }
    }

    private static void GenerateList()
    {
      try
      {
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load(EventVisitSyncer.path);
        for (XmlNode xmlNode1 = xmlDocument.FirstChild; xmlNode1 != null; xmlNode1 = xmlNode1.NextSibling)
        {
          if ("list".Equals(xmlNode1.Name))
          {
            for (XmlNode xmlNode2 = xmlNode1.FirstChild; xmlNode2 != null; xmlNode2 = xmlNode2.NextSibling)
            {
              XmlNamedNodeMap attributes = (XmlNamedNodeMap) xmlNode2.Attributes;
              if ("event".Equals(xmlNode2.Name))
              {
                EventVisitModel eventVisitModel = new EventVisitModel()
                {
                  id = int.Parse(attributes.GetNamedItem("id").Value),
                  startDate = uint.Parse(attributes.GetNamedItem("start_date").Value),
                  endDate = uint.Parse(attributes.GetNamedItem("end_date").Value),
                  title = attributes.GetNamedItem("title").Value,
                  checks = byte.Parse(attributes.GetNamedItem("checks").Value)
                };
                string str1 = attributes.GetNamedItem("goods1").Value;
                string str2 = attributes.GetNamedItem("counts1").Value;
                string str3 = attributes.GetNamedItem("goods2").Value;
                string str4 = attributes.GetNamedItem("counts2").Value;
                string[] strArray1 = str1.Split(',');
                string[] strArray2 = str3.Split(',');
                for (int index = 0; index < strArray1.Length; ++index)
                  eventVisitModel.box[index].reward1.goodId = int.Parse(strArray1[index]);
                for (int index = 0; index < strArray2.Length; ++index)
                  eventVisitModel.box[index].reward2.goodId = int.Parse(strArray2[index]);
                string[] strArray3 = str2.Split(',');
                string[] strArray4 = str4.Split(',');
                for (int index = 0; index < strArray3.Length; ++index)
                  eventVisitModel.box[index].reward1.SetCount(strArray3[index]);
                for (int index = 0; index < strArray4.Length; ++index)
                  eventVisitModel.box[index].reward2.SetCount(strArray4[index]);
                eventVisitModel.SetBoxCounts();
                EventVisitSyncer.list.Add(eventVisitModel);
              }
            }
          }
        }
        EventVisitModel runningEvent = EventVisitSyncer.GetRunningEvent();
        if (runningEvent == null)
          return;
        using (SendGPacket sendGpacket = new SendGPacket())
        {
          sendGpacket.writeH((short) 0);
          sendGpacket.writeD(runningEvent.startDate);
          sendGpacket.writeS(runningEvent.title, 60);
          sendGpacket.writeC((byte) 2);
          sendGpacket.writeC(runningEvent.checks);
          sendGpacket.writeH((short) 0);
          sendGpacket.writeD(runningEvent.id);
          sendGpacket.writeD(runningEvent.startDate);
          sendGpacket.writeD(runningEvent.endDate);
          bool flag = ConfigGB.ClientVersion == "1.15.39" || ConfigGB.ClientVersion == "1.15.41" || ConfigGB.ClientVersion == "1.15.42";
          for (int index = 0; index < 7; ++index)
          {
            VisitBox visitBox = runningEvent.box[index];
            if (flag)
              sendGpacket.writeC((byte) visitBox.RewardCount);
            else
              sendGpacket.writeD(visitBox.RewardCount);
            sendGpacket.writeD(visitBox.reward1.goodId);
            sendGpacket.writeD(visitBox.reward2.goodId);
          }
          EventVisitSyncer.MyinfoBytes = sendGpacket.mstream.ToArray();
        }
      }
      catch (Exception ex)
      {
        Logger.Exception(ex);
      }
    }

    public static void ResetPlayerEvent(long pId, int eventId)
    {
      if (pId == 0L)
        return;
      ComDiv.updateDB("player_events", "player_id", (object) pId, new string[4]
      {
        "last_visit_event_id",
        "last_visit_sequence1",
        "last_visit_sequence2",
        "next_visit_date"
      }, (object) eventId, (object) 0, (object) 0, (object) 0);
    }

    public static void ReGenerateList()
    {
      EventVisitSyncer.list.Clear();
      EventVisitSyncer.Load();
    }

    public static EventVisitModel GetEvent(int eventId)
    {
      try
      {
        for (int index = 0; index < EventVisitSyncer.list.Count; ++index)
        {
          EventVisitModel eventVisitModel = EventVisitSyncer.list[index];
          if (eventVisitModel.id == eventId)
            return eventVisitModel;
        }
      }
      catch (Exception ex)
      {
        Logger.Exception(ex);
      }
      return (EventVisitModel) null;
    }

    public static EventVisitModel GetRunningEvent()
    {
      try
      {
        int num = int.Parse(DateTime.Now.ToString("yyMMddHHmm"));
        for (int index = 0; index < EventVisitSyncer.list.Count; ++index)
        {
          EventVisitModel eventVisitModel = EventVisitSyncer.list[index];
          if ((long) eventVisitModel.startDate <= (long) num && (long) num < (long) eventVisitModel.endDate)
            return eventVisitModel;
        }
      }
      catch (Exception ex)
      {
        Logger.Exception(ex);
      }
      return (EventVisitModel) null;
    }
  }
}
