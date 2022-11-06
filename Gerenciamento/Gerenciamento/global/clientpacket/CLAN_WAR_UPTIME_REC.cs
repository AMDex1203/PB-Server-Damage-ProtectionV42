
// Type: Game.global.clientpacket.CLAN_WAR_UPTIME_REC
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.server;
using Game.data.model;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
  public class CLAN_WAR_UPTIME_REC : ReceiveGamePacket
  {
    private int formacao;

    public CLAN_WAR_UPTIME_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read() => this.formacao = (int) this.readC();

    public override void run()
    {
      try
      {
        Account player = this._client._player;
        if (player == null)
          return;
        Match match = player._match;
        if (match != null && player.matchSlot == match._leader)
        {
          match.formação = this.formacao;
          using (CLAN_WAR_MATCH_UPTIME_PAK warMatchUptimePak = new CLAN_WAR_MATCH_UPTIME_PAK(0U, this.formacao))
            match.SendPacketToPlayers((SendPacket) warMatchUptimePak);
        }
        else
          this._client.SendPacket((SendPacket) new CLAN_WAR_MATCH_UPTIME_PAK(2147483648U));
      }
      catch (Exception ex)
      {
        Logger.info(ex.ToString());
      }
    }
  }
}
