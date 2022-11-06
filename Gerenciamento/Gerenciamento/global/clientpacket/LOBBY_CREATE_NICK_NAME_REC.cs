
// Type: Game.global.clientpacket.LOBBY_CREATE_NICK_NAME_REC
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.filters;
using Core.managers;
using Core.models.account.players;
using Core.server;
using Core.xml;
using Game.data.managers;
using Game.data.model;
using Game.global.serverpacket;
using System;
using System.Collections.Generic;

namespace Game.global.clientpacket
{
  public class LOBBY_CREATE_NICK_NAME_REC : ReceiveGamePacket
  {
    private string name;

    public LOBBY_CREATE_NICK_NAME_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read() => this.name = this.readS((int) this.readC());

    public override void run()
    {
      try
      {
        Account player = this._client._player;
        if (player == null || player.player_name.Length > 0 || (string.IsNullOrEmpty(this.name) || this.name.Length < ConfigGS.minNickSize) || this.name.Length > ConfigGS.maxNickSize)
        {
          this._client.SendPacket((SendPacket) new LOBBY_CREATE_NICK_NAME_PAK(2147487763U));
        }
        else
        {
          foreach (string str in NickFilter._filter)
          {
            if (this.name.Contains(str))
            {
              this._client.SendPacket((SendPacket) new LOBBY_CREATE_NICK_NAME_PAK(2147487763U));
              return;
            }
          }
          if (!PlayerManager.isPlayerNameExist(this.name))
          {
            if (AccountManager.updatePlayerName(this.name, player.player_id))
            {
              NickHistoryManager.CreateHistory(player.player_id, player.player_name, this.name, "First nick");
              player.player_name = this.name;
              List<ItemsModel> creationAwards = BasicInventoryXML.creationAwards;
              if (creationAwards.Count > 0)
              {
                this._client.SendPacket((SendPacket) new INVENTORY_ITEM_CREATE_PAK(1, player, creationAwards));
                this._client.SendPacket((SendPacket) new SERVER_MESSAGE_ITEM_RECEIVE_PAK(0U));
              }
              this._client.SendPacket((SendPacket) new LOBBY_CREATE_NICK_NAME_PAK(0U));
              this._client.SendPacket((SendPacket) new BASE_QUEST_GET_INFO_PAK(player));
            }
            else
              this._client.SendPacket((SendPacket) new LOBBY_CREATE_NICK_NAME_PAK(2147487763U));
          }
          else
            this._client.SendPacket((SendPacket) new LOBBY_CREATE_NICK_NAME_PAK(2147483923U));
        }
      }
      catch (Exception ex)
      {
        Logger.warning("[LOBBY_CREATE_NICK_NAME_REC] " + ex.ToString());
      }
    }
  }
}
