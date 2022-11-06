
// Type: Game.global.serverpacket.ROOM_GET_LOBBY_USER_LIST_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;
using Game.data.model;
using System;
using System.Collections.Generic;

namespace Game.global.serverpacket
{
  public class ROOM_GET_LOBBY_USER_LIST_PAK : SendPacket
  {
    private List<Account> players;
    private List<int> playersIdxs;

    public ROOM_GET_LOBBY_USER_LIST_PAK(Channel ch)
    {
      this.players = ch.getWaitPlayers();
      this.playersIdxs = this.GetRandomIndexes(this.players.Count, this.players.Count >= 8 ? 8 : this.players.Count);
    }

    private List<int> GetRandomIndexes(int total, int count)
    {
      if (total == 0 || count == 0)
        return new List<int>();
      Random random = new Random();
      List<int> intList = new List<int>();
      for (int index = 0; index < total; ++index)
        intList.Add(index);
      for (int index1 = 0; index1 < intList.Count; ++index1)
      {
        int index2 = random.Next(intList.Count);
        int num = intList[index1];
        intList[index1] = intList[index2];
        intList[index2] = num;
      }
      return intList.GetRange(0, count);
    }

    public override void write()
    {
      this.writeH((short) 3855);
      this.writeD(this.playersIdxs.Count);
      for (int index = 0; index < this.playersIdxs.Count; ++index)
      {
        Account player = this.players[this.playersIdxs[index]];
        this.writeD(player.getSessionId());
        this.writeD(player.getRank());
        this.writeC((byte) (player.player_name.Length + 1));
        this.writeS(player.player_name, player.player_name.Length + 1);
      }
    }
  }
}
