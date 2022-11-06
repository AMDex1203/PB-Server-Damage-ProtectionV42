
// Type: Auth.global.clientpacket.BASE_CONFIG_SAVE_REC
// Assembly: pbserver_auth, Version=1.0.7796.38604, Culture=neutral, PublicKeyToken=null
// MVID: 1CFDFF97-782C-4EF8-A621-6E4FDDE32436
// Interprise: C:\Users\Cuzin\3,50pbserver_auth.exe

using Auth.data.model;
using Core.managers;
using Core.models.account.players;
using Core.server;

namespace Auth.global.clientpacket
{
  public class BASE_CONFIG_SAVE_REC : ReceiveLoginPacket
  {
    private int type;
    private DBQuery query = new DBQuery();

    public BASE_CONFIG_SAVE_REC(LoginClient client, byte[] data) => this.makeme(client, data);

    public override void read()
    {
      Account player = this._client._player;
      if (player == null || player._config == null)
        return;
      this.type = this.readD();
      if ((this.type & 1) == 1)
      {
        player._config.blood = (int) this.readH();
        player._config.sight = (int) this.readC();
        player._config.hand = (int) this.readC();
        player._config.config = this.readD();
        player._config.audio_enable = (int) this.readC();
        this.readB(5);
        player._config.audio1 = (int) this.readC();
        player._config.audio2 = (int) this.readC();
        player._config.fov = (int) this.readH();
        player._config.sensibilidade = (int) this.readC();
        player._config.mouse_invertido = (int) this.readC();
        int num1 = (int) this.readC();
        int num2 = (int) this.readC();
        player._config.msgConvite = (int) this.readC();
        player._config.chatSussurro = (int) this.readC();
        player._config.macro = (int) this.readC();
        int num3 = (int) this.readC();
        int num4 = (int) this.readC();
        int num5 = (int) this.readC();
      }
      if ((this.type & 2) == 2)
      {
        this.readB(5);
        byte[] numArray = this.readB(215);
        player._config.keys = numArray;
      }
      if ((this.type & 4) != 4)
        return;
      player._config.macro_1 = this.readS((int) this.readC());
      player._config.macro_2 = this.readS((int) this.readC());
      player._config.macro_3 = this.readS((int) this.readC());
      player._config.macro_4 = this.readS((int) this.readC());
      player._config.macro_5 = this.readS((int) this.readC());
    }

    public override void run()
    {
      Account player = this._client._player;
      if (player == null || player._config == null)
        return;
      PlayerConfig config = player._config;
      if ((this.type & 1) == 1)
        PlayerManager.updateConfigs(this.query, config);
      if ((this.type & 2) == 2)
        this.query.AddQuery("keys", (object) config.keys);
      if ((this.type & 4) == 4)
        PlayerManager.updateMacros(this.query, config, this.type);
      ComDiv.updateDB("player_configs", "owner_id", (object) player.player_id, this.query.GetTables(), this.query.GetValues());
    }
  }
}
