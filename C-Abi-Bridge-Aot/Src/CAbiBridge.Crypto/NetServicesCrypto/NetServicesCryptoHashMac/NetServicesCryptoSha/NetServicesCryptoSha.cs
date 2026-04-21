//async-await
//***********

//A question that comes up time and again. To what extent should
//you use async-await for methods, or does it make sense to
//incorporate synchronous methods into an async method?

//The answer is very simple:
//Async-await should really be used sparingly—only as much as is
//absolutely necessary. 

//Async makes sense for files, networking, and streams (I/O-bound operations)
//because most methods are based on async.  

//However, hashing and HMAC are already 100% CPU-bound. In most cases,
//switching to async-await offers no additional benefit; on the contrary,
//it actually introduces overhead.

//CONCLUSION:
//✔️ Make only I / O operations asynchronous
//✔️ Keep CPU-bound operations synchronous


using System.Security.Cryptography; 

namespace michele.natale;

partial class NetServicesCrypto
{
  public static byte[] ComputeHash(
    ReadOnlySpan<byte> bytes,
    HashAlgorithmName hname, int shake_output_length = 32)
      => ComputeHash(bytes, ToHashAlgorithmType(hname), shake_output_length);

  public static byte[] ComputeHmac(
    ReadOnlySpan<byte> bytes, ReadOnlySpan<byte> key, HashAlgorithmName hname)
      => ComputeHmac(bytes, key, ToHashAlgorithmType(hname));
  public static byte[] ComputeHashFile(
    string filename, HashAlgorithmName hname, int shake_output_length = 32) =>
      ComputeHashFile(filename, ToHashAlgorithmType(hname), shake_output_length);

  public static byte[] ComputeHmacFile(
    string filename, ReadOnlySpan<byte> key, HashAlgorithmName hname) =>
      ComputeHmacFile(filename, key, ToHashAlgorithmType(hname));

  public async static Task<byte[]> ComputeHashFileAsync(
    string filename, HashAlgorithmName hname,
    int shake_output_length = 32, CancellationToken ct = default) =>
      await ComputeHashFileAsync(filename, ToHashAlgorithmType(hname), shake_output_length, ct);

  public async static Task<byte[]> ComputeHmacFileAsync(
    string filename, ReadOnlyMemory<byte> key,
    HashAlgorithmName hname, CancellationToken ct = default) =>
      await ComputeHmacFileAsync(filename, key, ToHashAlgorithmType(hname), ct);

  public static byte[] ComputeHash(
    ReadOnlySpan<byte> bytes,
    HashAlgorithmType halgotype,
    int shake_output_length = 32) =>
   halgotype switch
   {
     HashAlgorithmType.Md5 => MD5.HashData(bytes),
     HashAlgorithmType.Sha1 => SHA1.HashData(bytes),
     HashAlgorithmType.Sha256 => SHA256.HashData(bytes),
     HashAlgorithmType.Sha384 => SHA384.HashData(bytes),
     HashAlgorithmType.Sha512 => SHA512.HashData(bytes),

     HashAlgorithmType.Sha3_256 => SHA3_256.HashData(bytes),
     HashAlgorithmType.Sha3_384 => SHA3_384.HashData(bytes),
     HashAlgorithmType.Sha3_512 => SHA3_512.HashData(bytes),

     HashAlgorithmType.Shake128 => Shake128.HashData(bytes, shake_output_length),
     HashAlgorithmType.Shake256 => Shake256.HashData(bytes, shake_output_length),
     _ => throw new NotSupportedException($"Unsupported algorithm: {halgotype}"),
   };

