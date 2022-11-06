
// Type: Game.global.clientpacket.CLAN_WAR_JOIN_ROOM_REC
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.models.enums;
using Core.server;
using Game.data.model;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
  public class CLAN_WAR_JOIN_ROOM_REC : ReceiveGamePacket
  {
    private int match;
    private int channel;
    private int unk;

    public CLAN_WAR_JOIN_ROOM_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read()
    {
      this.match = this.readD();
      this.unk = (int) this.readH();
      this.channel = (int) this.readH();
    }

    public override void run()
    {
      try
      {
        Account player = this._client._player;
        if (player == null || player.clanId == 0 || player._match == null)
          return;
        Channel channel;
        if (player != null && player.player_name.Length > 0 && (player._room == null && player.getChannel(out channel)))
        {
          Room room = channel.getRoom(this.match);
          Account p;
          if (room != null && room.getLeader(out p))
          {
            if (room.password.Length > 0 && !player.HaveGMLevel())
              this._client.SendPacket((SendPacket) new LOBBY_JOIN_ROOM_PAK(2147487749U));
            else if (room.limit == (byte) 1 && room._state >= RoomState.CountDown)
              this._client.SendPacket((SendPacket) new LOBBY_JOIN_ROOM_PAK(2147487763U));
            else if (room.kickedPlayers.Contains(player.player_id))
              this._client.SendPacket((SendPacket) new LOBBY_JOIN_ROOM_PAK(2147487756U));
            else if (room.addPlayer(player, this.unk) >= 0)
            {
              using (ROOM_GET_SLOTONEINFO_PAK getSlotoneinfoPak = new ROOM_GET_SLOTONEINFO_PAK(player))
                room.SendPacketToPlayers((SendPacket) getSlotoneinfoPak, player.player_id);
              this._client.SendPacket((SendPacket) new LOBBY_JOIN_ROOM_PAK(0U, player, p));
            }
            else
              this._client.SendPacket((SendPacket) new LOBBY_JOIN_ROOM_PAK(2147487747U));
          }
          else
            this._client.SendPacket((SendPacket) new LOBBY_JOIN_ROOM_PAK(2147487748U));
        }
        else
          this._client.SendPacket((SendPacket) new LOBBY_JOIN_ROOM_PAK(2147487748U));
      }
      catch (Exception ex)
      {
        Logger.warning("[CLAN_WAR_JOIN_ROOM_REC] " + ex.ToString());
      }
    }
  }
}
