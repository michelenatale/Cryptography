
using System.Security.Cryptography;

namespace michele.natale.LoginSystems.Services;

using static RandomHolder;

partial class AppServices
{
  /// <summary>
  /// CHACHA_POLY_TAG_SIZE = 16
  /// </summary>
  public const int CHACHA_POLY_TAG_SIZE = 16;

  /// <summary>
  /// CHACHA_POLY_NONCE_SIZE = 12
  /// </summary>
  public const int CHACHA_POLY_NONCE_SIZE = 12;

  /// <summary>
  /// CHACHA_POLY_MIN_KEY_SIZE = 16
  /// </summary>
  public const int CHACHA_POLY_MIN_KEY_SIZE = 16;

  /// <summary>
  /// CHACHA_POLY_MID_KEY_SIZE = 24
  /// </summary>
  public const int CHACHA_POLY_MID_KEY_SIZE = 24;

  /// <summary>
  /// CHACHA_POLY_MAX_KEY_SIZE = 32
  /// </summary>
  public const int CHACHA_POLY_MAX_KEY_SIZE = 32;

  /// <summary>
  /// CHACHA_POLY_MIN_PLAIN_SIZE = 8
  /// </summary>
  public const int CHACHA_POLY_MIN_PLAIN_SIZE = 8;

  /// <summary>
  /// CHACHA_POLY_MAX_PLAIN_SIZE = 1024 * 1024
  /// </summary>
  public const int CHACHA_POLY_MAX_PLAIN_SIZE = 1024 * 1024;


  /// <summary>
  /// ChaCha20Poly1305 Encryption Methode
  /// </summary>
  /// <param name="bytes">Plaintext</param>
  /// <param name="key">Key</param>
  /// <param name="associated">Additional</param>
  /// <returns>Cipher</returns>
  public byte[] EncryptionChaCha20Poly1305(
      ReadOnlySpan<byte> bytes, ReadOnlySpan<byte> key,
      ReadOnlySpan<byte> associated)
  {
    AssertChaChaPolyEnc(bytes, key);
    var associat = this.ToAssociated(associated, key);
    var cipher = EncChaCha20Poly1305Single(bytes, key, associat, out var tag, out var nonce);
    var result = new byte[cipher.Length + CHACHA_POLY_TAG_SIZE + CHACHA_POLY_NONCE_SIZE];

    Array.Copy(tag, result, CHACHA_POLY_TAG_SIZE);
    Array.Copy(nonce, 0, result, CHACHA_POLY_TAG_SIZE, CHACHA_POLY_NONCE_SIZE);
    Array.Copy(cipher, 0, result, CHACHA_POLY_TAG_SIZE + CHACHA_POLY_NONCE_SIZE, cipher.Length);
    Array.Clear(tag); Array.Clear(nonce); Array.Clear(cipher);

    return result;
  }

  /// <summary>
  /// ChaCha20Poly1305 Decryption Methode
  /// </summary>
  /// <param name="bytes">Cipher</param>
  /// <param name="key">Key</param>
  /// <param name="associated">Additional</param>
  /// <returns>Decipher</returns>
  /// <exception cref="CryptographicException"></exception>
  public byte[] DecryptionChaCha20Poly1305(
    ReadOnlySpan<byte> bytes, ReadOnlySpan<byte> key,
    ReadOnlySpan<byte> associated)
  {
    AssertChaChaPolyDec(key);
    var tag = bytes[..CHACHA_POLY_TAG_SIZE];
    var nonce = bytes.Slice(CHACHA_POLY_TAG_SIZE, CHACHA_POLY_NONCE_SIZE);

    var associat = this.ToAssociated(associated, key);
    var tnlength = CHACHA_POLY_TAG_SIZE + CHACHA_POLY_NONCE_SIZE;
    var decipher = DecChaCha20Poly1305Single(bytes[tnlength..], key, associat, tag, nonce);

    var length = bytes.Length - tnlength;
    if (decipher.Length == length) return decipher;

    throw new CryptographicException($"{nameof(DecryptionChaCha20Poly1305)} failed.");
  }

  /// <summary>
  /// ChaCha20Poly1305 Encryption Single Methode
  /// </summary>
  /// <param name="data">Plaintext</param>
  /// <param name="key">Key</param>
  /// <param name="associated">Additional</param>
  /// <param name="tag">Tag</param>
  /// <param name="nonce">nonce</param>
  /// <returns>Cipher</returns>
  private static byte[] EncChaCha20Poly1305Single(
    ReadOnlySpan<byte> data, ReadOnlySpan<byte> key,
    ReadOnlySpan<byte> associated, out byte[] tag, out byte[] nonce)
  {
    tag = new byte[CHACHA_POLY_TAG_SIZE];
    nonce = Instance.RngBytes(CHACHA_POLY_NONCE_SIZE);

    var cipher = new byte[data.Length];
    using var ccp = new ChaCha20Poly1305(key);
    ccp.Encrypt(nonce, data, cipher, tag, associated);

    return cipher;
  }

  /// <summary>
  /// ChaCha20Poly1305 Decryption Single Methode
  /// </summary>
  /// <param name="bytes">Cipher</param>
  /// <param name="key">Key</param>
  /// <param name="associated">Additional</param>
  /// <param name="tag">Tag</param>
  /// <param name="nonce">Nonce</param>
  /// <returns>Decypher</returns>
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

  /// <summary>
  /// Check Encryption Arguments
  /// </summary>
  /// <param name="bytes">Plaintext</param>
  /// <param name="key">Key</param>
  /// <exception cref="ArgumentOutOfRangeException"></exception>
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

  /// <summary>
  /// Check Decrytion Arguments
  /// </summary>
  /// <param name="key"></param>
  /// <exception cref="ArgumentOutOfRangeException"></exception>
  private static void AssertChaChaPolyDec(ReadOnlySpan<byte> key)
  {
    var length = key.Length;
    if (length != AES_GCM_MIN_KEY_SIZE && length != AES_GCM_MID_KEY_SIZE && length != AES_GCM_MAX_KEY_SIZE)
      throw new ArgumentOutOfRangeException(nameof(key));
  }

}
