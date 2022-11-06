
// Type: Game.global.clientpacket.LOBBY_GET_ROOMLIST_REC
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.models.account.clan;
using Core.models.enums;
using Core.server;
using Game.data.managers;
using Game.data.model;
using Game.global.serverpacket;
using System;
using System.Collections.Generic;

namespace Game.global.clientpacket
{
  public class LOBBY_GET_ROOMLIST_REC : ReceiveGamePacket
  {
    public LOBBY_GET_ROOMLIST_REC(GameClient client, byte[] data) => this.makeme(client, data);

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
        Channel channel = player.getChannel();
        if (channel == null)
          return;
        channel.RemoveEmptyRooms();
        List<Room> rooms = channel._rooms;
        List<Account> waitPlayers = channel.getWaitPlayers();
        int num1 = (int) Math.Ceiling((double) rooms.Count / 15.0);
        int num2 = (int) Math.Ceiling((double) waitPlayers.Count / 10.0);
        if (player.LastRoomPage >= num1)
          player.LastRoomPage = 0;
        if (player.LastPlayerPage >= num2)
          player.LastPlayerPage = 0;
        int count1 = 0;
        int count2 = 0;
        byte[] roomListData = this.GetRoomListData(player.LastRoomPage, ref count1, rooms);
        byte[] playerListData = this.GetPlayerListData(player.LastPlayerPage, ref count2, waitPlayers);
        this._client.SendPacket((SendPacket) new LOBBY_GET_ROOMLIST_PAK(rooms.Count, waitPlayers.Count, player.LastRoomPage++, player.LastPlayerPage++, count1, count2, roomListData, playerListData));
        if (player.showboxMessage && player.player_name.Length > 0)
        {
          player.showboxMessage = false;
          using (SERVER_MESSAGE_ANNOUNCE_PAK messageAnnouncePak = new SERVER_MESSAGE_ANNOUNCE_PAK("</> Point Blank Sword </>\n Participe já da nossa comunidade do Discord \n Discord: https://discord.gg/8KPvznjB4B \n Servidor Agora Comtém Anti Cheat Exclusivo. \n Adquira já seu VIP e tenha vantagens exclusivas \n </> Dúvidas, contate um membro da Staff </> "))
            this._client.SendPacket((SendPacket) messageAnnouncePak);
        }
        if (!player.firstEnterLobby || player.player_name.Length <= 0)
          return;
        player.firstEnterLobby = false;
        this._client.SendPacket((SendPacket) new LOBBY_CHATTING_PAK(ConfigGS.LobbyMessage ?? "", player.getSessionId(), 7, false, string.Format("Olá {0}, Seja muito bem vindo ao Point Blank SWORD! Players Online: {1}", (object) player.player_name, (object) GameManager._socketList.Count)));
        this._client.SendPacket((SendPacket) new LOBBY_CHATTING_PAK(ConfigGS.LobbyMessage ?? "", player.getSessionId(), 7, false, "Digite !help para ver a lista de comandos."));
        if (player.pc_cafe == 1)
          this._client.SendPacket((SendPacket) new LOBBY_CHATTING_PAK(ConfigGS.LobbyMessage ?? "", player.getSessionId(), 3, false, " [Você é VIP BASIC!] Aproveite nossa loja VIP feita especialmente para você!"));
        if (player.pc_cafe == 2)
          this._client.SendPacket((SendPacket) new LOBBY_CHATTING_PAK(ConfigGS.LobbyMessage ?? "", player.getSessionId(), 3, false, " [Você é VIP PREMIUM!] Aproveite nossa loja VIP feita especialmente para você!"));
        if (player.access == AccessLevel.Streamer)
          this._client.SendPacket((SendPacket)new LOBBY_CHATTING_PAK(ConfigGS.LobbyMessage ?? "", player.getSessionId(), 9, false, "[Você é um Streamer/Youtuber!] Ajude o servidor divulgando e suba de nivel!"));
        if (player.access == AccessLevel.Moderator)
          this._client.SendPacket((SendPacket) new LOBBY_CHATTING_PAK(ConfigGS.LobbyMessage ?? "", player.getSessionId(), 3, false, "[Você é um MODERADOR!] Seus comandos estão em: ;list1 e ;list2. [NÃO ABUSE DE SEU CARGO, POIS PODERÁ PERDÊ-LO!]"));
        if (player.access == AccessLevel.GameMaster)
          this._client.SendPacket((SendPacket) new LOBBY_CHATTING_PAK(ConfigGS.LobbyMessage ?? "", player.getSessionId(), 3, false, "[Você é um GAME MASTER!] Seus comandos estão em: ;list1 | ;list2 e ;list3. [NÃO ABUSE DE SEU CARGO, POIS PODERÁ PERDÊ-LO!]"));
        if (player.access == AccessLevel.Admin)         
                this._client.SendPacket((SendPacket) new LOBBY_CHATTING_PAK(ConfigGS.LobbyMessage ?? "", player.getSessionId(), 3, false, "[Você é um ADMINISTRADOR!] Seus comandos estão em: ;list1 | ;list2 | ;list3 e ;list4. [NÃO ABUSE DE SEU CARGO, POIS PODERÁ PERDÊ-LO!]"));
        if (player.access != AccessLevel.Developer)
          return;
        this._client.SendPacket((SendPacket) new LOBBY_CHATTING_PAK(ConfigGS.LobbyMessage ?? "", player.getSessionId(), 3, false, "[Você é um DESENVOLVEDOR!] Seus comandos estão em: ;list1 | ;list2 | ;list3 | ;list4 | ;list5 e ;list6. [NÃO ABUSE DE SEU CARGO, POIS PODERÁ PERDÊ-LO!]"));
      }
      catch (Exception ex)
      {
        Logger.warning("[LOBBY_GET_ROOMLIST_REC] " + ex.ToString());
      }
    }

    private byte[] GetRoomListData(int page, ref int count, List<Room> list)
    {
      using (SendGPacket p = new SendGPacket())
      {
        for (int index = page * 15; index < list.Count; ++index)
        {
          this.WriteRoomData(list[index], p);
          if (++count == 15)
            break;
        }
        return p.mstream.ToArray();
      }
    }

    private void WriteRoomData(Room room, SendGPacket p)
    {
      int num = 0;
      p.writeD(room._roomId);
      p.writeS(room.name, 23);
      p.writeH((short) room.mapId);
      p.writeC(room.stage4v4);
      p.writeC(room.room_type);
      p.writeC((byte) room._state);
      p.writeC((byte) room.getAllPlayers().Count);
      p.writeC((byte) room.getSlotCount());
      p.writeC((byte) room._ping);
      p.writeC(room.weaponsFlag);
      if (room.random_map > (byte) 0)
        num += 2;
      if (room.password.Length > 0)
        num += 4;
      if (room.limit > (byte) 0 && room._state > RoomState.Ready)
        num += 128;
      p.writeC((byte) num);
      p.writeC(room.special);
    }

    private void WritePlayerData(Account pl, SendGPacket p)
    {
      Clan clan = ClanManager.getClan(pl.clanId);
      p.writeD(pl.getSessionId());
      p.writeD(clan.logo);
      p.writeS(clan.name, 17);
      p.writeH((short) pl.getRank());
      p.writeS(pl.player_name, 33);
      p.writeC((byte) pl.name_color);
      p.writeC((byte) 31);
    }

    private byte[] GetPlayerListData(int page, ref int count, List<Account> list)
    {
      using (SendGPacket p = new SendGPacket())
      {
        for (int index = page * 10; index < list.Count; ++index)
        {
          this.WritePlayerData(list[index], p);
          if (++count == 10)
            break;
        }
        return p.mstream.ToArray();
      }
    }
  }
}
