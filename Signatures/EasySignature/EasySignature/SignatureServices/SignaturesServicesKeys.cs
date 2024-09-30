

namespace michele.natale.Cryptography.Signatures.Services;


partial class SignatureServices
{

  public static byte[] Xor(
    ReadOnlySpan<byte> left, ReadOnlySpan<byte> right)
  {
    var length = Math.Max(left.Length, right.Length);

    var result = new byte[length];
    for (var i = 0; i < length; i++)
      result[i % result.Length] = (byte)(left[i % left.Length] ^ right[i % right.Length]);

    return result;
  }

  public static byte[] Xor(
    ReadOnlySpan<byte> left, ReadOnlySpan<byte> right, int size)
  {
    var result = new byte[size];
    var length = Math.Max(left.Length, right.Length);
    for (var i = 0; i < length; i++)
    {
      var tmp = (byte)(left[i % left.Length] ^ right[i % right.Length]);
      if (tmp != 0) result[i % result.Length] = tmp;
      else result[i % result.Length] = left[i];
    }

    return result;
  }

  /// <summary>
  /// Clears the contents of all arrays.
  /// </summary>
  /// <param name="input"></param>
  public static void MemoryClear(params byte[][] input)
  {
    for (int i = 0; i < input.Length; i++)
      Array.Clear(input[i]);
  }
}