  public static byte[] ComputeHmac(
    ReadOnlySpan<byte> bytes, ReadOnlySpan<byte> key,
      HashAlgorithmType halgotype) =>
   ToHmacBasicAlgorithmType(halgotype) switch
   {
     HashAlgorithmType.Md5 => HMACMD5.HashData(key, bytes),
     HashAlgorithmType.Sha1 => HMACSHA1.HashData(key, bytes),

     HashAlgorithmType.Sha256 => HMACSHA256.HashData(key, bytes),
     HashAlgorithmType.Sha384 => HMACSHA384.HashData(key, bytes),
     HashAlgorithmType.Sha512 => HMACSHA512.HashData(key, bytes),

     HashAlgorithmType.Sha3_256 => HMACSHA3_256.HashData(key, bytes),
     HashAlgorithmType.Sha3_384 => HMACSHA3_384.HashData(key, bytes),
     HashAlgorithmType.Sha3_512 => HMACSHA3_512.HashData(key, bytes),

     _ => throw new NotSupportedException($"Unsupported algorithm: {halgotype}"),
   };

  public static byte[] ComputeHashFile(
    string filename, HashAlgorithmType halgotype,
    int shake_output_length = 32)
  {
    using var fs = new FileStream(
      filename, FileMode.Open, FileAccess.Read);

    return halgotype switch
    {
      HashAlgorithmType.Md5 => MD5.HashData(fs),
      HashAlgorithmType.Sha1 => SHA1.HashData(fs),
      HashAlgorithmType.Sha256 => SHA256.HashData(fs),
      HashAlgorithmType.Sha384 => SHA384.HashData(fs),
      HashAlgorithmType.Sha512 => SHA512.HashData(fs),

      HashAlgorithmType.Sha3_256 => SHA3_256.HashData(fs),
      HashAlgorithmType.Sha3_384 => SHA3_384.HashData(fs),
      HashAlgorithmType.Sha3_512 => SHA3_512.HashData(fs),

      HashAlgorithmType.Shake128 => Shake128.HashData(fs, shake_output_length),
      HashAlgorithmType.Shake256 => Shake256.HashData(fs, shake_output_length),

      _ => throw new NotSupportedException($"Unsupported algorithm: {halgotype}"),
    };
  }

  public static byte[] ComputeHmacFile(
    string filename, ReadOnlySpan<byte> key,
    HashAlgorithmType halgotype)
  {
    using var fs = new FileStream(
      filename, FileMode.Open, FileAccess.Read);

    return ToHmacBasicAlgorithmType(halgotype) switch
    {
      HashAlgorithmType.Md5 => HMACMD5.HashData(key, fs),
      HashAlgorithmType.Sha1 => HMACSHA1.HashData(key, fs),

      HashAlgorithmType.Sha256 => HMACSHA256.HashData(key, fs),
      HashAlgorithmType.Sha384 => HMACSHA384.HashData(key, fs),
      HashAlgorithmType.Sha512 => HMACSHA512.HashData(key, fs),

      HashAlgorithmType.Sha3_256 => HMACSHA3_256.HashData(key, fs),
      HashAlgorithmType.Sha3_384 => HMACSHA3_384.HashData(key, fs),
      HashAlgorithmType.Sha3_512 => HMACSHA3_512.HashData(key, fs),

      _ => throw new NotSupportedException($"Unsupported algorithm: {halgotype}"),
    };
  }

  public static async Task<byte[]> ComputeHashFileAsync( 
    string filename, HashAlgorithmType halgotype,
    int shake_output_length = 32,
    CancellationToken ct = default)
  {
    await using var fs = new FileStream(
      filename, FileMode.Open,
      FileAccess.Read, FileShare.Read,
      bufferSize: 81920, useAsync: true);

    return halgotype switch
    {
      HashAlgorithmType.Md5 => await MD5.HashDataAsync(fs, ct),
      HashAlgorithmType.Sha1 => await SHA1.HashDataAsync(fs, ct),

      HashAlgorithmType.Sha256 => await SHA256.HashDataAsync(fs, ct),
      HashAlgorithmType.Sha384 => await SHA384.HashDataAsync(fs, ct),
      HashAlgorithmType.Sha512 => await SHA512.HashDataAsync(fs, ct),

      HashAlgorithmType.Sha3_256 => await SHA3_256.HashDataAsync(fs, ct),
      HashAlgorithmType.Sha3_384 => await SHA3_384.HashDataAsync(fs, ct),
      HashAlgorithmType.Sha3_512 => await SHA3_512.HashDataAsync(fs, ct),

      HashAlgorithmType.Shake128 => await Shake128.HashDataAsync(fs, shake_output_length, ct),
      HashAlgorithmType.Shake256 => await Shake256.HashDataAsync(fs, shake_output_length, ct),

      _ => throw new NotSupportedException($"Unsupported algorithm: {halgotype}")
    };
  }

