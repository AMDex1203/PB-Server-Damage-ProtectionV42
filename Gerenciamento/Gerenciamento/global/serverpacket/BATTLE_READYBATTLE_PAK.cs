
// Type: Game.global.serverpacket.BATTLE_READYBATTLE_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.models.enums;
using Core.models.room;
using Core.server;
using Game.data.model;

namespace Game.global.serverpacket
{
  public class BATTLE_READYBATTLE_PAK : SendPacket
  {
    private int PlayersCount;
    private Room room;
    private byte[] Data;

    public BATTLE_READYBATTLE_PAK(Room r)
    {
      this.room = r;
      if (ConfigGS.isTestMode && ConfigGS.udpType == SERVER_UDP_STATE.RELAY)
      {
        this.room._slots[LoggerGS.TestSlot]._playerId = 0L;
        this.room._slots[LoggerGS.TestSlot].state = SLOT_STATE.EMPTY;
      }
      string clientVersion = GameManager.Config.ClientVersion;
      using (SendGPacket pk = new SendGPacket())
      {
        for (int index = 0; index < 16; ++index)
        {
          Slot slot = this.room._slots[index];
          if (slot.state >= SLOT_STATE.READY && slot._equip != null)
          {
            Account playerBySlot = this.room.getPlayerBySlot(slot);
            if (playerBySlot != null && playerBySlot._slotId == index)
            {
              this.WriteSlotInfo(slot, playerBySlot, clientVersion, pk);
              ++this.PlayersCount;
            }
          }
        }
        this.Data = pk.mstream.ToArray();
      }
    }

    private void WriteSlotInfo(Slot s, Account p, string client, SendGPacket pk)
    {
      pk.writeC((byte) s._id);
      pk.writeD(s._equip._red);
      pk.writeD(s._equip._blue);
      pk.writeD(s._equip._helmet);
      pk.writeD(s._equip._beret);
      pk.writeD(s._equip._dino);
      pk.writeD(s._equip._primary);
      pk.writeD(s._equip._secondary);
      pk.writeD(s._equip._melee);
      pk.writeD(s._equip._grenade);
      pk.writeD(s._equip._special);
      pk.writeD(0);
      if (p != null)
      {
        pk.writeC((byte) p._titles.Equiped1);
        pk.writeC((byte) p._titles.Equiped2);
        pk.writeC((byte) p._titles.Equiped3);
      }
      else
        pk.writeB(new byte[3]);
      if (!(client == "1.15.42"))
        return;
      pk.writeD(0);
    }

    public override void write()
    {
      this.writeH((short) 3426);
      this.writeH((short) this.room.mapId);
      this.writeC(this.room.stage4v4);
      this.writeC(this.room.room_type);
      this.writeC((byte) this.PlayersCount);
      this.writeB(this.Data);
    }
  }
}
