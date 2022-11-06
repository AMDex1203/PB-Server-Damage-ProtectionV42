
// Type: Game.GameClient
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.server;
using Game.data.model;
using Game.data.utils;
using Game.global;
using Game.global.clientpacket;
using Game.global.serverpacket;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;

namespace Game
{
  public class GameClient : IDisposable
  {
    public long player_id;
    public Socket _client;
    public Account _player;
    public Account SessionPlayer;
    public DateTime ConnectDate;
    public DateTime SessionDate;
    public bool PacketGetRoomList;
    public uint SessionId;
    public int Shift;
    public int firstPacketId;
    private byte[] lastCompleteBuffer;
    private bool disposed;
    private bool closed;
    private SafeHandle handle = (SafeHandle) new SafeFileHandle(IntPtr.Zero, true);

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (this.disposed)
        return;
      this._player = (Account) null;
      if (this._client != null)
      {
        this._client.Dispose();
        this._client = (Socket) null;
      }
      this.player_id = 0L;
      if (disposing)
        this.handle.Dispose();
      this.disposed = true;
    }

    public GameClient(Socket client) => this._client = client;

    public void Start()
    {
      this.Shift = (int) (this.SessionId % 7U) + 1;
      new Thread(new ThreadStart(this.init)).Start();
      new Thread(new ThreadStart(this.read)).Start();
      new Thread(new ThreadStart(this.ConnectionCheck)).Start();
      this.ConnectDate = DateTime.Now;
    }

    private void ConnectionCheck()
    {
      Thread.Sleep(10000);
      if (this._client == null || this.firstPacketId != 0)
        return;
      this.Close(0);
      Logger.warning("Connection destroyed due to no responses.");
    }

    public string GetIPAddress() => this._client != null && this._client.RemoteEndPoint != null ? ((IPEndPoint) this._client.RemoteEndPoint).Address.ToString() : "";

    public IPAddress GetAddress() => this._client != null && this._client.RemoteEndPoint != null ? ((IPEndPoint) this._client.RemoteEndPoint).Address : (IPAddress) null;

    private void init() => this.SendPacket((Core.server.SendPacket) new BASE_SERVER_LIST_PAK(this));

    public bool isSocketConnected() => (!this._client.Poll(1000, SelectMode.SelectRead) || this._client.Available != 0) && this._client.Connected;

