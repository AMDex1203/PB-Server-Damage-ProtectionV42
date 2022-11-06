
// Type: Core.xml.TitlesXML
// Assembly: Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 263E2FBF-1098-4552-8FB3-F6A660503737
// Interprise: C:\Users\Cuzin\3,50Core.dll

using Core.models.account.title;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Core.xml
{
  public class TitlesXML
  {
    private static List<TitleQ> titles = new List<TitleQ>();

    public static TitleQ getTitle(int titleId, bool ReturnNull = true)
    {
      if (titleId == 0)
        return !ReturnNull ? new TitleQ() : (TitleQ) null;
      for (int index = 0; index < TitlesXML.titles.Count; ++index)
      {
        TitleQ title = TitlesXML.titles[index];
        if (title._id == titleId)
          return title;
      }
      return (TitleQ) null;
    }

    public static void get2Titles(
      int titleId1,
      int titleId2,
      out TitleQ title1,
      out TitleQ title2,
      bool ReturnNull = true)
    {
      if (!ReturnNull)
      {
        title1 = new TitleQ();
        title2 = new TitleQ();
      }
      else
      {
        title1 = (TitleQ) null;
        title2 = (TitleQ) null;
      }
      if (titleId1 == 0 && titleId2 == 0)
        return;
      for (int index = 0; index < TitlesXML.titles.Count; ++index)
      {
        TitleQ title = TitlesXML.titles[index];
        if (title._id == titleId1)
          title1 = title;
        else if (title._id == titleId2)
          title2 = title;
      }
    }

    public static void get3Titles(
      int titleId1,
      int titleId2,
      int titleId3,
      out TitleQ title1,
      out TitleQ title2,
      out TitleQ title3,
      bool ReturnNull)
    {
      if (!ReturnNull)
      {
        title1 = new TitleQ();
        title2 = new TitleQ();
        title3 = new TitleQ();
      }
      else
      {
        title1 = (TitleQ) null;
        title2 = (TitleQ) null;
        title3 = (TitleQ) null;
      }
      if (titleId1 == 0 && titleId2 == 0 && titleId3 == 0)
        return;
      for (int index = 0; index < TitlesXML.titles.Count; ++index)
      {
        TitleQ title = TitlesXML.titles[index];
        if (title._id == titleId1)
          title1 = title;
        else if (title._id == titleId2)
          title2 = title;
        else if (title._id == titleId3)
          title3 = title;
      }
    }

    public static void Load()
    {
      string path = "data/titles/title_info.xml";
      if (File.Exists(path))
        TitlesXML.parse(path);
      else
        Logger.warning("[TitlesXML] Não existe o arquivo: " + path);
    }

    private static void parse(string path)
    {
      XmlDocument xmlDocument = new XmlDocument();
      using (FileStream fileStream = new FileStream(path, FileMode.Open))
      {
        if (fileStream.Length > 0L)
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
                  if ("title".Equals(xmlNode2.Name))
                  {
                    XmlNamedNodeMap attributes = (XmlNamedNodeMap) xmlNode2.Attributes;
                    int titleId = int.Parse(attributes.GetNamedItem("id").Value);
                    TitlesXML.titles.Add(new TitleQ(titleId)
                    {
                      _classId = int.Parse(attributes.GetNamedItem("list").Value),
                      _medals = int.Parse(attributes.GetNamedItem("medals").Value),
                      _brooch = int.Parse(attributes.GetNamedItem("brooch").Value),
                      _blueOrder = int.Parse(attributes.GetNamedItem("blueOrder").Value),
                      _insignia = int.Parse(attributes.GetNamedItem("insignia").Value),
                      _rank = int.Parse(attributes.GetNamedItem("rank").Value),
                      _slot = int.Parse(attributes.GetNamedItem("slot").Value),
                      _req1 = int.Parse(attributes.GetNamedItem("reqT1").Value),
                      _req2 = int.Parse(attributes.GetNamedItem("reqT2").Value)
                    });
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
  }
}
