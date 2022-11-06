
// Type: Game.global.clientpacket.CLAN_WAR_ACCEPT_BATTLE_REC
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.models.enums.match;
using Core.server;
using Game.data.model;
using Game.data.xml;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
  public class CLAN_WAR_ACCEPT_BATTLE_REC : ReceiveGamePacket
  {
    private int id;
    private int serverInfo;
    private int type;
    private uint erro;

    public CLAN_WAR_ACCEPT_BATTLE_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read()
    {
      this.readD();
      this.id = (int) this.readH();
      this.serverInfo = (int) this.readH();
      this.type = (int) this.readC();
    }

    public override void run()
    {
      try
      {
        Account player = this._client._player;
        if (player == null)
          return;
        Match match1 = player._match;
        Match match2 = ChannelsXML.getChannel(this.serverInfo - this.serverInfo / 10 * 10).getMatch(this.id);
        if (match1 != null && match2 != null && player.matchSlot == match1._leader)
        {
          if (this.type == 1)
          {
            if (match1.formação != match2.formação)
              this.erro = 2147487890U;
            else if (match2.getCountPlayers() != match1.formação || match1.getCountPlayers() != match1.formação)
              this.erro = 2147487889U;
            else if (match2._state == MatchState.Play || match1._state == MatchState.Play)
            {
              this.erro = 2147487888U;
            }
            else
            {
              match1._state = MatchState.Play;
              Account leader = match2.getLeader();
              if (leader != null && leader._match != null)
              {
                leader.SendPacket((SendPacket) new CLAN_WAR_ENEMY_INFO_PAK(match1));
                leader.SendPacket((SendPacket) new CLAN_WAR_CREATED_ROOM_PAK(match1));
                match2._slots[leader.matchSlot].state = SlotMatchState.Ready;
              }
              match2._state = MatchState.Play;
            }
          }
          else
          {
            Account leader = match2.getLeader();
            if (leader != null && leader._match != null)
              leader.SendPacket((SendPacket) new CLAN_WAR_RECUSED_BATTLE_PAK(2147487891U));
          }
        }
        else
          this.erro = 2147487892U;
        this._client.SendPacket((SendPacket) new CLAN_WAR_ACCEPTED_BATTLE_PAK(this.erro));
      }
      catch (Exception ex)
      {
        Logger.info("CLAN_WAR_ACCEPT_BATTLE_REC: " + ex.ToString());
      }
    }
  }
}
