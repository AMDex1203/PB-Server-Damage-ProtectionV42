
// Type: Core.models.account.mission.CardAwards
// Assembly: Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 263E2FBF-1098-4552-8FB3-F6A660503737
// Interprise: C:\Users\Cuzin\3,50Core.dll

namespace Core.models.account.mission
{
  public class CardAwards
  {
    public int _id;
    public int _card;
    public int _insignia;
    public int _medal;
    public int _brooch;
    public int _exp;
    public int _gp;

    public bool Unusable() => this._insignia == 0 && this._medal == 0 && (this._brooch == 0 && this._exp == 0) && this._gp == 0;
  }
}
