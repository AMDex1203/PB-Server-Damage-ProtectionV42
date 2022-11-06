
// Type: Game.global.serverpacket.CLAN_CREATE_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.managers;
using Core.models.account.clan;
using Core.server;
using Game.data.model;

namespace Game.global.serverpacket
{
  public class CLAN_CREATE_PAK : SendPacket
  {
    private Account _p;
    private Clan clan;
    private uint _erro;

    public CLAN_CREATE_PAK(uint erro, Clan clan, Account player)
    {
      this._erro = erro;
      this.clan = clan;
      this._p = player;
    }

    public override void write()
    {
      this.writeH((short) 1311);
      this.writeD(this._erro);
      if (this._erro != 0U)
        return;
      this.writeD(this.clan.id);
      this.writeS(this.clan.name, 17);
      this.writeC(this.clan.rank);
      this.writeC((byte) PlayerManager.getClanPlayers(this.clan.id));
      this.writeC((byte) this.clan.maxPlayers);
      this.writeD(this.clan.creationDate);
      this.writeD(this.clan.logo);
      this.writeB(new byte[10]);
      this.writeQ(this.clan.ownerId);
      this.writeS(this._p.player_name, 33);
      this.writeC((byte) this._p._rank);
      this.writeS(this.clan.informations, (int) byte.MaxValue);
      this.writeS("Temp", 21);
      this.writeC((byte) this.clan.limitRankId);
      this.writeC((byte) this.clan.limitAgeBigger);
      this.writeC((byte) this.clan.limitAgeSmaller);
      this.writeC((byte) this.clan.autoridade);
      this.writeS("", (int) byte.MaxValue);
      this.writeB(new byte[104]);
      this.writeT(this.clan.pontos);
      this.writeD(this._p._gp);
    }
  }
}
