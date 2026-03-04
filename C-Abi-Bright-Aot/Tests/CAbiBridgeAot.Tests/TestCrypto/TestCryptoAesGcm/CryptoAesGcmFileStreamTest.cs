

using System.Diagnostics;

namespace michele.natale.Tests;

using michele.natale.CAbiBridge;
using Pointers;
using System.Text;
using static CryptoTestUtils;

partial class CryptoAesGcmTest
{
  #region Native
  public static void TestAesGcmFile(int rounds)
  {
    Console.Write($"{nameof(TestAesGcmFile)}: ");

    ReadOnlySpan<byte> src = "data"u8, dest = "cipher"u8, srcr = "datar"u8;

    var sw = Stopwatch.StartNew();
    for (var i = 0; i < rounds; i++)
    {
      var max = (1 << 21) + 1024;
      var flength = Random.Shared.Next(max);
      SetRngFileData(Encoding.UTF8.GetString(src), flength);

      var associated = RngBytes(Random.Shared.Next(1, 64));
      using var key = new UsIPtr<byte>(RngBytes(NetServices.AES_GCM_MAX_KEY_SIZE));

      var err = Native.AesGcmEncryptFileAot(
       src, src.Length, dest, dest.Length,
       key.Ptr, key.Length, associated, associated.Length);
      AssertError(err);

      err = Native.AesGcmDecryptFileAot(
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

  public static void TestAesGcmFileManaged(int rounds)
  {
    Console.Write($"{nameof(TestAesGcmFileManaged)}: ");

    ReadOnlySpan<byte> src = "data"u8, dest = "cipher"u8, srcr = "datar"u8;

    var sw = Stopwatch.StartNew();
    for (var i = 0; i < rounds; i++)
    {
      var max = (1 << 21) + 1024;
      var flength = Random.Shared.Next(max);
      SetRngFileData(Encoding.UTF8.GetString(src), flength);

      var associated = RngBytes(Random.Shared.Next(1, 64));
      using var key = new UsIPtr<byte>(RngBytes(NetServices.AES_GCM_MAX_KEY_SIZE));

      var err = CryptoBridge.AesGcmEncryptFileManaged(src, dest, key, associated);
      AssertError(err);

      err = CryptoBridge.AesGcmDecryptFileManaged(dest, srcr, key, associated);
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
