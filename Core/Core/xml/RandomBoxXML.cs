
// Type: Core.xml.RandomBoxXML
// Assembly: Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 263E2FBF-1098-4552-8FB3-F6A660503737
// Interprise: C:\Users\Cuzin\3,50Core.dll

using Core.models.account.players;
using Core.models.randombox;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Core.xml
{
  public class RandomBoxXML
  {
    private static SortedList<int, RandomBoxModel> boxes = new SortedList<int, RandomBoxModel>();

    public static void LoadBoxes()
    {
      DirectoryInfo directoryInfo = new DirectoryInfo(Directory.GetCurrentDirectory() + "\\data\\cupons");
      if (!directoryInfo.Exists)
        return;
      foreach (FileInfo file in directoryInfo.GetFiles())
      {
        try
        {
          RandomBoxXML.LoadBox(int.Parse(file.Name.Substring(0, file.Name.Length - 4)));
        }
        catch
        {
        }
      }
    }

    private static void LoadBox(int id)
    {
      string path = "data/cupons/" + id.ToString() + ".xml";
      if (File.Exists(path))
        RandomBoxXML.parse(path, id);
      else
        Logger.warning("[RandomBoxXML] Não existe o arquivo: " + path);
    }

    private static void parse(string path, int cupomId)
    {
      XmlDocument xmlDocument = new XmlDocument();
      using (FileStream fileStream = new FileStream(path, FileMode.Open))
      {
        if (fileStream.Length == 0L)
        {
          Logger.warning("[RandomBoxXML] O arquivo está vazio: " + path);
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
                XmlNamedNodeMap attributes1 = (XmlNamedNodeMap) xmlNode1.Attributes;
                RandomBoxModel randomBoxModel = new RandomBoxModel()
                {
                  itemsCount = int.Parse(attributes1.GetNamedItem("count").Value)
                };
                for (XmlNode xmlNode2 = xmlNode1.FirstChild; xmlNode2 != null; xmlNode2 = xmlNode2.NextSibling)
                {
                  if ("item".Equals(xmlNode2.Name))
                  {
                    XmlNamedNodeMap attributes2 = (XmlNamedNodeMap) xmlNode2.Attributes;
                    ItemsModel itemsModel = (ItemsModel) null;
                    int id = int.Parse(attributes2.GetNamedItem("id").Value);
                    uint num = uint.Parse(attributes2.GetNamedItem("count").Value);
                    if (id > 0)
                      itemsModel = new ItemsModel(id)
                      {
                        _name = "Randombox",
                        _equip = int.Parse(attributes2.GetNamedItem("equip").Value),
                        _count = num
                      };
                    randomBoxModel.items.Add(new RandomBoxItem()
                    {
                      index = int.Parse(attributes2.GetNamedItem("index").Value),
                      percent = int.Parse(attributes2.GetNamedItem("percent").Value),
                      special = bool.Parse(attributes2.GetNamedItem("special").Value),
                      count = num,
                      item = itemsModel
                    });
                  }
                }
                randomBoxModel.SetTopPercent();
                RandomBoxXML.boxes.Add(cupomId, randomBoxModel);
              }
            }
          }
          catch (Exception ex)
          {
            Logger.error("[Box: " + cupomId.ToString() + "] " + ex.ToString());
          }
        }
        fileStream.Dispose();
        fileStream.Close();
      }
    }

    public static bool ContainsBox(int id) => RandomBoxXML.boxes.ContainsKey(id);

    public static RandomBoxModel getBox(int id)
    {
      try
      {
        return RandomBoxXML.boxes[id];
      }
      catch
      {
        return (RandomBoxModel) null;
      }
    }
  }
}
