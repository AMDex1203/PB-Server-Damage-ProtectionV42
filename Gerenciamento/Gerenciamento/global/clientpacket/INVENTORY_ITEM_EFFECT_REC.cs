
// Type: Game.global.clientpacket.INVENTORY_ITEM_EFFECT_REC
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.managers;
using Core.models.account.clan;
using Core.models.account.players;
using Core.server;
using Game.data.managers;
using Game.data.model;
using Game.data.utils;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
  public class INVENTORY_ITEM_EFFECT_REC : ReceiveGamePacket
  {
    private long objId;
    private uint objetivo;
    private uint erro = 1;
    private byte[] info;
    private string txt;

    public INVENTORY_ITEM_EFFECT_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read()
    {
      this.objId = this.readQ();
      this.info = this.readB((int) this.readC());
    }

    public override void run()
    {
      try
      {
        Account player = this._client._player;
        ItemsModel itemsModel = player == null ? (ItemsModel) null : player._inventory.getItem(this.objId);
        if (itemsModel != null && itemsModel._id > 1300000000)
        {
          int itemId = ComDiv.createItemId(12, ComDiv.getIdStatics(itemsModel._id, 2), ComDiv.getIdStatics(itemsModel._id, 3), 0);
          uint cuponDays = uint.Parse(DateTime.Now.AddDays((double) ComDiv.getIdStatics(itemsModel._id, 4)).ToString("yyMMddHHmm"));
          switch (itemId)
          {
            case 1200005000:
            case 1201052000:
              this.objetivo = BitConverter.ToUInt32(this.info, 0);
              break;
            case 1200010000:
            case 1201047000:
            case 1201051000:
              this.txt = ComDiv.arrayToString(this.info, this.info.Length);
              break;
            default:
              if (this.info.Length != 0)
              {
                this.objetivo = (uint) this.info[0];
                break;
              }
              break;
          }
          this.CreateCuponEffects(itemId, cuponDays, player);
        }
        else
          this.erro = 2147483648U;
        this._client.SendPacket((SendPacket) new INVENTORY_ITEM_EQUIP_PAK(this.erro, itemsModel, player));
      }
      catch (Exception ex)
      {
        Logger.info("INVENTORY_ITEM_EFFECT_REC: " + ex.ToString());
        this._client.SendPacket((SendPacket) new INVENTORY_ITEM_EQUIP_PAK(2147483648U));
      }
    }

    private void CreateCuponEffects(int cupomId, uint cuponDays, Account p)
    {
      switch (cupomId)
      {
        case 1200005000:
          Clan clan1 = ClanManager.getClan(p.clanId);
          if (clan1.id > 0 && clan1.ownerId == this._client.player_id && ComDiv.updateDB("clan_data", "color", (object) (int) this.objetivo, "clan_id", (object) clan1.id))
          {
            clan1.nameColor = (byte) this.objetivo;
            this._client.SendPacket((SendPacket) new CLAN_CHANGE_NAME_COLOR_PAK(clan1.nameColor));
            break;
          }
          this.erro = 2147483648U;
          break;
        case 1200006000:
          if (ComDiv.updateDB("accounts", "name_color", (object) (int) this.objetivo, "player_id", (object) p.player_id))
          {
            p.name_color = (int) this.objetivo;
            this._client.SendPacket((SendPacket) new INVENTORY_ITEM_CREATE_PAK(1, p, new ItemsModel(cupomId, 3, "NameColor [Active]", 2, cuponDays)));
            this._client.SendPacket((SendPacket) new BASE_2612_PAK(p));
            if (p._room == null)
              break;
            using (ROOM_GET_NICKNAME_PAK roomGetNicknamePak = new ROOM_GET_NICKNAME_PAK(p._slotId, p.player_name, p.name_color))
            {
              p._room.SendPacketToPlayers((SendPacket) roomGetNicknamePak);
              break;
            }
          }
          else
          {
            this.erro = 2147483648U;
            break;
          }
        case 1200009000:
          if ((int) this.objetivo >= 51 || (int) this.objetivo < p._rank - 10 || (int) this.objetivo > p._rank + 10)
          {
            this.erro = 2147483648U;
            break;
          }
          if (ComDiv.updateDB("player_bonus", "fakerank", (object) (int) this.objetivo, "player_id", (object) p.player_id))
          {
            p._bonus.fakeRank = (int) this.objetivo;
            this._client.SendPacket((SendPacket) new BASE_USER_EFFECTS_PAK(this.info.Length, p._bonus));
            this._client.SendPacket((SendPacket) new INVENTORY_ITEM_CREATE_PAK(1, p, new ItemsModel(cupomId, 3, "Patente falsa [Active]", 2, cuponDays)));
            if (p._room == null)
              break;
            p._room.updateSlotsInfo();
            break;
          }
          this.erro = 2147483648U;
          break;
        case 1200010000:
          if (string.IsNullOrEmpty(this.txt) || this.txt.Length < ConfigGS.minNickSize || this.txt.Length > ConfigGS.maxNickSize)
          {
            this.erro = 2147483648U;
            break;
          }
          if (ComDiv.updateDB("player_bonus", "fakenick", (object) p.player_name, "player_id", (object) p.player_id) && ComDiv.updateDB("accounts", "player_name", (object) this.txt, "player_id", (object) p.player_id))
          {
            p._bonus.fakeNick = p.player_name;
            p.player_name = this.txt;
            this._client.SendPacket((SendPacket) new BASE_USER_EFFECTS_PAK(this.info.Length, p._bonus));
            this._client.SendPacket((SendPacket) new AUTH_CHANGE_NICKNAME_PAK(p.player_name));
            this._client.SendPacket((SendPacket) new INVENTORY_ITEM_CREATE_PAK(1, p, new ItemsModel(cupomId, 3, "FakeNick [Active]", 2, cuponDays)));
            if (p._room == null)
              break;
            p._room.updateSlotsInfo();
            break;
          }
          this.erro = 2147483648U;
          break;
        case 1200014000:
          if (ComDiv.updateDB("player_bonus", "sightcolor", (object) (int) this.objetivo, "player_id", (object) p.player_id))
          {
            p._bonus.sightColor = (int) this.objetivo;
            this._client.SendPacket((SendPacket) new BASE_USER_EFFECTS_PAK(this.info.Length, p._bonus));
            this._client.SendPacket((SendPacket) new INVENTORY_ITEM_CREATE_PAK(1, p, new ItemsModel(cupomId, 3, "Cor da mira [Active]", 2, cuponDays)));
            break;
          }
          this.erro = 2147483648U;
          break;
        case 1201047000:
          if (string.IsNullOrEmpty(this.txt) || this.txt.Length < ConfigGS.minNickSize || (this.txt.Length > ConfigGS.maxNickSize || p._inventory.getItem(1200010000) != null))
          {
            this.erro = 2147483648U;
            break;
          }
          if (!PlayerManager.isPlayerNameExist(this.txt))
          {
            if (ComDiv.updateDB("accounts", "player_name", (object) this.txt, "player_id", (object) p.player_id))
            {
              NickHistoryManager.CreateHistory(p.player_id, p.player_name, this.txt, "Change nick");
              p.player_name = this.txt;
              if (p._room != null)
              {
                using (ROOM_GET_NICKNAME_PAK roomGetNicknamePak = new ROOM_GET_NICKNAME_PAK(p._slotId, p.player_name, p.name_color))
                  p._room.SendPacketToPlayers((SendPacket) roomGetNicknamePak);
              }
              this._client.SendPacket((SendPacket) new AUTH_CHANGE_NICKNAME_PAK(p.player_name));
              if (p.clanId > 0)
              {
                using (CLAN_MEMBER_INFO_UPDATE_PAK memberInfoUpdatePak = new CLAN_MEMBER_INFO_UPDATE_PAK(p))
                  ClanManager.SendPacket((SendPacket) memberInfoUpdatePak, p.clanId, -1L, true, true);
              }
              AllUtils.syncPlayerToFriends(p, true);
              break;
            }
            this.erro = 2147483648U;
            break;
          }
          this.erro = 2147483923U;
          break;
        case 1201051000:
          if (string.IsNullOrEmpty(this.txt) || this.txt.Length > 16)
          {
            this.erro = 2147483648U;
            break;
          }
          Clan clan2 = ClanManager.getClan(p.clanId);
          if (clan2.id > 0 && clan2.ownerId == this._client.player_id)
          {
            if (!ClanManager.isClanNameExist(this.txt) && ComDiv.updateDB("clan_data", "clan_name", (object) this.txt, "clan_id", (object) p.clanId))
            {
              clan2.name = this.txt;
              using (CLAN_CHANGE_NAME_PAK clanChangeNamePak = new CLAN_CHANGE_NAME_PAK(this.txt))
              {
                ClanManager.SendPacket((SendPacket) clanChangeNamePak, p.clanId, -1L, true, true);
                break;
              }
            }
            else
            {
              this.erro = 2147483648U;
              break;
            }
          }
          else
          {
            this.erro = 2147483648U;
            break;
          }
        case 1201052000:
          Clan clan3 = ClanManager.getClan(p.clanId);
          if (clan3.id > 0 && clan3.ownerId == this._client.player_id && (!ClanManager.isClanLogoExist(this.objetivo) && PlayerManager.updateClanLogo(p.clanId, this.objetivo)))
          {
            clan3.logo = this.objetivo;
            using (CLAN_CHANGE_LOGO_PAK clanChangeLogoPak = new CLAN_CHANGE_LOGO_PAK(this.objetivo))
            {
              ClanManager.SendPacket((SendPacket) clanChangeLogoPak, p.clanId, -1L, true, true);
              break;
            }
          }
          else
          {
            this.erro = 2147483648U;
            break;
          }
        case 1201085000:
          if (p._room != null)
          {
            Account playerBySlot = p._room.getPlayerBySlot((int) this.objetivo);
            if (playerBySlot != null)
            {
              this._client.SendPacket((SendPacket) new ROOM_INSPECTPLAYER_PAK(playerBySlot));
              break;
            }
            this.erro = 2147483648U;
            break;
          }
          this.erro = 2147483648U;
          break;
        default:
          Logger.error("[ITEM_EFFECT] Efeito do cupom não encontrado! Id: " + cupomId.ToString());
          this.erro = 2147483648U;
          break;
      }
    }
  }
}
