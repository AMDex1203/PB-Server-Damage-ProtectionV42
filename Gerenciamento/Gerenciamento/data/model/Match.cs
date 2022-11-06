
// Type: Game.data.model.Match
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.models.account.clan;
using Core.models.enums.match;
using Core.server;
using Game.data.managers;
using Game.data.utils;
using Game.data.xml;
using Game.global.serverpacket;
using System.Collections.Generic;
using System.Threading;

namespace Game.data.model
{
  public class Match
  {
    public Clan clan;
    public int formação;
    public int serverId;
    public int channelId;
    public int _matchId = -1;
    public int _leader;
    public int friendId;
    public SLOT_MATCH[] _slots = new SLOT_MATCH[8];
    public MatchState _state = MatchState.Ready;

    public Match(Clan clan)
    {
      this.clan = clan;
      for (int slot = 0; slot < 8; ++slot)
        this._slots[slot] = new SLOT_MATCH(slot);
    }

    public bool getSlot(int slotId, out SLOT_MATCH slot)
    {
      lock (this._slots)
      {
        slot = (SLOT_MATCH) null;
        if (slotId >= 0 && slotId < 16)
          slot = this._slots[slotId];
        return slot != null;
      }
    }

    public SLOT_MATCH getSlot(int slotId)
    {
      lock (this._slots)
        return slotId >= 0 && slotId < 16 ? this._slots[slotId] : (SLOT_MATCH) null;
    }

    public void setNewLeader(int leader, int oldLeader)
    {
      Monitor.Enter((object) this._slots);
      if (leader == -1)
      {
        for (int index = 0; index < this.formação; ++index)
        {
          if (index != oldLeader && this._slots[index]._playerId > 0L)
          {
            this._leader = index;
            break;
          }
        }
      }
      else
        this._leader = leader;
      Monitor.Exit((object) this._slots);
    }

    public bool addPlayer(Account player)
    {
      lock (this._slots)
      {
        for (int index = 0; index < this.formação; ++index)
        {
          SLOT_MATCH slot = this._slots[index];
          if (slot._playerId == 0L && slot.state == SlotMatchState.Empty)
          {
            slot._playerId = player.player_id;
            slot.state = SlotMatchState.Normal;
            player._match = this;
            player.matchSlot = index;
            player._status.updateClanMatch((byte) this.friendId);
            AllUtils.syncPlayerToClanMembers(player);
            return true;
          }
        }
      }
      return false;
    }

    public Account getPlayerBySlot(SLOT_MATCH slot)
    {
      try
      {
        long playerId = slot._playerId;
        return playerId > 0L ? AccountManager.getAccount(playerId, true) : (Account) null;
      }
      catch
      {
        return (Account) null;
      }
    }

    public Account getPlayerBySlot(int slotId)
    {
      try
      {
        long playerId = this._slots[slotId]._playerId;
        return playerId > 0L ? AccountManager.getAccount(playerId, true) : (Account) null;
      }
      catch
      {
        return (Account) null;
      }
    }

    public List<Account> getAllPlayers(int exception)
    {
      List<Account> accountList = new List<Account>();
      lock (this._slots)
      {
        for (int index = 0; index < 8; ++index)
        {
          long playerId = this._slots[index]._playerId;
          if (playerId > 0L && index != exception)
          {
            Account account = AccountManager.getAccount(playerId, true);
            if (account != null)
              accountList.Add(account);
          }
        }
      }
      return accountList;
    }

    public List<Account> getAllPlayers()
    {
      List<Account> accountList = new List<Account>();
      lock (this._slots)
      {
        for (int index = 0; index < 8; ++index)
        {
          long playerId = this._slots[index]._playerId;
          if (playerId > 0L)
          {
            Account account = AccountManager.getAccount(playerId, true);
            if (account != null)
              accountList.Add(account);
          }
        }
      }
      return accountList;
    }

    public void SendPacketToPlayers(SendPacket packet)
    {
      List<Account> allPlayers = this.getAllPlayers();
      if (allPlayers.Count == 0)
        return;
      byte[] completeBytes = packet.GetCompleteBytes("Match.SendPacketToPlayers(SendPacket)");
      foreach (Account account in allPlayers)
        account.SendCompletePacket(completeBytes);
    }

    public void SendPacketToPlayers(SendPacket packet, int exception)
    {
      List<Account> allPlayers = this.getAllPlayers(exception);
      if (allPlayers.Count == 0)
        return;
      byte[] completeBytes = packet.GetCompleteBytes("Match.SendPacketToPlayers(SendPacket,int)");
      foreach (Account account in allPlayers)
        account.SendCompletePacket(completeBytes);
    }

    public Account getLeader()
    {
      try
      {
        return AccountManager.getAccount(this._slots[this._leader]._playerId, true);
      }
      catch
      {
        return (Account) null;
      }
    }

    public int getServerInfo() => this.channelId + this.serverId * 10;

    public int getCountPlayers()
    {
      lock (this._slots)
      {
        int num = 0;
        foreach (SLOT_MATCH slot in this._slots)
        {
          if (slot._playerId > 0L)
            ++num;
        }
        return num;
      }
    }

    private void BaseRemovePlayer(Account p)
    {
      lock (this._slots)
      {
        SLOT_MATCH slot;
        if (!this.getSlot(p.matchSlot, out slot) || slot._playerId != p.player_id)
          return;
        slot._playerId = 0L;
        slot.state = SlotMatchState.Empty;
      }
    }

    public bool RemovePlayer(Account p)
    {
      Channel channel = ChannelsXML.getChannel(this.channelId);
      if (channel == null)
        return false;
      this.BaseRemovePlayer(p);
      if (this.getCountPlayers() == 0)
      {
        channel.RemoveMatch(this._matchId);
      }
      else
      {
        if (p.matchSlot == this._leader)
          this.setNewLeader(-1, -1);
        using (CLAN_WAR_REGIST_MERCENARY_PAK registMercenaryPak = new CLAN_WAR_REGIST_MERCENARY_PAK(this))
          this.SendPacketToPlayers((SendPacket) registMercenaryPak);
      }
      p.matchSlot = -1;
      p._match = (Match) null;
      return true;
    }
  }
}
