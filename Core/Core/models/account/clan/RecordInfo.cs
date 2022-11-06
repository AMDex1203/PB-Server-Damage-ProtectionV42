
// Type: Core.models.account.clan.RecordInfo
// Assembly: Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 263E2FBF-1098-4552-8FB3-F6A660503737
// Interprise: C:\Users\Cuzin\3,50Core.dll

namespace Core.models.account.clan
{
  public class RecordInfo
  {
    public long PlayerId;
    public int RecordValue;

    public RecordInfo(string[] split)
    {
      this.PlayerId = this.GetPlayerId(split);
      this.RecordValue = this.GetPlayerValue(split);
    }

    public long GetPlayerId(string[] split)
    {
      try
      {
        return long.Parse(split[0]);
      }
      catch
      {
        return 0;
      }
    }

    public int GetPlayerValue(string[] split)
    {
      try
      {
        return int.Parse(split[1]);
      }
      catch
      {
        return 0;
      }
    }

    public string GetSplit() => this.PlayerId.ToString() + "-" + this.RecordValue.ToString();
  }
}
