
// Type: Core.models.account.players.PlayerStats
// Assembly: Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 263E2FBF-1098-4552-8FB3-F6A660503737
// Interprise: C:\Users\Cuzin\3,50Core.dll

using System;

namespace Core.models.account.players
{
  public class PlayerStats
  {
    public int fights;
    public int fights_win;
    public int fights_lost;
    public int fights_draw;
    public int kills_count;
    public int totalkills_count;
    public int totalfights_count;
    public int deaths_count;
    public int escapes;
    public int headshots_count;
    public int ClanGames;
    public int ClanWins;

    public int GetKDRatio() => this.headshots_count <= 0 && this.kills_count <= 0 ? 0 : (int) Math.Floor(((double) (this.kills_count * 100) + 0.5) / (double) (this.kills_count + this.deaths_count));

    public int GetHSRatio() => this.kills_count <= 0 ? 0 : (int) Math.Floor((double) (this.headshots_count * 100) / (double) this.kills_count + 0.5);
  }
}
