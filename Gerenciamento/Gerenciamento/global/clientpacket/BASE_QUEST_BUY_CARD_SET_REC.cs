
// Type: Game.global.clientpacket.BASE_QUEST_BUY_CARD_SET_REC
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.managers;
using Core.models.account.players;
using Core.server;
using Core.xml;
using Game.data.model;
using Game.global.serverpacket;

namespace Game.global.clientpacket
{
  public class BASE_QUEST_BUY_CARD_SET_REC : ReceiveGamePacket
  {
    private int missionId;
    private uint erro;

    public BASE_QUEST_BUY_CARD_SET_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read() => this.missionId = (int) this.readC();

    public override void run()
    {
      Account player = this._client._player;
      if (player == null)
        return;
      PlayerMissions mission = player._mission;
      int missionPrice = MissionsXML.GetMissionPrice(this.missionId);
      if (player == null || missionPrice == -1 || (0 > player._gp - missionPrice || mission.mission1 == this.missionId) || (mission.mission2 == this.missionId || mission.mission3 == this.missionId))
      {
        this.erro = missionPrice == -1 ? 2147487820U : 2147487821U;
      }
      else
      {
        if (mission.mission1 == 0)
        {
          if (PlayerManager.updateMissionId(player.player_id, this.missionId, 0))
          {
            mission.mission1 = this.missionId;
            mission.list1 = new byte[40];
            mission.actualMission = 0;
            mission.card1 = 0;
          }
          else
            this.erro = 2147487820U;
        }
        else if (mission.mission2 == 0)
        {
          if (PlayerManager.updateMissionId(player.player_id, this.missionId, 1))
          {
            mission.mission2 = this.missionId;
            mission.list2 = new byte[40];
            mission.actualMission = 1;
            mission.card2 = 0;
          }
          else
            this.erro = 2147487820U;
        }
        else if (mission.mission3 == 0)
        {
          if (PlayerManager.updateMissionId(player.player_id, this.missionId, 2))
          {
            mission.mission3 = this.missionId;
            mission.list3 = new byte[40];
            mission.actualMission = 2;
            mission.card3 = 0;
          }
          else
            this.erro = 2147487820U;
        }
        else
          this.erro = 2147487822U;
        if (this.erro == 0U)
        {
          if (missionPrice == 0 || PlayerManager.updateAccountGold(player.player_id, player._gp - missionPrice))
            player._gp -= missionPrice;
          else
            this.erro = 2147487820U;
        }
      }
      this._client.SendPacket((SendPacket) new BASE_QUEST_BUY_CARD_SET_PAK(this.erro, player));
    }
  }
}
