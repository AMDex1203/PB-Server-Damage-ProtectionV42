
// Type: Core.filters.NickFilter
// Assembly: Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 263E2FBF-1098-4552-8FB3-F6A660503737
// Interprise: C:\Users\Cuzin\3,50Core.dll

using System;
using System.Collections.Generic;
using System.IO;

namespace Core.filters
{
  public static class NickFilter
  {
    public static List<string> _filter = new List<string>();

    public static void Load()
    {
      if (File.Exists("data/filters/nicks.txt"))
      {
        try
        {
          using (StreamReader streamReader = new StreamReader("data/filters/nicks.txt"))
          {
            string str;
            while ((str = streamReader.ReadLine()) != null)
              NickFilter._filter.Add(str);
            streamReader.Close();
          }
        }
        catch (Exception ex)
        {
          Logger.error("[NickFilter] " + ex.ToString());
        }
      }
      else
        Logger.warning("[Aviso]: O arquivo 1 de filtros não existe.");
    }
  }
}
