
// Type: Game.global.clientpacket.FRIEND_INVITE_REC
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.models.account;
using Core.models.enums.friends;
using Core.server;
using Game.data.managers;
using Game.data.model;
using Game.data.utils;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
  public class FRIEND_INVITE_REC : ReceiveGamePacket
  {
    private string playerName;
    private int idx1;
    private int idx2;

    public FRIEND_INVITE_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read() => this.playerName = this.readS(33);

    public override void run()
    {
      try
      {
        if (this._client == null)
          return;
        Account player = this._client._player;
        if (player == null || player.player_name.Length == 0 || player.player_name == this.playerName)
          this._client.SendPacket((SendPacket) new FRIEND_INVITE_PAK(2147487799U));
        else if (player.FriendSystem._friends.Count >= 50)
        {
          this._client.SendPacket((SendPacket) new FRIEND_INVITE_PAK(2147487800U));
        }
        else
        {
          Account account = AccountManager.getAccount(this.playerName, 1, 32);
          if (account != null)
          {
            if (player.FriendSystem.GetFriendIdx(account.player_id) == -1)
            {
              if (account.FriendSystem._friends.Count >= 50)
              {
                this._client.SendPacket((SendPacket) new FRIEND_INVITE_PAK(2147487800U));
              }
              else
              {
                int num = AllUtils.AddFriend(account, player, 2);
                if (AllUtils.AddFriend(player, account, num == 1 ? 0 : 1) == -1 || num == -1)
                {
                  this._client.SendPacket((SendPacket) new FRIEND_INVITE_PAK(2147487801U));
                }
                else
                {
                  Friend friend1 = player.FriendSystem.GetFriend(account.player_id, out this.idx1);
                  Friend friend2 = account.FriendSystem.GetFriend(player.player_id, out this.idx2);
                  if (friend2 != null)
                    account.SendPacket((SendPacket) new FRIEND_UPDATE_PAK(num == 0 ? FriendChangeState.Insert : FriendChangeState.Update, friend2, this.idx2), false);
                  if (friend1 == null)
                    return;
                  this._client.SendPacket((SendPacket) new FRIEND_UPDATE_PAK(FriendChangeState.Insert, friend1, this.idx1));
                }
              }
            }
            else
              this._client.SendPacket((SendPacket) new FRIEND_INVITE_PAK(2147487809U));
          }
          else
            this._client.SendPacket((SendPacket) new FRIEND_INVITE_PAK(2147487810U));
        }
      }
      catch (Exception ex)
      {
        Logger.info("[FRIEND_INVITE_REC] " + ex.ToString());
      }
    }
  }
}
