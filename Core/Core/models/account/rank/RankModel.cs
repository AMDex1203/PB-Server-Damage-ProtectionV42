
// Type: Core.models.account.rank.RankModel
// Assembly: Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 263E2FBF-1098-4552-8FB3-F6A660503737
// Interprise: C:\Users\Cuzin\3,50Core.dll

namespace Core.models.account.rank
{
  public class RankModel
  {
    public int _id;
    public string _name;
    public int _onNextLevel;
    public int _onGPUp;
    public int _onAllExp;

    public RankModel(int rank, string name, int onNextLevel, int onGPUp, int onAllExp)
    {
      this._id = rank;
      this._name = name;
      this._onNextLevel = onNextLevel;
      this._onGPUp = onGPUp;
      this._onAllExp = onAllExp;
    }
  }
}
