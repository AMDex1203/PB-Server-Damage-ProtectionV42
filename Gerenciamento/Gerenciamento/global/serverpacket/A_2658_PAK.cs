
// Type: Game.global.serverpacket.A_2658_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;

namespace Game.global.serverpacket
{
  public class A_2658_PAK : SendPacket
  {
    private int rewardId;
    private int roomId;
    private string player_name;

    public A_2658_PAK(string player_name, int roomId, int rewardId)
    {
      this.player_name = player_name;
      this.roomId = roomId;
      this.rewardId = rewardId;
    }

    public override void write()
    {
      this.writeH((short) 2658);
      this.writeC((byte) 0);
      this.writeD(0);
      this.writeD(0);
      this.writeD(this.roomId);
      this.writeD(this.rewardId);
      this.writeC((byte) (this.player_name.Length + 1));
      this.writeS(this.player_name, this.player_name.Length + 1);
    }
  }
}
