
// Type: Game.global.serverpacket.CLAN_DETAIL_INFO_PAK
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
  public class CLAN_DETAIL_INFO_PAK : SendPacket
  {
    private Clan clan;
    private int _erro;

    public CLAN_DETAIL_INFO_PAK(int erro, Clan c)
    {
      this._erro = erro;
      this.clan = c;
    }

    public override void write()
    {
      Account account = AccountManager.getAccount(this.clan.ownerId, 0);
      int clanPlayers = PlayerManager.getClanPlayers(this.clan.id);
      this.writeH((short) 1305);
      this.writeD(this._erro);
      this.writeD(this.clan.id);
      this.writeS(this.clan.name, 17);
      this.writeC(this.clan.rank);
      this.writeC((byte) clanPlayers);
      this.writeC((byte) this.clan.maxPlayers);
      this.writeD(this.clan.creationDate);
      this.writeD(this.clan.logo);
      this.writeC(this.clan.nameColor);
      this.writeC((byte) this.clan.getClanUnit());
      this.writeD(this.clan.exp);
      this.writeD(10);
      this.writeQ(this.clan.ownerId);
      this.writeS(account != null ? account.player_name : "", 33);
      this.writeC(account != null ? (byte) account._rank : (byte) 0);
      this.writeS(this.clan.informations, (int) byte.MaxValue);
      this.writeS("Temp", 21);
      this.writeC((byte) this.clan.limitRankId);
      this.writeC((byte) this.clan.limitAgeBigger);
      this.writeC((byte) this.clan.limitAgeSmaller);
      this.writeC((byte) this.clan.autoridade);
      this.writeS(this.clan.notice, (int) byte.MaxValue);
      this.writeD(this.clan.partidas);
      this.writeD(this.clan.vitorias);
      this.writeD(this.clan.derrotas);
      this.writeD(this.clan.partidas);
      this.writeD(this.clan.vitorias);
      this.writeD(this.clan.derrotas);
      this.writeQ(this.clan.BestPlayers.Exp.PlayerId);
      this.writeQ(this.clan.BestPlayers.Exp.PlayerId);
      this.writeQ(this.clan.BestPlayers.Wins.PlayerId);
      this.writeQ(this.clan.BestPlayers.Wins.PlayerId);
      this.writeQ(this.clan.BestPlayers.Kills.PlayerId);
      this.writeQ(this.clan.BestPlayers.Kills.PlayerId);
      this.writeQ(this.clan.BestPlayers.Headshot.PlayerId);
      this.writeQ(this.clan.BestPlayers.Headshot.PlayerId);
      this.writeQ(this.clan.BestPlayers.Participation.PlayerId);
      this.writeQ(this.clan.BestPlayers.Participation.PlayerId);
      this.writeT(this.clan.pontos);
    }
  }
}
