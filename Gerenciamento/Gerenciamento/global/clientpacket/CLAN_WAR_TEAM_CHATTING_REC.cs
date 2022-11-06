
// Type: Game.global.clientpacket.CLAN_WAR_TEAM_CHATTING_REC
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.models.enums;
using Core.models.enums.global;
using Core.models.enums.match;
using Core.server;
using Game.data.managers;
using Game.data.model;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
  public class CLAN_WAR_TEAM_CHATTING_REC : ReceiveGamePacket
  {
    private ChattingType type;
    private string text;

    public CLAN_WAR_TEAM_CHATTING_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read()
    {
      this.type = (ChattingType) this.readH();
      this.text = this.readS((int) this.readH());
    }

    public override void run()
    {
      try
      {
        Account player = this._client._player;
        if (player == null || player._match == null || this.type != ChattingType.Match)
          return;
        Match match = player._match;
        this.serverCommands(player, match);
        using (CLAN_WAR_TEAM_CHATTING_PAK warTeamChattingPak = new CLAN_WAR_TEAM_CHATTING_PAK(player.player_name, this.text))
          match.SendPacketToPlayers((SendPacket) warTeamChattingPak);
      }
      catch (Exception ex)
      {
        Logger.info("CLAN_WAR_TEAM_CHATTING_REC: " + ex.ToString());
      }
    }

    private bool serverCommands(Account player, Match match)
    {
      try
      {
        if (!player.HaveGMLevel() || !this.text.StartsWith(";") && !this.text.StartsWith("\\") && !this.text.StartsWith("."))
          return false;
        string str = this.text.Substring(1);
        if (str.StartsWith("o") && player.access >= AccessLevel.Moderator)
        {
          if (match != null)
          {
            AccountManager.getAccountDB((object) 2L, 2);
            for (int index = 0; index < match.formação; ++index)
            {
              SLOT_MATCH slot = match._slots[index];
              if (slot._playerId == 0L)
              {
                slot._playerId = 2L;
                slot.state = SlotMatchState.Normal;
              }
            }
            using (CLAN_WAR_REGIST_MERCENARY_PAK registMercenaryPak = new CLAN_WAR_REGIST_MERCENARY_PAK(match))
              match.SendPacketToPlayers((SendPacket) registMercenaryPak);
            this.text = "Disputa preenchida. [Servidor]";
          }
          else
            this.text = "Falha ao encher a disputa. [Servidor]";
        }
        else if (str.StartsWith("gg"))
        {
          match._state = MatchState.Play;
          match._slots[player.matchSlot].state = SlotMatchState.Ready;
          this._client.SendPacket((SendPacket) new CLAN_WAR_ENEMY_INFO_PAK(match));
          this._client.SendPacket((SendPacket) new CLAN_WAR_CREATED_ROOM_PAK(match));
        }
        else
          this.text = "Falha ao encontrar o comando digitado. [Servidor]";
        return true;
      }
      catch (OverflowException ex)
      {
        return true;
      }
      catch (Exception ex)
      {
        Logger.warning(ex.ToString());
        return true;
      }
    }
  }
}
