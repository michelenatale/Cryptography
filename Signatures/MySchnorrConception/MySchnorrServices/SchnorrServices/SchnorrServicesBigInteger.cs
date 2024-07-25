
using System.Numerics;

namespace michele.natale.Schnorrs.Services;

partial class SchnorrServices
{
  public static BigInteger RngBigIntegerPrime(int bits)
  {
    var (min, max) = ToMinMax(bits);
    var result = BigInteger.Zero;
    while (!IsMRPrime(result))
    {
      result = RngBigInteger(min, max);
      if (result.Sign < 0) result = -result;
      if (result.IsEven) result--;
    }
    return result;
  }

  public static BigInteger RngBigIntegerPrime(
     BigInteger min, BigInteger max)
  {
    var result = BigInteger.Zero;
    while (!IsMRPrime(result))
    {
      result = RngBigInteger(min, max);
      if (result.Sign < 0) result = -result;
      if (result.IsEven) result = -result;
    }
    return result;
  }

  public static BigInteger RngBigInteger(int bits)
  {
    var (min, max) = ToMinMax(bits);
    return RngBigInteger(min, max);
  }

  public static BigInteger RngBigInteger(
     BigInteger min, BigInteger max)
  {
    var bytes = (max - min).ToByteArray();
    FillCryptoBytes(bytes);
    bytes[^1] = (byte)(bytes.Last() & 0x7F);

    var result = BigInteger.Abs(new BigInteger(bytes));
    result += min;
    if (result > max || result < min) return RngBigInteger(min, max);
    return result;
  }

  public static (BigInteger Min, BigInteger Max) ToMinMax(int bits) =>
    (BigInteger.One << (bits - 1), (BigInteger.One << bits) - 1);

}
