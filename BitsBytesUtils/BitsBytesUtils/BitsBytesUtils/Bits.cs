
//At the moment (.Net8.0) the GetBitLength-Method of
//BigInteger does not work correctly.

 
using System.Numerics;
using System.Runtime.CompilerServices;

namespace michele.natale.BitsBytesUtils;


partial class BitsBytesUtils
{ 

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool[] ToBits<T>(
    T number, bool tobigendian = false)
    where T : INumber<T>, IBinaryInteger<T>
  {
    if (T.IsZero(number)) return [false];
    if (number == T.One) return [true, false];

    int length;
    var blength = length = BitLength(number);

    var result = new bool[blength];
    for (var i = 0; i < length; i++)
      if ((number & (T.One << i)) != T.Zero)
        result[i] = true;

    if (T.IsPositive(number) && result[^1] != false)
      result = [.. result, false];

    var start = 0;
    while (result[^(1 + start++)] == false) ; start--;

    if (!tobigendian && T.IsPositive(number))
      return result.Take(result.Length - start + 1).ToArray();

    if (!tobigendian) return result;

    return result.Reverse().ToArray();
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool[] ToBits(
    BigInteger number, bool tobigendian = false) =>
      !tobigendian ? ToBitStr(number).Reverse().Select(x => x == '1').ToArray()
      : ToBitStr(number).Select(x => x == '1').ToArray();


  public static T FromBits<T>(
    ReadOnlySpan<bool> bits, bool isbigendian = false)
      where T : INumber<T>, IBinaryInteger<T>
  {
    if (bits.Length == 1 && bits[0] == true) return -T.One;
    if (bits.Length == 1 && bits[0] == false) return T.Zero;

    var idx = !isbigendian ? bits.Length - 1 : 0;

    var number = T.Zero;
    var sign = !bits[idx] ? 1 : -1;
    for (var i = 0; i < bits.Length; i++)
      if (!isbigendian && bits[i])
        number |= T.One << i;
      else if (isbigendian && bits[^(1 + i)])
        number |= T.One << i;

    return number;
  }

  public static BigInteger FromBitsToBigInteger(
    ReadOnlySpan<bool> bits, bool isbigendian = false)
  {
    var src = isbigendian ?
      bits.ToArray().Reverse().ToArray() : bits.ToArray();

    return ToBigInteger(src);
  }

  private static BigInteger ToBigInteger(ReadOnlySpan<bool> bits)
  {
    if (bits.Length == 1 && bits[0] == false) return BigInteger.Zero;
    if (bits.Length == 1 && bits[0] == true) return BigInteger.MinusOne;

    var number = BigInteger.Zero;
    var sign = !bits[^1] ? 1 : -1;
    for (var i = 0; i < bits.Length; i++)
      if (bits[i]) number |= BigInteger.One << i;

    if (sign < 0)
      number = number - NextPowTwo(number, out _);

    return number;
  }

}
