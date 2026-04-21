

using System.Numerics;
using System.Runtime.CompilerServices; 


namespace michele.natale;

partial class NetServicesUtils
{

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static byte[] Converter(
    ReadOnlySpan<byte> bytes, int startbase, int targetbase)
  {
    if (bytes.Length == 0) return new byte[1];
    var cap = Convert.ToInt32(bytes.Length *
      Math.Log(startbase) / Math.Log(targetbase)) + 1;
    var result = new Stack<byte>(cap);

    var ext = true;
    var length = bytes.Length;
    var input = bytes.ToArray();
    byte remainder, accumulator;
    while (ext)
    {
      remainder = 0; ext = false;
      for (var i = 0; i < length; i++)
      {
        accumulator = (byte)((startbase * remainder + input[i]) % targetbase);
        input[i] = (byte)((startbase * remainder + input[i]) / targetbase);
        remainder = accumulator;
        if (input[i] > 0) ext = true;
      }
      result.Push(remainder);
    }

    return [.. result];
  }


  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static byte[] FromBase_X(ReadOnlySpan<byte> bytes, int basex)
  {
    //From Base X to Base 10
    if (basex == 10) return [.. bytes.ToArray()];

    var bi = BigInteger.Zero;
    var length = bytes.Length;
    for (var i = 0; i < length; i++)
      bi += bytes[^(1 + i)] * BigInteger.Pow(basex, i);

    return [.. bi.ToString().Select(x => (byte)(x - 48))];
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static byte[] ToBase_X(ReadOnlySpan<byte> bytes_base10, int basex) =>
    ToBaseX(BigInteger.Parse(string.Join("", bytes_base10.ToArray())), basex);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static byte[] ToBaseX(BigInteger base10, int basex)
  {
    //From Base 10 to Base X (Reverse !)
    if (basex == 10) return [.. base10.ToString().Select(x => (byte)(x - 48))];

    var tmp = new Stack<byte>();
    while (base10 != 0)
    {
      tmp.Push((byte)(base10 % basex));
      base10 /= basex;
    }

    var result = tmp.ToArray();
    return result;
  }

}
