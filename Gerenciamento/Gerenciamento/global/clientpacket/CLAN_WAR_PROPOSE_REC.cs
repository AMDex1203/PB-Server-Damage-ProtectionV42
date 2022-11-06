
// Type: Game.global.clientpacket.CLAN_WAR_PROPOSE_REC
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.models.enums.match;
using Core.server;
using Game.data.model;
using Game.data.xml;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
  public class CLAN_WAR_PROPOSE_REC : ReceiveGamePacket
  {
    private int id;
    private int serverInfo;
    private uint erro;

    public CLAN_WAR_PROPOSE_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read()
    {
      this.id = (int) this.readH();
      this.serverInfo = (int) this.readH();
    }

    public override void run()
    {
      try
      {
        Account player = this._client._player;
        if (player != null && player._match != null && (player.matchSlot == player._match._leader && player._match._state == MatchState.Ready))
        {
          Match match = ChannelsXML.getChannel(this.serverInfo - this.serverInfo / 10 * 10).getMatch(this.id);
          if (match != null)
          {
            Account leader = match.getLeader();
            if (leader != null && leader._connection != null && leader._isOnline)
              leader.SendPacket((SendPacket) new CLAN_WAR_MATCH_REQUEST_BATTLE_PAK(player._match, player));
            else
              this.erro = 2147483648U;
          }
          else
            this.erro = 2147483648U;
        }
        else
          this.erro = 2147483648U;
        this._client.SendPacket((SendPacket) new CLAN_WAR_MATCH_PROPOSE_PAK(this.erro));
      }
      catch (Exception ex)
      {
        Logger.info("CLAN_WAR_PROPOSE_REC: " + ex.ToString());
      }
    }
  }
}
