
// Type: Battle.Logger
// Assembly: pbserver_battle, Version=1.0.0.2646, Culture=neutral, PublicKeyToken=null
// MVID: AA2D7911-B687-4238-AB73-D37E54123039
// Interprise: C:\Users\Cuzin\3,50pbserver_battle.exe

using System;
using System.IO;

namespace Battle
{
  public static class Logger
  {
    private static string name = "logs/battle/" + DateTime.Now.ToString("yyyy-MM-dd--HH-mm-ss") + ".log";
    private static object Sync = new object();

    private static void write(string text, ConsoleColor color)
    {
      try
      {
        lock (Logger.Sync)
        {
          Console.ForegroundColor = color;
          Console.WriteLine(text);
          Logger.save(text);
        }
      }
      catch
      {
      }
    }

    public static void error(string text) => Logger.write(text, ConsoleColor.Red);

    public static void warning(string text) => Logger.write(text, ConsoleColor.Yellow);

    public static void info(string text) => Logger.write(text, ConsoleColor.Gray);

    private static void save(string text)
    {
      using (FileStream fileStream = new FileStream(Logger.name, FileMode.Append))
      {
        using (StreamWriter streamWriter = new StreamWriter((Stream) fileStream))
        {
          try
          {
            streamWriter?.WriteLine(text);
          }
          catch
          {
          }
          streamWriter.Flush();
          streamWriter.Close();
          fileStream.Flush();
          fileStream.Close();
        }
      }
    }

    public static void checkDirectory()
    {
      if (Directory.Exists("logs/battle"))
        return;
      Directory.CreateDirectory("logs/battle");
    }
  }
}
