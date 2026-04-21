

namespace michele.natale;

using Pointers;

partial class NetServicesCrypto
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


  public async static Task EncryptionFileWithCryptionAlgoAsync(
    string srcfile, string destfile,
    UsIPtr<byte> key, ReadOnlyMemory<byte> associated,
    CryptionAlgorithm crypt_algo, CancellationToken ct = default)
  {
    switch (crypt_algo)
    {
      case CryptionAlgorithm.AES: await EncryptionFileAesAsync(srcfile, destfile, key, associated, ct); break;
      case CryptionAlgorithm.AES_GCM: await EncryptionFileAesGcmAsync(srcfile, destfile, key, associated, ct); break;
      case CryptionAlgorithm.CHACHA20_POLY1305: await EncryptionFileChaCha20Poly1305Async(srcfile, destfile, key, associated, ct); break;
      default: throw new ArgumentException($"{nameof(crypt_algo)} has failed!", nameof(crypt_algo));
    }
  }

  public async static Task DecryptionFileWithCryptionAlgoAsync(
    string srcfile, string destfile, UsIPtr<byte> key,
    ReadOnlyMemory<byte> associated, CryptionAlgorithm crypt_algo,
    CancellationToken ct)
  {
    switch (crypt_algo)
    {
      case CryptionAlgorithm.AES: await DecryptionFileAesAsync(srcfile, destfile, key, associated, ct); break;
      case CryptionAlgorithm.AES_GCM: await DecryptionFileAesGcmAsync(srcfile, destfile, key, associated, ct); break;
      case CryptionAlgorithm.CHACHA20_POLY1305: await DecryptionFileChaCha20Poly1305Async(srcfile, destfile, key, associated, ct); break;
      default: throw new ArgumentException($"{nameof(crypt_algo)} has failed!", nameof(crypt_algo));
    }
  }


  public async static Task EncryptionFileWithCryptionAlgoAsync(
    FileStream fsin, FileStream fsout,
    int startin, int lengthin, int startout,
    UsIPtr<byte> key, ReadOnlyMemory<byte> associated,
    CryptionAlgorithm crypt_algo, CancellationToken ct = default)
  {
    switch (crypt_algo)
    {
      case CryptionAlgorithm.AES: await EncryptionFileAesAsync(fsin, fsout, startin, lengthin, startout, key, associated, ct); break;
      case CryptionAlgorithm.AES_GCM: await EncryptionFileAesGcmAsync(fsin, fsout, startin, lengthin, startout, key, associated, ct); break;
      case CryptionAlgorithm.CHACHA20_POLY1305: await EncryptionFileChaCha20Poly1305Async(fsin, fsout, startin, lengthin, startout, key, associated, ct); break;
      default: throw new ArgumentException($"{nameof(crypt_algo)} has failed!", nameof(crypt_algo));
    }
  }

  public async static Task DecryptionFileWithCryptionAlgoAsync(
    FileStream fsin, FileStream fsout,
    int startin, int lengthin, int startout,
    UsIPtr<byte> key, ReadOnlyMemory<byte> associated,
    CryptionAlgorithm crypt_algo, CancellationToken ct = default)
  {
    switch (crypt_algo)
    {
      case CryptionAlgorithm.AES: await DecryptionFileAesAsync(fsin, fsout, startin, lengthin, startout, key, associated, ct); break;
      case CryptionAlgorithm.AES_GCM: await DecryptionFileAesGcmAsync(fsin, fsout, startin, lengthin, startout, key, associated, ct); break;
      case CryptionAlgorithm.CHACHA20_POLY1305: await DecryptionFileChaCha20Poly1305Async(fsin, fsout, startin, lengthin, startout, key, associated, ct); break;
      default: throw new ArgumentException($"{nameof(crypt_algo)} has failed!", nameof(crypt_algo));
    }
  }
}
