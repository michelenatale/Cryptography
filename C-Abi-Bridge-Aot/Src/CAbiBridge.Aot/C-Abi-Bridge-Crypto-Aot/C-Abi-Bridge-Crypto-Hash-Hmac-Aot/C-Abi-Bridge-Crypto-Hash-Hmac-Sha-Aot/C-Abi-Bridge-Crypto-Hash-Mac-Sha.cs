
using System.Runtime.InteropServices;

namespace michele.natale.CAbiBridge;

using static NetServicesUtils;
using static NetServicesCrypto;

partial class CryptoBridge
{
  [UnmanagedCallersOnly(EntryPoint = "sha_256_hash_data_aot")]
  public unsafe static CError Sha256HashDataAot(
    byte* bytes_ptr, int length,
    byte** sha_out_ptr, int* sha_out_length)
  {
    *sha_out_ptr = null; *sha_out_length = 0;

    try
    {
      if (bytes_ptr is null || length <= 0)
        return new CError { error_code = (int)CErrorCode.InvalidLength };

      var span = new ReadOnlySpan<byte>(bytes_ptr, length).ToArray();
      var result = ComputeHash(span, HashAlgorithmType.Sha256);

      var buffer = (byte*)NativeMemory.Alloc((nuint)result.Length);
      result.CopyTo(new Span<byte>(buffer, result.Length));
      *sha_out_ptr = buffer; *sha_out_length = result.Length;

      return new CError { error_code = (int)CErrorCode.Ok };
    }
    catch (Exception ex)
    {
      CheckSetZero((nint*)*sha_out_ptr);
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }

  [UnmanagedCallersOnly(EntryPoint = "sha_384_hash_data_aot")]
  public unsafe static CError Sha384HashDataAot(
    byte* bytes_ptr, int length,
    byte** sha_out_ptr, int* sha_out_length)
  {
    *sha_out_ptr = null; *sha_out_length = 0;

    try
    {
      if (bytes_ptr is null || length <= 0)
        return new CError { error_code = (int)CErrorCode.InvalidLength };

      var span = new ReadOnlySpan<byte>(bytes_ptr, length).ToArray();
      var result = ComputeHash(span, HashAlgorithmType.Sha384);

      var buffer = (byte*)NativeMemory.Alloc((nuint)result.Length);
      result.CopyTo(new Span<byte>(buffer, result.Length));
      *sha_out_ptr = buffer; *sha_out_length = result.Length;

      return new CError { error_code = (int)CErrorCode.Ok };
    }
    catch (Exception ex)
    {
      CheckSetZero((nint*)*sha_out_ptr);
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }

  [UnmanagedCallersOnly(EntryPoint = "sha_512_hash_data_aot")]
  public unsafe static CError Sha512HashDataAot(
    byte* bytes_ptr, int length,
    byte** sha_out_ptr, int* sha_out_length)
  {
    *sha_out_ptr = null; *sha_out_length = 0;

    try
    {
      if (bytes_ptr is null || length <= 0)
        return new CError { error_code = (int)CErrorCode.InvalidLength };

      var span = new ReadOnlySpan<byte>(bytes_ptr, length).ToArray();
      var result = ComputeHash(span, HashAlgorithmType.Sha512);

      var buffer = (byte*)NativeMemory.Alloc((nuint)result.Length);
      result.CopyTo(new Span<byte>(buffer, result.Length));
      *sha_out_ptr = buffer; *sha_out_length = result.Length;

      return new CError { error_code = (int)CErrorCode.Ok };
    }
    catch (Exception ex)
    {
      CheckSetZero((nint*)*sha_out_ptr);
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }


  [UnmanagedCallersOnly(EntryPoint = "sha1_hash_data_aot")]
  public unsafe static CError Sha1HashDataAot(
  byte* bytes_ptr, int length,
  byte** sha_out_ptr, int* sha_out_length)
  {
    *sha_out_ptr = null; *sha_out_length = 0;

    try
    {
      if (bytes_ptr is null || length <= 0)
        return new CError { error_code = (int)CErrorCode.InvalidLength };

      var span = new ReadOnlySpan<byte>(bytes_ptr, length).ToArray();
      var result = ComputeHash(span, HashAlgorithmType.Sha1);

      var buffer = (byte*)NativeMemory.Alloc((nuint)result.Length);
      result.CopyTo(new Span<byte>(buffer, result.Length));
      *sha_out_ptr = buffer; *sha_out_length = result.Length;

      return new CError { error_code = (int)CErrorCode.Ok };
    }
    catch (Exception ex)
    {
      CheckSetZero((nint*)*sha_out_ptr);
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }

  [UnmanagedCallersOnly(EntryPoint = "md5_hash_data_aot")]
  public unsafe static CError Md5HashDataAot(
  byte* bytes_ptr, int length,
  byte** sha_out_ptr, int* sha_out_length)
  {
    *sha_out_ptr = null; *sha_out_length = 0;

    try
    {
      if (bytes_ptr is null || length <= 0)
        return new CError { error_code = (int)CErrorCode.InvalidLength };

      var span = new ReadOnlySpan<byte>(bytes_ptr, length).ToArray();
      var result = ComputeHash(span, HashAlgorithmType.Md5);

      var buffer = (byte*)NativeMemory.Alloc((nuint)result.Length);
      result.CopyTo(new Span<byte>(buffer, result.Length));
      *sha_out_ptr = buffer; *sha_out_length = result.Length;

      return new CError { error_code = (int)CErrorCode.Ok };
    }
    catch (Exception ex)
    {
      CheckSetZero((nint*)*sha_out_ptr);
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }


  [UnmanagedCallersOnly(EntryPoint = "sha3_256_hash_data_aot")]
  public unsafe static CError Sha3256HashDataAot(
    byte* bytes_ptr, int length,
    byte** sha_out_ptr, int* sha_out_length)
  {
    *sha_out_ptr = null; *sha_out_length = 0;

    try
    {
      if (bytes_ptr is null || length <= 0)
        return new CError { error_code = (int)CErrorCode.InvalidLength };

      var span = new ReadOnlySpan<byte>(bytes_ptr, length).ToArray();
      var result = ComputeHash(span, HashAlgorithmType.Sha3_256);

      var buffer = (byte*)NativeMemory.Alloc((nuint)result.Length);
      result.CopyTo(new Span<byte>(buffer, result.Length));
      *sha_out_ptr = buffer; *sha_out_length = result.Length;

      return new CError { error_code = (int)CErrorCode.Ok };
    }
    catch (Exception ex)
    {
      CheckSetZero((nint*)*sha_out_ptr);
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }

  [UnmanagedCallersOnly(EntryPoint = "sha3_384_hash_data_aot")]
  public unsafe static CError Sha3384HashDataAot(
    byte* bytes_ptr, int length,
    byte** sha_out_ptr, int* sha_out_length)
  {
    *sha_out_ptr = null; *sha_out_length = 0;

    try
    {
      if (bytes_ptr is null || length <= 0)
        return new CError { error_code = (int)CErrorCode.InvalidLength };

      var span = new ReadOnlySpan<byte>(bytes_ptr, length).ToArray();
      var result = ComputeHash(span, HashAlgorithmType.Sha3_384);

      var buffer = (byte*)NativeMemory.Alloc((nuint)result.Length);
      result.CopyTo(new Span<byte>(buffer, result.Length));
      *sha_out_ptr = buffer; *sha_out_length = result.Length;

      return new CError { error_code = (int)CErrorCode.Ok };
    }
    catch (Exception ex)
    {
      CheckSetZero((nint*)*sha_out_ptr);
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }

  [UnmanagedCallersOnly(EntryPoint = "sha3_512_hash_data_aot")]
  public unsafe static CError Sha3512HashDataAot(
    byte* bytes_ptr, int length,
    byte** sha_out_ptr, int* sha_out_length)
  {
    *sha_out_ptr = null; *sha_out_length = 0;

    try
    {
      if (bytes_ptr is null || length <= 0)
        return new CError { error_code = (int)CErrorCode.InvalidLength };

      var span = new ReadOnlySpan<byte>(bytes_ptr, length).ToArray();
      var result = ComputeHash(span, HashAlgorithmType.Sha3_512);

      var buffer = (byte*)NativeMemory.Alloc((nuint)result.Length);
      result.CopyTo(new Span<byte>(buffer, result.Length));
      *sha_out_ptr = buffer; *sha_out_length = result.Length;

      return new CError { error_code = (int)CErrorCode.Ok };
    }
    catch (Exception ex)
    {
      CheckSetZero((nint*)*sha_out_ptr);
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }

  [UnmanagedCallersOnly(EntryPoint = "shake_128_hash_data_aot")]
  public unsafe static CError Shake128HashDataAot(
    byte* bytes_ptr, int bytes_length,
    int shake_out_length, byte** shake_out_ptr)
  {
    *shake_out_ptr = null; 

    try
    {
      if (bytes_ptr is null || bytes_length <= 0)
        return new CError { error_code = (int)CErrorCode.InvalidLength };

      var span = new ReadOnlySpan<byte>(bytes_ptr, bytes_length).ToArray();
      var result = ComputeHash(span, HashAlgorithmType.Shake128, shake_out_length);

      if (result.Length != shake_out_length)
        throw new ArgumentOutOfRangeException(nameof(shake_out_length),
          $"Result.Length != {nameof(shake_out_length)}.Length!!");

      var buffer = (byte*)NativeMemory.Alloc((nuint)result.Length);
      result.CopyTo(new Span<byte>(buffer, result.Length));
      *shake_out_ptr = buffer;

      return new CError { error_code = (int)CErrorCode.Ok };
    }
    catch (Exception ex)
    {
      CheckSetZero((nint*)*shake_out_ptr);
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }

  [UnmanagedCallersOnly(EntryPoint = "shake_256_hash_data_aot")]
  public unsafe static CError Shake256HashDataAot(
    byte* bytes_ptr, int bytes_length,
    int shake_out_length, byte** shake_out_ptr)
  {
    *shake_out_ptr = null; 

    try
    {
      if (bytes_ptr is null || bytes_length <= 0)
        return new CError { error_code = (int)CErrorCode.InvalidLength };

      var span = new ReadOnlySpan<byte>(bytes_ptr, bytes_length).ToArray();
      var result = ComputeHash(span, HashAlgorithmType.Shake256, shake_out_length);

      if (result.Length != shake_out_length)
        throw new ArgumentOutOfRangeException(nameof(shake_out_length),
          $"Result.Length != {nameof(shake_out_length)}.Length!!");

      var buffer = (byte*)NativeMemory.Alloc((nuint)result.Length);
      result.CopyTo(new Span<byte>(buffer, result.Length));
      *shake_out_ptr = buffer;

      return new CError { error_code = (int)CErrorCode.Ok };
    }
    catch (Exception ex)
    {
      CheckSetZero((nint*)*shake_out_ptr);
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }

  [UnmanagedCallersOnly(EntryPoint = "hmac_sha_256_hash_data_aot")]
  public unsafe static CError HmacSha256HashDataAot(
    byte* bytes_ptr, int bytes_length,
    byte* key_ptr, int key_length,
    byte** sha_out_ptr, int* sha_out_length)
  {
    *sha_out_ptr = null; *sha_out_length = 0;

    try
    {
      if (bytes_ptr is null || bytes_length <= 0)
        return new CError { error_code = (int)CErrorCode.InvalidLength };

      if (key_ptr is null || key_length <= 0)
        return new CError { error_code = (int)CErrorCode.InvalidLength };

      var span_key = new ReadOnlySpan<byte>(key_ptr, key_length).ToArray();
      var span_bytes = new ReadOnlySpan<byte>(bytes_ptr, bytes_length).ToArray();
      var result = ComputeHmac(span_bytes, span_key, HashAlgorithmType.Sha256);

      var buffer = (byte*)NativeMemory.Alloc((nuint)result.Length);
      result.CopyTo(new Span<byte>(buffer, result.Length));
      *sha_out_ptr = buffer; *sha_out_length = result.Length;

      return new CError { error_code = (int)CErrorCode.Ok };
    }
    catch (Exception ex)
    {
      CheckSetZero((nint*)*sha_out_ptr);
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }

  [UnmanagedCallersOnly(EntryPoint = "hmac_sha_384_hash_data_aot")]
  public unsafe static CError HmacSha384HashDataAot(
    byte* bytes_ptr, int bytes_length,
    byte* key_ptr, int key_length,
    byte** sha_out_ptr, int* sha_out_length)
  {
    *sha_out_ptr = null; *sha_out_length = 0;

    try
    {
      if (bytes_ptr is null || bytes_length <= 0)
        return new CError { error_code = (int)CErrorCode.InvalidLength };

      if (key_ptr is null || key_length <= 0)
        return new CError { error_code = (int)CErrorCode.InvalidLength };

      var span_key = new ReadOnlySpan<byte>(key_ptr, key_length).ToArray();
      var span_bytes = new ReadOnlySpan<byte>(bytes_ptr, bytes_length).ToArray();
      var result = ComputeHmac(span_bytes, span_key, HashAlgorithmType.Sha384);

      var buffer = (byte*)NativeMemory.Alloc((nuint)result.Length);
      result.CopyTo(new Span<byte>(buffer, result.Length));
      *sha_out_ptr = buffer; *sha_out_length = result.Length;

      return new CError { error_code = (int)CErrorCode.Ok };
    }
    catch (Exception ex)
    {
      CheckSetZero((nint*)*sha_out_ptr);
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }

  [UnmanagedCallersOnly(EntryPoint = "hmac_sha_512_hash_data_aot")]
  public unsafe static CError HmacSha512HashDataAot(
    byte* bytes_ptr, int bytes_length,
    byte* key_ptr, int key_length,
    byte** sha_out_ptr, int* sha_out_length)
  {
    *sha_out_ptr = null; *sha_out_length = 0;

    try
    {
      if (bytes_ptr is null || bytes_length <= 0)
        return new CError { error_code = (int)CErrorCode.InvalidLength };

      if (key_ptr is null || key_length <= 0)
        return new CError { error_code = (int)CErrorCode.InvalidLength };

      var span_key = new ReadOnlySpan<byte>(key_ptr, key_length).ToArray();
      var span_bytes = new ReadOnlySpan<byte>(bytes_ptr, bytes_length).ToArray();
      var result = ComputeHmac(span_bytes, span_key, HashAlgorithmType.Sha512);

      var buffer = (byte*)NativeMemory.Alloc((nuint)result.Length);
      result.CopyTo(new Span<byte>(buffer, result.Length));
      *sha_out_ptr = buffer; *sha_out_length = result.Length;

      return new CError { error_code = (int)CErrorCode.Ok };
    }
    catch (Exception ex)
    {
      CheckSetZero((nint*)*sha_out_ptr);
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }

  [UnmanagedCallersOnly(EntryPoint = "hmac_sha1_hash_data_aot")]
  public unsafe static CError HmacSha1HashDataAot(
    byte* bytes_ptr, int bytes_length,
    byte* key_ptr, int key_length,
    byte** sha_out_ptr, int* sha_out_length)
  {
    *sha_out_ptr = null; *sha_out_length = 0;

    try
    {
      if (bytes_ptr is null || bytes_length <= 0)
        return new CError { error_code = (int)CErrorCode.InvalidLength };

      if (key_ptr is null || key_length <= 0)
        return new CError { error_code = (int)CErrorCode.InvalidLength };

      var span_key = new ReadOnlySpan<byte>(key_ptr, key_length).ToArray();
      var span_bytes = new ReadOnlySpan<byte>(bytes_ptr, bytes_length).ToArray();
      var result = ComputeHmac(span_bytes, span_key, HashAlgorithmType.Sha1);

      var buffer = (byte*)NativeMemory.Alloc((nuint)result.Length);
      result.CopyTo(new Span<byte>(buffer, result.Length));
      *sha_out_ptr = buffer; *sha_out_length = result.Length;

      return new CError { error_code = (int)CErrorCode.Ok };
    }
    catch (Exception ex)
    {
      CheckSetZero((nint*)*sha_out_ptr);
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }

  [UnmanagedCallersOnly(EntryPoint = "hmac_md5_hash_data_aot")]
  public unsafe static CError HmacMd5HashDataAot(
    byte* bytes_ptr, int bytes_length,
    byte* key_ptr, int key_length,
    byte** sha_out_ptr, int* sha_out_length)
  {
    *sha_out_ptr = null; *sha_out_length = 0;

    try
    {
      if (bytes_ptr is null || bytes_length <= 0)
        return new CError { error_code = (int)CErrorCode.InvalidLength };

      if (key_ptr is null || key_length <= 0)
        return new CError { error_code = (int)CErrorCode.InvalidLength };

      var span_key = new ReadOnlySpan<byte>(key_ptr, key_length).ToArray();
      var span_bytes = new ReadOnlySpan<byte>(bytes_ptr, bytes_length).ToArray();
      var result = ComputeHmac(span_bytes, span_key, HashAlgorithmType.Md5);

      var buffer = (byte*)NativeMemory.Alloc((nuint)result.Length);
      result.CopyTo(new Span<byte>(buffer, result.Length));
      *sha_out_ptr = buffer; *sha_out_length = result.Length;

      return new CError { error_code = (int)CErrorCode.Ok };
    }
    catch (Exception ex)
    {
      CheckSetZero((nint*)*sha_out_ptr);
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }

  [UnmanagedCallersOnly(EntryPoint = "hmac_sha3_256_hash_data_aot")]
  public unsafe static CError HmacSha3256HashDataAot(
    byte* bytes_ptr, int bytes_length,
    byte* key_ptr, int key_length,
    byte** sha_out_ptr, int* sha_out_length)
  {
    *sha_out_ptr = null; *sha_out_length = 0;

    try
    {
      if (bytes_ptr is null || bytes_length <= 0)
        return new CError { error_code = (int)CErrorCode.InvalidLength };

      if (key_ptr is null || key_length <= 0)
        return new CError { error_code = (int)CErrorCode.InvalidLength };

      var span_key = new ReadOnlySpan<byte>(key_ptr, key_length).ToArray();
      var span_bytes = new ReadOnlySpan<byte>(bytes_ptr, bytes_length).ToArray();
      var result = ComputeHmac(span_bytes, span_key, HashAlgorithmType.Sha3_256);

      var buffer = (byte*)NativeMemory.Alloc((nuint)result.Length);
      result.CopyTo(new Span<byte>(buffer, result.Length));
      *sha_out_ptr = buffer; *sha_out_length = result.Length;

      return new CError { error_code = (int)CErrorCode.Ok };
    }
    catch (Exception ex)
    {
      CheckSetZero((nint*)*sha_out_ptr);
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }

  [UnmanagedCallersOnly(EntryPoint = "hmac_sha3_384_hash_data_aot")]
  public unsafe static CError HmacSha3384HashDataAot(
    byte* bytes_ptr, int bytes_length,
    byte* key_ptr, int key_length,
    byte** sha_out_ptr, int* sha_out_length)
  {
    *sha_out_ptr = null; *sha_out_length = 0;

    try
    {
      if (bytes_ptr is null || bytes_length <= 0)
        return new CError { error_code = (int)CErrorCode.InvalidLength };

      if (key_ptr is null || key_length <= 0)
        return new CError { error_code = (int)CErrorCode.InvalidLength };

      var span_key = new ReadOnlySpan<byte>(key_ptr, key_length).ToArray();
      var span_bytes = new ReadOnlySpan<byte>(bytes_ptr, bytes_length).ToArray();
      var result = ComputeHmac(span_bytes, span_key, HashAlgorithmType.Sha3_384);

      var buffer = (byte*)NativeMemory.Alloc((nuint)result.Length);
      result.CopyTo(new Span<byte>(buffer, result.Length));
      *sha_out_ptr = buffer; *sha_out_length = result.Length;

      return new CError { error_code = (int)CErrorCode.Ok };
    }
    catch (Exception ex)
    {
      CheckSetZero((nint*)*sha_out_ptr);
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }

  [UnmanagedCallersOnly(EntryPoint = "hmac_sha3_512_hash_data_aot")]
  public unsafe static CError HmacSha3512HashDataAot(
    byte* bytes_ptr, int bytes_length,
    byte* key_ptr, int key_length,
    byte** sha_out_ptr, int* sha_out_length)
  {
    *sha_out_ptr = null; *sha_out_length = 0;

    try
    {
      if (bytes_ptr is null || bytes_length <= 0)
        return new CError { error_code = (int)CErrorCode.InvalidLength };

      if (key_ptr is null || key_length <= 0)
        return new CError { error_code = (int)CErrorCode.InvalidLength };

      var span_key = new ReadOnlySpan<byte>(key_ptr, key_length).ToArray();
      var span_bytes = new ReadOnlySpan<byte>(bytes_ptr, bytes_length).ToArray();
      var result = ComputeHmac(span_bytes, span_key, HashAlgorithmType.Sha3_512);

      var buffer = (byte*)NativeMemory.Alloc((nuint)result.Length);
      result.CopyTo(new Span<byte>(buffer, result.Length));
      *sha_out_ptr = buffer; *sha_out_length = result.Length;

      return new CError { error_code = (int)CErrorCode.Ok };
    }
    catch (Exception ex)
    {
      CheckSetZero((nint*)*sha_out_ptr);
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }
}
