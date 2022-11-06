
// Type: Auth.data.sync.client_side.Net_Clan_Sync
// Assembly: pbserver_auth, Version=1.0.7796.38604, Culture=neutral, PublicKeyToken=null
// MVID: 1CFDFF97-782C-4EF8-A621-6E4FDDE32436
// Interprise: C:\Users\Cuzin\3,50pbserver_auth.exe

using Auth.data.managers;
using Auth.data.model;
using Auth.data.sync.update;
using Core.server;

namespace Auth.data.sync.client_side
{
  public static class Net_Clan_Sync
  {
    public static void Load(ReceiveGPacket p)
    {
      long id1 = p.readQ();
      int num1 = (int) p.readC();
      Account account = AccountManager.getInstance().getAccount(id1, true);
      if (account == null)
        return;
      switch (num1)
      {
        case 0:
          ClanInfo.ClearList(account);
          break;
        case 1:
          long pId = p.readQ();
          string str = p.readS((int) p.readC());
          byte[] buffer = p.readB(4);
          byte num2 = p.readC();
          Account member = new Account()
          {
            player_id = pId,
            player_name = str,
            _rank = (int) num2
          };
          member._status.SetData(buffer, pId);
          ClanInfo.AddMember(account, member);
          break;
        case 2:
          long id2 = p.readQ();
          ClanInfo.RemoveMember(account, id2);
          break;
        case 3:
          int num3 = p.readD();
          int num4 = (int) p.readC();
          account.clan_id = num3;
          account.clanAccess = num4;
          break;
      }
    }
  }
}
