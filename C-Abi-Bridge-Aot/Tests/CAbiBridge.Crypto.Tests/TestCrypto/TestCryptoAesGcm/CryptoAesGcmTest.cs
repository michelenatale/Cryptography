

namespace michele.natale.Tests;

internal sealed partial class CryptoAesGcmTest
{
  public async static Task StartAsync(int rounds)
  {
    await TestAesGcmFileAsync(rounds);

    await TestAesGcmBytesAsync(rounds);

    await TestAesGcmBytesStressAsync();

    Console.WriteLine();
  }
}
