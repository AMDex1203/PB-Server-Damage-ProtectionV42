
// Type: Game.data.sync.client_side.Net_Clan_Servers_Sync
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.models.account.clan;
using Core.server;
using Game.data.managers;

namespace Game.data.sync.client_side
{
  public class Net_Clan_Servers_Sync
  {
    public static void Load(ReceiveGPacket p)
    {
      int num1 = (int) p.readC();
      int id = p.readD();
      Clan clan = ClanManager.getClan(id);
      if (num1 == 0)
      {
        if (clan != null)
          return;
        long num2 = p.readQ();
        int num3 = p.readD();
        string str1 = p.readS((int) p.readC());
        string str2 = p.readS((int) p.readC());
        ClanManager.AddClan(new Clan()
        {
          id = id,
          name = str1,
          ownerId = num2,
          logo = 0U,
          informations = str2,
          creationDate = num3
        });
      }
      else
      {
        if (clan == null)
          return;
        ClanManager.RemoveClan(clan);
      }
    }
  }
}
