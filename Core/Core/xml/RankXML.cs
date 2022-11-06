
// Type: Core.xml.RankXML
// Assembly: Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 263E2FBF-1098-4552-8FB3-F6A660503737
// Interprise: C:\Users\Cuzin\3,50Core.dll

using Core.models.account.players;
using Core.models.account.rank;
using Core.sql;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Xml;

namespace Core.xml
{
  public class RankXML
  {
    private static List<RankModel> _ranks = new List<RankModel>();
    private static SortedList<int, List<ItemsModel>> _awards = new SortedList<int, List<ItemsModel>>();

    public static void Load()
    {
      string path = "data/ranktemplate/rankplayertemplate.xml";
      if (File.Exists(path))
        RankXML.parse(path);
      else
        Logger.warning("[RankXML] Não existe o arquivo: " + path);
    }

    public static RankModel getRank(int rankId)
    {
      lock (RankXML._ranks)
      {
        for (int index = 0; index < RankXML._ranks.Count; ++index)
        {
          RankModel rank = RankXML._ranks[index];
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
          Logger.warning("[RankXML] O arquivo está vazio: " + path);
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
                    RankXML._ranks.Add(new RankModel(int.Parse(attributes.GetNamedItem("id").Value), attributes.GetNamedItem("name").Value, int.Parse(attributes.GetNamedItem("onNextLevel").Value), int.Parse(attributes.GetNamedItem("onGPUp").Value), int.Parse(attributes.GetNamedItem("onAllExp").Value)));
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

    public static List<ItemsModel> getAwards(int rank)
    {
      lock (RankXML._awards)
      {
        List<ItemsModel> itemsModelList;
        if (RankXML._awards.TryGetValue(rank, out itemsModelList))
          return itemsModelList;
      }
      return new List<ItemsModel>();
    }

    public static void LoadAwards()
    {
      try
      {
        using (NpgsqlConnection npgsqlConnection = SQLjec.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          npgsqlConnection.Open();
          command.CommandText = "SELECT * FROM info_rank_awards ORDER BY rank_id ASC";
          command.CommandType = CommandType.Text;
          NpgsqlDataReader npgsqlDataReader = command.ExecuteReader();
          while (npgsqlDataReader.Read())
            RankXML.AddItemToList(npgsqlDataReader.GetInt32(0), new ItemsModel(npgsqlDataReader.GetInt32(1))
            {
              _name = npgsqlDataReader.GetString(2),
              _count = (uint) npgsqlDataReader.GetInt32(3),
              _equip = npgsqlDataReader.GetInt32(4)
            });
          command.Dispose();
          npgsqlDataReader.Close();
          npgsqlConnection.Dispose();
          npgsqlConnection.Close();
        }
      }
      catch (Exception ex)
      {
        Logger.error(ex.ToString());
      }
    }

    private static void AddItemToList(int rank, ItemsModel item)
    {
      if (RankXML._awards.ContainsKey(rank))
        RankXML._awards[rank].Add(item);
      else
        RankXML._awards.Add(rank, new List<ItemsModel>()
        {
          item
        });
    }
  }
}
