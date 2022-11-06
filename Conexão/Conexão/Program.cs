
// Type: Auth.Program
// Assembly: pbserver_auth, Version=1.0.7796.38604, Culture=neutral, PublicKeyToken=null
// MVID: 1CFDFF97-782C-4EF8-A621-6E4FDDE32436
// Interprise: C:\Users\Cuzin\3,50pbserver_auth.exe

using Auth.data.configs;
using Auth.data.sync;
using Core;
using NetFwTypeLib;
using Core.managers;
using Core.managers.events;
using Core.managers.server;
using Core.models.account.players;
using Core.server;
using Core.xml;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;

namespace Auth
{
    public class Program
    {

        private static void ApplicationThreadException(object sender, ThreadExceptionEventArgs e) => Program.ReportCrash(e.Exception);

        public static void ReportCrash(Exception exception, string developerMessage = "") => new CrashReporterDotNET.ReportCrash("muawayatuatu156@gmail.com")
        {
            DeveloperMessage = developerMessage
        }.Send(exception);

        private static void CurrentDomainOnUnhandledException(
          object sender,
          UnhandledExceptionEventArgs unhandledExceptionEventArgs)
        {
            Program.ReportCrash((Exception)unhandledExceptionEventArgs.ExceptionObject);
            Environment.Exit(0);
        }

        private static void v1(ItemsModel item) => new ItemsModel(item)._objId = item._objId;

        private static void v2(ItemsModel item) => item.DeepCopy();

        public static int Counts;

        private static void bb(out byte[] a1)
        {
            a1 = new byte[144];
            using (SendGPacket sendGpacket = new SendGPacket())
            {
                for (int index = 0; index < 16; ++index)
                {
                    sendGpacket.writeH(index * 2, (ushort)10);
                    sendGpacket.writeH(32 + index * 2, (ushort)20);
                    sendGpacket.writeH(64 + index * 2, (ushort)30);
                    sendGpacket.writeH(96 + index * 2, (ushort)40);
                    sendGpacket.writeC(128 + index, (byte)50);
                }
                sendGpacket.mstream.ToArray();
            }
        }

        private static void bb(
          out byte[] a1,
          out byte[] a2,
          out byte[] a3,
          out byte[] a4,
          out byte[] a5)
        {
            a1 = new byte[32];
            a2 = new byte[32];
            a3 = new byte[32];
            a4 = new byte[32];
            a5 = new byte[16];
            using (SendGPacket sendGpacket1 = new SendGPacket())
            {
                using (SendGPacket sendGpacket2 = new SendGPacket())
                {
                    using (SendGPacket sendGpacket3 = new SendGPacket())
                    {
                        using (SendGPacket sendGpacket4 = new SendGPacket())
                        {
                            using (SendGPacket sendGpacket5 = new SendGPacket())
                            {
                                for (int index = 0; index < 16; ++index)
                                {
                                    sendGpacket1.writeH((ushort)10);
                                    sendGpacket2.writeH((ushort)20);
                                    sendGpacket3.writeH((ushort)30);
                                    sendGpacket4.writeH((ushort)40);
                                    sendGpacket5.writeC((byte)50);
                                }
                                sendGpacket1.mstream.ToArray();
                                sendGpacket2.mstream.ToArray();
                                sendGpacket3.mstream.ToArray();
                                sendGpacket4.mstream.ToArray();
                                sendGpacket5.mstream.ToArray();
                            }
                        }
                    }
                }
            }
        }

        public static void Logo(string date)
        {
            Console.Clear();
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(Program.CurrentDomainOnUnhandledException);
            Console.Title = "Gerenciamento De Conexão";
            Process ProcessApplication = Process.GetCurrentProcess();
            Logger.StartedFor = "auth";
            Logger.checkDirectorys();
            Console.ForegroundColor = ConsoleColor.Blue;
            Logger.checkDirectorys();
            Console.WriteLine("  Servidor Compilado No IP Atual : 10.0.0.4");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(" =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("   SERVIDOR DE CONEXÃO             ");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(" -----------------------------------------------------");
            Console.ResetColor();
            Console.WriteLine("  Authors: Origin, Cabra, Evrst, Surrender");
            Console.WriteLine("  Parceiro: ROONI    ");
            Console.WriteLine("  Empresa: SwordProject    ");
            Console.WriteLine("  " + date + "       ");
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
        }

        private static void Main(string[] args)
        {
            Process ProcessApplication = Process.GetCurrentProcess();
            //Logger.info("Success!");
            //Console.Clear();
            //Console.WriteLine("Usuario:");
           // if (Console.ReadLine() == "pretonaoemacaco")
               // Console.WriteLine("Senha:");
            //if (!(Console.ReadLine() == "Surrender0127."))
               // return;
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(Program.CurrentDomainOnUnhandledException);
            Console.Title = "Gerenciamento De Conexão";
            Logger.StartedFor = "auth";
            Logger.checkDirectorys();
            Logger.info(new StringUtil().getString());
            ConfigGA.Load();
            ConfigMaps.Load();
            ServerConfigSyncer.GenerateConfig(ConfigGA.configId);
            string date = ComDiv.GetLinkerTime(Assembly.GetExecutingAssembly()).ToString("dd/MM/yyyy HH:mm");
            ComDiv.GetDate();
            Console.ForegroundColor = ConsoleColor.Blue;
            Program.Logo(date);
            DateTime LiveDate = ComDiv.GetDate();
            if (LiveDate == new DateTime() || long.Parse(LiveDate.ToString("yyMMddHHmmss")) >= 221225220000)
                return;
            EventLoader.LoadAll();
            DirectXML.Start();
            BasicInventoryXML.Load();
            ServersXML.Load();
            MissionCardXML.LoadBasicCards(2);
            MapsXML.Load();
            ShopManager.Load(2);
            CupomEffectManager.LoadCupomFlags();
            MissionsXML.Load();

            //PointBlank.FirewallAuth.LoadInstances(ProcessApplication.ProcessName, Core.config.Config.SessionsBattle);
            //PointBlank.FirewallAuth.CreateRuleAllow(PointBlank.FirewallAuth.FirewallRuleNameAuthTCP, "", ConfigGA.authPort, NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_TCP); //Regra de permissão de acesso a todos endereços de ip na porta do Auth.

            if (ConfigGA.authIp != "127.0.0.1")
            {
                Logger.info("ERROR - CHANGE IP");
                return;
            }

            ComDiv.GetDate();
            Auth_SyncNet.Start();
            bool flag = LoginManager.Start();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(Program.StartSuccess());
            if (flag)
                LoggerGA.updateRAM2();
            Process.GetCurrentProcess().WaitForExit();
        }

        private static string StartSuccess() => Logger.erro ? "[Conexão] Falha na inicialização." : "[Conexão] Servidor de Login iniciado com sucesso. (" + DateTime.Now.ToString("yy/MM/dd HH:mm:ss") + ")";
    }
}
