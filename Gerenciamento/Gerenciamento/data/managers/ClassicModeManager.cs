
// Type: Game.data.managers.ClassicModeManager
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Game.data.managers
{
  public static class ClassicModeManager
  {
    public static List<int> itemscamp = new List<int>();
    public static List<int> itemscnpb = new List<int>();
    public static readonly List<int> CuponsEffectsBlocked = new List<int>();
    private static readonly string path = "data/TournamentRules.xml";

    public static void Load()
    {
      if (!File.Exists(ClassicModeManager.path))
      {
        Logger.warning(" [RegrasDeJogo] " + ClassicModeManager.path + " no exists.");
      }
      else
      {
        ClassicModeManager.GenerateList();
        Logger.warning(string.Format(" [RegrasDeJogo] Carregada {0} regras @CAMP.", (object) ClassicModeManager.itemscamp.Count));
        Logger.warning(string.Format(" [RegrasDeJogo] Carregada {0} regras @CNPB.", (object) ClassicModeManager.itemscnpb.Count));
        Logger.warning(string.Format(" [RegrasDeJogo] Carregado {0} CUPONS.", (object) ClassicModeManager.CuponsEffectsBlocked.Count));
      }
    }

    private static void GenerateList()
    {
      try
      {
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load(ClassicModeManager.path);
        for (XmlNode xmlNode1 = xmlDocument.FirstChild; xmlNode1 != null; xmlNode1 = xmlNode1.NextSibling)
        {
          if ("list".Equals(xmlNode1.Name))
          {
            for (XmlNode xmlNode2 = xmlNode1.FirstChild; xmlNode2 != null; xmlNode2 = xmlNode2.NextSibling)
            {
              if ("Rule".Equals(xmlNode2.Name))
              {
                XmlNamedNodeMap attributes = (XmlNamedNodeMap) xmlNode2.Attributes;
                string str = attributes.GetNamedItem("Name").Value;
                int num = int.Parse(attributes.GetNamedItem("Id").Value);
                if (str.Equals("@CAMP"))
                  ClassicModeManager.itemscamp.Add(num);
                else if (str.Equals("@CNPB"))
                  ClassicModeManager.itemscnpb.Add(num);
                else if (str.Equals("COUPON"))
                  ClassicModeManager.CuponsEffectsBlocked.Add(num);
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        Logger.error("Ocorreu um problema ao carregar os Tournament Rules!\r\n" + ex.ToString());
      }
    }

    public static void ReGenerateList()
    {
      ClassicModeManager.itemscamp.Clear();
      ClassicModeManager.itemscnpb.Clear();
      ClassicModeManager.CuponsEffectsBlocked.Clear();
      ClassicModeManager.Load();
    }

    public static bool IsBlocked(int listid, int id) => listid == id;

    public static bool CheckRoomRule(string name) => ConfigGS.EnableClassicRules && (name.Contains("@CAMP") || name.Contains("CAMP") || (name.Contains("@CNPB") || name.Contains("CNPB")));

    public static bool IsBlocked(int listid, int id, ref List<string> list, string category)
    {
      if (listid != id)
        return false;
      list.Add(category);
      return true;
    }
  }
}
