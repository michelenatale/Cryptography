

using System.Text;
using System.Numerics;
using System.Collections;
using System.Security.Cryptography;
using System.Runtime.CompilerServices;

namespace michele.natale.Cryptography.Randoms;

/// <summary>
/// A fast, cryptographically secure random number 
/// generator for all data types that need to be handled 
/// randomly in some way. 
/// <para>Created by © Michele Natale 2017</para>
/// </summary>
public class CryptoRandom : ICryptoRandom
{

  public bool IsThreadSave { get; private set; } = false;

  /// <summary>
  /// RandomNumberGenerator-instance
  /// </summary>
  private readonly RandomNumberGenerator Rand = null!;

  /// <summary>
  /// Author from this little Project.
  /// </summary>
  public string Author { get; private set; } = string.Empty;

  /// <summary>
  /// CryptoRandom Instance and Holder.
  /// </summary>
  public static CryptoRandom Instance { get; private set; } = new();

  /// <summary>
  /// ThreadSave CryptoRandom Instance and Holder.
  /// </summary>
  public static CryptoRandom Shared { get; } = new TSCryptoRandom().ThreadSave;

  /// <summary>
  /// Private ThreadSave EntryPoint
  /// </summary>
  /// <param name="is_thread_save"></param>
  private protected CryptoRandom(bool is_thread_save) : this() =>
    this.IsThreadSave = is_thread_save;

  #region C-Tor

  /// <summary>
  /// C-Tor
  /// </summary>
  public CryptoRandom()
  {
    this.Author = ToAuthor();
    this.Rand ??= RandomNumberGenerator.Create();
  }
  #endregion C-Tor

  #region Author

  /// <summary>
  /// Author Information
  /// </summary>
  /// <returns></returns>
  private static string ToAuthor()
  {
    var sb = new StringBuilder();
    sb.AppendLine("© CryptoRandom 2017");
    sb.AppendLine("Created by © Michele Natale 2017");
    sb.AppendLine("Updated to Net8.0 - 2024");
    sb.AppendLine(); sb.AppendLine();
    return sb.ToString();
  }
  #endregion Author

  #region Boolean

  /// <summary>
  /// Returns a cryptographically secure random value.
  /// </summary>
  /// <returns>Datatype bool</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public virtual bool NextCryptoBool()
  {
    return (this.NextCryptoByte() & 1) == 0;
  }

  /// <summary>
  /// Fills a buffer with cryptographically secure random values. 
  /// </summary>
  /// <param name="dest">Buffer</param>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public virtual void FillCryptoBools(bool[] dest)
  {
    var dl = (dest.Length - 1) / 32 + 1;
    var ints = new int[dl];
    this.FillCryptoInts<int>(ints);
    var a = new BitArray(ints);
    var b = new bool[a.Length];
    a.CopyTo(b, 0);
    Array.Copy(b, dest, dest.Length);
  }

  /// <summary>
  /// Returns a number of cryptographically secure random values. 
  /// </summary>
  /// <param name="size">Number of Size</param>
  /// <returns>Datatype bool</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public virtual bool[] RngCryptoBools(int size)
  {
    var result = new bool[size];
    this.FillCryptoBools(result);
    return result;
  }
  #endregion Boolean

  #region Bytes

  /// <summary>
  /// Returns a cryptographically secure random value.
  /// </summary>
  /// <param name="no_zeros">Selection with or without zeros</param>
  /// <returns>Datatype Byte</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public virtual byte NextCryptoByte(bool no_zeros = true)
  {
    var result = new byte[1];
    if (!no_zeros) this.Rand.GetBytes(result);
    else this.Rand.GetNonZeroBytes(result);
    return result.First();
  }

  /// <summary>
  /// Returns a cryptographically secure random value.
  /// </summary>
  /// <param name="max">Upper limit</param>
  /// <returns>Datatype Byte</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public virtual byte NextCryptoByte(byte max)
  {
    return this.RngInts(1, byte.MinValue, max).First();
  }

