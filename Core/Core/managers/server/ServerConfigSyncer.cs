
// Type: Core.managers.server.ServerConfigSyncer
// Assembly: Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 263E2FBF-1098-4552-8FB3-F6A660503737
// Interprise: C:\Users\Cuzin\3,50Core.dll

using Core.server;
using Core.sql;
using Npgsql;
using System;
using System.Data;

namespace Core.managers.server
{
  public static class ServerConfigSyncer
  {
    public static ServerConfig GenerateConfig(int configId)
    {
      ServerConfig serverConfig = (ServerConfig) null;
      if (configId == 0)
        return serverConfig;
      try
      {
        using (NpgsqlConnection npgsqlConnection = SQLjec.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          npgsqlConnection.Open();
          command.Parameters.AddWithValue("@cfg", (object) configId);
          command.CommandText = "SELECT * FROM info_login_configs WHERE config_id=@cfg";
          command.CommandType = CommandType.Text;
          NpgsqlDataReader npgsqlDataReader = command.ExecuteReader();
          while (npgsqlDataReader.Read())
            serverConfig = new ServerConfig()
            {
              configId = configId,
              onlyGM = npgsqlDataReader.GetBoolean(1),
              missions = npgsqlDataReader.GetBoolean(2),
              UserFileList = npgsqlDataReader.GetString(3),
              ClientVersion = npgsqlDataReader.GetString(4),
              GiftSystem = npgsqlDataReader.GetBoolean(5),
              ExitURL = npgsqlDataReader.GetString(6)
            };
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
      return serverConfig;
    }

    public static bool updateMission(ServerConfig cfg, bool mission)
    {
      cfg.missions = mission;
      return ComDiv.updateDB("info_login_configs", "missions", (object) mission, "config_id", (object) cfg.configId);
    }
  }
}
