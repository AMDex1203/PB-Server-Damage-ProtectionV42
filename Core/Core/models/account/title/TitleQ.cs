
// Type: Core.models.account.title.TitleQ
// Assembly: Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 263E2FBF-1098-4552-8FB3-F6A660503737
// Interprise: C:\Users\Cuzin\3,50Core.dll

namespace Core.models.account.title
{
  public class TitleQ
  {
    public int _id;
    public int _classId;
    public int _medals;
    public int _brooch;
    public int _blueOrder;
    public int _insignia;
    public int _rank;
    public int _slot;
    public int _req1;
    public int _req2;
    public long _flag;

    public TitleQ()
    {
    }

    public TitleQ(int titleId)
    {
      this._id = titleId;
      this._flag = 1L << titleId;
    }
  }
}
