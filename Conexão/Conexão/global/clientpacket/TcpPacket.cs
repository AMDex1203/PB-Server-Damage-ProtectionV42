
// Type: Auth.global.clientpacket.TcpPacket
// Assembly: pbserver_auth, Version=1.0.7796.38604, Culture=neutral, PublicKeyToken=null
// MVID: 1CFDFF97-782C-4EF8-A621-6E4FDDE32436
// Interprise: C:\Users\Cuzin\3,50pbserver_auth.exe

using System.Net.Sockets;

namespace Auth.global.clientpacket
{
  public class TcpPacket
  {
    public const int BufferSize = 8976;
    public byte[] Buffer = new byte[8976];
    public Socket WorkSocket;
    public int Length;
    public ushort Opcode;
  }
}
