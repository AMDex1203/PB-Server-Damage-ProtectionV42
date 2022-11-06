
// Type: Core.models.room.FragInfos
// Assembly: Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 263E2FBF-1098-4552-8FB3-F6A660503737
// Interprise: C:\Users\Cuzin\3,50Core.dll

using Core.models.enums.room;
using System;
using System.Collections.Generic;

namespace Core.models.room
{
  public class FragInfos
  {
    public byte killerIdx;
    public byte killsCount;
    public byte flag;
    public CharaKillType killingType;
    public int weapon;
    public int Score;
    public float x;
    public float y;
    public float z;
    public List<Frag> frags = new List<Frag>();

    public KillingMessage GetAllKillFlags()
    {
      KillingMessage killingMessage = (KillingMessage) 0;
      for (int index = 0; index < this.frags.Count; ++index)
      {
        Frag frag = this.frags[index];
        if (!killingMessage.HasFlag((Enum) frag.killFlag))
          killingMessage |= frag.killFlag;
      }
      return killingMessage;
    }
  }
}
