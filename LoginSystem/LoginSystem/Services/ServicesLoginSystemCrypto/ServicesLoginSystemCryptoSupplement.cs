
using System.Security.Cryptography;
using System.Text;

namespace michele.natale.LoginSystems.Services;

partial class AppServices
{
  #region Create New Keys

  /// <summary>
  /// Create a new Key with the GUID-Methode
  /// </summary>
  /// <param name="bytes"></param>
  /// <param name="offset"></param>
  /// <param name="size"></param>
  /// <returns></returns>
  public (byte[] Key, byte[] Result) RngNewKeyGuid(
    ReadOnlySpan<byte> bytes, int offset, int size)
  {
    var key = Guid.NewGuid().ToByteArray();
    var result = this.RngNewKey(key, bytes, offset, size);
    return (key, result);
  }

  /// <summary>
  /// Create a new Key with the Hash-Algorithm
  /// </summary>
  /// <param name="key"></param>
  /// <param name="bytes"></param>
  /// <param name="offset"></param>
  /// <param name="size"></param>
  /// <returns></returns>
  /// <exception cref="ArgumentOutOfRangeException"></exception>
  public byte[] RngNewKey(
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

  /// <summary>
  /// Create with the Assosiated-Argumens e new Assosiated.
  /// </summary>
  /// <param name="associat">Assosiated</param>
  /// <param name="key">Key</param>
  /// <returns></returns>
  private byte[] ToAssociated(ReadOnlySpan<byte> associat, ReadOnlySpan<byte> key)
  {
    var a = associat.IsEmpty ? AssociatedSuggestion : MD5.HashData(associat.ToArray());
    var k = MD5.HashData(a.Concat(SHA256.HashData(key)).ToArray());
    return this.RngNewKey(k, a, 0, 32);
  }

  /// <summary>
  /// Additional Assosiated Methode
  /// </summary>
  private readonly static byte[] AssociatedSuggestion =
   MD5.HashData(Encoding.UTF8.GetBytes("Login_System_Engine"));
  #endregion Associated Suggestion


}
