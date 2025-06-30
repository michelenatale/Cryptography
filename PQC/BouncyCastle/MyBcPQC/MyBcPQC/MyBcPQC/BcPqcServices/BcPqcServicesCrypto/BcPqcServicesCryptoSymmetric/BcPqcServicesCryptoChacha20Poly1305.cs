using System.Security.Cryptography;

namespace michele.natale.Services;

using Pointers;

partial class BcPqcServices
{

  public const int CHACHA_POLY_TAG_SIZE = 16;
  public const int CHACHA_POLY_NONCE_SIZE = 12;
  public const int CHACHA_POLY_MIN_KEY_SIZE = 16;
  public const int CHACHA_POLY_MID_KEY_SIZE = 24;
  public const int CHACHA_POLY_MAX_KEY_SIZE = 32;
  public const int CHACHA_POLY_MIN_PLAIN_SIZE = 8;
  public const int CHACHA_POLY_MAX_PLAIN_SIZE = 1024 * 1024;

  public static byte[] EncryptionChaCha20Poly1305(
      ReadOnlySpan<byte> bytes, UsIPtr<byte> key,
      ReadOnlySpan<byte> associated)
  {
    AssertChaChaPolyEnc(bytes, key);
    var associat = ToAssociated(associated, key);
    var cipher = EncChaCha20Poly1305Single(bytes, key, associat, out var tag, out var nonce);
    var result = new byte[cipher.Length + CHACHA_POLY_TAG_SIZE + CHACHA_POLY_NONCE_SIZE];

    Array.Copy(tag, result, CHACHA_POLY_TAG_SIZE);
    Array.Copy(nonce, 0, result, CHACHA_POLY_TAG_SIZE, CHACHA_POLY_NONCE_SIZE);
    Array.Copy(cipher, 0, result, CHACHA_POLY_TAG_SIZE + CHACHA_POLY_NONCE_SIZE, cipher.Length);
    Array.Clear(tag); Array.Clear(nonce); Array.Clear(cipher);

    return result;
  }

  public static byte[] DecryptionChaCha20Poly1305(
    ReadOnlySpan<byte> bytes, UsIPtr<byte> key,
    ReadOnlySpan<byte> associated)
  {
    AssertChaChaPolyDec(key);
    var tag = bytes[..CHACHA_POLY_TAG_SIZE];
    var nonce = bytes.Slice(CHACHA_POLY_TAG_SIZE, CHACHA_POLY_NONCE_SIZE);

    var associat = ToAssociated(associated, key);
    var tnlength = CHACHA_POLY_TAG_SIZE + CHACHA_POLY_NONCE_SIZE;
    var decipher = DecChaCha20Poly1305Single(bytes[tnlength..], key, associat, tag, nonce);

    var length = bytes.Length - tnlength;
    if (decipher.Length == length) return decipher;

    throw new CryptographicException($"{nameof(DecryptionChaCha20Poly1305)} failed.");
  }

  public static void EncryptionFileChaCha20Poly1305(
    string src, string dest,
    UsIPtr<byte> key, ReadOnlySpan<byte> associated)
  {
    AssertChaChaPolyEnc(src, dest, key);
    var associat = ToAssociated(associated, key);
    var buffer = new byte[CHACHA_POLY_MAX_PLAIN_SIZE];

    using var fsin = new FileStream(src, FileMode.Open, FileAccess.Read);
    using var fsout = new FileStream(dest, FileMode.Create, FileAccess.ReadWrite);

    fsout.Position = 0;
    int readbytes = 0, length = buffer.Length;
    while ((readbytes = fsin.Read(buffer)) > 0)
    {
      if (readbytes != length)
        Array.Resize(ref buffer, readbytes);

      var cipher = EncChaCha20Poly1305Single(
        buffer, key, associat, out var tag, out var nonce);

      fsout.Write(tag);
      fsout.Write(nonce);
      fsout.Write(cipher);
      Array.Clear(buffer);
    }
  }

  public static void DecryptionFileChaCha20Poly1305(
    string src, string dest,
    UsIPtr<byte> key, ReadOnlySpan<byte> associated)
  {
    AssertChaChaPolyDec(src, dest, key);

    var associat = ToAssociated(associated, key);
    using var fsin = new FileStream(src, FileMode.Open, FileAccess.Read);
    using var fsout = new FileStream(dest, FileMode.Create, FileAccess.ReadWrite);

    try
    {
      int readbytes = 0; fsin.Position = 0;
      var start = CHACHA_POLY_TAG_SIZE + CHACHA_POLY_NONCE_SIZE;
      var buffer = new byte[start + CHACHA_POLY_MAX_PLAIN_SIZE].AsSpan();
      while ((readbytes = fsin.Read(buffer)) > 0)
      {
        var decipher = DecChaCha20Poly1305Single(
          buffer[start..readbytes], key, associat,
          buffer[..CHACHA_POLY_TAG_SIZE],
          buffer.Slice(CHACHA_POLY_TAG_SIZE, CHACHA_POLY_NONCE_SIZE));

        fsout.Write(decipher);
        buffer.Clear();
      }
      return;
    }
    catch { }

    throw new CryptographicException($"Verifiy {nameof(DecryptionAes)} failed!");
  }

