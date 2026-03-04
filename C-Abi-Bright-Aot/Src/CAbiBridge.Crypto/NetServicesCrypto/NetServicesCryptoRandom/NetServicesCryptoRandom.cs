

using System.Numerics;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace michele.natale;


public partial class NetServices
{
  private static readonly RandomNumberGenerator CRand =
    RandomNumberGenerator.Create();

  public static byte[] RngCryptoBytes(
    int size, bool no_zeros = true)
  {
    var result = new byte[size];
    if (!no_zeros)
    {
      CRand.GetBytes(result);
      if (result.First() == 0)
        result[0]++;
    }
    else CRand.GetNonZeroBytes(result);
    return result;
  }

  public static void FillCryptoBytes(
    Span<byte> bytes, bool no_zeros = true)
  {
    if (!no_zeros)
    {
      CRand.GetBytes(bytes);
      if (bytes[0] == 0)
        bytes[0]++;
    }
    else CRand.GetNonZeroBytes(bytes);
  }

  public static int NextCryptoInt32()
  {
    var result = new byte[4];
    CRand.GetBytes(result);
    return int.Abs(BitConverter.ToInt32(result));
  }

  public static int NextCryptoInt32(int max) =>
    NextCryptoInt32(0, max);

  public static int NextCryptoInt32(int min, int max)
  {
    ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(min, max);
    return RngCryptoIntX(1, min, max)[0];
  }

  public static int[] RngCryptoInt32(int size)
  {
    return RngCryptoIntX(size, 0, int.MaxValue);
  }

  public static int[] RngCryptoInt32(int size, int max)
  {
    return RngCryptoIntX(size, 0, max);
  }

  public static int[] RngCryptoInt32(
    int size, int min, int max)
  {
    ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(min, max);
    return RngCryptoIntX(size, min, max);
  }

  public static long NextCryptoInt64()
  {
    var result = new byte[8];
    CRand.GetBytes(result);
    return long.Abs(BitConverter.ToInt64(result));
  }

  public static long NextCryptoInt64(long max) =>
    NextCryptoInt64(0L, max);

  public static long NextCryptoInt64(
    long min, long max)
  {
    ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(min, max);
    return RngCryptoIntX(1, min, max)[0];
  }

  public static long[] RngCryptoInt64(int size)
  {
    return RngCryptoIntX(size, 0L, long.MaxValue);
  }

  public static long[] RngCryptoInt64(
    int size, long max)
  {
    return RngCryptoIntX(size, 0L, max);
  }

  public static long[] RngCryptoInt64(
    int size, long min, long max)
  {
    ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(min, max);
    return RngCryptoIntX(size, min, max);
  }

  public static double NextCryptoDouble()
  {
    var result = RngCryptoIntX(1, ulong.MinValue, ulong.MaxValue)[0];
    // Multiplier equals 0x1.0p-53, or as 64 bits: 0x3CA0000000000000
    return (result >> 11) * 1.1102230246251565E-16;
  }

  public static double NextCryptoDouble(double max)
  {
    if (max < 0)
      throw new ArgumentOutOfRangeException(nameof(max), "Out of range");

    var result = RngCryptoIntX(1, ulong.MinValue, ulong.MaxValue)[0];

    // Multiplier equals 0x1.0p-53, or as 64 bits: 0x3CA0000000000000
    return max * ((result >> 11) * 1.1102230246251565E-16);
  }

  public static double NextCryptoDouble(double min, double max)
  {
    if (max < 0)
      throw new ArgumentOutOfRangeException(nameof(max), "Out of range");

    var result = RngCryptoIntX(1, ulong.MinValue, ulong.MaxValue)[0];

    // Multiplier equals 0x1.0p-53, or as 64 bits: 0x3CA0000000000000
    return min + (((result >> 11) * 1.1102230246251565E-16) * (max - min));
  }

