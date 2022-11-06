
// Type: Core.models.room.VoteKick
// Assembly: Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 263E2FBF-1098-4552-8FB3-F6A660503737
// Interprise: C:\Users\Cuzin\3,50Core.dll

using System.Collections.Generic;

namespace Core.models.room
{
  public class VoteKick
  {
    public int creatorIdx;
    public int victimIdx;
    public int motive;
    public int kikar = 1;
    public int deixar = 1;
    public int allies;
    public int enemys;
    public List<int> _votes = new List<int>();
    public bool[] TotalArray = new bool[16];

    public VoteKick(int creator, int victim)
    {
      this.creatorIdx = creator;
      this.victimIdx = victim;
      this._votes.Add(creator);
      this._votes.Add(victim);
      if (creator % 2 == victim % 2)
        ++this.allies;
      else
        ++this.enemys;
    }

    public int GetInGamePlayers()
    {
      int num = 0;
      for (int index = 0; index < 16; ++index)
      {
        if (this.TotalArray[index])
          ++num;
      }
      return num;
    }
  }
}
