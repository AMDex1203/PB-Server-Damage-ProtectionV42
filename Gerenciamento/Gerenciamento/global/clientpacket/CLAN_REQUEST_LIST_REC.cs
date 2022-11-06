
// Type: Game.global.clientpacket.CLAN_REQUEST_LIST_REC
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.managers;
using Core.models.account.clan;
using Core.server;
using Game.data.managers;
using Game.data.model;
using Game.global.serverpacket;
using System;
using System.Collections.Generic;

namespace Game.global.clientpacket
{
  public class CLAN_REQUEST_LIST_REC : ReceiveGamePacket
  {
    private int page;

    public CLAN_REQUEST_LIST_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read() => this.page = (int) this.readC();

    public override void run()
    {
      try
      {
        Account player = this._client._player;
        if (player == null)
          return;
        if (player.clanId == 0)
        {
          this._client.SendPacket((SendPacket) new CLAN_REQUEST_LIST_PAK(-1));
        }
        else
        {
          List<ClanInvite> clanRequestList = PlayerManager.getClanRequestList(player.clanId);
          using (SendGPacket p = new SendGPacket())
          {
            int count = 0;
            for (int index = this.page * 13; index < clanRequestList.Count; ++index)
            {
              this.WriteData(clanRequestList[index], p);
              if (++count == 13)
                break;
            }
            this._client.SendPacket((SendPacket) new CLAN_REQUEST_LIST_PAK(0, count, this.page, p.mstream.ToArray()));
          }
        }
      }
      catch (Exception ex)
      {
        Logger.info("CLAN_REQUEST_LIST_REC: " + ex.ToString());
      }
    }

    private void WriteData(ClanInvite invite, SendGPacket p)
    {
      p.writeQ(invite.player_id);
      Account account = AccountManager.getAccount(invite.player_id, 0);
      if (account != null)
      {
        p.writeS(account.player_name, 33);
        p.writeC((byte) account._rank);
      }
      else
        p.writeB(new byte[34]);
      p.writeD(invite.inviteDate);
    }
  }
}
