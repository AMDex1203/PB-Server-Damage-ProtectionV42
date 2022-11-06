
// Type: Game.global.clientpacket.FRIEND_DELETE_REC
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.managers;
using Core.models.account;
using Core.models.enums.friends;
using Core.server;
using Game.data.managers;
using Game.data.model;
using Game.data.sync.server_side;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
  public class FRIEND_DELETE_REC : ReceiveGamePacket
  {
    private int index;
    private uint erro;

    public FRIEND_DELETE_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read() => this.index = (int) this.readC();

    public override void run()
    {
      try
      {
        Account player = this._client._player;
        if (player == null)
          return;
        Friend friend1 = player.FriendSystem.GetFriend(this.index);
        if (friend1 != null)
        {
          PlayerManager.DeleteFriend(friend1.player_id, player.player_id);
          Account account = AccountManager.getAccount(friend1.player_id, 32);
          if (account != null)
          {
            int index = -1;
            Friend friend2 = account.FriendSystem.GetFriend(player.player_id, out index);
            if (friend2 != null)
            {
              friend2.removed = true;
              PlayerManager.UpdateFriendBlock(account.player_id, friend2);
              SEND_FRIENDS_INFOS.Load(account, friend2, 2);
              account.SendPacket((SendPacket) new FRIEND_UPDATE_PAK(FriendChangeState.Update, friend2, index), false);
            }
          }
          player.FriendSystem.RemoveFriend(friend1);
          this._client.SendPacket((SendPacket) new FRIEND_UPDATE_PAK(FriendChangeState.Delete, (Friend) null, 0, this.index));
        }
        else
          this.erro = 2147483648U;
        this._client.SendPacket((SendPacket) new FRIEND_REMOVE_PAK(this.erro));
        this._client.SendPacket((SendPacket) new FRIEND_MY_FRIENDLIST_PAK(player.FriendSystem._friends));
      }
      catch (Exception ex)
      {
        Logger.info("[FRIEND_DELETE_REC] " + ex.ToString());
      }
    }
  }
}
