

namespace michele.natale.Tests;


internal sealed partial class CryptoChaCha20Poly1305Test
{
  public static void StartNative(int rounds)
  {
    TestChaCha20Poly1305File(rounds);

    TestChaCha20Poly1305Bytes(rounds);

    TestChaCha20Poly1305BytesStress();

    Console.WriteLine();
  }


  public static void StartManaged(int rounds)
  {

    //Nur für Testzwecken der Managed-Methoden,
    //da mit 'UnmanagedCallersOnly' der Zugriff
    //zu Testszwecken nicht gewährt wird.
    //Bitte für die Nativen Tests dekommentieren!!

    //Only for testing purposes of managed-methods,
    //as access for testing purposes is not granted
    //with ‘UnmanagedCallersOnly’.
    //Please uncomment for native tests!

    TestChaCha20Poly1305FileManaged(rounds);

    TestChaCha20Poly1305BytesManaged(rounds);

    TestChaCha20Poly1305BytesStressManaged();

    Console.WriteLine();
  }
}
