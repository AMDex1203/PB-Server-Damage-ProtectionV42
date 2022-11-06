
// Type: Game.data.sync.client_side.Net_Clan_Sync
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;
using Game.data.managers;
using Game.data.model;

namespace Game.data.sync.client_side
{
  public static class Net_Clan_Sync
  {
    public static void Load(ReceiveGPacket p)
    {
      long id = p.readQ();
      int num1 = (int) p.readC();
      Account account = AccountManager.getAccount(id, true);
      if (account == null || num1 != 3)
        return;
      int num2 = p.readD();
      int num3 = (int) p.readC();
      account.clanId = num2;
      account.clanAccess = num3;
    }
  }
}
