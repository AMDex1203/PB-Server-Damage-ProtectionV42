
// Type: Core.models.account.mission.MisAwards
// Assembly: Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 263E2FBF-1098-4552-8FB3-F6A660503737
// Interprise: C:\Users\Cuzin\3,50Core.dll

namespace Core.models.account.mission
{
  public class MisAwards
  {
    public int _id;
    public int _blueOrder;
    public int _exp;
    public int _gp;

    public MisAwards(int id, int blueOrder, int exp, int gp)
    {
      this._id = id;
      this._blueOrder = blueOrder;
      this._exp = exp;
      this._gp = gp;
    }
  }
}
