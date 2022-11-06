
// Type: Game.data.xml.BattleServerXML
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Game.data.xml
{
  public static class BattleServerXML
  {
    public static List<BattleServer> Servers = new List<BattleServer>();

    public static void Load()
    {
      string path = "data/battle/battlestart.xml";
      if (File.Exists(path))
        BattleServerXML.parse(path);
      else
        Logger.info("[BattleServerXML] Não existe o arquivo: " + path);
    }

    public static BattleServer GetRandomServer()
    {
      if (BattleServerXML.Servers.Count == 0)
        return (BattleServer) null;
      int index = new Random().Next(BattleServerXML.Servers.Count);
      try
      {
        return BattleServerXML.Servers[index];
      }
      catch
      {
        return (BattleServer) null;
      }
    }

    private static void parse(string path)
    {
      XmlDocument xmlDocument = new XmlDocument();
      try
      {
        using (FileStream fileStream = new FileStream(path, FileMode.Open))
        {
          if (fileStream.Length > 0L)
          {
            xmlDocument.Load((Stream) fileStream);
            for (XmlNode xmlNode1 = xmlDocument.FirstChild; xmlNode1 != null; xmlNode1 = xmlNode1.NextSibling)
            {
              if ("list".Equals(xmlNode1.Name))
              {
                XmlNamedNodeMap attributes1 = (XmlNamedNodeMap) xmlNode1.Attributes;
                for (XmlNode xmlNode2 = xmlNode1.FirstChild; xmlNode2 != null; xmlNode2 = xmlNode2.NextSibling)
                {
                  if ("Server".Equals(xmlNode2.Name))
                  {
                    XmlNamedNodeMap attributes2 = (XmlNamedNodeMap) xmlNode2.Attributes;
                    BattleServer battleServer = new BattleServer(attributes2.GetNamedItem("IP").Value, int.Parse(attributes2.GetNamedItem("Sync").Value))
                    {
                      Port = int.Parse(attributes2.GetNamedItem("Port").Value)
                    };
                    BattleServerXML.Servers.Add(battleServer);
                  }
                }
              }
            }
          }
          fileStream.Dispose();
          fileStream.Close();
        }
      }
      catch (XmlException ex)
      {
        Logger.error("[BattleServerXML] Erro no arquivo: " + path + "\r\n" + ex.ToString());
      }
    }
  }
}
