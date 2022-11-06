
// Type: Game.global.clientpacket.EVENT_VISIT_CONFIRM_REC
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.managers.events;
using Core.managers.events.EventModels;
using Core.models.account.players;
using Core.models.enums.errors;
using Core.server;
using Game.data.model;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
  public class EVENT_VISIT_CONFIRM_REC : ReceiveGamePacket
  {
    private EventErrorEnum erro = EventErrorEnum.VisitEvent_Success;
    private int eventId;
    private EventVisitModel eventv;

    public EVENT_VISIT_CONFIRM_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read() => this.eventId = this.readD();

    public override void run()
    {
      try
      {
        if (this._client == null)
          return;
        Account player = this._client._player;
        if (player == null || string.IsNullOrEmpty(player.player_name))
          this.erro = EventErrorEnum.VisitEvent_UserFail;
        else if (player._event != null)
        {
          DateTime dateTime = DateTime.Now;
          int num1 = int.Parse(dateTime.ToString("yyMMdd"));
          if (player._event.NextVisitDate <= num1)
          {
            this.eventv = EventVisitSyncer.GetEvent(this.eventId);
            if (this.eventv == null)
              this.erro = EventErrorEnum.VisitEvent_Unknown;
            else if (this.eventv.EventIsEnabled())
            {
              PlayerEvent playerEvent = player._event;
              dateTime = DateTime.Now;
              dateTime = dateTime.AddDays(1.0);
              int num2 = int.Parse(dateTime.ToString("yyMMdd"));
              playerEvent.NextVisitDate = num2;
              ComDiv.updateDB("player_events", "player_id", (object) player.player_id, new string[2]
              {
                "next_visit_date",
                "last_visit_sequence1"
              }, (object) player._event.NextVisitDate, (object) ++player._event.LastVisitSequence1);
              bool flag = false;
              try
              {
                flag = this.eventv.box[player._event.LastVisitSequence2].reward1.IsReward;
              }
              catch
              {
              }
              if (!flag)
                ComDiv.updateDB("player_events", "last_visit_sequence2", (object) ++player._event.LastVisitSequence2, "player_id", (object) player.player_id);
            }
            else
              this.erro = EventErrorEnum.VisitEvent_WrongVersion;
          }
          else
            this.erro = EventErrorEnum.VisitEvent_AlreadyCheck;
        }
        else
          this.erro = EventErrorEnum.VisitEvent_Unknown;
        this._client.SendPacket((SendPacket) new EVENT_VISIT_CONFIRM_PAK(this.erro, this.eventv, player._event));
      }
      catch (Exception ex)
      {
        Logger.info("EVENT_VERIFICATION_CHECK_REC] " + ex.ToString());
      }
    }
  }
}
