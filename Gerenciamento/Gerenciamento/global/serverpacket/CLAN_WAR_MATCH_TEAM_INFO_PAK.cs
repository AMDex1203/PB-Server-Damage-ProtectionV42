
// Type: Game.global.serverpacket.CLAN_WAR_MATCH_TEAM_INFO_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.managers;
using Core.models.account.clan;
using Core.server;
using Game.data.managers;
using Game.data.model;

namespace Game.global.serverpacket
{
  public class CLAN_WAR_MATCH_TEAM_INFO_PAK : SendPacket
  {
    private uint _erro;
    private Clan c;
    private Account leader;

    public CLAN_WAR_MATCH_TEAM_INFO_PAK(uint erro, Clan c)
    {
      this._erro = erro;
      this.c = c;
      if (this.c == null)
        return;
      this.leader = AccountManager.getAccount(this.c.ownerId, 0);
      if (this.leader != null)
        return;
      this._erro = 2147483648U;
    }

    public CLAN_WAR_MATCH_TEAM_INFO_PAK(uint erro) => this._erro = erro;

    public override void write()
    {
      this.writeH((short) 1570);
      this.writeD(this._erro);
      if (this._erro != 0U)
        return;
      int clanPlayers = PlayerManager.getClanPlayers(this.c.id);
      this.writeD(this.c.id);
      this.writeS(this.c.name, 17);
      this.writeC(this.c.rank);
      this.writeC((byte) clanPlayers);
      this.writeC((byte) this.c.maxPlayers);
      this.writeD(this.c.creationDate);
      this.writeD(this.c.logo);
      this.writeC(this.c.nameColor);
      this.writeC((byte) this.c.getClanUnit(clanPlayers));
      this.writeD(this.c.exp);
      this.writeD(0);
      this.writeQ(this.c.ownerId);
      this.writeS(this.leader.player_name, 33);
      this.writeC((byte) this.leader._rank);
      this.writeS("", (int) byte.MaxValue);
    }
  }
}
