
// Type: Battle.Program
// Assembly: pbserver_battle, Version=1.0.0.2646, Culture=neutral, PublicKeyToken=null
// MVID: AA2D7911-B687-4238-AB73-D37E54123039
// Interprise: C:\Users\Cuzin\3,50pbserver_battle.exe

using Battle.config;
using Battle.data.sync;
using Battle.data.xml;
using Battle.network;
using NetFwTypeLib;
using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading;
using Core.server;

namespace Battle
{
    internal class Program
    {
        public static int Counts;
        private static DateTime GetLinkerTime(Assembly assembly, TimeZoneInfo target = null)
        {
            string location = assembly.Location;
            byte[] buffer = new byte[2048];
            using (FileStream fileStream = new FileStream(location, FileMode.Open, FileAccess.Read))
                fileStream.Read(buffer, 0, 2048);
            int int32 = BitConverter.ToInt32(buffer, 60);
            return TimeZoneInfo.ConvertTimeFromUtc(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds((double)BitConverter.ToInt32(buffer, int32 + 8)), target ?? TimeZoneInfo.Local);
        }

        private static string getOSName()
        {
            OperatingSystem osVersion = Environment.OSVersion;
            string str = new ComputerInfo().OSFullName;
            if (osVersion.ServicePack != "")
                str = str + " " + osVersion.ServicePack;
            return str + " (" + (Environment.Is64BitOperatingSystem ? "64" : "32") + " bits)";
        }

        private static bool test(
          bool paramCheck,
          string serverDate,
          string[] args,
          bool item2,
          DateTime itemG)
        {
            try
            {
                TimeSpan timeSpan = DateTime.ParseExact("1990203220000", "yyMMddHHmmss", (IFormatProvider)CultureInfo.InvariantCulture) - itemG;
                return !(Program.GetPublicIP() == "");
            }
            catch
            {
                return false;
            }
        }

        private static string GetPublicIP()
        {
            try
            {
                using (StreamReader streamReader = new StreamReader(WebRequest.Create("http://checkip.dyndns.org").GetResponse().GetResponseStream()))
                    return streamReader.ReadToEnd().Trim().Split(':')[1].Substring(1).Split('<')[0];
            }
            catch
            {
                return "";
            }
        }

        private static void ApplicationThreadException(object sender, ThreadExceptionEventArgs e) => Program.ReportCrash(e.Exception);

        public static void ReportCrash(Exception exception, string developerMessage = "")
        {
        }

        private static void CurrentDomainOnUnhandledException(
          object sender,
          UnhandledExceptionEventArgs unhandledExceptionEventArgs)
        {
            Program.ReportCrash((Exception)unhandledExceptionEventArgs.ExceptionObject);
            Environment.Exit(0);
        }

        private static void SpeedTest()
        {
            List<int> BoomPlayers = new List<int>();
            BoomPlayers.Add(1);
            BoomPlayers.Add(3);
            for (int index = 0; index < 50; ++index)
            {
                Program.Speed1(BoomPlayers);
                Program.Speed2(BoomPlayers);
            }
            Stopwatch stopwatch1 = new Stopwatch();
            stopwatch1.Start();
            stopwatch1.Stop();
            for (int index = 0; index < 50; ++index)
            {
                Stopwatch stopwatch2 = new Stopwatch();
                stopwatch2.Start();
                Program.Speed1(BoomPlayers);
                stopwatch2.Stop();
                Stopwatch stopwatch3 = new Stopwatch();
                stopwatch3.Start();
                Program.Speed2(BoomPlayers);
                stopwatch3.Stop();
            }
        }

        private static void Speed1(List<int> BoomPlayers)
        {
            foreach (int boomPlayer in BoomPlayers)
                ;
        }

        private static void Speed2(List<int> BoomPlayers)
        {
            for (int index = 0; index < BoomPlayers.Count; ++index)
            {
                int boomPlayer = BoomPlayers[index];
            }
        }

        private static void NEW(float value)
        {
        }

        protected static void Main(string[] args)
        {

            //Logger.info("Success!");
            //Console.Clear();
            //Console.WriteLine("Usuario:");
            //if (Console.ReadLine() == "pretonaoemacaco")
             //   Console.WriteLine("Senha:");
            //if (!(Console.ReadLine() == "Surrender0127."))
               // return;
            Console.Clear();
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(Program.CurrentDomainOnUnhandledException);
            Config.Load();
            Logger.checkDirectory();
            Console.Title = "Batalha [RAM: " + (GC.GetTotalMemory(true) / 1024) + " KB]";
            Process ProcessProgram = Process.GetCurrentProcess();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(" =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("  Servidor Compilado No IP Atual : 10.0.0.4");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(" =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("   BATALHA GAMING             ");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(" -----------------------------------------------------");
            Console.ResetColor();
            Console.WriteLine("  Authors: Origin, Cabra, Evrst, Surrender");
            Console.WriteLine("  Parceiro: ROONI    ");
            Console.WriteLine("  Empresa: SwordProject    ");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(" -----------------------------------------------------");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("  ESSE É UM SOFTWARE PAGO, TODOS OS DIREITOS AO DESENVOLVEDOR <>  ");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(" =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("[Batalha] Tempo de Explosão da C4: (" + Config.plantDuration.ToString() + "s/" + Config.defuseDuration.ToString() + "s)");
            DateTime LiveDate = ComDiv.GetDate();
            if (LiveDate == new DateTime() || long.Parse(LiveDate.ToString("yyMMddHHmmss")) >= 221225220000)
                return;
            MappingXML.Load();
            CharaXML.Load();
            MeleeExceptionsXML.Load();
            ServersXML.Load();

            PointBlank.FirewallSecurity.LoadInstances(ProcessProgram.ProcessName, Config.SessionsBattle);
            for (int i = 0; i < Config.SessionsBattle; i++)
            {
                PointBlank.FirewallSecurity.CreateRuleAllow(PointBlank.FirewallSecurity.FirewallRuleNameBattleUDP[i], "127.0.0.1/255.255.255.255", Config.hosPort + i, NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_UDP); //Regra de permissão de acesso a todos endereços de ip na porta do Battle que passaram na autenticação e no Game.
            }

            if (Config.serverIp != "127.0.0.1")
            {
                Logger.info("ERROR");
                return;
            }

            Battle_SyncNet.Start();
            BattleManager.init();
            Process.GetCurrentProcess().WaitForExit();
        }
    }
}
