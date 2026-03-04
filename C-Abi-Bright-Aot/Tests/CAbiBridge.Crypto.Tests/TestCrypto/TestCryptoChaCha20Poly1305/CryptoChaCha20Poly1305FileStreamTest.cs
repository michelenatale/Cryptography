


using System.Diagnostics;

namespace michele.natale.Tests;

using Pointers; 
using static CryptoTestUtils;

partial class CryptoChaCha20Poly1305Test
{

  public async static Task TestChaCha20Poly1305FileAsync(int rounds)
  {
    Console.Write($"{nameof(TestChaCha20Poly1305FileAsync)}: ");

    string src = "data", dest = "cipher", srcr = "datar";

    var sw = Stopwatch.StartNew();
    for (var i = 0; i < rounds; i++)
    {
      var max = (1 << 21) + 1024;
      var flength = Random.Shared.Next(max);
      await SetRngFileDataAsync(src, flength);

      var associated = RngBytes(Random.Shared.Next(1, 64));
      using var key = new UsIPtr<byte>(RngBytes(NetServices.AES_GCM_MAX_KEY_SIZE));

      await NetServices.EncryptionFileChaCha20Poly1305Async(src, dest, key, associated);
      await NetServices.DecryptionFileChaCha20Poly1305Async(dest, srcr, key, associated);

      if (!NetServices.FileEquals(src, srcr))
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();

    var t = sw.ElapsedMilliseconds;
    Console.WriteLine($" rounds = {rounds}, t = {t}ms, td = {t / rounds}ms");
  }
}
