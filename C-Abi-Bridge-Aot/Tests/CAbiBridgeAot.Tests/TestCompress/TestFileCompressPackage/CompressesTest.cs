
using System.Diagnostics;
using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace michele.natale.Tests;

using Compresses; 
using static CryptoTestUtils;

partial class CompressesTest
{
  public static void StartFileCompressPackage(int rounds)
  {
    TestPackNoneFile();
    TestPackNoneBsFile();
    TestPackGZipFile();
    TestPackBrotliFile();

    string srcfolder = "sourcefolder";
    PreparationAsync(srcfolder)
      .GetAwaiter().GetResult();
    TestPackNoneArchiv(srcfolder);
    TestPackGZipArchiv(srcfolder);
    TestPackBrotliArchiv(srcfolder);
    Finish(srcfolder);

    Console.WriteLine();

  }

  private static void TestPackNoneFile()
  {

    var rand = Random.Shared;
    var sw = Stopwatch.StartNew();

    string outputfolder = "output", archivepath = "test.fcp";

    // Pack Files
    string[] packlist = ["data2.txt", "data3.txt", "data2.txt", "data3.txt"];

    var compresstype = CompressionType.None;
     
    var archiv_path_utf8 = Encoding.UTF8.GetBytes(archivepath);
    var (fnamesptr, fnameslengthsptr, count) = ToPackListPtr(packlist);

    // 'PackFileBsCLAot' would use the
    // buffersize and compressionlevel
    var err = Native.PackFileAot(
      fnamesptr, fnameslengthsptr, count,
      archiv_path_utf8, archiv_path_utf8.Length,
      (byte)compresstype,
      out var totalfilesize, out var totalcompresssize);
    AssertError(err);

    //With HeaderInformation
    //Console.WriteLine($"Total File Size = {totalfilesize} Bytes, Total Compression Size = {totalcompresssize} Bytes and Total Compression Ratio = {totalcompresssize / (double)totalfilesize}\n");

    var output_folder_utf8 = Encoding.UTF8.GetBytes(outputfolder);

    // Extract files, here without a buffer size
    // 'UnPackFileArchivBsAot' would be set to buffersize
    err = Native.UnPackFileArchivAot(
      archiv_path_utf8, archiv_path_utf8.Length,
      output_folder_utf8, output_folder_utf8.Length);
    AssertError(err);

    if (!FileEqualsSpec(packlist, outputfolder))
      throw new Exception();

    sw.Stop();

    Console.Write($"{nameof(TestPackNoneFile)}Aot: ");

    var t = (double)sw.ElapsedMilliseconds;
    var (sum_size, fcnt) = SumFileSizes(packlist);
    var compress_size = new FileInfo(archivepath).Length;
    Console.WriteLine($" t = {t}ms; file_count = {fcnt}; file_sizes = {sum_size}; compress_size = {compress_size}\n");
  }

  private static void TestPackNoneBsFile()
  {

    var rand = Random.Shared;
    var sw = Stopwatch.StartNew();

    string outputfolder = "output", archivepath = "test.fcp";

    // Pack Files
    string[] packlist = ["data2.txt", "data3.txt", "data2.txt", "data3.txt"];

    var cl = Enum.GetValues<CompressionLevel>();
    var idx = rand.Next(0, cl.Length);
    var compresslevel = cl[idx];

    var compresstype = CompressionType.None;

    var archiv_path_utf8 = Encoding.UTF8.GetBytes(archivepath);
    var (fnamesptr, fnameslengthsptr, count) = ToPackListPtr(packlist);

    var err = Native.PackFileBsCLAot(
      fnamesptr, fnameslengthsptr, count,
      archiv_path_utf8, archiv_path_utf8.Length,
      (byte)compresstype, BUFFER_SIZE_DEFAULT, (byte)compresslevel,
      out var totalfilesize, out var totalcompresssize);
    AssertError(err);

    //With HeaderInformation
    //Console.WriteLine($"Total File Size = {totalfilesize} Bytes, Total Compression Size = {totalcompresssize} Bytes and Total Compression Ratio = {totalcompresssize / (double)totalfilesize}\n");

    var output_folder_utf8 = Encoding.UTF8.GetBytes(outputfolder);

    // UnPack Files
    err = Native.UnPackFileArchivBsAot(
      archiv_path_utf8, archiv_path_utf8.Length,
      output_folder_utf8, output_folder_utf8.Length,
      BUFFER_SIZE_DEFAULT);
    AssertError(err);

    if (!FileEqualsSpec(packlist, outputfolder))
      throw new Exception();

    sw.Stop();

    Console.Write($"{nameof(TestPackNoneBsFile)}Aot: ");

    var t = (double)sw.ElapsedMilliseconds;
    var (sum_size, fcnt) = SumFileSizes(packlist);
    var compress_size = new FileInfo(archivepath).Length;
    Console.WriteLine($" t = {t}ms; file_count = {fcnt}; file_sizes = {sum_size}; compress_size = {compress_size}\n");
  }

