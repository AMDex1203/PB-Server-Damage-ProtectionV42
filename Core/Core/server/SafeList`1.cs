
// Type: Core.server.SafeList`1
// Assembly: Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 263E2FBF-1098-4552-8FB3-F6A660503737
// Interprise: C:\Users\Cuzin\3,50Core.dll

using System.Collections.Generic;

namespace Core.server
{
  public class SafeList<T>
  {
    private List<T> _list = new List<T>();
    private object _sync = new object();

    public void Add(T value)
    {
      lock (this._sync)
        this._list.Add(value);
    }

    public void Clear()
    {
      lock (this._sync)
        this._list.Clear();
    }

    public bool Contains(T value)
    {
      lock (this._sync)
        return this._list.Contains(value);
    }

    public int Count()
    {
      lock (this._sync)
        return this._list.Count;
    }

    public bool Remove(T value)
    {
      lock (this._sync)
        return this._list.Remove(value);
    }
  }
}
