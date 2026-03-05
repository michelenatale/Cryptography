

namespace michele.natale.Tests;


public class Program
{
  public static void Main()
  {
    var rounds = 10;
    Tests(rounds);

    Console.WriteLine();
    Console.WriteLine("Finish");
    Console.ReadLine();
  }

  private static void Tests(int rounds)
  {
    CryptoRandomTest.Start(rounds * 1000);

    CryptoAesTest.StartNative(rounds);
    CryptoAesGcmTest.StartNative(rounds);
    CryptoChaCha20Poly1305Test.StartNative(rounds);


    ////Kann für die Tests der Nativen
    ////dekommentiert werden.
    //CryptoAesTest.StartManaged(rounds);
    //CryptoAesGcmTest.StartManaged(rounds);
    //CryptoChaCha20Poly1305Test.StartManaged(rounds);
  }
}