  public static double[] RngCryptoDouble(int size)
  {
    var result = new double[size];
    var values = RngCryptoIntX(size, ulong.MinValue, ulong.MaxValue);

    for (var i = 0; i < size; i++)
      result[i] = (values[i] >> 11) * 1.1102230246251565E-16;

    return result;
  }

  public static double[] RngCryptoDouble(
    int size, double max)
  {
    var result = new double[size];
    var values = RngCryptoIntX(size, ulong.MinValue, ulong.MaxValue);

    for (var i = 0; i < size; i++)
      result[i] = max * ((values[i] >> 11) * 1.1102230246251565E-16);

    return result;
  }

  public static double[] RngCryptoDouble(
    int size, double min, double max)
  {
    var result = new double[size];
    var values = RngCryptoIntX(size, ulong.MinValue, ulong.MaxValue);

    for (var i = 0; i < size; i++)
      result[i] = min + (((values[i] >> 11) * 1.1102230246251565E-16) * (max - min));

    return result;
  }

  public static float NextCryptoSingle()
  {
    var result = RngCryptoIntX(1, ulong.MinValue, ulong.MaxValue)[0];
    // Multiplier equals 0x1.0p-53, or as 64 bits: 0x3CA0000000000000
    return Lerp(0f, 1f, (float)(double)((result >> 11) * 1.1102230246251565E-16));

  }

  public static float NextCryptoSingle(float max)
  {
    if (max < 0)
      throw new ArgumentOutOfRangeException(nameof(max), "Out of range");

    var result = RngCryptoIntX(1, ulong.MinValue, ulong.MaxValue)[0];

    // Multiplier equals 0x1.0p-53, or as 64 bits: 0x3CA0000000000000
    return Lerp(0f, max, (float)(double)((result >> 11) * 1.1102230246251565E-16));
  }

  public static float NextCryptoSingle(float min, float max)
  {
    if (max < min)
      throw new ArgumentOutOfRangeException(nameof(max), "Out of range");

    if (max == min) return min;
    var result = RngCryptoIntX(1, ulong.MinValue, ulong.MaxValue)[0];

    // Multiplier equals 0x1.0p-53, or as 64 bits: 0x3CA0000000000000
    return Lerp(min, max, (float)(double)((result >> 11) * 1.1102230246251565E-16));
  }

  public static float[] RngCryptoSingle(int size)
  {
    var result = new float[size];
    var values = RngCryptoIntX(size, ulong.MinValue, ulong.MaxValue);

    for (var i = 0; i < size; i++)
      result[i] = Lerp(0f, 1f, (float)(double)((values[i] >> 11) * 1.1102230246251565E-16));

    return result;
  }

  public static float[] RngCryptoSingle(int size, float max)
  {
    var result = new float[size];
    var values = RngCryptoIntX(size, ulong.MinValue, ulong.MaxValue);

    for (var i = 0; i < size; i++)
      result[i] = Lerp(0f, max, (float)(double)((values[i] >> 11) * 1.1102230246251565E-16));

    return result;
  }

  public static float[] RngCryptoSingle(int size, float min, float max)
  {
    var result = new float[size];
    var values = RngCryptoIntX(size, ulong.MinValue, ulong.MaxValue);

    for (var i = 0; i < size; i++)
      result[i] = Lerp(min, max, (float)(double)((values[i] >> 11) * 1.1102230246251565E-16));

    return result;
  }

  //public static decimal NextCryptoDecimal()
  //{
  //  Span<byte> bytes = stackalloc byte[12]; // 96 Bit
  //  CRand.GetBytes(bytes);

  //  // 96-Bit Integer konstruieren (little endian)
  //  // Construct a 96-bit integer (little endian)
  //  var lo = BitConverter.ToInt32(bytes[0..4]);
  //  var mid = BitConverter.ToInt32(bytes[4..8]);
  //  var hi = BitConverter.ToInt32(bytes[8..12]);

