
// Type: Battle.network.BattleHandler
// Assembly: pbserver_battle, Version=1.0.0.2646, Culture=neutral, PublicKeyToken=null
// MVID: AA2D7911-B687-4238-AB73-D37E54123039
// Interprise: C:\Users\Cuzin\3,50pbserver_battle.exe

using Battle.config;
using Battle.data;
using Battle.data.enums;
using Battle.data.enums.bomb;
using Battle.data.enums.weapon;
using Battle.data.models;
using Battle.data.sync;
using Battle.data.xml;
using Battle.network.actions.damage;
using Battle.network.actions.others;
using Battle.network.actions.user;
using Battle.network.packets;
using SharpDX;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Battle.network
{
  public class BattleHandler
  {
    private UdpClient _client;
    private IPEndPoint remoteEP;

    public BattleHandler(UdpClient client, byte[] buff, IPEndPoint remote, DateTime date)
    {
      this._client = client;
      this.remoteEP = remote;
      this.BeginReceive(buff, date);
    }

    public void BeginReceive(byte[] buffer, DateTime date)
    {
      PacketModel packet = new PacketModel();
      packet._data = buffer;
      packet._receiveDate = date;
      ReceivePacket receivePacket = new ReceivePacket(packet._data);
      packet._opcode = (int) receivePacket.readC();
      packet._slot = (int) receivePacket.readC();
      packet._time = receivePacket.readT();
      packet._round = (int) receivePacket.readC();
      packet._length = (int) receivePacket.readUH();
      packet._respawnNumber = (int) receivePacket.readC();
      packet._roundNumber = (int) receivePacket.readC();
      packet._accountId = (int) receivePacket.readC();
      packet._unkInfo2 = (int) receivePacket.readC();
      if (packet._length > packet._data.Length)
      {
        Logger.warning("Pacote com tamanho inválido cancelado. [Length: " + packet._length.ToString() + "; DataLength: " + packet._data.Length.ToString() + "]");
      }
      else
      {
        this.getDecryptedData(packet);
        if (Config.isTestMode && packet._unkInfo2 > 0)
        {
          Logger.warning("[N] Unk: " + packet._unkInfo2.ToString() + " [" + BitConverter.ToString(packet._data) + "]");
          Logger.warning("[D] Unk: " + packet._unkInfo2.ToString() + " [" + BitConverter.ToString(packet._withEndData) + "]");
        }
        if (Config.enableLog && (packet._opcode == 3 || packet._opcode == 4 || packet._opcode == 131 || packet._opcode == 132))
          this.LoggerTestMode(packet._noEndData, packet);
        this.ReadPacket(packet);
      }
    }

    public void getDecryptedData(PacketModel packet)
    {
      if (packet._data.Length < packet._length)
        throw new Exception("Tamanho do pacote inválido.");
      byte[] data = new byte[packet._length - 13];
      Array.Copy((Array) packet._data, 13, (Array) data, 0, data.Length);
      byte[] numArray1 = AllUtils.decrypt(data, packet._length % 6 + 1);
      byte[] numArray2 = new byte[numArray1.Length - 9];
      Array.Copy((Array) numArray1, (Array) numArray2, numArray2.Length);
      packet._withEndData = numArray1;
      packet._noEndData = numArray2;
    }

    public void ReadPacket(PacketModel packet)
    {
      byte[] withEndData = packet._withEndData;
      byte[] noEndData = packet._noEndData;
      ReceivePacket receivePacket = new ReceivePacket(withEndData);
      int length = noEndData.Length;
      int num1 = 0;
      int num2 = 0;
      try
      {
        switch (packet._opcode)
        {
          case 3:
          case 4:
            receivePacket.Advance(length);
            uint UniqueRoomId1 = receivePacket.readUD();
            int num3 = (int) receivePacket.readC();
            num1 = receivePacket.readD();
            Room room1 = RoomsManager.getRoom(UniqueRoomId1);
            if (room1 == null || room1._startTime == new DateTime())
              break;
            Player player1 = room1.getPlayer(packet._slot, this.remoteEP);
            if (player1 != null && player1.AccountIdIsValid(packet._accountId))
            {
              player1._respawnByUser = packet._respawnNumber;
              if (packet._opcode == 4)
                room1._isBotMode = true;
              byte[] actions = this.WriteActionBytes(noEndData, room1, AllUtils.GetDuration(player1._date), packet);
              bool flag1 = packet._opcode == 4 && num3 == (int) byte.MaxValue;
              int slot = 0;
              if (flag1)
                slot = packet._slot;
              else if (packet._opcode == 3)
                slot = room1._isBotMode ? packet._slot : (int) byte.MaxValue;
              else
                Logger.warning("[Dedication invalid] d: " + num3.ToString() + "; s: " + packet._slot.ToString() + "; opc: " + packet._opcode.ToString());
              byte[] code4 = Packet4Creator.getCode4(actions, flag1 ? player1._date : room1._startTime, packet._round, slot);
              bool flag2 = packet._opcode == 3 && !room1._isBotMode && num3 != (int) byte.MaxValue;
              for (int index = 0; index < 16; ++index)
              {
                bool flag3 = index != packet._slot;
                Player player2 = room1._players[index];
                if (player2._client != null && player2.AccountIdIsValid() && ((num3 == (int) byte.MaxValue & flag3 ? 1 : (packet._opcode != 3 ? 0 : (room1._isBotMode ? 1 : 0)) & (flag3 ? 1 : 0)) | (flag2 ? 1 : 0)) != 0)
                  this.Send(code4, player2._client);
              }
            }
            break;
          case 65:
            int num4 = receivePacket.readD();
            string str1 = num4.ToString();
            num4 = receivePacket.readD();
            string str2 = num4.ToString();
            string udp = str1 + "." + str2;
            uint UniqueRoomId2 = receivePacket.readUD();
            int gen2 = receivePacket.readD();
            num2 = (int) receivePacket.readC();
            Room orGetRoom = RoomsManager.CreateOrGetRoom(UniqueRoomId2, gen2);
            if (orGetRoom == null)
              break;
            Player player3 = orGetRoom.AddPlayer(this.remoteEP, packet, udp);
            if (player3 != null)
            {
              if (!player3.Integrity)
                player3.ResetBattleInfos();
              this.Send(Packet66Creator.getCode66(), player3._client);
              string[] strArray = new string[5]
              {
                "[Batalha] Jogador se conectou na batalha. [",
                player3._client.Address?.ToString(),
                ":",
                null,
                null
              };
              num4 = player3._client.Port;
              strArray[3] = num4.ToString();
              strArray[4] = "]";
              Logger.warning(string.Concat(strArray));
            }
            break;
          case 67:
            receivePacket.readB(8);
            uint UniqueRoomId3 = receivePacket.readUD();
            num1 = receivePacket.readD();
            num2 = (int) receivePacket.readC();
            Room room2 = RoomsManager.getRoom(UniqueRoomId3);
            if (room2 == null)
              break;
            if (room2.RemovePlayer(packet._slot, this.remoteEP))
              Logger.warning("[Batalha] Jogador desconectou da batalha. [" + this.remoteEP.Address?.ToString() + ":" + this.remoteEP.Port.ToString() + "]");
            if (room2.getPlayersCount() == 0)
              RoomsManager.RemoveRoom(room2.UniqueRoomId);
            break;
          case 97:
            uint UniqueRoomId4 = receivePacket.readUD();
            num2 = (int) receivePacket.readC();
            num1 = receivePacket.readD();
            Room room3 = RoomsManager.getRoom(UniqueRoomId4);
            if (room3 == null)
              break;
            Player player4 = room3.getPlayer(this.remoteEP);
            if (player4 != null)
            {
              player4._date = packet._receiveDate;
              this.Send(packet._data, this.remoteEP);
            }
            break;
          case 131:
          case 132:
            receivePacket.Advance(length);
            uint UniqueRoomId5 = receivePacket.readUD();
            int slot1 = (int) receivePacket.readC();
            num1 = receivePacket.readD();
            Room room4 = RoomsManager.getRoom(UniqueRoomId5);
            if (room4 == null)
              break;
            Player player5 = room4.getPlayer(packet._slot, this.remoteEP);
            if (player5 != null && player5.AccountIdIsValid(packet._accountId))
            {
              room4._isBotMode = true;
              Player player2 = slot1 != (int) byte.MaxValue ? room4.getPlayer(slot1, false) : (Player) null;
              byte[] data = player2 == null ? Packet132Creator.getCode132(noEndData, player5._date, packet._round, packet._slot) : Packet132Creator.getCode132(noEndData, player2._date, packet._round, slot1);
              for (int index = 0; index < 16; ++index)
              {
                Player player6 = room4._players[index];
                if (player6._client != null && player6.AccountIdIsValid() && index != packet._slot)
                  this.Send(data, player6._client);
              }
            }
            break;
          default:
            Logger.warning("Pacote inválido: " + packet._opcode.ToString());
            Logger.warning("[Decrypted2] Data: " + BitConverter.ToString(withEndData));
            Logger.warning("[Encrypted2] Data: " + BitConverter.ToString(packet._data));
            break;
        }
      }
      catch (Exception ex)
      {
        Logger.warning(ex.ToString());
        Logger.warning("[Decrypted] Data: " + BitConverter.ToString(withEndData));
        Logger.warning("[Encrypted] Data: " + BitConverter.ToString(packet._data));
      }
    }

    public void LoggerTestMode(byte[] data, PacketModel packet)
    {
      ReceivePacket p = new ReceivePacket(data);
      if (packet._opcode == 131 || packet._opcode == 132)
      {
        double num = (double) p.readT();
      }
      for (int index = 0; index < 16; ++index)
      {
        ActionModel ac = new ActionModel();
        try
        {
          bool exception;
          ac._type = (P2P_SUB_HEAD) p.readC(out exception);
          if (exception)
            break;
          ac._slot = p.readUH();
          ac._lengthData = p.readUH();
          if (ac._lengthData == ushort.MaxValue)
            break;
          if (ac._type == P2P_SUB_HEAD.GRENADE)
            code1_GrenadeSync.ReadInfo(p, false, true);
          else if (ac._type == P2P_SUB_HEAD.DROPEDWEAPON)
            code2_WeaponSync.ReadInfo(p, false);
          else if (ac._type == P2P_SUB_HEAD.OBJECT_STATIC)
            code3_ObjectStatic.ReadInfo(p, false);
          else if (ac._type == P2P_SUB_HEAD.OBJECT_ANIM)
            code6_ObjectAnim.ReadInfo(p, false);
          else if (ac._type == P2P_SUB_HEAD.STAGEINFO_OBJ_STATIC)
            code9_StageInfoObjStatic.readSyncInfo(p, false);
          else if (ac._type == P2P_SUB_HEAD.STAGEINFO_OBJ_ANIM)
            code12_StageObjAnim.ReadInfo(p, true);
          else if (ac._type == P2P_SUB_HEAD.CONTROLED_OBJECT)
            code13_ControledObj.readSyncInfo(p, true);
          else if (ac._type == P2P_SUB_HEAD.USER || ac._type == P2P_SUB_HEAD.STAGEINFO_CHARA)
          {
            ac._flags = (Events) p.readUD();
            ac._data = p.readB((int) ac._lengthData - 9);
            this.GetUserLogs(data, ac);
          }
          else
            Logger.warning("[New user packet type '" + ac._type.ToString() + "' or '" + ((int) ac._type).ToString() + "']: " + BitConverter.ToString(data));
        }
        catch (Exception ex)
        {
          Logger.warning(ex.ToString());
          Logger.warning("1: " + BitConverter.ToString(packet._data));
          Logger.warning("2: " + BitConverter.ToString(data));
          break;
        }
      }
    }

    private void GetUserLogs(byte[] data, ActionModel ac)
    {
      if (ac._data.Length == 0)
        return;
      ReceivePacket p = new ReceivePacket(ac._data);
      uint num1 = 0;
      if (ac._flags.HasFlag((System.Enum) Events.ActionState))
      {
        ++num1;
        a1_unk.ReadInfo(p, false);
      }
      if (ac._flags.HasFlag((System.Enum) Events.Animation))
      {
        num1 += 2U;
        a2_unk.ReadInfo(ac, p, false);
      }
      if (ac._flags.HasFlag((System.Enum) Events.PosRotation))
      {
        num1 += 4U;
        a4_PositionSync.ReadInfo(p, false);
      }
      if (ac._flags.HasFlag((System.Enum) Events.OnLoadObject))
      {
        num1 += 8U;
        a8_MoveSync.readSyncInfo(ac, p, false);
      }
      if (ac._flags.HasFlag((System.Enum) Events.Unk1))
      {
        num1 += 16U;
        a10_unk.readSyncInfo(ac, p, false);
      }
      if (ac._flags.HasFlag((System.Enum) Events.RadioChat))
      {
        num1 += 32U;
        a20_RadioSync.readSyncInfo(ac, p, false);
      }
      if (ac._flags.HasFlag((System.Enum) Events.WeaponSync))
      {
        num1 += 64U;
        a40_WeaponSync.ReadInfo(ac, p, false, true);
      }
      if (ac._flags.HasFlag((System.Enum) Events.WeaponRecoil))
      {
        num1 += 128U;
        a80_WeaponRecoil.ReadInfo(ac, p, false);
      }
      if (ac._flags.HasFlag((System.Enum) Events.LifeSync))
      {
        num1 += 256U;
        int num2 = (int) a100_LifeSync.ReadInfo(p, false);
      }
      if (ac._flags.HasFlag((System.Enum) Events.Suicide))
      {
        num1 += 512U;
        a200_SuicideDamage.ReadInfo(p, false, true);
      }
      if (ac._flags.HasFlag((System.Enum) Events.Mission))
      {
        num1 += 1024U;
        a400_Mission.ReadInfo(ac, p, false, 0.0f, true);
      }
      if (ac._flags.HasFlag((System.Enum) Events.TakeWeapon))
      {
        num1 += 2048U;
        a800_WeaponAmmo.ReadInfo(ac, p, false);
      }
      if (ac._flags.HasFlag((System.Enum) Events.DropWeapon))
      {
        num1 += 4096U;
        a1000_DropWeapon.ReadInfo(p, true);
      }
      if (ac._flags.HasFlag((System.Enum) Events.FireSync))
      {
        num1 += 8192U;
        a2000_FireSync.ReadInfo(p, false);
      }
      if (ac._flags.HasFlag((System.Enum) Events.AIDamage))
      {
        num1 += 16384U;
        a4000_BotHitData.ReadInfo(p, false);
      }
      if (ac._flags.HasFlag((System.Enum) Events.NormalDamage))
      {
        num1 += 32768U;
        a8000_NormalHitData.ReadInfo(p, false, true);
      }
      if (ac._flags.HasFlag((System.Enum) Events.BoomDamage))
      {
        num1 += 65536U;
        a10000_BoomHitData.ReadInfo(p, false, true);
      }
      if (ac._flags.HasFlag((System.Enum) Events.Death))
      {
        num1 += 262144U;
        a40000_DeathData.ReadInfo(ac, p, false);
      }
      if (ac._flags.HasFlag((System.Enum) Events.SufferingDamage))
      {
        num1 += 524288U;
        a80000_SufferingDamage.ReadInfo(ac, p, false);
      }
      if (ac._flags.HasFlag((System.Enum) Events.PassPortal))
      {
        num1 += 1048576U;
        a100000_PassPortal.ReadInfo(ac, p, false);
      }
      if ((Events) num1 == ac._flags)
        return;
      Logger.warning("[" + ((uint) ac._flags).ToString() + "|" + ((uint) (ac._flags - num1)).ToString() + "] BUF ANORMAL: " + BitConverter.ToString(data));
    }

    private void RemoveHit(IList list, int idx) => list.RemoveAt(idx);

    public List<ObjectHitInfo> getNewActions(
      ActionModel ac,
      Room room,
      float time,
      out byte[] EventsData)
    {
      EventsData = new byte[0];
      if (room == null)
        return (List<ObjectHitInfo>) null;
      if (ac._data.Length == 0)
        return new List<ObjectHitInfo>();
      byte[] data = ac._data;
      List<ObjectHitInfo> objs = new List<ObjectHitInfo>();
      ReceivePacket p1 = new ReceivePacket(data);
      using (SendPacket s = new SendPacket())
      {
        int num1 = 0;
        Player player = room.getPlayer((int) ac._slot, true);
        if (ac._flags.HasFlag((System.Enum) Events.ActionState))
        {
          ++num1;
          a1_unk.Struct info = a1_unk.ReadInfo(p1, false);
          a1_unk.writeInfo(s, info);
        }
        if (ac._flags.HasFlag((System.Enum) Events.Animation))
        {
          num1 += 2;
          a2_unk.writeInfo(s, ac, p1, false);
        }
        if (ac._flags.HasFlag((System.Enum) Events.PosRotation))
        {
          num1 += 4;
          a4_PositionSync.Struct info = a4_PositionSync.ReadInfo(p1, false);
          a4_PositionSync.writeInfo(s, info);
          if (player != null)
            player.Position = new Half3(info._rotationX, info._rotationY, info._rotationZ);
        }
        if (ac._flags.HasFlag((System.Enum) Events.OnLoadObject))
        {
          num1 += 8;
          a8_MoveSync.Struct info = a8_MoveSync.readSyncInfo(ac, p1, false);
          a8_MoveSync.writeInfo(s, info);
          if (!room._isBotMode && info._objId != ushort.MaxValue && (info._spaceFlags.HasFlag((System.Enum) CharaMoves.HELI_STOPPED) || info._spaceFlags.HasFlag((System.Enum) CharaMoves.HELI_IN_MOVE)))
          {
            bool flag = false;
            ObjectInfo objectInfo = room.getObject((int) info._objId);
            if (objectInfo != null)
            {
              if (info._spaceFlags.HasFlag((System.Enum) CharaMoves.HELI_STOPPED))
              {
                AnimModel anim = objectInfo._anim;
                if (anim != null && anim._id == 0)
                  objectInfo._model.GetAnim(anim._nextAnim, 0.0f, 0.0f, objectInfo);
              }
              else if (info._spaceFlags.HasFlag((System.Enum) CharaMoves.HELI_IN_MOVE) && objectInfo._useDate.ToString("yyMMddHHmm") == "0101010000")
                flag = true;
              if (!flag)
                objs.Add(new ObjectHitInfo(3)
                {
                  objSyncId = 1,
                  objId = objectInfo._id,
                  objLife = objectInfo._life,
                  _animId1 = (int) byte.MaxValue,
                  _animId2 = objectInfo._anim != null ? objectInfo._anim._id : (int) byte.MaxValue,
                  _specialUse = AllUtils.GetDuration(objectInfo._useDate)
                });
            }
          }
        }
        if (ac._flags.HasFlag((System.Enum) Events.Unk1))
        {
          num1 += 16;
          a10_unk.writeInfo(s, ac, p1, false);
        }
        if (ac._flags.HasFlag((System.Enum) Events.RadioChat))
        {
          num1 += 32;
          a20_RadioSync.writeInfo(s, ac, p1, false);
        }
        if (ac._flags.HasFlag((System.Enum) Events.WeaponSync))
        {
          num1 += 64;
          a40_WeaponSync.Struct info = a40_WeaponSync.ReadInfo(ac, p1, false);
          a40_WeaponSync.writeInfo(s, info);
          if (player != null)
          {
            player.WeaponClass = (ClassType) info.WeaponClass;
            player.WeaponSlot = info.WeaponSlot;
            player._character = (CHARACTER_RES_ID) info._charaModelId;
            room.SyncInfo(objs, 2);
          }
        }
        if (ac._flags.HasFlag((System.Enum) Events.WeaponRecoil))
        {
          num1 += 128;
          a80_WeaponRecoil.writeInfo(s, ac, p1, false);
        }
        if (ac._flags.HasFlag((System.Enum) Events.LifeSync))
        {
          num1 += 256;
          a100_LifeSync.writeInfo(s, p1, false);
        }
        if (ac._flags.HasFlag((System.Enum) Events.Suicide))
        {
          num1 += 512;
          List<a200_SuicideDamage.HitData> hits = a200_SuicideDamage.ReadInfo(p1, false);
          int weaponId = 0;
          if (player != null)
          {
            List<DeathServerData> deaths = new List<DeathServerData>();
            int objId = -1;
            for (int index = 0; index < hits.Count; ++index)
            {
              a200_SuicideDamage.HitData hitData = hits[index];
              if (player != null && !player.isDead && player._life > 0)
              {
                int num2 = (int) (hitData._hitInfo >> 20);
                CHARA_DEATH deathType = (CHARA_DEATH) ((int) hitData._hitInfo & 15);
                int num3 = (int) (hitData._hitInfo >> 11) & 511;
                int hitPart = (int) (hitData._hitInfo >> 4) & 63;
                if (((int) (hitData._hitInfo >> 10) & 1) == 1)
                  objId = num3;
                weaponId = AllUtils.createItemId(AllUtils.itemClass(hitData.WeaponClass), (int) hitData._weaponSlot, (int) hitData.WeaponClass, hitData.WeaponId);
                player._life -= num2;
                if (player._life <= 0)
                  DamageManager.SetDeath(deaths, player, deathType);
                else
                  DamageManager.SetHitEffect(objs, player, deathType, hitPart);
                objs.Add(new ObjectHitInfo(2)
                {
                  objId = player._slot,
                  objLife = player._life,
                  deathType = deathType,
                  hitPart = hitPart,
                  weaponId = weaponId,
                  Position = hitData.PlayerPos
                });
              }
              else
                this.RemoveHit((IList) hits, index--);
            }
            if (deaths.Count > 0)
              Battle_SyncNet.SendDeathSync(room, player, objId, weaponId, deaths);
          }
          else
            hits = new List<a200_SuicideDamage.HitData>();
          a200_SuicideDamage.writeInfo(s, hits);
        }
        if (ac._flags.HasFlag((System.Enum) Events.Mission))
        {
          num1 += 1024;
          a400_Mission.Struct info = a400_Mission.ReadInfo(ac, p1, false, time);
          if (room.Map != null && player != null && (!player.isDead && (double) info._plantTime > 0.0) && !info.BombEnum.HasFlag((System.Enum) BombFlag.Stop))
          {
            BombPosition bomb = room.Map.GetBomb(info.BombId);
            if (bomb != null)
            {
              bool flag = info.BombEnum.HasFlag((System.Enum) BombFlag.Defuse);
              Vector3 vector3 = !flag ? (!info.BombEnum.HasFlag((System.Enum) BombFlag.Start) ? (Vector3) new Half3((ushort) 0, (ushort) 0, (ushort) 0) : (Vector3) bomb.Position) : (Vector3) room.BombPosition;
              double num2 = (double) Vector3.Distance((Vector3) player.Position, vector3);
              if ((bomb.Everywhere || num2 <= 2.0) && (player._team == 1 & flag || player._team == 0 && !flag))
              {
                if ((double) player._C4FTime != (double) info._plantTime)
                {
                  player._C4First = DateTime.Now;
                  player._C4FTime = info._plantTime;
                }
                double totalSeconds = (DateTime.Now - player._C4First).TotalSeconds;
                float num3 = flag ? player._defuseDuration : player._plantDuration;
                if (((double) time >= (double) info._plantTime + (double) num3 || totalSeconds >= (double) num3) && (!room._hasC4 && info.BombEnum.HasFlag((System.Enum) BombFlag.Start) || room._hasC4 & flag))
                {
                  room._hasC4 = !room._hasC4;
                  info._bombAll |= 2;
                  a400_Mission.SendC4UseSync(room, player, info);
                }
              }
            }
          }
          a400_Mission.writeInfo(s, info);
        }
        if (ac._flags.HasFlag((System.Enum) Events.TakeWeapon))
        {
          num1 += 2048;
          a800_WeaponAmmo.writeInfo(s, ac, p1, false);
        }
        if (ac._flags.HasFlag((System.Enum) Events.DropWeapon))
        {
          num1 += 4096;
          if (room != null && !room._isBotMode)
          {
            ++room._dropCounter;
            if (room._dropCounter > (int) Config.maxDrop)
              room._dropCounter = 0;
          }
          a1000_DropWeapon.writeInfo(s, p1, false, room != null ? room._dropCounter : 0);
        }
        if (ac._flags.HasFlag((System.Enum) Events.FireSync))
        {
          num1 += 8192;
          a2000_FireSync.writeInfo(s, p1, false);
        }
        if (ac._flags.HasFlag((System.Enum) Events.AIDamage))
        {
          num1 += 16384;
          a4000_BotHitData.writeInfo(s, p1, false);
        }
        int num4;
        if (ac._flags.HasFlag((System.Enum) Events.NormalDamage))
        {
          num1 += 32768;
          List<a8000_NormalHitData.HitData> hits = a8000_NormalHitData.ReadInfo(p1, false);
          List<DeathServerData> deaths = new List<DeathServerData>();
          int num2 = 0;
          if (player != null)
          {
            for (int index = 0; index < hits.Count; ++index)
            {
              a8000_NormalHitData.HitData hitData = hits[index];
              if (hitData.HitEnum != HitType.HelmetProtection && hitData.HitEnum != HitType.HeadshotProtection)
              {
                double num3 = (double) Vector3.Distance((Vector3) hitData.StartBullet, (Vector3) hitData.EndBullet);
                if (hitData._weaponSlot == (byte) 2 && (MeleeExceptionsXML.Contains(hitData.WeaponId) || num3 < 3.0) || hitData._weaponSlot != (byte) 2)
                {
                  int damage = (int) AllUtils.getHitDamageNORMAL(hitData._hitInfo);
                  int hitWho = AllUtils.getHitWho(hitData._hitInfo);
                  int hitPart = AllUtils.getHitPart(hitData._hitInfo);
                  CHARA_DEATH deathType = CHARA_DEATH.DEFAULT;
                  num2 = AllUtils.createItemId(AllUtils.itemClass(hitData.WeaponClass), (int) hitData._weaponSlot, (int) hitData.WeaponClass, hitData.WeaponId);
                  if (player.WeaponClass.ToString() == "Sniper")
                    damage += 25;
                  if (player.WeaponClass.ToString() == "Assault")
                  {
                    if (hitData.HitEnum != HitType.HelmetProtection && hitData.HitEnum != HitType.HeadshotProtection)
                    {
                      if (damage > 43)
                      {
                        if (hitPart == 29)
                        {
                          damage = 200;
                        }
                        else
                        {
                          damage = 1;
                          Logger.warning(string.Format("PEF DAMAGE DETECTED: {0}", (object) damage));
                        }
                      }
                      Logger.warning(string.Format("Jogador: {0} DanoDaArma: {1} HitPart: {2}.", (object) player._slot, (object) damage, (object) hitPart));
                    }
                    else
                      continue;
                  }
                  if (player.WeaponClass.ToString() == "SMG")
                  {
                    if (hitData.HitEnum != HitType.HelmetProtection && hitData.HitEnum != HitType.HeadshotProtection)
                    {
                      if (damage > 35)
                      {
                        if (hitPart == 29)
                        {
                          damage = 200;
                        }
                        else
                        {
                          damage = 1;
                          Logger.warning(string.Format("PEF DAMAGE DETECTED: {0}", (object) damage));
                        }
                      }
                      Logger.warning(string.Format("Jogador: {0} DanoDaArma: {1} HitPart: {2}.", (object) player._slot, (object) damage, (object) hitPart));
                    }
                    else
                      continue;
                  }
                  if (player.WeaponClass.ToString() == "Sniper")
                  {
                    if (hitData.HitEnum != HitType.HelmetProtection && hitData.HitEnum != HitType.HeadshotProtection)
                    {
                      if (damage > 210)
                      {
                        if (hitPart == 29)
                        {
                          damage = 200;
                        }
                        else
                        {
                          //damage = 1;
                          if (hitData._weaponInfo == (ushort) 18693 && hitData.WeaponId == 292)
                            damage = 200;
                          else if (hitData._weaponInfo == (ushort) 14853 && hitData.WeaponId == 232)
                            damage = 200;
                          else if (hitData._weaponInfo == (ushort) 5253 && hitData.WeaponId == 82)
                            damage = 200;
                          else if (hitData._weaponInfo == (ushort) 2053 && hitData.WeaponId == 32)
                            damage = 200;
                          //Logger.warning(string.Format("PEF Damage: {0} WeapondClass: {1} WeaponInfo: {2} WeapondID: {3}", (object) damage, (object) hitData.WeaponClass, (object) hitData._weaponInfo, (object) hitData.WeaponId));
                        }
                      }
                      Logger.warning(string.Format("Jogador: {0} DanoDaArma: {1} HitPart: {2}.", (object) player._slot, (object) damage, (object) hitPart));
                    }
                    else
                      continue;
                  }
                  if (player.WeaponClass.ToString() == "DualSMG")
                  {
                    if (hitData.HitEnum != HitType.HelmetProtection && hitData.HitEnum != HitType.HeadshotProtection)
                    {
                      if (damage > 43)
                      {
                        if (hitPart == 29)
                        {
                          damage = 200;
                        }
                        else
                        {
                          damage = 1;
                          Logger.warning(string.Format("PEF DAMAGE DETECTED: {0}", (object) damage));
                        }
                      }
                      Logger.warning(string.Format("Jogador: {0} DanoDaArma: {1} HitPart: {2}.", (object) player._slot, (object) damage, (object) hitPart));
                    }
                    else
                      continue;
                  }
                  if (player.WeaponClass.ToString() == "HandGun")
                  {
                    if (hitData.HitEnum != HitType.HelmetProtection && hitData.HitEnum != HitType.HeadshotProtection)
                    {
                      if (damage > 65)
                      {
                        if (hitPart == 29)
                        {
                          damage = 200;
                        }
                        else
                        {
                          //damage = 1;
                          //Logger.warning(string.Format("PEF DAMAGE DETECTED: {0}", (object) damage));
                        }
                      }
                      Logger.warning(string.Format("Jogador: {0} DanoDaArma: {1} HitPart: {2}.", (object) player._slot, (object) damage, (object) hitPart));
                    }
                    else
                      continue;
                  }
                  ObjectType hitType = AllUtils.getHitType(hitData._hitInfo);
                  switch (hitType)
                  {
                    case ObjectType.User:
                      Player p2;
                      if (room.getPlayer(hitWho, out p2) && player.RespawnLogicIsValid() && (!player.isDead && !p2.isDead) && !p2.Immortal)
                      {
                        if (hitPart == 29)
                          deathType = CHARA_DEATH.HEADSHOT;
                        if (room.stageType == 8 && deathType != CHARA_DEATH.HEADSHOT)
                          damage = 1;
                        else if (room.stageType == 7 && deathType == CHARA_DEATH.HEADSHOT)
                        {
                          if (room.LastRound == 1 && p2._team == 0 || room.LastRound == 2 && p2._team == 1)
                            damage /= 10;
                        }
                        else if (room.stageType == 13)
                          damage = 200;
                        if (Config.useHitMarker)
                          Battle_SyncNet.SendHitMarkerSync(room, player, (int) deathType, (int) hitData.HitEnum, damage);
                        DamageManager.SimpleDeath(deaths, objs, player, p2, damage, num2, hitPart, deathType);
                        break;
                      }
                      this.RemoveHit((IList) hits, index--);
                      break;
                    case ObjectType.UserObject:
                      int num5 = hitWho >> 4;
                      int num6 = hitWho & 15;
                      break;
                    case ObjectType.Object:
                      ObjectInfo objectInfo = room.getObject(hitWho);
                      ObjModel objM = objectInfo == null ? (ObjModel) null : objectInfo._model;
                      if (objM != null && objM.isDestroyable)
                      {
                        if (objectInfo._life > 0)
                        {
                          objectInfo._life -= damage;
                          if (objectInfo._life <= 0)
                          {
                            objectInfo._life = 0;
                            DamageManager.BoomDeath(room, player, num2, deaths, objs, hitData.BoomPlayers);
                          }
                          objectInfo.DestroyState = objM.CheckDestroyState(objectInfo._life);
                          DamageManager.SabotageDestroy(room, player, objM, objectInfo, damage);
                          objs.Add(new ObjectHitInfo(objM._updateId)
                          {
                            objId = objectInfo._id,
                            objLife = objectInfo._life,
                            killerId = (int) ac._slot,
                            objSyncId = objM._needSync ? 1 : 0,
                            _specialUse = AllUtils.GetDuration(objectInfo._useDate),
                            _animId1 = objM._anim1,
                            _animId2 = objectInfo._anim != null ? objectInfo._anim._id : (int) byte.MaxValue,
                            _destroyState = objectInfo.DestroyState
                          });
                          break;
                        }
                        break;
                      }
                      if (Config.sendFailMsg && objM == null)
                      {
                        Logger.warning("[Fire] Obj: " + hitWho.ToString() + "; Mapa: " + room._mapId.ToString() + "; objeto inválido.");
                        player.LogPlayerPos(hitData.EndBullet);
                        break;
                      }
                      break;
                    default:
                      string[] strArray = new string[6]
                      {
                        "[Warning] A new hit type: (",
                        hitType.ToString(),
                        "/",
                        null,
                        null,
                        null
                      };
                      num4 = (int) hitType;
                      strArray[3] = num4.ToString();
                      strArray[4] = "); by slot: ";
                      strArray[5] = ac._slot.ToString();
                      Logger.warning(string.Concat(strArray));
                      string str1 = hitData._boomInfo.ToString();
                      num4 = hitData.BoomPlayers.Count;
                      string str2 = num4.ToString();
                      Logger.warning("[Warning] BoomPlayers: " + str1 + ";" + str2);
                      break;
                  }
                }
                else
                  this.RemoveHit((IList) hits, index--);
              }
            }
            if (deaths.Count > 0)
              Battle_SyncNet.SendDeathSync(room, player, (int) byte.MaxValue, num2, deaths);
          }
          else
            hits = new List<a8000_NormalHitData.HitData>();
          a8000_NormalHitData.writeInfo(s, hits);
        }
        if (ac._flags.HasFlag((System.Enum) Events.BoomDamage))
        {
          num1 += 65536;
          List<a10000_BoomHitData.HitData> hits = a10000_BoomHitData.ReadInfo(p1, false);
          List<DeathServerData> deaths = new List<DeathServerData>();
          int weaponId = 0;
          if (player != null)
          {
            int num2 = -1;
            for (int index = 0; index < hits.Count; ++index)
            {
              a10000_BoomHitData.HitData hitData = hits[index];
              int damage = (int) AllUtils.getHitDamageNORMAL(hitData._hitInfo);
              int hitWho = AllUtils.getHitWho(hitData._hitInfo);
              int hitPart = AllUtils.getHitPart(hitData._hitInfo);
              weaponId = AllUtils.createItemId(AllUtils.itemClass(hitData.WeaponClass), (int) hitData._weaponSlot, (int) hitData.WeaponClass, hitData.WeaponId);
              ObjectType hitType = AllUtils.getHitType(hitData._hitInfo);
              switch (hitType)
              {
                case ObjectType.User:
                  ++num2;
                  Player p2;
                  if (damage > 0 && room.getPlayer(hitWho, out p2) && (player.RespawnLogicIsValid() && !p2.isDead) && !p2.Immortal)
                  {
                    if (hitData._deathType == (byte) 10)
                    {
                      p2._life += damage;
                      p2.CheckLifeValue();
                    }
                    else if (hitData._deathType == (byte) 2 && ClassType.Dino != hitData.WeaponClass && num2 % 2 == 0)
                    {
                      damage = (int) Math.Ceiling((double) damage / 2.7);
                      p2._life -= damage;
                      if (p2._life <= 0)
                        DamageManager.SetDeath(deaths, p2, (CHARA_DEATH) hitData._deathType);
                      else
                        DamageManager.SetHitEffect(objs, p2, player, (CHARA_DEATH) hitData._deathType, hitPart);
                    }
                    else
                    {
                      p2._life -= damage;
                      if (p2._life <= 0)
                        DamageManager.SetDeath(deaths, p2, (CHARA_DEATH) hitData._deathType);
                      else
                        DamageManager.SetHitEffect(objs, p2, player, (CHARA_DEATH) hitData._deathType, hitPart);
                    }
                    if (damage > 0)
                    {
                      if (Config.useHitMarker)
                        Battle_SyncNet.SendHitMarkerSync(room, player, (int) hitData._deathType, (int) hitData.HitEnum, damage);
                      objs.Add(new ObjectHitInfo(2)
                      {
                        objId = p2._slot,
                        objLife = p2._life,
                        deathType = (CHARA_DEATH) hitData._deathType,
                        weaponId = weaponId,
                        hitPart = hitPart
                      });
                      break;
                    }
                    break;
                  }
                  this.RemoveHit((IList) hits, index--);
                  break;
                case ObjectType.UserObject:
                  int num3 = hitWho >> 4;
                  int num5 = hitWho & 15;
                  break;
                case ObjectType.Object:
                  ObjectInfo objectInfo = room.getObject(hitWho);
                  ObjModel objM = objectInfo == null ? (ObjModel) null : objectInfo._model;
                  if (objM != null && objM.isDestroyable && objectInfo._life > 0)
                  {
                    objectInfo._life -= damage;
                    if (objectInfo._life <= 0)
                    {
                      objectInfo._life = 0;
                      DamageManager.BoomDeath(room, player, weaponId, deaths, objs, hitData.BoomPlayers);
                    }
                    objectInfo.DestroyState = objM.CheckDestroyState(objectInfo._life);
                    DamageManager.SabotageDestroy(room, player, objM, objectInfo, damage);
                    if (damage > 0)
                    {
                      objs.Add(new ObjectHitInfo(objM._updateId)
                      {
                        objId = objectInfo._id,
                        objLife = objectInfo._life,
                        killerId = (int) ac._slot,
                        objSyncId = objM._needSync ? 1 : 0,
                        _animId1 = objM._anim1,
                        _animId2 = objectInfo._anim != null ? objectInfo._anim._id : (int) byte.MaxValue,
                        _destroyState = objectInfo.DestroyState,
                        _specialUse = AllUtils.GetDuration(objectInfo._useDate)
                      });
                      break;
                    }
                    break;
                  }
                  if (Config.sendFailMsg && objM == null)
                  {
                    Logger.warning("[Boom] Obj: " + hitWho.ToString() + "; Mapa: " + room._mapId.ToString() + "; objeto inválido.");
                    player.LogPlayerPos(hitData.HitPos);
                    break;
                  }
                  break;
                default:
                  string[] strArray = new string[5]
                  {
                    "Grenade BOOM, new hit type: (",
                    hitType.ToString(),
                    "/",
                    null,
                    null
                  };
                  num4 = (int) hitType;
                  strArray[3] = num4.ToString();
                  strArray[4] = ")";
                  Logger.warning(string.Concat(strArray));
                  break;
              }
            }
            if (deaths.Count > 0)
              Battle_SyncNet.SendDeathSync(room, player, (int) byte.MaxValue, weaponId, deaths);
          }
          else
            hits = new List<a10000_BoomHitData.HitData>();
          a10000_BoomHitData.writeInfo(s, hits);
        }
        if (ac._flags.HasFlag((System.Enum) Events.Death))
        {
          num1 += 262144;
          a40000_DeathData.writeInfo(s, ac, p1, false);
        }
        if (ac._flags.HasFlag((System.Enum) Events.SufferingDamage))
        {
          num1 += 524288;
          a80000_SufferingDamage.writeInfo(s, ac, p1, false);
        }
        if (ac._flags.HasFlag((System.Enum) Events.PassPortal))
        {
          num1 += 1048576;
          a100000_PassPortal.Struct info = a100000_PassPortal.ReadInfo(ac, p1, false);
          a100000_PassPortal.writeInfo(s, info);
          if (player != null && !player.isDead)
            a100000_PassPortal.SendPassSync(room, player, info);
        }
        EventsData = s.mstream.ToArray();
        if ((long) num1 != (long) ac._flags)
          Logger.warning("[" + ((uint) ac._flags).ToString() + "|" + ((long) ac._flags - (long) num1).ToString() + ", BUF ANORMAL3: " + BitConverter.ToString(data));
        return objs;
      }
    }

    public void CheckDataFlags(ActionModel ac, PacketModel packet)
    {
      Events flags = ac._flags;
      if (!flags.HasFlag((System.Enum) Events.WeaponSync) || packet._opcode == 4 || (flags & (Events.TakeWeapon | Events.DropWeapon)) <= (Events) 0)
        return;
      ac._flags -= Events.WeaponSync;
    }

    public byte[] WriteActionBytes(byte[] data, Room room, float time, PacketModel packet)
    {
      ReceivePacket p = new ReceivePacket(data);
      List<ObjectHitInfo> objs = new List<ObjectHitInfo>();
      using (SendPacket s = new SendPacket())
      {
        for (int index = 0; index < 16; ++index)
        {
          ActionModel ac = new ActionModel();
          try
          {
            bool exception;
            ac._type = (P2P_SUB_HEAD) p.readC(out exception);
            if (!exception)
            {
              ac._slot = p.readUH();
              ac._lengthData = p.readUH();
              if (ac._lengthData != ushort.MaxValue)
              {
                s.writeC((byte) ac._type);
                s.writeH(ac._slot);
                s.writeH(ac._lengthData);
                if (ac._type == P2P_SUB_HEAD.GRENADE)
                  code1_GrenadeSync.writeInfo(s, p, false);
                else if (ac._type == P2P_SUB_HEAD.DROPEDWEAPON)
                  code2_WeaponSync.writeInfo(s, p, false);
                else if (ac._type == P2P_SUB_HEAD.OBJECT_STATIC)
                  code3_ObjectStatic.writeInfo(s, p, false);
                else if (ac._type == P2P_SUB_HEAD.OBJECT_ANIM)
                  code6_ObjectAnim.writeInfo(s, p, false);
                else if (ac._type == P2P_SUB_HEAD.STAGEINFO_OBJ_STATIC)
                  code9_StageInfoObjStatic.writeInfo(s, p, false);
                else if (ac._type == P2P_SUB_HEAD.STAGEINFO_OBJ_ANIM)
                  code12_StageObjAnim.writeInfo(s, p, false);
                else if (ac._type == P2P_SUB_HEAD.CONTROLED_OBJECT)
                  code13_ControledObj.writeInfo(s, p, false);
                else if (ac._type == P2P_SUB_HEAD.USER || ac._type == P2P_SUB_HEAD.STAGEINFO_CHARA)
                {
                  ac._flags = (Events) p.readUD();
                  ac._data = p.readB((int) ac._lengthData - 9);
                  this.CheckDataFlags(ac, packet);
                  byte[] EventsData;
                  objs.AddRange((IEnumerable<ObjectHitInfo>) this.getNewActions(ac, room, time, out EventsData));
                  s.GoBack(2);
                  s.writeH((ushort) (EventsData.Length + 9));
                  s.writeD((uint) ac._flags);
                  s.writeB(EventsData);
                  if (ac._data.Length == 0 && (uint) ac._lengthData - 9U > 0U)
                    break;
                }
                else
                {
                  Logger.warning("[New user packet type '" + ac._type.ToString() + "' or '" + ((int) ac._type).ToString() + "']: " + BitConverter.ToString(data));
                  throw new Exception("Unknown action type");
                }
              }
              else
                break;
            }
            else
              break;
          }
          catch (Exception ex)
          {
            Logger.warning("[WriteActionBytes]\r\n" + ex.ToString());
            Logger.warning("[WAB] Data: " + BitConverter.ToString(data));
            objs = new List<ObjectHitInfo>();
            break;
          }
        }
        if (objs.Count > 0)
          s.writeB(Packet4Creator.getCode4SyncData(objs));
        return s.mstream.ToArray();
      }
    }

    private void Send(byte[] data, IPEndPoint ip) => this._client.Send(data, data.Length, ip);
  }
}
