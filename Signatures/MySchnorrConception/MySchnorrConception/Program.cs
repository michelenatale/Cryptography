

using System.Security.Cryptography;

namespace MySchnorrConception;

using michele.natale.Schnorrs;
using michele.natale.Schnorrs.Services;

public class Program
{
  public static void Main()
  {
    TestSchnorrGroup();
    TestSchnorrParameters();

    TestSchnorrInfo();
    TestSchnorrSignatur();

    TestSchnorrMultiSignatur();

    Console.WriteLine();
    Console.WriteLine("FINISH");
    Console.ReadLine();
  }

  private static void TestSchnorrGroup()
  {
    Console.WriteLine($"Begin {nameof(TestSchnorrGroup)} ...");

    //Empty SchnorrGroup
    var group = new SchnorrGroup();

    //Random Parameter with Sha512
    group.GenerateParameters(new HashAlgorithmName("SHA256"));

    //Empty SchnorrGroup
    group.Reset();

    //Is True
    var empty = group.IsEmpty;

    //Random Parameter with Sha512
    group.GenerateParameters();

    //All Parameters in hex
    var hexs = group.ToHex();

    //All Parameter in bytes
    var bytes = group.ToBytes();

    //All Parameter as BigInteger
    var bis = group.ToBigIntegers();

    //PMEI-Protocol
    var pmei = group.ToPmei();

    //As Parameters
    var (p, q, g, h) = SchnorrGroup.FromHex(hexs);

    //From Parameters a new Instance
    var new_group_I = new SchnorrGroup((p, q, g, h));

    //From PMEI-Protocol a new Instance
    var new_group_II = SchnorrGroup.FromPmei(pmei);

    Console.WriteLine();
  }


  private static void TestSchnorrParameters()
  {
    Console.WriteLine($"Begin {nameof(TestSchnorrParameters)} ...");

    //Empty SchnorrParameters
    var param = new SchnorrParameters();

    //Random SchnorrParameters
    //without_keypair = true
    param.GenerateParameters(true, new HashAlgorithmName("SHA256"));

    //Empty SchnorrParameters
    param.Reset();

    //Is True
    var empty = param.IsEmpty;

    //Random Schnorrparameters
    //without_keypair = false
    param.GenerateParameters(false);

    //Create PMEI-Protocol
    var pmei_I = param.ToPmei();
    var pmei_II = param.ToPmei(false);

    //All Parameters in hex
    var hexs = param.ToKeysHex();

    //All Parameters in Bytes
    var bytes = param.ToKeysBytes();

    //All Parameters as bigIntegers
    var bis = param.ToKeysBigIntegers();

    //Empty SchnorrGroup
    var group = new SchnorrGroup();

    //Random Parameters
    group.GenerateParameters();

    //Extract SchnorrGroup
    group = param.PQG;

    //Instance over group, without KeyPair
    param = new SchnorrParameters(group);

    //Is a copy
    //Instance over a SchnorrParameters
    param = new SchnorrParameters(param);

    //New Instance over PMEI
    var new_param_I = new SchnorrParameters(pmei_I);

    //New Instance over PMEI
    var new_param_II = SchnorrParameters.FromPmei(pmei_II);

    Console.WriteLine();
  }


  private static void TestSchnorrInfo()
  {
    Console.WriteLine($"Begin {nameof(TestSchnorrInfo)} ...");

    //A Message
    var msg = "This is my Message!"u8.ToArray();

    //Empty SchnorrParameters
    var param = new SchnorrParameters();

    //Random Parameters
    //without_keypair is true
    param.GenerateParameters();

    //Is Empty
    param.Reset();

    //Is True
    var empty = param.IsEmpty;

    //Empty SchnorrParameters
    param = new SchnorrParameters();

    //Random Parameters
    //without_keypair is false
    param.GenerateParameters(false);

    //Instance SchnorrInfo for Signing and Verification
    var info = new SchnorrInfo(msg, param);

    //Signing
    var sign = info.Sign();

    //Verification
    var verify = info.Verify(sign);

    //check is verifiy = true
    if (!verify) throw new Exception();

    Console.WriteLine();
  }

  private static void TestSchnorrSignatur()
  {
    Console.WriteLine($"Begin {nameof(TestSchnorrSignatur)} ...");

    //Determining the size parameters (Option)
    var psz = SchnorrServices.P_MIN_SIZE;
    var qsz = SchnorrServices.Q_MIN_SIZE;

    //New Instace from SchnorrGroup - Empty
    var group = new SchnorrGroup();

    //Random Parameters 
    group.GenerateParameters();
    Console.WriteLine($"{nameof(group)} I = true");

    //New Instace from SchnorrGroup - Empty
    group = new SchnorrGroup();

    //Random Parameters with optional size giving
    group.GenerateParameters(psz, qsz);
    Console.WriteLine($"{nameof(group)} II = true");

    //New Instance SchnorrParameters - Empty
    var param = new SchnorrParameters();

    //Random Parameters without_keypair = false
    param.GenerateParameters(false);
    Console.WriteLine($"{nameof(param)} I = true");

    //New Instance SchnorrParameters - Empty
    param = new SchnorrParameters();

    //Random Parameters without_keypair = true
    param.GenerateParameters(true);
    Console.WriteLine($"{nameof(param)} II = true");

    //New Instance SchnorrParameters - Empty
    param = new SchnorrParameters();

    //Random Parameters with optional Size-
    //Parameter and without_keypair = true
    param.GenerateParameters(psz, qsz, true);
    Console.WriteLine($"{nameof(param)} III = true");

    //New Instance SchnorrParameters - Empty
    param = new SchnorrParameters();

    //Random Parameters with optional Size-
    //Parameter and without_keypair = false
    param.GenerateParameters(psz, qsz, false);
    Console.WriteLine($"{nameof(param)} IIII = true");

    //A Message
    var msg = "This is my Message!"u8.ToArray();

    //New Instance SchnorrInfo for Singning and Verification
    var schnorr_info = new SchnorrInfo(msg, param);
    Console.WriteLine($"{nameof(schnorr_info)} = true");

    //Signing
    var sign = schnorr_info.Sign();
    Console.WriteLine($"{nameof(sign)} = true");

    //Verification
    var verify = schnorr_info.Verify(sign);
    Console.WriteLine($"verifiy = {verify}");

    //Check verify =  true
    if (!verify) throw new Exception();

    Console.WriteLine();
  }

  private static void TestSchnorrMultiSignatur()
  {
    Console.WriteLine($"Begin {nameof(TestSchnorrMultiSignatur)} ...");

    //A Message
    var msg = "This is my Message!"u8.ToArray();

    //Number of signatories
    var size = 5;

    //New instance of Schnorrparameters
    var sparams = SchnorrParameters.RngSchnorrParameters(size); 

    //New Instance MultiSchnorrInfo for Singning and Verification
    var schnorr_infos = new SchnorrInfos(msg, sparams);
    Console.WriteLine($"{nameof(schnorr_infos)} = true");

    //Signing
    var sign = schnorr_infos.MultiSign();
    Console.WriteLine($"multi-{nameof(sign)} = true");

    //Verification
    var verify = schnorr_infos.MultiVerify(sign);
    Console.WriteLine($"multi-verifiy = {verify}");

    //Check verify =  true
    if (!verify) throw new Exception();

    Console.WriteLine();
  }

}


