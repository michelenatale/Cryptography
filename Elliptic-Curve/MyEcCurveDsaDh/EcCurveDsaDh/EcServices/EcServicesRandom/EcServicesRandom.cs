

namespace michele.natale.EcCurveDsaDh;

partial class EcService
{
  private static readonly Random Rand = new();

  public static void FillBytes(byte[] bytes)
  {
    Rand.NextBytes(bytes);
  }
  public static byte[] RngBytes(int size)
  {
    var result = new byte[size];
    Rand.NextBytes(result);
    return result;
  }

  public static void FillBytes(Span<byte> bytes)
  {
    Rand.NextBytes(bytes);
  }

  public static int NextInt32() =>
    Rand.Next();

  public static int NextInt32(int max) =>
    Rand.Next(0, max);

  public static int NextInt32(int min, int max) =>
    min < max ? Rand.Next() : throw new ArgumentOutOfRangeException(nameof(max));

  public static int[] RngInt32(int size)
  {
    var result = new int[size];
    for (var i = 0; i < size; i++)
      result[i] = Rand.Next();
    return result;
  }

  public static int[] RngInt32(int size, int max)
  {
    return RngInt32(size, 0, max);
  }

  public static int[] RngInt32(int size, int min, int max)
  {
    ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(min, max);
    var result = new int[size];
    for (var i = 0; i < size; i++)
      result[i] = Rand.Next(min, max);
    return result;
  }

  public static long NextInt64() =>
    Rand.NextInt64();

  public static long NextInt64(long max) =>
    Rand.NextInt64(0, max);

  public static long NextInt64(long min, long max) =>
    min < max ? Rand.Next() : throw new ArgumentOutOfRangeException(nameof(max));

  public static long[] RngInt64(int size)
  {
    var result = new long[size];
    for (var i = 0; i < size; i++)
      result[i] = Rand.Next();
    return result;
  }

  public static long[] RngInt64(int size, long max)
  {
    return RngInt64(size, 0, max);
  }

  public static long[] RngInt64(int size, long min, long max)
  {
    ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(min, max);
    var result = new long[size];
    for (var i = 0; i < size; i++)
      result[i] = Rand.NextInt64(min, max);
    return result;
  }
}
