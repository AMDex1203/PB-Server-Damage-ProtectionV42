﻿
// Type: Battle.network.SendPacket
// Assembly: pbserver_battle, Version=1.0.0.2646, Culture=neutral, PublicKeyToken=null
// MVID: AA2D7911-B687-4238-AB73-D37E54123039
// Interprise: C:\Users\Cuzin\3,50pbserver_battle.exe

using Microsoft.Win32.SafeHandles;
using SharpDX;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Battle.network
{
  public class SendPacket : IDisposable
  {
    public MemoryStream mstream = new MemoryStream();
    private bool disposed = false;
    private SafeHandle handle = (SafeHandle) new SafeFileHandle(IntPtr.Zero, true);

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (this.disposed)
        return;
      this.mstream.Dispose();
      if (disposing)
        this.handle.Dispose();
      this.disposed = true;
    }

    public byte[] GetArray() => this.mstream == null ? new byte[0] : this.mstream.ToArray();

    protected internal void writeB(byte[] value) => this.mstream.Write(value, 0, value.Length);

    protected internal void writeD(int value) => this.writeB(BitConverter.GetBytes(value));

    protected internal void writeD(uint value) => this.writeB(BitConverter.GetBytes(value));

    protected internal void writeH(short val) => this.writeB(BitConverter.GetBytes(val));

    protected internal void writeH(ushort val) => this.writeB(BitConverter.GetBytes(val));

    protected internal void writeC(bool value) => this.mstream.WriteByte(Convert.ToByte(value));

    protected internal void writeC(byte value) => this.mstream.WriteByte(value);

    protected internal void writeF(double value) => this.writeB(BitConverter.GetBytes(value));

    protected internal void writeT(float value) => this.writeB(BitConverter.GetBytes(value));

    protected internal void writeQ(long value) => this.writeB(BitConverter.GetBytes(value));

    protected internal void writeHVector(Half3 half)
    {
      this.writeH(half.X.RawValue);
      this.writeH(half.Y.RawValue);
      this.writeH(half.Z.RawValue);
    }

    protected internal void writeTVector(Half3 half)
    {
      this.writeT((float) half.X);
      this.writeT((float) half.Y);
      this.writeT((float) half.Z);
    }

    protected internal void writeS(string value)
    {
      if (value != null)
        this.writeB(Encoding.Unicode.GetBytes(value));
      this.writeH((short) 0);
    }

    protected internal void writeS(string name, int count)
    {
      if (name == null)
        return;
      this.writeB(Encoding.GetEncoding(1251).GetBytes(name));
      this.writeB(new byte[count - name.Length]);
    }

    protected internal void GoBack(int value) => this.mstream.Position -= (long) value;
  }
}
