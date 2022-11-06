
// Type: Game.global.serverpacket.CLAN_WAR_PARTY_LIST_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;
using Game.data.model;
using System.Collections.Generic;

namespace Game.global.serverpacket
{
  public class CLAN_WAR_PARTY_LIST_PAK : SendPacket
  {
    private List<Match> _c;
    private int _erro;

    public CLAN_WAR_PARTY_LIST_PAK(int erro, List<Match> c)
    {
      this._erro = erro;
      this._c = c;
    }

    public override void write()
    {
      this.writeH((short) 1541);
      this.writeC(this._erro == 0 ? (byte) this._c.Count : (byte) this._erro);
      if (this._erro > 0 || this._c.Count == 0)
        return;
      this.writeC((byte) 1);
      this.writeC((byte) 0);
      this.writeC((byte) this._c.Count);
      foreach (Match match in this._c)
      {
        this.writeH((short) match._matchId);
        this.writeH((ushort) match.getServerInfo());
        this.writeH((ushort) match.getServerInfo());
        this.writeC((byte) match._state);
        this.writeC((byte) match.friendId);
        this.writeC((byte) match.formação);
        this.writeC((byte) match.getCountPlayers());
        this.writeC((byte) 0);
        this.writeD(match._leader);
        Account leader = match.getLeader();
        if (leader != null)
        {
          this.writeC((byte) leader._rank);
          this.writeS(leader.player_name, 33);
          this.writeQ(leader.player_id);
          this.writeC((byte) match._slots[match._leader].state);
        }
        else
          this.writeB(new byte[43]);
      }
    }
  }
}
