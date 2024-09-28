

using System.Security.Cryptography;

namespace michele.natale.Cryptography.Randoms;

internal class CryptoRandom
{
  private readonly static RandomNumberGenerator Rand =
    RandomNumberGenerator.Create();

  public static void FillBytes(
    Span<byte> bytes, bool without_zeros = true)
  {
    if (without_zeros)
      Rand.GetNonZeroBytes(bytes);
    else Rand.GetBytes(bytes);
  }

  public static byte[] RngBytes(
    int size, bool without_zeros = true)
  {
    var result = new byte[size];
    if (without_zeros)
      Rand.GetNonZeroBytes(result);
    else Rand.GetBytes(result);
    return result;
  }

  public static int ToInt32()
  {
    var bytes = new byte[4];
    Rand.GetBytes(bytes);
    return BitConverter.ToInt32(bytes, 0);
  }

  public static long ToInt64()
  {
    var bytes = new byte[8];
    Rand.GetBytes(bytes);
    return BitConverter.ToInt64(bytes, 0);
  }

}
