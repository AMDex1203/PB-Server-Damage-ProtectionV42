﻿
// Type: Game.global.clientpacket.BASE_CONFIG_SAVE_REC
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.managers;
using Core.models.account.players;
using Core.server;
using Game.data.model;

namespace Game.global.clientpacket
{
  public class BASE_CONFIG_SAVE_REC : ReceiveGamePacket
  {
    private int type;
    private DBQuery query = new DBQuery();

    public BASE_CONFIG_SAVE_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read()
    {
      Account player = this._client._player;
      if (player == null)
        return;
      bool flag = player._config != null;
      if (!flag)
      {
        flag = PlayerManager.CreateConfigDB(player.player_id);
        if (flag)
          player._config = new PlayerConfig();
      }
      if (!flag)
        return;
      PlayerConfig config = player._config;
      this.type = this.readD();
      if ((this.type & 1) == 1)
      {
        config.blood = (int) this.readH();
        config.sight = (int) this.readC();
        config.hand = (int) this.readC();
        config.config = this.readD();
        config.audio_enable = (int) this.readC();
        this.readB(5);
        config.audio1 = (int) this.readC();
        config.audio2 = (int) this.readC();
        config.fov = (int) this.readH();
        config.sensibilidade = (int) this.readC();
        config.mouse_invertido = (int) this.readC();
        int num1 = (int) this.readC();
        int num2 = (int) this.readC();
        config.msgConvite = (int) this.readC();
        config.chatSussurro = (int) this.readC();
        config.macro = (int) this.readC();
        int num3 = (int) this.readC();
        int num4 = (int) this.readC();
        int num5 = (int) this.readC();
      }
      if ((this.type & 2) == 2)
      {
        this.readB(5);
        byte[] numArray = this.readB(215);
        config.keys = numArray;
      }
      if ((this.type & 4) != 4)
        return;
      config.macro_1 = this.readS((int) this.readC());
      config.macro_2 = this.readS((int) this.readC());
      config.macro_3 = this.readS((int) this.readC());
      config.macro_4 = this.readS((int) this.readC());
      config.macro_5 = this.readS((int) this.readC());
    }

    public override void run()
    {
      Account player = this._client._player;
      if (player == null)
        return;
      PlayerConfig config = player._config;
      if (config == null)
        return;
      if ((this.type & 1) == 1)
        PlayerManager.updateConfigs(this.query, config);
      if ((this.type & 2) == 2)
        this.query.AddQuery("keys", (object) config.keys);
      if ((this.type & 4) == 4)
        PlayerManager.updateMacros(this.query, config, this.type);
      ComDiv.updateDB("player_configs", "owner_id", (object) this._client.player_id, this.query.GetTables(), this.query.GetValues());
    }
  }
}
