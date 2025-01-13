

using System.Text;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace michele.natale.BitsBytesUtils;


public static class ExtensionsHelper
{

  public static int TypeSize<T>(this T number) where T : INumber<T> => Unsafe.SizeOf<T>();
  public static int TypeSize<T>(this Span<T> input) where T : INumber<T> => Unsafe.SizeOf<T>();
  public static int TypeSize<T>(this ReadOnlySpan<T> input) where T : INumber<T> => Unsafe.SizeOf<T>();

  public static T ToT<T>(this ReadOnlySpan<byte> bytes) where T : INumber<T>
  {
    if (bytes.Length < Unsafe.SizeOf<T>())
      throw new ArgumentOutOfRangeException(nameof(bytes),
        $"{nameof(bytes)}.Length has failed! Length = {Unsafe.SizeOf<T>()}");

    return Unsafe.ReadUnaligned<T>(ref MemoryMarshal.GetReference(bytes));
  }



  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static string ToBitString(this BigInteger number)
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


}
