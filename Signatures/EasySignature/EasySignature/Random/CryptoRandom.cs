

using System.Security.Cryptography;

namespace michele.natale.Cryptography.Randoms;

/// <summary>
/// Cryptographically secure random 
/// number generator for internal use. 
/// </summary>
internal class CryptoRandom
{
  private readonly static RandomNumberGenerator Rand =
    RandomNumberGenerator.Create();

  /// <summary>
  /// Fills an array of byte with random values.
  /// </summary>
  /// <param name="bytes">Desired buffer</param>
  /// <param name="without_zeros">Selection with or without zeros.</param>
  public static void FillBytes(
    Span<byte> bytes, bool without_zeros = true)
  {
    if (without_zeros)
      Rand.GetNonZeroBytes(bytes);
    else Rand.GetBytes(bytes);
  }

  /// <summary>
  /// Returns an array of byte with random values.
  /// </summary>
  /// <param name="size">Desired Size</param>
  /// <param name="without_zeros">Selection with or without zeros.</param>
  /// <returns>Array of byte</returns>
  public static byte[] RngBytes(
    int size, bool without_zeros = true)
  {
    var result = new byte[size];
    if (without_zeros)
      Rand.GetNonZeroBytes(result);
    else Rand.GetBytes(result);
    return result;
  }

  /// <summary>
  /// Returns the next Int32 value .
  /// </summary>
  /// <returns>Int32</returns>
  public static int ToInt32()
  {
    var bytes = new byte[4];
    Rand.GetBytes(bytes);
    return BitConverter.ToInt32(bytes, 0);
  }

  /// <summary>
  /// Returns the next Int64 value 
  /// </summary>
  /// <returns>Int64</returns>
  public static long ToInt64()
  {
    var bytes = new byte[8];
    Rand.GetBytes(bytes);
    return BitConverter.ToInt64(bytes, 0);
  }
}
