
// Type: Core.server.TimerState
// Assembly: Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 263E2FBF-1098-4552-8FB3-F6A660503737
// Interprise: C:\Users\Cuzin\3,50Core.dll

using System;
using System.Threading;

namespace Core.server
{
  public class TimerState
  {
    public Timer Timer = (Timer) null;
    public DateTime EndDate = new DateTime();
    private object sync = new object();

    public void StartJob(int period, TimerCallback callback)
    {
      lock (this.sync)
      {
        this.Timer = new Timer(callback, (object) this, period, -1);
        this.EndDate = DateTime.Now.AddMilliseconds((double) period);
      }
    }

    public int getTimeLeft()
    {
      if (this.Timer == null)
        return 0;
      int totalSeconds = (int) (this.EndDate - DateTime.Now).TotalSeconds;
      return totalSeconds < 0 ? 0 : totalSeconds;
    }

    public void StartTimer(TimeSpan period, TimerCallback callback)
    {
      lock (this.sync)
        this.Timer = new Timer(callback, (object) this, period, TimeSpan.Zero);
    }
  }
}
