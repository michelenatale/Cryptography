

using michele.natale.MsPqcs;
using michele.natale.TestMsPqcs;

namespace MyMsPQCTest;


public class Program
{
  public async static Task Main()
  {
    await TestBcPqc();
  }

  private async static Task TestBcPqc()
  {
    //Asymmetric Crypto
    TestBcCrypto();

    //Signature
    await TestBcSignatureStateless();
  }

  private static void TestBcCrypto()
  {
    TestMLKEM.Start();
  }

  private async static Task TestBcSignatureStateless()
  {
    //Signature Stateless
    await TestMLDSA.Start();
    //TestSLHDSA.Start();
  }
}