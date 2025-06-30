using System.Security.Cryptography;

namespace michele.natale.Services;

using Pointers;

partial class BcPqcServices
{

  public const int AES_GCM_TAG_SIZE = 16;
  public const int AES_GCM_NONCE_SIZE = 12;
  public const int AES_GCM_MIN_KEY_SIZE = 16;
  public const int AES_GCM_MID_KEY_SIZE = 24;
  public const int AES_GCM_MAX_KEY_SIZE = 32;
  public const int AES_GCM_MIN_PLAIN_SIZE = 8;
  public const int AES_GCM_MAX_PLAIN_SIZE = 1024 * 1024;

  public static byte[] EncryptionAesGcm(
      ReadOnlySpan<byte> bytes, UsIPtr<byte> key,
      ReadOnlySpan<byte> associated)
  {
    AssertAesGcmEnc(bytes, key);

    var associat = ToAssociated(associated, key);
    var cipher = EncAesGcmSingle(bytes, key, associat, out var tag, out var nonce);
    var result = new byte[cipher.Length + AES_GCM_TAG_SIZE + AES_GCM_NONCE_SIZE];

    Array.Copy(tag, result, AES_GCM_TAG_SIZE);
    Array.Copy(nonce, 0, result, AES_GCM_TAG_SIZE, AES_GCM_NONCE_SIZE);
    Array.Copy(cipher, 0, result, AES_GCM_TAG_SIZE + AES_GCM_NONCE_SIZE, cipher.Length);
    Array.Clear(tag); Array.Clear(nonce); Array.Clear(cipher);

    return result;
  }

  public static byte[] DecryptionAesGcm(
    ReadOnlySpan<byte> bytes, UsIPtr<byte> key,
    ReadOnlySpan<byte> associated)
  {
    AssertAesGcmDec(key);

    var tag = bytes[..AES_GCM_TAG_SIZE];
    var nonce = bytes.Slice(AES_GCM_TAG_SIZE, AES_GCM_NONCE_SIZE);

    var associat = ToAssociated(associated, key);
    var tnlength = AES_GCM_TAG_SIZE + AES_GCM_NONCE_SIZE;
    var decipher = DecAesGcmSingle(bytes[tnlength..], key, associat, tag, nonce);

    var length = bytes.Length - tnlength;
    if (decipher.Length == length) return decipher;

    throw new CryptographicException($"{nameof(DecryptionAesGcm)} failed.");
  }

  public static void EncryptionFileAesGcm(
    string src, string dest,
    UsIPtr<byte> key, ReadOnlySpan<byte> associated)
  {
    AssertAesGcmEnc(src, dest, key);
    var associat = ToAssociated(associated, key);
    var buffer = new byte[AES_GCM_MAX_PLAIN_SIZE];

    using var fsin = new FileStream(src, FileMode.Open, FileAccess.Read);
    using var fsout = new FileStream(dest, FileMode.Create, FileAccess.ReadWrite);

    fsout.Position = 0;
    int readbytes = 0, length = buffer.Length;
    while ((readbytes = fsin.Read(buffer)) > 0)
    {
      if (readbytes != length)
        Array.Resize(ref buffer, readbytes);

      var cipher = EncAesGcmSingle(
        buffer, key, associat, out var tag, out var nonce);

      fsout.Write(tag);
      fsout.Write(nonce);
      fsout.Write(cipher);
      Array.Clear(buffer);
    }
  }

  public static void DecryptionFileAesGcm(
    string src, string dest,
    UsIPtr<byte> key, ReadOnlySpan<byte> associated)
  {
    AssertAesGcmDec(src, dest, key);

    var associat = ToAssociated(associated, key);
    using var fsin = new FileStream(src, FileMode.Open, FileAccess.Read);
    using var fsout = new FileStream(dest, FileMode.Create, FileAccess.ReadWrite);

    try
    {
      int readbytes = 0; fsin.Position = 0;
      var start = AES_GCM_TAG_SIZE + AES_GCM_NONCE_SIZE;
      var buffer = new byte[start + AES_GCM_MAX_PLAIN_SIZE].AsSpan();
      while ((readbytes = fsin.Read(buffer)) > 0)
      {
        var decipher = DecAesGcmSingle(
          buffer[start..readbytes], key, associat,
          buffer[..AES_GCM_TAG_SIZE],
          buffer.Slice(AES_GCM_TAG_SIZE, AES_GCM_NONCE_SIZE));

        fsout.Write(decipher);
        buffer.Clear();
      }
      return;
    }
    catch { }

    throw new CryptographicException($"Verifiy {nameof(DecryptionAes)} failed!");
  }


  public static void EncryptionFileAesGcm(
    FileStream fsin, FileStream fsout,
    int startin, int lengthin, int startout,
    UsIPtr<byte> key, ReadOnlySpan<byte> associated)
  {
    AssertAesGcmEnc(fsin, fsout, key, startin, lengthin, startout);
    var associat = ToAssociated(associated, key);
    var buffer = new byte[AES_GCM_MAX_PLAIN_SIZE];

    int readbytes, length = buffer.Length;
    fsin.Position = startin; fsout.Position = startout;
    while ((readbytes = fsin.Read(buffer)) > 0)
    {
      if (readbytes != length)
        Array.Resize(ref buffer, readbytes);

      var cipher = EncAesGcmSingle(
        buffer, key, associat, out var tag, out var nonce);

      fsout.Write(tag);
      fsout.Write(nonce);
      fsout.Write(cipher);
      Array.Clear(buffer);
    }
  }

