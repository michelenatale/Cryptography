

using System.Numerics;

namespace michele.natale.BitsBytesUtils;


partial class BitsBytesUtils
{

  public static BigInteger NextRangePowTwo(BigInteger number)
  {
    if (number.Sign < 0)
      throw new ArgumentOutOfRangeException(nameof(number));

    if (number.IsPowerOfTwo) return number;

    var exp = 1;
    var result = BigInteger.Zero;
    while (result < number)
    {
      exp <<= 1;
      result = BigInteger.One << exp;
    }

    return result;
  }

  public static BigInteger NextRangePowTwo(
    BigInteger number, out int exp)
  {
    exp = -1;

    if (number.Sign < 0)
      throw new ArgumentOutOfRangeException(nameof(number));

    if (number.IsPowerOfTwo) return number;

    exp = 1;
    var result = BigInteger.Zero;
    while (result < number)
    {
      exp <<= 1;
      result = BigInteger.One << exp;
    }

    return result;
  }

  public static BigInteger NextPowTwo(BigInteger number, out int exp)
  {
    exp = -1;

    if (number.Sign < 0)
      throw new ArgumentOutOfRangeException(nameof(number));

    if (number.IsPowerOfTwo) return number;

    exp = 0;
    var result = BigInteger.Zero;
    while (result < number)
      result = BigInteger.One << exp++;

    exp--;

    return result;
  }

  public static int ToRangeSize(BigInteger number)
  {
    if (number.IsOne) return 1;
    if (number.IsZero) return 0;

    int exp;
    if (number.Sign < 0)
      _ = TwosComplementRange(number, out exp);
    else _ = NextRangePowTwo(number, out exp);
    return exp;
  }
}
