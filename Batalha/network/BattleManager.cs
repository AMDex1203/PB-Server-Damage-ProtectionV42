using System;
using System.Net;
using System.Net.Sockets;
using Battle.config;

namespace Battle.network
{
	// Token: 0x02000008 RID: 8
	public class BattleManager
	{
		// Token: 0x06000030 RID: 48 RVA: 0x00003B94 File Offset: 0x00001D94
		public static void init()
		{
			try
			{
				BattleManager.udpClient = new UdpClient();
				uint num = 0x80000000;
				uint num2 = 0x18000000;
				uint ioControlCode = num | num2 | 12U;
				BattleManager.udpClient.Client.IOControl((int)ioControlCode, new byte[]
				{
					Convert.ToByte(false)
				}, null);
				IPEndPoint ipendPoint = new IPEndPoint(IPAddress.Parse(Config.hosIp), (int)Config.hosPort);
				BattleManager.UdpState state = new BattleManager.UdpState(ipendPoint, BattleManager.udpClient);
				BattleManager.udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
				BattleManager.udpClient.Client.Bind(ipendPoint);
				BattleManager.udpClient.BeginReceive(new AsyncCallback(BattleManager.gerenciaRetorno), state);
				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine("[Batalha] Servidor De Batalha Pronto Para Jogar! (" + DateTime.Now.ToString("yy/MM/dd HH:mm:ss") + ")");
			}
			catch (Exception ex)
			{
				Logger.error(ex.ToString() + "\r\nOcorreu um erro ao listar as conexões UDP!!");
			}
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00003CA4 File Offset: 0x00001EA4
		private static void read(BattleManager.UdpState state)
		{
			try
			{
				BattleManager.udpClient.BeginReceive(new AsyncCallback(BattleManager.gerenciaRetorno), state);
			}
			catch (Exception ex)
			{
				Logger.error(ex.ToString());
			}
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00003CF0 File Offset: 0x00001EF0
		private static void gerenciaRetorno(IAsyncResult ar)
		{
			bool flag = !ar.IsCompleted;
			if (flag)
			{
				Logger.warning("ar is not completed.");
			}
			ar.AsyncWaitHandle.WaitOne(5000);
			DateTime now = DateTime.Now;
			IPEndPoint ipendPoint = new IPEndPoint(IPAddress.Any, 0);
			UdpClient c = ((BattleManager.UdpState)ar.AsyncState).c;
			IPEndPoint e = ((BattleManager.UdpState)ar.AsyncState).e;
			try
			{
				byte[] array = c.EndReceive(ar, ref ipendPoint);
				bool flag2 = array.Length >= 22;
				if (flag2)
				{
					new BattleHandler(BattleManager.udpClient, array, ipendPoint, now);
				}
				else
				{
					Logger.warning("No length (22) buffer: " + BitConverter.ToString(array));
				}
			}
			catch (Exception ex)
			{
				string str = "[Exception]: ";
				IPAddress address = ipendPoint.Address;
				Logger.warning(str + ((address != null) ? address.ToString() : null) + ":" + ipendPoint.Port.ToString());
				Logger.warning(ex.ToString());
			}
			BattleManager.UdpState state = new BattleManager.UdpState(e, c);
			BattleManager.read(state);
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00003E10 File Offset: 0x00002010
		public static void Send(byte[] data, IPEndPoint ip)
		{
			BattleManager.udpClient.Send(data, data.Length, ip);
		}

		// Token: 0x04000023 RID: 35
		private static UdpClient udpClient;

		// Token: 0x02000059 RID: 89
		private class UdpState
		{
			// Token: 0x06000198 RID: 408 RVA: 0x0000D818 File Offset: 0x0000BA18
			public UdpState(IPEndPoint e, UdpClient c)
			{
				this.e = e;
				this.c = c;
			}

			// Token: 0x040001C2 RID: 450
			public IPEndPoint e;

			// Token: 0x040001C3 RID: 451
			public UdpClient c;
		}
	}
}
