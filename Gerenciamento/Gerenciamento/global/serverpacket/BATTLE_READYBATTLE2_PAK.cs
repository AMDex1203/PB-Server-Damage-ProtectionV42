
// Type: Game.global.serverpacket.BATTLE_READYBATTLE2_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.models.account.title;
using Core.models.room;
using Core.server;

namespace Game.global.serverpacket
{
  public class BATTLE_READYBATTLE2_PAK : SendPacket
  {
    private Slot slot;
    private PlayerTitles title;

    public BATTLE_READYBATTLE2_PAK(Slot slot, PlayerTitles title)
    {
      this.slot = slot;
      this.title = title;
    }

    public override void write()
    {
      if (this.slot._equip == null)
        return;
      this.writeH((short) 3427);
      this.writeC((byte) this.slot._id);
      this.writeD(this.slot._equip._red);
      this.writeD(this.slot._equip._blue);
      this.writeD(this.slot._equip._helmet);
      this.writeD(this.slot._equip._beret);
      this.writeD(this.slot._equip._dino);
      this.writeD(this.slot._equip._primary);
      this.writeD(this.slot._equip._secondary);
      this.writeD(this.slot._equip._melee);
      this.writeD(this.slot._equip._grenade);
      this.writeD(this.slot._equip._special);
      this.writeD(0);
      this.writeC((byte) this.title.Equiped1);
      this.writeC((byte) this.title.Equiped2);
      this.writeC((byte) this.title.Equiped3);
      if (!(GameManager.Config.ClientVersion == "1.15.42"))
        return;
      this.writeD(0);
    }
  }
}