  public static void EncryptionFileChaCha20Poly1305(
    FileStream fsin, FileStream fsout,
    int startin, int lengthin, int startout,
    UsIPtr<byte> key, ReadOnlySpan<byte> associated)
  {
    AssertChaChaPolyEnc(fsin, fsout, key, startin, lengthin, startout);
    var associat = ToAssociated(associated, key);
    var buffer = new byte[CHACHA_POLY_MAX_PLAIN_SIZE];

    int readbytes, length = buffer.Length;
    fsin.Position = startin; fsout.Position = startout;
    while ((readbytes = fsin.Read(buffer)) > 0)
    {
      if (readbytes != length)
        Array.Resize(ref buffer, readbytes);

      var cipher = EncChaCha20Poly1305Single(
        buffer, key, associat, out var tag, out var nonce);

      fsout.Write(tag);
      fsout.Write(nonce);
      fsout.Write(cipher);
      Array.Clear(buffer);
    }
  }

  public static void DecryptionFileChaCha20Poly1305(
    FileStream fsin, FileStream fsout,
    int startin, int lengthin, int startout,
    UsIPtr<byte> key, ReadOnlySpan<byte> associated)
  {
    AssertChaChaPolyDec(fsin, fsout, key, startin, lengthin, startout);

    var associat = ToAssociated(associated, key);

    try
    {
      var start = CHACHA_POLY_TAG_SIZE + CHACHA_POLY_NONCE_SIZE;
      var buffer = new byte[start + CHACHA_POLY_MAX_PLAIN_SIZE].AsSpan();
      int readbytes = 0; fsin.Position = startin; fsout.Position = startout;
      while ((readbytes = fsin.Read(buffer)) > 0)
      {
        var decipher = DecChaCha20Poly1305Single(
          buffer[start..readbytes], key, associat,
          buffer[..CHACHA_POLY_TAG_SIZE],
          buffer.Slice(CHACHA_POLY_TAG_SIZE, CHACHA_POLY_NONCE_SIZE));

        fsout.Write(decipher);
        buffer.Clear();
      }
      return;
    }
    catch { }

    throw new CryptographicException($"Verifiy {nameof(DecryptionAes)} failed!");
  }

  private static byte[] EncChaCha20Poly1305Single(
    ReadOnlySpan<byte> data, UsIPtr<byte> key,
    ReadOnlySpan<byte> associated, out byte[] tag, out byte[] nonce)
  {
    tag = new byte[CHACHA_POLY_TAG_SIZE];
    nonce = RngCryptoBytes(CHACHA_POLY_NONCE_SIZE);

    var cipher = new byte[data.Length];
    using var ccp = new ChaCha20Poly1305(key.ToArray());
    ccp.Encrypt(nonce, data, cipher, tag, associated);

    return cipher;
  }

  private static byte[] DecChaCha20Poly1305Single(
    ReadOnlySpan<byte> bytes, UsIPtr<byte> key,
    ReadOnlySpan<byte> associated, ReadOnlySpan<byte> tag,
    ReadOnlySpan<byte> nonce)
  {
    var decipher = new byte[bytes.Length];
    using var ccp = new ChaCha20Poly1305(key.ToArray());
    ccp.Decrypt(nonce, bytes, tag, decipher, associated);

    return decipher;
  }

  private static void AssertChaChaPolyEnc(
    ReadOnlySpan<byte> bytes, UsIPtr<byte> key)
  {
    var length = bytes.Length;
    if (length > CHACHA_POLY_MAX_PLAIN_SIZE) throw new ArgumentOutOfRangeException(nameof(bytes));
    if (length < CHACHA_POLY_MIN_PLAIN_SIZE) throw new ArgumentOutOfRangeException(nameof(bytes));

    length = key.Length;
    if (length != CHACHA_POLY_MIN_KEY_SIZE && length != CHACHA_POLY_MID_KEY_SIZE && length != CHACHA_POLY_MAX_KEY_SIZE)
      throw new ArgumentOutOfRangeException(nameof(key));
  }

  private static void AssertChaChaPolyDec(UsIPtr<byte> key)
  {
    var length = key.Length;
    if (length != CHACHA_POLY_MIN_KEY_SIZE && length != CHACHA_POLY_MID_KEY_SIZE && length != CHACHA_POLY_MAX_KEY_SIZE)
      throw new ArgumentOutOfRangeException(nameof(key));
  }


  private static void AssertChaChaPolyEnc(
    string src, string dest,
    UsIPtr<byte> key)
  {
    if (!File.Exists(src))
      throw new FileNotFoundException(nameof(src));

    if (File.Exists(dest))
      File.Delete(dest);

    if (key.Length != CHACHA_POLY_MAX_KEY_SIZE)
      throw new ArgumentOutOfRangeException(nameof(key));

  }

  private static void AssertChaChaPolyDec(
    string src, string dest, UsIPtr<byte> key)
  {
    AssertChaChaPolyDec(key);

    if (!File.Exists(src))
      throw new FileNotFoundException(nameof(src));

    if (File.Exists(dest))
      File.Delete(dest);
  }

  private static void AssertChaChaPolyEnc(
     FileStream fsin, FileStream fsout, UsIPtr<byte> key,
     int startin, int lengthin, int startout)
  {
    if (key.Length != CHACHA_POLY_MAX_KEY_SIZE)
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


  private static void AssertChaChaPolyDec(
     FileStream fsin, FileStream fsout, UsIPtr<byte> key,
     int startin, int lengthin, int startout)
  {
    if (key.Length != CHACHA_POLY_MAX_KEY_SIZE)
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
