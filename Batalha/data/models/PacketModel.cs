
// Type: Battle.data.models.PacketModel
// Assembly: pbserver_battle, Version=1.0.0.2646, Culture=neutral, PublicKeyToken=null
// MVID: AA2D7911-B687-4238-AB73-D37E54123039
// Interprise: C:\Users\Cuzin\3,50pbserver_battle.exe

using System;

namespace Battle.data.models
{
  public class PacketModel
  {
    public int _opcode;
    public int _slot;
    public int _round;
    public int _length;
    public int _accountId;
    public int _unkInfo2;
    public int _respawnNumber;
    public int _roundNumber;
    public float _time;
    public byte[] _data;
    public byte[] _withEndData;
    public byte[] _noEndData;
    public DateTime _receiveDate;
  }
}
