
// Type: Core.Logger
// Assembly: Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 263E2FBF-1098-4552-8FB3-F6A660503737
// Interprise: C:\Users\Cuzin\3,50Core.dll

using System;
using System.IO;

namespace Core
{
  public static class Logger
  {
    private static string Date;
    public static string StartedFor;
    private static object Sync;
    public static bool erro;
    public static DateTime LastSaveLogTcpAuth1;
    public static DateTime LastSaveLogTcpAuth2;
    public static DateTime LastSaveLogTcpGame1;
    public static DateTime LastSaveLogTcpGame2;
    public static DateTime LastSaveLogUdpBattle1;
    public static DateTime LastSaveLogUdpBattle2;
    public static DateTime LastSaveLogUdpBattle3;
    public static bool Problem = false;
    private static StreamWriter SW_LOGIN;
    private static StreamWriter SW_WANING;
    private static StreamWriter SW_ERROR;
    private static StreamWriter SW_EXCEPTION;
    private static StreamWriter SW_ANALYZE;
    private static StreamWriter SW_ROOM;
    private static StreamWriter SW_CHAT_COMMANDS;
    private static StreamWriter SW_CHAT_ALL;
    private static StreamWriter SW_DEBUG;
    private static StreamWriter SW_BATTLE;
    private static StreamWriter SW_PING;
    private static StreamWriter SW_PACKETS;
    private static StreamWriter SW_ATTACKS;

    static Logger()
    {
      Logger.Date = DateTime.Now.ToString("yyyy-MM-dd--HH-mm-ss");
      Logger.StartedFor = "None";
      Logger.Sync = new object();
    }

    public static void checkDirectorys()
    {
      try
      {
        if (Logger.StartedFor != "auth")
        {
          if (!Directory.Exists("logs/cmd"))
            Directory.CreateDirectory("logs/cmd");
          if (!Directory.Exists("logs/game"))
            Directory.CreateDirectory("logs/game");
          if (!Directory.Exists("logs/rooms"))
            Directory.CreateDirectory("logs/rooms");
          if (!Directory.Exists("logs/error"))
            Directory.CreateDirectory("logs/error");
          if (Directory.Exists("logs/hack"))
            return;
          Directory.CreateDirectory("logs/hack");
        }
        else
        {
          if (!Directory.Exists("logs/auth"))
            Directory.CreateDirectory("logs/auth");
          if (!Directory.Exists("logs/login"))
            Directory.CreateDirectory("logs/login");
        }
      }
      catch (System.Exception ex)
      {
        Console.WriteLine(ex.ToString());
      }
    }

    public static void error(string text)
    {
      Logger.erro = true;
      Logger.write(text, ConsoleColor.Red);
    }

    public static void info(string text) => Logger.write(text, ConsoleColor.Gray);

    public static void LogCMD(string text)
    {
      try
      {
        Logger.save(text, "cmd");
      }
      catch
      {
      }
    }

    public static void Informations(string text)
    {
      if (!ConfigGB.LogInitialize)
        return;
      Console.ForegroundColor = ConsoleColor.DarkYellow;
      Console.WriteLine(text);
      Console.ResetColor();
    }

    public static void Exception(System.Exception ex)
    {
      try
      {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(ex.ToString());
        Console.ResetColor();
        if (!ConfigGB.SaveLogs)
          return;
        Logger.SW_EXCEPTION.WriteLine(string.Format("[{0}] {1}", (object) DateTime.Now, (object) ex));
        Logger.SW_EXCEPTION.Flush();
      }
      catch
      {
      }
    }

    public static void LogHack(string text)
    {
      try
      {
        Logger.save("[" + DateTime.Now.ToString("yy/MM/dd HH:mm:ss") + "]: " + text, "hack");
      }
      catch
      {
      }
    }

    public static void LogLogin(string text)
    {
      try
      {
        Logger.save("[Date: " + DateTime.Now.ToString("dd/MM/yy HH:mm") + "] Motive: " + text, "login");
      }
      catch
      {
      }
    }

    public static void LogProblems(string text, string problemInfo)
    {
      try
      {
        Logger.save("[Data: " + DateTime.Now.ToString("yy/MM/dd HH:mm:ss") + "]" + text, problemInfo);
      }
      catch
      {
      }
    }

    public static void LogRoom(string text, uint startDate, uint uniqueRoomId)
    {
      try
      {
        Logger.save(text, startDate, (ulong) uniqueRoomId);
      }
      catch
      {
      }
    }

    public static void save(string text, uint dateTime, ulong roomId)
    {
      using (FileStream fileStream = new FileStream("logs/rooms/" + dateTime.ToString() + "-" + roomId.ToString() + ".log", FileMode.Append))
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

    public static void save(string text, string type)
    {
      using (FileStream fileStream = new FileStream("logs/" + type + "/" + Logger.Date + ".log", FileMode.Append))
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

    public static void warning(string text) => Logger.write(text, ConsoleColor.Yellow);

    public static void write(string text, ConsoleColor color)
    {
      try
      {
        lock (Logger.Sync)
        {
          Console.ForegroundColor = color;
          Console.WriteLine(text);
          Logger.save(text, Logger.StartedFor);
        }
      }
      catch
      {
      }
    }
  }
}
