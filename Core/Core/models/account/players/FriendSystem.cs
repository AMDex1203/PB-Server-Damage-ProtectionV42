﻿
// Type: Core.models.account.players.FriendSystem
// Assembly: Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 263E2FBF-1098-4552-8FB3-F6A660503737
// Interprise: C:\Users\Cuzin\3,50Core.dll

using System.Collections.Generic;

namespace Core.models.account.players
{
  public class FriendSystem
  {
    public List<Friend> _friends = new List<Friend>();
    public bool MemoryCleaned;

    public void CleanList()
    {
      lock (this._friends)
      {
        foreach (Friend friend in this._friends)
          friend.player = (PlayerInfo) null;
      }
      this.MemoryCleaned = true;
    }

    public void AddFriend(Friend friend)
    {
      lock (this._friends)
        this._friends.Add(friend);
    }

    public bool RemoveFriend(Friend friend)
    {
      lock (this._friends)
        return this._friends.Remove(friend);
    }

    public void RemoveFriend(int index)
    {
      lock (this._friends)
        this._friends.RemoveAt(index);
    }

    public void RemoveFriend(long id)
    {
      lock (this._friends)
      {
        for (int index = 0; index < this._friends.Count; ++index)
        {
          if (this._friends[index].player_id == id)
          {
            this._friends.RemoveAt(index);
            break;
          }
        }
      }
    }

    public int GetFriendIdx(long id)
    {
      lock (this._friends)
      {
        for (int index = 0; index < this._friends.Count; ++index)
        {
          if (this._friends[index].player_id == id)
            return index;
        }
      }
      return -1;
    }

    public Friend GetFriend(int idx)
    {
      lock (this._friends)
      {
        try
        {
          return this._friends[idx];
        }
        catch
        {
          return (Friend) null;
        }
      }
    }

    public Friend GetFriend(long id)
    {
      lock (this._friends)
      {
        for (int index = 0; index < this._friends.Count; ++index)
        {
          Friend friend = this._friends[index];
          if (friend.player_id == id)
            return friend;
        }
      }
      return (Friend) null;
    }

    public Friend GetFriend(long id, out int index)
    {
      lock (this._friends)
      {
        for (int index1 = 0; index1 < this._friends.Count; ++index1)
        {
          Friend friend = this._friends[index1];
          if (friend.player_id == id)
          {
            index = index1;
            return friend;
          }
        }
      }
      index = -1;
      return (Friend) null;
    }
  }
}
