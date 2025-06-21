

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
    TestBcPqcService();

    TestBcCrypto(); 
  }

  private static void TestBcPqcService()
  {
    TestBcPqcServices.Start();
  }

  private static void TestBcCrypto()
  {
    TestMLKEM.Start();
  }

  private static void TestBcSignature()
  {
    TestMLDSA.Start();
    TestSLHDSA.Start();
    TestXMSS.Start();
    TestLMS.Start();
  }
}