  private static void TestPackGZipFile()
  {

    var rand = Random.Shared;
    var sw = Stopwatch.StartNew();

    string outputfolder = "output", archivepath = "test.fcp";

    // Pack Files
    string[] packlist = ["data2.txt", "data3.txt", "data2.txt", "data3.txt"];

    var compresstype = CompressionType.GZip;

    var archiv_path_utf8 = Encoding.UTF8.GetBytes(archivepath);
    var (fnamesptr, fnameslengthsptr, count) = ToPackListPtr(packlist);

    // 'PackFileBsCLAot' would use the buffersize and compressionlevel
    var err = Native.PackFileAot(
      fnamesptr, fnameslengthsptr, count,
      archiv_path_utf8, archiv_path_utf8.Length,
      (byte)compresstype,
      out var totalfilesize, out var totalcompresssize);
    AssertError(err);

    //With HeaderInformation
    //Console.WriteLine($"Total File Size = {totalfilesize} Bytes, Total Compression Size = {totalcompresssize} Bytes and Total Compression Ratio = {totalcompresssize / (double)totalfilesize}\n");

    var output_folder_utf8 = Encoding.UTF8.GetBytes(outputfolder);

    // UnPack Files
    // 'UnPackFileArchivBsAot' would be set to buffersize
    err = Native.UnPackFileArchivAot(
      archiv_path_utf8, archiv_path_utf8.Length,
      output_folder_utf8, output_folder_utf8.Length);
    AssertError(err);

    if (!FileEqualsSpec(packlist, outputfolder))
      throw new Exception();

    sw.Stop();

    Console.Write($"{nameof(TestPackGZipFile)}Aot: ");

    var t = (double)sw.ElapsedMilliseconds;
    var (sum_size, fcnt) = SumFileSizes(packlist);
    var compress_size = new FileInfo(archivepath).Length;
    Console.WriteLine($" t = {t}ms; file_count = {fcnt}; file_sizes = {sum_size}; compress_size = {compress_size}\n");

  }
  private static void TestPackBrotliFile()
  {

    var rand = Random.Shared;
    var sw = Stopwatch.StartNew();

    string outputfolder = "output", archivepath = "test.fcp";

    // Pack Files
    string[] packlist = ["data2.txt", "data3.txt", "data2.txt", "data3.txt"];

    var compresstype = CompressionType.Brotli;

    var archiv_path_utf8 = Encoding.UTF8.GetBytes(archivepath);
    var (fnamesptr, fnameslengthsptr, count) = ToPackListPtr(packlist);

    // 'PackFileBsCLAot' would use the buffersize and compressionlevel
    var err = Native.PackFileAot(
      fnamesptr, fnameslengthsptr, count,
      archiv_path_utf8, archiv_path_utf8.Length,
      (byte)compresstype,
      out var totalfilesize, out var totalcompresssize);
    AssertError(err);

    //With HeaderInformation
    //Console.WriteLine($"Total File Size = {totalfilesize} Bytes, Total Compression Size = {totalcompresssize} Bytes and Total Compression Ratio = {totalcompresssize / (double)totalfilesize}\n");

    var output_folder_utf8 = Encoding.UTF8.GetBytes(outputfolder);

    // UnPack Files
    // 'UnPackFileArchivBsAot' would be set to buffersize
    err = Native.UnPackFileArchivAot(
      archiv_path_utf8, archiv_path_utf8.Length,
      output_folder_utf8, output_folder_utf8.Length);
    AssertError(err);

    if (!FileEqualsSpec(packlist, outputfolder))
      throw new Exception();

    sw.Stop();

    Console.Write($"{nameof(TestPackBrotliFile)}Aot: ");

    var t = (double)sw.ElapsedMilliseconds;
    var (sum_size, fcnt) = SumFileSizes(packlist);
    var compress_size = new FileInfo(archivepath).Length;
    Console.WriteLine($" t = {t}ms; file_count = {fcnt}; file_sizes = {sum_size}; compress_size = {compress_size}\n\n");
  }