  //  // Vorzeichen: positiv, Scale: 28 Stellen
  //  // Sign: positive, scale: 28 digits
  //  const byte scale = 28;

  //  var isnegative = false;
  //  var value = new decimal(lo, mid, hi, isnegative, scale);

  //  // Bereich ist [0, ~0.9999999999999999999999999999]
  //  // Range is [0, ~0.9999999999999999999999999999]
  //  return value;
  //}



  //public static decimal NextCryptoDecimal()
  //{
  //  Span<byte> bytes = stackalloc byte[16];
  //  CRand.GetBytes(bytes);

  //  // 128-bit integer
  //  var big = new BigInteger(bytes, isUnsigned: true, isBigEndian: false);

  //  // modulo 10^28
  //  big %= BigInteger.Pow(10, 28);

  //  // jetzt ist big garantiert < 10^28
  //  int lo = (int)(big & 0xFFFFFFFF);
  //  int mid = (int)((big >> 32) & 0xFFFFFFFF);
  //  int hi = (int)((big >> 64) & 0xFFFFFFFF);

  //  return new decimal(lo, mid, hi, false, 28);
  //}

  public static decimal NextCryptoDecimal()
  {
    Span<byte> bytes = stackalloc byte[12]; // 96 Bit
    CRand.GetBytes(bytes);

    int lo = BitConverter.ToInt32(bytes[0..4]);
    int mid = BitConverter.ToInt32(bytes[4..8]);
    int hi = BitConverter.ToInt32(bytes[8..12]);

    // obere 3 Bits löschen → garantiert < 2^93
    hi &= 0x1FFFFFFF;

    const byte scale = 28;
    return new decimal(lo, mid, hi, false, scale);
  }

  public static decimal NextCryptoDecimal(decimal max)
  {
    if (max < 0)
      throw new ArgumentOutOfRangeException(nameof(max), "Out of range");

    return NextCryptoDecimal(0m, max);
  }

  public static decimal NextCryptoDecimal(decimal min, decimal max)
  {
    ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(min, max);

    var value = NextCryptoDecimal(); // [0,1)
    return min + (max - min) * value;
  }

  public static decimal[] RngCryptoDecimal01(int size)
  {
    var result = new decimal[size];
    Span<byte> bytes = stackalloc byte[12];

    for (int i = 0; i < size; i++)
    {
      CRand.GetBytes(bytes);

      int lo = BitConverter.ToInt32(bytes[0..4]) & 0x7FFFFFFF;
      int mid = BitConverter.ToInt32(bytes[4..8]) & 0x7FFFFFFF;
      int hi = BitConverter.ToInt32(bytes[8..12]) & 0x1FFFFFFF;

      result[i] = new decimal(lo, mid, hi, false, 28);
    }

    return result;
  }

  public static decimal[] RngCryptoDecimal(int size)
  {
    var result = new decimal[size];
    Span<byte> bytes = stackalloc byte[12];

    for (int i = 0; i < size; i++)
    {
      CRand.GetBytes(bytes);

      int lo = Unsafe.ReadUnaligned<int>(ref bytes[0]);
      int mid = Unsafe.ReadUnaligned<int>(ref bytes[4]);
      // sign-bit weg
      int hi = Unsafe.ReadUnaligned<int>(ref bytes[8]) & 0x7FFFFFFF;

      result[i] = new decimal(lo, mid, hi, false, 0);
    }

    return result;
  }

  public static decimal[] RngCryptoDecimal(
    int size, decimal max) =>
      RngCryptoDecimal(size, 0m, max);

