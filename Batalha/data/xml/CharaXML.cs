
// Type: Battle.data.xml.CharaXML
// Assembly: pbserver_battle, Version=1.0.0.2646, Culture=neutral, PublicKeyToken=null
// MVID: AA2D7911-B687-4238-AB73-D37E54123039
// Interprise: C:\Users\Cuzin\3,50pbserver_battle.exe

using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Battle.data.xml
{
  public class CharaXML
  {
    public static List<CharaModel> _charas = new List<CharaModel>();

    public static int getLifeById(int charaId, int type)
    {
      for (int index = 0; index < CharaXML._charas.Count; ++index)
      {
        CharaModel chara = CharaXML._charas[index];
        if (chara.Id == charaId && chara.Type == type)
          return chara.Life;
      }
      return 100;
    }

    public static void Load()
    {
      string path = "data/battle/charas.xml";
      if (File.Exists(path))
        CharaXML.parse(path);
      else
        Logger.warning("[CharaXML] Não existe o arquivo: " + path);
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
                  if ("Chara".Equals(xmlNode2.Name))
                  {
                    XmlNamedNodeMap attributes = (XmlNamedNodeMap) xmlNode2.Attributes;
                    CharaModel charaModel = new CharaModel()
                    {
                      Id = int.Parse(attributes.GetNamedItem("Id").Value),
                      Type = int.Parse(attributes.GetNamedItem("Type").Value),
                      Life = int.Parse(attributes.GetNamedItem("Life").Value)
                    };
                    CharaXML._charas.Add(charaModel);
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
      Logger.warning("[Aviso] Loaded " + CharaXML._charas.Count.ToString() + " charas information");
    }
  }
}
