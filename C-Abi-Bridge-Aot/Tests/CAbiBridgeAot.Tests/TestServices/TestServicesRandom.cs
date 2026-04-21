

using System.Numerics;
using System.Runtime.CompilerServices;


namespace michele.natale.Tests;

partial class TestServices
{

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static byte[] RngBaseXNumber(int size, int basex) =>
    RngInts<byte>(size, 0, (byte)basex);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static byte[] RngBytes(
    int size, bool non_zeros = true)
  {
    var rand = Random.Shared;
    var result = new byte[size];

    if (!non_zeros)
    {
      rand.NextBytes(result);
      return result;
    }

    for (var i = 0; i < size; i++)
      result[i] = (byte)rand.Next(1, 256);

    return result;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static T RngInt<T>()
    where T : INumber<T>, INumberBase<T>, IMinMaxValue<T>
  {
    var result = new T[1];
    RngInts(result, T.Zero, T.MaxValue);
    return result.First();
  }


  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static T RngInt<T>(T min, T max)
    where T : INumber<T>, INumberBase<T>
  {
    var result = new T[1];
    RngInts(result, min, max);
    return result.First();
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static T[] RngInts<T>(int size, T min, T max)
    where T : INumber<T>, INumberBase<T>
  {
    if (size == 0) return [];
    var result = new T[size];
    RngInts(result, min, max);
    return result;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private static void RngInts<T>(T[] ints, T min, T max)
    where T : INumber<T>, INumberBase<T>
  {
    if (IsNullOrEmpty(ints)) return;
    ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(min, max);

    var d = max - min;
    var length = ints.Length;
    var type_bits = Unsafe.SizeOf<T>();
    var bytes = RngBytes(type_bits * length, true);
    //DataTypes Int128 and UInt128 are not yet recognized as primitives. 
    if (typeof(T).IsPrimitive || typeof(T) == typeof(Int128) || typeof(T) == typeof(UInt128))
      for (int i = 0; i < length; i++)
      {
        var tmp = Unsafe.ReadUnaligned<T>(ref bytes[i * type_bits]);
        ints[i] = T.Abs(tmp); ints[i] %= d; ints[i] += min;
      }
  }
}
