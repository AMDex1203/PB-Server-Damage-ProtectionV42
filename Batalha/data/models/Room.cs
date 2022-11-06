
// Type: Battle.data.models.Room
// Assembly: pbserver_battle, Version=1.0.0.2646, Culture=neutral, PublicKeyToken=null
// MVID: AA2D7911-B687-4238-AB73-D37E54123039
// Interprise: C:\Users\Cuzin\3,50pbserver_battle.exe

using Battle.config;
using Battle.data.xml;
using Battle.network;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Net;

namespace Battle.data.models
{
  public class Room
  {
    public Player[] _players = new Player[16];
    public ObjectInfo[] _objects = new ObjectInfo[200];
    public DateTime LastObjsSync;
    public DateTime LastPlayersSync;
    public uint UniqueRoomId;
    public int _objsSyncRound;
    public int _serverRound;
    public int _genId2;
    public int _sourceToMap = -1;
    public int _serverId;
    public int _mapId;
    public int stageType;
    public int _roomId;
    public int _channelId;
    public int LastRound;
    public int _dropCounter;
    public int _bar1 = 6000;
    public int _bar2 = 6000;
    public int _default1 = 6000;
    public int _default2 = 6000;
    public GameServerModel gs;
    public MapModel Map;
    private object _lock = new object();
    private object _lock2 = new object();
    public bool _isBotMode;
    public bool _hasC4;
    public long LastStartTick;
    public Half3 BombPosition;
    public DateTime _startTime;

    public Room(int serverId)
    {
      this.gs = ServersXML.getServer(serverId);
      if (this.gs == null)
        return;
      this._serverId = serverId;
      for (int slot = 0; slot < 16; ++slot)
        this._players[slot] = new Player(slot);
      for (int id = 0; id < 200; ++id)
        this._objects[id] = new ObjectInfo(id);
    }

    public void SyncInfo(List<ObjectHitInfo> objs, int type)
    {
      lock (this._lock2)
      {
        if (this._isBotMode || !this.ObjectsIsValid())
          return;
        DateTime now = DateTime.Now;
        double totalSeconds1 = (now - this.LastObjsSync).TotalSeconds;
        double totalSeconds2 = (now - this.LastPlayersSync).TotalSeconds;
        if (totalSeconds1 >= 2.5 && (type & 1) == 1)
        {
          this.LastObjsSync = now;
          for (int index = 0; index < this._objects.Length; ++index)
          {
            ObjectInfo objectInfo = this._objects[index];
            ObjModel model = objectInfo._model;
            if (model != null && (model.isDestroyable && objectInfo._life != model._life || model._needSync))
            {
              float duration = AllUtils.GetDuration(objectInfo._useDate);
              AnimModel anim = objectInfo._anim;
              if (anim != null && (double) anim._duration > 0.0 && (double) duration >= (double) anim._duration)
                model.GetAnim(anim._nextAnim, duration, anim._duration, objectInfo);
              objs.Add(new ObjectHitInfo(model._updateId)
              {
                objSyncId = model._needSync ? 1 : 0,
                _animId1 = model._anim1,
                _animId2 = objectInfo._anim != null ? objectInfo._anim._id : (int) byte.MaxValue,
                _destroyState = objectInfo.DestroyState,
                objId = model._id,
                objLife = objectInfo._life,
                _specialUse = duration
              });
            }
          }
        }
        if (totalSeconds2 < 6.5 || (type & 2) != 2)
          return;
        this.LastPlayersSync = now;
        for (int index = 0; index < this._players.Length; ++index)
        {
          Player player = this._players[index];
          if (!player.Immortal && (player._maxLife != player._life || player.isDead))
            objs.Add(new ObjectHitInfo(4)
            {
              objId = player._slot,
              objLife = player._life
            });
        }
      }
    }

    public bool RoundIsValid(int number) => this.LastRound == number || this.LastRound + 1 == number;

    public bool ObjectsIsValid() => this._serverRound == this._objsSyncRound;

    public void ResyncTick(long startTick, int gen2)
    {
      if (startTick <= this.LastStartTick)
        return;
      this._startTime = new DateTime(startTick);
      if (this.LastStartTick > 0L)
        this.ResetRoomInfo(gen2);
      this.LastStartTick = startTick;
      if (Config.isTestMode)
        Logger.warning("[New tick is defined]");
    }

    public void ResetRoomInfo(int gen2)
    {
      for (int id = 0; id < 200; ++id)
        this._objects[id] = new ObjectInfo(id);
      this._mapId = RoomsManager.getGenV(gen2, 1);
      this.stageType = RoomsManager.getGenV(gen2, 2);
      this._sourceToMap = -1;
      this.Map = (MapModel) null;
      this.LastRound = 0;
      this._dropCounter = 0;
      this._isBotMode = false;
      this._hasC4 = false;
      this._serverRound = 0;
      this._objsSyncRound = 0;
      this.LastObjsSync = new DateTime();
      this.LastPlayersSync = new DateTime();
      this.BombPosition = new Half3();
      if (!Config.isTestMode)
        return;
      Logger.warning("A room has been reseted by server.");
    }

