
// Type: Auth.data.managers.AccountManager
// Assembly: pbserver_auth, Version=1.0.7796.38604, Culture=neutral, PublicKeyToken=null
// MVID: 1CFDFF97-782C-4EF8-A621-6E4FDDE32436
// Interprise: C:\Users\Cuzin\3,50pbserver_auth.exe

using Auth.data.model;
using Core;
using Core.models.account;
using Core.models.account.players;
using Core.models.enums.flags;
using Core.sql;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;

namespace Auth.data.managers
{
  public class AccountManager
  {
    public SortedList<long, Account> _contas = new SortedList<long, Account>();
    private static AccountManager acm = new AccountManager();

    public bool AddAccount(Account acc)
    {
      lock (this._contas)
      {
        if (!this._contas.ContainsKey(acc.player_id))
        {
          this._contas.Add(acc.player_id, acc);
          return true;
        }
      }
      return false;
    }

    public Account getAccountDB(object valor, object valor2, int type, int searchFlag)
    {
      if (type == 0 && (string) valor == "" || type == 1 && (long) valor == 0L || type == 2 && (string.IsNullOrEmpty((string) valor) || string.IsNullOrEmpty((string) valor2)))
        return (Account) null;
      Account acc = (Account) null;
      try
      {
        using (NpgsqlConnection npgsqlConnection = SQLjec.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          npgsqlConnection.Open();
          command.Parameters.AddWithValue("@valor", valor);
          switch (type)
          {
            case 0:
              command.CommandText = "SELECT * FROM accounts WHERE login=@valor LIMIT 1";
              break;
            case 1:
              command.CommandText = "SELECT * FROM accounts WHERE player_id=@valor LIMIT 1";
              break;
            case 2:
              command.Parameters.AddWithValue("@valor2", valor2);
              command.CommandText = "SELECT * FROM accounts WHERE login=@valor AND password=@valor2 LIMIT 1";
              break;
          }
          NpgsqlDataReader npgsqlDataReader = command.ExecuteReader();
          while (npgsqlDataReader.Read())
          {
            acc = new Account();
            acc.login = npgsqlDataReader.GetString(0);
            acc.password = npgsqlDataReader.GetString(1);
            acc.SetPlayerId(npgsqlDataReader.GetInt64(2), searchFlag);
            acc.player_name = npgsqlDataReader.GetString(3);
            acc.name_color = npgsqlDataReader.GetInt32(4);
            acc.clan_id = npgsqlDataReader.GetInt32(5);
            acc._rank = npgsqlDataReader.GetInt32(6);
            acc._gp = npgsqlDataReader.GetInt32(7);
            acc._exp = npgsqlDataReader.GetInt32(8);
            acc.pc_cafe = npgsqlDataReader.GetInt32(9);
            acc._statistic.fights = npgsqlDataReader.GetInt32(10);
            acc._statistic.fights_win = npgsqlDataReader.GetInt32(11);
            acc._statistic.fights_lost = npgsqlDataReader.GetInt32(12);
            acc._statistic.kills_count = npgsqlDataReader.GetInt32(13);
            acc._statistic.deaths_count = npgsqlDataReader.GetInt32(14);
            acc._statistic.headshots_count = npgsqlDataReader.GetInt32(15);
            acc._statistic.escapes = npgsqlDataReader.GetInt32(16);
            acc.access = npgsqlDataReader.GetInt32(17);
            acc.LastRankUpDate = (uint) npgsqlDataReader.GetInt64(20);
            acc._money = npgsqlDataReader.GetInt32(21);
            acc._isOnline = npgsqlDataReader.GetBoolean(22);
            acc._equip._primary = npgsqlDataReader.GetInt32(23);
            acc._equip._secondary = npgsqlDataReader.GetInt32(24);
            acc._equip._melee = npgsqlDataReader.GetInt32(25);
            acc._equip._grenade = npgsqlDataReader.GetInt32(26);
            acc._equip._special = npgsqlDataReader.GetInt32(27);
            acc._equip._red = npgsqlDataReader.GetInt32(28);
            acc._equip._blue = npgsqlDataReader.GetInt32(29);
            acc._equip._helmet = npgsqlDataReader.GetInt32(30);
            acc._equip._dino = npgsqlDataReader.GetInt32(31);
            acc._equip._beret = npgsqlDataReader.GetInt32(32);
            acc.brooch = npgsqlDataReader.GetInt32(33);
            acc.insignia = npgsqlDataReader.GetInt32(34);
            acc.medal = npgsqlDataReader.GetInt32(35);
            acc.blue_order = npgsqlDataReader.GetInt32(36);
            acc._mission.mission1 = npgsqlDataReader.GetInt32(37);
            acc.clanAccess = npgsqlDataReader.GetInt32(38);
            acc.effects = (CupomEffects) npgsqlDataReader.GetInt64(40);
            acc._statistic.fights_draw = npgsqlDataReader.GetInt32(41);
            acc._mission.mission2 = npgsqlDataReader.GetInt32(42);
            acc._mission.mission3 = npgsqlDataReader.GetInt32(43);
            acc._statistic.totalkills_count = npgsqlDataReader.GetInt32(44);
            acc._statistic.totalfights_count = npgsqlDataReader.GetInt32(45);
            acc._status.SetData((uint) npgsqlDataReader.GetInt64(46), acc.player_id);
            acc.MacAddress = (PhysicalAddress) npgsqlDataReader.GetValue(50);
            acc.ban_obj_id = npgsqlDataReader.GetInt64(51);
            if (this.AddAccount(acc) && acc._isOnline)
              acc.setOnlineStatus(false);
          }
          command.Dispose();
          npgsqlDataReader.Dispose();
          npgsqlDataReader.Close();
          npgsqlConnection.Dispose();
          npgsqlConnection.Close();
        }
      }
      catch (Exception ex)
      {
        Logger.error("Ocorreu um problema ao carregar as contas4!\r\n" + ex.ToString());
      }
      return acc;
    }

