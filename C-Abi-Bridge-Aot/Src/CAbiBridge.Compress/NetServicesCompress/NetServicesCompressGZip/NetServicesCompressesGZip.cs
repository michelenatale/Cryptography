
using michele.natale.Compresses;
using System.IO.Compression;

namespace michele.natale;


partial class NetServicesCompresses
{

  #region Async GZip Message Compress

  public async static Task<byte[]> CompressGZipAsync(
    ReadOnlyMemory<byte> bytes, CancellationToken ct,
    CompressionLevel compresslevel = CompressionLevel.Optimal)
  {
    AssertCompress(bytes.Span);

    await using var ms = new MemoryStream();
    await using (var gzip = new GZipStream(ms, compresslevel))
      await gzip.WriteAsync(bytes, ct).ConfigureAwait(false);

    return ms.ToArray();
  }

  public async static Task<byte[]> DecompressGZipAsync(
    byte[] bytes, CancellationToken ct)
  {
    await using var msout = new MemoryStream();
    await using var ms = new MemoryStream(bytes);
    await using (var gzip = new GZipStream(ms, CompressionMode.Decompress))
      await gzip.CopyToAsync(msout, ct).ConfigureAwait(false);

    return msout.ToArray();
  }

  #endregion Async GZip Message Compress

  #region Async GZip File Compress

  public async static Task CompressGZipAsync(
    string src, string dest, CancellationToken ct, int buffersize = 81920,
    CompressionLevel compresslevel = CompressionLevel.Optimal)
  {
    await using var fsin = new FileStream(src, FileMode.Open, FileAccess.Read);
    await using var fsout = new FileStream(dest, FileMode.Create, FileAccess.Write);
    await CompressGZipAsync(fsin, fsout, ct, buffersize, compresslevel).ConfigureAwait(false);
  }

  private async static Task CompressGZipAsync(
    Stream input, Stream output, CancellationToken ct, int buffersize = 81920,
    CompressionLevel compresslevel = CompressionLevel.Optimal)
  {
    int readbytes;
    var buffer = new byte[buffersize];

    await using var gzip = new GZipStream(output, compresslevel);
    while ((readbytes = await input.ReadAsync(
      buffer.AsMemory(0, buffer.Length), ct).ConfigureAwait(false)) > 0)
      await gzip.WriteAsync(buffer.AsMemory(0, readbytes), ct).ConfigureAwait(false);
  }

  public async static Task DecompressGZipAsync(
  string src, string dest, CancellationToken ct, int buffersize = 81920)
  {
    await using var fsin = new FileStream(src, FileMode.Open, FileAccess.Read);
    await using var fsout = new FileStream(dest, FileMode.Create, FileAccess.Write);
    await DecompressGZipAsync(fsin, fsout, ct, buffersize).ConfigureAwait(false);
  }

  private async static Task DecompressGZipAsync(
  Stream input, Stream output, CancellationToken ct, int buffersize = 81920)
  {
    int readbytes;
    var buffer = new byte[buffersize];

    await using var gzip = new GZipStream(input, CompressionMode.Decompress);
    while ((readbytes = await gzip.ReadAsync(
      buffer.AsMemory(0, buffer.Length), ct).ConfigureAwait(false)) > 0)
      await output.WriteAsync(buffer.AsMemory(0, readbytes), ct).ConfigureAwait(false);
  }

  #endregion Async GZip File Compress

  #region GZip FileCompressPackage Async 


  public async static Task<long> CompressGZipAsyncSpec(
      Stream input, Stream output, int buffersize = 81920,
      CompressionLevel compresslevel = CompressionLevel.Optimal)
  {
    var buffer = new byte[buffersize];
    await using var counting = new CountingStream(output);
    await using var GZip = new GZipStream(counting, compresslevel, leaveOpen: true);

    int readbytes;
    while ((readbytes = await input.ReadAsync(buffer.AsMemory(0, buffer.Length))) > 0)
      await GZip.WriteAsync(buffer.AsMemory(0, readbytes));

    await GZip.FlushAsync();
    return counting.BytesWritten; // exact compress length 
  }

  public static async Task<long> DecompressGZipAsyncSpec(
   Stream input, Stream output, long start, long length, int buffersize = 81920)
  {
    if (input.CanSeek)
      input.Seek(start, SeekOrigin.Begin);

    var buffer = new byte[buffersize];
    await using var counting = new CountingStream(output);
    await using var limited = new SubStream(input, length, leave_open: true);
    await using var GZip = new GZipStream(limited, CompressionMode.Decompress, true);

    int readbytes;
    while ((readbytes = await GZip.ReadAsync(buffer.AsMemory(0, buffer.Length)).ConfigureAwait(false)) > 0)
      await counting.WriteAsync(buffer.AsMemory(0, readbytes)).ConfigureAwait(false);

    return counting.BytesWritten; // exakte dekomprimierte Länge
  }

  #endregion GZip FileCompressPackage Async

  #region Assert Compress

  private static void AssertCompress(ReadOnlySpan<byte> bytes)
  {
    if (bytes.IsEmpty || bytes.Length == 0)
      throw new ArgumentNullException(nameof(bytes),
        $"{nameof(bytes)} has failed! NULL or Zero-Length");

    if (bytes.Length > MAX_MESSAGE_SIZE)
      throw new ArgumentOutOfRangeException(nameof(bytes),
        $"{nameof(bytes)}.Length has failed! " +
        $"Length exceeds maximum allowed size (8 MB).");
  }

  #endregion Assert Compress
}
