
//using System.Text;
//using System.IO.Compression;
//using System.Runtime.InteropServices; 
//using System.Runtime.CompilerServices;


//namespace michele.natale;

//using Compresses;

//partial class NetServicesCompresses
//{
//  #region File Compress Package - Pack

//  public async static Task<(long TotalFileSize, long TotalCompressSize)> PackFileAsync(
//      string[] filepathlist, string archivepath, CompressionType compressiontype,
//      int buffersize = 81920, CompressionLevel compresslevel = CompressionLevel.Optimal) =>
//        await PackFileAsync(filepathlist, archivepath, (byte)compressiontype, buffersize, compresslevel);

//  public async static Task<(long TotalFileSize, long TotalCompressSize)> PackFileAsync(
//      string[] filepathlist, string archivepath, byte compressiontype,
//      int buffersize = 81920, CompressionLevel compresslevel = CompressionLevel.Optimal)
//  {
//    var archiv = await ServicesCompress.CheckFCPFileExtensionAsync(archivepath);
//    await using var fsout = new FileStream(archiv.FullName, FileMode.Create, FileAccess.Write);
//    Console.WriteLine($"© FileCompressPackage 2025 - FILE PACKAGE - Created by © Michele Natale 2025");

//    var filesizesum = 0L;
//    foreach (var src in filepathlist)
//    {
//      await using var fsin = new FileStream(src, FileMode.Open, FileAccess.Read);
//      await WriteFileAsync(fsin, fsout, src, compressiontype, buffersize, compresslevel);
//      filesizesum += fsin.Length;
//    }

//    await fsout.FlushAsync();

//    var destlength = fsout.Length;
//    return (filesizesum, destlength);
//  }

//  private async static Task WriteFileAsync(
//    Stream input, Stream output, string filepath, byte compresstype,
//    int buffersize = 81920, CompressionLevel compresslevel = CompressionLevel.Optimal)
//  {
//    await output.FlushAsync();

//    var pos = output.Position;
//    var fileinfos = new FileInfo(filepath);
//    var namebytes = Encoding.UTF8.GetBytes(fileinfos.Name);
//    var headerbuffer = new byte[Unsafe.SizeOf<FileInfosHeader>()];

//    var ct = (CompressionType)compresstype;
//    output.Position = pos + headerbuffer.Length + namebytes.LongLength;

//    var compresslength = ct switch
//    {
//      CompressionType.None => await ServicesCompress.CopyChunkAsync(
//        input, output, 0, input.Length, buffersize),
//      CompressionType.GZip => await CompressedNet
//        .CompressGZipAsyncSpec(input, output, buffersize, compresslevel),
//      CompressionType.Brotli => await CompressedNet
//        .CompressBrotliAsyncSpec(input, output, buffersize, compresslevel),
//      _ => throw new InvalidOperationException(),
//    };

//    var header = new FileInfosHeader
//    {
//      FCP_Type = (byte)FcpType.File,
//      CompressionType = compresstype,
//      Version = FcpVersion.ToLong(),
//      OriginalLength = fileinfos.Length,
//      CompressedLength = compresslength,
//      NameLength = (byte)namebytes.Length,
//      CreationTimeUtcTicks = fileinfos.CreationTimeUtc.Ticks,
//      LastWriteTimeUtcTicks = fileinfos.LastWriteTimeUtc.Ticks,
//      LastAccessTimeUtcTicks = fileinfos.LastAccessTimeUtc.Ticks,
//    };

//    await output.FlushAsync();

//    output.Position = pos;
//    MemoryMarshal.Write(headerbuffer, in header);
//    output.Write(headerbuffer);
//    output.Write(namebytes, 0, namebytes.Length);

//    var newlength = pos + headerbuffer.LongLength + namebytes.LongLength + compresslength;
//    output.Position = newlength;
//    output.SetLength(newlength);

//    Console.WriteLine($"CompressType = {(CompressionType)header.CompressionType}, " +
//    $"PACK FILE: {Path.GetFileNameWithoutExtension(filepath)} ({header.OriginalLength} Bytes), " +
//    $"Created: {fileinfos.CreationTimeUtc}, Last Accessed: {fileinfos.LastAccessTimeUtc}, " +
//    $"Last Updated: {fileinfos.LastWriteTimeUtc}, Compression Ratio = {header.CompressedLength / (double)header.OriginalLength}");
//  }

//  #endregion File Compress Package - Pack



//}
