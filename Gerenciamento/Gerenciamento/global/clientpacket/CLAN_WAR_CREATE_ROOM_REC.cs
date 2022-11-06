
// Type: Game.global.clientpacket.CLAN_WAR_CREATE_ROOM_REC
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.models.enums.match;
using Core.server;
using Game.data.model;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
  public class CLAN_WAR_CREATE_ROOM_REC : ReceiveGamePacket
  {
    private Match MyMatch;
    private Match EnemyMatch;
    private int roomId = -1;

    public CLAN_WAR_CREATE_ROOM_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read()
    {
      Account player = this._client._player;
      if (player == null || player.clanId == 0)
        return;
      Channel channel = player.getChannel();
      this.MyMatch = player._match;
      if (channel == null || this.MyMatch == null)
        return;
      int id = (int) this.readH();
      this.readD();
      this.readD();
      this.EnemyMatch = channel.getMatch(id);
      try
      {
        if (this.EnemyMatch == null)
          return;
        lock (channel._rooms)
        {
          for (int index = 0; index < 300; ++index)
          {
            if (channel.getRoom(index) == null)
            {
              Room room = new Room(index, channel);
              int num1 = (int) this.readH();
              room.name = this.readS(23);
              room.mapId = (int) this.readH();
              room.stage4v4 = this.readC();
              room.room_type = this.readC();
              int num2 = (int) this.readH();
              room.initSlotCount((int) this.readC());
              int num3 = (int) this.readC();
              room.weaponsFlag = this.readC();
              room.random_map = this.readC();
              room.special = this.readC();
              room.password = "";
              room.killtime = 3;
              room.addPlayer(player);
              channel.AddRoom(room);
              this._client.SendPacket((SendPacket) new LOBBY_CREATE_ROOM_PAK(0U, room, player));
              this.roomId = index;
              break;
            }
          }
        }
      }
      catch (Exception ex)
      {
        Logger.error(ex.ToString());
      }
    }

    public override void run()
    {
      if (this.roomId == -1)
        return;
      using (CLAN_WAR_ENEMY_INFO_PAK clanWarEnemyInfoPak = new CLAN_WAR_ENEMY_INFO_PAK(this.EnemyMatch))
      {
        using (CLAN_WAR_JOINED_ROOM_PAK warJoinedRoomPak = new CLAN_WAR_JOINED_ROOM_PAK(this.EnemyMatch, this.roomId, 0))
        {
          byte[] completeBytes1 = clanWarEnemyInfoPak.GetCompleteBytes("CLAN_WAR_CREATE_ROOM_REC-1");
          byte[] completeBytes2 = warJoinedRoomPak.GetCompleteBytes("CLAN_WAR_CREATE_ROOM_REC-2");
          foreach (Account allPlayer in this.MyMatch.getAllPlayers(this.MyMatch._leader))
          {
            if (allPlayer._match != null)
            {
              allPlayer.SendCompletePacket(completeBytes1);
              allPlayer.SendCompletePacket(completeBytes2);
              this.MyMatch._slots[allPlayer.matchSlot].state = SlotMatchState.Ready;
            }
          }
        }
      }
      using (CLAN_WAR_ENEMY_INFO_PAK clanWarEnemyInfoPak = new CLAN_WAR_ENEMY_INFO_PAK(this.MyMatch))
      {
        using (CLAN_WAR_JOINED_ROOM_PAK warJoinedRoomPak = new CLAN_WAR_JOINED_ROOM_PAK(this.MyMatch, this.roomId, 1))
        {
          byte[] completeBytes1 = clanWarEnemyInfoPak.GetCompleteBytes("CLAN_WAR_CREATE_ROOM_REC-3");
          byte[] completeBytes2 = warJoinedRoomPak.GetCompleteBytes("CLAN_WAR_CREATE_ROOM_REC-4");
          foreach (Account allPlayer in this.EnemyMatch.getAllPlayers())
          {
            if (allPlayer._match != null)
            {
              allPlayer.SendCompletePacket(completeBytes1);
              allPlayer.SendCompletePacket(completeBytes2);
              this.MyMatch._slots[allPlayer.matchSlot].state = SlotMatchState.Ready;
            }
          }
        }
      }
    }
  }
}
