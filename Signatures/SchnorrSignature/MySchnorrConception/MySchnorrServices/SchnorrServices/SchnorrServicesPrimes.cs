
using System.Numerics;

namespace michele.natale.Schnorrs.Services;

partial class SchnorrServices
{
  public static bool IsMRPrime(BigInteger bignumber)
  {
    if (bignumber < 2UL) return false;
    if (bignumber == 2UL) return true;
    if (bignumber.IsEven)
      return false; // even number 

    int s = 0;
    BigInteger z = bignumber - 1;
    while ((z & 1) == 0)
    {
      z >>= 1;
      s += 1;
    }

    BigInteger tmp;
    for (int a = 2; a <= 5; a++)
    {
      tmp = BigInteger.ModPow(a, z, bignumber);
      if (tmp == 1 || tmp == bignumber - 1) continue;
      for (int r = 1; r < s; r++)
      {
        tmp = BigInteger.ModPow(tmp, 2, bignumber);
        if (tmp == 1) return false;
        if (tmp == bignumber - 1) break;
      }
      if (!(tmp == bignumber - 1))
        return false;
    }
    return true;
  }
}
