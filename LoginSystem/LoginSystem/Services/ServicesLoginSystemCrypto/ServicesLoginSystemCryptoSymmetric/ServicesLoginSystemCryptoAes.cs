
using System.Diagnostics;
using System.Security.Cryptography;

namespace michele.natale.LoginSystems.Services;

using static RandomHolder;

partial class AppServices
{
  //um Timing Attacts entgegenzuwirken.
  private const long TimeSleep = 120; //ms
  private static readonly bool TimeLoc = true;

  /// <summary>
  /// Initialization Vector
  /// </summary>
  public const int AES_IV_SIZE = 16;

  /// <summary>
  /// Tag
  /// </summary>
  public const int AES_TAG_SIZE = 16;

  /// <summary>
  /// Key
  /// </summary>
  public const int AES_KEY_SIZE = 32;

  /// <summary>
  /// Minimum plaintext length
  /// </summary>
  public const int AES_MIN_PLAIN_SIZE = 8;

  /// <summary>
  /// Maximum plaintext length
  /// </summary>
  public const int AES_MAX_PLAIN_SIZE = 1024 * 1024;

  /// <summary>
  /// AES Encryption Methode
  /// </summary>
  /// <param name="bytes">Plaintext</param>
  /// <param name="key">Key</param>
  /// <param name="associated">Additional</param>
  /// <returns>Ciphertext</returns>
  public byte[] EncryptionAes(
    ReadOnlySpan<byte> bytes,
    ReadOnlySpan<byte> key,
    ReadOnlySpan<byte> associated)
  {
    AssertAesEnc(bytes, key);
    var iv = Instance.RngBytes(AES_IV_SIZE);
    var associat = this.ToAssociated(associated, key);

    var sw = Stopwatch.StartNew();
    var cipher = EncryptionAesSingle(
      bytes.ToArray(), key.ToArray(), iv, associat);

    var tag = ToTag(cipher, key.ToArray(), associat);
    var length = cipher.Length + AES_TAG_SIZE + AES_IV_SIZE;
    var result = new byte[length];

    Array.Copy(tag, result, tag.Length);
    Array.Copy(iv, 0, result, tag.Length, iv.Length);
    Array.Copy(cipher, 0, result, tag.Length + iv.Length, cipher.Length);
    Array.Clear(cipher); Array.Clear(tag); Array.Clear(associat);

    var deltatime = (int)(TimeSleep - sw.ElapsedMilliseconds);
    if (TimeLoc)
      if (deltatime > 0)
        Thread.Sleep(deltatime);

    return result;
  }

  /// <summary>
  /// AES Decryption Methode
  /// </summary>
  /// <param name="bytes">Cipher</param>
  /// <param name="key">Key</param>
  /// <param name="associated">Additional</param>
  /// <returns>Decipher</returns>
  /// <exception cref="CryptographicException"></exception>
  public byte[] DecryptionAes(
   ReadOnlySpan<byte> bytes,
   ReadOnlySpan<byte> key,
   ReadOnlySpan<byte> associated)
  {
    AssertAesDec(key);
    var associat = this.ToAssociated(associated, key);
    var tag = bytes.Slice(0, AES_TAG_SIZE).ToArray();
    var iv = bytes.Slice(AES_TAG_SIZE, AES_IV_SIZE).ToArray();
    var cipher = bytes.Slice(AES_TAG_SIZE + AES_IV_SIZE).ToArray();

    try
    {
      if (Verify(cipher, key.ToArray(), tag, associat))
        return DecryptionAesSingle(
          cipher, key.ToArray(), iv, associat);
    }
    catch { this.ClearPrimitives(associat, tag, iv, cipher); }
    finally { this.ClearPrimitives(associat, tag, iv, cipher); }

    throw new CryptographicException($"Verifiy {nameof(DecryptionAes)} failed!");
  }

  /// <summary>
  /// AES Encryption Single Methode
  /// </summary>
  /// <param name="bytes">Plaintext</param>
  /// <param name="key">Key</param>
  /// <param name="iv">initialisation Vector</param>
  /// <param name="associated">Additional</param>
  /// <returns>Cipher</returns>
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

    Xor(bytes, associated);
    using var ictf = aesc.CreateEncryptor();
    return ictf.TransformFinalBlock(bytes, 0, bytes.Length);
  }

  /// <summary>
  /// AES Decryption Single Methode
  /// </summary>
  /// <param name="bytes">Cipher</param>
  /// <param name="key">Key</param>
  /// <param name="iv">Initial Vector</param>
  /// <param name="associated">Additional</param>
  /// <returns>Decipher</returns>
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
    var result = ictf.TransformFinalBlock(bytes, 0, bytes.Length);
    Xor(result, associated);
    return result;
  }

  /// <summary>
  /// Check the plaintext
  /// </summary>
  /// <param name="bytes">Plaintext</param>
  /// <param name="key">Key</param>
  /// <exception cref="ArgumentOutOfRangeException"></exception>
  private static void AssertAesEnc(
    ReadOnlySpan<byte> bytes,
    ReadOnlySpan<byte> key)
  {
    if ((bytes.Length < AES_MIN_PLAIN_SIZE) || (bytes.Length > AES_MAX_PLAIN_SIZE))
      throw new ArgumentOutOfRangeException(nameof(bytes));

    if (key.Length != AES_KEY_SIZE)
      throw new ArgumentOutOfRangeException(nameof(bytes));
  }

  /// <summary>
  /// Check the Ciphertext
  /// </summary>
  /// <param name="key">Key</param>
  /// <exception cref="ArgumentOutOfRangeException"></exception>
  private static void AssertAesDec(ReadOnlySpan<byte> key)
  {
    if (key.Length != AES_KEY_SIZE)
      throw new ArgumentOutOfRangeException(nameof(key));
  }

  /// <summary>
  /// Creates a tag for the Cryption.
  /// </summary>
  /// <param name="cipher"></param>
  /// <param name="key"></param>
  /// <param name="assosiat"></param>
  /// <returns></returns>
  private static byte[] ToTag(
    byte[] cipher, byte[] key, byte[] assosiat)
  {
    int ts = AES_TAG_SIZE, ks = AES_KEY_SIZE;
    var start = (key.Sum(x => x) % (ks - ts - 1)) + 1;
    var k = SHA256.HashData(key.Skip(start).Take(ts).ToArray());

    var src = MD5.HashData(cipher).Concat(assosiat).ToArray();
    var hash = HMACSHA512.HashData(k, src);

    start = (hash.Sum(x => x) % (hash.Length - ts - 1)) + 1;
    return hash.Skip(start).Take(ts).ToArray();
  }

  /// <summary>
  /// Checks the verification
  /// </summary>
  /// <param name="cipher">Cipher</param>
  /// <param name="key">Key</param>
  /// <param name="tag">Tag</param>
  /// <param name="assosiat">Additional</param>
  /// <returns></returns>
  private static bool Verify(
    byte[] cipher, byte[] key, byte[] tag, byte[] assosiat) =>
      tag.SequenceEqual(ToTag(cipher, key, assosiat));

  /// <summary>
  /// A simple Xor-Cryption
  /// </summary>
  /// <param name="bytes">Bytes</param>
  /// <param name="entropie">Entropie</param>
  private static void Xor(byte[] bytes, byte[] entropie)
  {
    var szb = bytes.Length;
    var sze = entropie.Length;
    var count = szb + sze;
    for (var i = 0; i < count; i++)
      bytes[i % szb] ^= entropie[i % sze];
  }
}
