
// Type: Auth.global.clientpacket.BASE_LOGIN_REC
// Assembly: pbserver_auth, Version=1.0.7796.38604, Culture=neutral, PublicKeyToken=null
// MVID: 1CFDFF97-782C-4EF8-A621-6E4FDDE32436
// Interprise: C:\Users\Cuzin\3,50pbserver_auth.exe

using Auth.data.managers;
using Auth.data.model;
using Auth.data.sync;
using Auth.data.sync.server_side;
using Auth.global.serverpacket;
using Core;
using System;
using Core.managers;
using Core.managers.server;
using Core.models.enums.errors;
using Core.models.enums.global;
using Core.models.enums.Login;
using Core.models.servers;
using Core.server;
using Core.xml;
using System.Net;
using System.Net.NetworkInformation;

namespace Auth.global.clientpacket
{
    public class BASE_LOGIN_REC : ReceiveLoginPacket
    {
        private string login;
        private string passN;
        private string passEnc;
        private string UserFileListMD5;
        private string d3d9MD5;
        private string GameVersion;
        private string PublicIP;
        private string LocalIP;
        private int loginSize;
        private int passSize;
        private IsRealiP IsRealIP;
        private ulong key;
        private ClientLocale GameLocale;
        private PhysicalAddress MacAddress;

        public BASE_LOGIN_REC(LoginClient client, byte[] data) => this.makeme(client, data);

        public override void read()
        {
            this.GameVersion = this.readC().ToString() + "." + this.readH().ToString() + "." + this.readH().ToString();
            this.GameLocale = (ClientLocale)this.readC();
            this.loginSize = (int)this.readC();
            this.passSize = (int)this.readC();
            this.login = this.readS(this.loginSize);
            this.passN = this.readS(this.passSize);
            this.passEnc = ComDiv.gen5(this.passN);
            this.MacAddress = new PhysicalAddress(this.readB(6));
            this.readB(2);
            this.IsRealIP = (IsRealiP)this.readC();
            this.LocalIP = this.readC().ToString() + "." + this.readC().ToString() + "." + this.readC().ToString() + "." + this.readC().ToString();
            this.key = this.readUQ();
            this.UserFileListMD5 = this.readS(32);
            this.readD();
            this.d3d9MD5 = this.readS(32);
            int num = (int)this.readC();
            this.PublicIP = this._client.GetIPAddress();
        }