  public static async Task<byte[]> ComputeHmacFileAsync(
    string filename, ReadOnlyMemory<byte> key,
    HashAlgorithmType halgotype, CancellationToken ct = default)
  {
    await using var fs = new FileStream(
      filename, FileMode.Open,
      FileAccess.Read, FileShare.Read,
      bufferSize: 81920, useAsync: true);

    return ToHmacBasicAlgorithmType(halgotype) switch
    {
      HashAlgorithmType.Md5 => await HMACMD5.HashDataAsync(key, fs, ct),
      HashAlgorithmType.Sha1 => await HMACSHA1.HashDataAsync(key, fs, ct),

      HashAlgorithmType.Sha256 => await HMACSHA256.HashDataAsync(key, fs, ct),
      HashAlgorithmType.Sha384 => await HMACSHA384.HashDataAsync(key, fs, ct),
      HashAlgorithmType.Sha512 => await HMACSHA512.HashDataAsync(key, fs, ct),

      HashAlgorithmType.Sha3_256 => await HMACSHA3_256.HashDataAsync(key, fs, ct),
      HashAlgorithmType.Sha3_384 => await HMACSHA3_384.HashDataAsync(key, fs, ct),
      HashAlgorithmType.Sha3_512 => await HMACSHA3_512.HashDataAsync(key, fs, ct),

      _ => throw new NotSupportedException($"Unsupported algorithm: {halgotype}")
    };
  }


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

      var _ when halgoname == HashAlgorithmName.SHA3_256 => HashAlgorithmType.Sha3_256,
      var _ when halgoname == HashAlgorithmName.SHA3_384 => HashAlgorithmType.Sha3_384,
      var _ when halgoname == HashAlgorithmName.SHA3_512 => HashAlgorithmType.Sha3_512,

      //var _ when halgoname == HashAlgorithmName.Shake128 => HashAlgorithmType.Shake128,
      //var _ when halgoname == HashAlgorithmName.Shake256 => HashAlgorithmType.Shake256,

