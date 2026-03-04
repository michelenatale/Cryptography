



namespace michele.natale;


using Tests;


public class Program
{
  public async static Task Main()
  {
    var rounds = 10;
    await TestCryptoAsync(rounds);

    Console.WriteLine();
    Console.WriteLine("Finish");
    Console.ReadLine();
  }

  private async static Task TestCryptoAsync(int rounds)
  { 
    await CryptoAesTest.StartAsync(rounds);
    await CryptoAesGcmTest.StartAsync(rounds);
    await CryptoChaCha20Poly1305Test.StartAsync(rounds);
  }


}