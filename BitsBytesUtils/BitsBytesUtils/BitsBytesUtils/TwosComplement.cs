

using System.Numerics;

namespace michele.natale.BitsBytesUtils;


partial class BitsBytesUtils
{
  public static BigInteger TwosComplementRange(BigInteger number)
  {
    //Only negative numbers
    if (number.Sign >= 0) return number;
    var result = NextRangePowTwo(-number) + number;
    return result;
  }

  public static BigInteger FromTwosComplement(BigInteger number)
  { 
    var result = new List<byte>();
    var bytes = number.ToByteArray();
    result.Add((byte)(256 - bytes.First()));
    for (var i = 1; i < bytes.Length; i++) 
      result.Add((byte)(255 - bytes[i]));
    return new BigInteger(result.ToArray());
  }

  public static BigInteger TwosComplementRange(
    BigInteger number, out int exp)
  {
    //Only negative numbers
    exp = -1;
    if (number.Sign >= 0) return number;
    var result = NextRangePowTwo(-number, out exp) + number;
    return result;
  }

  public static BigInteger TwosComplementRange(
    BigInteger number, int unsigned_range_size)
  {
    //TwosComplementRange:
    // For negative values, does the same as the
    // conversion to an Unsigned, specifically using
    // the next PowerTwo range (P2 as the exponent).

    if (number.Sign >= 0) return number;

    if (!int.IsPow2(unsigned_range_size))
      throw new ArgumentException(
        $"{nameof(unsigned_range_size)} must be a power two number!",
          nameof(unsigned_range_size));

    var exp = unsigned_range_size;
    var range_size = BigInteger.One << exp;
    var result = range_size + number;

    if (result.Sign >= 0) return result;

    return range_size + (number % range_size);
  }

  public static byte[] TwosComplement(
    byte[] value, int length)
  {
    //TwosComplement:
    // Does the same as the Unary minus operator
    // for negative values.

    var result = new byte[length];

    var carry = 1u;
    for (var i = 0; i < length; i++)
    {
      var digit = (byte)~value[i] + carry;
      result[i] = (byte)digit;
      carry = digit >> 8;
    }

    if (carry != 0)
    {
      result = new byte[length + 1];
      result[length] = 1;
    }

    if (result.SequenceEqual(value))
      return [.. result, .. new byte[] { 0 }];

    return result;
  }
}
