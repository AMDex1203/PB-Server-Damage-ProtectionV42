
// Type: Game.data.model.Account
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.managers;
using Core.models.account;
using Core.models.account.players;
using Core.models.account.title;
using Core.models.enums;
using Core.models.enums.flags;
using Core.server;
using Core.sql;
using Game.data.managers;
using Game.data.sync;
using Game.data.xml;
using Game.global.serverpacket;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Net;

namespace Game.data.model
{
  public class Account
  {
    public byte[] LocalIP = new byte[4];
    public bool _isOnline;
    public bool HideGMcolor;
    public bool AntiKickGM;
    public bool LoadedShop;
    public bool DebugPing;
    public bool DebugHitMarker;
    public string player_name = "";
    public string password;
    public string login;
    public long player_id;
    public long ban_obj_id;
    public uint LastRankUpDate;
    public uint LastLoginDate;
    public IPAddress PublicIP;
    public CupomEffects effects;
    public PlayerSession Session;
    public int LastRoomPage;
    public int LastPlayerPage;
    public int tourneyLevel;
    public int channelId = -1;
    public int clanAccess;
    public int clanDate;
    public int _exp;
    public int _gp;
    public int clanId;
    public int _money;
    public int brooch;
    public int insignia;
    public int medal;
    public int blue_order;
    public int _slotId = -1;
    public int name_color;
    public int _rank;
    public int pc_cafe;
    public int matchSlot = -1;
    public PlayerEquipedItems _equip = new PlayerEquipedItems();
    public PlayerInventory _inventory = new PlayerInventory();
    public PlayerConfig _config;
    public GameClient _connection;
    public Room _room;
    protected Account administrador;
    public PlayerBonus _bonus;
    public Match _match;
    public AccessLevel access;
    public PlayerMissions _mission = new PlayerMissions();
    public PlayerStats _statistic = new PlayerStats();
    public FriendSystem FriendSystem = new FriendSystem();
    public PlayerTitles _titles = new PlayerTitles();
    public AccountStatus _status = new AccountStatus();
    public PlayerEvent _event;
    public DateTime LastSlotChange;
    public DateTime LastLobbyEnter;
    public DateTime LastPingDebug;
    public bool firstEnterLobby;
    public bool showboxMessage = true;

    public bool Set(Account administrador, Room room)
    {
      this.administrador = administrador;
      this._room = room;
      return true;
    }

    public Account()
    {
      this.LastSlotChange = DateTime.Now;
      this.LastLobbyEnter = DateTime.Now;
    }

    public void SimpleClear()
    {
      this._titles = new PlayerTitles();
      this._mission = new PlayerMissions();
      this._inventory = new PlayerInventory();
      this._status = new AccountStatus();
      this.FriendSystem.CleanList();
      this.Session = (PlayerSession) null;
      this._event = (PlayerEvent) null;
      this._match = (Match) null;
      this._room = (Room) null;
      this._config = (PlayerConfig) null;
      this._connection = (GameClient) null;
    }

    public void SetPublicIP(IPAddress address)
    {
      if (address == null)
        this.PublicIP = new IPAddress(new byte[4]);
      this.PublicIP = address;
    }

    public void SetPublicIP(string address) => this.PublicIP = IPAddress.Parse(address);

    public Channel getChannel() => ChannelsXML.getChannel(this.channelId);

    public void ResetPages()
    {
      this.LastRoomPage = 0;
      this.LastPlayerPage = 0;
    }

    public bool getChannel(out Channel channel)
    {
      channel = ChannelsXML.getChannel(this.channelId);
      return channel != null;
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
      lock (AccountManager._contas)
        AccountManager._contas[this.player_id] = this;
    }

    public int getRank() => this._bonus != null && this._bonus.fakeRank != 55 ? this._bonus.fakeRank : this._rank;

    public void Close(int time, bool kicked = false)
    {
      if (this._connection == null)
        return;
      this._connection.Close(time, kicked);
    }

    public void SendPacket(Core.server.SendPacket sp)
    {
      if (this._connection == null)
        return;
      this._connection.SendPacket(sp);
    }

