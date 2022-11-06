using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Core;
using Core.managers.events;
using Core.managers.server;
using Core.models.account;
using Core.models.enums;
using Core.models.enums.flags;
using Core.models.enums.friends;
using Core.models.enums.missions;
using Core.models.room;
using Core.models.servers;
using Core.server;
using Core.xml;
using Game.data.managers;
using Game.data.model;
using Game.data.sync.client_side;
using Game.data.utils;
using Game.data.xml;
using Game.global.serverpacket;

namespace Game.data.sync
{
	// Token: 0x02000186 RID: 390
	public static class Game_SyncNet
	{
		// Token: 0x06000509 RID: 1289 RVA: 0x000281A0 File Offset: 0x000263A0
		public static void Start()
		{
			try
			{
				Game_SyncNet.udp = new UdpClient(ConfigGS.syncPort);
				uint num = 0x80000000;
				uint num2 = 0x18000000;
				uint ioControlCode = num | num2 | 12U;
				Game_SyncNet.udp.Client.IOControl((int)ioControlCode, new byte[]
				{
					Convert.ToByte(false)
				}, null);
				new Thread(new ThreadStart(Game_SyncNet.read)).Start();
			}
			catch (Exception ex)
			{
				Logger.error(ex.ToString());
			}
		}

		// Token: 0x0600050A RID: 1290 RVA: 0x00028228 File Offset: 0x00026428
		public static void read()
		{
			try
			{
				Game_SyncNet.udp.BeginReceive(new AsyncCallback(Game_SyncNet.recv), null);
			}
			catch
			{
			}
		}

		// Token: 0x0600050B RID: 1291 RVA: 0x00028264 File Offset: 0x00026464
		private static void recv(IAsyncResult res)
		{
			if (GameManager.ServerIsClosed)
			{
				return;
			}
			IPEndPoint ipendPoint = new IPEndPoint(IPAddress.Any, 8000);
			byte[] array = Game_SyncNet.udp.EndReceive(res, ref ipendPoint);
			Thread.Sleep(5);
			new Thread(new ThreadStart(Game_SyncNet.read)).Start();
			if (array.Length >= 2)
			{
				Game_SyncNet.LoadPacket(array);
			}
		}

