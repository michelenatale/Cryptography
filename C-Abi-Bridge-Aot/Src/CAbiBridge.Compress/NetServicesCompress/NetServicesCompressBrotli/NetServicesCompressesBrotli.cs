
using michele.natale.Compresses;
using System.IO.Compression;

namespace michele.natale;


partial class NetServicesCompresses
{
  #region Async Brotli Message Compress

  public async static Task<byte[]> CompressBrotliAsync(
    ReadOnlyMemory<byte> bytes, CancellationToken ct,
    CompressionLevel compresslevel = CompressionLevel.Optimal)
  {
    AssertCompress(bytes.Span);

    await using var ms = new MemoryStream();
    await using (var brotli = new BrotliStream(ms, compresslevel))
      await brotli.WriteAsync(bytes, ct).ConfigureAwait(false);

    return ms.ToArray();
  }

  public async static Task<byte[]> DecompressBrotliAsync(
    byte[] bytes, CancellationToken ct)
  {
    await using var msout = new MemoryStream();
    await using var ms = new MemoryStream(bytes);
    await using (var brotli = new BrotliStream(ms, CompressionMode.Decompress))
      await brotli.CopyToAsync(msout, ct).ConfigureAwait(false);

    return msout.ToArray();
  }

  #endregion Async Brotli Message Compress

  #region Async Brotli File Compress

  public async static Task CompressBrotliAsync(
    string src, string dest, CancellationToken ct, int buffersize = 81920,
    CompressionLevel compresslevel = CompressionLevel.Optimal)
  {
    await using var fsin = new FileStream(src, FileMode.Open, FileAccess.Read);
    await using var fsout = new FileStream(dest, FileMode.Create, FileAccess.Write);
    await CompressBrotliAsync(fsin, fsout, ct, buffersize, compresslevel).ConfigureAwait(false);
  }

  private async static Task CompressBrotliAsync(
     Stream input, Stream output, CancellationToken ct, int buffersize = 81920,
     CompressionLevel compresslevel = CompressionLevel.Optimal)
  {
    int readbytes;
    var buffer = new byte[buffersize];

    await using var brotli = new BrotliStream(output, compresslevel);
    while ((readbytes = await input.ReadAsync(
      buffer.AsMemory(0, buffer.Length), ct).ConfigureAwait(false)) > 0)
      await brotli.WriteAsync(buffer.AsMemory(0, readbytes), ct).ConfigureAwait(false);
  }

  public async static Task DecompressBrotliAsync(
    string src, string dest, CancellationToken ct, int buffersize = 81920)
  {
    await using var fsin = new FileStream(src, FileMode.Open, FileAccess.Read);
    await using var fsout = new FileStream(dest, FileMode.Create, FileAccess.Write);
    await DecompressBrotliAsync(fsin, fsout, ct, buffersize).ConfigureAwait(false);
  }

  private async static Task DecompressBrotliAsync(
    Stream input, Stream output, CancellationToken ct, int buffersize = 81920)
  {
    int readbytes;
    var buffer = new byte[buffersize];

    await using var brotli = new BrotliStream(input, CompressionMode.Decompress);
    while ((readbytes = await brotli.ReadAsync(
      buffer.AsMemory(0, buffer.Length), ct).ConfigureAwait(false)) > 0)
      await output.WriteAsync(buffer.AsMemory(0, readbytes), ct).ConfigureAwait(false);
  }

  #endregion Async Brotli File Compress 

  #region Brotli FileCompressPackage Async 

