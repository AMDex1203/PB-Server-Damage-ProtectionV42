
// Type: Game.global.serverpacket.LOBBY_GET_PLAYERINFO2_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;
using Game.data.managers;
using Game.data.model;

namespace Game.global.serverpacket
{
  public class LOBBY_GET_PLAYERINFO2_PAK : SendPacket
  {
    private Account ac;

    public LOBBY_GET_PLAYERINFO2_PAK(long player) => this.ac = AccountManager.getAccount(player, true);

    public override void write()
    {
      this.writeH((short) 3100);
      if (this.ac != null && this.ac._equip != null)
      {
        this.writeD(this.ac._equip._primary);
        this.writeD(this.ac._equip._secondary);
        this.writeD(this.ac._equip._melee);
        this.writeD(this.ac._equip._grenade);
        this.writeD(this.ac._equip._special);
        this.writeD(this.ac._equip._red);
        this.writeD(this.ac._equip._blue);
        this.writeD(this.ac._equip._helmet);
        this.writeD(this.ac._equip._beret);
        this.writeD(this.ac._equip._dino);
      }
      else
      {
        this.writeD(0);
        this.writeD(601002003);
        this.writeD(702001001);
        this.writeD(803007001);
        this.writeD(904007002);
        this.writeD(1001001005);
        this.writeD(1001002006);
        this.writeD(1102003001);
        this.writeD(0);
        this.writeD(1006003041);
      }
      this.writeD(0);
    }
  }
}
