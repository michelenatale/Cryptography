

namespace MyECSchnorrConceptionTest;

using michele.natale.Schnorrs;
using static michele.natale.Schnorrs.EcServices.EcSchnorrServices;

public class Program
{
  public static void Main()
  {
    TestEcSchnorrCurve();

    TestEcSchnorr();

    TestEcSchnorrMulti();
  }

  private static void TestEcSchnorrCurve()
  {
    //Random Curve
    var curve = RngEcCurve();

    //Generate all EcCurveParameters 
    var param = RngEcCurveParameters();

  }

  public static void TestEcSchnorr()
  {
    //Random Curve
    var curve = RngEcCurve();

    //New Instance of EcSchnorrParameters 
    var ec_schnorr_param = new EcSchnorrParameters(curve);

    //Generate all Parameters into EcSchnorrParameters.
    //All EcCurveParameters is included.
    //without_keypair = false. 
    ec_schnorr_param.GenerateParameters(false);

    //A Message 
    var message = "This is my Message!"u8.ToArray();

    //Generate the signature.
    var signature = Sign(message, ec_schnorr_param);

    //Check Verify.
    var verify = Verify(message, signature, ec_schnorr_param);

    //Output and check from result.
    Console.WriteLine(verify);
    if (!verify) throw new Exception();
  }


  public static void TestEcSchnorrMulti()
  {
    
    //A Message 
    var message = "This is my Message!"u8.ToArray(); 

    //Random Curve
    var curve = RngEcCurve();

    //Number of signatories
    var size = 5;

    //Create new EcSchnorrParameters.
    //All keypairs are random regenerated.
    var ecsparams = EcSchnorrParameters.RngEcSchnorrParameters(curve, size);

    //Create a EcSchnorrInfos for sign and verify.
    var ecinfos = new EcSchnorrInfos(message, ecsparams);

    //Generate the signature.
    var sign = ecinfos.MultiSign();

    //Check is verifiy
    var verify = ecinfos.MultiVerify(sign); 

    //Output and check from result.
    Console.WriteLine(verify);
    if (!verify) throw new Exception();
  }


}
