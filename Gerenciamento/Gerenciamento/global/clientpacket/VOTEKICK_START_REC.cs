
// Type: Game.global.clientpacket.VOTEKICK_START_REC
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

namespace Game.global.clientpacket
{
  public class VOTEKICK_START_REC : ReceiveGamePacket
  {
    private int motive;
    private int slotIdx;
    private uint erro;

    public VOTEKICK_START_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read()
    {
      this.slotIdx = (int) this.readC();
      this.motive = (int) this.readC();
    }

    public override void run()
    {
      try
      {
        Account player = this._client._player;
        Room room = player == null ? (Room) null : player._room;
        if (room == null || room._state != RoomState.Battle || player._slotId == this.slotIdx)
          return;
        Slot slot = room.getSlot(player._slotId);
        if (slot == null || slot.state != SLOT_STATE.BATTLE || room._slots[this.slotIdx].state != SLOT_STATE.BATTLE)
          return;
        room.getPlayingPlayers(true, out int _, out int _);
        if (player._rank < ConfigGS.minRankVote && !player.HaveGMLevel())
          this.erro = 2147487972U;
        else if (room.vote.Timer != null)
          this.erro = 2147487968U;
        else if (slot.NextVoteDate > DateTime.Now)
          this.erro = 2147487969U;
        this._client.SendPacket((SendPacket) new VOTEKICK_CHECK_PAK(this.erro));
        if (this.erro > 0U)
          return;
        slot.NextVoteDate = DateTime.Now.AddMinutes(1.0);
        room.votekick = new VoteKick(slot._id, this.slotIdx)
        {
          motive = this.motive
        };
        this.ChargeVoteKickArray(room);
        using (VOTEKICK_START_PAK votekickStartPak = new VOTEKICK_START_PAK(room.votekick))
          room.SendPacketToPlayers((SendPacket) votekickStartPak, SLOT_STATE.BATTLE, 0, player._slotId, this.slotIdx);
        AllUtils.LogVotekickStart(room, player, slot);
        room.StartVote();
      }
      catch (Exception ex)
      {
        Logger.info("VOTEKICK_START_REC: " + ex.ToString());
      }
    }

    private void ChargeVoteKickArray(Room room)
    {
      for (int index = 0; index < 16; ++index)
      {
        Slot slot = room._slots[index];
        room.votekick.TotalArray[index] = slot.state == SLOT_STATE.BATTLE;
      }
    }
  }
}
