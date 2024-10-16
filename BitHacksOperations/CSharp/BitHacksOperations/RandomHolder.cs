

using System.Numerics;
using System.Security.Cryptography; 
using System.Runtime.CompilerServices;

namespace michele.natale.Randoms;

public class RandomHolder
{

  public static readonly RandomNumberGenerator Rand =
    RandomNumberGenerator.Create();


  public static byte[] RngBytes(
    int size, bool no_zeros = true)
  {
    var bytes = new byte[size];
    if (no_zeros)
      Rand.GetNonZeroBytes(bytes);
    else Rand.GetBytes(bytes);
    return bytes;
  }

  public static T NextInt<T>()
    where T : INumber<T> =>
      RngInts<T>(1).First();

  public static T NextInt<T>(T max)
    where T : INumber<T>
  {
    var result = new T[1];
    RngInts(result, T.Zero, max);
    return result.First();
  }

  public static T NextInt<T>(T min, T max)
    where T : INumber<T>
  {
    if (min == max) return min;
    var result = new T[1];
    RngInts(result, min, max);
    return result.First();
  }

  public static T[] RngInts<T>(int size)
    where T : INumber<T>
  {
    var int_size = size * Unsafe.SizeOf<T>();

    var result = new T[size];
    var bytes = new byte[int_size];

    Rand.GetNonZeroBytes(bytes);
    Buffer.BlockCopy(bytes, 0, result, 0, bytes.Length);

    return result;
  }

  private static void RngInts<T>(T[] ints, T min, T max)
      where T : INumber<T>, INumberBase<T>
  {
    if (IsNullOrEmpty(ints)) return;
    ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(min, max);

    var d = max - min;
    var length = ints.Length;
    var type_bits = Unsafe.SizeOf<T>();
    var bytes = new byte[type_bits * length];
    Rand.GetNonZeroBytes(bytes);
    //DataTypes Int128 and UInt128 are not yet recognized as primitives. 
    if (typeof(T).IsPrimitive || typeof(T) == typeof(Int128) || typeof(T) == typeof(UInt128))
      for (int i = 0; i < length; i++)
      {
        var tmp = Unsafe.ReadUnaligned<T>(ref bytes[i * type_bits]);
        ints[i] = T.Abs(tmp); ints[i] %= d; ints[i] += min;
      }
  }

  public static void FillInts<T>(T[] input)
    where T : INumber<T>
  {
    var int_size = input.Length * Unsafe.SizeOf<T>();

    var bytes = new byte[int_size];

    Rand.GetNonZeroBytes(bytes);
    Buffer.BlockCopy(bytes, 0, input, 0, bytes.Length);
  }

  private static bool IsNullOrEmpty<T>(T[] ints)
     where T : INumber<T>, INumberBase<T>
  {
    if (ints is null) return true;
    return ints.Length == 0;
  }
}

