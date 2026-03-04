


using System.Diagnostics;
using System.Text;

namespace michele.natale.Tests;

using Pointers;
using CAbiBridge;
using static CryptoTestUtils;

partial class CryptoAesTest
{
  #region Native

  public static void TestAesFile(int rounds)
  {
    Console.Write($"{nameof(TestAesFile)}: ");

    ReadOnlySpan<byte> src = "data"u8, dest = "cipher"u8, srcr = "datar"u8;
    var ssrc = Encoding.UTF8.GetString(src);


    var sw = Stopwatch.StartNew();
    for (var i = 0; i < rounds; i++)
    {
      var max = (1 << 21) + 1024;
      var flength = Random.Shared.Next(max);
      SetRngFileData(ssrc, flength);

      var associated = RngBytes(Random.Shared.Next(1, 64));
      using var key = new UsIPtr<byte>(RngBytes(NetServices.AES_KEY_SIZE));

      var err = Native.AesEncryptFileAot(
        src, src.Length, dest, dest.Length,
        key.Ptr, key.Length, associated, associated.Length);
      AssertError(err);

      err = Native.AesDecryptFileAot(
        dest, dest.Length, srcr, srcr.Length,
        key.Ptr, key.Length, associated, associated.Length);
      AssertError(err);

      if (!NetServices.FileEquals(src, srcr))
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();

    var t = sw.ElapsedMilliseconds;
    Console.WriteLine($" rounds = {rounds}, t = {t}ms, td = {t / rounds}ms");
  }

  #endregion Native


  #region Managed
  public static void TestAesFileManaged(int rounds)
  {
    Console.Write($"{nameof(TestAesFileManaged)}: ");

    ReadOnlySpan<byte> src = "data"u8, dest = "cipher"u8, srcr = "datar"u8;
    var ssrc = Encoding.UTF8.GetString(src);


    var sw = Stopwatch.StartNew();
    for (var i = 0; i < rounds; i++)
    {
      var max = (1 << 21) + 1024;
      var flength = Random.Shared.Next(max);
      SetRngFileData(ssrc, flength);

      var associated = RngBytes(Random.Shared.Next(1, 64));
      using var key = new UsIPtr<byte>(RngBytes(NetServices.AES_KEY_SIZE));

      var err = CryptoBridge.AesEncryptFileManaged(src, dest, key, associated);
      AssertError(err);

      err = CryptoBridge.AesDecryptFileManaged(dest, srcr, key, associated);
      AssertError(err);

      if (!NetServices.FileEquals(src, srcr))
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();

    var t = sw.ElapsedMilliseconds;
    Console.WriteLine($" rounds = {rounds}, t = {t}ms, td = {t / rounds}ms");
  }

  #endregion Managed
}
