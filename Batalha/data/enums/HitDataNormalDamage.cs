
// Type: Battle.data.enums.HitDataNormalDamage
// Assembly: pbserver_battle, Version=1.0.0.2646, Culture=neutral, PublicKeyToken=null
// MVID: AA2D7911-B687-4238-AB73-D37E54123039
// Interprise: C:\Users\Cuzin\3,50pbserver_battle.exe

using Battle.data.enums.weapon;
using SharpDX;
using System.Collections.Generic;

namespace Battle.data.enums
{
  public class HitDataNormalDamage
  {
    public byte WeaponSlot;
    public ushort BoomInfo;
    public ushort WeaponInfo;
    public int HitInfo;
    public int WeaponId;
    public int WeaponDamage;
    public int WeaponObjectId;
    public int HitPart;
    public ObjectType ObjectType;
    public CharaDeathEnum DeathType;
    public Half3 StartBullet;
    public Half3 EndBullet;
    public List<int> BoomPlayers;
    public HitType HitType;
    public ClassType WeaponClass;
    public HitCharaPart2Enum CharaHitPart;
    public float Range;

    public void SetData()
    {
      this.HitType = (HitType) (this.HitInfo >> 17 & 7);
      this.WeaponClass = (ClassType) ((int) this.WeaponInfo & 63);
      this.ObjectType = (ObjectType) (this.HitInfo & 3);
      this.WeaponId = (int) this.WeaponInfo >> 6;
      this.WeaponDamage = this.HitInfo >> 21;
      this.WeaponObjectId = this.HitInfo >> 2 & 511;
      this.HitPart = this.HitInfo >> 11 & 63;
      this.CharaHitPart = (HitCharaPart2Enum) this.HitPart;
      this.DeathType = this.HitPart == 29 ? CharaDeathEnum.HEADSHOT : CharaDeathEnum.DEFAULT;
      this.Range = Vector3.Distance((Vector3) this.StartBullet, (Vector3) this.EndBullet);
    }
  }
}