  /// <summary>
  /// Returns a cryptographically secure random value.
  /// </summary>
  /// <param name="min">Lower Limit</param>
  /// <param name="max">Upper Limit</param>
  /// <returns>Datatype Byte</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public virtual byte NextCryptoByte(byte min, byte max)
  {
    return this.RngInts(1, min, max).First();
  }

  /// <summary>
  /// Returns a number of cryptographically secure random values. 
  /// </summary>
  /// <param name="size">Number of Size</param>
  /// <param name="no_zeros">Selection with or without zeros</param>
  /// <returns>Datatype Byte</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public virtual byte[] RngCryptoBytes(int size, bool no_zeros = true)
  {
    var result = new byte[size];
    if (!no_zeros) this.Rand.GetBytes(result);
    else this.Rand.GetNonZeroBytes(result);
    return result;
  }

  /// <summary>
  /// Returns a number of cryptographically secure random values. 
  /// </summary>
  /// <param name="size">Number of Size</param>
  /// <param name="min">Lower Limit</param>
  /// <param name="max">Upper Limit</param>
  /// <returns>Array of byte</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public virtual byte[] RngCryptoBytes(int size, byte min, byte max)
  {
    return this.RngInts(size, min, max);
  }

  /// <summary>
  /// Fills a buffer with cryptographically secure random values. 
  /// </summary>
  /// <param name="bytes">buffer</param>
  /// <param name="no_zeros">Selection with or without zeros</param>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public virtual void FillCryptoBytes(Span<byte> bytes, bool no_zeros = true)
  {
    if (!no_zeros) this.Rand.GetBytes(bytes);
    else this.Rand.GetNonZeroBytes(bytes);
  }

  /// <summary>
  /// Fills a buffer with cryptographically secure random values. 
  /// </summary>
  /// <param name="bytes">uffer</param>
  /// <param name="min">Lower limit</param>
  /// <param name="max">Upper limit</param>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public virtual void FillCryptoBytes(Span<byte> bytes, byte min, byte max)
  {
    var result = new byte[bytes.Length];
    this.RngInts(result, min, max);
    result.CopyTo(bytes);
  }

  #endregion Bytes

  #region Ints

  /// <summary>
  /// Returns a cryptographically secure random value.
  /// </summary>
  /// <typeparam name="T">Generic Type</typeparam>
  /// <returns>Datatype Generic T</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public virtual T NextCryptoInt<T>()
    where T : INumber<T>, INumberBase<T>, IMinMaxValue<T>
  {
    return this.RngInts(1, T.Zero, T.MaxValue).First();
  }

  /// <summary>
  /// Returns a cryptographically secure random value.
  /// </summary>
  /// <typeparam name="T">Generic Type</typeparam>
  /// <param name="max">Upper Limit</param>
  /// <returns>Datatype Generic T</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public virtual T NextCryptoInt<T>(T max)
    where T : INumber<T>, INumberBase<T>
  {
    return this.NextCryptoInt(T.Zero, max);
  }

  /// <summary>
  /// Returns a cryptographically secure random value.
  /// </summary>
  /// <typeparam name="T">Generic Type</typeparam>
  /// <param name="min">Lower Limit</param>
  /// <param name="max">Upper Limit</param>
  /// <returns>Datatype generic T</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public virtual T NextCryptoInt<T>(T min, T max)
    where T : INumber<T>, INumberBase<T>
  {
    return this.RngInts(1, min, max).First();
  }

  /// <summary>
  /// Returns a number of cryptographically secure random values. 
  /// </summary>
  /// <typeparam name="T">Generic Type T</typeparam>
  /// <param name="size">Number of Size</param>
  /// <returns>Datatype Generic T</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public virtual T[] RngCryptoInts<T>(int size)
    where T : INumber<T>, INumberBase<T>, IMinMaxValue<T>
  {
    return this.RngInts(size, T.Zero, T.MaxValue);
  }

