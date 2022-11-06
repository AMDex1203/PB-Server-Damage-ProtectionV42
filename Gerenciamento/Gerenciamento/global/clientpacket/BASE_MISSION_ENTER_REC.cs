
// Type: Game.global.clientpacket.BASE_MISSION_ENTER_REC
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.models.account.players;
using Core.server;
using Game.data.model;
using System;

namespace Game.global.clientpacket
{
  public class BASE_MISSION_ENTER_REC : ReceiveGamePacket
  {
    private int cardIdx;
    private int actualMission;
    private int cardFlags;

    public BASE_MISSION_ENTER_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read()
    {
      this.actualMission = (int) this.readC();
      this.cardIdx = (int) this.readC();
      this.cardFlags = (int) this.readUH();
    }

    public override void run()
    {
      try
      {
        if (this._client == null)
          return;
        Account player = this._client._player;
        if (player == null)
          return;
        PlayerMissions mission = player._mission;
        DBQuery dbQuery = new DBQuery();
        if (mission.getCard(this.actualMission) != this.cardIdx)
        {
          if (this.actualMission == 0)
            mission.card1 = this.cardIdx;
          else if (this.actualMission == 1)
            mission.card2 = this.cardIdx;
          else if (this.actualMission == 2)
            mission.card3 = this.cardIdx;
          else if (this.actualMission == 3)
            mission.card4 = this.cardIdx;
          dbQuery.AddQuery("card" + (this.actualMission + 1).ToString(), (object) this.cardIdx);
        }
        mission.selectedCard = this.cardFlags == (int) ushort.MaxValue;
        if (mission.actualMission != this.actualMission)
        {
          dbQuery.AddQuery("actual_mission", (object) this.actualMission);
          mission.actualMission = this.actualMission;
        }
        ComDiv.updateDB("player_missions", "owner_id", (object) this._client.player_id, dbQuery.GetTables(), dbQuery.GetValues());
      }
      catch (Exception ex)
      {
        Logger.info("BASE_MISSION_ENTER_REC: " + ex.ToString());
      }
    }
  }
}
