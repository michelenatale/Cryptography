
using System.Numerics;

namespace michele.natale.Schnorrs.Services;


partial class SchnorrServices
{
  public const int Q_MIN_SIZE = 160;
  public const int Q_MAX_SIZE = 256;
  public const int P_MIN_SIZE = 1024;
  public const int P_MAX_SIZE = 3072;

  public static SchnorrGroup GenerateRngSchnorrGroup(
    int psize = P_MAX_SIZE, int qsize = Q_MIN_SIZE)
  {
    return new SchnorrGroup(GenerateParametersPQG(psize, qsize));
  }

  public static (byte[] P, byte[] Q, byte[] G) GenerateParametersPQG(
    int psize = P_MIN_SIZE, int qsize = Q_MIN_SIZE)
  {
    AssertPQG(psize, qsize);
    var q = RngBigIntegerPrime(qsize);
    var p = Rng_P_From_Q(q, psize);
    var g = Rng_G_From_PQ(p, q, qsize);
    var r = ToBytes(p, q, g);
    return (r[0], r[1], r[2]);
  }
  private static BigInteger Rng_P_From_Q(BigInteger q, int bits)
  {
    //Q must be a divisor of P, plus a prime.

    BigInteger result;
    var (min, max) = ToMinMax(bits);
    while (true)
    {
      result = RngBigInteger(min, max);
      var remainder = BigInteger.ModPow(result - 1, 1, q);
      result -= remainder;
      if (result.IsEven) continue;
      if (IsMRPrime(result)) break;
    }
    return result;
  }

  private static BigInteger Rng_G_From_PQ(BigInteger p, BigInteger q, int qsize)
  {
    var ratio = (p - 1) / q;
    var k = RngBigInteger(qsize);

    //g^q ≡ 1 mod p
    return BigInteger.ModPow(k, ratio, p);
  }

  private static bool IsPowerTwo(int number)
  {
    if (number < 1) return false;
    return (number & (number - 1)) == 0;
  }

  private static void AssertPQG(int psize, int qsize)
  {
    if (psize < P_MIN_SIZE || psize > P_MAX_SIZE)
      throw new ArgumentException($"{nameof(psize)} has failed!");

    if (qsize < Q_MIN_SIZE || qsize > Q_MAX_SIZE)
      throw new ArgumentException($"{nameof(qsize)} has failed!");

    if (!IsPowerTwo(psize) && psize % 512 != 0)
      throw new ArgumentException($"{nameof(psize)} is not a PowerOfTwo!");

    if (!IsPowerTwo(qsize) && qsize % 32 != 0)
      throw new ArgumentException($"{nameof(qsize)} is not a PowerOfTwo!");
  }
}
