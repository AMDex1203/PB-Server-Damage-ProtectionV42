
// Type: Game.global.clientpacket.FRIEND_ACCEPT_REC
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
  public class FRIEND_ACCEPT_REC : ReceiveGamePacket
  {
    private int index;
    private uint erro;

    public FRIEND_ACCEPT_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read() => this.index = (int) this.readC();

    public override void run()
    {
      try
      {
        Account player = this._client._player;
        if (player == null)
          return;
        Friend friend1 = player.FriendSystem.GetFriend(this.index);
        if (friend1 != null && friend1.state > 0)
        {
          Account account = AccountManager.getAccount(friend1.player_id, 32);
          if (account != null)
          {
            if (friend1.player == null)
              friend1.SetModel(account.player_id, account._rank, account.player_name, account._isOnline, account._status);
            else
              friend1.player.SetInfo(account._rank, account.player_name, account._isOnline, account._status);
            friend1.state = 0;
            PlayerManager.UpdateFriendState(player.player_id, friend1);
            this._client.SendPacket((SendPacket) new FRIEND_UPDATE_PAK(FriendChangeState.Accept, (Friend) null, 0, this.index));
            this._client.SendPacket((SendPacket) new FRIEND_UPDATE_PAK(FriendChangeState.Update, friend1, this.index));
            int index = -1;
            Friend friend2 = account.FriendSystem.GetFriend(player.player_id, out index);
            if (friend2 != null && friend2.state > 0)
            {
              if (friend2.player == null)
                friend2.SetModel(player.player_id, player._rank, player.player_name, player._isOnline, player._status);
              else
                friend2.player.SetInfo(player._rank, player.player_name, player._isOnline, player._status);
              friend2.state = 0;
              PlayerManager.UpdateFriendState(account.player_id, friend2);
              SEND_FRIENDS_INFOS.Load(account, friend2, 1);
              account.SendPacket((SendPacket) new FRIEND_UPDATE_PAK(FriendChangeState.Update, friend2, index), false);
            }
          }
          else
            this.erro = 2147483648U;
        }
        else
          this.erro = 2147483648U;
        if (this.erro <= 0U)
          return;
        this._client.SendPacket((SendPacket) new FRIEND_ACCEPT_PAK(this.erro));
      }
      catch (Exception ex)
      {
        Logger.info("[FRIEND_ACCEPT_REC] " + ex.ToString());
      }
    }
  }
}
