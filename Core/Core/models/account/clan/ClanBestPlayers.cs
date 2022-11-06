
// Type: Core.models.account.clan.ClanBestPlayers
// Assembly: Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 263E2FBF-1098-4552-8FB3-F6A660503737
// Interprise: C:\Users\Cuzin\3,50Core.dll

using Core.models.account.players;
using Core.models.room;
using Core.server;

namespace Core.models.account.clan
{
  public class ClanBestPlayers
  {
    public RecordInfo Exp;
    public RecordInfo Participation;
    public RecordInfo Wins;
    public RecordInfo Kills;
    public RecordInfo Headshot;

    public void SetPlayers(string Exp, string Part, string Wins, string Kills, string Hs)
    {
      string[] split1 = Exp.Split('-');
      string[] split2 = Part.Split('-');
      string[] split3 = Wins.Split('-');
      string[] split4 = Kills.Split('-');
      string[] split5 = Hs.Split('-');
      this.Exp = new RecordInfo(split1);
      this.Participation = new RecordInfo(split2);
      this.Wins = new RecordInfo(split3);
      this.Kills = new RecordInfo(split4);
      this.Headshot = new RecordInfo(split5);
    }

    public void SetDefault()
    {
      string[] split = new string[2]{ "0", "0" };
      this.Exp = new RecordInfo(split);
      this.Participation = new RecordInfo(split);
      this.Wins = new RecordInfo(split);
      this.Kills = new RecordInfo(split);
      this.Headshot = new RecordInfo(split);
    }

    public long GetPlayerId(string[] split)
    {
      try
      {
        return long.Parse(split[0]);
      }
      catch
      {
        return 0;
      }
    }

    public int GetPlayerValue(string[] split)
    {
      try
      {
        return int.Parse(split[1]);
      }
      catch
      {
        return 0;
      }
    }

    public void SetBestExp(Slot slot)
    {
      if (slot.exp <= this.Exp.RecordValue)
        return;
      this.Exp.PlayerId = slot._playerId;
      this.Exp.RecordValue = slot.exp;
    }

    public void SetBestHeadshot(Slot slot)
    {
      if (slot.headshots <= this.Headshot.RecordValue)
        return;
      this.Headshot.PlayerId = slot._playerId;
      this.Headshot.RecordValue = slot.headshots;
    }

    public void SetBestKills(Slot slot)
    {
      if (slot.allKills <= this.Kills.RecordValue)
        return;
      this.Kills.PlayerId = slot._playerId;
      this.Kills.RecordValue = slot.allKills;
    }

    public void SetBestWins(PlayerStats stats, Slot slot, bool WonTheMatch)
    {
      if (!WonTheMatch)
        return;
      ComDiv.updateDB("accounts", "clan_wins_pt", (object) ++stats.ClanWins, "player_id", (object) slot._playerId);
      if (stats.ClanWins <= this.Wins.RecordValue)
        return;
      this.Wins.PlayerId = slot._playerId;
      this.Wins.RecordValue = stats.ClanWins;
    }

    public void SetBestParticipation(PlayerStats stats, Slot slot)
    {
      ComDiv.updateDB("accounts", "clan_game_pt", (object) ++stats.ClanGames, "player_id", (object) slot._playerId);
      if (stats.ClanGames <= this.Participation.RecordValue)
        return;
      this.Participation.PlayerId = slot._playerId;
      this.Participation.RecordValue = stats.ClanGames;
    }
  }
}
