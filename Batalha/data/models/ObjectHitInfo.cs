
// Type: Battle.data.models.ObjectHitInfo
// Assembly: pbserver_battle, Version=1.0.0.2646, Culture=neutral, PublicKeyToken=null
// MVID: AA2D7911-B687-4238-AB73-D37E54123039
// Interprise: C:\Users\Cuzin\3,50pbserver_battle.exe

using Battle.data.enums;
using SharpDX;

namespace Battle.data.models
{
  public class ObjectHitInfo
  {
    public int syncType;
    public int objSyncId;
    public int objId;
    public int objLife;
    public int weaponId;
    public int killerId;
    public int _animId1;
    public int _animId2;
    public int _destroyState;
    public CHARA_DEATH deathType = CHARA_DEATH.DEFAULT;
    public int hitPart;
    public Half3 Position;
    public float _specialUse;

    public ObjectHitInfo(int type) => this.syncType = type;
  }
}
