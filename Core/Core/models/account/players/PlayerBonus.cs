
// Type: Core.models.account.players.PlayerBonus
// Assembly: Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 263E2FBF-1098-4552-8FB3-F6A660503737
// Interprise: C:\Users\Cuzin\3,50Core.dll

namespace Core.models.account.players
{
  public class PlayerBonus
  {
    public int bonuses;
    public int sightColor = 4;
    public int freepass;
    public int fakeRank = 55;
    public string fakeNick = "";
    public long ownerId;

    public bool RemoveBonuses(int itemId)
    {
      int bonuses = this.bonuses;
      int freepass = this.freepass;
      switch (itemId)
      {
        case 1200001000:
          this.Decrease(1);
          break;
        case 1200002000:
          this.Decrease(2);
          break;
        case 1200003000:
          this.Decrease(4);
          break;
        case 1200004000:
          this.Decrease(32);
          break;
        case 1200011000:
          this.freepass = 0;
          break;
        case 1200037000:
          this.Decrease(8);
          break;
        case 1200038000:
          this.Decrease(128);
          break;
        case 1200119000:
          this.Decrease(64);
          break;
      }
      return this.bonuses != bonuses || this.freepass != freepass;
    }

    public bool AddBonuses(int itemId)
    {
      int bonuses = this.bonuses;
      int freepass = this.freepass;
      switch (itemId)
      {
        case 1200001000:
          this.Increase(1);
          break;
        case 1200002000:
          this.Increase(2);
          break;
        case 1200003000:
          this.Increase(4);
          break;
        case 1200004000:
          this.Increase(32);
          break;
        case 1200011000:
          this.freepass = 1;
          break;
        case 1200037000:
          this.Increase(8);
          break;
        case 1200038000:
          this.Increase(128);
          break;
        case 1200119000:
          this.Increase(64);
          break;
      }
      return this.bonuses != bonuses || this.freepass != freepass;
    }

    private void Decrease(int value) => this.bonuses &= ~value;

    private void Increase(int value) => this.bonuses |= value;
  }
}