    public void getFriendlyAccounts(FriendSystem system)
    {
      if (system == null)
        return;
      if (system._friends.Count == 0)
        return;
      try
      {
        using (NpgsqlConnection npgsqlConnection = SQLjec.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          npgsqlConnection.Open();
          List<string> stringList = new List<string>();
          for (int index = 0; index < system._friends.Count; ++index)
          {
            Friend friend = system._friends[index];
            string parameterName = "@valor" + index.ToString();
            command.Parameters.AddWithValue(parameterName, (object) friend.player_id);
            stringList.Add(parameterName);
          }
          string str = string.Join(",", stringList.ToArray());
          command.CommandText = "SELECT player_name,player_id,rank,online,status FROM accounts WHERE player_id in (" + str + ") ORDER BY player_id";
          NpgsqlDataReader npgsqlDataReader = command.ExecuteReader();
          while (npgsqlDataReader.Read())
          {
            Friend friend = system.GetFriend(npgsqlDataReader.GetInt64(1));
            if (friend != null)
            {
              friend.player.player_name = npgsqlDataReader.GetString(0);
              friend.player._rank = npgsqlDataReader.GetInt32(2);
              friend.player._isOnline = npgsqlDataReader.GetBoolean(3);
              friend.player._status.SetData((uint) npgsqlDataReader.GetInt64(4), friend.player_id);
              if (friend.player._isOnline && !this._contas.ContainsKey(friend.player_id))
              {
                friend.player.setOnlineStatus(false);
                friend.player._status.ResetData(friend.player_id);
              }
            }
          }
          command.Dispose();
          npgsqlDataReader.Dispose();
          npgsqlDataReader.Close();
          npgsqlConnection.Dispose();
          npgsqlConnection.Close();
        }
      }
      catch (Exception ex)
      {
        Logger.error("Ocorreu um problema ao carregar (FriendlyAccounts)!\r\n" + ex.ToString());
      }
    }

    public void getFriendlyAccounts(FriendSystem system, bool isOnline)
    {
      if (system == null)
        return;
      if (system._friends.Count == 0)
        return;
      try
      {
        using (NpgsqlConnection npgsqlConnection = SQLjec.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          List<string> stringList = new List<string>();
          for (int index = 0; index < system._friends.Count; ++index)
          {
            Friend friend = system._friends[index];
            if (friend.state > 0)
              return;
            string parameterName = "@valor" + index.ToString();
            command.Parameters.AddWithValue(parameterName, (object) friend.player_id);
            stringList.Add(parameterName);
          }
          string str = string.Join(",", stringList.ToArray());
          if (str == "")
            return;
          npgsqlConnection.Open();
          command.Parameters.AddWithValue("@on", (object) isOnline);
          command.CommandText = "SELECT player_name,player_id,rank,status FROM accounts WHERE player_id in (" + str + ") AND online=@on ORDER BY player_id";
          NpgsqlDataReader npgsqlDataReader = command.ExecuteReader();
          while (npgsqlDataReader.Read())
          {
            Friend friend = system.GetFriend(npgsqlDataReader.GetInt64(1));
            if (friend != null)
            {
              friend.player.player_name = npgsqlDataReader.GetString(0);
              friend.player._rank = npgsqlDataReader.GetInt32(2);
              friend.player._isOnline = isOnline;
              friend.player._status.SetData((uint) npgsqlDataReader.GetInt64(3), friend.player_id);
              if (isOnline && !this._contas.ContainsKey(friend.player_id))
              {
                friend.player.setOnlineStatus(false);
                friend.player._status.ResetData(friend.player_id);
              }
            }
          }
          command.Dispose();
          npgsqlDataReader.Dispose();
          npgsqlDataReader.Close();
          npgsqlConnection.Dispose();
          npgsqlConnection.Close();
        }
      }
      catch (Exception ex)
      {
        Logger.error("Ocorreu um problema ao carregar (FriendAccounts2)!\r\n" + ex.ToString());
      }
    }

    public static AccountManager getInstance() => AccountManager.acm;

