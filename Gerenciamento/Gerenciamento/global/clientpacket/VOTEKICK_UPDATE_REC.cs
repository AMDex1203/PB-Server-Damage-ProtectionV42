
// Type: Game.global.clientpacket.VOTEKICK_UPDATE_REC
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.models.enums;
using Core.models.room;
using Core.server;
using Game.data.model;
using Game.data.utils;
using Game.global.serverpacket;
using System;
using System.Threading;

namespace Game.global.clientpacket
{
  public class VOTEKICK_UPDATE_REC : ReceiveGamePacket
  {
    private byte type;

    public VOTEKICK_UPDATE_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read() => this.type = this.readC();

    public override void run()
    {
      try
      {
        Account player = this._client._player;
        Room room = player == null ? (Room) null : player._room;
        Slot slot;
        if (room == null || room._state != RoomState.Battle || (room.vote.Timer == null || room.votekick == null) || (!room.getSlot(player._slotId, out slot) || slot.state != SLOT_STATE.BATTLE))
          return;
        VoteKick votekick = room.votekick;
        if (votekick._votes.Contains(player._slotId))
        {
          this._client.SendPacket((SendPacket) new VOTEKICK_UPDATE_RESULT_PAK(2147487985U));
        }
        else
        {
          lock (votekick._votes)
            votekick._votes.Add(slot._id);
          if (this.type == (byte) 0)
          {
            ++votekick.kikar;
            if (slot._team == votekick.victimIdx % 2)
              ++votekick.allies;
            else
              ++votekick.enemys;
          }
          else
            ++votekick.deixar;
          if (votekick._votes.Count >= votekick.GetInGamePlayers())
          {
            room.vote.Timer = (Timer) null;
            AllUtils.votekickResult(room);
          }
          else
          {
            using (VOTEKICK_UPDATE_PAK votekickUpdatePak = new VOTEKICK_UPDATE_PAK(votekick))
              room.SendPacketToPlayers((SendPacket) votekickUpdatePak, SLOT_STATE.BATTLE, 0);
          }
        }
      }
      catch (Exception ex)
      {
        Logger.info("VOTEKICK_UPDATE_REC: " + ex.ToString());
      }
    }
  }
}
