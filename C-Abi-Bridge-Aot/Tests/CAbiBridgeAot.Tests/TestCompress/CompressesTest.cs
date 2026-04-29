 

namespace michele.natale.Tests; 

internal sealed partial class CompressesTest
{
  public static void StartNative(int rounds)
  {
    StartCompress(rounds);
    StartFileCompressPackage(rounds);
  }
}
