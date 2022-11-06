
// Type: Game.global.clientpacket.BASE_USER_ENTER_REC
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.managers.events;
using Core.models.account.players;
using Core.server;
using Game.data.managers;
using Game.data.model;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
  public class BASE_USER_ENTER_REC : ReceiveGamePacket
  {
    private string login;
    private byte[] LocalIP;
    private long pId;
    private uint erro;
    private int IsRealIP;

    public BASE_USER_ENTER_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read()
    {
      this.login = this.readS((int) this.readC());
      this.pId = this.readQ();
      this.IsRealIP = (int) this.readC();
      this.LocalIP = this.readB(4);
    }

    public override void run()
    {
      if (this._client == null)
        return;
      try
      {
        if (this.LocalIP[0] == (byte) 0 || this.LocalIP[3] == (byte) 0)
        {
          this.erro = 2147483648U;
          Logger.warning("[Aviso] LocalIP off: " + this.LocalIP[0].ToString() + "." + this.LocalIP[1].ToString() + "." + this.LocalIP[2].ToString() + "." + this.LocalIP[3].ToString());
        }
        else if (this._client._player != null)
        {
          this.erro = 2147483648U;
        }
        else
        {
          Account accountDb = AccountManager.getAccountDB((object) this.pId, 2, 0);
          if (accountDb != null && accountDb.login == this.login && accountDb._status.serverId == (byte) 0)
          {
            this._client.player_id = accountDb.player_id;
            accountDb._connection = this._client;
            accountDb.GetAccountInfos(29);
            accountDb.LocalIP = this.LocalIP;
            accountDb.LoadInventory();
            accountDb.LoadMissionList();
            accountDb.LoadPlayerBonus();
            this.EnableQuestMission(accountDb);
            accountDb._inventory.LoadBasicItems();
            accountDb.SetPublicIP(this._client.GetAddress());
            accountDb.Session = new PlayerSession()
            {
              _sessionId = this._client.SessionId,
              _playerId = this._client.player_id
            };
            accountDb.updateCacheInfo();
            accountDb._status.updateServer((byte) ConfigGS.serverId);
            this._client._player = accountDb;
            ComDiv.updateDB("accounts", "lastip", (object) accountDb.PublicIP.ToString(), "player_id", (object) accountDb.player_id);
          }
          else
            this.erro = 2147483648U;
        }
        this._client.SendPacket((SendPacket) new BASE_USER_ENTER_PAK(this.erro));
        if (this.erro <= 0U)
          return;
        this._client.Close(500);
      }
      catch (Exception ex)
      {
        Logger.info("BASE_USER_ENTER_REC: " + ex.ToString());
        this._client.Close(0);
      }
    }

    private void EnableQuestMission(Account player)
    {
      PlayerEvent playerEvent = player._event;
      if (playerEvent == null || playerEvent.LastQuestFinish != 0 || EventQuestSyncer.GetRunningEvent() == null)
        return;
      player._mission.mission4 = 13;
    }
  }
}
