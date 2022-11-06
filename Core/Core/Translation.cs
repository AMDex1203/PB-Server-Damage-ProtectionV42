
// Type: Core.Translation
// Assembly: Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 263E2FBF-1098-4552-8FB3-F6A660503737
// Interprise: C:\Users\Cuzin\3,50Core.dll

using System.Collections.Generic;
using System.IO;

namespace Core
{
  public static class Translation
  {
    private static SortedList<string, string> strings = new SortedList<string, string>();

    public static void Load()
    {
      foreach (string readAllLine in File.ReadAllLines("config/translate/strings.ini"))
      {
        int length = readAllLine.IndexOf("=");
        if (length >= 0)
        {
          string key = readAllLine.Substring(0, length);
          string str = readAllLine.Substring(length + 1);
          Translation.strings.Add(key, str);
        }
      }
    }

    public static string GetLabel(string title)
    {
      try
      {
        string str;
        return Translation.strings.TryGetValue(title, out str) ? str.Replace("\\n", '\n'.ToString()) : title;
      }
      catch
      {
        return title;
      }
    }

    public static string GetLabel(string title, params object[] args) => string.Format(Translation.GetLabel(title), args);
  }
}
