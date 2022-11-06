
// Type: Game.global.serverpacket.CLAN_WAR_MATCH_TEAM_LIST_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;
using Game.data.model;
using System.Collections.Generic;

namespace Game.global.serverpacket
{
  public class CLAN_WAR_MATCH_TEAM_LIST_PAK : SendPacket
  {
    private List<Match> matchs;
    private int myMatchIdx;
    private int _page;
    private int MatchCount;

    public CLAN_WAR_MATCH_TEAM_LIST_PAK(int page, List<Match> matchs, int matchId)
    {
      this._page = page;
      this.myMatchIdx = matchId;
      this.MatchCount = matchs.Count - 1;
      this.matchs = matchs;
    }

    public override void write()
    {
      this.writeH((short) 1545);
      this.writeH((ushort) this.MatchCount);
      if (this.MatchCount == 0)
        return;
      this.writeH((short) 1);
      this.writeH((short) 0);
      this.writeC((byte) this.MatchCount);
      for (int index = 0; index < this.matchs.Count; ++index)
      {
        Match match = this.matchs[index];
        if (match._matchId != this.myMatchIdx)
        {
          this.writeH((short) match._matchId);
          this.writeH((short) match.getServerInfo());
          this.writeH((short) match.getServerInfo());
          this.writeC((byte) match._state);
          this.writeC((byte) match.friendId);
          this.writeC((byte) match.formação);
          this.writeC((byte) match.getCountPlayers());
          this.writeD(match._leader);
          this.writeC((byte) 0);
          this.writeD(match.clan.id);
          this.writeC(match.clan.rank);
          this.writeD(match.clan.logo);
          this.writeS(match.clan.name, 17);
          this.writeT(match.clan.pontos);
          this.writeC(match.clan.nameColor);
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
}
