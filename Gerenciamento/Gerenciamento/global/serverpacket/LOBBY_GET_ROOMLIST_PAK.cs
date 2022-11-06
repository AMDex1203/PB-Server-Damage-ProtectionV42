
// Type: Game.global.serverpacket.LOBBY_GET_ROOMLIST_PAK
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.server;

namespace Game.global.serverpacket
{
  public class LOBBY_GET_ROOMLIST_PAK : SendPacket
  {
    private int _roomPage;
    private int _playerPage;
    private int _allPlayers;
    private int _allRooms;
    private int _count1;
    private int _count2;
    private byte[] _salas;
    private byte[] _waiting;

    public LOBBY_GET_ROOMLIST_PAK(
      int allRooms,
      int allPlayers,
      int roomPage,
      int playerPage,
      int count1,
      int count2,
      byte[] rooms,
      byte[] players)
    {
      this._allRooms = allRooms;
      this._allPlayers = allPlayers;
      this._roomPage = roomPage;
      this._playerPage = playerPage;
      this._salas = rooms;
      this._waiting = players;
      this._count1 = count1;
      this._count2 = count2;
    }

    public override void write()
    {
      this.writeH((short) 3074);
      this.writeD(this._allRooms);
      this.writeD(this._roomPage);
      this.writeD(this._count1);
      this.writeB(this._salas);
      this.writeD(this._allPlayers);
      this.writeD(this._playerPage);
      this.writeD(this._count2);
      this.writeB(this._waiting);
    }
  }
}
