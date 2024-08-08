
using System.Numerics;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace michele.natale.LoginSystems.Services;

using static Services.AppServices;


/// <summary>
/// A RandomHolder class (cryptographically secure)
/// </summary>
partial class RandomHolder
{
  private readonly RandomNumberGenerator Rand =
    RandomNumberGenerator.Create();

  /// <summary>
  /// A Instance of the RandomHolder
  /// </summary>
  public readonly static RandomHolder Instance = new();

  /// <summary>
  /// Generates a random byte series.
  /// </summary>
  /// <param name="size">The Size of the Bytes</param>
  /// <param name="no_zeros">Selection with or without zeros.</param>
  /// <returns></returns>
  public byte[] RngBytes(int size, bool no_zeros = true)
  {
    var result = new byte[size];
    if (no_zeros) this.Rand.GetBytes(result);
    else this.Rand.GetNonZeroBytes(result);
    return result;
  }

  /// <summary>
  /// Fill a Array with Bytes
  /// </summary>
  /// <param name="bytes">A Array of Bytes</param>
  /// <param name="no_zeros">Selection with or without zeros.</param>
  public void FillBytes(Span<byte> bytes, bool no_zeros = true)
  {
    if (no_zeros) this.Rand.GetBytes(bytes);
    else this.Rand.GetNonZeroBytes(bytes);
  }

  /// <summary>
  /// Return a Single Integer
  /// </summary>
  /// <returns>A Single Integer</returns>
  public int NextInt32()
  {
    var result = new byte[4];
    this.Rand.GetBytes(result);
    return int.Abs(BitConverter.ToInt32(result));
  }

  /// <summary>
  /// Returns an integer with maximum.
  /// </summary>
  /// <param name="max">Max</param>
  /// <returns>A integer</returns>
  public int NextInt32(int max) =>
    this.NextInt32(0, max);

  /// <summary>
  /// Returns an integer in the range min and max.
  /// </summary>
  /// <param name="min">lower limit</param>
  /// <param name="max">upper limit</param>
  /// <returns>a Integer</returns>
  public int NextInt32(int min, int max)
  {
    return this.RngIntX(1, min, max).First();
  }

  /// <summary>
  /// Return a Random Array of Integer of a certain size.
  /// </summary>
  /// <param name="size">Size of the array of integer</param>
  /// <returns>Instance of Array of Integer</returns>
  public int[] RngInt32(int size)
  {
    return this.RngIntX(size, 0, int.MaxValue);
  }

  /// <summary>
  /// Returns a random array of integers with a certain upper limit and a certain size.
  /// </summary>
  /// <param name="size">Array Size</param>
  /// <param name="max">Upper limit</param>
  /// <returns>Array of interger</returns>
  public int[] RngInt32(int size, int max)
  {
    return this.RngIntX(size, 0, max);
  }

  /// <summary>
  /// Returns a random array of integers in a specific range and size.
  /// </summary>
  /// <param name="size">Array of Size</param>
  /// <param name="min">lower limit</param>
  /// <param name="max">upper limit</param>
  /// <returns>Array if integer</returns>
  public int[] RngInt32(int size, int min, int max)
  {
    return this.RngIntX(size, min, max);
  }

  /// <summary>
  /// Returns a long Number.
  /// </summary>
  /// <returns>a long number</returns>
  public long NextInt64()
  {
    var result = new byte[8];
    this.Rand.GetBytes(result);
    return long.Abs(BitConverter.ToInt64(result));
  }

  /// <summary>
  /// Returns a number of long with uuper limit.
  /// </summary>
  /// <param name="max">upper limit</param>
  /// <returns>a number of long</returns>
  public long NextInt64(long max) =>
    this.NextInt64(0, max);

  /// <summary>
  /// Return a number of long in a specific range.
  /// </summary>
  /// <param name="min">lower limit</param>
  /// <param name="max">upper limit</param>
  /// <returns>numer of long</returns>
  public long NextInt64(long min, long max)
  {
    return this.RngIntX(1, min, max).First();
  }

  /// <summary>
  /// Returns a random array of longs with specific size.
  /// </summary>
  /// <param name="size">the size</param>
  /// <returns>number of long</returns>
  public long[] RngInt64(int size)
  {
    return this.RngIntX(size, 0L, long.MaxValue);
  }

  public long[] RngInt64(int size, long max)
  {
    return this.RngIntX(size, 0L, max);
  }

  /// <summary>
  /// Returns a random array of integers in a specific range and size.
  /// </summary>
  /// <param name="size">size</param>
  /// <param name="min">lower limit</param>
  /// <param name="max">upper limit</param>
  /// <returns>return a Array of long</returns>
  public long[] RngInt64(int size, long min, long max)
  {
    return this.RngIntX(size, min, max);
  }

  /// <summary>
  /// the generic methode for a random number.
  /// </summary>
  /// <typeparam name="T">generic Type</typeparam>
  /// <param name="size">the size of Array</param>
  /// <param name="min">lower limit</param>
  /// <param name="max">upper limit</param>
  /// <returns></returns>
  private T[] RngIntX<T>(int size, T min, T max)
    where T : INumber<T>, IMinMaxValue<T>
  {
    var result = new T[size];
    this.RngIntX(result, min, max);
    return result;
  }

  /// <summary>
  /// Fill a Array with generic Type number with a specific range.
  /// </summary>
  /// <typeparam name="T">Generic Type</typeparam>
  /// <param name="ints">generic integer</param>
  /// <param name="min">lower limit</param>
  /// <param name="max">upper limit</param>
  public void RngIntX<T>(T[] ints, T min, T max)
    where T : INumber<T>, IMinMaxValue<T>
  {
    ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(min, max);

    var d = max - min;
    var tmp = new T[1];
    var length = ints.Length;
    var tsize = Marshal.SizeOf(typeof(T));
    var bytes = new byte[tsize * length];
    this.Rand.GetNonZeroBytes(bytes);
    for (int i = 0; i < length; i++)
    {
      Buffer.BlockCopy(bytes, i * tsize, tmp, 0, tsize);
      tmp[0] = T.Abs(tmp[0]) % d;
      ints[i] = min + tmp.First();
    }
  }

  /// <summary>
  /// Return a Array of Alpha Chars, with a size.
  /// </summary>
  /// <param name="size">the size of the array</param>
  /// <returns>A string with alpha chars</returns>
  public string RngAlphaString(int size)
  {
    var stralpha = AppServicesHolder.ToAlphaString()
      .OrderBy(x => Instance.NextInt32())
      .ToArray();

    var idxs = new byte[size];
    Instance.FillBytes(idxs);

    var length = stralpha.Length;
    var result = new StringBuilder();
    foreach (var idx in idxs)
      result.Append(stralpha[idx % length]);

    return result.ToString();
  }
}
