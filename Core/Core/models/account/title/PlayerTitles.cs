
// Type: Core.models.account.title.PlayerTitles
// Assembly: Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 263E2FBF-1098-4552-8FB3-F6A660503737
// Interprise: C:\Users\Cuzin\3,50Core.dll

namespace Core.models.account.title
{
  public class PlayerTitles
  {
    public int Equiped1;
    public int Equiped2;
    public int Equiped3;
    public int Slots = 1;
    public long ownerId;
    public long Flags;

    public long Add(long flag)
    {
      this.Flags |= flag;
      return this.Flags;
    }

    public bool Contains(long flag) => (this.Flags & flag) == flag || flag == 0L;

    public void SetEquip(int index, int value)
    {
      switch (index)
      {
        case 0:
          this.Equiped1 = value;
          break;
        case 1:
          this.Equiped2 = value;
          break;
        case 2:
          this.Equiped3 = value;
          break;
      }
    }

    public int GetEquip(int index)
    {
      switch (index)
      {
        case 0:
          return this.Equiped1;
        case 1:
          return this.Equiped2;
        case 2:
          return this.Equiped3;
        default:
          return 0;
      }
    }
  }
}
