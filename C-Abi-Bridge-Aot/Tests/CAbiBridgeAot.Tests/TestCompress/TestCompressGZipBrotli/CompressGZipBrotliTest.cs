

using System.Diagnostics;
using System.IO.Compression;
using System.Text;

namespace michele.natale.Tests;

using static CryptoTestUtils;

partial class CompressesTest
{
  private static string OriginalString =>
    File.ReadAllText("data.txt"); //5.294 MB

  public static void StartCompress(int rounds)
  {
    TestCompressGzip(rounds);
    TestCompressFileGzip(rounds);
    TestCompressFileBsGzip(rounds);

    TestCompressBrotli(rounds);
    TestCompressFileBrotli(rounds);
    TestCompressFileBsBrotli(rounds);
  }

  private static void TestCompressGzip(int rounds)
  {
    Console.Write($"{nameof(TestCompressGzip)}Aot: ");

    var message = Encoding.UTF8.GetBytes(OriginalString);

    var rand = Random.Shared;
    var sw = Stopwatch.StartNew();
    for (int i = 0; i < rounds; i++)
    {
      var compresslevel = Enum.GetValues<CompressionLevel>();
      var idx = rand.Next(0, compresslevel.Length);

      var err = Native.CompressMessageGZipAot(
        message, message.Length, (byte)compresslevel[idx],
        out var out_ptr, out var out_length);
      AssertError(err);

      var compress = ToBytes(out_ptr, out_length);
      Native.FreeBuffer(out_ptr);

      err = Native.DecompressMessageGZipAot(
        compress, compress.Length,
        out out_ptr, out out_length);
      AssertError(err);

      var decompress = ToBytes(out_ptr, out_length);
      Native.FreeBuffer(out_ptr);

      if (!message.SequenceEqual(decompress))
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();

    var t = (double)sw.ElapsedMilliseconds;
    Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms");
  }

  private static void TestCompressFileGzip(int rounds)
  {
    Console.Write($"{nameof(TestCompressFileGzip)}Aot: ");


    string src = "data.txt", dest = "datacompress", destr = "datar.txt";
    File.Delete(dest); File.Delete(destr);

    var rand = Random.Shared;
    var sw = Stopwatch.StartNew();
    for (int i = 0; i < rounds; i++)
    {
      var compresslevel = Enum.GetValues<CompressionLevel>();
      var idx = rand.Next(0, compresslevel.Length);

      var src_utf8 = Encoding.UTF8.GetBytes(src);
      var dest_utf8 = Encoding.UTF8.GetBytes(dest);

      var err = Native.CompressFileGZipAot(
        src_utf8, src_utf8.Length,
        dest_utf8, dest_utf8.Length,
        (byte)compresslevel[idx]);
      AssertError(err);

      var destr_utf8 = Encoding.UTF8.GetBytes(destr);

      err = Native.DecompressFileGZipAot(
        dest_utf8, dest_utf8.Length,
        destr_utf8, destr_utf8.Length);
      AssertError(err);

      var istrue = Native.EqualFilesAot(
        src_utf8, src_utf8.Length,
        destr_utf8, destr_utf8.Length,
        out err);
      AssertError(err);

      if (!istrue)
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();

    var t = (double)sw.ElapsedMilliseconds;
    Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms");
  }

  private static void TestCompressFileBsGzip(int rounds)
  {
    Console.Write($"{nameof(TestCompressFileBsGzip)}Aot: ");


    string src = "data.txt", dest = "datacompress", destr = "datar.txt";
    File.Delete(dest); File.Delete(destr);

    var rand = Random.Shared;
    var sw = Stopwatch.StartNew();
    for (int i = 0; i < rounds; i++)
    {
      var compresslevel = Enum.GetValues<CompressionLevel>();
      var idx = rand.Next(0, compresslevel.Length);

      var src_utf8 = Encoding.UTF8.GetBytes(src);
      var dest_utf8 = Encoding.UTF8.GetBytes(dest);

      var err = Native.CompressFileBufferSizeGZipAot(
        src_utf8, src_utf8.Length,
        dest_utf8, dest_utf8.Length,
        BUFFER_SIZE_DEFAULT, (byte)compresslevel[idx]);
      AssertError(err);

      var destr_utf8 = Encoding.UTF8.GetBytes(destr);

      err = Native.DecompressFileBufferSizeGZipAot(
        dest_utf8, dest_utf8.Length,
        destr_utf8, destr_utf8.Length,
        BUFFER_SIZE_DEFAULT);
      AssertError(err);

      var istrue = Native.EqualFilesAot(
        src_utf8, src_utf8.Length,
        src_utf8, src_utf8.Length,
        out err);
      AssertError(err);

      if (!istrue)
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();

    var t = (double)sw.ElapsedMilliseconds;
    Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms");
  }



  private static void TestCompressBrotli(int rounds)
  {
    Console.Write($"{nameof(TestCompressBrotli)}Aot: ");

    var message = Encoding.UTF8.GetBytes(OriginalString);

    var rand = Random.Shared;
    var sw = Stopwatch.StartNew();
    for (int i = 0; i < rounds; i++)
    {
      var compresslevel = Enum.GetValues<CompressionLevel>();
      var idx = rand.Next(0, compresslevel.Length);

      var err = Native.CompressMessageBrotliAot(
        message, message.Length, (byte)compresslevel[idx],
        out var out_ptr, out var out_length);
      AssertError(err);

      var compress = ToBytes(out_ptr, out_length);
      Native.FreeBuffer(out_ptr);

      err = Native.DecompressMessageBrotliAot(
        compress, compress.Length,
        out out_ptr, out out_length);
      AssertError(err);

      var decompress = ToBytes(out_ptr, out_length);
      Native.FreeBuffer(out_ptr);

      if (!message.SequenceEqual(decompress))
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();

    var t = (double)sw.ElapsedMilliseconds;
    Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms");
  }

  private static void TestCompressFileBrotli(int rounds)
  {
    Console.Write($"{nameof(TestCompressFileBrotli)}Aot: ");


    string src = "data.txt", dest = "datacompress", destr = "datar.txt";
    File.Delete(dest); File.Delete(destr);

    var rand = Random.Shared;
    var sw = Stopwatch.StartNew();
    for (int i = 0; i < rounds; i++)
    {
      var compresslevel = Enum.GetValues<CompressionLevel>();
      var idx = rand.Next(0, compresslevel.Length);

      var src_utf8 = Encoding.UTF8.GetBytes(src);
      var dest_utf8 = Encoding.UTF8.GetBytes(dest);

      var err = Native.CompressFileBufferSizeBrotliAot(
        src_utf8, src_utf8.Length,
        dest_utf8, dest_utf8.Length,
        BUFFER_SIZE_DEFAULT, (byte)compresslevel[idx]);
      AssertError(err);

      var destr_utf8 = Encoding.UTF8.GetBytes(destr);

      err = Native.DecompressFileBufferSizeBrotliAot(
        dest_utf8, dest_utf8.Length,
        destr_utf8, destr_utf8.Length,
        BUFFER_SIZE_DEFAULT);
      AssertError(err);

      var istrue = Native.EqualFilesAot(
        src_utf8, src_utf8.Length,
        src_utf8, src_utf8.Length,
        out err);
      AssertError(err);

      if (!istrue)
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();

    var t = (double)sw.ElapsedMilliseconds;
    Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms");
  }

  private static void TestCompressFileBsBrotli(int rounds)
  {
    Console.Write($"{nameof(TestCompressFileBsBrotli)}Aot: ");


    string src = "data.txt", dest = "datacompress", destr = "datar.txt";
    File.Delete(dest); File.Delete(destr);

    var rand = Random.Shared;
    var sw = Stopwatch.StartNew();
    for (int i = 0; i < rounds; i++)
    {
      var compresslevel = Enum.GetValues<CompressionLevel>();
      var idx = rand.Next(0, compresslevel.Length);

      var src_utf8 = Encoding.UTF8.GetBytes(src);
      var dest_utf8 = Encoding.UTF8.GetBytes(dest);

      var err = Native.CompressFileBrotliAot(
        src_utf8, src_utf8.Length,
        dest_utf8, dest_utf8.Length,
        (byte)compresslevel[idx]);
      AssertError(err);

      var destr_utf8 = Encoding.UTF8.GetBytes(destr);

      err = Native.DecompressFileBrotliAot(
        dest_utf8, dest_utf8.Length,
        destr_utf8, destr_utf8.Length);
      AssertError(err);

      var istrue = Native.EqualFilesAot(
        src_utf8, src_utf8.Length,
        src_utf8, src_utf8.Length,
        out err);
      AssertError(err);

      if (!istrue)
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();

    var t = (double)sw.ElapsedMilliseconds;
    Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms\n");
  }

}
