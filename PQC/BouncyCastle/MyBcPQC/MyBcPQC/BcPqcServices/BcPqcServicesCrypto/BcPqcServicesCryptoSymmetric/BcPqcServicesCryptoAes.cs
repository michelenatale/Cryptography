
using System.Diagnostics;
using System.Security.Cryptography;

namespace michele.natale.Services;

using Pointers;

partial class BcPqcServices
{

  //um Timing Attacts entgegenzuwirken.
  private const long TimeSleep = 120; //ms
  private static readonly bool TimeLoc = true;

  public const int AES_IV_SIZE = 16;
  public const int AES_TAG_SIZE = 16;
  public const int AES_KEY_SIZE = 32;
  public const int AES_MIN_PLAIN_SIZE = 8;
  public const int AES_MAX_PLAIN_SIZE = 1024 * 1024;

  #region AES En- & Decryption
  public static byte[] EncryptionAes(
    ReadOnlySpan<byte> bytes,
    UsIPtr<byte> key,
    ReadOnlySpan<byte> associated)
  {
    AssertAesEnc(bytes, key);
    var iv = RngCryptoBytes(AES_IV_SIZE);
    var associat = ToAssociated(associated, key);

    var sw = Stopwatch.StartNew();
    var cipher = EncryptionAesSingle(
      bytes.ToArray(), key.ToArray(), iv, associat);

    var tag = ToTag(cipher, key.ToArray(), associat);
    var length = cipher.Length + AES_TAG_SIZE + AES_IV_SIZE;
    var result = new byte[length];

    Array.Copy(tag, result, tag.Length);
    Array.Copy(iv, 0, result, tag.Length, iv.Length);
    Array.Copy(cipher, 0, result, tag.Length + iv.Length, cipher.Length);
    MemoryClear(cipher, tag, iv, associat);

    var deltatime = (int)(TimeSleep - sw.ElapsedMilliseconds);
    if (TimeLoc)
      if (deltatime > 0)
        Thread.Sleep(deltatime);

    return result;
  }

  public static byte[] DecryptionAes(
   ReadOnlySpan<byte> bytes,
   UsIPtr<byte> key,
   ReadOnlySpan<byte> associated)
  {
    AssertAesDec(key);
    var associat = ToAssociated(associated, key);
    var tag = bytes[..AES_TAG_SIZE].ToArray();
    var iv = bytes.Slice(AES_TAG_SIZE, AES_IV_SIZE).ToArray();
    var cipher = bytes[(AES_TAG_SIZE + AES_IV_SIZE)..].ToArray();

    try
    {
      if (Verify(cipher, key.ToArray(), tag, associat))
        return DecryptionAesSingle(
          cipher, key.ToArray(), iv, associat);
    }
    catch { ResetBytes(associat, tag, iv, cipher); }
    finally { ResetBytes(associat, tag, iv, cipher); }

    throw new CryptographicException($"Verifiy {nameof(DecryptionAes)} failed!");
  }
  #endregion AES En- & Decryption

  #region AES File En- & Decryption

  #region AES File En- & Decryption
  public static void EncryptionFileAes(
    string src, string dest, UsIPtr<byte> key,
    ReadOnlySpan<byte> associated)
  {
    AssertAesEnc(src, dest, key);
    var iv = RngCryptoBytes(AES_IV_SIZE);
    var associat = ToAssociated(associated, key);
    var buffer = new byte[AES_MAX_PLAIN_SIZE];

    var sw = Stopwatch.StartNew();
    using var fsin = new FileStream(src, FileMode.Open, FileAccess.Read);
    using var fsout = new FileStream(dest, FileMode.Create, FileAccess.ReadWrite);

    int readbytes = 0, length = buffer.Length;
    fsout.Position = AES_TAG_SIZE + AES_IV_SIZE;
    //var cnt = fsin.Length / (AES_MAX_PLAIN_SIZE - 1) + 1;
    while ((readbytes = fsin.Read(buffer)) > 0)
    {
      if (readbytes != length)
        Array.Resize(ref buffer, readbytes);

      var cipher = EncryptionAesSingle(
        buffer, key.ToArray(), iv, associat);

      fsout.Write(cipher);
      Array.Clear(buffer);
    }

    var pos = AES_TAG_SIZE + AES_IV_SIZE;
    fsout.Position = AES_TAG_SIZE + AES_IV_SIZE;
    var tag = ToTag(fsout, key.ToArray(), associat, pos);

    fsout.Position = 0; fsout.Write(tag); fsout.Write(iv);
    MemoryClear(tag, iv, associat);

    var deltatime = (int)(TimeSleep - sw.ElapsedMilliseconds);
    if (TimeLoc)
      if (deltatime > 0)
        Thread.Sleep(deltatime);
  }

