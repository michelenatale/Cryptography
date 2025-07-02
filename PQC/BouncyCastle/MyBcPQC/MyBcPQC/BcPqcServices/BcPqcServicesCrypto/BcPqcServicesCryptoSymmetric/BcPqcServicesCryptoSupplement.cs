

using System.Security.Cryptography;
using System.Text;


namespace michele.natale.Services;

using Pointers;

partial class BcPqcServices
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
    if (size == 63) return [.. hash.Skip(1)];
    var start = (hash.Sum(x => x) % (64 - size - 1)) + 1;
    return [.. hash.Skip(start).Take(size)];
  }
  #endregion Create New Keys

  #region Associated Suggestion
  private static byte[] ToAssociated(ReadOnlySpan<byte> associat, UsIPtr<byte> key)
  {
    var a = associat.IsEmpty ? AssociatedSuggestion : MD5.HashData(associat.ToArray());
    var k = MD5.HashData([.. a, .. SHA256.HashData(key.ToArray())]);
    return RngNewKey(k, a, 0, 32);
  }

  private readonly static byte[] AssociatedSuggestion =
    MD5.HashData(Encoding.UTF8.GetBytes("MyBcPQCEngine"));
  #endregion Associated Suggestion

  #region Resets Byte Arrays
  private static void ResetBytes(params byte[][] bytes)
  {
    for (var i = 0; i < bytes.Length; i++)
      Array.Clear(bytes[i]);
  }

  public static void MemoryClear(params byte[][] bytes)
  {
    foreach (var itm in bytes)
      Array.Clear(itm);
  }
  #endregion Resets Byte Arrays 

  #region File Equality

  public static bool FileEquals(string leftfile, string rightfile)
  {
    using var fsleft = new FileStream(leftfile, FileMode.Open, FileAccess.Read);
    using var fsright = new FileStream(rightfile, FileMode.Open, FileAccess.Read);
    return SHA256.HashData(fsleft).SequenceEqual(SHA256.HashData(fsright));
  }
  #endregion File Equality
}
