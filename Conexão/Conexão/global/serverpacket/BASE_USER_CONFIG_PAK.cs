
// Type: Auth.global.serverpacket.BASE_USER_CONFIG_PAK
// Assembly: pbserver_auth, Version=1.0.7796.38604, Culture=neutral, PublicKeyToken=null
// MVID: 1CFDFF97-782C-4EF8-A621-6E4FDDE32436
// Interprise: C:\Users\Cuzin\3,50pbserver_auth.exe

using Core.models.account.players;
using Core.server;

namespace Auth.global.serverpacket
{
  public class BASE_USER_CONFIG_PAK : SendPacket
  {
    private int error;
    private PlayerConfig c;
    private bool isValid;

    public BASE_USER_CONFIG_PAK(int error, PlayerConfig config)
    {
      this.error = error;
      this.c = config;
      this.isValid = this.c != null;
    }

    public BASE_USER_CONFIG_PAK(int error) => this.error = error;

    public override void write()
    {
      this.writeH((short) 2568);
      this.writeD(this.error);
      if (this.error < 0)
        return;
      this.writeC(!this.isValid);
      if (!this.isValid)
        return;
      this.writeH((short) this.c.blood);
      this.writeC((byte) this.c.sight);
      this.writeC((byte) this.c.hand);
      this.writeD(this.c.config);
      this.writeD(this.c.audio_enable);
      this.writeH((short) 0);
      this.writeC((byte) this.c.audio1);
      this.writeC((byte) this.c.audio2);
      this.writeC((byte) this.c.fov);
      this.writeC((byte) 0);
      this.writeC((byte) this.c.sensibilidade);
      this.writeC((byte) this.c.mouse_invertido);
      this.writeH((short) 0);
      this.writeC((byte) this.c.msgConvite);
      this.writeC((byte) this.c.chatSussurro);
      this.writeD(this.c.macro);
      this.writeB(new byte[5]
      {
        (byte) 0,
        (byte) 57,
        (byte) 248,
        (byte) 16,
        (byte) 0
      });
      this.writeB(this.c.keys);
      this.writeC((byte) (this.c.macro_1.Length + 1));
      this.writeS(this.c.macro_1, this.c.macro_1.Length + 1);
      this.writeC((byte) (this.c.macro_2.Length + 1));
      this.writeS(this.c.macro_2, this.c.macro_2.Length + 1);
      this.writeC((byte) (this.c.macro_3.Length + 1));
      this.writeS(this.c.macro_3, this.c.macro_3.Length + 1);
      this.writeC((byte) (this.c.macro_4.Length + 1));
      this.writeS(this.c.macro_4, this.c.macro_4.Length + 1);
      this.writeC((byte) (this.c.macro_5.Length + 1));
      this.writeS(this.c.macro_5, this.c.macro_5.Length + 1);
    }
  }
}
