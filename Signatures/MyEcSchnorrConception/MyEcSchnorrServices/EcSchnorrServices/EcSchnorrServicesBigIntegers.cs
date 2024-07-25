using System.Numerics;

namespace michele.natale.Schnorrs.EcServices;



partial class EcSchnorrServices
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

  public static BigInteger RngBigIntegerBSize(int bytesize)
  {
    var result = new byte[bytesize];
    FillCryptoBytes(result);
    return BigInteger.Abs(new BigInteger(result));
  }

  public static (BigInteger Min, BigInteger Max) ToMinMax(int bits) =>
    (BigInteger.One << (bits - 1), (BigInteger.One << bits) - 1);

  public static BigInteger ToBILE(
    ReadOnlySpan<byte> bytes, bool isunsign = true, bool is_little_endian = false)
  {
    return new BigInteger(bytes, isunsign, !is_little_endian);
  }

  public static byte[] FromBILE(BigInteger number, bool isunsign = true, bool is_little_endian = false)
  {
    return number.ToByteArray(isunsign, !is_little_endian);
  }


}