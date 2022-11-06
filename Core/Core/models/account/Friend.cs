
// Type: Core.models.account.Friend
// Assembly: Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 263E2FBF-1098-4552-8FB3-F6A660503737
// Interprise: C:\Users\Cuzin\3,50Core.dll

using Core.models.account.players;

namespace Core.models.account
{
  public class Friend
  {
    public long player_id;
    public int state;
    public bool removed;
    public PlayerInfo player;

    public Friend(long player_id)
    {
      this.player_id = player_id;
      this.player = new PlayerInfo(player_id);
    }

    public Friend(long player_id, int rank, string name, bool isOnline, AccountStatus status)
    {
      this.player_id = player_id;
      this.SetModel(player_id, rank, name, isOnline, status);
    }

    public void SetModel(
      long player_id,
      int rank,
      string name,
      bool isOnline,
      AccountStatus status)
    {
      this.player = new PlayerInfo(player_id, rank, name, isOnline, status);
    }
  }
}
