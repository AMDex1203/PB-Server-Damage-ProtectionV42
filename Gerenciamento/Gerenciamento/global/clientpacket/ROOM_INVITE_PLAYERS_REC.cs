
// Type: Game.global.clientpacket.ROOM_INVITE_PLAYERS_REC
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;
using Game.data.managers;
using Game.data.model;
using Game.global.serverpacket;

namespace Game.global.clientpacket
{
  public class ROOM_INVITE_PLAYERS_REC : ReceiveGamePacket
  {
    private int count;
    private uint erro;

    public ROOM_INVITE_PLAYERS_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read() => this.count = this.readD();

    public override void run()
    {
      Account player = this._client._player;
      if (player != null && player._room != null)
      {
        Channel channel = player.getChannel();
        if (channel != null)
        {
          using (ROOM_INVITE_SHOW_PAK roomInviteShowPak = new ROOM_INVITE_SHOW_PAK(player, player._room))
          {
            byte[] completeBytes = roomInviteShowPak.GetCompleteBytes(nameof (ROOM_INVITE_PLAYERS_REC));
            for (int index = 0; index < this.count; ++index)
            {
              try
              {
                AccountManager.getAccount(channel.getPlayer(this.readUD())._playerId, true)?.SendCompletePacket(completeBytes);
              }
              catch
              {
              }
            }
          }
        }
      }
      else
        this.erro = 2147483648U;
      this._client.SendPacket((SendPacket) new ROOM_INVITE_RETURN_PAK(this.erro));
    }
  }
}
