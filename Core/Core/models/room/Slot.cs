
// Type: Core.models.room.Slot
// Assembly: Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 263E2FBF-1098-4552-8FB3-F6A660503737
// Interprise: C:\Users\Cuzin\3,50Core.dll

using Core.models.account.players;
using Core.models.enums;
using Core.models.enums.flags;
using Core.models.enums.item;
using PointBlank;
using System;
using System.Collections.Generic;
using System.Net;

namespace Core.models.room
{
  public class Slot
  {
    public DateTime lastTick = new DateTime();
    public int TICKS = 0;
    public SLOT_STATE state;
    public IPEndPoint client;
    public float PartidaTime;
    public ResultIcon bonusFlags;
    public PlayerEquipedItems _equip;
    public long _playerId;
    public DeadEnum _deathState = DeadEnum.isAlive;
    public bool firstRespawn = true;
    public bool repeatLastState;
    public bool check;
    public bool espectador;
    public bool specGM;
    public bool withHost;
    public int _id;
    public int aiLevel;
    public int latency;
    public int failLatencyTimes;
    public int ping;
    public int passSequence;
    public int isPlaying;
    public int earnedXP;
    public int spawnsCount;
    public int headshots;
    public int lastKillState;
    public int killsOnLife;
    public int exp;
    public int money;
    public int gp;
    public int Score;
    public int allKills;
    public int allDeaths;
    public int objetivos;
    public int BonusXP;
    public int BonusGP;
    public int unkItem;
    public DateTime NextVoteDate;
    public DateTime startTime;
    public DateTime preStartDate;
    public DateTime preLoadDate;
    public ushort damageBar1;
    public ushort damageBar2;
    public int PlayerIdRegister;
    public ClassType weaponClass = ClassType.Unknown;
    public List<int> armas_usadas = new List<int>();
    public bool MissionsCompleted;
    public PlayerMissions Missions;
    public Core.server.TimerState timing = new Core.server.TimerState();
    public List<int> EquipmentsUsed = new List<int>();
    public int life = 100;
    public int maxLife = 100;
    public float plantDuration;
    public float defuseDuration;
    public float C4FTime;
    public bool isDead = true;
    public bool TRexImmortal;
    public Half3 position;
    public IPEndPoint _client;
    public DateTime pingDate;
    public DateTime lastDie;
    public DateTime C4First;

    public void StopTiming()
    {
      if (this.timing == null)
        return;
      this.timing.Timer = (System.Threading.Timer) null;
    }

    public Slot(int slotIdx) => this.SetSlotId(slotIdx);

    public void SetSlotId(int slotIdx) => this._id = slotIdx;

    public bool AccountIdIsValid(int number) => this.PlayerIdRegister == number;

    public int _team => this._id % 2;

    public int _flag => 1 << this._id;

    public void ResetSlot()
    {
      this.repeatLastState = false;
      this._deathState = DeadEnum.isAlive;
      this.StopTiming();
      this.check = false;
      this.espectador = false;
      this.specGM = false;
      this.withHost = false;
      this.firstRespawn = true;
      this.failLatencyTimes = 0;
      this.latency = 0;
      this.ping = 0;
      this.passSequence = 0;
      this.allDeaths = 0;
      this.allKills = 0;
      this.bonusFlags = ResultIcon.None;
      this.killsOnLife = 0;
      this.lastKillState = 0;
      this.Score = 0;
      this.gp = 0;
      this.exp = 0;
      this.headshots = 0;
      this.objetivos = 0;
      this.BonusGP = 0;
      this.BonusXP = 0;
      this.spawnsCount = 0;
      this.damageBar1 = (ushort) 0;
      this.damageBar2 = (ushort) 0;
      this.earnedXP = 0;
      this.isPlaying = 0;
      this.money = 0;
      this.NextVoteDate = new DateTime();
      this.aiLevel = 0;
      this.armas_usadas.Clear();
      this.MissionsCompleted = false;
      this.Missions = (PlayerMissions) null;
    }

    public void SetMissionsClone(PlayerMissions missions)
    {
      this.Missions = missions.DeepCopy();
      this.MissionsCompleted = false;
    }

    public double inBattleTime(DateTime date) => this.startTime == new DateTime() ? 0.0 : (date - this.startTime).TotalSeconds;
  }
}
