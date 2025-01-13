
//At the moment the GetBitLength-Method of
//BigInteger does not work correctly.


using System.Numerics;
using System.Runtime.CompilerServices;

namespace michele.natale.BitsBytesUtils;


partial class BitsBytesUtils
{
  public static int BitLengthLog<T>(T unsign_number)
    where T : INumber<T>, IBinaryInteger<T>
  {
    if (!IsUnsignedType(unsign_number))
      throw new TypeInitializationException(nameof(unsign_number),
        new Exception($"Parameter {nameof(unsign_number)}: Only unsigned NumberType!"));

    if (T.One == unsign_number) return 1;
    if (T.IsZero(unsign_number)) return 1;

    return int.CreateChecked(T.Log2(unsign_number)) + 1;
  }

  public static int BitLength<T>(T number)
    where T : INumber<T>, IBinaryInteger<T>
  {
    if (T.One == number) return 1;
    if (T.IsZero(number)) return 1;

    var sz = Unsafe.SizeOf<T>();

    var bsz = 8 * sz;
    var result = bsz;
    for (var i = 0; i < bsz; i++)
    {
      if ((number & (T.One << (bsz - i - 1))) == T.Zero)
        result--;
      else break;
    }

    return result;
  }

  public static int BitLengthLog(BigInteger number) =>
    BitLength(number);

  public static int BitLength(BigInteger number) =>
    //Caution
    //Here the size.size is determined based on the current number.
    //Make sure that the number is within the desired range.
    number.IsZero ? 1 :
      number.Sign < 0
        ? (int)BigInteger.Log2(TwosComplementRange(number)) + 1
        : (int)BigInteger.Log2(number) + 1;

  public static int BitLength(BigInteger number, int unsigned_range_size) =>
    number.IsZero ? 1 :
      number.Sign < 0
        ? (int)BigInteger.Log2(TwosComplementRange(number, unsigned_range_size)) + 1
        : (int)BigInteger.Log2(number) + 1;

  public static int BitLength(BigInteger number, out int exp)
  {
    exp = 0;
    if (number.IsZero) return 1;

    exp = 1;
    if (number.IsOne) return 1;

    if (number.Sign < 0)
      return (int)BigInteger.Log2(
        TwosComplementRange(number, out exp)) + 1;

    _ = NextRangePowTwo(number, out exp);
    return (int)BigInteger.Log2(number) + 1;
  }
}
