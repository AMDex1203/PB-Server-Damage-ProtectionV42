
// Type: Game.global.clientpacket.CLAN_WAR_PARTY_CONTEXT_REC
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
  public class CLAN_WAR_PARTY_CONTEXT_REC : ReceiveGamePacket
  {
    private int matchs;

    public CLAN_WAR_PARTY_CONTEXT_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read()
    {
    }

    public override void run()
    {
      try
      {
        Account player = this._client._player;
        if (player != null && player.clanId > 0)
        {
          Channel channel = player.getChannel();
          if (channel != null && channel._type == 4)
          {
            lock (channel._matchs)
            {
              foreach (Match match in channel._matchs)
              {
                if (match.clan.id == player.clanId)
                  ++this.matchs;
              }
            }
          }
        }
        this._client.SendPacket((SendPacket) new CLAN_WAR_PARTY_CONTEXT_PAK(this.matchs));
      }
      catch (Exception ex)
      {
        Logger.info(ex.ToString());
      }
    }
  }
}
