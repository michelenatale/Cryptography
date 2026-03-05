

using System.Diagnostics;
using System.Security.Cryptography;

namespace michele.natale;

using Pointers;

partial class NetServices
{
  #region AES En- & Decryption
  public static byte[] EncryptionAes(
    ReadOnlySpan<byte> bytes,
    UsIPtr<byte> key, ReadOnlySpan<byte> associated)
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
}
