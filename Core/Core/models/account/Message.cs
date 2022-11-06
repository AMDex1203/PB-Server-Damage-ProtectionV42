
// Type: Core.models.account.Message
// Assembly: Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 263E2FBF-1098-4552-8FB3-F6A660503737
// Interprise: C:\Users\Cuzin\3,50Core.dll

using Core.models.enums;
using System;
using System.Globalization;

namespace Core.models.account
{
  public class Message
  {
    public int object_id;
    public int clanId;
    public int type;
    public int state;
    public long sender_id;
    public long expireDate;
    public string sender_name = "";
    public string text = "";
    public NoteMessageClan cB;
    public int DaysRemaining;

    public Message()
    {
    }

    public Message(long expire, DateTime start)
    {
      this.expireDate = expire;
      this.SetDaysRemaining(start);
    }

    public Message(double days)
    {
      DateTime end = DateTime.Now.AddDays(days);
      this.expireDate = long.Parse(end.ToString("yyMMddHHmm"));
      this.SetDaysRemaining(end, DateTime.Now);
    }

    private void SetDaysRemaining(DateTime now) => this.SetDaysRemaining(DateTime.ParseExact(this.expireDate.ToString(), "yyMMddHHmm", (IFormatProvider) CultureInfo.InvariantCulture), now);

    private void SetDaysRemaining(DateTime end, DateTime now)
    {
      int num = (int) Math.Ceiling((end - now).TotalDays);
      this.DaysRemaining = num < 0 ? 0 : num;
    }
  }
}
