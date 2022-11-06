
// Type: Battle.network.BattlePacketWriter
// Assembly: pbserver_battle, Version=1.0.0.2646, Culture=neutral, PublicKeyToken=null
// MVID: AA2D7911-B687-4238-AB73-D37E54123039
// Interprise: C:\Users\Cuzin\3,50pbserver_battle.exe

using Microsoft.Win32.SafeHandles;
using PointBlank;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Battle.network
{
  public class BattlePacketWriter : IDisposable
  {
    public MemoryStream memory = new MemoryStream();
    private bool disposed = false;
    private SafeHandle handle = (SafeHandle) new SafeFileHandle(IntPtr.Zero, true);

    protected internal void WriteB(byte[] value) => this.memory.Write(value, 0, value.Length);

    protected internal void WriteD(int value) => this.WriteB(BitConverter.GetBytes(value));

    protected internal void WriteD(uint value) => this.WriteB(BitConverter.GetBytes(value));

    protected internal void WriteH(short val) => this.WriteB(BitConverter.GetBytes(val));

    protected internal void WriteH(ushort val) => this.WriteB(BitConverter.GetBytes(val));

    protected internal void WriteC(bool value) => this.memory.WriteByte(Convert.ToByte(value));

    protected internal void WriteC(byte value) => this.memory.WriteByte(value);

    protected internal void WriteT(float value) => this.WriteB(BitConverter.GetBytes(value));

    protected internal void WriteHVector(Half3 half)
    {
      this.WriteH(half.X.RawValue);
      this.WriteH(half.Y.RawValue);
      this.WriteH(half.Z.RawValue);
    }

    protected internal void WriteTVector(Half3 half)
    {
      this.WriteT((float) half.X);
      this.WriteT((float) half.Y);
      this.WriteT((float) half.Z);
    }

    protected internal void GoBack(int value) => this.memory.Position -= (long) value;

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (this.disposed)
        return;
      this.memory.Dispose();
      if (disposing)
      {
        this.handle.Dispose();
        this.handle = (SafeHandle) null;
      }
      this.disposed = true;
    }
  }
}
