

using System.Text;
using System.Security.Cryptography;


namespace michele.natale.EcCurveDsaDh;

partial class EcService
{
  #region Create New Keys
  public static byte[] RngNewKey(
    ReadOnlySpan<byte> key, ReadOnlySpan<byte> bytes,
    int offset, int size)
  {
    if (size < 8 || size > 64)
      throw new ArgumentOutOfRangeException(nameof(size),
        $"{nameof(size)} >= 8 and  {nameof(size)} <= 64");
    using var hmac = new HMACSHA512(key.ToArray());
    var hash = hmac.ComputeHash(bytes.ToArray(), offset, bytes.Length - offset);
    if (size == 64) return hash;
    if (size == 63) return hash.Skip(1).ToArray();
    var start = (hash.Sum(x => x) % (64 - size - 1)) + 1;
    return hash.Skip(start).Take(size).ToArray();
  }
  #endregion Create New Keys

  #region Associated Suggestion
  private static byte[] ToAssociated(ReadOnlySpan<byte> associat, ReadOnlySpan<byte> key)
  {
    var a = associat.IsEmpty ? AssociatedSuggestion : MD5.HashData(associat.ToArray());
    var k = MD5.HashData(a.Concat(SHA256.HashData(key)).ToArray());
    return RngNewKey(k, a, 0, 32);
  }

  private readonly static byte[] AssociatedSuggestion =
   MD5.HashData(Encoding.UTF8.GetBytes("EcCurveDsaDhEngine"));
  #endregion Associated Suggestion

  #region Resets Byte Arrays
  private static void ResetBytes(params byte[][] bytes)
  {
    for (var i = 0; i < bytes.Length; i++)
      Array.Clear(bytes[i]);
  }
  #endregion Resets Byte Arrays 
}
