
// Type: Battle.data.models.ObjectInfo
// Assembly: pbserver_battle, Version=1.0.0.2646, Culture=neutral, PublicKeyToken=null
// MVID: AA2D7911-B687-4238-AB73-D37E54123039
// Interprise: C:\Users\Cuzin\3,50pbserver_battle.exe

using Battle.data.xml;
using System;

namespace Battle.data.models
{
  public class ObjectInfo
  {
    public int _id;
    public int _life = 100;
    public int DestroyState;
    public AnimModel _anim;
    public DateTime _useDate;
    public ObjModel _model;

    public ObjectInfo()
    {
    }

    public ObjectInfo(int id) => this._id = id;
  }
}