  public static void DecryptionFileAes(
    string src, string dest, UsIPtr<byte> key,
    ReadOnlySpan<byte> associated)
  {
    AssertAesDec(src, dest, key);

    var associat = ToAssociated(associated, key);
    byte[] tag = new byte[AES_TAG_SIZE], iv = new byte[AES_IV_SIZE];
    using var fsin = new FileStream(src, FileMode.Open, FileAccess.Read);
    using var fsout = new FileStream(dest, FileMode.Create, FileAccess.Write);
    var cnt = fsin.Read(tag); cnt += fsin.Read(iv);

    try
    {
      fsin.Position = AES_TAG_SIZE + AES_IV_SIZE;
      if (Verify(fsin, key.ToArray(), tag, associat, cnt))
      {
        fsin.Position = AES_IV_SIZE + AES_TAG_SIZE;
        var buffer = new byte[AES_GCM_MAX_PLAIN_SIZE + 16];
        int readbytes = 0, length = AES_GCM_MAX_PLAIN_SIZE + 16;
        while ((readbytes = fsin.Read(buffer)) > 0)
        {
          if (readbytes != length)
            Array.Resize(ref buffer, readbytes);

          var decipher = DecryptionAesSingle(
            buffer, key.ToArray(), iv, associat);

          fsout.Write(decipher);
          Array.Clear(buffer);

          if (buffer.Length != length)
            Array.Resize(ref buffer, length);
        }
        return;
      }
    }
    catch { MemoryClear(associat, tag, iv); }
    finally { MemoryClear(associat, tag, iv); }

    throw new CryptographicException($"Verifiy {nameof(DecryptionAes)} failed!");
  }
  #endregion AES File En- & Decryption

  #region AES Transfer File Stream En- & Decryption

  public static void EncryptionFileAes(
    FileStream fsin, FileStream fsout,
    int startin, int lengthin, int startout,
    UsIPtr<byte> key, ReadOnlySpan<byte> associated)
  {
    AssertAesEnc(fsin, fsout, key, startin, lengthin, startout);
    fsin.Position = startin; fsout.Position = startout;

    var iv = RngCryptoBytes(AES_IV_SIZE);
    var associat = ToAssociated(associated, key);
    var buffer = new byte[AES_MAX_PLAIN_SIZE];

    var sw = Stopwatch.StartNew();
    int readbytes, length = buffer.Length;
    fsout.Position = startout + AES_TAG_SIZE + AES_IV_SIZE;
    while ((readbytes = fsin.Read(buffer)) > 0)
    {
      if (readbytes != length)
        Array.Resize(ref buffer, readbytes);

      var cipher = EncryptionAesSingle(
        buffer, key.ToArray(), iv, associat);

      fsout.Write(cipher);
      Array.Clear(buffer);
    }

    var pos = startout + AES_TAG_SIZE + AES_IV_SIZE;
    fsout.Position = startout + AES_TAG_SIZE + AES_IV_SIZE;
    var tag = ToTag(fsout, key.ToArray(), associat, pos);

    fsout.Position = startout; fsout.Write(tag); fsout.Write(iv);
    MemoryClear(tag, iv, associat);

    var deltatime = (int)(TimeSleep - sw.ElapsedMilliseconds);
    if (TimeLoc)
      if (deltatime > 0)
        Thread.Sleep(deltatime);
  }

