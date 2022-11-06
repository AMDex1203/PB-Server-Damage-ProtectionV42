
// Type: Core.xml.MissionsXML
// Assembly: Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 263E2FBF-1098-4552-8FB3-F6A660503737
// Interprise: C:\Users\Cuzin\3,50Core.dll

using Core.sql;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;

namespace Core.xml
{
  public class MissionsXML
  {
    public static uint _missionPage1;
    public static uint _missionPage2;
    private static List<MissionModel> Missions = new List<MissionModel>();

    public static void Load()
    {
      try
      {
        using (NpgsqlConnection npgsqlConnection = SQLjec.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          npgsqlConnection.Open();
          command.CommandText = "SELECT * FROM info_missions ORDER BY mission_id ASC";
          command.CommandType = CommandType.Text;
          NpgsqlDataReader npgsqlDataReader = command.ExecuteReader();
          while (npgsqlDataReader.Read())
          {
            bool boolean = npgsqlDataReader.GetBoolean(2);
            MissionModel missionModel = new MissionModel()
            {
              id = npgsqlDataReader.GetInt32(0),
              price = npgsqlDataReader.GetInt32(1)
            };
            uint num1 = (uint) (1 << missionModel.id);
            int num2 = (int) Math.Ceiling((double) missionModel.id / 32.0);
            if (boolean)
            {
              if (num2 == 1)
                MissionsXML._missionPage1 += num1;
              else if (num2 == 2)
                MissionsXML._missionPage2 += num1;
            }
            MissionsXML.Missions.Add(missionModel);
          }
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

    public static int GetMissionPrice(int id)
    {
      for (int index = 0; index < MissionsXML.Missions.Count; ++index)
      {
        MissionModel mission = MissionsXML.Missions[index];
        if (mission.id == id)
          return mission.price;
      }
      return -1;
    }
  }
}