  public static decimal[] RngCryptoDecimal(int size, decimal min, decimal max)
  {
    ArgumentOutOfRangeException.ThrowIfNegative(min);
    ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(min, max);

    if (max == decimal.MaxValue)
      max = 9999999999999999999999999999m; // 10^28 - 1

    ////Wenn max = decimal.maxvalue, entsteht vielfach ein overflow.
    ////decimal limit = 9999999999999999999999999999m; // 10^28 - 1
    //if (max > 1_000_000_000_000_000_000_000_000_000m)
    //  throw new ArgumentOutOfRangeException(
    //    nameof(max), "max too large for decimal multiplication");

    var result = new decimal[size];
    Span<byte> bytes = stackalloc byte[12];

    for (int i = 0; i < size; i++)
    {
      CRand.GetBytes(bytes);

      int lo = Unsafe.ReadUnaligned<int>(ref bytes[0]);
      int mid = Unsafe.ReadUnaligned<int>(ref bytes[4]);
      int hi = Unsafe.ReadUnaligned<int>(ref bytes[8]) & 0x1FFFFFFF; // WICHTIG: 0x1FFFFFFF

      decimal rnd01 = new decimal(lo, mid, hi, false, 28); // jetzt wirklich 0–1

      result[i] = min + (max - min) * rnd01;
    }

    return result;
  }

  //public static decimal[] RngCryptoDecimal(
  //  int size, decimal min, decimal max)
  //{
  //  ArgumentOutOfRangeException.ThrowIfNegative(min);
  //  ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(min, max);

  //  if(max == decimal.MaxValue)
  //    max = 9999999999999999999999999999m; // 10^28 - 1

  //  ////Wenn max = decimal.maxvalue, entsteht vielfach ein overflow.
  //  ////decimal limit = 9999999999999999999999999999m; // 10^28 - 1
  //  //if (max > 1_000_000_000_000_000_000_000_000_000m)
  //  //  throw new ArgumentOutOfRangeException(
  //  //    nameof(max), "max too large for decimal multiplication");

  //  var result = new decimal[size];
  //  Span<byte> bytes = stackalloc byte[12];

  //  for (int i = 0; i < size; i++)
  //  {
  //    CRand.GetBytes(bytes);
  //    int lo = Unsafe.ReadUnaligned<int>(ref bytes[0]);
  //    int mid = Unsafe.ReadUnaligned<int>(ref bytes[4]);
  //    int hi = Unsafe.ReadUnaligned<int>(ref bytes[8]) & 0x7FFFFFFF;

  //    decimal rnd01 = new decimal(lo, mid, hi, false, 28); // 0–1

  //    result[i] = min + (max - min) * rnd01;
  //  }

  //  return result;
  //}


  //public static decimal[] RngCryptoDecimal(int size, decimal max)
  //{
  //  ArgumentOutOfRangeException.ThrowIfNegative(max);

  //  var result = new decimal[size];
  //  Span<byte> bytes = stackalloc byte[12];

  //  for (int i = 0; i < size; i++)
  //  {
  //    CRand.GetBytes(bytes);

  //    int lo = BitConverter.ToInt32(bytes[0..4]) & 0x7FFFFFFF;
  //    int mid = BitConverter.ToInt32(bytes[4..8]) & 0x7FFFFFFF;
  //    int hi = BitConverter.ToInt32(bytes[8..12]) & 0x1FFFFFFF;

  //    decimal rnd = new decimal(lo, mid, hi, false, 28);
  //    result[i] = max * rnd;
  //  }

  //  return result;
  //}


  //public static decimal[] RngCryptoDecimal(int size, decimal min, decimal max)
  //{
  //  ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(min, max);

  //  var result = new decimal[size];
  //  Span<byte> bytes = stackalloc byte[12];

  //  for (int i = 0; i < size; i++)
  //  {
  //    CRand.GetBytes(bytes);

  //    int lo = BitConverter.ToInt32(bytes[0..4]) & 0x7FFFFFFF;
  //    int mid = BitConverter.ToInt32(bytes[4..8]) & 0x7FFFFFFF;
  //    int hi = BitConverter.ToInt32(bytes[8..12]) & 0x1FFFFFFF;

  //    decimal rnd = new decimal(lo, mid, hi, false, 28);
  //    result[i] = min + (max - min) * rnd;
  //  }

