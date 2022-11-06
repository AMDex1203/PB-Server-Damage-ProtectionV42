
// Type: Game.global.clientpacket.ROOM_REQUEST_HOST_REC
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
  public class ROOM_REQUEST_HOST_REC : ReceiveGamePacket
  {
    public ROOM_REQUEST_HOST_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read()
    {
    }

    public override void run()
    {
      try
      {
        if (this._client == null)
          return;
        Account player = this._client._player;
        Room r = player == null ? (Room) null : player._room;
        if (r != null)
        {
          if (r._state != RoomState.Ready || r._leader == player._slotId)
            return;
          List<Account> allPlayers = r.getAllPlayers();
          if (allPlayers.Count == 0)
            return;
          if (player.access >= AccessLevel.GameMaster)
          {
            this.ChangeLeader(r, allPlayers, player._slotId);
          }
          else
          {
            if (!r.requestHost.Contains(player.player_id))
            {
              r.requestHost.Add(player.player_id);
              if (r.requestHost.Count() < allPlayers.Count / 2 + 1)
              {
                using (ROOM_GET_HOST_PAK roomGetHostPak = new ROOM_GET_HOST_PAK(player._slotId))
                  this.SendPacketToRoom((SendPacket) roomGetHostPak, allPlayers);
              }
            }
            if (r.requestHost.Count() < allPlayers.Count / 2 + 1)
              return;
            this.ChangeLeader(r, allPlayers, player._slotId);
          }
        }
        else
          this._client.SendPacket((SendPacket) new ROOM_GET_HOST_PAK(2147483648U));
      }
      catch (Exception ex)
      {
        Logger.info("ROOM_REQUEST_HOST_REC: " + ex.ToString());
      }
    }

    private void ChangeLeader(Room r, List<Account> players, int slotId)
    {
      r.setNewLeader(slotId, 0, -1, true);
      using (ROOM_CHANGE_HOST_PAK roomChangeHostPak = new ROOM_CHANGE_HOST_PAK(slotId))
        this.SendPacketToRoom((SendPacket) roomChangeHostPak, players);
      r.updateSlotsInfo();
      r.requestHost.Clear();
    }

    private void SendPacketToRoom(SendPacket packet, List<Account> players)
    {
      byte[] completeBytes = packet.GetCompleteBytes(nameof (ROOM_REQUEST_HOST_REC));
      foreach (Account player in players)
        player.SendCompletePacket(completeBytes);
    }
  }
}
