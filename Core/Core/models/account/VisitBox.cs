
// Type: Core.models.account.VisitBox
// Assembly: Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 263E2FBF-1098-4552-8FB3-F6A660503737
// Interprise: C:\Users\Cuzin\3,50Core.dll

namespace Core.models.account
{
  public class VisitBox
  {
    public VisitItem reward1;
    public VisitItem reward2;
    public int RewardCount;

    public VisitBox()
    {
      this.reward1 = new VisitItem();
      this.reward2 = new VisitItem();
    }

    public void SetCount()
    {
      if (this.reward1 != null && this.reward1.goodId > 0)
        ++this.RewardCount;
      if (this.reward2 == null || this.reward2.goodId <= 0)
        return;
      ++this.RewardCount;
    }
  }
}
