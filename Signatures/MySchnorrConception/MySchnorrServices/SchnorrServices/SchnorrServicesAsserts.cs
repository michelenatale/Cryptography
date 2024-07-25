
using System.Numerics;

namespace michele.natale.Schnorrs.Services;

partial class SchnorrServices
{

  public static int BitOfNumber(byte[] bytes)
  {
    //Endian noch beachten
    var number = new BigInteger(bytes);
    return (int)BigInteger.Log2(number) + 1;
  }

  public static int BitOfNumber(BigInteger number)
  {
    return (int)BigInteger.Log2(number) + 1;
  }
}
