
// Type: Core.xml.ClanRankXML
// Assembly: Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 263E2FBF-1098-4552-8FB3-F6A660503737
// Interprise: C:\Users\Cuzin\3,50Core.dll

using Core.models.account.rank;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Core.xml
{
  public class ClanRankXML
  {
    private static List<RankModel> _ranks = new List<RankModel>();

    public static void Load()
    {
      string path = "data/ranktemplate/rankclantemplate.xml";
      if (File.Exists(path))
        ClanRankXML.parse(path);
      else
        Logger.warning("[ClanRankXML] Não existe o arquivo: " + path);
    }

    public static RankModel getRank(int rankId)
    {
      lock (ClanRankXML._ranks)
      {
        for (int index = 0; index < ClanRankXML._ranks.Count; ++index)
        {
          RankModel rank = ClanRankXML._ranks[index];
          if (rank._id == rankId)
            return rank;
        }
        return (RankModel) null;
      }
    }

    private static void parse(string path)
    {
      XmlDocument xmlDocument = new XmlDocument();
      using (FileStream fileStream = new FileStream(path, FileMode.Open))
      {
        if (fileStream.Length == 0L)
        {
          Logger.warning("[ClanRankXML] O arquivo está vazio: " + path);
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
                  if ("rank".Equals(xmlNode2.Name))
                  {
                    XmlNamedNodeMap attributes = (XmlNamedNodeMap) xmlNode2.Attributes;
                    ClanRankXML._ranks.Add(new RankModel(int.Parse(attributes.GetNamedItem("id").Value), "", int.Parse(attributes.GetNamedItem("onNextLevel").Value), 0, int.Parse(attributes.GetNamedItem("onAllExp").Value)));
                  }
                }
              }
            }
          }
          catch (XmlException ex)
          {
            Logger.warning(ex.ToString());
          }
        }
        fileStream.Dispose();
        fileStream.Close();
      }
    }
  }
}
