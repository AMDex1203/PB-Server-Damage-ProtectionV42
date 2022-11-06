
// Type: Game.global.clientpacket.LOBBY_QUICKJOIN_ROOM_REC
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.models.enums;
using Core.server;
using Game.data.model;
using Game.global.serverpacket;
using System;
using System.Collections.Generic;

namespace Game.global.clientpacket
{
  public class LOBBY_QUICKJOIN_ROOM_REC : ReceiveGamePacket
  {
    private List<Room> salas = new List<Room>();

    public LOBBY_QUICKJOIN_ROOM_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read()
    {
    }

    public override void run()
    {
      try
      {
        Account player = this._client._player;
        if (player == null)
          return;
        Channel channel;
        if (player != null && player.player_name.Length > 0 && (player._room == null && player._match == null) && player.getChannel(out channel))
        {
          lock (channel._rooms)
          {
            for (int index1 = 0; index1 < channel._rooms.Count; ++index1)
            {
              Room room = channel._rooms[index1];
              if (room.room_type != (byte) 10 && room.password.Length == 0 && (room.limit == (byte) 0 && room.special != (byte) 5) && (!room.kickedPlayers.Contains(player.player_id) || player.HaveGMLevel()))
              {
                for (int index2 = 0; index2 < 16; ++index2)
                {
                  Core.models.room.Slot slot = room._slots[index2];
                  if (slot._playerId == 0L && slot.state == SLOT_STATE.EMPTY)
                  {
                    this.salas.Add(room);
                    break;
                  }
                }
              }
            }
          }
        }
        if (this.salas.Count == 0)
          this._client.SendPacket((SendPacket) new LOBBY_QUICKJOIN_ROOM_PAK());
        else
          this.getRandomRoom(player);
        this.salas = (List<Room>) null;
      }
      catch (Exception ex)
      {
        Logger.warning("[ROOM_JOIN_QUICK_REC] " + ex.ToString());
      }
    }

    private void getRandomRoom(Account player)
    {
      if (player != null)
      {
        Room sala = this.salas[new Random().Next(this.salas.Count)];
        Account p;
        if (sala != null && sala.getLeader(out p) && sala.addPlayer(player) >= 0)
        {
          player.ResetPages();
          using (ROOM_GET_SLOTONEINFO_PAK getSlotoneinfoPak = new ROOM_GET_SLOTONEINFO_PAK(player))
            sala.SendPacketToPlayers((SendPacket) getSlotoneinfoPak, player.player_id);
          this._client.SendPacket((SendPacket) new LOBBY_JOIN_ROOM_PAK(0U, player, p));
        }
        else
          this._client.SendPacket((SendPacket) new LOBBY_QUICKJOIN_ROOM_PAK());
      }
      else
        this._client.SendPacket((SendPacket) new LOBBY_QUICKJOIN_ROOM_PAK());
    }
  }
}
