

using System.IO.Compression;
using System.Runtime.InteropServices;

namespace michele.natale.CAbiBridge;

using static CryptoBridge;
using static NetServicesUtils;
using static NetServicesCompresses;


partial class CompressBridge
{

  [UnmanagedCallersOnly(EntryPoint = "compress_message_brotli_aot")]
  public unsafe static CError CompressMessageBrotliAot(
    byte* bytes_ptr, int length,
    byte compresslevel,
    byte** out_ptr, int* out_length)
  {
    *out_ptr = null; *out_length = 0;

    try
    {
      if (bytes_ptr is null || length <= 0)
        return new CError { error_code = (int)CErrorCode.InvalidLength };

      var bytes = ToMemSafe(bytes_ptr, length);

      var result = CompressBrotliAsync(
        bytes, CancellationToken.None, (CompressionLevel)compresslevel)
          .GetAwaiter().GetResult();

      var buffer = (byte*)NativeMemory.Alloc((nuint)result.Length);
      result.CopyTo(new Span<byte>(buffer, result.Length));
      *out_ptr = buffer; *out_length = result.Length;

      return new CError { error_code = (int)CErrorCode.Ok };
    }
    catch (Exception ex)
    {
      CheckSetZero((nint*)*out_ptr); *out_length = 0;
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }

  [UnmanagedCallersOnly(EntryPoint = "decompress_message_brotli_aot")]
  public unsafe static CError DecompressMessageBrotliAot(
  byte* bytes_ptr, int length,
  byte** out_ptr, int* out_length)
  {
    *out_ptr = null; *out_length = 0;

    try
    {
      if (bytes_ptr is null || length <= 0)
        return new CError { error_code = (int)CErrorCode.InvalidLength };

      var bytes = ToBytesSafe(bytes_ptr, length);

      var result = DecompressBrotliAsync(
        bytes, CancellationToken.None)
          .GetAwaiter().GetResult();

      var buffer = (byte*)NativeMemory.Alloc((nuint)result.Length);
      result.CopyTo(new Span<byte>(buffer, result.Length));
      *out_ptr = buffer; *out_length = result.Length;

      return new CError { error_code = (int)CErrorCode.Ok };
    }
    catch (Exception ex)
    {
      CheckSetZero((nint*)*out_ptr); *out_length = 0;
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }



  [UnmanagedCallersOnly(EntryPoint = "compress_file_brotli_aot")]
  public unsafe static CError CompressFileBrotliAot(
    byte* src_utf8_ptr, int src_length,
    byte* dest_utf8_ptr, int dest_length,
    byte compresslevel) =>
      CompressFileBrotliImplement(
        src_utf8_ptr, src_length,
        dest_utf8_ptr, dest_length,
        BUFFER_SIZE_DEFAULT, compresslevel);

  [UnmanagedCallersOnly(EntryPoint = "compress_file_buffer_size_brotli_aot")]
  public unsafe static CError CompressFileBufferSizeBrotliAot(
    byte* src_utf8_ptr, int src_length,
    byte* dest_utf8_ptr, int dest_length,
    int buffersize, byte compresslevel) =>
      CompressFileBrotliImplement(
        src_utf8_ptr, src_length,
        dest_utf8_ptr, dest_length,
        buffersize, compresslevel);

  private static unsafe CError CompressFileBrotliImplement(
    byte* src_utf8_ptr, int src_length,
    byte* dest_utf8_ptr, int dest_length,
    int buffersize, byte compresslevel)
  {
    try
    {
      if (src_utf8_ptr is null || src_length <= 0)
        return new CError { error_code = (int)CErrorCode.InvalidLength };

      if (dest_utf8_ptr is null || dest_length <= 0)
        return new CError { error_code = (int)CErrorCode.InvalidLength };

      if (buffersize <= 0) buffersize = BUFFER_SIZE_DEFAULT;

      var src = ToStringUtf8Safe(src_utf8_ptr, src_length);
      var dest = ToStringUtf8Safe(dest_utf8_ptr, dest_length);

      CompressBrotliAsync(src, dest, CancellationToken.None,
        buffersize, (CompressionLevel)compresslevel)
          .GetAwaiter().GetResult();

      return new CError { error_code = (int)CErrorCode.Ok };
    }
    catch (Exception ex)
    {
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }



  [UnmanagedCallersOnly(EntryPoint = "decompress_file_brotli_aot")]
  public unsafe static CError DecompressFileBrotliAot(
    byte* src_utf8_ptr, int src_length,
    byte* dest_utf8_ptr, int dest_length) =>
      DecompressFileBrotliImplement(
        src_utf8_ptr, src_length,
        dest_utf8_ptr, dest_length,
        BUFFER_SIZE_DEFAULT);

  [UnmanagedCallersOnly(EntryPoint = "decompress_file_buffer_size_brotli_aot")]
  public unsafe static CError DecompressFileBufferSizeBrotliAot(
    byte* src_utf8_ptr, int src_length,
    byte* dest_utf8_ptr, int dest_length,
    int buffersize) =>
      DecompressFileBrotliImplement(
        src_utf8_ptr, src_length,
        dest_utf8_ptr, dest_length,
        buffersize);

  private static unsafe CError DecompressFileBrotliImplement(
    byte* src_utf8_ptr, int src_length,
    byte* dest_utf8_ptr, int dest_length,
    int buffersize)
  {
    try
    {
      if (src_utf8_ptr is null || src_length <= 0)
        return new CError { error_code = (int)CErrorCode.InvalidLength };

      if (dest_utf8_ptr is null || dest_length <= 0)
        return new CError { error_code = (int)CErrorCode.InvalidLength };

      if (buffersize <= 0) buffersize = BUFFER_SIZE_DEFAULT;

      var src = ToStringUtf8Safe(src_utf8_ptr, src_length);
      var dest = ToStringUtf8Safe(dest_utf8_ptr, dest_length);

      DecompressBrotliAsync(src, dest, CancellationToken.None, buffersize)
          .GetAwaiter().GetResult();

      return new CError { error_code = (int)CErrorCode.Ok };
    }
    catch (Exception ex)
    {
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }
}