  private static void TestPackNoneArchiv(string srcfolder)
  {

    var rand = Random.Shared;
    var sw = Stopwatch.StartNew();
    string outputfolder = "output", archivepath = "test.fcp";

    var compresstype = CompressionType.None;
    var src_folder_utf8 = Encoding.UTF8.GetBytes(srcfolder);
    var archive_path_utf8 = Encoding.UTF8.GetBytes(archivepath);

    var err = Native.PackArchivAot(
      src_folder_utf8, src_folder_utf8.Length,
      archive_path_utf8, archive_path_utf8.Length,
      (byte)compresstype,
      out var totalfilesize, out var totalcompresssize);
    AssertError(err);

    //With HeaderInformation
    //Console.WriteLine($"Total File Size = {totalfilesize} Bytes, Total Compression Size = {totalcompresssize} Bytes and Total Compression Ratio = {totalcompresssize / (double)totalfilesize}\n");

    var output_folder_utf8 = Encoding.UTF8.GetBytes(outputfolder);

    err = Native.UnPackFileArchivAot(
      archive_path_utf8, archive_path_utf8.Length,
      output_folder_utf8, output_folder_utf8.Length);
    AssertError(err);

    if (!FileEqualsSpec(srcfolder, outputfolder))
      throw new Exception();

    sw.Stop();

    Console.Write($"{nameof(TestPackNoneArchiv)}Aot: ");

    var t = (double)sw.ElapsedMilliseconds;
    var (sum_size, fcnt) = SumFileSizesFolder(srcfolder);
    var compress_size = new FileInfo(archivepath).Length;
    Console.WriteLine($" t = {t}ms; file_count = {fcnt}; file_sizes = {sum_size}; compress_size = {compress_size}\n");
  }

  private static void TestPackGZipArchiv(string srcfolder)
  {

    var rand = Random.Shared;
    var sw = Stopwatch.StartNew();
    string outputfolder = "output", archivepath = "test.fcp";

    var compresstype = CompressionType.GZip;
    var src_folder_utf8 = Encoding.UTF8.GetBytes(srcfolder);
    var archive_path_utf8 = Encoding.UTF8.GetBytes(archivepath);

    var err = Native.PackArchivAot(
      src_folder_utf8, src_folder_utf8.Length,
      archive_path_utf8, archive_path_utf8.Length,
      (byte)compresstype,
      out var totalfilesize, out var totalcompresssize);
    AssertError(err);

    //With HeaderInformation
    //Console.WriteLine($"Total File Size = {totalfilesize} Bytes, Total Compression Size = {totalcompresssize} Bytes and Total Compression Ratio = {totalcompresssize / (double)totalfilesize}\n");

    var output_folder_utf8 = Encoding.UTF8.GetBytes(outputfolder);

    err = Native.UnPackFileArchivAot(
      archive_path_utf8, archive_path_utf8.Length,
      output_folder_utf8, output_folder_utf8.Length);
    AssertError(err);

    if (!FileEqualsSpec(srcfolder, outputfolder))
      throw new Exception();

    sw.Stop();

    Console.Write($"{nameof(TestPackGZipArchiv)}Aot: ");

    var t = (double)sw.ElapsedMilliseconds;
    var (sum_size, fcnt) = SumFileSizesFolder(srcfolder);
    var compress_size = new FileInfo(archivepath).Length;
    Console.WriteLine($" t = {t}ms; file_count = {fcnt}; file_sizes = {sum_size}; compress_size = {compress_size}\n");
  }

