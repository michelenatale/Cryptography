

using System.Security.Cryptography;

namespace michele.natale.EcCurveDsaDh;

partial class EcService
{


  public const int CHACHA_POLY_TAG_SIZE = 16;
  public const int CHACHA_POLY_NONCE_SIZE = 12;
  public const int CHACHA_POLY_MIN_KEY_SIZE = 16;
  public const int CHACHA_POLY_MID_KEY_SIZE = 24;
  public const int CHACHA_POLY_MAX_KEY_SIZE = 32;
  public const int CHACHA_POLY_MIN_PLAIN_SIZE = 8;
  public const int CHACHA_POLY_MAX_PLAIN_SIZE = 1024 * 1024;


  public static byte[] EncryptionChaCha20Poly1305(
      ReadOnlySpan<byte> bytes, ReadOnlySpan<byte> key,
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
    ReadOnlySpan<byte> bytes, ReadOnlySpan<byte> key,
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


  private static byte[] EncChaCha20Poly1305Single(
    ReadOnlySpan<byte> data, ReadOnlySpan<byte> key,
    ReadOnlySpan<byte> associated, out byte[] tag, out byte[] nonce)
  {
    tag = new byte[CHACHA_POLY_TAG_SIZE];
    nonce = RngBytes(CHACHA_POLY_NONCE_SIZE);

    var cipher = new byte[data.Length];
    using var ccp = new ChaCha20Poly1305(key);
    ccp.Encrypt(nonce, data, cipher, tag, associated);

    return cipher;
  }

  private static byte[] DecChaCha20Poly1305Single(
    ReadOnlySpan<byte> bytes, ReadOnlySpan<byte> key,
    ReadOnlySpan<byte> associated, ReadOnlySpan<byte> tag,
    ReadOnlySpan<byte> nonce)
  {
    var decipher = new byte[bytes.Length];
    using var ccp = new ChaCha20Poly1305(key);
    ccp.Decrypt(nonce, bytes, tag, decipher, associated);

    return decipher;
  }

  private static void AssertChaChaPolyEnc(
    ReadOnlySpan<byte> bytes, ReadOnlySpan<byte> key)
  {
    var length = bytes.Length;
    if (length > AES_GCM_MAX_PLAIN_SIZE) throw new ArgumentOutOfRangeException(nameof(bytes));
    if (length < AES_GCM_MIN_PLAIN_SIZE) throw new ArgumentOutOfRangeException(nameof(bytes));

    length = key.Length;
    if (length != AES_GCM_MIN_KEY_SIZE && length != AES_GCM_MID_KEY_SIZE && length != AES_GCM_MAX_KEY_SIZE)
      throw new ArgumentOutOfRangeException(nameof(key));
  }

  private static void AssertChaChaPolyDec(ReadOnlySpan<byte> key)
  {
    var length = key.Length;
    if (length != AES_GCM_MIN_KEY_SIZE && length != AES_GCM_MID_KEY_SIZE && length != AES_GCM_MAX_KEY_SIZE)
      throw new ArgumentOutOfRangeException(nameof(key));
  }


}
