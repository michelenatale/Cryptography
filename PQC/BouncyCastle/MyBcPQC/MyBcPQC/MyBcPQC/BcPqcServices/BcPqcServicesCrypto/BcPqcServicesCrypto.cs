

namespace michele.natale.Services;

using Pointers;

partial class BcPqcServices
{

  public static CryptionAlgorithm ToCryptionAlgorithm(string crypt_algo)
  {
    return crypt_algo switch
    {
      string obj when obj == CryptionAlgorithm.AES.ToString() => CryptionAlgorithm.AES,
      string obj when obj == CryptionAlgorithm.AES_GCM.ToString() => CryptionAlgorithm.AES_GCM,
      string obj when obj == CryptionAlgorithm.CHACHA20_POLY1305.ToString() => CryptionAlgorithm.CHACHA20_POLY1305,
      _ => throw new ArgumentException($"{crypt_algo} is failed"),
    };
  }

  public static byte[] EncryptionWithCryptionAlgo(
    ReadOnlySpan<byte> bytes, UsIPtr<byte> key,
    ReadOnlySpan<byte> associated, CryptionAlgorithm crypt_algo)
  {
    return crypt_algo switch
    {
      var obj when obj == CryptionAlgorithm.AES => EncryptionAes(bytes, key, associated),
      var obj when obj == CryptionAlgorithm.AES_GCM => EncryptionAesGcm(bytes, key, associated),
      var obj when obj == CryptionAlgorithm.CHACHA20_POLY1305 => EncryptionChaCha20Poly1305(bytes, key, associated),
      _ => throw new ArgumentException(null),
    };
  }

  public static byte[] DecryptionWithCryptionAlgo(
    ReadOnlySpan<byte> bytes, UsIPtr<byte> key,
    ReadOnlySpan<byte> associated, CryptionAlgorithm crypt_algo)
  {
    return crypt_algo switch
    {
      var obj when obj == CryptionAlgorithm.AES => DecryptionAes(bytes, key, associated),
      var obj when obj == CryptionAlgorithm.AES_GCM => DecryptionAesGcm(bytes, key, associated),
      var obj when obj == CryptionAlgorithm.CHACHA20_POLY1305 => DecryptionChaCha20Poly1305(bytes, key, associated),
      _ => throw new ArgumentException(null),
    };
  }


  public static void EncryptionFileWithCryptionAlgo(
    string srcfile, string destfile,
    UsIPtr<byte> key, ReadOnlySpan<byte> associated,
    CryptionAlgorithm crypt_algo)
  {
    switch (crypt_algo)
    {
      case CryptionAlgorithm.AES: EncryptionFileAes(srcfile, destfile, key, associated); break;
      case CryptionAlgorithm.AES_GCM: EncryptionFileAesGcm(srcfile, destfile, key, associated); break;
      case CryptionAlgorithm.CHACHA20_POLY1305: EncryptionFileChaCha20Poly1305(srcfile, destfile, key, associated); break;
      default: throw new ArgumentException($"{nameof(crypt_algo)} has failed!", nameof(crypt_algo));
    }
  }

  public static void DecryptionFileWithCryptionAlgo(
    string srcfile, string destfile, UsIPtr<byte> key,
    ReadOnlySpan<byte> associated, CryptionAlgorithm crypt_algo)
  {
    switch (crypt_algo)
    {
      case CryptionAlgorithm.AES: DecryptionFileAes(srcfile, destfile, key, associated); break;
      case CryptionAlgorithm.AES_GCM: DecryptionFileAesGcm(srcfile, destfile, key, associated); break;
      case CryptionAlgorithm.CHACHA20_POLY1305: DecryptionFileChaCha20Poly1305(srcfile, destfile, key, associated); break;
      default: throw new ArgumentException($"{nameof(crypt_algo)} has failed!", nameof(crypt_algo));
    }
  }


  public static void EncryptionFileWithCryptionAlgo(
    FileStream fsin, FileStream fsout,
    int startin, int lengthin, int startout,
    UsIPtr<byte> key, ReadOnlySpan<byte> associated,
    CryptionAlgorithm crypt_algo)
  {
    switch (crypt_algo)
    {
      case CryptionAlgorithm.AES: EncryptionFileAes(fsin, fsout, startin, lengthin, startout, key, associated); break;
      case CryptionAlgorithm.AES_GCM: EncryptionFileAesGcm(fsin, fsout, startin, lengthin, startout, key, associated); break;
      case CryptionAlgorithm.CHACHA20_POLY1305: EncryptionFileChaCha20Poly1305(fsin, fsout, startin, lengthin, startout, key, associated); break;
      default: throw new ArgumentException($"{nameof(crypt_algo)} has failed!", nameof(crypt_algo));
    }
  }

  public static void DecryptionFileWithCryptionAlgo(
    FileStream fsin, FileStream fsout,
    int startin, int lengthin, int startout,
    UsIPtr<byte> key, ReadOnlySpan<byte> associated,
    CryptionAlgorithm crypt_algo)
  {
    switch (crypt_algo)
    {
      case CryptionAlgorithm.AES: DecryptionFileAes(fsin, fsout, startin, lengthin, startout, key, associated); break;
      case CryptionAlgorithm.AES_GCM: DecryptionFileAesGcm(fsin, fsout, startin, lengthin, startout, key, associated); break;
      case CryptionAlgorithm.CHACHA20_POLY1305: DecryptionFileChaCha20Poly1305(fsin, fsout, startin, lengthin, startout, key, associated); break;
      default: throw new ArgumentException($"{nameof(crypt_algo)} has failed!", nameof(crypt_algo));
    }
  }

}
