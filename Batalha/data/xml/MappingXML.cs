
// Type: Battle.data.xml.MappingXML
// Assembly: pbserver_battle, Version=1.0.0.2646, Culture=neutral, PublicKeyToken=null
// MVID: AA2D7911-B687-4238-AB73-D37E54123039
// Interprise: C:\Users\Cuzin\3,50pbserver_battle.exe

using Battle.data.models;
using SharpDX;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Battle.data.xml
{
  public class MappingXML
  {
    public static List<MapModel> _maps = new List<MapModel>();

    public static MapModel getMapById(int mapId)
    {
      for (int index = 0; index < MappingXML._maps.Count; ++index)
      {
        MapModel map = MappingXML._maps[index];
        if (map._id == mapId)
          return map;
      }
      return (MapModel) null;
    }

    public static void SetObjectives(ObjModel obj, Room room)
    {
      if (obj._ultraSYNC == 0)
        return;
      if (obj._ultraSYNC == 1 || obj._ultraSYNC == 3)
      {
        room._bar1 = obj._life;
        room._default1 = room._bar1;
      }
      else
      {
        if (obj._ultraSYNC != 2 && obj._ultraSYNC != 4)
          return;
        room._bar2 = obj._life;
        room._default2 = room._bar2;
      }
    }

    public static void Load()
    {
      string path = "data/battle/maps.xml";
      if (File.Exists(path))
        MappingXML.parse(path);
      else
        Logger.warning("[MappingXML] Não existe o arquivo: " + path);
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
                  if ("Map".Equals(xmlNode2.Name))
                  {
                    XmlNamedNodeMap attributes = (XmlNamedNodeMap) xmlNode2.Attributes;
                    MapModel map = new MapModel()
                    {
                      _id = int.Parse(attributes.GetNamedItem("Id").Value)
                    };
                    MappingXML.BombsXML(xmlNode2, map);
                    MappingXML.ObjectsXML(xmlNode2, map);
                    MappingXML._maps.Add(map);
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

    private static void BombsXML(XmlNode xmlNode, MapModel map)
    {
      for (XmlNode xmlNode1 = xmlNode.FirstChild; xmlNode1 != null; xmlNode1 = xmlNode1.NextSibling)
      {
        if ("BombPositions".Equals(xmlNode1.Name))
        {
          for (XmlNode xmlNode2 = xmlNode1.FirstChild; xmlNode2 != null; xmlNode2 = xmlNode2.NextSibling)
          {
            if ("Bomb".Equals(xmlNode2.Name))
            {
              XmlNamedNodeMap attributes = (XmlNamedNodeMap) xmlNode2.Attributes;
              BombPosition bombPosition = new BombPosition()
              {
                X = float.Parse(attributes.GetNamedItem("X").Value),
                Y = float.Parse(attributes.GetNamedItem("Y").Value),
                Z = float.Parse(attributes.GetNamedItem("Z").Value)
              };
              bombPosition.Position = new Half3(bombPosition.X, bombPosition.Y, bombPosition.Z);
              if ((double) bombPosition.X == 0.0 && (double) bombPosition.Y == 0.0 && (double) bombPosition.Z == 0.0)
                bombPosition.Everywhere = true;
              map._bombs.Add(bombPosition);
            }
          }
        }
      }
    }

    private static void ObjectsXML(XmlNode xmlNode, MapModel map)
    {
      for (XmlNode xmlNode1 = xmlNode.FirstChild; xmlNode1 != null; xmlNode1 = xmlNode1.NextSibling)
      {
        if ("objects".Equals(xmlNode1.Name))
        {
          for (XmlNode xmlNode2 = xmlNode1.FirstChild; xmlNode2 != null; xmlNode2 = xmlNode2.NextSibling)
          {
            if ("Obj".Equals(xmlNode2.Name))
            {
              XmlNamedNodeMap attributes = (XmlNamedNodeMap) xmlNode2.Attributes;
              ObjModel objModel = new ObjModel(bool.Parse(attributes.GetNamedItem("NeedSync").Value))
              {
                _id = int.Parse(attributes.GetNamedItem("Id").Value),
                _life = int.Parse(attributes.GetNamedItem("Life").Value),
                _anim1 = int.Parse(attributes.GetNamedItem("Anim1").Value)
              };
              if (objModel._life > -1)
                objModel.isDestroyable = true;
              if (objModel._anim1 > (int) byte.MaxValue)
              {
                if (objModel._anim1 == 256)
                  objModel._ultraSYNC = 1;
                else if (objModel._anim1 == 257)
                  objModel._ultraSYNC = 2;
                else if (objModel._anim1 == 258)
                  objModel._ultraSYNC = 3;
                else if (objModel._anim1 == 259)
                  objModel._ultraSYNC = 4;
                objModel._anim1 = (int) byte.MaxValue;
              }
              MappingXML.AnimsXML(xmlNode2, objModel);
              MappingXML.DEffectsXML(xmlNode2, objModel);
              map._objects.Add(objModel);
            }
          }
        }
      }
    }

    private static void AnimsXML(XmlNode xmlNode, ObjModel obj)
    {
      for (XmlNode xmlNode1 = xmlNode.FirstChild; xmlNode1 != null; xmlNode1 = xmlNode1.NextSibling)
      {
        if ("Anims".Equals(xmlNode1.Name))
        {
          for (XmlNode xmlNode2 = xmlNode1.FirstChild; xmlNode2 != null; xmlNode2 = xmlNode2.NextSibling)
          {
            if ("Sync".Equals(xmlNode2.Name))
            {
              XmlNamedNodeMap attributes = (XmlNamedNodeMap) xmlNode2.Attributes;
              AnimModel animModel = new AnimModel()
              {
                _id = int.Parse(attributes.GetNamedItem("Id").Value),
                _duration = float.Parse(attributes.GetNamedItem("Date").Value),
                _nextAnim = int.Parse(attributes.GetNamedItem("Next").Value),
                _otherObj = int.Parse(attributes.GetNamedItem("OtherOBJ").Value),
                _otherAnim = int.Parse(attributes.GetNamedItem("OtherANIM").Value)
              };
              if (animModel._id == 0)
                obj._noInstaSync = true;
              if (animModel._id != (int) byte.MaxValue)
                obj._updateId = 3;
              obj._anims.Add(animModel);
            }
          }
        }
      }
    }

    private static void DEffectsXML(XmlNode xmlNode, ObjModel obj)
    {
      for (XmlNode xmlNode1 = xmlNode.FirstChild; xmlNode1 != null; xmlNode1 = xmlNode1.NextSibling)
      {
        if ("DestroyEffects".Equals(xmlNode1.Name))
        {
          for (XmlNode xmlNode2 = xmlNode1.FirstChild; xmlNode2 != null; xmlNode2 = xmlNode2.NextSibling)
          {
            if ("Effect".Equals(xmlNode2.Name))
            {
              XmlNamedNodeMap attributes = (XmlNamedNodeMap) xmlNode2.Attributes;
              DEffectModel deffectModel = new DEffectModel()
              {
                _id = int.Parse(attributes.GetNamedItem("Id").Value),
                _life = int.Parse(attributes.GetNamedItem("Percent").Value)
              };
              obj._effects.Add(deffectModel);
            }
          }
        }
      }
    }
  }
}
