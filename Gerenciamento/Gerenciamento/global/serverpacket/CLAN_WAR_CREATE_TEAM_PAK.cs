
// Type: Game.global.serverpacket.CLAN_WAR_CREATE_TEAM_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;
using Game.data.model;

namespace Game.global.serverpacket
{
  public class CLAN_WAR_CREATE_TEAM_PAK : SendPacket
  {
    private uint _erro;
    private Match m;

    public CLAN_WAR_CREATE_TEAM_PAK(uint erro, Match mt = null)
    {
      this._erro = erro;
      this.m = mt;
    }

    public override void write()
    {
      this.writeH((short) 1547);
      this.writeD(this._erro);
      if (this._erro != 0U)
        return;
      this.writeH((short) this.m._matchId);
      this.writeH((short) this.m.getServerInfo());
      this.writeH((short) this.m.getServerInfo());
      this.writeC((byte) this.m._state);
      this.writeC((byte) this.m.friendId);
      this.writeC((byte) this.m.formação);
      this.writeC((byte) this.m.getCountPlayers());
      this.writeD(this.m._leader);
      this.writeC((byte) 0);
    }
  }
}