  /// <summary>
  /// Returns a number of cryptographically secure random values. 
  /// </summary>
  /// <typeparam name="T">Generic Type T</typeparam>
  /// <param name="size">Number of Size</param>
  /// <param name="max">Upper limit</param>
  /// <returns>Datatype Generic T</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public virtual T[] RngCryptoInts<T>(int size, T max)
    where T : INumber<T>, INumberBase<T>
  {
    return this.RngInts(size, T.Zero, max);
  }

  /// <summary>
  /// Returns a number of cryptographically secure random values. 
  /// </summary>
  /// <typeparam name="T">Generic Type T</typeparam>
  /// <param name="size">Number of Size</param>
  /// <param name="min">Lower Limit</param>
  /// <param name="max">upper Limit</param>
  /// <returns>Datatype Generic T</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public virtual T[] RngCryptoInts<T>(int size, T min, T max)
    where T : INumber<T>, INumberBase<T>
  {
    return this.RngInts(size, min, max);
  }

  /// <summary>
  /// Fills a buffer with cryptographically secure random values. 
  /// </summary>
  /// <typeparam name="T">Generic Type T</typeparam>
  /// <param name="ints">Buffer</param>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public virtual void FillCryptoInts<T>(Span<T> ints)
    where T : INumber<T>, INumberBase<T>, IMinMaxValue<T>
  {
    var result = this.RngInts(ints.Length, T.Zero, T.MaxValue);
    result.CopyTo(ints);
  }

  /// <summary>
  /// Fills a buffer with cryptographically secure random values. 
  /// </summary>
  /// <typeparam name="T">Generic Type T</typeparam>
  /// <param name="ints">Buffer</param>
  /// <param name="max">Upper Limit</param>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public virtual void FillCryptoInts<T>(Span<T> ints, T max)
    where T : INumber<T>, INumberBase<T>
  {
    this.FillCryptoInts(ints, T.Zero, max);
  }

