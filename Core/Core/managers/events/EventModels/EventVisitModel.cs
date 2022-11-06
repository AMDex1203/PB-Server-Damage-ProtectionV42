
// Type: Core.managers.events.EventModels.EventVisitModel
// Assembly: Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 263E2FBF-1098-4552-8FB3-F6A660503737
// Interprise: C:\Users\Cuzin\3,50Core.dll

using Core.models.account;
using System;
using System.Collections.Generic;

namespace Core.managers.events.EventModels
{
  public class EventVisitModel
  {
    public int id;
    public byte checks = 7;
    public uint startDate;
    public uint endDate;
    public string title = "";
    public string _goods1 = "";
    public string _counts1 = "";
    public string _goods2 = "";
    public string _counts2 = "";
    public List<VisitBox> box = new List<VisitBox>();

    public EventVisitModel()
    {
      for (int index = 0; index < 7; ++index)
        this.box.Add(new VisitBox());
    }

    public bool EventIsEnabled()
    {
      uint num = uint.Parse(DateTime.Now.ToString("yyMMddHHmm"));
      return this.startDate <= num && num < this.endDate;
    }

    public VisitItem getReward(int idx, int rewardIdx)
    {
      try
      {
        return rewardIdx == 0 ? this.box[idx].reward1 : this.box[idx].reward2;
      }
      catch
      {
        return (VisitItem) null;
      }
    }

    public void SetBoxCounts()
    {
      for (int index = 0; index < 7; ++index)
        this.box[index].SetCount();
    }
  }
}
