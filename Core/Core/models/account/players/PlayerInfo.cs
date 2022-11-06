
// Type: Core.models.account.players.PlayerInfo
// Assembly: Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 263E2FBF-1098-4552-8FB3-F6A660503737
// Interprise: C:\Users\Cuzin\3,50Core.dll

using Core.server;

namespace Core.models.account.players
{
  public class PlayerInfo
  {
    public int _rank;
    private long player_id;
    public string player_name;
    public bool _isOnline;
    public AccountStatus _status;

    public PlayerInfo(long player_id)
    {
      this.player_id = player_id;
      this._status = new AccountStatus();
    }

    public PlayerInfo(long player_id, int rank, string name, bool isOnline, AccountStatus status)
    {
      this.player_id = player_id;
      this.SetInfo(rank, name, isOnline, status);
    }

    public void setOnlineStatus(bool state)
    {
      if (this._isOnline == state || !ComDiv.updateDB("accounts", "online", (object) state, "player_id", (object) this.player_id))
        return;
      this._isOnline = state;
    }

    public void SetInfo(int rank, string name, bool isOnline, AccountStatus status)
    {
      this._rank = rank;
      this.player_name = name;
      this._isOnline = isOnline;
      this._status = status;
    }
  }
}
