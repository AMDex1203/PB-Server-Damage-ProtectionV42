
// Type: Core.xml.DirectXML
// Assembly: Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 263E2FBF-1098-4552-8FB3-F6A660503737
// Interprise: C:\Users\Cuzin\3,50Core.dll

using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Core.xml
{
  public class DirectXML
  {
    public static List<string> md5s = new List<string>();

    public static void Start()
    {
      string path = "data/DirectX.xml";
      if (File.Exists(path))
        DirectXML.Load(path);
      else
        Logger.warning("[DirectXML] Não existe o arquivo: " + path);
    }

    public static bool IsValid(string md5)
    {
      if (string.IsNullOrEmpty(md5))
        return true;
      for (int index = 0; index < DirectXML.md5s.Count; ++index)
      {
        if (DirectXML.md5s[index] == md5)
          return true;
      }
      return false;
    }

    private static void Load(string path)
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
                  if ("d3d9".Equals(xmlNode2.Name))
                  {
                    XmlNamedNodeMap attributes = (XmlNamedNodeMap) xmlNode2.Attributes;
                    DirectXML.md5s.Add(attributes.GetNamedItem("md5").Value);
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
