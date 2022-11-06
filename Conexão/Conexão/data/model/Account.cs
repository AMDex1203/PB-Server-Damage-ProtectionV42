
// Type: Auth.data.model.Account
// Assembly: pbserver_auth, Version=1.0.7796.38604, Culture=neutral, PublicKeyToken=null
// MVID: 1CFDFF97-782C-4EF8-A621-6E4FDDE32436
// Interprise: C:\Users\Cuzin\3,50pbserver_auth.exe

using Auth.data.managers;
using Core.managers;
using Core.models.account;
using Core.models.account.players;
using Core.models.account.title;
using Core.models.enums.flags;
using Core.server;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;

namespace Auth.data.model
{
  public class Account
  {
    public bool _myConfigsLoaded;
    public bool _isOnline;
    public CupomEffects effects;
    public Account client;
    public uint LastRankUpDate;
    public bool checkSourceInfo;
    public string player_name = "";
    public string password;
    public string login;
    public int tourneyLevel;
    public int _exp;
    public int _gp;
    public int clan_id;
    public int clanAccess;
    public int _money;
    public int pc_cafe;
    public int _rank;
    public int brooch;
    public int insignia;
    public int medal;
    public int blue_order;
    public int name_color;
    public int access;
    public long player_id;
    public long ban_obj_id;
    public PhysicalAddress MacAddress;
    public PlayerEquipedItems _equip = new PlayerEquipedItems();
    public PlayerInventory _inventory = new PlayerInventory();
    public LoginClient _connection;
    public PlayerBonus _bonus;
    public PlayerMissions _mission = new PlayerMissions();
    public PlayerStats _statistic = new PlayerStats();
    public PlayerConfig _config;
    public PlayerTitles _titles;
    public AccountStatus _status = new AccountStatus();
    public PlayerEvent _event;
    public FriendSystem FriendSystem = new FriendSystem();
    public List<Account> _clanPlayers = new List<Account>();

    public void SimpleClear()
    {
      this._config = (PlayerConfig) null;
      this._titles = (PlayerTitles) null;
      this._bonus = (PlayerBonus) null;
      this._event = (PlayerEvent) null;
      this._connection = (LoginClient) null;
      this._inventory = new PlayerInventory();
      this.FriendSystem = new FriendSystem();
      this._clanPlayers = new List<Account>();
      this._equip = new PlayerEquipedItems();
      this._mission = new PlayerMissions();
      this._status = new AccountStatus();
    }

    public void setOnlineStatus(bool online)
    {
      if (this._isOnline == online || !ComDiv.updateDB("accounts", nameof (online), (object) online, "player_id", (object) this.player_id))
        return;
      this._isOnline = online;
    }

    public void updateCacheInfo()
    {
      if (this.player_id == 0L)
        return;
      lock (AccountManager.getInstance()._contas)
        AccountManager.getInstance()._contas[this.player_id] = this;
    }

    public void Close(int time)
    {
      if (this._connection == null)
        return;
      this._connection.Close(time, true);
    }

    public void SendPacket(Core.server.SendPacket sp)
    {
      if (this._connection == null)
        return;
      this._connection.SendPacket(sp);
    }

    public void SendPacket(byte[] data)
    {
      if (this._connection == null)
        return;
      this._connection.SendPacket(data);
    }

    public void SendCompletePacket(byte[] data)
    {
      if (this._connection == null)
        return;
      this._connection.SendCompletePacket(data);
    }

    public void LoadInventory()
    {
      lock (this._inventory._items)
        this._inventory._items.AddRange((IEnumerable<ItemsModel>) PlayerManager.getInventoryItems(this.player_id));
    }

    public void LoadMissionList()
    {
      PlayerMissions mission = MissionManager.getInstance().getMission(this.player_id, this._mission.mission1, this._mission.mission2, this._mission.mission3, this._mission.mission4);
      if (mission == null)
        MissionManager.getInstance().addMissionDB(this.player_id);
      else
        this._mission = mission;
    }

    public void SetPlayerId(long id, int LoadType)
    {
      this.player_id = id;
      this.GetAccountInfos(LoadType);
    }

    public void SetPlayerId()
    {
      this._titles = new PlayerTitles();
      this._bonus = new PlayerBonus();
      this.GetAccountInfos(8);
    }

