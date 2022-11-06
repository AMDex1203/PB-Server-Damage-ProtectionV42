
// Type: Core.xml.MapsXML
// Assembly: Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 263E2FBF-1098-4552-8FB3-F6A660503737
// Interprise: C:\Users\Cuzin\3,50Core.dll

using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Core.xml
{
  public class MapsXML
  {
    public static List<byte> TagList = new List<byte>();
    public static List<ushort> ModeList = new List<ushort>();
    public static uint maps1;
    public static uint maps2;
    public static uint maps3;
    public static uint maps4;

    public static void Load()
    {
      string path = "data/maps/modes.xml";
      if (File.Exists(path))
        MapsXML.parse(path);
      else
        Logger.warning("[MapsXML] Não existe o arquivo: " + path);
    }

    private static void parse(string path)
    {
      XmlDocument xmlDocument = new XmlDocument();
      using (FileStream fileStream = new FileStream(path, FileMode.Open))
      {
        if (fileStream.Length == 0L)
        {
          Logger.warning("[MapsXML] O arquivo está vazio: " + path);
        }
        else
        {
          try
          {
            int num1 = 0;
            int num2 = 1;
            xmlDocument.Load((Stream) fileStream);
            for (XmlNode xmlNode1 = xmlDocument.FirstChild; xmlNode1 != null; xmlNode1 = xmlNode1.NextSibling)
            {
              if ("list".Equals(xmlNode1.Name))
              {
                for (XmlNode xmlNode2 = xmlNode1.FirstChild; xmlNode2 != null; xmlNode2 = xmlNode2.NextSibling)
                {
                  if ("map".Equals(xmlNode2.Name))
                  {
                    XmlNamedNodeMap attributes = (XmlNamedNodeMap) xmlNode2.Attributes;
                    uint num3 = (uint) (1 << num1++);
                    int num4 = num2;
                    if (num1 == 32)
                    {
                      ++num2;
                      num1 = 0;
                    }
                    MapsXML.TagList.Add(byte.Parse(attributes.GetNamedItem("tag").Value));
                    MapsXML.ModeList.Add(ushort.Parse(attributes.GetNamedItem("mode").Value));
                    if (bool.Parse(attributes.GetNamedItem("enable").Value))
                    {
                      switch (num4)
                      {
                        case 1:
                          MapsXML.maps1 += num3;
                          break;
                        case 2:
                          MapsXML.maps2 += num3;
                          break;
                        case 3:
                          MapsXML.maps3 += num3;
                          break;
                        case 4:
                          MapsXML.maps4 += num3;
                          break;
                        default:
                          Logger.warning("[Lista não definida] Flag: " + num3.ToString() + "; List: " + num4.ToString());
                          break;
                      }
                    }
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
