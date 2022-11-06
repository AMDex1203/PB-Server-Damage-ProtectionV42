﻿
// Type: Core.managers.MissionManager
// Assembly: Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 263E2FBF-1098-4552-8FB3-F6A660503737
// Interprise: C:\Users\Cuzin\3,50Core.dll

using Core.models.account.players;
using Core.server;
using Core.sql;
using Npgsql;
using System;
using System.Data;

namespace Core.managers
{
  public class MissionManager
  {
    private static MissionManager acm = new MissionManager();

    public static MissionManager getInstance() => MissionManager.acm;

    public void addMissionDB(long player_id)
    {
      if (player_id == 0L)
        return;
      try
      {
        using (NpgsqlConnection npgsqlConnection = SQLjec.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          npgsqlConnection.Open();
          command.Parameters.AddWithValue("@owner", (object) player_id);
          command.CommandText = "INSERT INTO player_missions (owner_id) VALUES (@owner)";
          command.CommandType = CommandType.Text;
          command.ExecuteNonQuery();
          command.Dispose();
          npgsqlConnection.Close();
        }
      }
      catch (Exception ex)
      {
        Logger.error(ex.ToString());
      }
    }

    public PlayerMissions getMission(
      long pId,
      int mission1,
      int mission2,
      int mission3,
      int mission4)
    {
      if (pId == 0L)
        return (PlayerMissions) null;
      PlayerMissions playerMissions = (PlayerMissions) null;
      try
      {
        using (NpgsqlConnection npgsqlConnection = SQLjec.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          npgsqlConnection.Open();
          command.Parameters.AddWithValue("@owner", (object) pId);
          command.CommandText = "SELECT * FROM player_missions WHERE owner_id=@owner";
          command.CommandType = CommandType.Text;
          NpgsqlDataReader npgsqlDataReader = command.ExecuteReader();
          while (npgsqlDataReader.Read())
          {
            playerMissions = new PlayerMissions()
            {
              actualMission = npgsqlDataReader.GetInt32(1),
              card1 = npgsqlDataReader.GetInt32(2),
              card2 = npgsqlDataReader.GetInt32(3),
              card3 = npgsqlDataReader.GetInt32(4),
              card4 = npgsqlDataReader.GetInt32(5),
              mission1 = mission1,
              mission2 = mission2,
              mission3 = mission3,
              mission4 = mission4
            };
            npgsqlDataReader.GetBytes(6, 0L, playerMissions.list1, 0, 40);
            npgsqlDataReader.GetBytes(7, 0L, playerMissions.list2, 0, 40);
            npgsqlDataReader.GetBytes(8, 0L, playerMissions.list3, 0, 40);
            npgsqlDataReader.GetBytes(9, 0L, playerMissions.list4, 0, 40);
            playerMissions.UpdateSelectedCard();
          }
          command.Dispose();
          npgsqlDataReader.Close();
          npgsqlConnection.Dispose();
          npgsqlConnection.Close();
        }
        return playerMissions;
      }
      catch (Exception ex)
      {
        Logger.error(ex.ToString());
        return (PlayerMissions) null;
      }
    }

    public void updateCurrentMissionList(long player_id, PlayerMissions mission)
    {
      byte[] currentMissionList = mission.getCurrentMissionList();
      ComDiv.updateDB("player_missions", nameof (mission) + (mission.actualMission + 1).ToString(), (object) currentMissionList, "owner_id", (object) player_id);
    }
  }
}