    public Account getAccount(long id)
    {
      if (id == 0L)
        return (Account) null;
      try
      {
        Account account = (Account) null;
        return this._contas.TryGetValue(id, out account) ? account : this.getAccountDB((object) id, (object) null, 1, 0);
      }
      catch
      {
        return (Account) null;
      }
    }

    public Account getAccount(long id, bool noUseDB)
    {
      if (id == 0L)
        return (Account) null;
      try
      {
        Account account = (Account) null;
        return this._contas.TryGetValue(id, out account) ? account : (noUseDB ? (Account) null : this.getAccountDB((object) id, (object) null, 1, 0));
      }
      catch
      {
        return (Account) null;
      }
    }

    public bool CreateAccount(out Account p, string login, string password)
    {
      try
      {
        using (NpgsqlConnection npgsqlConnection = SQLjec.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          npgsqlConnection.Open();
          command.Parameters.AddWithValue("@login", (object) login);
          command.Parameters.AddWithValue("@pass", (object) password);
          command.CommandText = "INSERT INTO accounts (login, password) VALUES (@login,@pass)";
          command.ExecuteNonQuery();
          command.CommandText = "SELECT * FROM accounts WHERE login=@login";
          NpgsqlDataReader npgsqlDataReader = command.ExecuteReader();
          Account acc = new Account();
          while (npgsqlDataReader.Read())
          {
            acc.login = login;
            acc.password = password;
            acc.player_id = npgsqlDataReader.GetInt64(2);
            acc.SetPlayerId();
            acc.player_name = npgsqlDataReader.GetString(3);
            acc.name_color = npgsqlDataReader.GetInt32(4);
            acc.clan_id = npgsqlDataReader.GetInt32(5);
            acc._rank = npgsqlDataReader.GetInt32(6);
            acc._gp = npgsqlDataReader.GetInt32(7);
            acc._exp = npgsqlDataReader.GetInt32(8);
            acc.pc_cafe = npgsqlDataReader.GetInt32(9);
            acc._statistic.fights = npgsqlDataReader.GetInt32(10);
            acc._statistic.fights_win = npgsqlDataReader.GetInt32(11);
            acc._statistic.fights_lost = npgsqlDataReader.GetInt32(12);
            acc._statistic.kills_count = npgsqlDataReader.GetInt32(13);
            acc._statistic.deaths_count = npgsqlDataReader.GetInt32(14);
            acc._statistic.headshots_count = npgsqlDataReader.GetInt32(15);
            acc._statistic.escapes = npgsqlDataReader.GetInt32(16);
            acc.access = npgsqlDataReader.GetInt32(17);
            acc.LastRankUpDate = (uint) npgsqlDataReader.GetInt64(20);
            acc._money = npgsqlDataReader.GetInt32(21);
            acc._isOnline = npgsqlDataReader.GetBoolean(22);
            acc._equip._primary = npgsqlDataReader.GetInt32(23);
            acc._equip._secondary = npgsqlDataReader.GetInt32(24);
            acc._equip._melee = npgsqlDataReader.GetInt32(25);
            acc._equip._grenade = npgsqlDataReader.GetInt32(26);
            acc._equip._special = npgsqlDataReader.GetInt32(27);
            acc._equip._red = npgsqlDataReader.GetInt32(28);
            acc._equip._blue = npgsqlDataReader.GetInt32(29);
            acc._equip._helmet = npgsqlDataReader.GetInt32(30);
            acc._equip._dino = npgsqlDataReader.GetInt32(31);
            acc._equip._beret = npgsqlDataReader.GetInt32(32);
            acc.brooch = npgsqlDataReader.GetInt32(33);
            acc.insignia = npgsqlDataReader.GetInt32(34);
            acc.medal = npgsqlDataReader.GetInt32(35);
            acc.blue_order = npgsqlDataReader.GetInt32(36);
            acc._mission.mission1 = npgsqlDataReader.GetInt32(37);
            acc.clanAccess = npgsqlDataReader.GetInt32(38);
            acc.effects = (CupomEffects) npgsqlDataReader.GetInt64(40);
            acc._statistic.fights_draw = npgsqlDataReader.GetInt32(41);
            acc._mission.mission2 = npgsqlDataReader.GetInt32(42);
            acc._mission.mission3 = npgsqlDataReader.GetInt32(43);
            acc._statistic.totalkills_count = npgsqlDataReader.GetInt32(44);
            acc._statistic.totalfights_count = npgsqlDataReader.GetInt32(45);
            acc._status.SetData((uint) npgsqlDataReader.GetInt64(46), acc.player_id);
            acc.MacAddress = (PhysicalAddress) npgsqlDataReader.GetValue(50);
            acc.ban_obj_id = npgsqlDataReader.GetInt64(51);
          }
          p = acc;
          this.AddAccount(acc);
          command.Dispose();
          npgsqlConnection.Dispose();
          npgsqlConnection.Close();
          return true;
        }
      }
      catch (Exception ex)
      {
        Logger.warning("[AccountManager.CreateAccount] " + ex.ToString());
        p = (Account) null;
        return false;
      }
    }
  }
}
