
// Type: Core.managers.events.PlayTimeModel
// Assembly: Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 263E2FBF-1098-4552-8FB3-F6A660503737
// Interprise: C:\Users\Cuzin\3,50Core.dll

using System;

namespace Core.managers.events
{
  public class PlayTimeModel
  {
    public uint _startDate;
    public uint _endDate;
    public string _title = "";
    public long _time;
    public int _goodReward1;
    public int _goodReward2;
    public int _goodCount1;
    public int _goodCount2;

    public bool EventIsEnabled()
    {
      uint num = uint.Parse(DateTime.Now.ToString("yyMMddHHmm"));
      return this._startDate <= num && num < this._endDate;
    }

    public int GetRewardCount(int goodId) => goodId == this._goodReward1 ? this._goodCount1 : (goodId == this._goodReward2 ? this._goodCount2 : 0);
  }
}
