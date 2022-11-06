
// Type: Game.global.serverpacket.BATTLE_PRESTARTBATTLE_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;
using Game.data.model;

namespace Game.global.serverpacket
{
  public class BATTLE_PRESTARTBATTLE_PAK : SendPacket
  {
    private Account player;
    private Account leader;
    private Room room;
    private bool isPreparing;
    private bool LoadHits;
    private uint UniqueRoomId;
    private int gen2;

    public BATTLE_PRESTARTBATTLE_PAK(Account p, Account l, bool more, int gen2)
    {
      this.player = p;
      this.leader = l;
      this.LoadHits = more;
      this.gen2 = gen2;
      this.room = p._room;
      if (this.room == null)
        return;
      this.isPreparing = this.room.isPreparing();
      this.UniqueRoomId = this.room.UniqueRoomId;
    }

    public BATTLE_PRESTARTBATTLE_PAK()
    {
    }

    public override void write()
    {
      this.writeH((short) 3349);
      this.writeD(this.isPreparing);
      if (!this.isPreparing)
        return;
      this.writeD(this.player._slotId);
      this.writeC((byte) ConfigGS.udpType);
      this.writeIP(this.leader.PublicIP);
      this.writeH((short) 29890);
      this.writeB(this.leader.LocalIP);
      this.writeH((short) 29890);
      this.writeC((byte) 0);
      this.writeIP(this.player.PublicIP);
      this.writeH((short) 29890);
      this.writeB(this.player.LocalIP);
      this.writeH((short) 29890);
      this.writeC((byte) 0);
      this.writeIP(this.room.UDPServer.Connection.Address);
      this.writeH((ushort) this.room.UDPServer.Port);
      this.writeD(this.UniqueRoomId);
      this.writeD(this.gen2);
      if (!this.LoadHits)
        return;
      this.writeB(new byte[35]
      {
        (byte) 32,
        (byte) 21,
        (byte) 22,
        (byte) 23,
        (byte) 24,
        (byte) 25,
        (byte) 17,
        (byte) 27,
        (byte) 28,
        (byte) 29,
        (byte) 26,
        (byte) 31,
        (byte) 9,
        (byte) 33,
        (byte) 14,
        (byte) 30,
        (byte) 1,
        (byte) 2,
        (byte) 3,
        (byte) 4,
        (byte) 5,
        (byte) 6,
        (byte) 7,
        (byte) 8,
        (byte) 20,
        (byte) 10,
        (byte) 11,
        (byte) 12,
        (byte) 13,
        (byte) 34,
        (byte) 15,
        (byte) 16,
        (byte) 0,
        (byte) 18,
        (byte) 19
      });
    }
  }
}
