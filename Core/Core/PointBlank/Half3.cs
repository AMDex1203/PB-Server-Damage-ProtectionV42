
// Type: PointBlank.Half3
// Assembly: Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 263E2FBF-1098-4552-8FB3-F6A660503737
// Interprise: C:\Users\Cuzin\3,50Core.dll

namespace PointBlank
{
  public struct Half3
  {
    public Half X;
    public Half Y;
    public Half Z;

    public Half3(float x, float y, float z)
    {
      this.X = new Half(x);
      this.Y = new Half(y);
      this.Z = new Half(z);
    }

    public Half3(ushort x, ushort y, ushort z)
    {
      this.X = new Half(x);
      this.Y = new Half(y);
      this.Z = new Half(z);
    }

    public static implicit operator Half3(Vector3 value) => new Half3(value.X, value.Y, value.Z);

    public static implicit operator Vector3(Half3 value) => new Vector3((float) value.X, (float) value.Y, (float) value.Z);
  }
}
