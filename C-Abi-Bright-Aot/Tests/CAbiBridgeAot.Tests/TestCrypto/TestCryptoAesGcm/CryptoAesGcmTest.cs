

namespace michele.natale.Tests;

internal sealed partial class CryptoAesGcmTest
{
  public static void StartNative(int rounds)
  {
    TestAesGcmFile(rounds);

    TestAesGcmBytes(rounds);

    TestAesGcmBytesStress();

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

    TestAesGcmFileManaged(rounds);

    TestAesGcmBytesManaged(rounds);

    TestAesGcmBytesStressManaged();

    Console.WriteLine();
  }
}
