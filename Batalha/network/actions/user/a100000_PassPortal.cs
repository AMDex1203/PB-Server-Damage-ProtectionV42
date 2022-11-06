
// Type: Battle.network.actions.user.a100000_PassPortal
// Assembly: pbserver_battle, Version=1.0.0.2646, Culture=neutral, PublicKeyToken=null
// MVID: AA2D7911-B687-4238-AB73-D37E54123039
// Interprise: C:\Users\Cuzin\3,50pbserver_battle.exe

using Battle.data.models;
using Battle.data.sync;

namespace Battle.network.actions.user
{
  public class a100000_PassPortal
  {
    public static a100000_PassPortal.Struct ReadInfo(
      ActionModel ac,
      ReceivePacket p,
      bool genLog)
    {
      a100000_PassPortal.Struct @struct = new a100000_PassPortal.Struct()
      {
        _portal = p.readUH()
      };
      if (genLog)
        Logger.warning("Slot " + ac._slot.ToString() + " passed on the portal [" + @struct._portal.ToString() + "]");
      return @struct;
    }

    public static void ReadInfo(ReceivePacket p) => p.Advance(2);

    public static void SendPassSync(Room room, Player p, a100000_PassPortal.Struct info) => Battle_SyncNet.SendPortalPass(room, p, (int) info._portal);

    public static void writeInfo(SendPacket s, ActionModel ac, ReceivePacket p, bool genLog)
    {
      a100000_PassPortal.Struct info = a100000_PassPortal.ReadInfo(ac, p, genLog);
      a100000_PassPortal.writeInfo(s, info);
    }

    public static void writeInfo(SendPacket s, a100000_PassPortal.Struct info) => s.writeH(info._portal);

    public class Struct
    {
      public ushort _portal;
    }
  }
}
