

using System.IO.Compression;
using System.Runtime.InteropServices;

namespace michele.natale.CAbiBridge;


using Compresses;
using static CryptoBridge;
using static NetServicesUtils;
using static NetServicesCompresses;

partial class CompressBridge
{
  #region Pack File Compress - File Pack

  [UnmanagedCallersOnly(EntryPoint = "pack_file_aot")]
  public unsafe static CError PackFileAot(
    byte** pack_list_utf8_ptr, int* pack_list_length, int pack_list_count,
    byte* archiv_path_utf8_ptr, int archiv_path_length, byte compresstype,
    long* total_file_size, long* total_compress_size) =>
     PackFileImplement(
       pack_list_utf8_ptr, pack_list_length, pack_list_count,
       archiv_path_utf8_ptr, archiv_path_length,
       compresstype, BUFFER_SIZE_DEFAULT, CompressionLevel.Optimal,
       total_file_size, total_compress_size);


  [UnmanagedCallersOnly(EntryPoint = "pack_file_bs_cl_aot")]
  public unsafe static CError PackFileBsCLAot(
    byte** pack_list_utf8_ptr, int* pack_list_length, int pack_list_count,
    byte* archiv_path_utf8_ptr, int archiv_path_length,
    byte compresstype, int buffersize, byte compresslevel,
    long* total_file_size, long* total_compress_size) =>
     PackFileImplement(
       pack_list_utf8_ptr, pack_list_length, pack_list_count,
       archiv_path_utf8_ptr, archiv_path_length,
       compresstype, buffersize, (CompressionLevel)compresslevel,
       total_file_size, total_compress_size);

