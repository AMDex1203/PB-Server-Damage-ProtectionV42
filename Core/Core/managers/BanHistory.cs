
// Type: Core.managers.BanHistory
// Assembly: Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 263E2FBF-1098-4552-8FB3-F6A660503737
// Interprise: C:\Users\Cuzin\3,50Core.dll

using System;

namespace Core.managers
{
  public class BanHistory
  {
    public long object_id;
    public long provider_id;
    public string type;
    public string value;
    public string reason;
    public DateTime startDate;
    public DateTime endDate;

    public BanHistory()
    {
      this.startDate = DateTime.Now;
      this.type = "";
      this.value = "";
      this.reason = "";
    }
  }
}
