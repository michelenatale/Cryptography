


using System.Security.Cryptography;

namespace michele.natale.Services;


partial class BcPqcServices
{

  internal static byte[] Sha512Concat(byte[][] bytes)
  {
    var cnt = bytes.Length;
    var length = bytes.Sum(x => x.Length);
    var result = new byte[cnt * 64];
    for (var i = 0; i < cnt; i++)
      SHA512.HashData(bytes[i]).CopyTo(result, i * 64);

    return result;
  }

  internal static byte[] XorSpec(byte[][] bytes)
  {
    var cnt = bytes.Length;
    if (cnt < 1) return [];
    if (cnt == 1) return bytes.First();

    var length = bytes.Max(x => x.Length);
    var result = new byte[length];
    for (var i = 1; i < cnt; i++)
      result = Xor_Spec(result, Xor_Spec(bytes[i - 1], bytes[i]));

    return result;
  }

  private static byte[] Xor_Spec(ReadOnlySpan<byte> left, ReadOnlySpan<byte> right)
  {
    var length = Math.Max(left.Length, right.Length);

    var result = new byte[length];
    for (var i = 0; i < length; i++)
      result[i % result.Length] = (byte)(left[i % left.Length] ^ right[i % right.Length]);

    return result;
  }

  //private static byte[] Xor(byte[][] input)
  //{
  //  var length = input.First().Length;
  //  if (!input.All(x => length == x.Length))
  //    throw new ArgumentOutOfRangeException(
  //      $"{nameof(input)}.Length has failed!");

  //  var result = new byte[length];
  //  foreach (var itm in input)
  //    for (var i = 0; i < length; i++)
  //      result[i] ^= itm[i];

  //  return result;
  //}
}
