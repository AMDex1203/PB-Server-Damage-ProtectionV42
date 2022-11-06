
// Type: Game.global.clientpacket.BATTLE_RESPAWN_REC
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.managers;
using Core.models.account.players;
using Core.models.enums;
using Core.models.enums.flags;
using Core.models.room;
using Core.server;
using Game.data.managers;
using Game.data.model;
using Game.data.sync;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
  public class BATTLE_RESPAWN_REC : ReceiveGamePacket
  {
    private PlayerEquipedItems equip;
    private int WeaponsFlag;

    public BATTLE_RESPAWN_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read()
    {
      this.equip = new PlayerEquipedItems();
      this.equip._primary = this.readD();
      this.equip._secondary = this.readD();
      this.equip._melee = this.readD();
      this.equip._grenade = this.readD();
      this.equip._special = this.readD();
      this.readD();
      this.equip._red = this.readD();
      this.equip._blue = this.readD();
      this.equip._helmet = this.readD();
      this.equip._beret = this.readD();
      this.equip._dino = this.readD();
      this.WeaponsFlag = (int) this.readC();
    }

    public override void run()
    {
      try
      {
        Account player = this._client._player;
        if (player == null)
          return;
        Room room = player._room;
        if (room == null || room._state != RoomState.Battle)
          return;
        Slot slot = room.getSlot(player._slotId);
        if (slot == null || slot.state != SLOT_STATE.BATTLE)
          return;
        if (slot._deathState.HasFlag((Enum) DeadEnum.isDead) || slot._deathState.HasFlag((Enum) DeadEnum.useChat))
          slot._deathState = DeadEnum.isAlive;
        PlayerManager.CheckEquipedItems(this.equip, player._inventory._items, true);
        if (this.ClassicModeCheck(room, this.equip, player))
          this._client.SendPacket((SendPacket) new SERVER_MESSAGE_ANNOUNCE_PAK("Você não pode equipar este item devido as regras da sala. O Servidor retornou para sua arma equipada no lobby da sala."));
        slot._equip = this.equip;
        if ((this.WeaponsFlag & 8) > 0)
          this.insertItem(this.equip._primary, slot);
        if ((this.WeaponsFlag & 4) > 0)
          this.insertItem(this.equip._secondary, slot);
        if ((this.WeaponsFlag & 2) > 0)
          this.insertItem(this.equip._melee, slot);
        if ((this.WeaponsFlag & 1) > 0)
          this.insertItem(this.equip._grenade, slot);
        this.insertItem(this.equip._special, slot);
        if (slot._team == 0)
          this.insertItem(this.equip._red, slot);
        else
          this.insertItem(this.equip._blue, slot);
        this.insertItem(this.equip._helmet, slot);
        this.insertItem(this.equip._beret, slot);
        this.insertItem(this.equip._dino, slot);
        using (BATTLE_RESPAWN_PAK battleRespawnPak = new BATTLE_RESPAWN_PAK(room, slot))
          room.SendPacketToPlayers((SendPacket) battleRespawnPak, SLOT_STATE.BATTLE, 0);
        if (slot.firstRespawn)
        {
          slot.firstRespawn = false;
          Game_SyncNet.SendUDPPlayerSync(room, slot, player.effects, 0);
        }
        else
          Game_SyncNet.SendUDPPlayerSync(room, slot, player.effects, 2);
        if ((int) room.weaponsFlag == this.WeaponsFlag)
          return;
        Logger.warning("Room: " + room.weaponsFlag.ToString() + "; Player: " + this.WeaponsFlag.ToString());
      }
      catch (Exception ex)
      {
        Logger.warning("[BATTLE_RESPAWN_REC] " + ex.ToString());
      }
    }

    private bool ClassicModeCheck(Room room, PlayerEquipedItems equip, Account player)
    {
      bool flag = false;
      if (!room.name.ToLower().Contains("@camp") && !room.name.ToLower().Contains("camp") && (!room.name.ToLower().Contains("@cnpb") && !room.name.ToLower().Contains("cnpb")) && (!room.name.ToLower().Contains("@79") && !room.name.ToLower().Contains("79") && (!room.name.ToLower().Contains("@lan") && !room.name.ToLower().Contains("lan"))) && (!room.name.ToLower().Contains("@reload") && !room.name.ToLower().Contains("reload") && (!room.name.ToLower().Contains("@sbarrett") && !room.name.ToLower().Contains("sbarrett")) && (!room.name.ToLower().Contains("@sshotgun") && !room.name.ToLower().Contains("sshotgun") && (!room.name.ToLower().Contains("@s120") && !room.name.ToLower().Contains("s120")))) && (!room.name.ToLower().Contains("@sdual") && !room.name.ToLower().Contains("sdual") && (!room.name.ToLower().Contains("@sarco") && !room.name.ToLower().Contains("sarco"))) || !ConfigGS.EnableClassicRules)
        return false;
      if (room.name.ToLower().Contains("@camp") || room.name.ToLower().Contains(" camp") || (room.name.ToLower().Contains("camp ") || room.name.ToLower().Contains("camp")))
      {
        for (int index = 0; index < ClassicModeManager.itemscamp.Count; ++index)
        {
          int listid = ClassicModeManager.itemscamp[index];
          if (ClassicModeManager.IsBlocked(listid, equip._primary))
          {
            equip._primary = player._equip._primary;
            flag = true;
          }
          else if (ClassicModeManager.IsBlocked(listid, equip._secondary))
          {
            equip._secondary = player._equip._secondary;
            flag = true;
          }
          else if (ClassicModeManager.IsBlocked(listid, equip._melee))
          {
            equip._melee = player._equip._melee;
            flag = true;
          }
          else if (ClassicModeManager.IsBlocked(listid, equip._grenade))
          {
            equip._grenade = player._equip._grenade;
            flag = true;
          }
          else if (ClassicModeManager.IsBlocked(listid, equip._special))
          {
            equip._special = player._equip._special;
            flag = true;
          }
        }
      }
      return flag;
    }

    private void insertItem(int id, Slot slot)
    {
      lock (slot.armas_usadas)
      {
        if (slot.armas_usadas.Contains(id))
          return;
        slot.armas_usadas.Add(id);
      }
    }
  }
}
