

namespace michele.natale.Tests;

internal sealed partial class CryptoAesTest
{
  public async static Task StartAsync(int rounds)
  {
    await TestAesFileAsync(rounds);

    await TestAesBytesAsync(rounds);

    await TestAesBytesStressAsync();

    Console.WriteLine();
  }
}
