
// Type: Auth.data.configs.ConfigMaps
// Assembly: pbserver_auth, Version=1.0.7796.38604, Culture=neutral, PublicKeyToken=null
// MVID: 1CFDFF97-782C-4EF8-A621-6E4FDDE32436
// Interprise: C:\Users\Cuzin\3,50pbserver_auth.exe

using Core;

namespace Auth.data.configs
{
  internal class ConfigMaps
  {
    public static int Chaos;
    public static int Tutorial;
    public static int Deathmatch;
    public static int Destruction;
    public static int Sabotage;
    public static int Supression;
    public static int Defense;
    public static int Challenge;
    public static int Dinosaur;
    public static int Sniper;
    public static int Shotgun;
    public static int HeadHunter;
    public static int Knuckle;
    public static int CrossCounter;
    public static int TheifMode;
    private static ConfigFile configFile;

    public static void Load()
    {
      try
      {
        ConfigMaps.configFile = new ConfigFile("data/maps/defaults.ini");
      }
      catch
      {
        Logger.error("Erro ao carregar as configurações dos mapas.");
        return;
      }
      ConfigMaps.Tutorial = ConfigMaps.configFile.readInt32("Tutorial", 0);
      ConfigMaps.Deathmatch = ConfigMaps.configFile.readInt32("Deathmatch", 1);
      ConfigMaps.Destruction = ConfigMaps.configFile.readInt32("Destruction", 25);
      ConfigMaps.Sabotage = ConfigMaps.configFile.readInt32("Sabotage", 35);
      ConfigMaps.Supression = ConfigMaps.configFile.readInt32("Supression", 11);
      ConfigMaps.Defense = ConfigMaps.configFile.readInt32("Defense", 39);
      ConfigMaps.Challenge = ConfigMaps.configFile.readInt32("Challenge", 1);
      ConfigMaps.Dinosaur = ConfigMaps.configFile.readInt32("Dinosaur", 40);
      ConfigMaps.Sniper = ConfigMaps.configFile.readInt32("Sniper", 1);
      ConfigMaps.Shotgun = ConfigMaps.configFile.readInt32("Shotgun", 1);
      ConfigMaps.HeadHunter = ConfigMaps.configFile.readInt32("HeadHunter", 0);
      ConfigMaps.Knuckle = ConfigMaps.configFile.readInt32("Knuckle", 0);
      ConfigMaps.CrossCounter = ConfigMaps.configFile.readInt32("Cross-Counter", 54);
      ConfigMaps.Chaos = ConfigMaps.configFile.readInt32("Chaos", 1);
      ConfigMaps.TheifMode = ConfigMaps.configFile.readInt32("TheifMode", 1);
    }
  }
}