  public static void DecryptionFileAesGcm(
    FileStream fsin, FileStream fsout,
    int startin, int lengthin, int startout,
    UsIPtr<byte> key, ReadOnlySpan<byte> associated)
  {
    AssertAesGcmDec(fsin, fsout, key, startin, lengthin, startout);

    var associat = ToAssociated(associated, key);

    try
    {
      var start = CHACHA_POLY_TAG_SIZE + CHACHA_POLY_NONCE_SIZE;
      var buffer = new byte[start + CHACHA_POLY_MAX_PLAIN_SIZE].AsSpan();
      int readbytes = 0; fsin.Position = startin; fsout.Position = startout;
      while ((readbytes = fsin.Read(buffer)) > 0)
      {
        var decipher = DecAesGcmSingle(
          buffer[start..readbytes], key, associat,
          buffer[..AES_GCM_TAG_SIZE],
          buffer.Slice(AES_GCM_TAG_SIZE, AES_GCM_NONCE_SIZE));

        fsout.Write(decipher);
        buffer.Clear();
      }
      return;
    }
    catch { }

    throw new CryptographicException($"Verifiy {nameof(DecryptionAes)} failed!");
  }

  private static byte[] EncAesGcmSingle(
    ReadOnlySpan<byte> bytes, UsIPtr<byte> key,
    ReadOnlySpan<byte> associated, out byte[] tag, out byte[] nonce)
  {
    tag = new byte[AES_GCM_TAG_SIZE];
    nonce = new byte[AES_GCM_NONCE_SIZE];
    FillCryptoBytes(nonce);

    var cipher = new byte[bytes.Length];
    using var aes = new AesGcm(key.ToArray(), tag.Length);
    aes.Encrypt(nonce, bytes, cipher, tag, associated);

    return cipher;
  }

  private static byte[] DecAesGcmSingle(
    ReadOnlySpan<byte> bytes, UsIPtr<byte> key,
    ReadOnlySpan<byte> associated, ReadOnlySpan<byte> tag,
    ReadOnlySpan<byte> nonce)
  {
    var decipher = new byte[bytes.Length];
    using var aes = new AesGcm(key.ToArray(), tag.Length);
    aes.Decrypt(nonce, bytes, tag, decipher, associated);

    return decipher;
  }

  private static void AssertAesGcmEnc(
    ReadOnlySpan<byte> bytes, UsIPtr<byte> key)
  {
    var length = bytes.Length;
    if (length > AES_GCM_MAX_PLAIN_SIZE) throw new ArgumentOutOfRangeException(nameof(bytes));
    if (length < AES_GCM_MIN_PLAIN_SIZE) throw new ArgumentOutOfRangeException(nameof(bytes));

    length = key.Length;
    if (length != AES_GCM_MIN_KEY_SIZE && length != AES_GCM_MID_KEY_SIZE && length != AES_GCM_MAX_KEY_SIZE)
      throw new ArgumentOutOfRangeException(nameof(key));
  }

  private static void AssertAesGcmDec(UsIPtr<byte> key)
  {
    var length = key.Length;
    if (length != AES_GCM_MIN_KEY_SIZE && length != AES_GCM_MID_KEY_SIZE && length != AES_GCM_MAX_KEY_SIZE)
      throw new ArgumentOutOfRangeException(nameof(key));
  }

  private static void AssertAesGcmEnc(
    string src, string dest,
    UsIPtr<byte> key)
  {
    if (!File.Exists(src))
      throw new FileNotFoundException(nameof(src));

    if (File.Exists(dest))
      File.Delete(dest);

    if (key.Length != AES_GCM_MAX_KEY_SIZE)
      throw new ArgumentOutOfRangeException(nameof(key));

  }

  private static void AssertAesGcmDec(
    string src, string dest, UsIPtr<byte> key)
  {
    AssertAesGcmDec(key);

    if (!File.Exists(src))
      throw new FileNotFoundException(nameof(src));

    if (File.Exists(dest))
      File.Delete(dest);
  }


  private static void AssertAesGcmEnc(
     FileStream fsin, FileStream fsout, UsIPtr<byte> key,
     int startin, int lengthin, int startout)
  {
    if (key.Length != AES_GCM_MAX_KEY_SIZE)
      throw new ArgumentOutOfRangeException(nameof(key));

    ArgumentOutOfRangeException.ThrowIfNegative(startout);

    if (startin + lengthin < fsin.Length)
      throw new ArgumentOutOfRangeException(nameof(startin));

    if (!fsin.CanRead)
      throw new ArgumentException(
        $"Stream must CanRead, has failed!", nameof(fsin));

    if (!fsout.CanRead) throw new ArgumentException(
        $"Stream must CanRead, has failed!", nameof(fsout));

    if (!fsout.CanWrite) throw new ArgumentException(
        $"Stream must Canwrite, has failed!", nameof(fsout));

  }


  private static void AssertAesGcmDec(
     FileStream fsin, FileStream fsout, UsIPtr<byte> key,
     int startin, int lengthin, int startout)
  {
    if (key.Length != AES_GCM_MAX_KEY_SIZE)
      throw new ArgumentOutOfRangeException(nameof(key));

    ArgumentOutOfRangeException.ThrowIfNegative(startout);

    if (startin + lengthin < fsin.Length)
      throw new ArgumentOutOfRangeException(nameof(startin));

    if (!fsin.CanRead)
      throw new ArgumentException(
        $"Stream must CanRead, has failed!", nameof(fsin));

    //if (!fsout.CanRead) throw new ArgumentException(
    //    $"Stream must CanRead, has failed!", nameof(fsout));

    if (!fsout.CanWrite) throw new ArgumentException(
        $"Stream must Canwrite, has failed!", nameof(fsout));

  }
}
