
// Type: Game.global.clientpacket.EVENT_PLAYTIME_REWARD_REC
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.managers;
using Core.managers.events;
using Core.models.account.players;
using Core.models.shop;
using Core.server;
using Game.data.model;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
  public class EVENT_PLAYTIME_REWARD_REC : ReceiveGamePacket
  {
    private int goodId;

    public EVENT_PLAYTIME_REWARD_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read() => this.goodId = this.readD();

    public override void run()
    {
      try
      {
        Account player = this._client._player;
        if (player == null)
          return;
        PlayerEvent playerEvent = player._event;
        GoodItem good = ShopManager.getGood(this.goodId);
        if (good == null || playerEvent == null)
          return;
        PlayTimeModel runningEvent = EventPlayTimeSyncer.GetRunningEvent();
        if (runningEvent == null)
          return;
        uint rewardCount = (uint) runningEvent.GetRewardCount(this.goodId);
        if (playerEvent.LastPlaytimeFinish != 1 || rewardCount <= 0U || !ComDiv.updateDB("player_events", "last_playtime_finish", (object) 2, "player_id", (object) this._client.player_id))
          return;
        playerEvent.LastPlaytimeFinish = 2;
        this._client.SendPacket((SendPacket) new INVENTORY_ITEM_CREATE_PAK(1, player, new ItemsModel(good._item._id, good._item._category, "Playtime reward", good._item._equip, rewardCount)));
      }
      catch (Exception ex)
      {
        Logger.info("EVENT_PLAYTIME_REWARD_REC] " + ex.ToString());
      }
    }
  }
}
