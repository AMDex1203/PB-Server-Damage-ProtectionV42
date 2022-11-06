
// Type: Game.global.serverpacket.BASE_QUEST_DELETE_CARD_SET_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;
using Game.data.model;

namespace Game.global.serverpacket
{
  public class BASE_QUEST_DELETE_CARD_SET_PAK : SendPacket
  {
    private uint erro;
    private Account p;

    public BASE_QUEST_DELETE_CARD_SET_PAK(uint erro, Account player)
    {
      this.erro = erro;
      this.p = player;
    }

    public override void write()
    {
      this.writeH((short) 2608);
      this.writeD(this.erro);
      if (this.erro != 0U)
        return;
      this.writeC((byte) this.p._mission.actualMission);
      this.writeC((byte) this.p._mission.card1);
      this.writeC((byte) this.p._mission.card2);
      this.writeC((byte) this.p._mission.card3);
      this.writeC((byte) this.p._mission.card4);
      this.writeB(ComDiv.getCardFlags(this.p._mission.mission1, this.p._mission.list1));
      this.writeB(ComDiv.getCardFlags(this.p._mission.mission2, this.p._mission.list2));
      this.writeB(ComDiv.getCardFlags(this.p._mission.mission3, this.p._mission.list3));
      this.writeB(ComDiv.getCardFlags(this.p._mission.mission4, this.p._mission.list4));
      this.writeC((byte) this.p._mission.mission1);
      this.writeC((byte) this.p._mission.mission2);
      this.writeC((byte) this.p._mission.mission3);
      this.writeC((byte) this.p._mission.mission4);
    }
  }
}
