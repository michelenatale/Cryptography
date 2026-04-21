
namespace michele.natale.Tests;

internal sealed partial class CryptoPqcMlDsaTest
{
  public static void StartNative(int rounds)
  {
    Start_Native(rounds);

    Console.WriteLine();
  }

  private static void Start_Native(int rounds)
  {
    TestPqcMlDsaCreateKeyPairs(rounds * 10);
    TestPqcMlDsaCreateKeyPairsParam(rounds * 10);
    TestPqcMlDsaSaveLoadKeyPairs(rounds * 10);

    TestPqcMlDsaSingleSignature(rounds);
    TestPqcMlDsaSingleSignatureKpiSaveLoad(rounds);

    TestPqcMlDsaSingleSignatureFile(rounds);
    TestPqcMlDsaSingleSignatureKpiSaveLoadFile(rounds);

    TestPqcMlDsaMultiSignKpf(rounds);           //kpi = kpf = key-pair-file
    TestPqcMlDsaMultiSignFileKpfAsync(rounds);  //kpi = kpf = key-pair-file
  }
}
