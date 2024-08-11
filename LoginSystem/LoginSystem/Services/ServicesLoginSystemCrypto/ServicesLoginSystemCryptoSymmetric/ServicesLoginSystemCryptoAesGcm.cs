
using System.Security.Cryptography;

namespace michele.natale.LoginSystems.Services;

using static RandomHolder;


partial class AppServices
{

  /// <summary>
  /// AES_GCM_TAG_SIZE = 16
  /// </summary>
  public const int AES_GCM_TAG_SIZE = 16;

  /// <summary>
  /// AES_GCM_NONCE_SIZE = 12
  /// </summary>
  public const int AES_GCM_NONCE_SIZE = 12;

  /// <summary>
  /// AES_GCM_MIN_KEY_SIZE = 16
  /// </summary>
  public const int AES_GCM_MIN_KEY_SIZE = 16;

  /// <summary>
  /// AES_GCM_MID_KEY_SIZE = 24
  /// </summary>
  public const int AES_GCM_MID_KEY_SIZE = 24;

  /// <summary>
  /// AES_GCM_MAX_KEY_SIZE = 32
  /// </summary>
  public const int AES_GCM_MAX_KEY_SIZE = 32;

  /// <summary>
  /// AES_GCM_MIN_PLAIN_SIZE = 8
  /// </summary>
  public const int AES_GCM_MIN_PLAIN_SIZE = 8;

  /// <summary>
  /// AES_GCM_MAX_PLAIN_SIZE = 1024 * 1024
  /// </summary>
  public const int AES_GCM_MAX_PLAIN_SIZE = 1024 * 1024;

  /// <summary>
  /// Aes Gcm Encryption Methode
  /// </summary>
  /// <param name="bytes">Plaintext</param>
  /// <param name="key">Key</param>
  /// <param name="associated">Additional</param>
  /// <returns>Cipher</returns>
  public byte[] EncryptionAesGcm(
      ReadOnlySpan<byte> bytes, ReadOnlySpan<byte> key,
      ReadOnlySpan<byte> associated)
  {
    AssertAesGcmEnc(bytes, key);

    var associat = this.ToAssociated(associated, key);
    var cipher = EncAesGcmSingle(bytes, key, associat, out var tag, out var nonce);
    var result = new byte[cipher.Length + AES_GCM_TAG_SIZE + AES_GCM_NONCE_SIZE];

    Array.Copy(tag, result, AES_GCM_TAG_SIZE);
    Array.Copy(nonce, 0, result, AES_GCM_TAG_SIZE, AES_GCM_NONCE_SIZE);
    Array.Copy(cipher, 0, result, AES_GCM_TAG_SIZE + AES_GCM_NONCE_SIZE, cipher.Length);
    Array.Clear(tag); Array.Clear(nonce); Array.Clear(cipher);

    return result;
  }

  /// <summary>
  /// Aes Gcm Decryption Methode
  /// </summary>
  /// <param name="bytes">Cipher</param>
  /// <param name="key">Key</param>
  /// <param name="associated">Additional</param>
  /// <returns>Decipher</returns>
  /// <exception cref="CryptographicException"></exception>
  public byte[] DecryptionAesGcm(
    ReadOnlySpan<byte> bytes, ReadOnlySpan<byte> key,
    ReadOnlySpan<byte> associated)
  {
    AssertAesGcmDec(key);

    var tag = bytes[..AES_GCM_TAG_SIZE];
    var nonce = bytes.Slice(AES_GCM_TAG_SIZE, AES_GCM_NONCE_SIZE);

    var associat = this.ToAssociated(associated, key);
    var tnlength = AES_GCM_TAG_SIZE + AES_GCM_NONCE_SIZE;
    var decipher = DecAesGcmSingle(bytes[tnlength..], key, associat, tag, nonce);

    var length = bytes.Length - tnlength;
    if (decipher.Length == length) return decipher;

    throw new CryptographicException($"{nameof(DecryptionAesGcm)} failed.");
  }

  /// <summary>
  /// Aes Gcm Encryption Single Methode
  /// </summary>
  /// <param name="bytes">Plaintext</param>
  /// <param name="key">Key</param>
  /// <param name="associated">Additional</param>
  /// <param name="tag">Tag</param>
  /// <param name="nonce">Nonce</param>
  /// <returns>Cipher</returns>
  private static byte[] EncAesGcmSingle(
    ReadOnlySpan<byte> bytes, ReadOnlySpan<byte> key,
    ReadOnlySpan<byte> associated, out byte[] tag, out byte[] nonce)
  {
    tag = new byte[AES_GCM_TAG_SIZE];
    nonce = new byte[AES_GCM_NONCE_SIZE];
    Instance.FillBytes(nonce);

    var cipher = new byte[bytes.Length];
    using var aes = new AesGcm(key, tag.Length);
    aes.Encrypt(nonce, bytes, cipher, tag, associated);

    return cipher;
  }

  /// <summary>
  /// Aes Gcm Decryption Single Methode
  /// </summary>
  /// <param name="bytes">Cipher</param>
  /// <param name="key">Key</param>
  /// <param name="associated">Additional</param>
  /// <param name="tag">Tag</param>
  /// <param name="nonce">Nonce</param>
  /// <returns>Decipher</returns>
  private static byte[] DecAesGcmSingle(
    ReadOnlySpan<byte> bytes, ReadOnlySpan<byte> key,
    ReadOnlySpan<byte> associated, ReadOnlySpan<byte> tag,
    ReadOnlySpan<byte> nonce)
  {
    var decipher = new byte[bytes.Length];
    using var aes = new AesGcm(key, tag.Length);
    aes.Decrypt(nonce, bytes, tag, decipher, associated);

    return decipher;
  }

  /// <summary>
  /// Check Encryption Arguments
  /// </summary>
  /// <param name="bytes">Plaintetxt</param>
  /// <param name="key">Key</param>
  /// <exception cref="ArgumentOutOfRangeException"></exception>
  private static void AssertAesGcmEnc(
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
  /// Check Decryption Arguments
  /// </summary>
  /// <param name="key">Key</param>
  /// <exception cref="ArgumentOutOfRangeException"></exception>
  private static void AssertAesGcmDec(ReadOnlySpan<byte> key)
  {
    var length = key.Length;
    if (length != AES_GCM_MIN_KEY_SIZE && length != AES_GCM_MID_KEY_SIZE && length != AES_GCM_MAX_KEY_SIZE)
      throw new ArgumentOutOfRangeException(nameof(key));
  }

}
