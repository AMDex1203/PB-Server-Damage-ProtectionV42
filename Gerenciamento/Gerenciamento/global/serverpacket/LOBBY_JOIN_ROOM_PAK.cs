
// Type: Game.global.serverpacket.LOBBY_JOIN_ROOM_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.models.account.clan;
using Core.server;
using Game.data.managers;
using Game.data.model;
using System.Collections.Generic;

namespace Game.global.serverpacket
{
  public class LOBBY_JOIN_ROOM_PAK : SendPacket
  {
    private uint erro;
    private Room room;
    private int slotId;
    private Account leader;

    public LOBBY_JOIN_ROOM_PAK(uint erro, Account player = null, Account leader = null)
    {
      this.erro = erro;
      if (player == null)
        return;
      this.slotId = player._slotId;
      this.room = player._room;
      this.leader = leader;
    }

    public override void write()
    {
      this.writeH((short) 3082);
      if (this.erro == 0U && this.room != null && this.leader != null)
      {
        lock (this.room._slots)
          this.WriteData();
      }
      else
        this.writeD(this.erro);
    }

    private void WriteData()
    {
      List<Account> allPlayers = this.room.getAllPlayers();
      this.writeD(this.room._roomId);
      this.writeD(this.slotId);
      this.writeD(this.room._roomId);
      this.writeS(this.room.name, 23);
      this.writeH((short) this.room.mapId);
      this.writeC(this.room.stage4v4);
      this.writeC(this.room.room_type);
      this.writeC((byte) this.room._state);
      this.writeC((byte) allPlayers.Count);
      this.writeC((byte) this.room.getSlotCount());
      this.writeC((byte) this.room._ping);
      this.writeC(this.room.weaponsFlag);
      this.writeC(this.room.random_map);
      this.writeC(this.room.special);
      this.writeS(this.leader.player_name, 33);
      this.writeD(this.room.killtime);
      this.writeC(this.room.limit);
      this.writeC(this.room.seeConf);
      this.writeH((short) this.room.autobalans);
      this.writeS(this.room.password, 4);
      this.writeC((byte) this.room.countdown.getTimeLeft());
      this.writeD(this.room._leader);
      for (int index = 0; index < 16; ++index)
      {
        Core.models.room.Slot slot = this.room._slots[index];
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
      this.writeC((byte) allPlayers.Count);
      for (int index = 0; index < allPlayers.Count; ++index)
      {
        Account account = allPlayers[index];
        this.writeC((byte) account._slotId);
        this.writeC((byte) (account.player_name.Length + 1));
        this.writeS(account.player_name, account.player_name.Length + 1);
        this.writeC((byte) account.name_color);
      }
      if (!this.room.isBotMode())
        return;
      this.writeC(this.room.aiCount);
      this.writeC(this.room.aiLevel);
    }
  }
}