    public void SendCompletePacket(byte[] data)
    {
      try
      {
        if (data.Length < 4)
          return;
        if (ConfigGS.debugMode)
        {
          ushort uint16 = BitConverter.ToUInt16(data, 2);
          string str1 = "";
          string str2 = BitConverter.ToString(data);
          char[] chArray = new char[5]
          {
            '-',
            ',',
            '.',
            ':',
            '\t'
          };
          foreach (string str3 in str2.Split(chArray))
            str1 = str1 + " " + str3;
          Logger.warning("[" + uint16.ToString() + "]" + str1);
        }
        this._client.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(this.SendCallback), (object) this._client);
      }
      catch
      {
        this.Close(0);
      }
    }

    public void SendPacket(byte[] data)
    {
      try
      {
        if (data.Length < 2)
          return;
        ushort uint16_1 = Convert.ToUInt16(data.Length - 2);
        List<byte> byteList = new List<byte>(data.Length + 2);
        byteList.AddRange((IEnumerable<byte>) BitConverter.GetBytes(uint16_1));
        byteList.AddRange((IEnumerable<byte>) data);
        byte[] array = byteList.ToArray();
        if (ConfigGS.debugMode)
        {
          ushort uint16_2 = BitConverter.ToUInt16(data, 0);
          string str1 = "";
          string str2 = BitConverter.ToString(array);
          char[] chArray = new char[5]
          {
            '-',
            ',',
            '.',
            ':',
            '\t'
          };
          foreach (string str3 in str2.Split(chArray))
            str1 = str1 + " " + str3;
          Logger.warning("[" + uint16_2.ToString() + "]" + str1);
        }
        if (array.Length != 0)
          this._client.BeginSend(array, 0, array.Length, SocketFlags.None, new AsyncCallback(this.SendCallback), (object) this._client);
        byteList.Clear();
      }
      catch
      {
        this.Close(0);
      }
    }

    public void SendPacket(Core.server.SendPacket bp)
    {
      try
      {
        using (bp)
        {
          bp.write();
          byte[] array1 = bp.mstream.ToArray();
          if (array1.Length < 2)
            return;
          ushort uint16_1 = Convert.ToUInt16(array1.Length - 2);
          List<byte> byteList = new List<byte>(array1.Length + 2);
          byteList.AddRange((IEnumerable<byte>) BitConverter.GetBytes(uint16_1));
          byteList.AddRange((IEnumerable<byte>) array1);
          byte[] array2 = byteList.ToArray();
          if (ConfigGS.debugMode)
          {
            ushort uint16_2 = BitConverter.ToUInt16(array1, 0);
            string str1 = "";
            string str2 = BitConverter.ToString(array2);
            char[] chArray = new char[5]
            {
              '-',
              ',',
              '.',
              ':',
              '\t'
            };
            foreach (string str3 in str2.Split(chArray))
              str1 = str1 + " " + str3;
            Logger.warning("[" + uint16_2.ToString() + "]" + str1);
          }
          if (array2.Length != 0)
            this._client.BeginSend(array2, 0, array2.Length, SocketFlags.None, new AsyncCallback(this.SendCallback), (object) this._client);
          bp.mstream.Close();
          byteList.Clear();
        }
      }
      catch
      {
        this.Close(0);
      }
    }

    private void SendCallback(IAsyncResult ar)
    {
      try
      {
        Socket asyncState = (Socket) ar.AsyncState;
        if (asyncState == null || !asyncState.Connected)
          return;
        asyncState.EndSend(ar);
      }
      catch
      {
        this.Close(0);
      }
    }

    private void read()
    {
      try
      {
        GameClient.StateObject stateObject = new GameClient.StateObject();
        stateObject.workSocket = this._client;
        this._client.BeginReceive(stateObject.buffer, 0, 8096, SocketFlags.None, new AsyncCallback(this.OnReceiveCallback), (object) stateObject);
      }
      catch
      {
        this.Close(0);
      }
    }

    private void OnReceiveCallback(IAsyncResult ar)
    {
      try
      {
        GameClient.StateObject asyncState = (GameClient.StateObject) ar.AsyncState;
        int length = asyncState.workSocket.EndReceive(ar);
        if (length <= 0)
          return;
        byte[] buffer = new byte[length];
        Array.Copy((Array) asyncState.buffer, 0, (Array) buffer, 0, length);
        int FirstLength = (int) BitConverter.ToUInt16(buffer, 0) & (int) short.MaxValue;
        byte[] numArray = new byte[FirstLength + 2];
        Array.Copy((Array) buffer, 2, (Array) numArray, 0, numArray.Length);
        this.lastCompleteBuffer = buffer;
        this.RunPacket(ComDiv.decrypt(numArray, this.Shift), numArray);
        this.checkoutN(buffer, FirstLength);
        new Thread(new ThreadStart(this.read)).Start();
      }
      catch
      {
        this.Close(0);
      }
    }

    public void checkoutN(byte[] buffer, int FirstLength)
    {
      int length = buffer.Length;
      try
      {
        byte[] numArray = new byte[length - FirstLength - 4];
        Array.Copy((Array) buffer, FirstLength + 4, (Array) numArray, 0, numArray.Length);
        if (numArray.Length == 0)
          return;
        int FirstLength1 = (int) BitConverter.ToUInt16(numArray, 0) & (int) short.MaxValue;
        byte[] data = new byte[FirstLength1 + 2];
        Array.Copy((Array) numArray, 2, (Array) data, 0, data.Length);
        byte[] buff = new byte[FirstLength1 + 2];
        Array.Copy((Array) ComDiv.decrypt(data, this.Shift), 0, (Array) buff, 0, buff.Length);
        this.RunPacket(buff, numArray);
        this.checkoutN(numArray, FirstLength1);
      }
      catch
      {
      }
    }

    public void Close(int time, bool kicked = false)
    {
      if (!this.closed)
      {
        try
        {
          this.closed = true;
          GameManager.RemoveSocket(this);
          Account player = this._player;
          if (this.player_id > 0L && player != null)
          {
            Channel channel = player.getChannel();
            Room room = player._room;
            Match match = player._match;
            player.setOnlineStatus(false);
            room?.RemovePlayer(player, false, kicked ? 1 : 0);
            match?.RemovePlayer(player);
            channel?.RemovePlayer(player);
            player._status.ResetData(this.player_id);
            AllUtils.syncPlayerToFriends(player, false);
            AllUtils.syncPlayerToClanMembers(player);
            player.SimpleClear();
            player.updateCacheInfo();
            this._player = (Account) null;
          }
          this.player_id = 0L;
        }
        catch (Exception ex)
        {
          Logger.warning("Error by: " + this.player_id.ToString() + "\n" + ex.ToString());
        }
      }
      if (this._client != null)
        this._client.Close(time);
      this.Dispose();
    }

    private void FirstPacketCheck(ushort packetId)
    {
      if (this.firstPacketId != 0)
        return;
      this.firstPacketId = (int) packetId;
      if (packetId == (ushort) 2579 || packetId == (ushort) 2644)
        return;
      this.Close(0);
      Logger.warning("Connection destroyed due to Unknown first packet. [" + packetId.ToString() + "]");
    }

    private void RunPacket(byte[] buff, byte[] simple)
    {
      ushort uint16 = BitConverter.ToUInt16(buff, 0);
      this.FirstPacketCheck(uint16);
      if (this.closed)
        return;
      ReceiveGamePacket receiveGamePacket = (ReceiveGamePacket) null;
      switch (uint16)
      {
        case 275:
          receiveGamePacket = (ReceiveGamePacket) new FRIEND_INVITE_FOR_ROOM_REC(this, buff);
          goto case 2694;
        case 280:
          receiveGamePacket = (ReceiveGamePacket) new FRIEND_ACCEPT_REC(this, buff);
          goto case 2694;
        case 282:
          receiveGamePacket = (ReceiveGamePacket) new FRIEND_INVITE_REC(this, buff);
          goto case 2694;
        case 284:
          receiveGamePacket = (ReceiveGamePacket) new FRIEND_DELETE_REC(this, buff);
          goto case 2694;
        case 290:
          receiveGamePacket = (ReceiveGamePacket) new AUTH_SEND_WHISPER_REC(this, buff);
          goto case 2694;
        case 292:
          receiveGamePacket = (ReceiveGamePacket) new AUTH_SEND_WHISPER2_REC(this, buff);
          goto case 2694;
        case 297:
          receiveGamePacket = (ReceiveGamePacket) new AUTH_FIND_USER_REC(this, buff);
          goto case 2694;
        case 417:
          receiveGamePacket = (ReceiveGamePacket) new BOX_MESSAGE_CREATE_REC(this, buff);
          goto case 2694;
        case 419:
          receiveGamePacket = (ReceiveGamePacket) new BOX_MESSAGE_REPLY_REC(this, buff);
          goto case 2694;
        case 422:
          receiveGamePacket = (ReceiveGamePacket) new BOX_MESSAGE_VIEW_REC(this, buff);
          goto case 2694;
        case 424:
          receiveGamePacket = (ReceiveGamePacket) new BOX_MESSAGE_DELETE_REC(this, buff);
          goto case 2694;
        case 530:
          receiveGamePacket = (ReceiveGamePacket) new SHOP_BUY_ITEM_REC(this, buff);
          goto case 2694;
        case 534:
          receiveGamePacket = (ReceiveGamePacket) new INVENTORY_ITEM_EQUIP_REC(this, buff);
          goto case 2694;
        case 536:
          receiveGamePacket = (ReceiveGamePacket) new INVENTORY_ITEM_EFFECT_REC(this, buff);
          goto case 2694;
        case 540:
          receiveGamePacket = (ReceiveGamePacket) new BOX_MESSAGE_GIFT_TAKE_REC(this, buff);
          goto case 2694;
        case 542:
          receiveGamePacket = (ReceiveGamePacket) new INVENTORY_ITEM_EXCLUDE_REC(this, buff);
          goto case 2694;
        case 544:
          receiveGamePacket = (ReceiveGamePacket) new AUTH_WEB_CASH_REC(this, buff);
          goto case 2694;
        case 548:
          receiveGamePacket = (ReceiveGamePacket) new AUTH_CHECK_NICKNAME_REC(this, buff);
          goto case 2694;
        case 1304:
          receiveGamePacket = (ReceiveGamePacket) new CLAN_GET_INFO_REC(this, buff);
          goto case 2694;
        case 1306:
          receiveGamePacket = (ReceiveGamePacket) new CLAN_MEMBER_CONTEXT_REC(this, buff);
          goto case 2694;
        case 1308:
          receiveGamePacket = (ReceiveGamePacket) new CLAN_MEMBER_LIST_REC(this, buff);
          goto case 2694;
        case 1310:
          receiveGamePacket = (ReceiveGamePacket) new CLAN_CREATE_REC(this, buff);
          goto case 2694;
        case 1312:
          receiveGamePacket = (ReceiveGamePacket) new CLAN_CLOSE_REC(this, buff);
          goto case 2694;
        case 1314:
          receiveGamePacket = (ReceiveGamePacket) new CLAN_CHECK_CREATE_INVITE_REC(this, buff);
          goto case 2694;
        case 1316:
          receiveGamePacket = (ReceiveGamePacket) new CLAN_CREATE_INVITE_REC(this, buff);
          goto case 2694;
        case 1318:
          receiveGamePacket = (ReceiveGamePacket) new CLAN_PLAYER_CLEAN_INVITES_REC(this, buff);
          goto case 2694;
        case 1320:
          receiveGamePacket = (ReceiveGamePacket) new CLAN_REQUEST_CONTEXT_REC(this, buff);
          goto case 2694;
        case 1322:
          receiveGamePacket = (ReceiveGamePacket) new CLAN_REQUEST_LIST_REC(this, buff);
          goto case 2694;
        case 1324:
          receiveGamePacket = (ReceiveGamePacket) new CLAN_REQUEST_INFO_REC(this, buff);
          goto case 2694;
        case 1326:
          receiveGamePacket = (ReceiveGamePacket) new CLAN_REQUEST_ACCEPT_REC(this, buff);
          goto case 2694;
        case 1329:
          receiveGamePacket = (ReceiveGamePacket) new CLAN_REQUEST_DENIAL_REC(this, buff);
          goto case 2694;
        case 1332:
          receiveGamePacket = (ReceiveGamePacket) new CLAN_PLAYER_LEAVE_REC(this, buff);
          goto case 2694;
        case 1334:
          receiveGamePacket = (ReceiveGamePacket) new CLAN_DEMOTE_KICK_REC(this, buff);
          goto case 2694;
        case 1337:
          receiveGamePacket = (ReceiveGamePacket) new CLAN_PROMOTE_MASTER_REC(this, buff);
          goto case 2694;
        case 1340:
          receiveGamePacket = (ReceiveGamePacket) new CLAN_PROMOTE_AUX_REC(this, buff);
          goto case 2694;
        case 1343:
          receiveGamePacket = (ReceiveGamePacket) new CLAN_DEMOTE_NORMAL_REC(this, buff);
          goto case 2694;
        case 1358:
          receiveGamePacket = (ReceiveGamePacket) new CLAN_CHATTING_REC(this, buff);
          goto case 2694;
        case 1360:
          receiveGamePacket = (ReceiveGamePacket) new CLAN_CHECK_DUPLICATE_LOGO_REC(this, buff);
          goto case 2694;
        case 1362:
          receiveGamePacket = (ReceiveGamePacket) new CLAN_REPLACE_NOTICE_REC(this, buff);
          goto case 2694;
        case 1364:
          receiveGamePacket = (ReceiveGamePacket) new CLAN_REPLACE_INTRO_REC(this, buff);
          goto case 2694;
        case 1372:
          receiveGamePacket = (ReceiveGamePacket) new CLAN_SAVEINFO3_REC(this, buff);
          goto case 2694;
        case 1381:
          receiveGamePacket = (ReceiveGamePacket) new CLAN_ROOM_INVITED_REC(this, buff);
          goto case 2694;
        case 1390:
          receiveGamePacket = (ReceiveGamePacket) new CLAN_CHAT_1390_REC(this, buff);
          goto case 2694;
        case 1392:
          receiveGamePacket = (ReceiveGamePacket) new CLAN_MESSAGE_INVITE_REC(this, buff);
          goto case 2694;
        case 1394:
          receiveGamePacket = (ReceiveGamePacket) new CLAN_MESSAGE_REQUEST_INTERACT_REC(this, buff);
          goto case 2694;
        case 1396:
          receiveGamePacket = (ReceiveGamePacket) new CLAN_MSG_FOR_PLAYERS_REC(this, buff);
          goto case 2694;
        case 1416:
          receiveGamePacket = (ReceiveGamePacket) new CLAN_CREATE_REQUIREMENTS_REC(this, buff);
          goto case 2694;
        case 1441:
          receiveGamePacket = (ReceiveGamePacket) new CLAN_CLIENT_ENTER_REC(this, buff);
          goto case 2694;
        case 1443:
          receiveGamePacket = (ReceiveGamePacket) new CLAN_CLIENT_LEAVE_REC(this, buff);
          goto case 2694;
        case 1445:
          receiveGamePacket = (ReceiveGamePacket) new CLAN_CLIENT_CLAN_LIST_REC(this, buff);
          goto case 2694;
        case 1447:
          receiveGamePacket = (ReceiveGamePacket) new CLAN_CHECK_DUPLICATE_NAME_REC(this, buff);
          goto case 2694;
        case 1451:
          receiveGamePacket = (ReceiveGamePacket) new CLAN_CLIENT_CLAN_CONTEXT_REC(this, buff);
          goto case 2694;
        case 1538:
          receiveGamePacket = (ReceiveGamePacket) new CLAN_WAR_PARTY_CONTEXT_REC(this, buff);
          goto case 2694;
        case 1540:
          receiveGamePacket = (ReceiveGamePacket) new CLAN_WAR_PARTY_LIST_REC(this, buff);
          goto case 2694;
        case 1542:
          receiveGamePacket = (ReceiveGamePacket) new CLAN_WAR_MATCH_TEAM_CONTEXT_REC(this, buff);
          goto case 2694;
        case 1544:
          receiveGamePacket = (ReceiveGamePacket) new CLAN_WAR_MATCH_TEAM_LIST_REC(this, buff);
          goto case 2694;
        case 1546:
          receiveGamePacket = (ReceiveGamePacket) new CLAN_WAR_CREATE_TEAM_REC(this, buff);
          goto case 2694;
        case 1548:
          receiveGamePacket = (ReceiveGamePacket) new CLAN_WAR_JOIN_TEAM_REC(this, buff);
          goto case 2694;
        case 1550:
          receiveGamePacket = (ReceiveGamePacket) new CLAN_WAR_LEAVE_TEAM_REC(this, buff);
          goto case 2694;
        case 1553:
          receiveGamePacket = (ReceiveGamePacket) new CLAN_WAR_PROPOSE_REC(this, buff);
          goto case 2694;
        case 1558:
          receiveGamePacket = (ReceiveGamePacket) new CLAN_WAR_ACCEPT_BATTLE_REC(this, buff);
          goto case 2694;
        case 1565:
          receiveGamePacket = (ReceiveGamePacket) new CLAN_WAR_CREATE_ROOM_REC(this, buff);
          goto case 2694;
        case 1567:
          receiveGamePacket = (ReceiveGamePacket) new CLAN_WAR_JOIN_ROOM_REC(this, buff);
          goto case 2694;
        case 1569:
          receiveGamePacket = (ReceiveGamePacket) new CLAN_WAR_MATCH_TEAM_INFO_REC(this, buff);
          goto case 2694;
        case 1571:
          receiveGamePacket = (ReceiveGamePacket) new CLAN_WAR_UPTIME_REC(this, buff);
          goto case 2694;
        case 1576:
          receiveGamePacket = (ReceiveGamePacket) new CLAN_WAR_TEAM_CHATTING_REC(this, buff);
          goto case 2694;
        case 2571:
          receiveGamePacket = (ReceiveGamePacket) new BASE_CHANNEL_LIST_REC(this, buff);
          goto case 2694;
        case 2573:
          receiveGamePacket = (ReceiveGamePacket) new BASE_CHANNEL_ENTER_REC(this, buff);
          goto case 2694;
        case 2575:
          receiveGamePacket = (ReceiveGamePacket) new BASE_HEARTBEAT_REC(this, buff);
          goto case 2694;
        case 2577:
          receiveGamePacket = (ReceiveGamePacket) new BASE_SERVER_CHANGE_REC(this, buff);
          goto case 2694;
        case 2579:
          receiveGamePacket = (ReceiveGamePacket) new BASE_USER_ENTER_REC(this, buff);
          goto case 2694;
        case 2581:
          receiveGamePacket = (ReceiveGamePacket) new BASE_CONFIG_SAVE_REC(this, buff);
          goto case 2694;
        case 2584:
          receiveGamePacket = (ReceiveGamePacket) new CM_2584(this, buff);
          goto case 2694;
        case 2591:
          receiveGamePacket = (ReceiveGamePacket) new BASE_GET_USER_STATS_REC(this, buff);
          goto case 2694;
        case 2601:
          receiveGamePacket = (ReceiveGamePacket) new BASE_MISSION_ENTER_REC(this, buff);
          goto case 2694;
        case 2605:
          receiveGamePacket = (ReceiveGamePacket) new BASE_QUEST_BUY_CARD_SET_REC(this, buff);
          goto case 2694;
        case 2607:
          receiveGamePacket = (ReceiveGamePacket) new BASE_QUEST_DELETE_CARD_SET_REC(this, buff);
          goto case 2694;
        case 2619:
          receiveGamePacket = (ReceiveGamePacket) new BASE_TITLE_GET_REC(this, buff);
          goto case 2694;
        case 2621:
          receiveGamePacket = (ReceiveGamePacket) new BASE_TITLE_USE_REC(this, buff);
          goto case 2694;
        case 2623:
          receiveGamePacket = (ReceiveGamePacket) new BASE_TITLE_DETACH_REC(this, buff);
          goto case 2694;
        case 2627:
          receiveGamePacket = (ReceiveGamePacket) new BASE_CHATTING_REC(this, buff);
          goto case 2694;
        case 2635:
          receiveGamePacket = (ReceiveGamePacket) new BASE_MISSION_SUCCESS_REC(this, buff);
          goto case 2694;
        case 2639:
          receiveGamePacket = (ReceiveGamePacket) new LOBBY_GET_PLAYERINFO_REC(this, buff);
          goto case 2694;
        case 2642:
          receiveGamePacket = (ReceiveGamePacket) new BASE_SERVER_LIST_REFRESH_REC(this, buff);
          goto case 2694;
        case 2644:
          receiveGamePacket = (ReceiveGamePacket) new BASE_SERVER_PASSW_REC(this, buff);
          goto case 2694;
        case 2654:
          receiveGamePacket = (ReceiveGamePacket) new BASE_USER_EXIT_REC(this, buff);
          goto case 2694;
        case 2661:
          receiveGamePacket = (ReceiveGamePacket) new EVENT_VISIT_CONFIRM_REC(this, buff);
          goto case 2694;
        case 2663:
          receiveGamePacket = (ReceiveGamePacket) new EVENT_VISIT_REWARD_REC(this, buff);
          goto case 2694;
        case 2684:
          receiveGamePacket = (ReceiveGamePacket) new GM_LOG_LOBBY_REC(this, buff);
          goto case 2694;
        case 2686:
          receiveGamePacket = (ReceiveGamePacket) new GM_LOG_ROOM_REC(this, buff);
          goto case 2694;
        case 2694:
          if (receiveGamePacket == null)
            break;
          new Thread(new ThreadStart(receiveGamePacket.run)).Start();
          break;
        case 2817:
          receiveGamePacket = (ReceiveGamePacket) new SHOP_LEAVE_REC(this, buff);
          goto case 2694;
        case 2819:
          receiveGamePacket = (ReceiveGamePacket) new SHOP_ENTER_REC(this, buff);
          goto case 2694;
        case 2821:
          receiveGamePacket = (ReceiveGamePacket) new SHOP_LIST_REC(this, buff);
          goto case 2694;
        case 3073:
          receiveGamePacket = (ReceiveGamePacket) new LOBBY_GET_ROOMLIST_REC(this, buff);
          goto case 2694;
        case 3077:
          receiveGamePacket = (ReceiveGamePacket) new LOBBY_QUICKJOIN_ROOM_REC(this, buff);
          goto case 2694;
        case 3079:
          receiveGamePacket = (ReceiveGamePacket) new LOBBY_ENTER_REC(this, buff);
          goto case 2694;
        case 3081:
          receiveGamePacket = (ReceiveGamePacket) new LOBBY_JOIN_ROOM_REC(this, buff);
          goto case 2694;
        case 3083:
          receiveGamePacket = (ReceiveGamePacket) new LOBBY_LEAVE_REC(this, buff);
          goto case 2694;
        case 3087:
          receiveGamePacket = (ReceiveGamePacket) new LOBBY_GET_ROOMINFO_REC(this, buff);
          goto case 2694;
        case 3089:
          receiveGamePacket = (ReceiveGamePacket) new LOBBY_CREATE_ROOM_REC(this, buff);
          goto case 2694;
        case 3094:
          receiveGamePacket = (ReceiveGamePacket) new A_3094_REC(this, buff);
          goto case 2694;
        case 3099:
          receiveGamePacket = (ReceiveGamePacket) new LOBBY_GET_PLAYERINFO2_REC(this, buff);
          goto case 2694;
        case 3101:
          receiveGamePacket = (ReceiveGamePacket) new LOBBY_CREATE_NICK_NAME_REC(this, buff);
          goto case 2694;
        case 3329:
          receiveGamePacket = (ReceiveGamePacket) new BATTLE_3329_REC(this, buff);
          goto case 2694;
        case 3331:
          receiveGamePacket = (ReceiveGamePacket) new BATTLE_READYBATTLE_REC(this, buff);
          goto case 2694;
        case 3333:
          receiveGamePacket = (ReceiveGamePacket) new BATTLE_STARTBATTLE_REC(this, buff);
          goto case 2694;
        case 3337:
          receiveGamePacket = (ReceiveGamePacket) new BATTLE_RESPAWN_REC(this, buff);
          goto case 2694;
        case 3344:
          receiveGamePacket = (ReceiveGamePacket) new BATTLE_SENDPING_REC(this, buff);
          goto case 2694;
        case 3348:
          receiveGamePacket = (ReceiveGamePacket) new BATTLE_PRESTARTBATTLE_REC(this, buff);
          goto case 2694;
        case 3354:
          receiveGamePacket = (ReceiveGamePacket) new BATTLE_DEATH_REC(this, buff);
          goto case 2694;
        case 3356:
          receiveGamePacket = (ReceiveGamePacket) new BATTLE_MISSION_BOMB_INSTALL_REC(this, buff);
          goto case 2694;
        case 3358:
          receiveGamePacket = (ReceiveGamePacket) new BATTLE_MISSION_BOMB_UNINSTALL_REC(this, buff);
          goto case 2694;
        case 3368:
          receiveGamePacket = (ReceiveGamePacket) new BATTLE_MISSION_GENERATOR_INFO_REC(this, buff);
          goto case 2694;
        case 3372:
          receiveGamePacket = (ReceiveGamePacket) new BATTLE_TIMERSYNC_REC(this, buff);
          goto case 2694;
        case 3376:
          receiveGamePacket = (ReceiveGamePacket) new BATTLE_CHANGE_DIFFICULTY_LEVEL_REC(this, buff);
          goto case 2694;
        case 3378:
          receiveGamePacket = (ReceiveGamePacket) new BATTLE_RESPAWN_FOR_AI_REC(this, buff);
          goto case 2694;
        case 3384:
          receiveGamePacket = (ReceiveGamePacket) new BATTLE_PLAYER_LEAVE_REC(this, buff);
          goto case 2694;
        case 3386:
          receiveGamePacket = (ReceiveGamePacket) new BATTLE_MISSION_DEFENCE_INFO_REC(this, buff);
          goto case 2694;
        case 3390:
          receiveGamePacket = (ReceiveGamePacket) new BATTLE_DINO_DEATHBLOW_REC(this, buff);
          goto case 2694;
        case 3394:
          receiveGamePacket = (ReceiveGamePacket) new BATTLE_ENDTUTORIAL_REC(this, buff);
          goto case 2694;
        case 3396:
          receiveGamePacket = (ReceiveGamePacket) new VOTEKICK_START_REC(this, buff);
          goto case 2694;
        case 3400:
          receiveGamePacket = (ReceiveGamePacket) new VOTEKICK_UPDATE_REC(this, buff);
          goto case 2694;
        case 3428:
          receiveGamePacket = (ReceiveGamePacket) new A_3428_REC(this, buff);
          goto case 2694;
        case 3585:
          receiveGamePacket = (ReceiveGamePacket) new INVENTORY_ENTER_REC(this, buff);
          goto case 2694;
        case 3589:
          receiveGamePacket = (ReceiveGamePacket) new INVENTORY_LEAVE_REC(this, buff);
          goto case 2694;
        case 3841:
          receiveGamePacket = (ReceiveGamePacket) new ROOM_GET_PLAYERINFO_REC(this, buff);
          goto case 2694;
        case 3845:
          receiveGamePacket = (ReceiveGamePacket) new ROOM_CHANGE_SLOT_REC(this, buff);
          goto case 2694;
        case 3847:
          receiveGamePacket = (ReceiveGamePacket) new BATTLE_ROOM_INFO_REC(this, buff);
          goto case 2694;
        case 3849:
          receiveGamePacket = (ReceiveGamePacket) new ROOM_CLOSE_SLOT_REC(this, buff);
          goto case 2694;
        case 3854:
          receiveGamePacket = (ReceiveGamePacket) new ROOM_GET_LOBBY_USER_LIST_REC(this, buff);
          goto case 2694;
        case 3858:
          receiveGamePacket = (ReceiveGamePacket) new ROOM_CHANGE_INFO2_REC(this, buff);
          goto case 2694;
        case 3862:
          receiveGamePacket = (ReceiveGamePacket) new BASE_PROFILE_ENTER_REC(this, buff);
          goto case 2694;
        case 3864:
          receiveGamePacket = (ReceiveGamePacket) new BASE_PROFILE_LEAVE_REC(this, buff);
          goto case 2694;
        case 3866:
          receiveGamePacket = (ReceiveGamePacket) new ROOM_REQUEST_HOST_REC(this, buff);
          goto case 2694;
        case 3868:
          receiveGamePacket = (ReceiveGamePacket) new ROOM_RANDOM_HOST2_REC(this, buff);
          goto case 2694;
        case 3870:
          receiveGamePacket = (ReceiveGamePacket) new ROOM_CHANGE_HOST_REC(this, buff);
          goto case 2694;
        case 3872:
          receiveGamePacket = (ReceiveGamePacket) new ROOM_RANDOM_HOST_REC(this, buff);
          goto case 2694;
        case 3874:
          receiveGamePacket = (ReceiveGamePacket) new ROOM_CHANGE_TEAM_REC(this, buff);
          goto case 2694;
        case 3884:
          receiveGamePacket = (ReceiveGamePacket) new ROOM_INVITE_PLAYERS_REC(this, buff);
          goto case 2694;
        case 3886:
          receiveGamePacket = (ReceiveGamePacket) new ROOM_CHANGE_INFO_REC(this, buff);
          goto case 2694;
        case 3890:
          receiveGamePacket = (ReceiveGamePacket) new A_3890_REC(this, buff);
          goto case 2694;
        case 3894:
          receiveGamePacket = (ReceiveGamePacket) new A_3894_REC(this, buff);
          goto case 2694;
        case 3900:
          receiveGamePacket = (ReceiveGamePacket) new A_3900_REC(this, buff);
          goto case 2694;
        case 3902:
          receiveGamePacket = (ReceiveGamePacket) new A_3902_REC(this, buff);
          goto case 2694;
        case 3904:
          receiveGamePacket = (ReceiveGamePacket) new BATTLE_LOADING_REC(this, buff);
          goto case 2694;
        case 3906:
          receiveGamePacket = (ReceiveGamePacket) new ROOM_CHANGE_PASSW_REC(this, buff);
          goto case 2694;
        case 3910:
          receiveGamePacket = (ReceiveGamePacket) new EVENT_PLAYTIME_REWARD_REC(this, buff);
          goto case 2694;
        default:
          StringUtil stringUtil = new StringUtil();
          stringUtil.AppendLine("|[GC]| Opcode não encontrado " + uint16.ToString());
          stringUtil.AppendLine("Encry/SemLength/Cheio: " + BitConverter.ToString(simple));
          stringUtil.AppendLine("SemEnc/SemLength/Cheio: " + BitConverter.ToString(buff));
          stringUtil.AppendLine("Enc/ComLength/TUDO: " + BitConverter.ToString(this.lastCompleteBuffer));
          stringUtil.AppendLine("pId: " + this.player_id.ToString());
          Logger.error(stringUtil.getString());
          goto case 2694;
      }
    }

    private class StateObject
    {
      public Socket workSocket;
      public const int BufferSize = 8096;
      public byte[] buffer = new byte[8096];
    }
  }
}
