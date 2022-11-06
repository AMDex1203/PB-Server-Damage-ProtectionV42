
// Type: Battle.network.BattlePacketReader
// Assembly: pbserver_battle, Version=1.0.0.2646, Culture=neutral, PublicKeyToken=null
// MVID: AA2D7911-B687-4238-AB73-D37E54123039
// Interprise: C:\Users\Cuzin\3,50pbserver_battle.exe

using PointBlank;
using System;

namespace Battle.network
{
  public class BattlePacketReader
  {
    private readonly byte[] buffer;
    private int offset;

    public BattlePacketReader(byte[] buffer) => this.buffer = buffer;

    protected internal void Advance(int bytes)
    {
      this.offset += bytes;
      if (this.offset <= this.buffer.Length)
        return;
      Logger.error(string.Format(" [BattlePacketReader] Offset ultrapassou o valor do buffer. ({0}/{1})", (object) this.offset, (object) this.buffer.Length));
    }

    protected internal int ReadInt()
    {
      int int32 = BitConverter.ToInt32(this.buffer, this.offset);
      this.offset += 4;
      return int32;
    }

    protected internal uint ReadUint()
    {
      uint uint32 = BitConverter.ToUInt32(this.buffer, this.offset);
      this.offset += 4;
      return uint32;
    }

    protected internal Half3 ReadUshortVector() => new Half3(this.ReadUshort(), this.ReadUshort(), this.ReadUshort());

    protected internal Half3 ReadFloatVector() => new Half3(this.ReadFloat(), this.ReadFloat(), this.ReadFloat());

    protected internal byte ReadByteOutException(out bool exception)
    {
      try
      {
        exception = false;
        return this.buffer[this.offset++];
      }
      catch
      {
        exception = true;
        return 0;
      }
    }

    protected internal byte ReadByte() => this.buffer[this.offset++];

    protected internal byte[] ReadB(int Length)
    {
      byte[] numArray = new byte[Length];
      Array.Copy((Array) this.buffer, this.offset, (Array) numArray, 0, Length);
      this.offset += Length;
      return numArray;
    }

    protected internal long ReadLong()
    {
      long int64 = BitConverter.ToInt64(this.buffer, this.offset);
      this.offset += 8;
      return int64;
    }

    protected internal ulong ReadUlong()
    {
      ulong uint64 = BitConverter.ToUInt64(this.buffer, this.offset);
      this.offset += 8;
      return uint64;
    }

    protected internal short ReadShort()
    {
      short int16 = BitConverter.ToInt16(this.buffer, this.offset);
      this.offset += 2;
      return int16;
    }

    protected internal ushort ReadUshort()
    {
      ushort uint16 = BitConverter.ToUInt16(this.buffer, this.offset);
      this.offset += 2;
      return uint16;
    }

    protected internal float ReadFloat()
    {
      float single = BitConverter.ToSingle(this.buffer, this.offset);
      this.offset += 4;
      return single;
    }
  }
}
