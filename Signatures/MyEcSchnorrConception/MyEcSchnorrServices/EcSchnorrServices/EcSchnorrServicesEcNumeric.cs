using System.Numerics;

namespace michele.natale.Schnorrs.EcServices;

partial class EcSchnorrServices
{

  public static BigInteger ModuloExt(BigInteger x, BigInteger m)
  {
    x %= m;
    if (x.Sign < 0) x += m;
    return x % m;
  }

  public static BigInteger ModularInverse(BigInteger value, BigInteger modulo)
  {
    if (value.IsZero)
      throw new DivideByZeroException();

    if (value.Sign < 0)
      return modulo - ModularInverse(-value, modulo);

    var a = BigInteger.Zero;
    var aa = BigInteger.One;
    BigInteger b = modulo, bb = value;

    while (b != 0)
    {
      var prov = b;
      var quotient = bb / b;

      b = bb - quotient * prov;
      bb = prov;

      prov = a;
      a = aa - quotient * prov;
      aa = prov;
    }

    var c = aa;
    var gcd = bb;

    if (gcd != 1)
      throw new Exception($"Is not Coprime");

    if (ModuloExt(value * c, modulo) != 1)
      throw new ArithmeticException("Inverse has failed.");

    return ModuloExt(c, modulo);
  }

  public static BigInteger ToGroupNumber(BigInteger modulo, int length)
  {
    length /= 8;
    while (true)
    {
      var result = RngBigIntegerBSize(length) % modulo;
      if (result.Sign < 0) result = ModuloExt(result, modulo);
      if (IsCoprime(result, modulo)) return result;
    }
  }

  public static bool IsCoprime(BigInteger left, BigInteger right) =>
    BigInteger.GreatestCommonDivisor(left, right) == BigInteger.One;
}