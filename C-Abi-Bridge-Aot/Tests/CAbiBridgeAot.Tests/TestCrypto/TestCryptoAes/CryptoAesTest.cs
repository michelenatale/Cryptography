
 

namespace michele.natale.Tests;

internal sealed partial class CryptoAesTest
{
  public static void StartNative(int rounds)
  {
    Start_Native(rounds);

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

    Start_Managed(rounds);

    Console.WriteLine();

  }

  private static void Start_Native(int rounds)
  {
    TestAesFile(rounds);

    TestAesBytes(rounds);

    TestAesBytesStress();
  }

  private static void Start_Managed(int rounds)
  {
    //Nur für Testzwecken der Managed-Methoden,
    //da mit 'UnmanagedCallersOnly' der Zugriff
    //zu Testszwecken nicht gewährt wird.
    //Für die Nativen Tests dekommentieren!!

    //Only for testing purposes of managed-methods,
    //as access for testing purposes is not granted
    //with ‘UnmanagedCallersOnly’.
    //Uncomment for native tests!!

    TestAesFileManaged(rounds);

    TestAesBytesManaged(rounds);

    TestAesBytesStressManaged();
  }


}
