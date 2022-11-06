
// Type: Core.xml.ServersXML
// Assembly: Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 263E2FBF-1098-4552-8FB3-F6A660503737
// Interprise: C:\Users\Cuzin\3,50Core.dll

using Core.models.servers;
using Core.sql;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;

namespace Core.xml
{
  public class ServersXML
  {
    public static List<GameServerModel> _servers = new List<GameServerModel>();

    public static GameServerModel getServer(int id)
    {
      lock (ServersXML._servers)
      {
        for (int index = 0; index < ServersXML._servers.Count; ++index)
        {
          GameServerModel server = ServersXML._servers[index];
          if (server._id == id)
            return server;
        }
        return (GameServerModel) null;
      }
    }

    public static void Load()
    {
      try
      {
        using (NpgsqlConnection npgsqlConnection = SQLjec.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          npgsqlConnection.Open();
          command.CommandText = "SELECT * FROM info_gameservers ORDER BY id ASC";
          command.CommandType = CommandType.Text;
          NpgsqlDataReader npgsqlDataReader = command.ExecuteReader();
          while (npgsqlDataReader.Read())
          {
            GameServerModel gameServerModel = new GameServerModel(npgsqlDataReader.GetString(3), (ushort) npgsqlDataReader.GetInt32(5))
            {
              _id = npgsqlDataReader.GetInt32(0),
              _state = npgsqlDataReader.GetInt32(1),
              _type = npgsqlDataReader.GetInt32(2),
              _port = (ushort) npgsqlDataReader.GetInt32(4),
              _maxPlayers = npgsqlDataReader.GetInt32(6)
            };
            ServersXML._servers.Add(gameServerModel);
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

    public static void UpdateServer(int serverId)
    {
      GameServerModel server = ServersXML.getServer(serverId);
      if (server == null)
        return;
      try
      {
        using (NpgsqlConnection npgsqlConnection = SQLjec.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          npgsqlConnection.Open();
          command.Parameters.AddWithValue("@id", (object) serverId);
          command.CommandText = "SELECT * FROM info_gameservers WHERE id=@id";
          command.CommandType = CommandType.Text;
          NpgsqlDataReader npgsqlDataReader = command.ExecuteReader();
          while (npgsqlDataReader.Read())
          {
            server._state = npgsqlDataReader.GetInt32(1);
            server._type = npgsqlDataReader.GetInt32(2);
            server._ip = npgsqlDataReader.GetString(3);
            server._port = (ushort) npgsqlDataReader.GetInt32(4);
            server._syncPort = (ushort) npgsqlDataReader.GetInt32(5);
            server._maxPlayers = npgsqlDataReader.GetInt32(6);
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
  }
}
