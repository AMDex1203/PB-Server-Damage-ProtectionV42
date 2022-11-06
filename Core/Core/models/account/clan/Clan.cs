
// Type: Core.models.account.clan.Clan
// Assembly: Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 263E2FBF-1098-4552-8FB3-F6A660503737
// Interprise: C:\Users\Cuzin\3,50Core.dll

using Core.managers;

namespace Core.models.account.clan
{
  public class Clan
  {
    public int id = 0;
    public int creationDate;
    public int partidas;
    public int vitorias;
    public int derrotas;
    public int autoridade;
    public int limitRankId;
    public int limitAgeBigger;
    public int limitAgeSmaller;
    public int exp;
    public byte rank;
    public byte nameColor;
    public int maxPlayers = 50;
    public string name = "";
    public string informations = "";
    public string notice = "";
    public long ownerId;
    public uint logo = uint.MaxValue;
    public float pontos = 1000f;
    public ClanBestPlayers BestPlayers = new ClanBestPlayers();

    public int getClanUnit() => this.getClanUnit(PlayerManager.getClanPlayers(this.id));

    public int getClanUnit(int count)
    {
      if (count >= 250)
        return 7;
      if (count >= 200)
        return 6;
      if (count >= 150)
        return 5;
      if (count >= 100)
        return 4;
      if (count >= 50)
        return 3;
      if (count >= 30)
        return 2;
      return count >= 10 ? 1 : 0;
    }
  }
}
