
// Type: Game.LoggerGS
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Game.data.managers;
using System;
using System.Threading.Tasks;

namespace Game
{
  public static class LoggerGS
  {
    public static int TestSlot = 1;

    public static async void updateRAM()
    {
      while (true)
      {
        Console.Title = "Gerenciamento De Client [Servidores: " + ConfigGS.serverId.ToString() + "; Usuários: " + GameManager._socketList.Count.ToString() + "; Contas Carregadas: " + AccountManager._contas.Count.ToString() + "; RAM: " + (GC.GetTotalMemory(true) / 1024L).ToString() + " KB]";
        await Task.Delay(1000);
      }
    }
  }
}