    public void SendPacket(Core.server.SendPacket sp, bool OnlyInServer)
    {
      if (this._connection != null)
      {
        this._connection.SendPacket(sp);
      }
      else
      {
        if (OnlyInServer || this._status.serverId == byte.MaxValue || (int) this._status.serverId == ConfigGS.serverId)
          return;
        Game_SyncNet.SendBytes(this.player_id, sp, (int) this._status.serverId);
      }
    }

    public void SendPacket(byte[] data)
    {
      if (this._connection == null)
        return;
      this._connection.SendPacket(data);
    }

    public void SendPacket(byte[] data, bool OnlyInServer)
    {
      if (this._connection != null)
      {
        this._connection.SendPacket(data);
      }
      else
      {
        if (OnlyInServer || this._status.serverId == byte.MaxValue || (int) this._status.serverId == ConfigGS.serverId)
          return;
        Game_SyncNet.SendBytes(this.player_id, data, (int) this._status.serverId);
      }
    }

    public bool ExecuteQuery(string query)
    {
      int num = 0;
      try
      {
        using (NpgsqlConnection npgsqlConnection = SQLjec.getInstance().conn())
        {
          using (NpgsqlCommand command = npgsqlConnection.CreateCommand())
          {
            npgsqlConnection.Open();
            command.CommandText = query;
            num = command.ExecuteNonQuery();
            npgsqlConnection.Close();
            if (num > 1)
              Logger.error(string.Format(" [Account] (ExecuteQuery) Query: {0} Success: {1}", (object) query, (object) num));
          }
        }
      }
      catch (Exception ex)
      {
        Logger.error(string.Format(" [Account] (ExecuteQuery) Query: {0}\nException: {1}", (object) query, (object) ex));
      }
      return num == 1;
    }

    public void SendCompletePacket(byte[] data)
    {
      if (this._connection == null)
        return;
      this._connection.SendCompletePacket(data);
    }

