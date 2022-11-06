﻿
// Type: Auth.global.ReceiveLoginPacket
// Assembly: pbserver_auth, Version=1.0.7796.38604, Culture=neutral, PublicKeyToken=null
// MVID: 1CFDFF97-782C-4EF8-A621-6E4FDDE32436
// Interprise: C:\Users\Cuzin\3,50pbserver_auth.exe

using Core;
using System;
using System.Text;

namespace Auth.global
{
  public abstract class ReceiveLoginPacket
  {
    public byte[] _buffer;
    private int _offset = 4;
    public LoginClient _client;

    protected internal void makeme(LoginClient client, byte[] buffer)
    {
      this._client = client;
      this._buffer = buffer;
      this.read();
    }

    protected internal int readD()
    {
      int int32 = BitConverter.ToInt32(this._buffer, this._offset);
      this._offset += 4;
      return int32;
    }

    protected internal byte readC()
    {
      byte num = this._buffer[this._offset];
      ++this._offset;
      return num;
    }

    protected internal byte[] readB(int Length)
    {
      byte[] numArray = new byte[Length];
      Array.Copy((Array) this._buffer, this._offset, (Array) numArray, 0, Length);
      this._offset += Length;
      return numArray;
    }

    protected internal short readH()
    {
      short int16 = BitConverter.ToInt16(this._buffer, this._offset);
      this._offset += 2;
      return int16;
    }

    protected internal double readF()
    {
      double num = BitConverter.ToDouble(this._buffer, this._offset);
      this._offset += 8;
      return num;
    }

    protected internal long readQ()
    {
      long int64 = BitConverter.ToInt64(this._buffer, this._offset);
      this._offset += 8;
      return int64;
    }

    protected internal ulong readUQ()
    {
      ulong uint64 = BitConverter.ToUInt64(this._buffer, this._offset);
      this._offset += 8;
      return uint64;
    }

    protected internal string readS(int Length)
    {
      string str = "";
      try
      {
        str = ConfigGB.EncodeText.GetString(this._buffer, this._offset, Length);
        int length = str.IndexOf(char.MinValue);
        if (length != -1)
          str = str.Substring(0, length);
        this._offset += Length;
      }
      catch
      {
      }
      return str;
    }

    protected internal string readS(int Length, int CodePage)
    {
      string str = "";
      try
      {
        str = Encoding.GetEncoding(CodePage).GetString(this._buffer, this._offset, Length);
        int length = str.IndexOf(char.MinValue);
        if (length != -1)
          str = str.Substring(0, length);
        this._offset += Length;
      }
      catch
      {
      }
      return str;
    }

    protected internal string readS()
    {
      string str = "";
      try
      {
        str = Encoding.Unicode.GetString(this._buffer, this._offset, this._buffer.Length - this._offset);
        int length = str.IndexOf(char.MinValue);
        if (length != -1)
          str = str.Substring(0, length);
        this._offset += str.Length + 1;
      }
      catch
      {
      }
      return str;
    }

    public abstract void read();

    public abstract void run();
  }
}