  private static void TestPackBrotliArchiv(string srcfolder)
  {

    var rand = Random.Shared;
    var sw = Stopwatch.StartNew();
    string outputfolder = "output", archivepath = "test.fcp";

    var compresstype = CompressionType.Brotli;
    var src_folder_utf8 = Encoding.UTF8.GetBytes(srcfolder);
    var archive_path_utf8 = Encoding.UTF8.GetBytes(archivepath);

    var err = Native.PackArchivAot(
      src_folder_utf8, src_folder_utf8.Length,
      archive_path_utf8, archive_path_utf8.Length,
      (byte)compresstype,
      out var totalfilesize, out var totalcompresssize);
    AssertError(err);

    //With HeaderInformation
    //Console.WriteLine($"Total File Size = {totalfilesize} Bytes, Total Compression Size = {totalcompresssize} Bytes and Total Compression Ratio = {totalcompresssize / (double)totalfilesize}\n");

    var output_folder_utf8 = Encoding.UTF8.GetBytes(outputfolder);

    err = Native.UnPackFileArchivAot(
      archive_path_utf8, archive_path_utf8.Length,
      output_folder_utf8, output_folder_utf8.Length);
    AssertError(err);

    if (!FileEqualsSpec(srcfolder, outputfolder))
      throw new Exception();

    sw.Stop();

    Console.Write($"{nameof(TestPackBrotliArchiv)}Aot: ");

    var t = (double)sw.ElapsedMilliseconds;
    var (sum_size, fcnt) = SumFileSizesFolder(srcfolder);
    var compress_size = new FileInfo(archivepath).Length;
    Console.WriteLine($" t = {t}ms; file_count = {fcnt}; file_sizes = {sum_size}; compress_size = {compress_size}\n");
  }


  public static unsafe (IntPtr FNamesPtr, IntPtr FLengthsPtr, int count) ToPackListPtr(string[] packlist)
  {
    if (packlist is null || packlist.Length == 0)
      return (IntPtr.Zero, IntPtr.Zero, 0);

    var count = packlist.Length;

    var names_size = (nuint)(count * sizeof(byte*));
    var names = (byte**)NativeMemory.Alloc(names_size);

    var lengths_size = (nuint)(count * sizeof(int));
    var lengths = (int*)NativeMemory.Alloc(lengths_size);

    for (var i = 0; i < count; i++)
    {
      var utf8 = Encoding.UTF8.GetBytes(packlist[i] ?? string.Empty);
      var str_ptr = (byte*)NativeMemory.Alloc((nuint)(utf8.Length + 1));
      utf8.AsSpan().CopyTo(new Span<byte>(str_ptr, utf8.Length));
      str_ptr[utf8.Length] = 0; //set last zero

      names[i] = str_ptr;
      lengths[i] = utf8.Length;
    }

    return ((nint)names, (nint)lengths, count);
  }



  private static bool FileEqualsSpec(string[] filelist, string outputfolder)
  {
    //Special Tester
    foreach (var file in filelist)
      if (!FileEquals(file, Path.Combine(outputfolder, file)))
        return false;
    return true;
  }

  private static bool FileEqualsSpec(string srcfolder, string destfolder)
  {
    var left = new DirectoryInfo(srcfolder)
      .GetFiles("*.*", SearchOption.AllDirectories).OrderBy(x => x.FullName).ToArray();

    var right = new DirectoryInfo(destfolder)
      .GetFiles("*.*", SearchOption.AllDirectories).OrderBy(x => x.FullName).ToArray();

    if (EqualitySpec(left, right, srcfolder))
    {
      var length = left.Length;
      for (var i = 0; i < length; i++)
        if (!FileEquals(left[i].FullName, right[i].FullName))
          return false;
      return true;
    }

    return false;
  }