		// Token: 0x0600050C RID: 1292 RVA: 0x000282C0 File Offset: 0x000264C0
		private static void LoadPacket(byte[] buffer)
		{
			ReceiveGPacket receiveGPacket = new ReceiveGPacket(buffer);
			short num = receiveGPacket.readH();
			try
			{
				if (num == 1)
				{
					Net_Room_Pass_Portal.Load(receiveGPacket);
				}
				else if (num == 2)
				{
					Net_Room_C4.Load(receiveGPacket);
				}
				else if (num == 3)
				{
					Net_Room_Death.Load(receiveGPacket);
				}
				else if (num == 4)
				{
					Net_Room_HitMarker.Load(receiveGPacket);
				}
				else if (num == 5)
				{
					Net_Room_Sabotage_Sync.Load(receiveGPacket);
				}
				else if (num == 10)
				{
					Account account = AccountManager.getAccount(receiveGPacket.readQ(), true);
					if (account != null)
					{
						account.SendPacket(new AUTH_ACCOUNT_KICK_PAK(1));
						account.SendPacket(new SERVER_MESSAGE_ERROR_PAK(2147487744U));
						account.Close(1000, false);
					}
				}
				else if (num == 11)
				{
					int num2 = (int)receiveGPacket.readC();
					int num3 = (int)receiveGPacket.readC();
					Account account2 = AccountManager.getAccount(receiveGPacket.readQ(), 0);
					if (account2 != null)
					{
						Account account3 = AccountManager.getAccount(receiveGPacket.readQ(), true);
						if (account3 != null)
						{
							FriendState amigostate = (num3 == 1) ? FriendState.Online : FriendState.Offline;
							if (num2 == 0)
							{
								int num4 = -1;
								Friend friend = account3.FriendSystem.GetFriend(account2.player_id, out num4);
								if (num4 != -1 && friend != null && friend.state == 0)
								{
									account3.SendPacket(new FRIEND_UPDATE_PAK(FriendChangeState.Update, friend, amigostate, num4));
								}
							}
							else
							{
								account3.SendPacket(new CLAN_MEMBER_INFO_CHANGE_PAK(account2, amigostate));
							}
						}
					}
				}
				else if (num == 13)
				{
					long id = receiveGPacket.readQ();
					byte b = receiveGPacket.readC();
					byte[] data = receiveGPacket.readB((int)receiveGPacket.readUH());
					Account account4 = AccountManager.getAccount(id, true);
					if (account4 != null)
					{
						if (b == 0)
						{
							account4.SendPacket(data);
						}
						else
						{
							account4.SendCompletePacket(data);
						}
					}
				}
				else if (num == 15)
				{
					int id2 = receiveGPacket.readD();
					int lastCount = receiveGPacket.readD();
					GameServerModel server = ServersXML.getServer(id2);
					if (server != null)
					{
						server._LastCount = lastCount;
					}
				}
				else if (num == 16)
				{
					Net_Clan_Sync.Load(receiveGPacket);
				}
				else if (num == 17)
				{
					Net_Friend_Sync.Load(receiveGPacket);
				}
				else if (num == 18)
				{
					Net_Inventory_Sync.Load(receiveGPacket);
				}
				else if (num == 19)
				{
					Net_Player_Sync.Load(receiveGPacket);
				}
				else if (num == 20)
				{
					Net_Server_Warning.LoadGMWarning(receiveGPacket);
				}
				else if (num == 21)
				{
					Net_Clan_Servers_Sync.Load(receiveGPacket);
				}
				else if (num == 22)
				{
					Net_Server_Warning.LoadShopRestart(receiveGPacket);
				}
				else if (num == 23)
				{
					Net_Server_Warning.LoadServerUpdate(receiveGPacket);
				}
				else if (num == 24)
				{
					Net_Server_Warning.LoadShutdown(receiveGPacket);
				}
				else if (num == 31)
				{
					int index = (int)receiveGPacket.readC();
					EventLoader.ReloadEvent(index);
					Logger.warning("[Game_SyncNet] Evento re-carregado.");
				}
				else if (num == 32)
				{
					int configId = (int)receiveGPacket.readC();
					ServerConfigSyncer.GenerateConfig(configId);
					Logger.warning("[Game_SyncNet] Configurações (DB) resetadas.");
				}
				else
				{
					Logger.warning("[Game_SyncNet] Tipo de conexão não encontrada: " + num.ToString());
				}
			}
			catch (Exception ex)
			{
				Logger.error("[Crash/Game_SyncNet] Tipo: " + num.ToString() + "\r\n" + ex.ToString());
				if (receiveGPacket != null)
				{
					Logger.error("COMP: " + BitConverter.ToString(receiveGPacket.getBuffer()));
				}
			}
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x000285E0 File Offset: 0x000267E0
		public static void SendUDPPlayerSync(Room room, Slot slot, CupomEffects effects, int type)
		{
			using (SendGPacket sendGPacket = new SendGPacket())
			{
				sendGPacket.writeH(1);
				sendGPacket.writeD(room.UniqueRoomId);
				sendGPacket.writeD(room.mapId * 16 + (int)room.room_type);
				sendGPacket.writeQ(room.StartTick);
				sendGPacket.writeC((byte)type);
				sendGPacket.writeC((byte)room.rodada);
				sendGPacket.writeC((byte)slot._id);
				sendGPacket.writeC((byte)slot.spawnsCount);
				sendGPacket.writeC(BitConverter.GetBytes(slot._playerId)[0]);
				if (type == 0 || type == 2)
				{
					Game_SyncNet.WriteCharaInfo(sendGPacket, room, slot, effects);
				}
				Game_SyncNet.SendPacket(sendGPacket.mstream.ToArray(), room.UDPServer.Connection);
			}
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x000286B4 File Offset: 0x000268B4
		private static void WriteCharaInfo(SendGPacket pk, Room room, Slot slot, CupomEffects effects)
		{
			int num;
			if (room.room_type == 7 || room.room_type == 12)
			{
				if ((room.rodada == 1 && slot._team == 1) || (room.rodada == 2 && slot._team == 0))
				{
					num = ((room.rodada == 2) ? slot._equip._red : slot._equip._blue);
				}
				else if (room.TRex == slot._id)
				{
					num = -1;
				}
				else
				{
					num = slot._equip._dino;
				}
			}
			else
			{
				num = ((slot._team == 0) ? slot._equip._red : slot._equip._blue);
			}
			int num2 = 0;
			if (effects.HasFlag(CupomEffects.Ketupat))
			{
				num2 += 10;
			}
			if (effects.HasFlag(CupomEffects.HP5))
			{
				num2 += 5;
			}
			if (effects.HasFlag(CupomEffects.HP10))
			{
				num2 += 10;
			}
			if (num == -1)
			{
				pk.writeC(byte.MaxValue);
				pk.writeH(ushort.MaxValue);
			}
			else
			{
				pk.writeC((byte)ComDiv.getIdStatics(num, 2));
				pk.writeH((short)ComDiv.getIdStatics(num, 4));
			}
			pk.writeC((byte)num2);
			pk.writeC(effects.HasFlag(CupomEffects.C4SpeedKit));
		}

		// Token: 0x0600050F RID: 1295 RVA: 0x00028808 File Offset: 0x00026A08
		public static void SendUDPRoundSync(Room room)
		{
			using (SendGPacket sendGPacket = new SendGPacket())
			{
				sendGPacket.writeH(3);
				sendGPacket.writeD(room.UniqueRoomId);
				sendGPacket.writeD(room.mapId * 16 + (int)room.room_type);
				sendGPacket.writeC((byte)room.rodada);
				Game_SyncNet.SendPacket(sendGPacket.mstream.ToArray(), room.UDPServer.Connection);
			}
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x00028888 File Offset: 0x00026A88
		public static GameServerModel GetServer(AccountStatus status)
		{
			return Game_SyncNet.GetServer((int)status.serverId);
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x00028895 File Offset: 0x00026A95
		public static GameServerModel GetServer(int serverId)
		{
			if (serverId == 255 || serverId == ConfigGS.serverId)
			{
				return null;
			}
			return ServersXML.getServer(serverId);
		}

		// Token: 0x06000512 RID: 1298 RVA: 0x000288B0 File Offset: 0x00026AB0
		public static void UpdateGSCount(int serverId)
		{
			try
			{
				double totalSeconds = (DateTime.Now - Game_SyncNet.LastSyncCount).TotalSeconds;
				if (totalSeconds >= 5.0)
				{
					Game_SyncNet.LastSyncCount = DateTime.Now;
					int num = 0;
					foreach (Channel channel in ChannelsXML._channels)
					{
						num += channel._players.Count;
					}
					foreach (GameServerModel gameServerModel in ServersXML._servers)
					{
						if (gameServerModel._id == serverId)
						{
							gameServerModel._LastCount = num;
						}
						else
						{
							using (SendGPacket sendGPacket = new SendGPacket())
							{
								sendGPacket.writeH(15);
								sendGPacket.writeD(serverId);
								sendGPacket.writeD(num);
								Game_SyncNet.SendPacket(sendGPacket.mstream.ToArray(), gameServerModel.Connection);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.warning("[Game_SyncNet.UpdateGSCount] " + ex.ToString());
			}
		}

		// Token: 0x06000513 RID: 1299 RVA: 0x00028A0C File Offset: 0x00026C0C
		public static void SendBytes(long playerId, SendPacket sp, int serverId)
		{
			if (sp == null)
			{
				return;
			}
			GameServerModel server = Game_SyncNet.GetServer(serverId);
			if (server == null)
			{
				return;
			}
			byte[] bytes = sp.GetBytes("Game_SyncNet.SendBytes");
			using (SendGPacket sendGPacket = new SendGPacket())
			{
				sendGPacket.writeH(13);
				sendGPacket.writeQ(playerId);
				sendGPacket.writeC(0);
				sendGPacket.writeH((ushort)bytes.Length);
				sendGPacket.writeB(bytes);
				Game_SyncNet.SendPacket(sendGPacket.mstream.ToArray(), server.Connection);
			}
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x00028A94 File Offset: 0x00026C94
		public static void SendBytes(long playerId, byte[] buffer, int serverId)
		{
			if (buffer.Length == 0)
			{
				return;
			}
			GameServerModel server = Game_SyncNet.GetServer(serverId);
			if (server == null)
			{
				return;
			}
			using (SendGPacket sendGPacket = new SendGPacket())
			{
				sendGPacket.writeH(13);
				sendGPacket.writeQ(playerId);
				sendGPacket.writeC(0);
				sendGPacket.writeH((ushort)buffer.Length);
				sendGPacket.writeB(buffer);
				Game_SyncNet.SendPacket(sendGPacket.mstream.ToArray(), server.Connection);
			}
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x00028B10 File Offset: 0x00026D10
		public static void SendCompleteBytes(long playerId, byte[] buffer, int serverId)
		{
			if (buffer.Length == 0)
			{
				return;
			}
			GameServerModel server = Game_SyncNet.GetServer(serverId);
			if (server == null)
			{
				return;
			}
			using (SendGPacket sendGPacket = new SendGPacket())
			{
				sendGPacket.writeH(13);
				sendGPacket.writeQ(playerId);
				sendGPacket.writeC(1);
				sendGPacket.writeH((ushort)buffer.Length);
				sendGPacket.writeB(buffer);
				Game_SyncNet.SendPacket(sendGPacket.mstream.ToArray(), server.Connection);
			}
		}

		// Token: 0x06000516 RID: 1302 RVA: 0x00028B8C File Offset: 0x00026D8C
		public static void SendPacket(byte[] data, IPEndPoint ip)
		{
			Game_SyncNet.udp.Send(data, data.Length, ip);
		}

		// Token: 0x06000517 RID: 1303 RVA: 0x00028BA0 File Offset: 0x00026DA0
		public static void genDeath(Room room, Slot killer, FragInfos kills, bool isSuicide)
		{
			bool flag = room.isBotMode();
			int num;
			Net_Room_Death.RegistryFragInfos(room, killer, out num, flag, isSuicide, kills);
			if (flag)
			{
				killer.Score += killer.killsOnLife + (int)room.IngameAiLevel + num;
				if (killer.Score > 65535)
				{
					killer.Score = 65535;
					Logger.warning("[PlayerId: " + killer._id.ToString() + "] chegou a pontuação máxima do modo BOT.");
				}
				kills.Score = killer.Score;
			}
			else
			{
				killer.Score += num;
				AllUtils.CompleteMission(room, killer, kills, MISSION_TYPE.NA, 0);
				kills.Score = num;
			}
			using (BATTLE_DEATH_PAK battle_DEATH_PAK = new BATTLE_DEATH_PAK(room, kills, killer, flag))
			{
				room.SendPacketToPlayers(battle_DEATH_PAK, SLOT_STATE.BATTLE, 0);
			}
			Net_Room_Death.EndBattleByDeath(room, killer, flag, isSuicide);
		}

		// Token: 0x04000330 RID: 816
		private static DateTime LastSyncCount;

		// Token: 0x04000331 RID: 817
		public static UdpClient udp;
	}
}
