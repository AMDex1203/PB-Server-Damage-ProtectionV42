
// Type: Battle.data.AllUtils
// Assembly: pbserver_battle, Version=1.0.0.2646, Culture=neutral, PublicKeyToken=null
// MVID: AA2D7911-B687-4238-AB73-D37E54123039
// Interprise: C:\Users\Cuzin\3,50pbserver_battle.exe

using Battle.data.enums;
using Battle.data.enums.weapon;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Battle.data
{
  public class AllUtils
  {
    public static string gen5(string text)
    {
      using (HMACMD5 hmacmD5 = new HMACMD5(Encoding.UTF8.GetBytes("/x!a@r-$r%an¨.&e&+f*f(f(a)")))
      {
        byte[] hash = hmacmD5.ComputeHash(Encoding.UTF8.GetBytes(text));
        StringBuilder stringBuilder = new StringBuilder();
        for (int index = 0; index < hash.Length; ++index)
          stringBuilder.Append(hash[index].ToString("x2"));
        return stringBuilder.ToString();
      }
    }

    public static float GetDuration(DateTime date) => (float) (DateTime.Now - date).TotalSeconds;

    public static int createItemId(int class1, int usage, int classtype, int number) => class1 * 100000000 + usage * 1000000 + classtype * 1000 + number;

    public static int itemClass(ClassType cw)
    {
      int num1 = 1;
      int num2;
      switch (cw)
      {
        case ClassType.Assault:
          num1 = 1;
          goto label_24;
        case ClassType.SMG:
          num2 = 1;
          break;
        default:
          num2 = cw == ClassType.DualSMG ? 1 : 0;
          break;
      }
      if (num2 != 0)
      {
        num1 = 2;
      }
      else
      {
        int num3;
        switch (cw)
        {
          case ClassType.Sniper:
            num1 = 3;
            goto label_24;
          case ClassType.Shotgun:
            num3 = 1;
            break;
          default:
            num3 = cw == ClassType.DualShotgun ? 1 : 0;
            break;
        }
        if (num3 != 0)
        {
          num1 = 4;
        }
        else
        {
          int num4;
          switch (cw)
          {
            case ClassType.HandGun:
            case ClassType.DualHandGun:
              num4 = 1;
              break;
            case ClassType.MG:
              num1 = 5;
              goto label_24;
            default:
              num4 = cw == ClassType.CIC ? 1 : 0;
              break;
          }
          if (num4 != 0)
            num1 = 6;
          else if (cw == ClassType.Knife || cw == ClassType.DualKnife || cw == ClassType.Knuckle)
          {
            num1 = 7;
          }
          else
          {
            switch (cw)
            {
              case ClassType.Throwing:
                num1 = 8;
                break;
              case ClassType.Item:
                num1 = 9;
                break;
              case ClassType.Dino:
                num1 = 0;
                break;
            }
          }
        }
      }
label_24:
      return num1;
    }

    public static ObjectType getHitType(uint info) => (ObjectType) ((int) info & 3);

    public static int getHitWho(uint info) => (int) (info >> 2) & 511;

    public static int getHitPart(uint info) => (int) (info >> 11) & 63;

    public static ushort getHitDamageBOT(uint info) => (ushort) (info >> 20);

    public static ushort getHitDamageNORMAL(uint info) => (ushort) (info >> 21);

    public static int getHitHelmet(uint info) => (int) (info >> 17) & 7;

    public static int GetRoomInfo(uint UniqueRoomId, int type)
    {
      switch (type)
      {
        case 0:
          return (int) UniqueRoomId & 4095;
        case 1:
          return (int) (UniqueRoomId >> 12) & (int) byte.MaxValue;
        case 2:
          return (int) (UniqueRoomId >> 20) & 4095;
        default:
          return 0;
      }
    }

    public static byte[] encrypt(byte[] data, int shift)
    {
      byte[] numArray = new byte[data.Length];
      Buffer.BlockCopy((Array) data, 0, (Array) numArray, 0, numArray.Length);
      int length = numArray.Length;
      byte num1 = numArray[0];
      for (int index = 0; index < length; ++index)
      {
        byte num2 = index >= length - 1 ? num1 : numArray[index + 1];
        numArray[index] = (byte) ((int) num2 >> 8 - shift | (int) numArray[index] << shift);
      }
      return numArray;
    }

    public static byte[] decrypt(byte[] data, int shift)
    {
      try
      {
        byte[] numArray = new byte[data.Length];
        Buffer.BlockCopy((Array) data, 0, (Array) numArray, 0, numArray.Length);
        int length = numArray.Length;
        byte num1 = numArray[length - 1];
        for (int index = length - 1; ((long) index & 2147483648L) == 0L; --index)
        {
          byte num2 = index <= 0 ? num1 : numArray[index - 1];
          numArray[index] = (byte) ((int) num2 << 8 - shift | (int) numArray[index] >> shift);
        }
        return numArray;
      }
      catch
      {
        Logger.warning(BitConverter.ToString(data));
        return new byte[0];
      }
    }

    public static int percentage(int total, int percent) => total * percent / 100;

    public static float percentage(float total, int percent) => (float) ((double) total * (double) percent / 100.0);
  }
}
