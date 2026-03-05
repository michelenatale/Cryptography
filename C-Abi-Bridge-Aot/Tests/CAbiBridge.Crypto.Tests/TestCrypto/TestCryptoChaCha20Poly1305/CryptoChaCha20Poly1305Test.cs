

namespace michele.natale.Tests;


internal sealed partial class CryptoChaCha20Poly1305Test
{
  public async static Task StartAsync(int rounds)
  {
    await TestChaCha20Poly1305FileAsync(rounds);

    await TestChaCha20Poly1305BytesAsync(rounds);

    await TestChaCha20Poly1305BytesStressAsync();

    Console.WriteLine(); 
  }
}
