
using System.Diagnostics;
using System.Security.Cryptography;

namespace michele.natale.EcCurveDsaDh;

partial class EcService
{

  //um Timing Attacts entgegenzuwirken.
  private const long TimeSleep = 120; //ms
  private static readonly bool TimeLoc = true;

  public const int AES_IV_SIZE = 16;
  public const int AES_TAG_SIZE = 16;
  public const int AES_KEY_SIZE = 32;
  public const int AES_MIN_PLAIN_SIZE = 8;
  public const int AES_MAX_PLAIN_SIZE = 1024 * 1024;

  public static byte[] EncryptionAes(
    ReadOnlySpan<byte> bytes,
    ReadOnlySpan<byte> key,
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
    Array.Clear(cipher); Array.Clear(tag); Array.Clear(associat);

    var deltatime = (int)(TimeSleep - sw.ElapsedMilliseconds);
    if (TimeLoc)
      if (deltatime > 0)
        Thread.Sleep(deltatime);

    return result;
  }

  public static byte[] DecryptionAes(
   ReadOnlySpan<byte> bytes,
   ReadOnlySpan<byte> key,
   ReadOnlySpan<byte> associated)
  {
    AssertAesDec(key);
    var associat = ToAssociated(associated, key);
    var tag = bytes.Slice(0, AES_TAG_SIZE).ToArray();
    var iv = bytes.Slice(AES_TAG_SIZE, AES_IV_SIZE).ToArray();
    var cipher = bytes.Slice(AES_TAG_SIZE + AES_IV_SIZE).ToArray();

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



  private static void AssertAesEnc(
    ReadOnlySpan<byte> bytes,
    ReadOnlySpan<byte> key)
  {
    if ((bytes.Length < AES_MIN_PLAIN_SIZE) || (bytes.Length > AES_MAX_PLAIN_SIZE))
      throw new ArgumentOutOfRangeException(nameof(bytes));

    if (key.Length != AES_KEY_SIZE)
      throw new ArgumentOutOfRangeException(nameof(bytes));
  }

  private static void AssertAesDec(ReadOnlySpan<byte> key)
  {
    if (key.Length != AES_KEY_SIZE)
      throw new ArgumentOutOfRangeException(nameof(key));
  }

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

  private static bool Verify(
    byte[] cipher, byte[] key, byte[] tag, byte[] assosiat) =>
      tag.SequenceEqual(ToTag(cipher, key, assosiat));

}
