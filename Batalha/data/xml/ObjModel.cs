
// Type: Battle.data.xml.ObjModel
// Assembly: pbserver_battle, Version=1.0.0.2646, Culture=neutral, PublicKeyToken=null
// MVID: AA2D7911-B687-4238-AB73-D37E54123039
// Interprise: C:\Users\Cuzin\3,50pbserver_battle.exe

using Battle.data.models;
using System;
using System.Collections.Generic;

namespace Battle.data.xml
{
  public class ObjModel
  {
    public int _id;
    public int _life;
    public int _anim1;
    public int _ultraSYNC;
    public int _updateId = 1;
    public bool _needSync;
    public bool isDestroyable;
    public bool _noInstaSync;
    public List<AnimModel> _anims;
    public List<DEffectModel> _effects;

    public ObjModel(bool needSYNC)
    {
      this._needSync = needSYNC;
      if (needSYNC)
        this._anims = new List<AnimModel>();
      this._effects = new List<DEffectModel>();
    }

    public int CheckDestroyState(int life)
    {
      for (int index = this._effects.Count - 1; index > -1; --index)
      {
        DEffectModel effect = this._effects[index];
        if (effect._life >= life)
          return effect._id;
      }
      return 0;
    }

    public int GetARandomAnim(Room room, ObjectInfo obj)
    {
      if (this._anims != null && this._anims.Count > 0)
      {
        AnimModel anim = this._anims[new Random().Next(this._anims.Count)];
        obj._anim = anim;
        obj._useDate = DateTime.Now;
        if (anim._otherObj > 0)
        {
          ObjectInfo objectInfo = room._objects[anim._otherObj];
          this.GetAnim(anim._otherAnim, 0.0f, 0.0f, objectInfo);
        }
        return anim._id;
      }
      obj._anim = (AnimModel) null;
      return (int) byte.MaxValue;
    }

    public void GetAnim(int animId, float time, float duration, ObjectInfo obj)
    {
      if (animId == (int) byte.MaxValue || obj == null || (obj._model == null || obj._model._anims == null) || obj._model._anims.Count == 0)
        return;
      ObjModel model = obj._model;
      for (int index = 0; index < model._anims.Count; ++index)
      {
        AnimModel anim = model._anims[index];
        if (anim._id == animId)
        {
          obj._anim = anim;
          time -= duration;
          obj._useDate = DateTime.Now.AddSeconds((double) time * -1.0);
          break;
        }
      }
    }
  }
}
