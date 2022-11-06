
// Type: Auth.ConfigGA
// Assembly: pbserver_auth, Version=1.0.7796.38604, Culture=neutral, PublicKeyToken=null
// MVID: 1CFDFF97-782C-4EF8-A621-6E4FDDE32436
// Interprise: C:\Users\Cuzin\3,50pbserver_auth.exe

using Core;
using Core.models.enums.global;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth
{
    public static class ConfigGA
    {
        public static string authIp;
        public static bool isTestMode;
        public static bool Outpost;
        public static bool AUTO_ACCOUNTS;
        public static bool debugMode;
        public static int syncPort;
        public static int configId;
        public static int maxNickSize;
        public static int minNickSize;
        public static int minLoginSize;
        public static int minPassSize;
        public static int authPort;
        public static ulong LauncherKey;
        public static int LimitAccountIp;
        public static string ClientVersion;
        public static int LoginType;
        public static int MacAddressLimit;
        public static List<ClientLocale> GameLocales;
        public static ClientLocale LocalGame;

        public static void Load()
        {
            ConfigFile configFile = new ConfigFile("config/Conexão.ini");
            ConfigGB.dbHost = configFile.readString("dbhost", "localhost");
            ConfigGB.dbName = configFile.readString("dbname", "");
            ConfigGB.dbUser = configFile.readString("dbuser", "root");
            ConfigGB.dbPass = configFile.readString("dbpass", "");
            ConfigGB.dbPort = configFile.readInt32("dbport", 0);
            ConfigGA.configId = configFile.readInt32("configId", 0);
            ConfigGA.authIp = configFile.readString("authIp", "127.0.0.1");
            ConfigGA.authPort = configFile.readInt32("authPort", 39190);
            ConfigGA.syncPort = configFile.readInt32("syncPort", 0);
            ConfigGA.ClientVersion = configFile.readString("ClientVersion", "1.15.37");
            ConfigGA.AUTO_ACCOUNTS = configFile.readBoolean("autoaccounts", false);
            ConfigGA.LoginType = configFile.readInt32("LoginType", 0);
            ConfigGA.debugMode = configFile.readBoolean("debugMode", true);
            ConfigGA.isTestMode = configFile.readBoolean("isTestMode", true);
            ConfigGB.ClientVersion = configFile.readString("ClientVersion", "");
            ConfigGB.EncodeText = Encoding.GetEncoding(configFile.readInt32("EncodingPage", 0));
            ConfigGA.Outpost = configFile.readBoolean("Outpost", false);
            ConfigGA.LauncherKey = configFile.readUInt64("LauncherKey", 0UL);
            ConfigGA.LimitAccountIp = configFile.readInt32("LimitAccountIp", 0);
            ConfigGA.MacAddressLimit = configFile.readInt32("MacAddressLimit", 0);
            ConfigGA.minNickSize = configFile.readInt32("minNickSize", 0);
            ConfigGA.maxNickSize = configFile.readInt32("maxNickSize", 0);
            ConfigGA.minLoginSize = configFile.readInt32("minLoginSize", 0);
            ConfigGA.minPassSize = configFile.readInt32("minPassSize", 0);
            ConfigGA.GameLocales = new List<ClientLocale>();
            string str1 = configFile.readString("GameLocales", "None");
            char[] chArray = new char[1] { ',' };
            foreach (string str2 in str1.Split(chArray))
            {
                ClientLocale result;
                Enum.TryParse<ClientLocale>(str2, out result);
                ConfigGA.GameLocales.Add(result);
            }
        }

        public static void CheckUserFile()
        {
            //Permite a entrada do usuario caso a user file list for a correta.
            string userfile = "5CA56B1E483FF9E24F62BE21C6BA6670";
            if (userfile == "5CA56B1E483FF9E24F62BE21C6BA6670")
            {
                userfile = "IS VALID";
            }
            AUTO_ACCOUNTS = false;
        }
    }
}
