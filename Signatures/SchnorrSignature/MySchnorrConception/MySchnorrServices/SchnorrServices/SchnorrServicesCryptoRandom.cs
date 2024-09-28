using System.Numerics;
using System.Runtime.InteropServices;
using System.Security.Cryptography;


namespace michele.natale.Schnorrs.Services;

partial class SchnorrServices
{

  private static readonly RandomNumberGenerator CRand =
    RandomNumberGenerator.Create();

  public static byte[] RngCryptoBytes(int size, bool no_zeros = true)
  {
    var result = new byte[size];
    if (no_zeros) CRand.GetBytes(result);
    else CRand.GetNonZeroBytes(result);
    return result;
  }

  public static void FillCryptoBytes(Span<byte> bytes, bool no_zeros = true)
  {
    if (no_zeros) CRand.GetBytes(bytes);
    else CRand.GetNonZeroBytes(bytes);
  }

  public static int NextCryptoInt32()
  {
    var result = new byte[4];
    CRand.GetBytes(result);
    return int.Abs(BitConverter.ToInt32(result));
  }

  public static int NextCryptoInt32(int max) =>
    NextCryptoInt32(0, max);

  public static int NextCryptoInt32(int min, int max)
  {
    ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(min, max);
    return RngCryptoIntX(1, min, max).First();
  }

  public static int[] RngCryptoInt32(int size)
  {
    return RngCryptoIntX(size, 0, int.MaxValue);
  }

  public static int[] RngCryptoInt32(int size, int max)
  {
    return RngCryptoIntX(size, 0, max);
  }

  public static int[] RngCryptoInt32(int size, int min, int max)
  {
    ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(min, max);
    return RngCryptoIntX(size, min, max);
  }

  public static long NextCryptoInt64()
  {
    var result = new byte[8];
    CRand.GetBytes(result);
    return long.Abs(BitConverter.ToInt64(result));
  }

  public static long NextCryptoInt64(long max) =>
    NextCryptoInt64(0, max);

  public static long NextCryptoInt64(long min, long max)
  {
    ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(min, max);
    return RngCryptoIntX(1, min, max).First();
  }

  public static long[] RngCryptoInt64(int size)
  {
    return RngCryptoIntX(size, 0L, long.MaxValue);
  }

  public static long[] RngCryptoInt64(int size, long max)
  {
    return RngCryptoIntX(size, 0L, max);
  }

  public static long[] RngCryptoInt64(int size, long min, long max)
  {
    ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(min, max);
    return RngCryptoIntX(size, min, max);
  }


  private static T[] RngCryptoIntX<T>(int size, T min, T max) where T : INumber<T>, IMinMaxValue<T>
  {
    var result = new T[size];
    var tsize = Marshal.SizeOf(typeof(T));
    var buffer = new byte[size * tsize];
    var maxmin = double.CreateSaturating(max - min);
    var denominator = double.CreateSaturating(T.MaxValue) + 1.0;
    CRand.GetNonZeroBytes(buffer);
    for (var i = 0; i < result.Length; i++)
    {
      Buffer.BlockCopy(buffer, i * tsize, result, i, tsize);
      var tmp = double.Abs(double.CreateSaturating(result[i]) / denominator);
      tmp *= maxmin;
      tmp += double.CreateSaturating(min);
      result[i] = T.Abs(T.CreateSaturating(tmp));
    }
    return result;
  }


}
