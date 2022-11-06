
// Type: Auth.data.sync.update.ClanInfo
// Assembly: pbserver_auth, Version=1.0.7796.38604, Culture=neutral, PublicKeyToken=null
// MVID: 1CFDFF97-782C-4EF8-A621-6E4FDDE32436
// Interprise: C:\Users\Cuzin\3,50pbserver_auth.exe

using Auth.data.model;

namespace Auth.data.sync.update
{
  public class ClanInfo
  {
    public static void AddMember(Account player, Account member)
    {
      lock (player._clanPlayers)
        player._clanPlayers.Add(member);
    }

    public static void RemoveMember(Account player, long id)
    {
      lock (player._clanPlayers)
      {
        for (int index = 0; index < player._clanPlayers.Count; ++index)
        {
          if (player._clanPlayers[index].player_id == id)
          {
            player._clanPlayers.RemoveAt(index);
            break;
          }
        }
      }
    }

    public static void ClearList(Account p)
    {
      lock (p._clanPlayers)
      {
        p.clan_id = 0;
        p.clanAccess = 0;
        p._clanPlayers.Clear();
      }
    }
  }
}
