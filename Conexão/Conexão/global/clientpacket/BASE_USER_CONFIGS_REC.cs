
// Type: Auth.global.clientpacket.BASE_USER_CONFIGS_REC
// Assembly: pbserver_auth, Version=1.0.7796.38604, Culture=neutral, PublicKeyToken=null
// MVID: 1CFDFF97-782C-4EF8-A621-6E4FDDE32436
// Interprise: C:\Users\Cuzin\3,50pbserver_auth.exe

using Auth.data.model;
using Auth.global.serverpacket;
using Core;
using Core.managers;
using Core.models.account;
using Core.server;
using System;
using System.Collections.Generic;

namespace Auth.global.clientpacket
{
  public class BASE_USER_CONFIGS_REC : ReceiveLoginPacket
  {
    public BASE_USER_CONFIGS_REC(LoginClient client, byte[] data) => this.makeme(client, data);

    public override void read()
    {
    }

    public override void run()
    {
      try
      {
        Account player = this._client._player;
        if (player == null || player._myConfigsLoaded)
          return;
        this._client.SendPacket((SendPacket) new BASE_EXIT_URL_PAK(LoginManager.Config.ExitURL));
        if (player.FriendSystem._friends.Count > 0)
          this._client.SendPacket((SendPacket) new BASE_USER_FRIENDS_PAK(player.FriendSystem._friends));
        this.SendMessagesList(player);
        this._client.SendPacket((SendPacket) new BASE_USER_CONFIG_PAK(0, player._config));
      }
      catch (Exception ex)
      {
        Logger.warning("[BASE_USER_CONFIGS_REC] " + ex.ToString());
      }
    }

    private void SendMessagesList(Account p)
    {
      List<Message> messages = MessageManager.getMessages(p.player_id);
      if (messages.Count == 0)
        return;
      MessageManager.RecicleMessages(p.player_id, messages);
      if (messages.Count == 0)
        return;
      int num = (int) Math.Ceiling((double) messages.Count / 25.0);
      for (int pageIdx = 0; pageIdx < num; ++pageIdx)
        this._client.SendPacket((SendPacket) new BASE_USER_MESSAGES_PAK(pageIdx, messages));
    }
  }
}
