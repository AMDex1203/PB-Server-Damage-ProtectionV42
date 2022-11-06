
// Type: Game.global.serverpacket.ROOM_GET_SLOTINFO_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.models.account.clan;
using Core.models.room;
using Core.server;
using Game.data.managers;
using Game.data.model;
using System;

namespace Game.global.serverpacket
{
  public class ROOM_GET_SLOTINFO_PAK : SendPacket
  {
    private Room room;

    public ROOM_GET_SLOTINFO_PAK(Room r) => this.room = r;

    public override void write()
    {
      try
      {
        if (this.room == null)
          return;
        this.writeH((short) 3861);
        if (this.room.getLeader() == null)
          this.room.setNewLeader(-1, 0, this.room._leader, false);
        if (this.room.getLeader() == null)
          return;
        this.writeD(this.room._leader);
        for (int index = 0; index < 16; ++index)
        {
          Slot slot = this.room._slots[index];
          Account playerBySlot = this.room.getPlayerBySlot(slot);
          if (playerBySlot != null)
          {
            Clan clan = ClanManager.getClan(playerBySlot.clanId);
            this.writeC((byte) slot.state);
            this.writeC((byte) playerBySlot.getRank());
            this.writeD(clan.id);
            this.writeD(playerBySlot.clanAccess);
            this.writeC(clan.rank);
            this.writeD(clan.logo);
            this.writeC((byte) playerBySlot.pc_cafe);
            this.writeC((byte) playerBySlot.tourneyLevel);
            this.writeD((uint) playerBySlot.effects);
            this.writeS(clan.name, 17);
            this.writeD(0);
            this.writeC((byte) 31);
          }
          else
          {
            this.writeC((byte) slot.state);
            this.writeB(new byte[10]);
            this.writeD(uint.MaxValue);
            this.writeB(new byte[28]);
          }
        }
      }
      catch (Exception ex)
      {
        Logger.warning("[ROOM_GET_SLOTINFO_PAK] " + ex.ToString());
      }
    }
  }
}
