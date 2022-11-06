
// Type: Game.data.BattlePacketWriter
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Microsoft.Win32.SafeHandles;
using PointBlank;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Game.data
{
  public class BattlePacketWriter : IDisposable
  {
    public MemoryStream memory = new MemoryStream();
    private bool disposed;
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