    public bool RoundResetRoomF1(int round)
    {
      lock (this._lock)
      {
        if (this.LastRound != round)
        {
          if (Config.isTestMode)
            Logger.warning("Reseting room. [Last: " + this.LastRound.ToString() + "; New: " + round.ToString() + "]");
          DateTime now = DateTime.Now;
          this.LastRound = round;
          this._hasC4 = false;
          this.BombPosition = new Half3();
          this._dropCounter = 0;
          this._objsSyncRound = 0;
          this._sourceToMap = this._mapId;
          if (!this._isBotMode)
          {
            for (int index = 0; index < 16; ++index)
            {
              Player player = this._players[index];
              player._life = player._maxLife;
            }
            this.LastPlayersSync = now;
            this.Map = MappingXML.getMapById(this._mapId);
            List<ObjModel> objModelList = this.Map != null ? this.Map._objects : (List<ObjModel>) null;
            if (objModelList != null)
            {
              for (int index = 0; index < objModelList.Count; ++index)
              {
                ObjModel objModel = objModelList[index];
                ObjectInfo objectInfo = this._objects[objModel._id];
                objectInfo._life = objModel._life;
                if (!objModel._noInstaSync)
                {
                  objModel.GetARandomAnim(this, objectInfo);
                }
                else
                {
                  objectInfo._anim = new AnimModel()
                  {
                    _nextAnim = 1
                  };
                  objectInfo._useDate = now;
                }
                objectInfo._model = objModel;
                objectInfo.DestroyState = 0;
                MappingXML.SetObjectives(objModel, this);
              }
            }
            this.LastObjsSync = now;
            this._objsSyncRound = round;
          }
          return true;
        }
      }
      return false;
    }

    public bool RoundResetRoomS1(int round)
    {
      lock (this._lock)
      {
        if (this.LastRound != round)
        {
          if (Config.isTestMode)
            Logger.warning("Reseting room. [Last: " + this.LastRound.ToString() + "; New: " + round.ToString() + "]");
          this.LastRound = round;
          this._hasC4 = false;
          this._dropCounter = 0;
          this.BombPosition = new Half3();
          if (!this._isBotMode)
          {
            for (int index = 0; index < 16; ++index)
            {
              Player player = this._players[index];
              player._life = player._maxLife;
            }
            DateTime now = DateTime.Now;
            this.LastPlayersSync = now;
            for (int index = 0; index < this._objects.Length; ++index)
            {
              ObjectInfo objectInfo = this._objects[index];
              ObjModel model = objectInfo._model;
              if (model != null)
              {
                objectInfo._life = model._life;
                if (!model._noInstaSync)
                {
                  model.GetARandomAnim(this, objectInfo);
                }
                else
                {
                  objectInfo._anim = new AnimModel()
                  {
                    _nextAnim = 1
                  };
                  objectInfo._useDate = now;
                }
                objectInfo.DestroyState = 0;
              }
            }
            this.LastObjsSync = now;
            this._objsSyncRound = round;
            if (this.stageType == 3 || this.stageType == 5)
            {
              this._bar1 = this._default1;
              this._bar2 = this._default2;
            }
          }
          return true;
        }
      }
      return false;
    }

    public Player AddPlayer(IPEndPoint client, PacketModel packet, string udp)
    {
      if (Config.udpVersion != udp)
        return (Player) null;
      try
      {
        Player player = this._players[packet._slot];
        if (!player.CompareIP(client))
        {
          player._client = client;
          player._date = packet._receiveDate;
          player._playerIdByUser = packet._accountId;
          return player;
        }
      }
      catch (Exception ex)
      {
        Logger.warning(ex.ToString());
      }
      return (Player) null;
    }

    public bool getPlayer(int slot, out Player p)
    {
      try
      {
        p = this._players[slot];
      }
      catch
      {
        p = (Player) null;
      }
      return p != null;
    }

    public Player getPlayer(int slot, bool isActive)
    {
      Player player;
      try
      {
        player = this._players[slot];
      }
      catch
      {
        player = (Player) null;
      }
      return player == null || isActive && player._client == null ? (Player) null : player;
    }

    public Player getPlayer(int slot, IPEndPoint client)
    {
      Player player;
      try
      {
        player = this._players[slot];
      }
      catch
      {
        player = (Player) null;
      }
      return player == null || !player.CompareIP(client) ? (Player) null : player;
    }

    public ObjectInfo getObject(int id)
    {
      ObjectInfo objectInfo;
      try
      {
        objectInfo = this._objects[id];
      }
      catch
      {
        objectInfo = (ObjectInfo) null;
      }
      return objectInfo;
    }

    public Player getPlayer(IPEndPoint ip)
    {
      try
      {
        foreach (Player player in this._players)
        {
          if (player.CompareIP(ip))
            return player;
        }
        return (Player) null;
      }
      catch
      {
        return (Player) null;
      }
    }

    public bool RemovePlayer(int slot, IPEndPoint ip)
    {
      try
      {
        Player player = this._players[slot];
        if (!player.CompareIP(ip))
          return false;
        player.ResetAllInfos();
        return true;
      }
      catch
      {
        return false;
      }
    }

    public int getPlayersCount()
    {
      int num = 0;
      for (int index = 0; index < 16; ++index)
      {
        if (this._players[index]._client != null)
          ++num;
      }
      return num;
    }
  }
}
