
namespace michele.natale.Tests;

internal sealed partial class CryptoPqcMlKemTest
{
  public static void StartNative(int rounds)
  {
    Start_Native(rounds);

    Console.WriteLine();
  }

  private static void Start_Native(int rounds)
  {
    TestPqcMlKemCreateKeyPairs(rounds * 10);
    TestPqcMlKemCreateKeyPairsParam(rounds * 10);
    TestPqcMlKemSafeLoadKeyPairs(rounds * 10);

    TestPqcMlKemCapsulationSharedKeyWithPublicKey(rounds * 10);
    TestPqcMlKemSharedKeyFromCapsualtionPrivateKey(rounds * 10);

    TestPqcMlKemEnDecryptionBytes(rounds);
    TestPqcMlKemEnDecryptionBytesStress();

    TestPqcMlKemEnDecryptionFile(rounds);
    TestPqcMlKemEnDecryptionKpfFile(rounds);
  }
}
