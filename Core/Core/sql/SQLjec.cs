
// Type: Core.sql.SQLjec
// Assembly: Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 263E2FBF-1098-4552-8FB3-F6A660503737
// Interprise: C:\Users\Cuzin\3,50Core.dll

using Npgsql;
using System.Runtime.Remoting.Contexts;

namespace Core.sql
{
  [Synchronization]
  public class SQLjec
  {
    private static SQLjec sql = new SQLjec();
    protected NpgsqlConnectionStringBuilder connBuilder;

    public SQLjec() => this.connBuilder = new NpgsqlConnectionStringBuilder()
    {
      Database = ConfigGB.dbName,
      Host = ConfigGB.dbHost,
      Username = ConfigGB.dbUser,
      Password = ConfigGB.dbPass,
      Port = ConfigGB.dbPort
    };

    public static SQLjec getInstance() => SQLjec.sql;

    public NpgsqlConnection conn() => new NpgsqlConnection(this.connBuilder.ConnectionString);
  }
}
