

using System.Numerics;
using System.Runtime.CompilerServices;

namespace michele.natale.Cryptography.Signatures.Services;


partial class SignatureServices
{

  //public static byte[] Xor(
  //  ReadOnlySpan<byte> left, ReadOnlySpan<byte> right)
  //{
  //  var length = Math.Max(left.Length, right.Length);

  //  var result = new byte[length];
  //  for (var i = 0; i < length; i++)
  //    result[i % result.Length] = 
  //      (byte)(left[i % left.Length] ^ 
  //        right[i % right.Length]);

  //  return result;
  //}

  public static byte[] Xor(
    ReadOnlySpan<byte> left, ReadOnlySpan<byte> right, int size)
  {
    var result = new byte[size];
    var length = Math.Max(left.Length, right.Length);
    for (var i = 0; i < length; i++)
      result[i % result.Length] =
        (byte)(left[i % left.Length] ^
          right[i % right.Length]);

    return result;
  }

  public static T[] XorSpec<T>(
    ReadOnlySpan<T> left, ReadOnlySpan<T> right, int size)
      where T : unmanaged, INumber<T>, IBitwiseOperators<T, T, T>
  {
    var result = new T[size];
    var sz = new int[] { size, left.Length, right.Length };
    var length = sz.Max();
    var cp = ToCoprimes(sz, sz.Min() / 3, 3);
    int offset1 = cp[0], offset2 = cp[1], offset3 = cp[2];
    for (var i = 0; i < length; i++)
      result[i * offset2 % result.Length] =
        left[i * offset1 % left.Length] ^
          right[i * offset3 % right.Length];

    return result;
  }

  internal static T[] ToEntropie<T>(T[] src, T[] seed, int size)
    where T : unmanaged, INumber<T>,
      IMinMaxValue<T>, IBitwiseOperators<T, T, T>
  {
    var sz = (SingleSignature.MIN_KEY_SIZE + SingleSignature.MAX_KEY_SIZE) >> 1;
    var xor = XorSpec<T>(src, seed, sz);
    var entro_basic = ToEntropieBasic(src, seed, sz);
    return XorSpec<T>(xor, entro_basic, size);
  }

  private static T[] ToEntropieBasic<T>(T[] src, T[] seed, int size)
    where T : unmanaged, INumber<T>, IMinMaxValue<T>
  {
    var tsz = Unsafe.SizeOf<T>();
    var s = new byte[src.Length * tsz];
    var ss = new byte[seed.Length * tsz];
    Buffer.BlockCopy(src, 0, s, 0, s.Length);
    Buffer.BlockCopy(seed, 0, ss, 0, ss.Length);

    var szs = tsz * src.Length;
    var szss = tsz * seed.Length;
    ulong sum1 = 0ul, sum2 = 0ul;
    for (var i = 0; i < szs; i++) sum1 += s[i];
    for (var i = 0; i < szss; i++) sum2 += ss[i];

    var result = new T[size];
    var sum = sum1 + sum2;
    ToNumbers(result);
    for (var i = 0; i < size - 1; i++)
    {
      var idx = sum;
      idx += ulong.CreateSaturating(src[i % src.Length]);
      idx += ulong.CreateSaturating(seed[i % seed.Length]);
      idx %= (ulong)size;
      (result[idx], result[i]) = (result[i], result[idx]);
    }

    return result;
  }

  private static void ToNumbers<T>(T[] numbers)
      where T : unmanaged, INumber<T>, IMinMaxValue<T>
  {
    var k = T.MaxValue;
    var length = numbers.Length;
    for (var i = 0; i < length; i++)
      numbers[i] = k--;
  }

  /// <summary>
  /// Clears the contents of all arrays.
  /// </summary>
  /// <param name="input"></param>
  public static void MemoryClear(params byte[][] input)
  {
    for (int i = 0; i < input.Length; i++)
      Array.Clear(input[i]);
  }
}
