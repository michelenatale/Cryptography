

using michele.natale.MsPqcs;
using michele.natale.TestMsPqcs;  

namespace MyMsPQCTest;


public class Program
{
  public static async Task Main()
  {

    await TestBcPqc();
  }

  private static async Task TestBcPqc()
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

  private static async Task TestBcSignatureStateless()
  {
    //Signature Stateless
    await TestMLDSA.Start();
    //TestSLHDSA.Start();
  }
}