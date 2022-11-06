
// Type: Game.global.clientpacket.CLAN_WAR_LEAVE_TEAM_REC
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.server;
using Game.data.model;
using Game.data.utils;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
  public class CLAN_WAR_LEAVE_TEAM_REC : ReceiveGamePacket
  {
    private uint erro;

    public CLAN_WAR_LEAVE_TEAM_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read()
    {
    }

    public override void run()
    {
      try
      {
        Account player = this._client._player;
        Match match = player == null ? (Match) null : player._match;
        if (match == null || !match.RemovePlayer(player))
          this.erro = 2147483648U;
        this._client.SendPacket((SendPacket) new CLAN_WAR_LEAVE_TEAM_PAK(this.erro));
        if (this.erro != 0U)
          return;
        player._status.updateClanMatch(byte.MaxValue);
        AllUtils.syncPlayerToClanMembers(player);
      }
      catch (Exception ex)
      {
        Logger.info(ex.ToString());
      }
    }
  }
}
