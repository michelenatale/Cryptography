


using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

namespace michele.natale.BitsBytesUtils;


partial class BitsBytesUtils
{

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static string ToBitStr<T>(T number)
    where T : INumber<T>, IBinaryInteger<T>
  {
    //In .Net8.0 not possible! Why ??
    //return number.ToString("B")!;

    if (T.IsZero(number)) return "0";
    if (number == T.One) return "01";

    var idx = 0;
    var bytes = ToBytes(number);
    while (bytes[^(1 + idx++)] == 0) ; idx--;

    var sign = T.IsPositive(number) ? 1 : -1;
    var result = new StringBuilder(bytes.Length * 8);
    var bits = Convert.ToString(bytes[^(1 + idx)], 2);
    if (bits.First() != '0' && sign == 1)
      result.Append('0');

    result.Append(bits);
    idx = bytes.Length - idx - 1;
    for (idx--; idx >= 0; idx--)
      result.Append(Convert.ToString(bytes[idx], 2).PadLeft(8, '0'));

    return result.ToString();
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static byte[] ToBitBytes<T>(T number)
    where T : INumber<T>, IBinaryInteger<T> =>
      ToBitStr(number).Select(x => x == '1' ? (byte)1 : byte.MinValue).ToArray();



  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static string ToBitStr(BigInteger number)
  {
    if (number == -1) return "1";
    if (number.IsZero) return "0";
    if (number.IsOne) return "01";

    var bytes = number.ToByteArray();
    var idx = bytes.Length - 1;

    var result = new StringBuilder(bytes.Length * 8);
    var bits = Convert.ToString(bytes[idx], 2);
    if (bits.First() != '0' && number.Sign == 1)
      result.Append('0');

    result.Append(bits);
    for (idx--; idx >= 0; idx--)
      result.Append(Convert.ToString(bytes[idx], 2).PadLeft(8, '0'));

    return result.ToString();
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static byte[] ToBitBytes(BigInteger number) =>
      ToBitStr(number).Select(x => x == '1' ? (byte)1 : byte.MinValue).ToArray();



  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static T FromBitString<T>(string bits)
    where T : INumber<T>, IBinaryInteger<T>
  {
    ReadOnlySpan<char> src = bits;
    return FromBitString<T>(src);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static BigInteger FromBitStringToBigInteger(string bits)
  {
    ReadOnlySpan<char> src = bits;
    return FromBitStringToBigInteger(src);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static T FromBitString<T>(ReadOnlySpan<char> bits)
    where T : INumber<T>, IBinaryInteger<T>
  {
    var bools = bits.ToArray().Select(x => x == '1').Reverse();
    return FromBits<T>(bools.ToArray());
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static BigInteger FromBitStringToBigInteger(ReadOnlySpan<char> bits)
  {
    var bools = bits.ToArray().Select(x => x == '1').Reverse();
    return FromBitsToBigInteger(bools.ToArray());
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static T FromBitBytes<T>(ReadOnlySpan<byte> bits)
    where T : INumber<T>, IBinaryInteger<T>
  {
    var bools = bits.ToArray().Select(x => x == 1).Reverse();
    return FromBits<T>(bools.ToArray());
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static BigInteger FromBitBytesToBigInteger(ReadOnlySpan<byte> bits)
  {
    var bools = bits.ToArray().Select(x => x == 1).Reverse();
    return FromBitsToBigInteger(bools.ToArray());
  }
}
