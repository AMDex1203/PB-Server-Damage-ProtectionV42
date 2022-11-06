
// Type: Game.global.serverpacket.CLAN_WAR_JOIN_TEAM_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;
using Game.data.model;

namespace Game.global.serverpacket
{
  public class CLAN_WAR_JOIN_TEAM_PAK : SendPacket
  {
    private Match m;
    private uint _erro;

    public CLAN_WAR_JOIN_TEAM_PAK(uint erro, Match m = null)
    {
      this._erro = erro;
      this.m = m;
    }

    public override void write()
    {
      this.writeH((short) 1549);
      this.writeD(this._erro);
      if (this._erro != 0U)
        return;
      this.writeH((short) this.m._matchId);
      this.writeH((ushort) this.m.getServerInfo());
      this.writeH((ushort) this.m.getServerInfo());
      this.writeC((byte) this.m._state);
      this.writeC((byte) this.m.friendId);
      this.writeC((byte) this.m.formação);
      this.writeC((byte) this.m.getCountPlayers());
      this.writeD(this.m._leader);
      this.writeC((byte) 0);
      this.writeD(this.m.clan.id);
      this.writeC(this.m.clan.rank);
      this.writeD(this.m.clan.logo);
      this.writeS(this.m.clan.name, 17);
      this.writeT(this.m.clan.pontos);
      this.writeC(this.m.clan.nameColor);
      for (int index = 0; index < this.m.formação; ++index)
      {
        SLOT_MATCH slot = this.m._slots[index];
        Account playerBySlot = this.m.getPlayerBySlot(slot);
        if (playerBySlot != null)
        {
          this.writeC((byte) playerBySlot._rank);
          this.writeS(playerBySlot.player_name, 33);
          this.writeQ(playerBySlot.player_id);
          this.writeC((byte) slot.state);
        }
        else
          this.writeB(new byte[43]);
      }
    }
  }
}
