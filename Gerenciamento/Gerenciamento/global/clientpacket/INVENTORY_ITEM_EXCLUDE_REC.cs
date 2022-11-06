
// Type: Game.global.clientpacket.INVENTORY_ITEM_EXCLUDE_REC
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.managers;
using Core.models.account.players;
using Core.models.enums.flags;
using Core.server;
using Game.data.model;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
  public class INVENTORY_ITEM_EXCLUDE_REC : ReceiveGamePacket
  {
    private long objId;
    private uint erro = 1;

    public INVENTORY_ITEM_EXCLUDE_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read() => this.objId = this.readQ();

    public override void run()
    {
      try
      {
        Account player = this._client._player;
        if (player == null)
          return;
        ItemsModel itemsModel = player._inventory.getItem(this.objId);
        PlayerBonus bonus = player._bonus;
        if (itemsModel == null)
          this.erro = 2147483648U;
        else if (ComDiv.getIdStatics(itemsModel._id, 1) == 12)
        {
          if (bonus == null)
          {
            this._client.SendPacket((SendPacket) new INVENTORY_ITEM_EXCLUDE_PAK(2147483648U));
            return;
          }
          if (!bonus.RemoveBonuses(itemsModel._id))
          {
            if (itemsModel._id == 1200014000)
            {
              if (ComDiv.updateDB("player_bonus", "sightcolor", (object) 4, "player_id", (object) player.player_id))
              {
                bonus.sightColor = 4;
                this._client.SendPacket((SendPacket) new BASE_USER_EFFECTS_PAK(0, bonus));
              }
              else
                this.erro = 2147483648U;
            }
            else if (itemsModel._id == 1200010000)
            {
              if (bonus.fakeNick.Length == 0)
                this.erro = 2147483648U;
              else if (ComDiv.updateDB("accounts", "player_name", (object) bonus.fakeNick, "player_id", (object) player.player_id) && ComDiv.updateDB("player_bonus", "fakenick", (object) "", "player_id", (object) player.player_id))
              {
                player.player_name = bonus.fakeNick;
                bonus.fakeNick = "";
                this._client.SendPacket((SendPacket) new BASE_USER_EFFECTS_PAK(0, bonus));
                this._client.SendPacket((SendPacket) new AUTH_CHANGE_NICKNAME_PAK(player.player_name));
              }
              else
                this.erro = 2147483648U;
            }
            else if (itemsModel._id == 1200009000)
            {
              if (ComDiv.updateDB("player_bonus", "fakerank", (object) 55, "player_id", (object) player.player_id))
              {
                bonus.fakeRank = 55;
                this._client.SendPacket((SendPacket) new BASE_USER_EFFECTS_PAK(0, bonus));
              }
              else
                this.erro = 2147483648U;
            }
            else if (itemsModel._id == 1200006000)
            {
              if (ComDiv.updateDB("accounts", "name_color", (object) 0, "player_id", (object) player.player_id))
              {
                player.name_color = 0;
                this._client.SendPacket((SendPacket) new BASE_2612_PAK(player));
                Room room = player._room;
                if (room != null)
                {
                  using (ROOM_GET_NICKNAME_PAK roomGetNicknamePak = new ROOM_GET_NICKNAME_PAK(player._slotId, player.player_name, player.name_color))
                    room.SendPacketToPlayers((SendPacket) roomGetNicknamePak);
                }
              }
              else
                this.erro = 2147483648U;
            }
          }
          else
            PlayerManager.updatePlayerBonus(player.player_id, bonus.bonuses, bonus.freepass);
          CupomFlag cupomEffect = CupomEffectManager.getCupomEffect(itemsModel._id);
          if (cupomEffect != null && cupomEffect.EffectFlag > (CupomEffects) 0 && player.effects.HasFlag((Enum) cupomEffect.EffectFlag))
          {
            player.effects -= cupomEffect.EffectFlag;
            PlayerManager.updateCupomEffects(player.player_id, player.effects);
          }
        }
        if (this.erro == 1U && itemsModel != null)
        {
          if (PlayerManager.DeleteItem(itemsModel._objId, player.player_id))
            player._inventory.RemoveItem(itemsModel);
          else
            this.erro = 2147483648U;
        }
        this._client.SendPacket((SendPacket) new INVENTORY_ITEM_EXCLUDE_PAK(this.erro, this.objId));
      }
      catch (Exception ex)
      {
        Logger.info("[INVENTORY_ITEM_EXCLUDE_REC] " + ex.ToString());
        this._client.SendPacket((SendPacket) new INVENTORY_ITEM_EXCLUDE_PAK(2147483648U));
      }
    }
  }
}
