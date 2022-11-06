
// Type: PointBlank.Half
// Assembly: Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 263E2FBF-1098-4552-8FB3-F6A660503737
// Interprise: C:\Users\Cuzin\3,50Core.dll

namespace PointBlank
{
  public struct Half
  {
    public ushort RawValue;

    public Half(float value) => this.RawValue = HalfUtils.Pack(value);

    public Half(ushort rawvalue) => this.RawValue = rawvalue;

    public static implicit operator float(Half value) => HalfUtils.Unpack(value.RawValue);
  }
}
