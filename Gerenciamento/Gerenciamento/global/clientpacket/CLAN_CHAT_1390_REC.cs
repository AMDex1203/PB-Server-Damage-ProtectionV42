
// Type: Game.global.clientpacket.CLAN_CHAT_1390_REC
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.models.enums.global;
using Core.server;
using Game.data.managers;
using Game.data.model;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
  public class CLAN_CHAT_1390_REC : ReceiveGamePacket
  {
    private ChattingType type;
    private string text;

    public CLAN_CHAT_1390_REC(GameClient client, byte[] data) => this.makeme(client, data);

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
        if (player == null || this.type != ChattingType.Clan_Member_Page)
          return;
        this.serverCommands(player, player._room);
        using (CLAN_CHAT_1390_PAK clanChat1390Pak = new CLAN_CHAT_1390_PAK(player, this.text))
          ClanManager.SendPacket((SendPacket) clanChat1390Pak, player.clanId, -1L, true, true);
      }
      catch
      {
      }
    }

    private bool serverCommands(Account player, Room room)
    {
      try
      {
        if (!player.HaveGMLevel() || !this.text.StartsWith(";") && !this.text.StartsWith("\\") && !this.text.StartsWith("."))
          return false;
        Logger.LogCMD("[" + this.text + "] playerId: " + player.player_id.ToString() + "; Nick: '" + player.player_name + "'; Login: '" + player.login + "'; Ip: '" + player.PublicIP.ToString() + "'; Date: '" + DateTime.Now.ToString("dd/MM/yy HH:mm") + "'");
        string str = this.text.Substring(1);
        if (str.StartsWith("vv "))
          this._client.SendPacket((SendPacket) new HELPER_PAK(ushort.Parse(str.Substring(3))));
        else
          this.text = Translation.GetLabel("UnknownCmd");
        return true;
      }
      catch (Exception ex)
      {
        Logger.warning("[CLAN_CHAT_1390_REC] " + ex.ToString());
        this.text = Translation.GetLabel("CrashProblemCmd");
        return true;
      }
    }
  }
}
