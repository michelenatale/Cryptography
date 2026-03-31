

using System.Security.Cryptography;

namespace michele.natale;

partial class NetServicesCrypto
{
  public static byte[] ComputeHash(
    ReadOnlySpan<byte> bytes, HashAlgorithmName hname)
      => ComputeHash(bytes, ToHashAlgorithmType(hname));

  public static byte[] ComputeHmac(
    ReadOnlySpan<byte> bytes, ReadOnlySpan<byte> key, HashAlgorithmName hname)
      => ComputeHmac(bytes, key, ToHashAlgorithmType(hname));

  public static byte[] ComputeHash(
    ReadOnlySpan<byte> bytes, HashAlgorithmType halgotype) =>
   halgotype switch
   {
     HashAlgorithmType.Md5 => MD5.HashData(bytes),
     HashAlgorithmType.Sha1 => SHA1.HashData(bytes),
     HashAlgorithmType.Sha256 => SHA256.HashData(bytes),
     HashAlgorithmType.Sha384 => SHA384.HashData(bytes),
     HashAlgorithmType.Sha512 => SHA512.HashData(bytes),
     _ => throw new NotSupportedException($"Unsupported algorithm: {halgotype}"),
   };

  public static byte[] ComputeHmac(
    ReadOnlySpan<byte> bytes, ReadOnlySpan<byte> key,
      HashAlgorithmType halgotype) =>
   halgotype switch
   {
     HashAlgorithmType.Md5 => HMACMD5.HashData(key, bytes),
     HashAlgorithmType.Sha1 => HMACSHA1.HashData(key, bytes),
     HashAlgorithmType.Sha256 => HMACSHA256.HashData(key, bytes),
     HashAlgorithmType.Sha384 => HMACSHA384.HashData(key, bytes),
     HashAlgorithmType.Sha512 => HMACSHA512.HashData(key, bytes),
     _ => throw new NotSupportedException($"Unsupported algorithm: {halgotype}"),
   };

  public static HashAlgorithmType ToHashAlgorithmType(
    HashAlgorithmName halgoname)
  {
    return halgoname switch
    {
      var _ when halgoname == HashAlgorithmName.MD5 => HashAlgorithmType.Md5,
      var _ when halgoname == HashAlgorithmName.SHA1 => HashAlgorithmType.Sha1,
      var _ when halgoname == HashAlgorithmName.SHA256 => HashAlgorithmType.Sha256,
      var _ when halgoname == HashAlgorithmName.SHA384 => HashAlgorithmType.Sha384,
      var _ when halgoname == HashAlgorithmName.SHA512 => HashAlgorithmType.Sha512,
      _ => throw new NotSupportedException($"Unsupported algorithm: {halgoname}"),
    };
  }

  public enum HashAlgorithmType
  {
    Md5,
    Sha1,
    Sha256,
    Sha384,
    Sha512,
    HmacMd5,
    HmacSha1,
    HmacSha256,
    HmacSha384,
    HmacSha512,
  }
}
