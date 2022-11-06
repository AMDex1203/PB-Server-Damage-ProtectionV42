
// Type: Game.global.clientpacket.BASE_TITLE_GET_REC
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.managers;
using Core.models.account.players;
using Core.models.account.title;
using Core.server;
using Core.xml;
using Game.data.model;
using Game.global.serverpacket;
using System;
using System.Collections.Generic;

namespace Game.global.clientpacket
{
  public class BASE_TITLE_GET_REC : ReceiveGamePacket
  {
    private int titleIdx;
    private uint erro;

    public BASE_TITLE_GET_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read() => this.titleIdx = (int) this.readC();

    public override void run()
    {
      try
      {
        Account player = this._client._player;
        if (player == null || this.titleIdx >= 45)
          return;
        if (player._titles.ownerId == 0L)
        {
          TitleManager.getInstance().CreateTitleDB(player.player_id);
          player._titles = new PlayerTitles()
          {
            ownerId = player.player_id
          };
        }
        TitleQ title = TitlesXML.getTitle(this.titleIdx);
        if (title != null)
        {
          TitleQ title1;
          TitleQ title2;
          TitlesXML.get2Titles(title._req1, title._req2, out title1, out title2, false);
          if ((title._req1 == 0 || title1 != null) && (title._req2 == 0 || title2 != null) && (player._rank >= title._rank && player.brooch >= title._brooch && (player.medal >= title._medals && player.blue_order >= title._blueOrder)) && (player.insignia >= title._insignia && !player._titles.Contains(title._flag) && (player._titles.Contains(title1._flag) && player._titles.Contains(title2._flag))))
          {
            player.brooch -= title._brooch;
            player.medal -= title._medals;
            player.blue_order -= title._blueOrder;
            player.insignia -= title._insignia;
            long flags = player._titles.Add(title._flag);
            TitleManager.getInstance().updateTitlesFlags(player.player_id, flags);
            List<ItemsModel> awards = TitleAwardsXML.getAwards(this.titleIdx);
            if (awards.Count > 0)
              this._client.SendPacket((SendPacket) new INVENTORY_ITEM_CREATE_PAK(1, player, awards));
            this._client.SendPacket((SendPacket) new BASE_QUEST_UPDATE_INFO_PAK(player));
            TitleManager.getInstance().updateRequi(player.player_id, player.medal, player.insignia, player.blue_order, player.brooch);
            if (player._titles.Slots < title._slot)
            {
              player._titles.Slots = title._slot;
              ComDiv.updateDB("player_titles", "titleslots", (object) player._titles.Slots, "owner_id", (object) player.player_id);
            }
          }
          else
            this.erro = 2147487875U;
        }
        else
          this.erro = 2147487875U;
        this._client.SendPacket((SendPacket) new BASE_TITLE_GET_PAK(this.erro, player._titles.Slots));
      }
      catch (Exception ex)
      {
        Logger.warning("[BASE_TITLE_GET_REC] " + ex.ToString());
      }
    }
  }
}
