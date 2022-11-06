
// Type: Core.xml.MissionAwards
// Assembly: Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 263E2FBF-1098-4552-8FB3-F6A660503737
// Interprise: C:\Users\Cuzin\3,50Core.dll

using Core.models.account.mission;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Core.xml
{
  public class MissionAwards
  {
    private static List<MisAwards> _awards = new List<MisAwards>();

    public static void Load()
    {
      string path = "data/cards/MissionAwards.xml";
      if (File.Exists(path))
        MissionAwards.parse(path);
      else
        Logger.warning("[MissionAwards] Não existe o arquivo: " + path);
    }

    private static void parse(string path)
    {
      XmlDocument xmlDocument = new XmlDocument();
      using (FileStream fileStream = new FileStream(path, FileMode.Open))
      {
        if (fileStream.Length == 0L)
        {
          Logger.warning("[MissionAwards] O arquivo está vazio: " + path);
        }
        else
        {
          try
          {
            xmlDocument.Load((Stream) fileStream);
            for (XmlNode xmlNode1 = xmlDocument.FirstChild; xmlNode1 != null; xmlNode1 = xmlNode1.NextSibling)
            {
              if ("list".Equals(xmlNode1.Name))
              {
                for (XmlNode xmlNode2 = xmlNode1.FirstChild; xmlNode2 != null; xmlNode2 = xmlNode2.NextSibling)
                {
                  if ("mission".Equals(xmlNode2.Name))
                  {
                    XmlNamedNodeMap attributes = (XmlNamedNodeMap) xmlNode2.Attributes;
                    int id = int.Parse(attributes.GetNamedItem("id").Value);
                    int blueOrder = int.Parse(attributes.GetNamedItem("blueOrder").Value);
                    int exp = int.Parse(attributes.GetNamedItem("exp").Value);
                    int gp = int.Parse(attributes.GetNamedItem("gp").Value);
                    MissionAwards._awards.Add(new MisAwards(id, blueOrder, exp, gp));
                  }
                }
              }
            }
          }
          catch (XmlException ex)
          {
            Logger.error(ex.ToString());
          }
        }
        fileStream.Dispose();
        fileStream.Close();
      }
    }

    public static MisAwards getAward(int mission)
    {
      lock (MissionAwards._awards)
      {
        for (int index = 0; index < MissionAwards._awards.Count; ++index)
        {
          MisAwards award = MissionAwards._awards[index];
          if (award._id == mission)
            return award;
        }
        return (MisAwards) null;
      }
    }
  }
}
