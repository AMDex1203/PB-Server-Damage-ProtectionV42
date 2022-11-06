
// Type: Game.global.clientpacket.CLAN_CHECK_DUPLICATE_LOGO_REC
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;
using Game.data.managers;
using Game.data.model;
using Game.global.serverpacket;

namespace Game.global.clientpacket
{
  public class CLAN_CHECK_DUPLICATE_LOGO_REC : ReceiveGamePacket
  {
    private uint logo;
    private uint erro;

    public CLAN_CHECK_DUPLICATE_LOGO_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read() => this.logo = this.readUD();

    public override void run()
    {
      Account player = this._client._player;
      if (player == null || (int) ClanManager.getClan(player.clanId).logo == (int) this.logo || ClanManager.isClanLogoExist(this.logo))
        this.erro = 2147483648U;
      this._client.SendPacket((SendPacket) new CLAN_CHECK_DUPLICATE_MARK_PAK(this.erro));
    }
  }
}