  /// <summary>
  /// Asynchronously compresses data from an input stream into an output stream using Brotli compression.
  /// </summary>
  /// <param name="input">
  /// The source <see cref="Stream"/> containing the uncompressed data. 
  /// The stream must be readable.
  /// </param>
  /// <param name="output">
  /// The destination <see cref="Stream"/> where the compressed data will be written. 
  /// The stream must be writable. The stream remains open after compression.
  /// </param>
  /// <param name="buffersize">
  /// The size of the buffer used for reading and writing. 
  /// Default is 81,920 bytes (80 KB).
  /// </param>
  /// <param name="compresslevel">
  /// The <see cref="CompressionLevel"/> that determines the balance between compression speed and ratio.
  /// Default is <see cref="CompressionLevel.Optimal"/>.
  /// </param>
  /// <returns>
  /// A task that represents the asynchronous compression operation. 
  /// The task result contains the exact number of bytes written to the output stream (compressed length).
  /// </returns>
  /// <remarks>
  /// <para>
  /// This method wraps the output stream in a <c>CountingStream</c> to measure the number of bytes written.
  /// </para>
  /// <para>
  /// The <c>BrotliStream</c> is created with <c>leaveOpen: true</c>, so the output stream remains open after compression.
  /// </para>
  /// <para>
  /// The returned length is the compressed size, not the original size.
  /// </para>
  /// </remarks>
  public async static Task<long> CompressBrotliAsyncSpec(
      Stream input, Stream output, int buffersize = 81920,
      CompressionLevel compresslevel = CompressionLevel.Optimal)
  {
    var buffer = new byte[buffersize];
    await using var counting = new CountingStream(output);
    await using var brotli = new BrotliStream(counting, compresslevel, leaveOpen: true);

    int readbytes;
    while ((readbytes = await input.ReadAsync(buffer.AsMemory(0, buffer.Length))) > 0)
      await brotli.WriteAsync(buffer.AsMemory(0, readbytes));

    await brotli.FlushAsync();
    return counting.BytesWritten; // exact compress length 
  }


  /// <summary>
  /// Asynchronously decompresses a Brotli-compressed segment from an input stream into an output stream.
  /// </summary>
  /// <param name="input">
  /// The source <see cref="Stream"/> containing the compressed data. 
  /// Must be readable. If <see cref="Stream.CanSeek"/> is true, the stream is positioned at <paramref name="start"/>.
  /// </param>
  /// <param name="output">
  /// The destination <see cref="Stream"/> where the decompressed data will be written. 
  /// Must be writable. The stream remains open after decompression.
  /// </param>
  /// <param name="start">
  /// The byte offset in the input stream where the compressed segment begins. 
  /// Used only if the input stream supports seeking.
  /// </param>
  /// <param name="length">
  /// The length, in bytes, of the compressed segment to read from the input stream. 
  /// A <c>SubStream</c> wrapper ensures that only this range is consumed.
  /// </param>
  /// <param name="buffersize">
  /// The size of the buffer used for reading and writing. 
  /// Default is 81,920 bytes (80 KB).
  /// </param>
  /// <returns>
  /// A task that represents the asynchronous decompression operation. 
  /// The task result contains the exact number of bytes written to the output stream (decompressed length).
  /// </returns>
  /// <remarks>
  /// <para>
  /// This method wraps the output stream in a <c>CountingStream</c> to measure the number of decompressed bytes written.
  /// </para>
  /// <para>
  /// The input stream is wrapped in a <c>SubStream</c> to ensure that only the specified compressed segment is read.
  /// </para>
  /// <para>
  /// The <c>BrotliStream</c> is created with <c>leaveOpen: true</c>, so both input and output streams remain open after decompression.
  /// </para>
  /// <para>
  /// The returned length is the decompressed size, not the compressed size.
  /// </para>
  /// </remarks>
  public static async Task<long> DecompressBrotliAsyncSpec(
    Stream input, Stream output, long start, long length, int buffersize = 81920)
  {
    if (input.CanSeek)
      input.Seek(start, SeekOrigin.Begin);

    var buffer = new byte[buffersize];
    await using var counting = new CountingStream(output);
    await using var limited = new SubStream(input, length, leave_open: true);
    await using var brotli = new BrotliStream(limited, CompressionMode.Decompress, true);

    int readbytes;
    while ((readbytes = await brotli.ReadAsync(buffer.AsMemory(0, buffer.Length)).ConfigureAwait(false)) > 0)
      await counting.WriteAsync(buffer.AsMemory(0, readbytes)).ConfigureAwait(false);

    return counting.BytesWritten; // exakte dekomprimierte Länge
  }

  #endregion Brotli FileCompressPackage Async

  #region Assert Compress
  //See GZip Compress
  #endregion Assert Compress
}