  /// <summary>
  /// Fills a buffer with cryptographically secure random values. 
  /// </summary>
  /// <typeparam name="T">Generic Type T</typeparam>
  /// <param name="ints">Buffer</param>
  /// <param name="min">Lower Limit</param>
  /// <param name="max">Upper Limit</param>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public virtual void FillCryptoInts<T>(Span<T> ints, T min, T max)
    where T : INumber<T>, INumberBase<T>
  {
    var result = this.RngInts(ints.Length, min, max);
    result.CopyTo(ints);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private T[] RngInts<T>(int size, T min, T max)
    where T : INumber<T>, INumberBase<T>
  {
    if (size == 0) return [];
    var result = new T[size];
    this.RngInts(result, min, max);
    return result;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private unsafe void RngInts<T>(T[] ints, T min, T max)
    where T : INumber<T>, INumberBase<T>
  {
    if (IsNullOrEmpty(ints)) return;
    ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(min, max);

    var d = max - min;
    //var tmp = new T[1];
    var length = ints.Length;
    var type_bits = Unsafe.SizeOf<T>();
    var bytes = new byte[type_bits * length];
    this.Rand.GetNonZeroBytes(bytes);
    //DataTypes Int128 and UInt128 are not yet recognized as primitives. 
    if (typeof(T).IsPrimitive || typeof(T) == typeof(Int128) || typeof(T) == typeof(UInt128))
      for (int i = 0; i < length; i++)
      {
        var tmp = Unsafe.ReadUnaligned<T>(ref bytes[i * type_bits]);
        ints[i] = T.Abs(tmp); ints[i] %= d; ints[i] += min;
      }
  }

  #endregion Ints

  #region Strings

  /// <summary>
  /// Returns a number of cryptographically secure random letters, 
  /// whereby the letters are predefined. 
  /// </summary>
  /// <param name="count">Number of Count</param>
  /// <param name="alphabet">
  /// Specified string from which the random letters are drawn.  
  /// </param>
  /// <returns>Datatype String</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public virtual string NextString(int count, string alphabet)
  {
    var sb = new StringBuilder(count);

    for (int n = 0; n < count; ++n)
      sb.Append(alphabet[this.NextCryptoInt(0, alphabet.Length)]);

    return sb.ToString();
  }
  #endregion Strings

  #region Doubles


  /// <summary>
  ///  Returns a cryptographically secure random value.
  /// </summary>
  /// <returns>Datatype Double</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public virtual double NextCryptoDouble()
  {
    // Multiplier equals 0x1.0p-53, or as 64 bits: 0x3CA0000000000000
    return (this.NextCryptoInt<ulong>() >> 11) * 1.1102230246251565E-16;
  }

  /// <summary>
  /// Returns a cryptographically secure random value.
  /// </summary>
  /// <param name="min">Lower Limit</param>
  /// <param name="max">Upper Limit</param>
  /// <returns>Datatype Double</returns>
  /// <exception cref="ArgumentOutOfRangeException"></exception>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public virtual double NextCryptoDouble(double min, double max)
  {
    if (max > min)
      return min + this.NextCryptoDouble() * (max - min);
    throw new ArgumentOutOfRangeException(nameof(max), "Out of range");
  }

  /// <summary>
  /// Returns a number of cryptographically secure random values. 
  /// </summary>
  /// <param name="size">Number of Size</param>
  /// <returns></returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public virtual double[] RngCryptoDouble(int size)
  {
    var result = new double[size];
    this.FillCryptoDoubles(result);
    return result;
  }

  /// <summary>
  /// Returns a number of cryptographically secure random values. 
  /// </summary>
  /// <param name="size">Number of Size</param>
  /// <param name="max">Upper limit</param>
  /// <returns>Datatype Double</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public virtual double[] RngCryptoDouble(int size, double max)
  {
    return this.RngCryptoDouble(size, 0.0, max);
  }

  /// <summary>
  /// Returns a number of cryptographically secure random values. 
  /// </summary>
  /// <param name="size">Number of Size</param>
  /// <param name="min">Lower Limit</param>
  /// <param name="max">Upper Limit</param>
  /// <returns>Datatype Double</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public virtual double[] RngCryptoDouble(int size, double min, double max)
  {
    var result = new double[size];
    this.FillCryptoDoubles(result, min, max);
    return result;
  }

  /// <summary>
  /// Fills a buffer with cryptographically secure random values. 
  /// </summary>
  /// <param name="dest">Buffer</param>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public virtual void FillCryptoDoubles(double[] dest)
  {
    var uls = new ulong[dest.Length];
    this.FillCryptoInts<ulong>(uls);

    for (var i = 0; i < dest.Length; i++)
      dest[i] = (uls[i] >> 11) * 1.1102230246251565E-16;
  }

  /// <summary>
  /// Fills a buffer with cryptographically secure random values. 
  /// </summary>
  /// <param name="dest">Buffer</param>
  /// <param name="max">Upper Limit</param>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public virtual void FillCryptoDoubles(double[] dest, double max)
  {
    this.FillCryptoDoubles(dest, 0.0, max);
  }

  /// <summary>
  /// Fills a buffer with cryptographically secure random values. 
  /// </summary>
  /// <param name="dest">Buffer</param>
  /// <param name="min">Lower Limit</param>
  /// <param name="max">Upper Limit</param>
  /// <exception cref="ArgumentOutOfRangeException"></exception>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public virtual void FillCryptoDoubles(double[] dest, double min, double max)
  {
    if (max > min)
    {
      this.FillCryptoDoubles(dest);
      for (var i = 0; i < dest.Length; i++)
        dest[i] = min + dest[i] * (max - min);
      return;
    }
    throw new ArgumentOutOfRangeException(nameof(max), "Out of range");
  }

  #endregion Double

  #region Shuffel

  /// <summary>
  /// Randomly shuffles a string cryptographically.
  /// </summary>
  /// <param name="str">The String</param>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public virtual void Shuffle(ref string str)
  {
    if (str.Length > 1)
    {
      var arr = str.ToCharArray();
      this.Shuffle(arr);
      str = new string(arr);
    }
  }

  /// <summary>
  /// Randomly shuffles a buffer with values cryptographically 
  /// with the Fisher-Yates algorithm.
  /// </summary>
  /// <typeparam name="T">Generic Type T</typeparam>
  /// <param name="items">Buffer of Generic Type T</param>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public virtual void Shuffle<T>(T[] items) => this.Shuffle(items, 0, -1);

  /// <summary>
  /// Randomly shuffles a buffer with values cryptographically 
  /// with the Fisher-Yates algorithm.
  /// </summary>
  /// <typeparam name="T">Generic type T</typeparam>
  /// <param name="items">Buffer</param>
  /// <param name="offset">Offset</param>
  /// <param name="count">Number of Count</param>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public virtual void Shuffle<T>(T[] items, int offset, int count)
  {
    // Fisher-Yates algorithm
    count = AssertBounds(items.Length, offset, count);

    T temp;
    int j, end = count + offset;

    while (count > 0)
    {
      j = offset + this.NextCryptoInt(count--);
      temp = items[j];
      items[j] = items[--end];
      items[end] = temp;
    }
  }

  /// <summary>
  /// Randomly shuffles a List with values cryptographically 
  /// with the Fisher-Yates algorithm.
  /// </summary>
  /// <typeparam name="T">Generic Type T</typeparam>
  /// <param name="items">List of generic Type T</param>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public virtual void Shuffle<T>(IList<T> items) => this.Shuffle(items, 0, -1);


  /// <summary>
  /// Randomly shuffles a List with values cryptographically 
  /// with the Fisher-Yates algorithm.
  /// </summary>
  /// <typeparam name="T">Generic Type T</typeparam>
  /// <param name="items">List of Generic Type T</param>
  /// <param name="offset">Offset</param>
  /// <param name="count">Number of Count</param>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public virtual void Shuffle<T>(IList<T> items, int offset, int count)
  {
    // Fisher-Yates algorithm
    count = AssertBounds(items.Count, offset, count);

    T temp;
    int j, end = count + offset;

    while (count > 0)
    {
      j = offset + this.NextCryptoInt(count--);
      temp = items[j];
      items[j] = items[--end];
      items[end] = temp;
    }
  }
  #endregion Shuffel

  #region Utils

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private static bool IsNullOrEmpty<T>(T[] ints)
    where T : INumber<T>, INumberBase<T>
  {
    if (ints is null) return true;
    return ints.Length == 0;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private static int AssertBounds(int length, int offset, int count)
  {
    if (offset >= length || offset < 0)
    {
      if (offset == 0 && count <= 0)
        // Legal exception for empty array
        return 0;
      throw new ArgumentOutOfRangeException(nameof(offset), "Offset out of range");
    }

    if (count < 0)
      return length - offset;

    if ((long)offset + count > length)
      throw new ArgumentException("Count exceeds length", nameof(count));

    return count;
  }
  #endregion Utils

  #region ThreadSaveCryptoRandom

  private sealed class TSCryptoRandom : TSCryptoRandomBase
  {
    [ThreadStatic]
    private static CryptoRandom? TSRand;

    internal CryptoRandom ThreadSave { get; private set; } = null!;

    public TSCryptoRandom() : base(is_thread_save: true)
    {
      LocalRand = TSRand ?? Create();
      this.ThreadSave = LocalRand ?? Create();
    }

    private static CryptoRandom LocalRand { get; set; } = null!;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static CryptoRandom Create() => TSRand = new CryptoRandom(true);
  }

  private abstract class TSCryptoRandomBase : CryptoRandom
  {
    private protected TSCryptoRandomBase(bool is_thread_save)
    {
      this.IsThreadSave = is_thread_save;
    }
  }


  #endregion ThreadSaveCryptoRandom

}
