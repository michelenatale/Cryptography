using System.Diagnostics;

namespace michele.natale.Services;

using Pointers;
public class TestBcPqcServices
{
  public static void Start()
  {
    var rounds = 10;//10000

    TestAesFile(rounds);
    TestAesGcmFile(rounds);
    TestChaCha20Poly1305File(rounds);

    Console.WriteLine();
  }

  private static void TestAesFile(int rounds)
  {
    Console.Write($"{nameof(TestAesFile)}: ");

    string src = "data", dest = "cipher", srcr = "datar";

    var sw = Stopwatch.StartNew();
    for (var i = 0; i < rounds; i++)
    {
      var max = (1 << 21) + 1024;
      var flength = Random.Shared.Next(max);
      SetRngFileData(src, flength);

      var associated = RngBytes(Random.Shared.Next(1, 64));
      var key = new UsIPtr<byte>(RngBytes(BcPqcServices.AES_KEY_SIZE));

      BcPqcServices.EncryptionFileAes(src, dest, key, associated);
      BcPqcServices.DecryptionFileAes(dest, srcr, key, associated);

      if (!BcPqcServices.FileEquals(src, srcr))
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();

    var t = sw.ElapsedMilliseconds;
    Console.WriteLine($" rounds = {rounds}, t = {t}ms, td = {t / rounds}ms");
  }

  private static void TestAesGcmFile(int rounds)
  {
    Console.Write($"{nameof(TestAesGcmFile)}: ");

    string src = "data", dest = "cipher", srcr = "datar";

    var sw = Stopwatch.StartNew();
    for (var i = 0; i < rounds; i++)
    {
      var max = (1 << 21) + 1024;
      var flength = Random.Shared.Next(max);
      SetRngFileData(src, flength);

      var associated = RngBytes(Random.Shared.Next(1, 64));
      var key = new UsIPtr<byte>(RngBytes(BcPqcServices.AES_GCM_MAX_KEY_SIZE));

      BcPqcServices.EncryptionFileAesGcm(src, dest, key, associated);
      BcPqcServices.DecryptionFileAesGcm(dest, srcr, key, associated);

      if (!BcPqcServices.FileEquals(src, srcr))
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();

    var t = sw.ElapsedMilliseconds;
    Console.WriteLine($" rounds = {rounds}, t = {t}ms, td = {t / rounds}ms");
  }

  private static void TestChaCha20Poly1305File(int rounds)
  {
    Console.Write($"{nameof(TestChaCha20Poly1305File)}: ");

    string src = "data", dest = "cipher", srcr = "datar";

    var sw = Stopwatch.StartNew();
    for (var i = 0; i < rounds; i++)
    {
      var max = (1 << 21) + 1024;
      var flength = Random.Shared.Next(max);
      SetRngFileData(src, flength);

      var associated = RngBytes(Random.Shared.Next(1, 64));
      var key = new UsIPtr<byte>(RngBytes(BcPqcServices.AES_GCM_MAX_KEY_SIZE));

      BcPqcServices.EncryptionFileChaCha20Poly1305(src, dest, key, associated);
      BcPqcServices.DecryptionFileChaCha20Poly1305(dest, srcr, key, associated);

      if (!BcPqcServices.FileEquals(src, srcr))
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();

    var t = sw.ElapsedMilliseconds;
    Console.WriteLine($" rounds = {rounds}, t = {t}ms, td = {t / rounds}ms");
  }



  private static void SetRngFileData(string filename, int size)
  {
    using var fsout = new FileStream(filename, FileMode.Create, FileAccess.Write);

    var length = size < 1024 * 1024 ? size : 1024 * 1024;

    while (length > 0)
    {
      fsout.Write(RngBytes(length));
      size -= length; length = size < 1024 * 1024 ? size : 1024 * 1024;
    }
  }

  private static byte[] RngBytes(int size)
  {
    var rand = Random.Shared;
    var result = new byte[size];
    rand.NextBytes(result);
    if (result[0] == 0) result[0]++;
    return result;
  }



  //private static void SaveBytes(
  //  ReadOnlySpan<byte> bytes, string filenname)
  //{
  //  using var fsout = new FileStream(filenname, FileMode.Create, FileAccess.Write);

  //  int start = 0, readbytes = 0, length = bytes.Length;
  //  var offset = length < 1024 * 1024 ? length : 1024 * 1024;

  //  while ((readbytes = bytes.Slice(start, offset).Length) > 0)
  //  {
  //    fsout.Write(bytes.Slice(start, offset));
  //    start += readbytes; length -= readbytes;
  //    offset = length < 1024 * 1024 ? length : 1024 * 1024;
  //  }
  //}
}
