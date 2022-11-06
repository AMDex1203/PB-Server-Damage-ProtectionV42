
// Type: Core.models.room.Frag
// Assembly: Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 263E2FBF-1098-4552-8FB3-F6A660503737
// Interprise: C:\Users\Cuzin\3,50Core.dll

using Core.models.enums.room;

namespace Core.models.room
{
  public class Frag
  {
    public byte victimWeaponClass;
    public byte hitspotInfo;
    public byte flag;
    public KillingMessage killFlag;
    public float x;
    public float y;
    public float z;
    public int VictimSlot;

    public Frag()
    {
    }

    public Frag(byte hitspotInfo) => this.SetHitspotInfo(hitspotInfo);

    public void SetHitspotInfo(byte value)
    {
      this.hitspotInfo = value;
      this.VictimSlot = (int) value & 15;
    }
  }
}
