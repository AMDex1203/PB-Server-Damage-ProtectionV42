
// Type: Core.managers.BanManager
// Assembly: Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 263E2FBF-1098-4552-8FB3-F6A660503737
// Interprise: C:\Users\Cuzin\3,50Core.dll

using Core.server;
using Core.sql;
using Npgsql;
using System;

namespace Core.managers
{
  public static class BanManager
  {
    public static BanHistory GetAccountBan(long object_id)
    {
      BanHistory banHistory = new BanHistory();
      if (object_id == 0L)
        return banHistory;
      try
      {
        using (NpgsqlConnection npgsqlConnection = SQLjec.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          npgsqlConnection.Open();
          command.Parameters.AddWithValue("@obj", (object) object_id);
          command.CommandText = "SELECT * FROM ban_history WHERE object_id=@obj";
          NpgsqlDataReader npgsqlDataReader = command.ExecuteReader();
          while (npgsqlDataReader.Read())
          {
            banHistory.object_id = object_id;
            banHistory.provider_id = npgsqlDataReader.GetInt64(1);
            banHistory.type = npgsqlDataReader.GetString(2);
            banHistory.value = npgsqlDataReader.GetString(3);
            banHistory.reason = npgsqlDataReader.GetString(4);
            banHistory.startDate = npgsqlDataReader.GetDateTime(5);
            banHistory.endDate = npgsqlDataReader.GetDateTime(6);
          }
          command.Dispose();
          npgsqlDataReader.Close();
          npgsqlConnection.Dispose();
          npgsqlConnection.Close();
        }
      }
      catch (Exception ex)
      {
        Logger.warning(ex.ToString());
        return (BanHistory) null;
      }
      return banHistory;
    }

    public static void GetBanStatus(string mac, string ip, out bool validMac, out bool validIp)
    {
      validMac = false;
      validIp = false;
      try
      {
        DateTime now = DateTime.Now;
        using (NpgsqlConnection npgsqlConnection = SQLjec.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          npgsqlConnection.Open();
          command.Parameters.AddWithValue("@mac", (object) mac);
          command.Parameters.AddWithValue("@ip", (object) ip);
          command.CommandText = "SELECT * FROM ban_history WHERE value in (@mac,@ip)";
          NpgsqlDataReader npgsqlDataReader = command.ExecuteReader();
          while (npgsqlDataReader.Read())
          {
            string str1 = npgsqlDataReader.GetString(2);
            string str2 = npgsqlDataReader.GetString(3);
            if (!(npgsqlDataReader.GetDateTime(6) < now))
            {
              if (str1 == "MAC" && str2 == mac)
                validMac = true;
              else if (str1 == "IP" && str2 == ip)
                validIp = true;
            }
          }
          command.Dispose();
          npgsqlDataReader.Close();
          npgsqlConnection.Dispose();
          npgsqlConnection.Close();
        }
      }
      catch (Exception ex)
      {
        Logger.warning(ex.ToString());
      }
    }

    public static BanHistory SaveHistory(
      long provider,
      string type,
      string value,
      DateTime end)
    {
      BanHistory banHistory = new BanHistory()
      {
        provider_id = provider,
        type = type,
        value = value,
        endDate = end
      };
      try
      {
        using (NpgsqlConnection npgsqlConnection = SQLjec.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          npgsqlConnection.Open();
          command.Parameters.AddWithValue("@provider", (object) banHistory.provider_id);
          command.Parameters.AddWithValue("@type", (object) banHistory.type);
          command.Parameters.AddWithValue("@value", (object) banHistory.value);
          command.Parameters.AddWithValue("@reason", (object) banHistory.reason);
          command.Parameters.AddWithValue("@start", (object) banHistory.startDate);
          command.Parameters.AddWithValue("@end", (object) banHistory.endDate);
          command.CommandText = "INSERT INTO ban_history(provider_id,type,value,reason,start_date,expire_date)VALUES(@provider,@type,@value,@reason,@start,@end) RETURNING object_id";
          object obj = command.ExecuteScalar();
          banHistory.object_id = (long) obj;
          command.Dispose();
          npgsqlConnection.Dispose();
          npgsqlConnection.Close();
          return banHistory;
        }
      }
      catch
      {
        return (BanHistory) null;
      }
    }

    public static bool SaveBanReason(long object_id, string reason) => ComDiv.updateDB("ban_history", nameof (reason), (object) reason, nameof (object_id), (object) object_id);
  }
}
