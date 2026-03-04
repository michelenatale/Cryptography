

using System.Text;
using System.Diagnostics;

namespace michele.natale.Tests;

using Pointers;
using CAbiBridge;
using static CryptoTestUtils;

partial class CryptoChaCha20Poly1305Test
{

  #region Native
  public static void TestChaCha20Poly1305File(int rounds)
  {
    Console.Write($"{nameof(TestChaCha20Poly1305File)}: ");

    ReadOnlySpan<byte> src = "data"u8, dest = "cipher"u8, srcr = "datar"u8;

    var sw = Stopwatch.StartNew();
    for (var i = 0; i < rounds; i++)
    {
      var max = (1 << 21) + 1024;
      var flength = Random.Shared.Next(max);
      SetRngFileData(Encoding.UTF8.GetString(src), flength);

      var associated = RngBytes(Random.Shared.Next(1, 64));
      using var key = new UsIPtr<byte>(RngBytes(NetServices.AES_GCM_MAX_KEY_SIZE));

      var err = Native.ChaCha20Poly1305EncryptFileAot(
      src, src.Length, dest, dest.Length,
      key.Ptr, key.Length, associated, associated.Length);
      AssertError(err);

      err = Native.ChaCha20Poly1305DecryptFileAot(
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
  public static void TestChaCha20Poly1305FileManaged(int rounds)
  {
    Console.Write($"{nameof(TestChaCha20Poly1305FileManaged)}: ");

    ReadOnlySpan<byte> src = "data"u8, dest = "cipher"u8, srcr = "datar"u8;

    var sw = Stopwatch.StartNew();
    for (var i = 0; i < rounds; i++)
    {
      var max = (1 << 21) + 1024;
      var flength = Random.Shared.Next(max);
      SetRngFileData(Encoding.UTF8.GetString(src), flength);

      var associated = RngBytes(Random.Shared.Next(1, 64));
      using var key = new UsIPtr<byte>(RngBytes(NetServices.AES_GCM_MAX_KEY_SIZE));

      var err = CryptoBridge.ChaCha20Poly1305EncryptFileManaged(src, dest, key, associated);
      AssertError(err);

      err = CryptoBridge.ChaCha20Poly1305DecryptFileManaged(dest, srcr, key, associated);
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
