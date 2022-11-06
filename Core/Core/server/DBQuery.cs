
// Type: Core.server.DBQuery
// Assembly: Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 263E2FBF-1098-4552-8FB3-F6A660503737
// Interprise: C:\Users\Cuzin\3,50Core.dll

using System.Collections.Generic;

namespace Core.server
{
  public class DBQuery
  {
    private List<string> tables;
    private List<object> values;

    public DBQuery()
    {
      this.tables = new List<string>();
      this.values = new List<object>();
    }

    public void AddQuery(string table, object value)
    {
      this.tables.Add(table);
      this.values.Add(value);
    }

    public string[] GetTables() => this.tables.ToArray();

    public object[] GetValues() => this.values.ToArray();
  }
}
