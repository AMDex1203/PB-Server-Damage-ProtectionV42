
// Type: Game.global.clientpacket.INVENTORY_LEAVE_REC
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.managers;
using Core.models.account.players;
using Core.models.enums;
using Core.server;
using Game.data.model;
using Game.data.utils;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
  public class INVENTORY_LEAVE_REC : ReceiveGamePacket
  {
    private int erro;
    private int type;
    private PlayerEquipedItems data;

    public INVENTORY_LEAVE_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read() => this.type = this.readD();

    public override void run()
    {
      try
      {
        if (this._client == null)
          return;
        Account player = this._client._player;
        if (player == null)
          return;
        this.data = new PlayerEquipedItems();
        DBQuery query = new DBQuery();
        if ((this.type & 1) == 1)
          this.LoadCharaData(player, query);
        if ((this.type & 2) == 2)
          this.LoadWeaponsData(player, query);
        if (ComDiv.updateDB("accounts", "player_id", (object) player.player_id, query.GetTables(), query.GetValues()))
        {
          this.UpdateChara(player);
          this.UpdateWeapons(player);
        }
        Room room = player._room;
        if (room != null)
        {
          if (this.type > 0)
            AllUtils.updateSlotEquips(player, room);
          room.changeSlotState(player._slotId, SLOT_STATE.NORMAL, true);
        }
        this._client.SendPacket((SendPacket) new INVENTORY_LEAVE_PAK(this.erro, this.erro > 0 ? 3 : 0));
      }
      catch (Exception ex)
      {
        Logger.info("INVENTORY_LEAVE_REC: " + ex.ToString());
      }
    }

    private void LoadWeaponsData(Account p, DBQuery query)
    {
      this.data._primary = this.readD();
      this.data._secondary = this.readD();
      this.data._melee = this.readD();
      this.data._grenade = this.readD();
      this.data._special = this.readD();
      if (p != null)
        PlayerManager.updateWeapons(this.data, p._equip, query);
      else
        this.erro = -1;
    }

    private void UpdateWeapons(Account p)
    {
      if ((this.type & 2) != 2)
        return;
      p._equip._primary = this.data._primary;
      p._equip._secondary = this.data._secondary;
      p._equip._melee = this.data._melee;
      p._equip._grenade = this.data._grenade;
      p._equip._special = this.data._special;
    }

    private void UpdateChara(Account p)
    {
      if ((this.type & 1) != 1)
        return;
      p._equip._red = this.data._red;
      p._equip._blue = this.data._blue;
      p._equip._helmet = this.data._helmet;
      p._equip._beret = this.data._beret;
      p._equip._dino = this.data._dino;
    }

    private void LoadCharaData(Account p, DBQuery query)
    {
      this.data._red = this.readD();
      this.data._blue = this.readD();
      this.data._helmet = this.readD();
      this.data._beret = this.readD();
      this.data._dino = this.readD();
      if (p != null)
        PlayerManager.updateChars(this.data, p._equip, query);
      else
        this.erro = -1;
    }
  }
}
