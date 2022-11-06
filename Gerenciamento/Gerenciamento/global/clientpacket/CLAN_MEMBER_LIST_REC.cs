
// Type: Game.global.clientpacket.CLAN_MEMBER_LIST_REC
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.server;
using Game.data.managers;
using Game.data.model;
using Game.global.serverpacket;
using System;
using System.Collections.Generic;

namespace Game.global.clientpacket
{
  public class CLAN_MEMBER_LIST_REC : ReceiveGamePacket
  {
    private int page;

    public CLAN_MEMBER_LIST_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read() => this.page = (int) this.readC();

    public override void run()
    {
      try
      {
        Account player = this._client._player;
        if (player == null)
          return;
        if (ClanManager.getClan(player.clanId).id == 0)
        {
          this._client.SendPacket((SendPacket) new CLAN_MEMBER_LIST_PAK(-1));
        }
        else
        {
          List<Account> clanPlayers = ClanManager.getClanPlayers(player.clanId, -1L, false);
          using (SendGPacket p = new SendGPacket())
          {
            int count = 0;
            for (int index = this.page * 14; index < clanPlayers.Count; ++index)
            {
              this.WriteData(clanPlayers[index], p);
              if (++count == 14)
                break;
            }
            this._client.SendPacket((SendPacket) new CLAN_MEMBER_LIST_PAK(this.page, count, p.mstream.ToArray()));
          }
        }
      }
      catch (Exception ex)
      {
        Logger.info("CLAN_MEMBER_LIST_REC: " + ex.ToString());
      }
    }

    private void WriteData(Account member, SendGPacket p)
    {
      p.writeQ(member.player_id);
      p.writeS(member.player_name, 33);
      p.writeC((byte) member._rank);
      p.writeC((byte) member.clanAccess);
      p.writeQ(ComDiv.GetClanStatus(member._status, member._isOnline));
      p.writeD(member.clanDate);
      p.writeC((byte) member.name_color);
    }
  }
}