    public void SendCompletePacket(byte[] data, bool OnlyInServer)
    {
      if (this._connection != null)
      {
        this._connection.SendCompletePacket(data);
      }
      else
      {
        if (OnlyInServer || this._status.serverId == byte.MaxValue || (int) this._status.serverId == ConfigGS.serverId)
          return;
        Game_SyncNet.SendCompleteBytes(this.player_id, data, (int) this._status.serverId);
      }
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

    public void LoadPlayerBonus()
    {
      PlayerBonus playerBonusDb = PlayerManager.getPlayerBonusDB(this.player_id);
      if (playerBonusDb.ownerId == 0L)
      {
        PlayerManager.CreatePlayerBonusDB(this.player_id);
        playerBonusDb.ownerId = this.player_id;
      }
      this._bonus = playerBonusDb;
    }

    public uint getSessionId() => this.Session == null ? 0U : this.Session._sessionId;

    public void SetPlayerId(long id)
    {
      this.player_id = id;
      this.GetAccountInfos(35);
    }

    public void SetPlayerId(long id, int LoadType)
    {
      this.player_id = id;
      this.GetAccountInfos(LoadType);
    }

    public void GetAccountInfos(int LoadType)
    {
      if (LoadType <= 0 || this.player_id <= 0L)
        return;
      if ((LoadType & 1) == 1)
        this._titles = TitleManager.getInstance().getTitleDB(this.player_id);
      if ((LoadType & 2) == 2)
        this._bonus = PlayerManager.getPlayerBonusDB(this.player_id);
      if ((LoadType & 4) == 4)
      {
        List<Friend> friendList = PlayerManager.getFriendList(this.player_id);
        if (friendList.Count > 0)
        {
          this.FriendSystem._friends = friendList;
          AccountManager.getFriendlyAccounts(this.FriendSystem);
        }
      }
      if ((LoadType & 8) == 8)
        this._event = PlayerManager.getPlayerEventDB(this.player_id);
      if ((LoadType & 16) == 16)
        this._config = PlayerManager.getConfigDB(this.player_id);
      if ((LoadType & 32) != 32)
        return;
      List<Friend> friendList1 = PlayerManager.getFriendList(this.player_id);
      if (friendList1.Count <= 0)
        return;
      this.FriendSystem._friends = friendList1;
    }

    public bool UpdateAccountCash(int cashValue) => cashValue >= 0 && this.ExecuteQuery(string.Format("UPDATE accounts SET money='{0}' WHERE id='{1}'", (object) cashValue, (object) this.player_id));


    public void SetCuponsFlags()
    {
      lock (this._inventory._items)
      {
        this.effects = (CupomEffects) 0;
        bool flag = this._room != null && ClassicModeManager.CheckRoomRule(this._room.name.ToUpper());
        for (int index = 0; index < this._inventory._items.Count; ++index)
        {
          ItemsModel itemsModel = this._inventory._items[index];
          if (itemsModel._category == 3 && itemsModel._equip == 2)
          {
            CupomFlag cupomEffect = CupomEffectManager.getCupomEffect(itemsModel._id);
            if (cupomEffect != null && cupomEffect.EffectFlag > (CupomEffects) 0 && !this.effects.HasFlag((System.Enum) cupomEffect.EffectFlag) && (!flag || flag && !ClassicModeManager.CuponsEffectsBlocked.Contains(itemsModel._id)))
              this.effects |= cupomEffect.EffectFlag;
          }
        }
      }
    }

    public int CheckEquipedItems(PlayerEquipedItems equipedItems, bool BattleRules = false)
    {
      int num = 0;
      bool flag1 = false;
      bool flag2 = false;
      bool flag3 = false;
      bool flag4 = false;
      bool flag5 = false;
      bool flag6 = false;
      bool flag7 = false;
      bool flag8 = false;
      bool flag9 = false;
      bool flag10 = false;
      if (equipedItems._primary == 0)
        flag1 = true;
      if (BattleRules)
      {
        if (!flag1 && (equipedItems._primary == 300005025 || equipedItems._primary == 400006007))
          flag1 = true;
        if (!flag3 && equipedItems._melee == 702023001)
          flag3 = true;
      }
      if (equipedItems._beret == 0)
        flag9 = true;
      lock (this._inventory)
      {
        for (int index = 0; index < this._inventory._items.Count; ++index)
        {
          ItemsModel itemsModel = this._inventory._items[index];
          if (itemsModel._count > 0U)
          {
            if (itemsModel._id == equipedItems._primary)
              flag1 = true;
            else if (itemsModel._id == equipedItems._secondary)
              flag2 = true;
            else if (itemsModel._id == equipedItems._melee)
              flag3 = true;
            else if (itemsModel._id == equipedItems._grenade)
              flag4 = true;
            else if (itemsModel._id == equipedItems._special)
              flag5 = true;
            else if (itemsModel._id == equipedItems._red)
              flag6 = true;
            else if (itemsModel._id == equipedItems._blue)
              flag7 = true;
            else if (itemsModel._id == equipedItems._helmet)
              flag8 = true;
            else if (itemsModel._id == equipedItems._beret)
              flag9 = true;
            else if (itemsModel._id == equipedItems._dino)
              flag10 = true;
            if (flag1 & flag2 & flag3 & flag4 & flag5 & flag6 & flag7 & flag8 & flag9 & flag10)
              break;
          }
        }
      }
      if (!flag1 || !flag2 || (!flag3 || !flag4) || !flag5)
        num += 2;
      if (!flag6 || !flag7 || (!flag8 || !flag9) || !flag10)
        ++num;
      if (!flag1)
        equipedItems._primary = 0;
      if (!flag2)
        equipedItems._secondary = 601002003;
      if (!flag3)
        equipedItems._melee = 702001001;
      if (!flag4)
        equipedItems._grenade = 803007001;
      if (!flag5)
        equipedItems._special = 904007002;
      if (!flag6)
        equipedItems._red = 1001001005;
      if (!flag7)
        equipedItems._blue = 1001002006;
      if (!flag8)
        equipedItems._helmet = 1102003001;
      if (!flag9)
        equipedItems._beret = 0;
      if (!flag10)
        equipedItems._dino = 1006003041;
      return num;
    }

    public bool UseChatGM()
    {
      if (this.HideGMcolor)
        return false;
      return this._rank == 53 || this._rank == 54;
    }

    public bool IsGM() => this._rank == 53 || this._rank == 54 || this.HaveGMLevel();

    public bool HaveGMLevel() => this.access > AccessLevel.Streamer;

    public bool HaveAcessLevel() => this.access > AccessLevel.Normal;
  }
}