        public override void run()
        {
            try
            {
                DirectXML.IsValid(this.d3d9MD5);
                ServerConfig config = LoginManager.Config;
                if (config == null || !ConfigGA.isTestMode && !ConfigGA.GameLocales.Contains(this.GameLocale) || (this.login.Length < ConfigGA.minLoginSize || !ConfigGA.isTestMode && this.passN.Length < ConfigGA.minPassSize) || (this.LocalIP == "0.0.0.0" || this.MacAddress.GetAddressBytes() == new byte[6] || this.GameVersion != config.ClientVersion || ConfigGA.LauncherKey > 0UL && (long)this.key != (long)ConfigGA.LauncherKey))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("[AUTH] Modified client suspected, connection destroyed.  [" + this.login + "]");
                    this._client.SendPacket(new BASE_LOGIN_PAK((EventErrorEnum)0x80000000, this.login, 0));
                    if (ConfigGA.LauncherKey > 0 && (long)this.key != (long)ConfigGA.LauncherKey)
                        Console.WriteLine("Key: " + this.key.ToString() + " não compativel [" + this.login + "]");
                    else if (LoginManager.Config == null)
                    {
                        Console.WriteLine("Configuração do servidor invalida [" + this.login + "]");
                        this._client.SendPacket((SendPacket)new BASE_LOGIN_PAK(EventErrorEnum.Login_User_Break, this.login, 0L));
                    }
                    else if (ConfigGA.LocalGame != this.GameLocale)
                    {
                        Console.WriteLine("[Auth] Conexão Suspeita Bloqueada/País do cliente bloqueado: " + this.GameLocale.ToString() + " [" + this.login + "]");
                        this._client.SendPacket((SendPacket)new BASE_LOGIN_PAK(EventErrorEnum.Login_BLOCK_COUNTRY, this.login, 0));
                    }
                    else if (this.IsRealIP == IsRealiP.Host && (!IPAddress.TryParse(this.LocalIP, out IPAddress _) || !IPAddress.TryParse(this.PublicIP, out IPAddress _)))
                    {
                        Logger.warning("Invalid IP: " + this.LocalIP + " ~ " + this.PublicIP + " [" + this.login + "]");
                        this._client.SendPacket((SendPacket)new BASE_LOGIN_PAK(EventErrorEnum.Login_User_Break, this.login, 0L));
                    }
                    else if (this.IsRealIP == IsRealiP.Host && (this.LocalIP == null || this.LocalIP == "127.0.0.1" || (this.LocalIP == "0" || this.LocalIP == "0.0.0.0") || this.LocalIP.StartsWith("0")))
                    {
                        Logger.warning("Null IP: " + this.LocalIP + "  [" + this.login + "]");
                        this._client.SendPacket((SendPacket)new BASE_LOGIN_PAK(EventErrorEnum.Login_BLOCK_IP, this.login, 0L));
                    }
                    else if (this.login.Length < 4)
                    {
                        Console.WriteLine("Login muito pequeno [" + this.login + "]");
                        this._client.SendPacket((SendPacket)new BASE_LOGIN_PAK(EventErrorEnum.Login_ID_PASS_INCORRECT, this.login, 0L));
                    }
                    else if (this.passN.Length < 4)
                    {
                        Console.WriteLine("Senha muito pequena [" + this.login + "]");
                        this._client.SendPacket((SendPacket)new BASE_LOGIN_PAK(EventErrorEnum.Login_ID_PASS_INCORRECT, this.login, 0L));
                    }
                    else if (this.login.Length > 16)
                    {
                        Console.WriteLine("Login muito grande [" + this.login + "]");
                        this._client.SendPacket((SendPacket)new BASE_LOGIN_PAK(EventErrorEnum.Login_User_Break, this.login, 0L));
                    }
                    else if (this.passN.Length > 16)
                    {
                        Console.WriteLine("Senha muito grande [" + this.login + "]");
                        this._client.SendPacket((SendPacket)new BASE_LOGIN_PAK(EventErrorEnum.Login_User_Break, this.login, 0L));
                    }
                    else if (this.MacAddress.GetAddressBytes() == new byte[6])
                    {
                        Console.WriteLine("MAC invalid. [" + this.login + "]");
                        this._client.SendPacket((SendPacket)new BASE_LOGIN_PAK(EventErrorEnum.Login_User_Break, this.login, 0L));
                    }
                    else if (this.MacAddress.ToString() == "000000000000")
                    {
                        Console.WriteLine("MAC invalid:  000000000000 [" + this.login + "]");
                        this._client.SendPacket((SendPacket)new BASE_LOGIN_PAK(EventErrorEnum.Login_DB_BUFFER_FAIL, this.login, 0L));
                    }
                    else if (this.GameVersion != LoginManager.Config.ClientVersion)
                    {
                        Console.WriteLine("Versão não suportada: " + this.GameVersion + " [" + this.login + "]");
                        this._client.SendPacket((SendPacket)new BASE_LOGIN_PAK(EventErrorEnum.Login_Failed_Default, this.login, 0L));
                    }
                    else
                    {
                        string text = "";
                        if (config == null)
                            text = "Config do servidor inválida [" + this.login + "]";
                        else if (!ConfigGA.isTestMode && !ConfigGA.GameLocales.Contains(this.GameLocale))
                            text = "País: " + this.GameLocale.ToString() + " do cliente bloqueado [" + this.login + "]";
                        else if (this.login.Length < ConfigGA.minLoginSize)
                            text = "Login muito pequeno [" + this.login + "]";
                        else if (!ConfigGA.isTestMode && this.passN.Length < ConfigGA.minPassSize)
                            text = "Senha muito pequena [" + this.login + "]";
                        else if (this.LocalIP == "0.0.0.0" || this.LocalIP == "145.14.134.111")
                            text = "IP inválido. [" + this.login + "]";
                        else if (this.MacAddress.GetAddressBytes() == new byte[6])
                            text = "MAC inválido. [" + this.login + "]";
                        else if (this.GameVersion != config.ClientVersion)
                            text = "Versão: " + this.GameVersion + " não compatível [" + this.login + "]";
                        else if (ConfigGA.LauncherKey > 0UL && (long)this.key != (long)ConfigGA.LauncherKey)
                            text = "Chave: " + this.key.ToString() + " não compatível [" + this.login + "]";
                        else if (this.UserFileListMD5 == "NULL")
                            ConfigGA.CheckUserFile();
                        else if (this.UserFileListMD5 == "EMPTY")
                            Environment.Exit(0);
                        else
                            text = "UserFileList: " + this.UserFileListMD5 + " não compatível [" + this.login + "]";
                        this._client.SendPacket((SendPacket)new SERVER_MESSAGE_DISCONNECT_PAK(2147483904U, false));
                        Logger.LogLogin(text);
                        this._client.Close(1000, true);
                    }
                }
                else
                {
                    this._client._player = AccountManager.getInstance().getAccountDB((object)this.login, (object)null, 0, 0);
                    if (this._client._player == null && ConfigGA.AUTO_ACCOUNTS && !AccountManager.getInstance().CreateAccount(out this._client._player, this.login, this.passEnc))
                    {
                        this._client.SendPacket((SendPacket)new BASE_LOGIN_PAK(EventErrorEnum.Login_DELETE_ACCOUNT, this.login, 0L));
                        Logger.LogLogin("Falha ao criar conta automaticamente [" + this.login + "]");
                        this._client.Close(1000, false);
                    }
                    else
                    {
                        Account player = this._client._player;
                        if (player == null || !player.ComparePassword(this.passEnc))
                        {
                            string str = "";
                            if (player == null)
                                str = "Conta retornada da DB é nula";
                            else if (!player.ComparePassword(this.passEnc))
                                str = "Senha inválida";
                            this._client.SendPacket((SendPacket)new BASE_LOGIN_PAK(EventErrorEnum.Login_DELETE_ACCOUNT, this.login, 0L));
                            Logger.LogLogin(str + " [" + this.login + "]");
                            this._client.Close(1000, false);
                        }
                        else if (player.access >= 0)
                        {
                            if (player.MacAddress != this.MacAddress)
                                ComDiv.updateDB("accounts", "last_mac", (object)this.MacAddress, "player_id", (object)player.player_id);
                            bool validMac;
                            bool validIp;
                            BanManager.GetBanStatus(this.MacAddress.ToString(), this.PublicIP, out validMac, out validIp);
                            if (validMac | validIp)
                            {
                                if (validMac)
                                    Logger.LogLogin("MAC banido [" + this.login + "]");
                                else
                                    Logger.LogLogin("IP banido [" + this.login + "]");
                                this._client.SendPacket((SendPacket)new BASE_LOGIN_PAK(validIp ? EventErrorEnum.Login_BLOCK_IP : EventErrorEnum.Login_BLOCK_ACCOUNT, this.login, 0L));
                                this._client.Close(1000, false);
                            }
                            else if (player.IsGM() && config.onlyGM || player.access >= 0 && !config.onlyGM)
                            {
                                Account account = AccountManager.getInstance().getAccount(player.player_id, true);
                                if (!player._isOnline)
                                {
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine("[Login] Id:{0} User: {1} Nick: {2} logou. IP: {3}", (object)player.player_id, (object)player.login, player.player_name.Length > 0 ? (object)player.player_name : (object)"New User", (object)this.PublicIP);
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine("=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=");
                                    if (BanManager.GetAccountBan(player.ban_obj_id).endDate > DateTime.Now)
                                    {
                                        this._client.SendPacket((SendPacket)new BASE_LOGIN_PAK(EventErrorEnum.Login_BLOCK_ACCOUNT, this.login, 0L));
                                        Logger.LogLogin("Conta com ban ativo [" + this.login + "]");
                                        this._client.Close(1000, false);
                                    }
                                    else
                                    {
                                        player.SetPlayerId(player.player_id, 31);
                                        player._clanPlayers = ClanManager.getClanPlayers(player.clan_id, player.player_id);
                                        this._client.SendPacket((SendPacket)new BASE_LOGIN_PAK(0, this.login, player.player_id));
                                        this._client.SendPacket((SendPacket)new AUTH_WEB_CASH_PAK(0));
                                        if (player.clan_id > 0)
                                            this._client.SendPacket((SendPacket)new BASE_USER_CLAN_MEMBERS_PAK(player._clanPlayers));
                                        player._status.SetData(uint.MaxValue, player.player_id);
                                        player._status.updateServer((byte)0);
                                        player.setOnlineStatus(true);
                                        if (account != null)
                                            account._connection = this._client;
                                        SEND_REFRESH_ACC.RefreshAccount(player, true);
                                        Logger.LogLogin("ID: " + player.player_id.ToString() + " Login: " + this.login + " IP: " + this.PublicIP + " MAC: " + this.MacAddress.ToString() + " LocalIP: " + this.LocalIP);
                                    }
                                }
                                else
                                {
                                    this._client.SendPacket((SendPacket)new BASE_LOGIN_PAK(EventErrorEnum.Login_ALREADY_LOGIN_WEB, this.login, 0L));
                                    Logger.LogLogin("Conta online [" + this.login + "]");
                                    if (account != null && account._connection != null)
                                    {
                                        account.SendPacket((SendPacket)new AUTH_ACCOUNT_KICK_PAK(1));
                                        account.SendPacket((SendPacket)new SERVER_MESSAGE_ERROR_PAK(2147487744U));
                                        account.Close(1000);
                                    }
                                    else
                                        Logger.LogLogin(" [Login] O Usuário ja está conectado. Conexão simultânea. Login: " + this.login);
                                    Auth_SyncNet.SendLoginKickInfo(player);
                                    this._client.Close(1000, false);
                                }
                            }
                            else
                            {
                                this._client.SendPacket((SendPacket)new BASE_LOGIN_PAK(EventErrorEnum.Login_TIME_OUT_2, this.login, 0L));
                                Logger.LogLogin("Nível de acesso inválido [" + this.login + "]");
                                this._client.Close(1000, false);
                            }
                        }
                        else
                        {
                            this._client.SendPacket((SendPacket)new BASE_LOGIN_PAK(EventErrorEnum.Login_BLOCK_ACCOUNT, this.login, 0L));
                            Logger.LogLogin("Banido permanente [" + this.login + "]");
                            this._client.Close(1000, false);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.warning("[BASE_LOGIN_REC] " + ex.ToString());
            }
        }

        private void LoginQueue()
        {
            GameServerModel server = ServersXML.getServer(0);
            if (server._LastCount < server._maxPlayers)
                return;
            if (LoginManager._loginQueue.Count >= 100)
            {
                this._client.SendPacket((SendPacket)new BASE_LOGIN_PAK(EventErrorEnum.Login_SERVER_USER_FULL, this.login, 0L));
                Logger.LogLogin("Servidor cheio [" + this.login + "]");
                this._client.Close(1000, false);
            }
            else
            {
                int num = LoginManager.EnterQueue(this._client);
                this._client.SendPacket((SendPacket)new A_LOGIN_QUEUE_PAK(num + 1, (num + 1) * 120));
            }
        }
    }
}
