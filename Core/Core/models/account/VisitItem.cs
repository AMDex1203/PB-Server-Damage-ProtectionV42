
// Type: Core.models.account.VisitItem
// Assembly: Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 263E2FBF-1098-4552-8FB3-F6A660503737
// Interprise: C:\Users\Cuzin\3,50Core.dll

namespace Core.models.account
{
  public class VisitItem
  {
    public int goodId;
    public int count;
    public bool IsReward;

    public void SetCount(string text)
    {
      this.count = int.Parse(text);
      if (this.count <= 0)
        return;
      this.IsReward = true;
    }
  }
}
