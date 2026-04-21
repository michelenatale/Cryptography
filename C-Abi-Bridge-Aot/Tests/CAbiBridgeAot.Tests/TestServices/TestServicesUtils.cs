

using System.Numerics;
using System.Runtime.CompilerServices;


namespace michele.natale.Tests;

partial class TestServices
{

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool IsNullOrEmpty<T>(T[] ints)
    where T : INumber<T>, INumberBase<T>
  {
    if (ints is null) return true;
    return ints.Length == 0;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static byte[] TrimLast(ReadOnlySpan<byte> bytes)
  {
    var idx = 0;
    while (bytes[^(1 + idx++)] == 0) ;
    idx--;

    var length = bytes.Length;
    return bytes[..(length - idx)].ToArray();
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static byte[] TrimFirst(ReadOnlySpan<byte> bytes)
  {
    var idx = 0;
    while (bytes[idx++] == 0) ;
    idx--;
 
    return bytes[idx..].ToArray();
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static byte[] Trim(ReadOnlySpan<byte> bytes) =>   
    TrimLast(TrimFirst(bytes));
   
}
