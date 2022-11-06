
// Type: Game.global.clientpacket.CLAN_WAR_MATCH_TEAM_LIST_REC
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
  public class CLAN_WAR_MATCH_TEAM_LIST_REC : ReceiveGamePacket
  {
    private int page;

    public CLAN_WAR_MATCH_TEAM_LIST_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read() => this.page = (int) this.readH();

    public override void run()
    {
      try
      {
        Account player = this._client._player;
        if (player == null || player._match == null)
          return;
        Channel channel = player.getChannel();
        if (channel == null || channel._type != 4)
          return;
        this._client.SendPacket((SendPacket) new CLAN_WAR_MATCH_TEAM_LIST_PAK(this.page, channel._matchs, player._match._matchId));
      }
      catch (Exception ex)
      {
        Logger.info(ex.ToString());
      }
    }
  }
}