  //  return result;
  //}


  public static bool NextCryptoBool()
  {
    var result = new byte[1];
    CRand.GetBytes(result);
    return (result.First() & 1) == 0;
  }

  public static bool[] RngCryptoBool(int size)
  {
    var bytes = new byte[(size + 7) / 8];
    CRand.GetNonZeroBytes(bytes);

    var result = new bool[size];
    int bitIndex = 0;

    foreach (var b in bytes)
      for (int i = 0; i < 8 && bitIndex < size; i++)
        result[bitIndex++] = ((b >> i) & 1) == 1;

    return result;
  }


  //public static bool[] RngCryptoBool(int size)
  //{
  //  var sz = (size - 1) / 32 + 1;

  //  var ints = RngCryptoIntX(
  //    sz, int.MinValue, int.MaxValue);
  //  var a = new BitArray(ints);
  //  var b = new bool[a.Length];
  //  a.CopyTo(b, 0);

  //  var result = new bool[size];
  //  Array.Copy(b, result, size);

  //  return result;
  //}

  //private static T[] RngCryptoIntX<T>(int size, T min, T max)
  //  where T : INumber<T>, IMinMaxValue<T>
  //{
  //  var tsize = Unsafe.SizeOf<T>();
  //  var result = new T[size];
  //  var buffer = new byte[size * tsize];
  //  var maxmin = double.CreateSaturating(max - min);
  //  var denominator = double.CreateSaturating(T.MaxValue) + 1.0;
  //  CRand.GetNonZeroBytes(buffer);
  //  for (var i = 0; i < result.Length; i++)
  //  {
  //    Buffer.BlockCopy(buffer, i * tsize, result, i, tsize);
  //    var tmp = double.Abs(double.CreateSaturating(result[i]) / denominator);
  //    tmp *= maxmin; tmp += double.CreateSaturating(min);
  //    result[i] = T.Abs(T.CreateSaturating(tmp));
  //  }
  //  return result;
  //}

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private static T[] RngCryptoIntX<T>(int size, T min, T max)
  where T : unmanaged, INumber<T>, IMinMaxValue<T>, INumberBase<T>
  {
    var result = new T[size];
    RngCryptoIntX(result, min, max);
    return result;
  }


  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private static void RngCryptoIntX<T>(T[] ints, T min, T max)
      where T : unmanaged, INumber<T>, IMinMaxValue<T>, INumberBase<T>
  {
    if (IsNullOrEmpty(ints)) return;
    ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(min, max);

    T range = max - min;
    int size = Unsafe.SizeOf<T>();
    Span<byte> buffer = stackalloc byte[size];
    for (int i = 0; i < ints.Length; i++)
    {
      T value;
      while (true)
      {
        CRand.GetNonZeroBytes(buffer);
        switch (size)
        {
          case 1:
            {
              byte u = buffer[0];
              byte r = (byte)(u % byte.CreateChecked(range));
              value = T.CreateChecked(r);
              break;
            }

          case 2:
            {
              ushort u = MemoryMarshal.Read<ushort>(buffer);
              ushort r = (ushort)(u % ushort.CreateChecked(range));
              value = T.CreateChecked(r);
              break;
            }

          case 4:
            {
              uint u = MemoryMarshal.Read<uint>(buffer);
              uint r = u % uint.CreateChecked(range);
              value = T.CreateChecked(r);
              break;
            }

          case 8:
            {
              ulong u = MemoryMarshal.Read<ulong>(buffer);
              ulong r = u % ulong.CreateChecked(range);
              value = T.CreateChecked(r);
              break;
            }

          default:
            throw new NotSupportedException($"Unsupported integer size {size}");
        }

        // value ∈ [0, range)
        // kein Rejection nötig, wenn Mod-Bias ok ist
        break;
      }

      ints[i] = value + min;
    }
  }



