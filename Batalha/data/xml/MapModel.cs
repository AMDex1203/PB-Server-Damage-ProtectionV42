
// Type: Battle.data.xml.MapModel
// Assembly: pbserver_battle, Version=1.0.0.2646, Culture=neutral, PublicKeyToken=null
// MVID: AA2D7911-B687-4238-AB73-D37E54123039
// Interprise: C:\Users\Cuzin\3,50pbserver_battle.exe

using System.Collections.Generic;

namespace Battle.data.xml
{
  public class MapModel
  {
    public int _id;
    public List<ObjModel> _objects = new List<ObjModel>();
    public List<BombPosition> _bombs = new List<BombPosition>();

    public BombPosition GetBomb(int bombId)
    {
      try
      {
        return this._bombs[bombId];
      }
      catch
      {
        return (BombPosition) null;
      }
    }
  }
}