  private unsafe static CError PackFileImplement(
    byte** pack_list_utf8_ptr, int* pack_list_length, int pack_list_count,
    byte* archiv_path_utf8_ptr, int archiv_path_length,
    byte compresstype, int buffersize, CompressionLevel compresslevel,
    long* total_file_size, long* total_compress_size)
  {
    try
    {
      if (pack_list_utf8_ptr is null || pack_list_length is null || pack_list_count <= 0)
        return new CError { error_code = (int)CErrorCode.InvalidLength };

      if (archiv_path_utf8_ptr is null || archiv_path_length <= 0)
        return new CError { error_code = (int)CErrorCode.InvalidLength };

      if (buffersize < 0) buffersize = BUFFER_SIZE_DEFAULT;

      var pack_list = ToStringList(pack_list_utf8_ptr, pack_list_length, pack_list_count);
      var archive_path = ToStringUtf8Safe(archiv_path_utf8_ptr, archiv_path_length);

      var (totalfilesize, totalcompresssize) =
        FileCompressPackage.PackFileAsync(
          pack_list, archive_path, compresstype,
          buffersize, compresslevel)
          .GetAwaiter().GetResult();

      *total_file_size = totalfilesize;
      *total_compress_size = totalcompresssize;


      return new CError { error_code = (int)CErrorCode.Ok };
    }
    catch (Exception ex)
    {
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }

  #endregion Pack File Compress - File Pack

  #region Pack File Compress - Archiv Pack

  [UnmanagedCallersOnly(EntryPoint = "pack_archiv_aot")]
  public unsafe static CError PackArchivAot(
    byte* src_folder_utf8_ptr, int src_folder_length,
    byte* dest_folder_utf8_ptr, int dest_folder_length, byte compresstype,
    long* total_file_size, long* total_compress_size) =>
      PackArchivImplement(
        src_folder_utf8_ptr, src_folder_length,
        dest_folder_utf8_ptr, dest_folder_length,
        compresstype, BUFFER_SIZE_DEFAULT, CompressionLevel.Optimal,
        total_file_size, total_compress_size);


  [UnmanagedCallersOnly(EntryPoint = "pack_archiv_bs_cl_aot")]
  public unsafe static CError PackArchivBsCLAot(
     byte* src_folder_utf8_ptr, int src_folder_length,
    byte* dest_folder_utf8_ptr, int dest_folder_length,
    byte compresstype, int buffersize, byte compresslevel,
    long* total_file_size, long* total_compress_size) =>
      PackArchivImplement(
        src_folder_utf8_ptr, src_folder_length,
        dest_folder_utf8_ptr, dest_folder_length,
        compresstype, buffersize, (CompressionLevel)compresslevel,
        total_file_size, total_compress_size);

  private unsafe static CError PackArchivImplement(
    byte* src_folder_utf8_ptr, int src_folder_length,
    byte* dest_folder_utf8_ptr, int dest_folder_length,
    byte compresstype, int buffersize, CompressionLevel compresslevel,
    long* total_file_size, long* total_compress_size)
  {
    try
    {

      if (src_folder_utf8_ptr is null || src_folder_length <= 0)
        return new CError { error_code = (int)CErrorCode.InvalidLength };

      if (dest_folder_utf8_ptr is null || dest_folder_length <= 0)
        return new CError { error_code = (int)CErrorCode.InvalidLength };

      if (buffersize < 0) buffersize = BUFFER_SIZE_DEFAULT;

      var pack_list = ToStringUtf8Safe(src_folder_utf8_ptr, src_folder_length);
      var archive_path = ToStringUtf8Safe(dest_folder_utf8_ptr, dest_folder_length);

      var (totalfilesize, totalcompresssize) =
        FileCompressPackage.PackArchivAsync(
          pack_list, archive_path, compresstype,
          buffersize, compresslevel)
          .GetAwaiter().GetResult();

      *total_file_size = totalfilesize;
      *total_compress_size = totalcompresssize;


      return new CError { error_code = (int)CErrorCode.Ok };
    }
    catch (Exception ex)
    {
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }

  #endregion Pack File Compress - Archiv Pack

  #region Pack File Compress - UnPack

  [UnmanagedCallersOnly(EntryPoint = "unpack_file_archiv_aot")]
  public unsafe static CError UnPackFileArchivAot(
    byte* archiv_path_ptr, int archiv_path_length,
    byte* output_folder_ptr, int output_folder_length) =>
      UnPackFileArchivImplement(
        archiv_path_ptr, archiv_path_length,
        output_folder_ptr, output_folder_length,
        BUFFER_SIZE_DEFAULT);

  [UnmanagedCallersOnly(EntryPoint = "unpack_file_archiv_bs_aot")]
  public unsafe static CError UnPackFileArchivBsAot(
    byte* archiv_path_ptr, int archiv_path_length,
    byte* output_folder_ptr, int output_folder_length,
    int buffersize) =>
      UnPackFileArchivImplement(
        archiv_path_ptr, archiv_path_length,
        output_folder_ptr, output_folder_length,
        buffersize);

  public unsafe static CError UnPackFileArchivImplement(
    byte* archiv_path_ptr, int archiv_path_length,
    byte* output_folder_ptr, int output_folder_length,
    int buffersize)
  {
    try
    {

      if (archiv_path_ptr is null || archiv_path_length <= 0)
        return new CError { error_code = (int)CErrorCode.InvalidLength };

      if (output_folder_ptr is null || output_folder_length <= 0)
        return new CError { error_code = (int)CErrorCode.InvalidLength };

      if (buffersize <= 0) buffersize = BUFFER_SIZE_DEFAULT;

      var src = ToStringUtf8Safe(archiv_path_ptr, archiv_path_length);
      var dest = ToStringUtf8Safe(output_folder_ptr, output_folder_length);

      FileCompressPackage.UnPackAsync(src, dest, buffersize)
        .GetAwaiter().GetResult();

      return new CError { error_code = (int)CErrorCode.Ok };
    }
    catch (Exception ex)
    {
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }

  #endregion Pack File Compress - UnPack

  #region Pack File Compress - Utils

  private unsafe static string[] ToStringList(
    byte** pack_list_utf8_ptr, int* pack_list_length, int count)
  {

    if (pack_list_utf8_ptr is null || pack_list_length is null || count <= 0)
      throw new ArgumentNullException(nameof(pack_list_utf8_ptr));

    var result = new List<string>(count);

    for (var i = 0; i < count; i++)
      result.Add(ToStringUtf8Safe(
        *(pack_list_utf8_ptr + i),
        *(pack_list_length + i)));

    return [.. result];
  }

  #endregion Pack File Compress - Utils
}
