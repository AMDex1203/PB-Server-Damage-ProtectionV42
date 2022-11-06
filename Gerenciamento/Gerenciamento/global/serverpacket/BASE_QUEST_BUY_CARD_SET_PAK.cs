
// Type: Game.global.serverpacket.BASE_QUEST_BUY_CARD_SET_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;
using Game.data.model;

namespace Game.global.serverpacket
{
  public class BASE_QUEST_BUY_CARD_SET_PAK : SendPacket
  {
    private Account player;
    private uint _erro;

    public BASE_QUEST_BUY_CARD_SET_PAK(uint erro, Account p)
    {
      this._erro = erro;
      this.player = p;
    }

    public override void write()
    {
      this.writeH((short) 2606);
      this.writeD(this._erro);
      if (this._erro != 0U)
        return;
      this.writeD(this.player._gp);
      this.writeC((byte) this.player._mission.actualMission);
      this.writeC((byte) this.player._mission.actualMission);
      this.writeC((byte) this.player._mission.card1);
      this.writeC((byte) this.player._mission.card2);
      this.writeC((byte) this.player._mission.card3);
      this.writeC((byte) this.player._mission.card4);
      this.writeB(ComDiv.getCardFlags(this.player._mission.mission1, this.player._mission.list1));
      this.writeB(ComDiv.getCardFlags(this.player._mission.mission2, this.player._mission.list2));
      this.writeB(ComDiv.getCardFlags(this.player._mission.mission3, this.player._mission.list3));
      this.writeB(ComDiv.getCardFlags(this.player._mission.mission4, this.player._mission.list4));
      this.writeC((byte) this.player._mission.mission1);
      this.writeC((byte) this.player._mission.mission2);
      this.writeC((byte) this.player._mission.mission3);
      this.writeC((byte) this.player._mission.mission4);
    }
  }
}
