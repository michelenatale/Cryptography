

using System.Security.Cryptography;

namespace michele.natale;

using Pointers;

partial class NetServices
{
  #region AES Asserts
  private static void AssertAesEnc(
    ReadOnlySpan<byte> bytes,
    UsIPtr<byte> key)
  {
    if ((bytes.Length < AES_MIN_PLAIN_SIZE) || (bytes.Length > AES_MAX_PLAIN_SIZE))
      throw new ArgumentOutOfRangeException(nameof(bytes));

    if (key.Length != AES_KEY_SIZE)
      throw new ArgumentOutOfRangeException(nameof(key));
  }

  private static void AssertAesDec(UsIPtr<byte> key)
  {
    if (key.Length != AES_KEY_SIZE)
      throw new ArgumentOutOfRangeException(nameof(key));
  }

  private static void AssertAesEnc(
    string src, string dest,
    UsIPtr<byte> key)
  {
    if (!File.Exists(src))
      throw new FileNotFoundException(nameof(src));

    if (File.Exists(dest))
      File.Delete(dest);

    if (key.Length != AES_KEY_SIZE)
      throw new ArgumentOutOfRangeException(nameof(key));
  }

  private static void AssertAesDec(
    string src, string dest, UsIPtr<byte> key)
  {
    AssertAesDec(key);

    if (!File.Exists(src))
      throw new FileNotFoundException(nameof(src));

    if (File.Exists(dest))
      File.Delete(dest);
  }


  private static void AssertAesEnc(
     FileStream fsin, FileStream fsout, UsIPtr<byte> key,
     int startin, int lengthin, int startout)
  {
    if (key.Length != AES_KEY_SIZE)
      throw new ArgumentOutOfRangeException(nameof(key));

    //using var fsin = new FileStream(src, FileMode.Open, FileAccess.Read);
    //using var fsout = new FileStream(dest, FileMode.Create, FileAccess.ReadWrite);

    ArgumentOutOfRangeException.ThrowIfNegative(startout);

    if (startin + lengthin > fsin.Length)
      throw new ArgumentOutOfRangeException(nameof(startin));

    if (!fsin.CanRead)
      throw new ArgumentException(
        $"Stream must CanRead, has failed!", nameof(fsin));

    if (!fsout.CanRead) throw new ArgumentException(
        $"Stream must CanRead, has failed!", nameof(fsout));

    if (!fsout.CanWrite) throw new ArgumentException(
        $"Stream must Canwrite, has failed!", nameof(fsout));

  }

  private static void AssertAesDec(
     FileStream fsin, FileStream fsout, UsIPtr<byte> key,
     int startin, int lengthin, int startout)
  {
    if (key.Length != AES_KEY_SIZE)
      throw new ArgumentOutOfRangeException(nameof(key));

    ArgumentOutOfRangeException.ThrowIfNegative(startout);

    if (startin + lengthin > fsin.Length)
      throw new ArgumentOutOfRangeException(nameof(startin));

    if (!fsin.CanRead)
      throw new ArgumentException(
        $"Stream must CanRead, has failed!", nameof(fsin));

    //if (!fsout.CanRead) throw new ArgumentException(
    //    $"Stream must CanRead, has failed!", nameof(fsout));

    if (!fsout.CanWrite) throw new ArgumentException(
        $"Stream must Canwrite, has failed!", nameof(fsout));
  }

  #endregion AES Asserts

  #region AES Utils
  private static byte[] ToTag(
    byte[] cipher, byte[] key, byte[] assosiat)
  {
    int ts = AES_TAG_SIZE, ks = AES_KEY_SIZE;
    var start = (key.Sum(x => x) % (ks - ts - 1)) + 1;
    var k = SHA256.HashData([.. key.Skip(start).Take(ts)]);

    var src = MD5.HashData(cipher).Concat(assosiat).ToArray();
    var hash = HMACSHA512.HashData(k, src);

    start = (hash.Sum(x => x) % (hash.Length - ts - 1)) + 1;
    return [.. hash.Skip(start).Take(ts)];
  }


  //private static byte[] ToTag(
  //  Stream strm, byte[] key, byte[] assosiat, int pos = 0)
  //{
  //  int ts = AES_TAG_SIZE, ks = AES_KEY_SIZE;
  //  var start = (key.Sum(x => x) % (ks - ts - 1)) + 1;
  //  var k = SHA256.HashData([.. key.Skip(start).Take(ts)]);

  //  strm.Position = pos;
  //  var src = MD5.HashData(strm).Concat(assosiat).ToArray();
  //  var hash = HMACSHA512.HashData(k, src);

  //  start = (hash.Sum(x => x) % (hash.Length - ts - 1)) + 1;
  //  return [.. hash.Skip(start).Take(ts)];
  //}

  private static async Task<byte[]> ToTagAsync(
    Stream strm, byte[] key, byte[] assosiat, int pos = 0)
  {
    int ts = AES_TAG_SIZE, ks = AES_KEY_SIZE;
    var start = (key.Sum(x => x) % (ks - ts - 1)) + 1;
    var k = SHA256.HashData([.. key.Skip(start).Take(ts)]);

    int read;
    strm.Position = pos;
    using var md5 = MD5.Create();
    var buffer = new byte[8192];

    while ((read = await strm.ReadAsync(buffer.AsMemory(0, buffer.Length))) > 0)
      md5.TransformBlock(buffer, 0, read, null, 0);

    md5.TransformFinalBlock([], 0, 0);
    var src = md5.Hash!.Concat(assosiat).ToArray();
    var hash = HMACSHA512.HashData(k, src);
    start = (hash.Sum(x => x) % (hash.Length - ts - 1)) + 1;

    return [.. hash.Skip(start).Take(ts)];
  }


  private static bool Verify(
    byte[] cipher, byte[] key, byte[] tag, byte[] assosiat) =>
      tag.SequenceEqual(ToTag(cipher, key, assosiat));

  //private static bool Verify(
  //  Stream cipher, byte[] key, byte[] tag, byte[] assosiat, int pos = 0) =>
  //    tag.SequenceEqual(ToTagAsync(cipher, key, assosiat, pos));

  private static async Task<bool> VerifyAsync(
    Stream cipher, byte[] key, byte[] tag, byte[] assosiat, int pos = 0)
  {
    var calc = await ToTagAsync(cipher, key, assosiat, pos);
    return tag.AsSpan().SequenceEqual(calc);
  }


  #endregion AES Utils
}
