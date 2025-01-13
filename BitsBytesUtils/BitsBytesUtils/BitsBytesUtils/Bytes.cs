

using System.Numerics;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;


namespace michele.natale.BitsBytesUtils;


partial class BitsBytesUtils
{

  public static byte[] ToBytes<T>(T number)
    where T : INumber<T>, IBinaryInteger<T>
  {
    var result = new byte[Unsafe.SizeOf<T>()];
    Unsafe.As<byte, T>(ref result[0]) = number;
    return result;
  }

  public static byte[][] ToBytes<T>(ReadOnlySpan<T> numbers)
    where T : INumber<T>, IBinaryInteger<T> => 
      ToBytes(numbers, 0, numbers.Length);

  public static byte[][] ToBytes<T>(
    ReadOnlySpan<T> numbers, int startidx)
      where T : INumber<T>, IBinaryInteger<T> =>
        ToBytes(numbers, startidx, numbers.Length - startidx);

  public static byte[][] ToBytes<T>(
    ReadOnlySpan<T> numbers, int startidx, int length)
      where T : INumber<T>, IBinaryInteger<T>
  {
    if (numbers.Length - startidx - length < 0)
      throw new ArgumentOutOfRangeException(nameof(numbers),
        $"{nameof(numbers)}.Length or {nameof(startidx)} or {nameof(length)} has failed!");

    var result = new byte[length][];
    for (var i = 0; i < length; i++)
      result[i] = ToBytes(numbers[i + startidx]);
    return result;
  }

  public static byte[][] ToBytes(
    ReadOnlySpan<BigInteger> numbers)
  {
    var length = numbers.Length;
    var result = new byte[length][];
    for (var i = 0; i < length; i++)
      result[i] = numbers[i].ToByteArray();
    return result;
  }

  public static T FromBytes<T>(ReadOnlySpan<byte> bytes)
    where T : INumber<T>
  {
    if (bytes.Length != Unsafe.SizeOf<T>())
      throw new ArgumentOutOfRangeException(nameof(bytes));

    return Unsafe.ReadUnaligned<T>(
      ref MemoryMarshal.GetReference(bytes));
  }

  public static T FromBytes<T>(
    ReadOnlySpan<byte> bytes, int startidx)
      where T : INumber<T> =>
        FromBytes<T>(bytes, startidx, Unsafe.SizeOf<T>());

  public static T FromBytes<T>(
    ReadOnlySpan<byte> bytes, int startidx, int length)
      where T : INumber<T>
  {
    if (bytes.Length - startidx - length < 0)
      throw new ArgumentOutOfRangeException(nameof(bytes),
        $"{nameof(bytes)}.Length or {nameof(startidx)} or {nameof(length)} has failed!");

    return Unsafe.ReadUnaligned<T>(
      ref MemoryMarshal.GetReference(
        bytes.Slice(startidx, length)));
  }

  public static T[] FromBytes<T>(ReadOnlySpan<byte[]> bytes)
   where T : INumber<T>
  {
    var length = bytes.Length;
    var result = new T[length];
    for (int i = 0; i < length; i++)
      result[i] = FromBytes<T>(bytes[i]);
    return result;
  }

  public static BigInteger[] FromBytesToBigIntegers(
    ReadOnlySpan<byte[]> bytes)
  {
    var length = bytes.Length;
    var result = new BigInteger[length];
    for (int i = 0; i < length; i++)
      result[i] = new BigInteger(bytes[i]);
    return result;
  }
}
