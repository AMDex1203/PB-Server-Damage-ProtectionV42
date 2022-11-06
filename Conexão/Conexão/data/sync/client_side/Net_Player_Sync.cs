
// Type: Auth.data.sync.client_side.Net_Player_Sync
// Assembly: pbserver_auth, Version=1.0.7796.38604, Culture=neutral, PublicKeyToken=null
// MVID: 1CFDFF97-782C-4EF8-A621-6E4FDDE32436
// Interprise: C:\Users\Cuzin\3,50pbserver_auth.exe

using Auth.data.managers;
using Auth.data.model;
using Core.server;

namespace Auth.data.sync.client_side
{
  public static class Net_Player_Sync
  {
    public static void Load(ReceiveGPacket p)
    {
      long id = p.readQ();
      int num1 = (int) p.readC();
      int num2 = (int) p.readC();
      int num3 = p.readD();
      int num4 = p.readD();
      Account account = AccountManager.getInstance().getAccount(id, true);
      if (account == null || num1 != 0)
        return;
      account._rank = num2;
      account._gp = num3;
      account._money = num4;
    }
  }
}
