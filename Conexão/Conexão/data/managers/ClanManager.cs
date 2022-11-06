
// Type: Auth.data.managers.ClanManager
// Assembly: pbserver_auth, Version=1.0.7796.38604, Culture=neutral, PublicKeyToken=null
// MVID: 1CFDFF97-782C-4EF8-A621-6E4FDDE32436
// Interprise: C:\Users\Cuzin\3,50pbserver_auth.exe

using Auth.data.model;
using Core;
using Core.models.account.clan;
using Core.sql;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;

namespace Auth.data.managers
{
  public class ClanManager
  {
    public static Clan getClanDB(object valor, int type)
    {
      try
      {
        Clan clan = new Clan();
        if (type == 1 && (int) valor <= 0 || type == 0 && string.IsNullOrEmpty(valor.ToString()))
          return clan;
        using (NpgsqlConnection npgsqlConnection = SQLjec.getInstance().conn())
        {
          string str = type == 0 ? "clan_name" : "clan_id";
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          npgsqlConnection.Open();
          command.Parameters.AddWithValue("@valor", valor);
          command.CommandText = "SELECT * FROM clan_data WHERE " + str + "=@valor";
          command.CommandType = CommandType.Text;
          NpgsqlDataReader npgsqlDataReader = command.ExecuteReader();
          while (npgsqlDataReader.Read())
          {
            clan.id = npgsqlDataReader.GetInt32(0);
            clan.rank = npgsqlDataReader.GetByte(1);
            clan.name = npgsqlDataReader.GetString(2);
            clan.ownerId = npgsqlDataReader.GetInt64(3);
            clan.logo = (uint) npgsqlDataReader.GetInt64(4);
            clan.nameColor = npgsqlDataReader.GetByte(5);
          }
          command.Dispose();
          npgsqlDataReader.Close();
          npgsqlConnection.Dispose();
          npgsqlConnection.Close();
        }
        return clan.id == 0 ? new Clan() : clan;
      }
      catch
      {
        return new Clan();
      }
    }

    public static List<Account> getClanPlayers(int clanId, long exception)
    {
      List<Account> accountList = new List<Account>();
      if (clanId <= 0)
        return accountList;
      try
      {
        using (NpgsqlConnection npgsqlConnection = SQLjec.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          npgsqlConnection.Open();
          command.Parameters.AddWithValue("@clan", (object) clanId);
          command.CommandText = "SELECT player_id,player_name,rank,online,status FROM accounts WHERE clan_id=@clan";
          command.CommandType = CommandType.Text;
          NpgsqlDataReader npgsqlDataReader = command.ExecuteReader();
          while (npgsqlDataReader.Read())
          {
            long int64 = npgsqlDataReader.GetInt64(0);
            if (int64 != exception)
            {
              Account account = new Account()
              {
                player_id = int64,
                player_name = npgsqlDataReader.GetString(1),
                _rank = npgsqlDataReader.GetInt32(2),
                _isOnline = npgsqlDataReader.GetBoolean(3)
              };
              account._status.SetData((uint) npgsqlDataReader.GetInt64(4), int64);
              if (account._isOnline && !AccountManager.getInstance()._contas.ContainsKey(int64))
              {
                account.setOnlineStatus(false);
                account._status.ResetData(account.player_id);
              }
              accountList.Add(account);
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
      return accountList;
    }

    public static List<Account> getClanPlayers(
      int clanId,
      long exception,
      bool isOnline)
    {
      List<Account> accountList = new List<Account>();
      if (clanId <= 0)
        return accountList;
      try
      {
        using (NpgsqlConnection npgsqlConnection = SQLjec.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          npgsqlConnection.Open();
          command.Parameters.AddWithValue("@clan", (object) clanId);
          command.Parameters.AddWithValue("@on", (object) isOnline);
          command.CommandText = "SELECT player_id,player_name,rank,online,status FROM accounts WHERE clan_id=@clan AND online=@on";
          command.CommandType = CommandType.Text;
          NpgsqlDataReader npgsqlDataReader = command.ExecuteReader();
          while (npgsqlDataReader.Read())
          {
            long int64 = npgsqlDataReader.GetInt64(0);
            if (int64 != exception)
            {
              Account account = new Account()
              {
                player_id = int64,
                player_name = npgsqlDataReader.GetString(1),
                _rank = npgsqlDataReader.GetInt32(2),
                _isOnline = npgsqlDataReader.GetBoolean(3)
              };
              account._status.SetData((uint) npgsqlDataReader.GetInt64(4), int64);
              if (account._isOnline && !AccountManager.getInstance()._contas.ContainsKey(int64))
              {
                account.setOnlineStatus(false);
                account._status.ResetData(account.player_id);
              }
              accountList.Add(account);
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
      return accountList;
    }
  }
}
