


using System.Diagnostics;

namespace michele.natale.Tests;

using Pointers; 
using static CryptoTestUtils;

partial class CryptoAesTest
{
  public async static Task TestAesFileAsync(int rounds)
  {
    Console.Write($"{nameof(TestAesFileAsync)}: ");

    string src = "data", dest = "cipher", srcr = "datar";

    var sw = Stopwatch.StartNew();
    for (var i = 0; i < rounds; i++)
    {
      var max = (1 << 21) + 1024;
      var flength = Random.Shared.Next(max);
      await SetRngFileDataAsync(src, flength);

      var associated = RngBytes(Random.Shared.Next(1, 64));
      using var key = new UsIPtr<byte>(RngBytes(NetServices.AES_KEY_SIZE));

      await NetServices.EncryptionFileAesAsync(src, dest, key, associated);
      await NetServices.DecryptionFileAesAsync(dest, srcr, key, associated);

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