  private static bool FileEquals(string left, string right)
  {
    using var fleft = new FileStream(left, FileMode.Open, FileAccess.Read);
    using var fright = new FileStream(right, FileMode.Open, FileAccess.Read);

    using var sha = SHA512.Create();
    return sha.ComputeHash(fleft).SequenceEqual(sha.ComputeHash(fright));
  }

  private static bool EqualitySpec(
    ReadOnlySpan<FileInfo> left, ReadOnlySpan<FileInfo> right, string srcfolder)
  {
    if (left.Length != right.Length)
      return false;

    var length = left.Length;
    for (int i = 0; i < length; i++)
    {
      var idx1 = left[i].FullName.IndexOf(srcfolder, StringComparison.Ordinal);
      var idx2 = right[i].FullName.IndexOf(srcfolder, StringComparison.Ordinal);
      if (!left[i].FullName.Substring(idx1).SequenceEqual(right[i].FullName.Substring(idx2)))
        return false;
    }
    return true;
  }


  private static async Task PreparationAsync(string srcfolder)
  {
    Console.WriteLine($"A SourceFolder with many files and directories is created.\n");
    string[] packlist = { "data.txt", "data2.txt", "data3.txt" };
    await CreateRngFolders(srcfolder, packlist);
  }

  private async static Task CreateRngFolders(
    string basefolder, string[] files)
  {
    var rand = Random.Shared;

    if (Directory.Exists(basefolder))
      Directory.Delete(basefolder, true);

    Directory.CreateDirectory(basefolder);
    var file = files[rand.Next(files.Length)];
    var dest = Path.Combine(basefolder, file);
    await CopyFileAsync(file, dest, overwrite: true);
    for (int i = 0; i < 3; i++)
    {
      var subroot = Path.Combine(basefolder, RngFolderName(8));
      Directory.CreateDirectory(subroot);

      var current = subroot;
      file = files[rand.Next(files.Length)];
      dest = Path.Combine(current, file);
      await CopyFileAsync(file, dest, overwrite: true);
      for (var depth = 0; depth < 3; depth++)
      {
        current = Path.Combine(current, RngFolderName(8));
        Directory.CreateDirectory(current);

        var c = rand.Next(files.Length) + 1;
        for (int j = 0; j < c; j++)
          if (rand.NextDouble() < 0.95) // 95% Chance
          {
            file = files[rand.Next(files.Length)];
            dest = Path.Combine(current, file);
            await CopyFileAsync(file, dest, overwrite: true);
          }
      }
    }
  }

  private static async Task CopyFileAsync(
    string sourcepath, string destinationPath, bool overwrite = false)
  {
    if (!File.Exists(sourcepath))
      throw new FileNotFoundException("Source file not found.", sourcepath);

    if (File.Exists(destinationPath) && overwrite)
      File.Delete(destinationPath);

    await using var fsin = new FileStream(sourcepath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true);
    await using var fsout = new FileStream(destinationPath, FileMode.CreateNew, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true);

    await fsin.CopyToAsync(fsout);
  }

  private static string RngFolderName(int size) =>
    Guid.NewGuid().ToString("N").Substring(0, size);


  private static void Finish(string srcfolder)
  {
    Console.WriteLine($"The source directory is deleted again.\n");
    if (Directory.Exists(srcfolder))
      Directory.Delete(srcfolder, true);
  }

  private static (long Size, int FCnt) SumFileSizesFolder(string foldername)
  {
    var files = Directory.GetFiles(
      foldername,"*.*",SearchOption.AllDirectories);

    return SumFileSizes(files);
  }

  private static (long Size, int FCnt) SumFileSizes(string[] files)
  {
    var result = 0L;
    foreach (var fname in files)
    {
      var fi = new FileInfo(fname);
      if (!fi.Exists)
        throw new FileNotFoundException(nameof(files));
      result += fi.Length;
    }

    return (result, files.Length);
  }
}
