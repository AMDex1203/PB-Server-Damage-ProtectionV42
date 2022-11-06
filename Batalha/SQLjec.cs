
// Type: Battle.SQLjec
// Assembly: pbserver_battle, Version=1.0.0.2646, Culture=neutral, PublicKeyToken=null
// MVID: AA2D7911-B687-4238-AB73-D37E54123039
// Interprise: C:\Users\Cuzin\3,50pbserver_battle.exe

using Battle.config;
using Npgsql;
using System.Runtime.Remoting.Contexts;

namespace Battle
{
  [Synchronization]
  public class SQLjec
  {
    private static SQLjec sql = new SQLjec();
    protected NpgsqlConnectionStringBuilder connBuilder;

    public SQLjec() => this.connBuilder = new NpgsqlConnectionStringBuilder()
    {
      Database = Config.dbName,
      Host = Config.dbHost,
      Username = Config.dbUser,
      Password = Config.dbPass,
      Port = Config.dbPort
    };

    public static SQLjec getInstance() => SQLjec.sql;

    public NpgsqlConnection conn() => new NpgsqlConnection(this.connBuilder.ConnectionString);
  }
}
