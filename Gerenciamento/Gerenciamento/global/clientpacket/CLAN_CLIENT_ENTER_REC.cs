
// Type: Game.global.clientpacket.CLAN_CLIENT_ENTER_REC
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.managers;
using Core.models.account.clan;
using Core.models.enums;
using Core.server;
using Game.data.managers;
using Game.data.model;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
  public class CLAN_CLIENT_ENTER_REC : ReceiveGamePacket
  {
    private int id;

    public CLAN_CLIENT_ENTER_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read()
    {
    }

    public override void run()
    {
      try
      {
        Account player = this._client._player;
        if (player == null)
          return;
        Room room = player._room;
        if (room != null)
        {
          room.changeSlotState(player._slotId, SLOT_STATE.CLAN, false);
          room.StopCountDown(player._slotId);
          room.updateSlotsInfo();
        }
        Clan clan = ClanManager.getClan(player.clanId);
        if (player.clanId == 0 && player.player_name.Length > 0)
          this.id = PlayerManager.getRequestClanId(player.player_id);
        this._client.SendPacket((SendPacket) new CLAN_CLIENT_ENTER_PAK(this.id > 0 ? this.id : clan.id, player.clanAccess));
        if (clan.id <= 0 || this.id != 0)
          return;
        this._client.SendPacket((SendPacket) new CLAN_DETAIL_INFO_PAK(0, clan));
      }
      catch (Exception ex)
      {
        Logger.info("CLAN_CLIENT_ENTER_REC: " + ex.ToString());
      }
    }
  }
}
