

using System.Numerics;
using System.Runtime.CompilerServices;

namespace michele.natale.BitsBytesUtils;


partial class BitsBytesUtils
{

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static int LeadingZerosCount<T>(T number)
    where T : INumber<T>, IBinaryInteger<T>
  {
    var sz = Unsafe.SizeOf<T>();
    if (T.IsZero(number)) return 8 * sz;
    if (T.One == number) return 8 * sz - 1;

    var result = 0;
    var bsz = 8 * sz;
    for (var i = 0; i < bsz; i++)
    {
      if ((number & (T.One << (bsz - i - 1))) == T.Zero)
        result++;
      else break;
    }

    return result;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static int LeadingZerosCount(BigInteger number)
  {
    //Caution
    //Here the size.size is determined based on the current number.
    //Make sure that the number is within the desired range.
    var blength = BitLength(number, out int tsz);
    if (number.IsZero) return tsz;
    if (number.IsOne) return tsz - 1;

    return tsz - blength;
  }


  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static int LeadingZerosCount(
    BigInteger number, out int tsz)
  {
    tsz = -1;

    var blength = BitLength(number, out tsz);
    if (number.IsZero) return tsz;
    if (number.IsOne) return tsz - 1;

    return tsz - blength;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static int LeadingZerosCount(
    BigInteger number, int unsigned_range_size)
  {
    if (number.IsZero) return unsigned_range_size;
    if (number.IsOne) return unsigned_range_size - 1;
    var blength = BitLength(number, unsigned_range_size);

    return unsigned_range_size - blength;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static long LeadingZerosCount<T>(
    ReadOnlySpan<T> number, bool isbigendian)
      where T : INumber<T>, IBinaryInteger<T>
  {
    if (isbigendian)
      return LeadingZerosCountBE(number);
    return LeadingZerosCountLE(number);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static long LeadingZerosCountLE<T>(
    ReadOnlySpan<T> number)
      where T : INumber<T>, IBinaryInteger<T>
  {
    var zeros = 0;
    var size = number.Length;
    var sz = Unsafe.SizeOf<T>();
    while (number[^(zeros++ + 1)] == T.Zero) ;
    var additional = --zeros * 8 * sz;
    return LeadingZerosCount(number[size - zeros - 1]) + additional;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static long LeadingZerosCountBE<T>(
    ReadOnlySpan<T> number)
      where T : INumber<T>, IBinaryInteger<T>
  {
    var zeros = 0;
    var sz = Unsafe.SizeOf<T>();
    while (number[zeros++] == T.Zero) ;
    var additional = --zeros * 8 * sz;
    return LeadingZerosCount(number[zeros]) + additional;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static int LeadingZerosCountString(string str)
  {
    if (string.IsNullOrEmpty(str)) return 0;

    var idx = 0;
    while (str[idx++] == '0') ; idx--;
    return idx;
  }
}
