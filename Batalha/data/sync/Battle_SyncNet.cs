using Battle.config;
using Battle.data.models;
using Battle.data.sync.client_side;
using Battle.network;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Battle.data.sync
{
    public class Battle_SyncNet
    {
        private static UdpClient udp;
        public static void Start()
        {
            try
            {
                Battle_SyncNet.udp = new UdpClient((int)Config.syncPort);
                uint num = 0x80000000;
                uint num2 = 0x18000000;
                uint ioControlCode = num | num2 | 12U;
                Battle_SyncNet.udp.Client.IOControl((int)ioControlCode, new byte[]
                {
                    Convert.ToByte(false)
                }, null);
                Battle_SyncNet.udp.Client.DontFragment = false;
                new Thread(new ThreadStart(Battle_SyncNet.read)).Start();
            }
            catch (Exception e)
            {
                Logger.warning(e.ToString());
            }
        }
        public static void read()
        {
            try
            {
                Battle_SyncNet.udp.BeginReceive(new AsyncCallback(Battle_SyncNet.recv), null);
            }
            catch (Exception ex)
            {
                Logger.error(ex.ToString());
            }
        }
        private static void recv(IAsyncResult res)
        {
            IPEndPoint ipendPoint = new IPEndPoint(IPAddress.Any, 8000);
            byte[] array = Battle_SyncNet.udp.EndReceive(res, ref ipendPoint);
            new Thread(new ThreadStart(Battle_SyncNet.read)).Start();
            bool flag = array.Length >= 2;
            if (flag)
            {
                Battle_SyncNet.LoadPacket(array);
            }
        }
        private static void LoadPacket(byte[] buffer)
        {
            ReceivePacket receivePacket = new ReceivePacket(buffer);
            short num = receivePacket.readH();
            bool flag = num == 1;
            if (flag)
            {
                RespawnSync.Load(receivePacket);
            }
            else
            {
                bool flag2 = num == 2;
                if (flag2)
                {
                    RemovePlayerSync.Load(receivePacket);
                }
                else
                {
                    bool flag3 = num == 3;
                    if (flag3)
                    {
                        uint uniqueRoomId = receivePacket.readUD();
                        int gen = receivePacket.readD();
                        int serverRound = (int)receivePacket.readC();
                        Room room = RoomsManager.getRoom(uniqueRoomId, gen);
                        bool flag4 = room != null;
                        if (flag4)
                        {
                            room._serverRound = serverRound;
                        }
                    }
                }
            }
        }
        public static void SendPortalPass(Room room, Player pl, int portalIdx)
        {
            if (room.stageType == 7)
            {
                pl._life = pl._maxLife;
                using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
                {
                    using (SendPacket sendPacket = new SendPacket())
                    {
                        sendPacket.writeH(1);
                        sendPacket.writeH((short)room._roomId);
                        sendPacket.writeH((short)room._channelId);
                        sendPacket.writeC((byte)pl._slot);
                        sendPacket.writeC((byte)portalIdx);
                        Battle_SyncNet.SendData(room, socket, sendPacket.mstream.ToArray());
                    }
                }
            }
        }
        public static void SendDeathSync(Room room, Player killer, int objId, int weaponId, List<DeathServerData> deaths)
        {
            using (Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
            using (SendPacket p = new SendPacket())
            {
                p.writeH(3);
                p.writeH((short)room._roomId);
                p.writeH((short)room._channelId);
                p.writeC((byte)killer._slot);
                p.writeC((byte)objId);
                p.writeD(weaponId);
                p.writeTVector(killer.Position);
                p.writeC((byte)deaths.Count);
                for (int i = 0; i < deaths.Count; i++)
                {
                    DeathServerData ob = deaths[i];
                    p.writeC((byte)ob._player.WeaponClass);
                    p.writeC((byte)(((int)ob._deathType * 16) + ob._player._slot));
                    p.writeTVector(ob._player.Position);
                    p.writeC(0);
                }
                SendData(room, s, p.mstream.ToArray());
            }
        }
        public static void SendBombSync(Room room, Player pl, int type, int bombArea)
        {
            using (Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
            using (SendPacket p = new SendPacket())
            {
                p.writeH(2);
                p.writeH((short)room._roomId);
                p.writeH((short)room._channelId);
                p.writeC((byte)type);
                p.writeC((byte)pl._slot);
                if (type == 0)
                {
                    p.writeC((byte)bombArea);
                    p.writeTVector(pl.Position);
                    room.BombPosition = pl.Position;
                }
                SendData(room, s, p.mstream.ToArray());
            }
        }
        public static void SendHitMarkerSync(Room room, Player pl, int deathType, int hitEnum, int damage)
        {
            using (Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
            using (SendPacket p = new SendPacket())
            {
                p.writeH(4);
                p.writeH((short)room._roomId);
                p.writeH((short)room._channelId);
                p.writeC((byte)pl._slot);
                p.writeC((byte)deathType);
                p.writeC((byte)hitEnum);
                p.writeH((short)damage);
                SendData(room, s, p.mstream.ToArray());
            }
        }
        public static void SendSabotageSync(Room room, Player pl, int damage, int ultraSYNC)
        {
            using (Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
            using (SendPacket p = new SendPacket())
            {
                p.writeH(5);
                p.writeH((short)room._roomId);
                p.writeH((short)room._channelId);
                p.writeC((byte)pl._slot);
                p.writeH((ushort)room._bar1);
                p.writeH((ushort)room._bar2);
                p.writeC((byte)ultraSYNC); //barnumber (1 = primeiro/2 = segundo)
                p.writeH((ushort)damage);
                SendData(room, s, p.mstream.ToArray());
            }
        }
        private static void SendData(Room room, Socket socket, byte[] data)
        {
            if (Config.sendInfoToServ)
                socket.SendTo(data, room.gs.Connection);
        }
    }
}