  //[MethodImpl(MethodImplOptions.AggressiveInlining)]
  //private static void RngCryptoIntX<T>(T[] ints, T min, T max)
  //    where T : INumber<T>, IMinMaxValue<T>, INumberBase<T>
  //{

  //  //Another option that is even slightly more powerful.
  //  //https://github.com/michelenatale/Cryptography/tree/main/CryptoRandom

  //  if (IsNullOrEmpty(ints)) return;
  //  ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(min, max);

  //  var d = max - min;
  //  var maxv = T.MaxValue;
  //  var length = ints.Length;
  //  var tsize = Unsafe.SizeOf<T>();
  //  var bytes = new byte[tsize * length];
  //  CRand.GetNonZeroBytes(bytes);
  //  //DataTypes Int128 and UInt128 are not yet recognized as primitives. 
  //  if (typeof(T).IsPrimitive || typeof(T) == typeof(Int128) || typeof(T) == typeof(UInt128))
  //    for (int i = 0; i < length; i++)
  //    {
  //      var tmp = Unsafe.ReadUnaligned<T>(ref bytes[i * tsize]);
  //      if (tmp == T.MinValue) tmp++;
  //      if (T.Sign(tmp) < 0) tmp += maxv;
  //      ints[i] = tmp; ints[i] %= d; ints[i] += min;
  //    }
  //}

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private static bool IsNullOrEmpty<T>(T[] ints)
    where T : INumber<T>, INumberBase<T>
  {
    if (ints is null) return true;
    return ints.Length == 0;
  }




  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private static float Lerp(float first, float second, float by) =>
     //https://ask.godotengine.org/88479/how-to-use-lerp-functions-in-c%23
     first * (1f - by) + second * by;
}






//[MethodImpl(MethodImplOptions.AggressiveInlining)]
//private static void RngCryptoIntX<T>(T[] ints, T min, T max)
//  where T : unmanaged, INumber<T>, IMinMaxValue<T>, INumberBase<T>
//{
//  if (IsNullOrEmpty(ints)) return;
//  ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(min, max);

//  // Bereichgrösse
//  var d = max - min;
//  var length = ints.Length;
//  var tsize = Unsafe.SizeOf<T>();
//  var bytes = new byte[tsize * length];


//  if (typeof(T).IsPrimitive || typeof(T) == typeof(Int128) || typeof(T) == typeof(UInt128))
//  {
//    for (int i = 0; i < length; i++)
//    {
//      CRand.GetNonZeroBytes(bytes);
//      var tmp = Unsafe.ReadUnaligned<T>(ref bytes[i * tsize]);

//      // Verhindern von Minimalwerten, die Probleme verursachen könnten
//      if (tmp == T.MinValue) tmp++;

//      // Um Bias zu vermeiden, verwenden wir Rejection Sampling:
//      // Berechne die maximale zulässige Zufallszahl, die sich
//      // gleichmaessig auf d verteilen lässt.
//      //T.Floor(T.Divide(T.MaxValue, d)) * d;
//      var maxAcceptable = (T.MaxValue / d) * d;
//      if (tmp > maxAcceptable)
//      {
//        i--; continue;
//      }

//      // Wiederhole, bis tmp im gültigen Bereich liegt
//      // (vereinfachte Implementierung:
//      // solange tmp > maxAcceptable, erneut lesen)
//      // Beachte: Für vollständige Generic-Implementierung
//      // ggf. eine robuste Schleife einbauen.
//      while (tmp < min || tmp >= max)
//      {
//        // Neue zufällige Bytes lesen, erneute Umwandlung
//        CRand.GetNonZeroBytes(bytes);
//        tmp = Unsafe.ReadUnaligned<T>(ref bytes[i * tsize]);
//        if (tmp == T.MinValue) tmp++;
//      }

//      // Endgültige Zuordnung
//      var adjusted = T.Abs(tmp);
//      // modulo d
//      var moduloed = adjusted % d;
//      ints[i] = min + moduloed;
//    }
//  }
//}