      _ => throw new NotSupportedException($"Unsupported algorithm: {halgoname}"),
    };
  }

  public static HashAlgorithmName FromHashAlgorithmType(
    HashAlgorithmType halgoname)
  {
    return halgoname switch
    {
      HashAlgorithmType.Md5 => HashAlgorithmName.MD5,
      HashAlgorithmType.Sha1 => HashAlgorithmName.SHA1,

      HashAlgorithmType.Sha256 => HashAlgorithmName.SHA256,
      HashAlgorithmType.Sha384 => HashAlgorithmName.SHA384,
      HashAlgorithmType.Sha512 => HashAlgorithmName.SHA512,

      HashAlgorithmType.Sha3_256 => HashAlgorithmName.SHA3_256,
      HashAlgorithmType.Sha3_384 => HashAlgorithmName.SHA3_384,
      HashAlgorithmType.Sha3_512 => HashAlgorithmName.SHA3_512,

      _ => throw new NotSupportedException($"Unsupported algorithm: {halgoname}"),
    };
  }

  public static HashAlgorithmType ToHmacBasicAlgorithmType(
    HashAlgorithmType halgoname)
  {
    return halgoname switch
    {
      HashAlgorithmType.HmacMd5 => HashAlgorithmType.Md5,
      HashAlgorithmType.HmacSha1 => HashAlgorithmType.Sha1,

      HashAlgorithmType.HmacSha256 => HashAlgorithmType.Sha256,
      HashAlgorithmType.HmacSha384 => HashAlgorithmType.Sha384,
      HashAlgorithmType.HmacSha512 => HashAlgorithmType.Sha512,

      HashAlgorithmType.HmacSha3_256 => HashAlgorithmType.Sha3_256,
      HashAlgorithmType.HmacSha3_384 => HashAlgorithmType.Sha3_384,
      HashAlgorithmType.HmacSha3_512 => HashAlgorithmType.Sha3_512,

      HashAlgorithmType.Md5 => HashAlgorithmType.Md5,
      HashAlgorithmType.Sha1 => HashAlgorithmType.Sha1,

      HashAlgorithmType.Sha256 => HashAlgorithmType.Sha256,
      HashAlgorithmType.Sha384 => HashAlgorithmType.Sha384,
      HashAlgorithmType.Sha512 => HashAlgorithmType.Sha512,

      HashAlgorithmType.Sha3_256 => HashAlgorithmType.Sha3_256,
      HashAlgorithmType.Sha3_384 => HashAlgorithmType.Sha3_384,
      HashAlgorithmType.Sha3_512 => HashAlgorithmType.Sha3_512,
      _ => throw new NotSupportedException($"Unsupported algorithm: {halgoname}"),
    };
  }

  public static HashAlgorithmType FromHmacBasicAlgorithmType(
    HashAlgorithmType halgoname)
  {
    return halgoname switch
    {
      HashAlgorithmType.Md5 => HashAlgorithmType.HmacMd5,
      HashAlgorithmType.Sha1 => HashAlgorithmType.HmacSha1,

      HashAlgorithmType.Sha256 => HashAlgorithmType.HmacSha256,
      HashAlgorithmType.Sha384 => HashAlgorithmType.HmacSha384,
      HashAlgorithmType.Sha512 => HashAlgorithmType.HmacSha512,

      HashAlgorithmType.Sha3_256 => HashAlgorithmType.HmacSha3_256,
      HashAlgorithmType.Sha3_384 => HashAlgorithmType.HmacSha3_384,
      HashAlgorithmType.Sha3_512 => HashAlgorithmType.HmacSha3_512,

      HashAlgorithmType.HmacMd5 => HashAlgorithmType.HmacMd5,
      HashAlgorithmType.HmacSha1 => HashAlgorithmType.HmacSha1,

      HashAlgorithmType.HmacSha256 => HashAlgorithmType.HmacSha256,
      HashAlgorithmType.HmacSha384 => HashAlgorithmType.HmacSha384,
      HashAlgorithmType.HmacSha512 => HashAlgorithmType.HmacSha512,

      HashAlgorithmType.HmacSha3_256 => HashAlgorithmType.HmacSha3_256,
      HashAlgorithmType.HmacSha3_384 => HashAlgorithmType.HmacSha3_384,
      HashAlgorithmType.HmacSha3_512 => HashAlgorithmType.HmacSha3_512,

      _ => throw new NotSupportedException($"Unsupported algorithm: {halgoname}"),
    };
  }

  public enum HashAlgorithmType : byte
  {
    // Classic hashes
    Md5,
    Sha1,
    Sha256,
    Sha384,
    Sha512,

    // SHA3 family
    Sha3_256,
    Sha3_384,
    Sha3_512,

    // SHAKE (XOF)
    Shake128,
    Shake256,

    // Classic HMAC-SHA variants
    HmacMd5,
    HmacSha1,
    HmacSha256,
    HmacSha384,
    HmacSha512,

    //HMAC-SHA3 family
    HmacSha3_256,
    HmacSha3_384,
    HmacSha3_512
  }
}
