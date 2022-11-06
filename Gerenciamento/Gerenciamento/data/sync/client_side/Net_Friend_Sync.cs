
// Type: Game.data.sync.client_side.Net_Friend_Sync
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.models.account;
using Core.server;
using Game.data.managers;
using Game.data.model;

namespace Game.data.sync.client_side
{
  public class Net_Friend_Sync
  {
    public static void Load(ReceiveGPacket p)
    {
      long id = p.readQ();
      int num1 = (int) p.readC();
      long num2 = p.readQ();
      Friend friend = (Friend) null;
      if (num1 <= 1)
      {
        int num3 = (int) p.readC();
        bool flag = p.readC() == (byte) 1;
        friend = new Friend(num2)
        {
          state = num3,
          removed = flag
        };
      }
      if (friend == null && num1 <= 1)
        return;
      Account account = AccountManager.getAccount(id, true);
      if (account == null)
        return;
      if (num1 <= 1)
      {
        friend.player.player_name = account.player_name;
        friend.player._rank = account._rank;
        friend.player._isOnline = account._isOnline;
        friend.player._status = account._status;
      }
      if (num1 == 0)
        account.FriendSystem.AddFriend(friend);
      else if (num1 == 1)
      {
        if (account.FriendSystem.GetFriend(num2) == null)
          ;
      }
      else
      {
        if (num1 != 2)
          return;
        account.FriendSystem.RemoveFriend(num2);
      }
    }
  }
}
