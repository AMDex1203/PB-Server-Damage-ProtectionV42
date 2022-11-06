
// Type: Core.xml.TitleAwardsXML
// Assembly: Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 263E2FBF-1098-4552-8FB3-F6A660503737
// Interprise: C:\Users\Cuzin\3,50Core.dll

using Core.models.account.players;
using Core.models.account.title;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Core.xml
{
  public class TitleAwardsXML
  {
    public static List<TitleA> awards = new List<TitleA>();

    public static void Load()
    {
      string path = "data/titles/title_awards2.xml";
      if (File.Exists(path))
        TitleAwardsXML.parse(path);
      else
        Logger.warning("[TitleAwards] Não existe o arquivo: " + path);
    }

    public static List<ItemsModel> getAwards(int titleId)
    {
      List<ItemsModel> itemsModelList = new List<ItemsModel>();
      lock (TitleAwardsXML.awards)
      {
        for (int index = 0; index < TitleAwardsXML.awards.Count; ++index)
        {
          TitleA award = TitleAwardsXML.awards[index];
          if (award._id == titleId)
            itemsModelList.Add(award._item);
        }
      }
      return itemsModelList;
    }

    public static bool Contains(int titleId, int itemId)
    {
      if (itemId == 0)
        return false;
      for (int index = 0; index < TitleAwardsXML.awards.Count; ++index)
      {
        TitleA award = TitleAwardsXML.awards[index];
        if (award._id == titleId && award._item._id == itemId)
          return true;
      }
      return false;
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
                    int id = int.Parse(attributes.GetNamedItem("itemid").Value);
                    TitleAwardsXML.awards.Add(new TitleA()
                    {
                      _id = int.Parse(attributes.GetNamedItem("id").Value),
                      _item = new ItemsModel(id, "Title reward", int.Parse(attributes.GetNamedItem("equip").Value), uint.Parse(attributes.GetNamedItem("count").Value))
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
