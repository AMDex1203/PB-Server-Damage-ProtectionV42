
// Type: Battle.network.actions.user.a80000_SufferingDamage
// Assembly: pbserver_battle, Version=1.0.0.2646, Culture=neutral, PublicKeyToken=null
// MVID: AA2D7911-B687-4238-AB73-D37E54123039
// Interprise: C:\Users\Cuzin\3,50pbserver_battle.exe

using Battle.data.models;

namespace Battle.network.actions.user
{
  public class a80000_SufferingDamage
  {
    public static a80000_SufferingDamage.Struct ReadInfo(
      ActionModel ac,
      ReceivePacket p,
      bool genLog)
    {
      a80000_SufferingDamage.Struct @struct = new a80000_SufferingDamage.Struct()
      {
        _hitEffects = p.readC(),
        _hitPart = p.readC()
      };
      if (genLog)
      {
        Logger.warning("[1] Effect: " + ((int) @struct._hitEffects >> 4).ToString() + "; By slot: " + ((int) @struct._hitEffects & 15).ToString());
        Logger.warning("[2] Slot " + ac._slot.ToString() + " action 524288: " + @struct._hitEffects.ToString() + ";" + @struct._hitPart.ToString());
      }
      return @struct;
    }

    public static void ReadInfo(ReceivePacket p) => p.Advance(2);

    public static void writeInfo(SendPacket s, ActionModel ac, ReceivePacket p, bool genLog)
    {
      a80000_SufferingDamage.Struct info = a80000_SufferingDamage.ReadInfo(ac, p, genLog);
      a80000_SufferingDamage.writeInfo(s, info);
    }

    public static void writeInfo(SendPacket s, a80000_SufferingDamage.Struct info)
    {
      s.writeC(info._hitEffects);
      s.writeC(info._hitPart);
    }

    public class Struct
    {
      public byte _hitEffects;
      public byte _hitPart;
    }
  }
}