  public static void DecryptionFileAes(
    FileStream fsin, FileStream fsout,
    int startin, int lengthin, int startout,
    UsIPtr<byte> key, ReadOnlySpan<byte> associated)
  {
    AssertAesDec(fsin, fsout, key, startin, lengthin, startout);

    var sw = Stopwatch.StartNew();
    var associat = ToAssociated(associated, key);
    byte[] tag = new byte[AES_TAG_SIZE], iv = new byte[AES_IV_SIZE];
    var cnt = fsin.Read(tag); cnt += fsin.Read(iv);

    try
    {
      var pos = fsin.Position = startin + AES_TAG_SIZE + AES_IV_SIZE;
      if (Verify(fsin, key.ToArray(), tag, associat, (int)pos))
      {
        fsin.Position = pos; fsout.Position = startout;
        var buffer = new byte[AES_GCM_MAX_PLAIN_SIZE + 16];
        int readbytes = 0, length = AES_GCM_MAX_PLAIN_SIZE + 16;
        while ((readbytes = fsin.Read(buffer)) > 0)
        {
          if (readbytes != length)
            Array.Resize(ref buffer, readbytes);

          var decipher = DecryptionAesSingle(
            buffer, key.ToArray(), iv, associat);

          fsout.Write(decipher);
          Array.Clear(buffer);

          if (buffer.Length != length)
            Array.Resize(ref buffer, length);
        }

        var deltatime = (int)(TimeSleep - sw.ElapsedMilliseconds);
        if (TimeLoc)
          if (deltatime > 0)
            Thread.Sleep(deltatime);

        return;
      }
    }
    catch { MemoryClear(associat, tag, iv); }
    finally { MemoryClear(associat, tag, iv); }

    throw new CryptographicException($"Verifiy {nameof(DecryptionAes)} failed!");
  }

  #endregion AES Transfer File Stream En- & Decryption

  #endregion AES File En- & Decryption

  #region AES Single En- & Decryption
  private static byte[] EncryptionAesSingle(
    byte[] bytes, byte[] key,
    byte[] iv, byte[] associated)
  {
    var aesc = Aes.Create();
    {
      aesc.IV = iv;
      aesc.Key = key;
      aesc.Mode = CipherMode.CBC;
      aesc.Padding = PaddingMode.PKCS7;
    }

    using var ictf = aesc.CreateEncryptor();
    return ictf.TransformFinalBlock(bytes, 0, bytes.Length);

  }

  private static byte[] DecryptionAesSingle(
    byte[] bytes, byte[] key,
    byte[] iv, byte[] associated)
  {
    var aesc = Aes.Create();
    {
      aesc.IV = iv;
      aesc.Key = key;
      aesc.Mode = CipherMode.CBC;
      aesc.Padding = PaddingMode.PKCS7;
    }

    using var ictf = aesc.CreateDecryptor();
    return ictf.TransformFinalBlock(bytes, 0, bytes.Length);
  }
  #endregion AES Single En- & Decryption

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


  private static byte[] ToTag(Stream strm, byte[] key, byte[] assosiat, int pos = 0)
  {
    int ts = AES_TAG_SIZE, ks = AES_KEY_SIZE;
    var start = (key.Sum(x => x) % (ks - ts - 1)) + 1;
    var k = SHA256.HashData([.. key.Skip(start).Take(ts)]);

    strm.Position = pos;
    var src = MD5.HashData(strm).Concat(assosiat).ToArray();
    var hash = HMACSHA512.HashData(k, src);

    start = (hash.Sum(x => x) % (hash.Length - ts - 1)) + 1;
    return [.. hash.Skip(start).Take(ts)];
  }

  private static bool Verify(
    byte[] cipher, byte[] key, byte[] tag, byte[] assosiat) =>
      tag.SequenceEqual(ToTag(cipher, key, assosiat));

  private static bool Verify(
    Stream cipher, byte[] key, byte[] tag, byte[] assosiat, int pos = 0) =>
      tag.SequenceEqual(ToTag(cipher, key, assosiat, pos));

  #endregion AES Utils
}
