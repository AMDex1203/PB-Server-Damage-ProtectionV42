
// Type: PointBlank.HalfUtils
// Assembly: Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 263E2FBF-1098-4552-8FB3-F6A660503737
// Interprise: C:\Users\Cuzin\3,50Core.dll

using System.Runtime.InteropServices;

namespace PointBlank
{
  public class HalfUtils
  {
    private static readonly uint[] HalfToFloatMantissaTable = new uint[2048];
    private static readonly uint[] HalfToFloatExponentTable = new uint[64];
    private static readonly uint[] HalfToFloatOffsetTable = new uint[64];
    private static readonly ushort[] FloatToHalfBaseTable = new ushort[512];
    private static readonly byte[] FloatToHalfShiftTable = new byte[512];

    public static void Load()
    {
      HalfUtils.HalfToFloatMantissaTable[0] = 0U;
      for (int index = 1; index < 1024; ++index)
      {
        uint num1 = (uint) (index << 13);
        uint num2 = 0;
        for (; ((int) num1 & 8388608) == 0; num1 <<= 1)
          num2 -= 8388608U;
        uint num3 = num1 & 8388609U;
        uint num4 = num2 + 947912704U;
        HalfUtils.HalfToFloatMantissaTable[index] = num3 | num4;
      }
      for (int index = 1024; index < 2048; ++index)
        HalfUtils.HalfToFloatMantissaTable[index] = (uint) (939524096 + (index - 1024 << 13));
      HalfUtils.HalfToFloatExponentTable[0] = 0U;
      for (int index = 1; index < 63; ++index)
        HalfUtils.HalfToFloatExponentTable[index] = index < 31 ? (uint) (index << 23) : (uint) (int.MinValue + (index - 32 << 23));
      HalfUtils.HalfToFloatExponentTable[31] = 1199570944U;
      HalfUtils.HalfToFloatExponentTable[32] = 2147483648U;
      HalfUtils.HalfToFloatExponentTable[63] = 947912704U;
      HalfUtils.HalfToFloatOffsetTable[0] = 0U;
      for (int index = 1; index < 64; ++index)
        HalfUtils.HalfToFloatOffsetTable[index] = 1024U;
      HalfUtils.HalfToFloatOffsetTable[32] = 0U;
      for (int index = 0; index < 256; ++index)
      {
        int num = index - (int) sbyte.MaxValue;
        if (num < -24)
        {
          HalfUtils.FloatToHalfBaseTable[index | 0] = (ushort) 0;
          HalfUtils.FloatToHalfBaseTable[index | 256] = (ushort) 32768;
          HalfUtils.FloatToHalfShiftTable[index | 0] = (byte) 24;
          HalfUtils.FloatToHalfShiftTable[index | 256] = (byte) 24;
        }
        else if (num < -14)
        {
          HalfUtils.FloatToHalfBaseTable[index | 0] = (ushort) (1024 >> -num - 14);
          HalfUtils.FloatToHalfBaseTable[index | 256] = (ushort) (1024 >> -num - 14 | 32768);
          HalfUtils.FloatToHalfShiftTable[index | 0] = (byte) (-num - 1);
          HalfUtils.FloatToHalfShiftTable[index | 256] = (byte) (-num - 1);
        }
        else if (num <= 15)
        {
          HalfUtils.FloatToHalfBaseTable[index | 0] = (ushort) (num + 15 << 10);
          HalfUtils.FloatToHalfBaseTable[index | 256] = (ushort) (num + 15 << 10 | 32768);
          HalfUtils.FloatToHalfShiftTable[index | 0] = (byte) 13;
          HalfUtils.FloatToHalfShiftTable[index | 256] = (byte) 13;
        }
        else if (num >= 128)
        {
          HalfUtils.FloatToHalfBaseTable[index | 0] = (ushort) 31744;
          HalfUtils.FloatToHalfBaseTable[index | 256] = (ushort) 64512;
          HalfUtils.FloatToHalfShiftTable[index | 0] = (byte) 13;
          HalfUtils.FloatToHalfShiftTable[index | 256] = (byte) 13;
        }
        else
        {
          HalfUtils.FloatToHalfBaseTable[index | 0] = (ushort) 31744;
          HalfUtils.FloatToHalfBaseTable[index | 256] = (ushort) 64512;
          HalfUtils.FloatToHalfShiftTable[index | 0] = (byte) 24;
          HalfUtils.FloatToHalfShiftTable[index | 256] = (byte) 24;
        }
      }
    }

    public static ushort Pack(float f)
    {
      HalfUtils.FloatToUint floatToUint = new HalfUtils.FloatToUint()
      {
        floatValue = f
      };
      return (ushort) ((uint) HalfUtils.FloatToHalfBaseTable[(int) (floatToUint.uintValue >> 23) & 511] + ((floatToUint.uintValue & 8388607U) >> (int) HalfUtils.FloatToHalfShiftTable[(int) (floatToUint.uintValue >> 23) & 511]));
    }

    public static float Unpack(ushort h) => new HalfUtils.FloatToUint()
    {
      uintValue = (HalfUtils.HalfToFloatMantissaTable[(long) HalfUtils.HalfToFloatOffsetTable[(int) h >> 10] + (long) ((int) h & 1023)] + HalfUtils.HalfToFloatExponentTable[(int) h >> 10])
    }.floatValue;

    [StructLayout(LayoutKind.Explicit)]
    private struct FloatToUint
    {
      [FieldOffset(0)]
      public uint uintValue;
      [FieldOffset(0)]
      public float floatValue;
    }
  }
}
