

using michele.natale.BcPqcs;
using michele.natale.Services;
using michele.natale.TestBcPqcs;

namespace MyBcPQCTest;


public class Program
{
  public static void Main()
  {
    TestBcPqc();
  }

  private static void TestBcPqc()
  {
    //ServicesTest
    TestBcPqcService();

    //Asymmetric Crypto
    TestBcCrypto();

    //Signature
    TestBcSignatureStateless();

    //Signature
    TestBcSignatureStateful();
  }

  private static void TestBcPqcService()
  {
    //Asymmetric Crypto
    TestBcPqcServices.Start();
  }

  private static void TestBcCrypto()
  {
    TestMLKEM.Start();
  }

  private static void TestBcSignatureStateless()
  {
    //Signature Stateless
    TestMLDSA.Start();
    TestSLHDSA.Start();
  }

  private static void TestBcSignatureStateful()
  {
    //Signature Stateful
    TestLMS.Start();

    //Not yet available in the C# version.
    //TestXMSS.Start();
  }
}