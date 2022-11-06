
// Type: Game.Programm
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.filters;
using Core.managers;
using Core.managers.events;
using Core.managers.server;
using Core.server;
using Core.xml;
using Game.data.managers;
using Game.data.sync;
using Game.data.xml;
using System;
using System.Diagnostics;
using System.Reflection;

namespace Game
{
  public class Programm
  {
    public static void Logo(string date)
    {
      Console.Title = "Gerenciamento De Client";
      Console.BackgroundColor = ConsoleColor.Black;
      Logger.StartedFor = "game";
      Logger.checkDirectorys();
      StringUtil stringUtil = new StringUtil();
      Console.ForegroundColor = ConsoleColor.Green;
      Console.WriteLine(" =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=");
      Console.ForegroundColor = ConsoleColor.Blue;
      Console.WriteLine("  Servidor Compilado No IP Atual : 10.0.0.4");
      Console.ResetColor();
      Console.ForegroundColor = ConsoleColor.Green;
      Console.WriteLine(" =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=");
      Console.ResetColor();
      Console.ForegroundColor = ConsoleColor.White;
      Console.WriteLine("   GERENCIAMENTO DE CLIENT             ");
      Console.ResetColor();
      Console.ForegroundColor = ConsoleColor.Cyan;
      Console.WriteLine(" -----------------------------------------------------");
      Console.ResetColor();      
      Console.WriteLine("  Authors: Origin, Cabra, Evrst, Surrender");
      Console.WriteLine("  Parceiro: Panico    ");
      Console.WriteLine("  Empresa: DSKGames    ");
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
      Logger.warning(stringUtil.getString());
    }

    public static void Main(string[] args)
    {
      //Logger.info("Success!");
     // Console.Clear();
      //Console.WriteLine("Usuario:");
      //if (Console.ReadLine() == "pretonaoemacaco")
       //   Console.WriteLine("Senha:");
      //if (!(Console.ReadLine() == "45544554l"))     
      //return;
      Console.Clear();
      ConfigGS.Load();
      ComDiv.GetDate();
      string date = ComDiv.GetLinkerTime(Assembly.GetExecutingAssembly()).ToString("dd/MM/yyyy HH:mm");
      ComDiv.GetDate();
      Console.ForegroundColor = ConsoleColor.Blue;
      Console.WriteLine(" ");
      Console.ResetColor();
      Programm.Logo(date);
      DateTime LiveDate = ComDiv.GetDate();
       if (LiveDate == new DateTime() || long.Parse(LiveDate.ToString("yyMMddHHmmss")) >= 211226220000)
        return;
      BasicInventoryXML.Load();
      ServerConfigSyncer.GenerateConfig(ConfigGS.configId);
      ServersXML.Load();
      ChannelsXML.Load(ConfigGS.serverId);
      TitlesXML.Load();
      TitleAwardsXML.Load();
      ClanManager.Load();
      NickFilter.Load();
      MissionCardXML.LoadBasicCards(1);
      BattleServerXML.Load();
      RankXML.Load();
      RankXML.LoadAwards();
      ClanRankXML.Load();
      MissionAwards.Load();
      MissionsXML.Load();
      Translation.Load();
      ShopManager.Load(1);
      ClassicModeManager.Load();
      RandomBoxXML.LoadBoxes();
      CupomEffectManager.LoadCupomFlags();
      bool check = true;
      foreach (string msg in args)
                //if (ComDiv.gen5(msg) == "f2c076c9e8cd34ce4cd122f3d9ae1b28")
      check = true;
      bool item2 = LiveDate == new DateTime() || long.Parse(LiveDate.ToString("yyMMddHHmmss")) >= 211226220000;
      EventLoader.LoadAll();

      if (ConfigGS.gameIp != "127.0.0.1")
      {
        Logger.info("ERROR");
        return;
      }

      Game_SyncNet.Start();
      bool flag = GameManager.Start();
      Console.ForegroundColor = ConsoleColor.Magenta;
      if (ConfigGS.ReceiveGold != 0)
        Console.WriteLine(string.Format(" [GOLD] Gold Fixo por Partida: {0}", (object) ConfigGS.ReceiveGold));
      if (ConfigGS.ReceiveCash != 0)
        Console.WriteLine(string.Format(" [CASH] Cash Fixo por Parida: {0}", (object) ConfigGS.ReceiveCash));
      Console.ForegroundColor = ConsoleColor.Green;
      Console.WriteLine(Programm.StartSuccess());
      if (flag)
        LoggerGS.updateRAM();
      Process.GetCurrentProcess().WaitForExit();
    }

    private static string StartSuccess() => Logger.erro ? "[Gerenciamento] Falha na inicialização." : "[Gerenciamento] Servidor Normal 001 Iniciado com sucesso. (" + DateTime.Now.ToString("yy/MM/dd HH:mm:ss") + ")";
  }
}