    public void GetAccountInfos(int LoadType)
    {
      if (LoadType == 0 || this.player_id == 0L)
        return;
      if ((LoadType & 1) == 1)
        this._titles = TitleManager.getInstance().getTitleDB(this.player_id);
      if ((LoadType & 2) == 2)
        this._bonus = PlayerManager.getPlayerBonusDB(this.player_id);
      if ((LoadType & 4) == 4)
      {
        List<Friend> friendList = PlayerManager.getFriendList(this.player_id);
        if (friendList.Count > 0)
          this.FriendSystem._friends = friendList;
      }
      if ((LoadType & 8) == 8)
      {
        this._event = PlayerManager.getPlayerEventDB(this.player_id);
        if (this._event == null)
        {
          PlayerManager.addEventDB(this.player_id);
          this._event = new PlayerEvent();
        }
      }
      if ((LoadType & 16) != 16)
        return;
      this._config = PlayerManager.getConfigDB(this.player_id);
    }

    public bool ComparePassword(string pass) => ConfigGA.isTestMode || pass == this.password;

    public void DiscountPlayerItems()
    {
      bool flag = false;
      uint num1 = uint.Parse(DateTime.Now.ToString("yyMMddHHmm"));
      List<object> objectList = new List<object>();
      int num2 = this._bonus != null ? this._bonus.bonuses : 0;
      int num3 = this._bonus != null ? this._bonus.freepass : 0;
      lock (this._inventory._items)
      {
        for (int index = 0; index < this._inventory._items.Count; ++index)
        {
          ItemsModel itemsModel = this._inventory._items[index];
          if (itemsModel._count <= num1 & itemsModel._equip == 2)
          {
            if (itemsModel._category == 3)
            {
              if (this._bonus != null)
              {
                if (!this._bonus.RemoveBonuses(itemsModel._id))
                {
                  if (itemsModel._id == 1200014000)
                  {
                    ComDiv.updateDB("player_bonus", "sightcolor", (object) 4, "player_id", (object) this.player_id);
                    this._bonus.sightColor = 4;
                  }
                  else if (itemsModel._id == 1200006000)
                  {
                    ComDiv.updateDB("accounts", "name_color", (object) 0, "player_id", (object) this.player_id);
                    this.name_color = 0;
                  }
                  else if (itemsModel._id == 1200009000)
                  {
                    ComDiv.updateDB("player_bonus", "fakerank", (object) 55, "player_id", (object) this.player_id);
                    this._bonus.fakeRank = 55;
                  }
                  else if (itemsModel._id == 1200010000 && this._bonus.fakeNick.Length > 0)
                  {
                    ComDiv.updateDB("player_bonus", "fakenick", (object) "", "player_id", (object) this.player_id);
                    ComDiv.updateDB("accounts", "player_name", (object) this._bonus.fakeNick, "player_id", (object) this.player_id);
                    this.player_name = this._bonus.fakeNick;
                    this._bonus.fakeNick = "";
                  }
                }
                CupomFlag cupomEffect = CupomEffectManager.getCupomEffect(itemsModel._id);
                if (cupomEffect != null && cupomEffect.EffectFlag > (CupomEffects) 0 && this.effects.HasFlag((Enum) cupomEffect.EffectFlag))
                {
                  this.effects -= cupomEffect.EffectFlag;
                  flag = true;
                }
              }
              else
                continue;
            }
            objectList.Add((object) itemsModel._objId);
            this._inventory._items.RemoveAt(index--);
          }
          else if (itemsModel._count == 0U)
          {
            objectList.Add((object) itemsModel._objId);
            this._inventory._items.RemoveAt(index--);
          }
        }
        ComDiv.deleteDB("player_items", "object_id", objectList.ToArray(), "owner_id", (object) this.player_id);
      }
      if (this._bonus != null && (this._bonus.bonuses != num2 || this._bonus.freepass != num3))
        PlayerManager.updatePlayerBonus(this.player_id, this._bonus.bonuses, this._bonus.freepass);
      if (this.effects < (CupomEffects) 0)
        this.effects = (CupomEffects) 0;
      if (flag)
        PlayerManager.updateCupomEffects(this.player_id, this.effects);
      this._inventory.LoadBasicItems();
      int num4 = PlayerManager.CheckEquipedItems(this._equip, this._inventory._items);
      if (num4 <= 0)
        return;
      DBQuery query = new DBQuery();
      if ((num4 & 2) == 2)
        PlayerManager.updateWeapons(this._equip, query);
      if ((num4 & 1) == 1)
        PlayerManager.updateChars(this._equip, query);
      ComDiv.updateDB("accounts", "player_id", (object) this.player_id, query.GetTables(), query.GetValues());
    }

    public bool IsGM() => this._rank == 53 || this._rank == 54 || this.access > 2;

    public bool HaveGMLevel() => this.access > 2;

    public bool HaveAcessLevel() => this.access > 0;
  }
}
