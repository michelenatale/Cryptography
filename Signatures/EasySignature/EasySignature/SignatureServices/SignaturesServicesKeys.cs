 

namespace michele.natale.Cryptography.Signatures.Services;


partial class SignatureServices
{

  //public static byte[] ToNewKey(ReadOnlySpan<byte> seed, int size)
  //{
  //  if (seed.Length < 8)
  //    throw new ArgumentOutOfRangeException(nameof(seed),
  //    $"{nameof(seed.Length)} >= 8 ?");

  //  var s = seed.ToArray();
  //  var sum = BitConverter.GetBytes(s.Sum(x => x));
  //  if (BitConverter.IsLittleEndian) Array.Reverse(sum);

  //  var h1 = SHA512.HashData(s.Concat(sum).ToArray());
  //  var h2 = SHA512.HashData(s.Reverse().Concat(sum).ToArray());

  //  byte[] result;
  //  if (size < 58)
  //    result = ToNewKey(h1, h2, 0, size);
  //  else result = HMACSHA512.HashData(h2, h1);

  //  MemoryClear(s, sum, h1, h2);
  //  return result;
  //}

  //private static byte[] ToNewKey(byte[] key, byte[] bytes, int offset, int size)
  //{
  //  if (size < 8 || size > 64)
  //    throw new ArgumentOutOfRangeException(nameof(size),
  //      $"{nameof(size)} >= 8 and  {nameof(size)} <= 64");
  //  using var hmac = new HMACSHA512(key);
  //  var hash = hmac.ComputeHash(bytes, offset, bytes.Length - offset);
  //  if (size == 64) return hash;
  //  if (size == 63) return hash.Skip(1).ToArray();
  //  var start = (hash.Sum(x => x) % (64 - size - 1)) + 1;
  //  return hash.Skip(start).Take(size).ToArray();
  //}

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
