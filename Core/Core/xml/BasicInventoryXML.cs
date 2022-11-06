
// Type: Core.xml.BasicInventoryXML
// Assembly: Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 263E2FBF-1098-4552-8FB3-F6A660503737
// Interprise: C:\Users\Cuzin\3,50Core.dll

using Core.models.account.players;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Core.xml
{
  public class BasicInventoryXML
  {
    public static List<ItemsModel> basic = new List<ItemsModel>();
    public static List<ItemsModel> creationAwards = new List<ItemsModel>();

    public static void Load()
    {
      string path = "data/playertemplate/startitems.xml";
      if (File.Exists(path))
        BasicInventoryXML.parse(path);
      else
        Logger.warning("[BasicInventoryXML] Não existe o arquivo: " + path);
    }

    public static ItemsModel GetItemsBasic(int item_id)
    {
      lock (BasicInventoryXML.basic)
      {
        for (int index = 0; index < BasicInventoryXML.basic.Count; ++index)
        {
          ItemsModel itemsModel = BasicInventoryXML.basic[index];
          if (itemsModel._id == item_id)
            return itemsModel;
        }
        return (ItemsModel) null;
      }
    }

    public static ItemsModel GetItemsAwards(int item_id)
    {
      lock (BasicInventoryXML.creationAwards)
      {
        for (int index = 0; index < BasicInventoryXML.creationAwards.Count; ++index)
        {
          ItemsModel creationAward = BasicInventoryXML.creationAwards[index];
          if (creationAward._id == item_id)
            return creationAward;
        }
        return (ItemsModel) null;
      }
    }

    private static void parse(string path)
    {
      XmlDocument xmlDocument = new XmlDocument();
      using (FileStream fileStream = new FileStream(path, FileMode.Open))
      {
        if (fileStream.Length == 0L)
        {
          Logger.warning("[BasicInventoryXml] O arquivo está vazio: " + path);
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
                  if ("message".Equals(xmlNode2.Name))
                  {
                    XmlNamedNodeMap attributes = (XmlNamedNodeMap) xmlNode2.Attributes;
                    int num = int.Parse(attributes.GetNamedItem("present").Value);
                    ItemsModel itemsModel = new ItemsModel(int.Parse(attributes.GetNamedItem("item_id").Value))
                    {
                      _name = attributes.GetNamedItem("item_name").Value,
                      _count = uint.Parse(attributes.GetNamedItem("item_count").Value),
                      _equip = int.Parse(attributes.GetNamedItem("item_equip").Value)
                    };
                    switch (num)
                    {
                      case 0:
                        BasicInventoryXML.basic.Add(itemsModel);
                        continue;
                      case 1:
                        BasicInventoryXML.creationAwards.Add(itemsModel);
                        continue;
                      default:
                        continue;
                    }
                  }
                }
              }
            }
          }
          catch (Exception ex)
          {
            Logger.error(ex.ToString());
          }
        }
      }
    }
  }
}
