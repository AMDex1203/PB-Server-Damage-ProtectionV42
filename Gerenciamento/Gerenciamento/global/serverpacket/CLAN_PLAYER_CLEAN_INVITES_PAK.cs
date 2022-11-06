﻿
// Type: Game.global.serverpacket.CLAN_PLAYER_CLEAN_INVITES_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;

namespace Game.global.serverpacket
{
  public class CLAN_PLAYER_CLEAN_INVITES_PAK : SendPacket
  {
    private uint _erro;

    public CLAN_PLAYER_CLEAN_INVITES_PAK(uint error) => this._erro = error;

    public override void write()
    {
      this.writeH((short) 1319);
      this.writeD(this._erro);
    }
  }
}
