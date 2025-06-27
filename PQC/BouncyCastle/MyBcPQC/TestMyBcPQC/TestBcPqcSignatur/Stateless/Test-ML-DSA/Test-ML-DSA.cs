//ML-DSA (Dilithium)
//Module-Lattice-Based
//FIPS PUB 204
//https://pq-crystals.org/dilithium/index.shtml
//https://nvlpubs.nist.gov/nistpubs/fips/nist.fips.204.pdf


using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System.Diagnostics;
using System.Security;
using System.Security.Cryptography;

namespace michele.natale.BcPqcs;

using Services;
using TestBcPqcs;

public class TestMLDSA
{
  public static void Start()
  {
    var rounds = 10;
    Test_ML_DSA_Single_Signature(rounds);
    Test_ML_DSA_Single_Signature_File(rounds);

    Test_ML_DSA_Alice_Bob_Signature(rounds);

    Test_ML_DSA_Multi_Signature(rounds);
    Test_ML_DSA_Multi_Signature_File(rounds);

    Console.WriteLine();
  }

  private static void Test_ML_DSA_Single_Signature(int rounds)
  {
    //A new key pair is always created and saved. This takes time. 
    //For the temporary exchange, it would not be necessary to save
    //the keys, as all generated keys are only required for the
    //respective session.

    //Es wird immer wieder ein neuer Schlüsselpaar erstellt und
    //abgespeichert. Das braucht seine Zeit. 

    //Für den temporären Austausch, wäre das Abspeichern der
    //Schlüssel nicht notwending, da alle generierten Schlüssel
    //nur für die jeweilige Sitzung benötigt werden.

    Console.Write($"{nameof(Test_ML_DSA_Single_Signature)}: ");

    if (rounds < 10) rounds = 10;
    var rand = new SecureRandom();
    var parameters = BcPqcServices.ToMLDsaParameters();

    var sw = Stopwatch.StartNew();
    for (var i = 0; i < rounds; i++)
    {

      //Create a random Message
      var size = rand.Next(10, 128);
      var message = BcPqcServices.RngCryptoBytes(size);

      //ML-DSA Parameter select. Only the first 3 parameters
      var idx = rand.Next(parameters.Length);
      var parameter = parameters[idx];

      //Create and Save a legal ML-DSA-KeyPair
      var kpfile = "mldsa_keypair.key";
      var (privk, pubk) = MLDSA.ToKeyPair(rand, parameter);

      var mldsainfo = new MlDsaKeyPairInfo(pubk, privk, parameter);
      mldsainfo.SaveKeyPair(kpfile, true);

      //Load KeyPairs Again
      var info = MlDsaKeyPairInfo.Load_KeyPair(kpfile);
      if (!mldsainfo.Equals(info))
        throw new Exception();

      var signature = MLDSA.Sign(info, message);
      var verify = MLDSA.Verify(info, signature, message);

      if (!verify)
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();

    var t = sw.ElapsedMilliseconds;
    Console.WriteLine($" rounds = {rounds}, t = {t}ms, td = {t / rounds}ms");
  }

  private static void Test_ML_DSA_Single_Signature_File(int rounds)
  {

    //A new key pair is always created and saved. This takes time. 
    //For the temporary exchange, it would not be necessary to save
    //the keys, as all generated keys are only required for the
    //respective session.

    //Es wird immer wieder ein neuer Schlüsselpaar erstellt und
    //abgespeichert. Das braucht seine Zeit. 

    //Für den temporären Austausch, wäre das Abspeichern der
    //Schlüssel nicht notwending, da alle generierten Schlüssel
    //nur für die jeweilige Sitzung benötigt werden.

    Console.Write($"{nameof(Test_ML_DSA_Single_Signature_File)}: ");

    if (rounds < 10) rounds = 10;

    var srcfile = "data";
    var rand = new SecureRandom();
    var parameters = BcPqcServices.ToMLDsaParameters();

    var sw = Stopwatch.StartNew();
    for (var i = 0; i < rounds; i++)
    {
      //Create a rng-testfile 
      var max = (1 << 21) + 1024; //ca. 2Mb
      var size = Random.Shared.Next(max);
      SetRngFileData(srcfile, size);

      //ML-DSA Parameter select. Only the first 3 parameters
      var idx = rand.Next(parameters.Length);
      var parameter = parameters[idx];

      //Create and Save a legal ML-DSA-KeyPair
      var kpfile = "mldsa_keypair.key";
      var (privkey, pubkey) = MLDSA.ToKeyPair(rand, parameter);
      var mldsainfo = new MlDsaKeyPairInfo(pubkey, privkey, parameter);
      mldsainfo.SaveKeyPair(kpfile, true);

      //Load KeyPairs again
      var info = MlDsaKeyPairInfo.Load_KeyPair(kpfile);
      if (!mldsainfo.Equals(info))
        throw new Exception();

      var signature = MLDSA.Sign(info, srcfile);
      var verify = MLDSA.Verify(info, signature, srcfile);

      if (!verify)
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }
    sw.Stop();

    var t = sw.ElapsedMilliseconds;
    Console.WriteLine($" rounds = {rounds}, t = {t}ms, td = {t / rounds}ms");
  }

  private static void Test_ML_DSA_Alice_Bob_Signature(int rounds)
  {

    Console.Write($"{nameof(Test_ML_DSA_Alice_Bob_Signature)}: ");

    rounds = rounds < 10 ? 10 : 10 * rounds;

    var rand = new SecureRandom();
    var parameters = BcPqcServices.ToMLDsaParameters();

    var sw = Stopwatch.StartNew();
    for (var i = 0; i < rounds; i++)
    {

      //Create a random Message
      var size = rand.Next(10, 128);
      var message = BcPqcServices.RngCryptoBytes(size);

      //Alice select a ML-DSA Parameter.
      var idx = rand.Next(parameters.Length);
      var parameter = parameters[idx];

      {
        //Alice <<>> Bob
        //**************

        //Alice selects an ML-DSA-Parameter and has
        //all keys generated with one instance. 
        using var alice = new AliceMLDSA(parameter);

        //Alice signs the message with her PrivateKey.
        var (sign, pubkey) = alice.Sign(message);

        //Bob can now use Alice's PubKey to verify the message.
        var verify = BobMLDSA.Verify(alice.Info.PubKey, sign, message, parameter);

        if (!verify)
          throw new Exception();
      }


      idx = rand.Next(parameters.Length);
      parameter = parameters[idx];

      {
        //Bob <<>> Alice
        //**************

        //Bob selects an ML-DSA-Parameter and has
        //all keys generated with one instance. 
        using var bob = new BobMLDSA(parameter);

        //Bob signs the message with his PrivateKey.
        var (sign, pubkey) = bob.Sign(message);

        //Alice can now use Bob's PubKey to verify the message.
        var verify = AliceMLDSA.Verify(bob.Info.PubKey, sign, message, parameter);

        if (!verify)
          throw new Exception();
      }

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();

    var t = sw.ElapsedMilliseconds;
    Console.WriteLine($" rounds = {rounds}, t = {t}ms, td = {t / (2 * rounds)}ms");
  }

  private static void Test_ML_DSA_Multi_Signature(int rounds)
  {
    Console.Write($"{nameof(Test_ML_DSA_Multi_Signature)}: ");

    if (rounds < 10) rounds = 10;

    var s = 0; //Sum Signatories.
    var rand = new SecureRandom();
    var parameters = BcPqcServices.ToMLDsaParameters();

    var sw = Stopwatch.StartNew();
    for (var i = 0; i < rounds; i++)
    {
      //number of signatories
      var cnt = rand.Next(5, 30); s += cnt;

      //Create random Message
      var size = rand.Next(10, 128);
      var message = BcPqcServices.RngCryptoBytes(size);

      //ML-DSA Parameter select. Only the first 3 parameters
      var idx = rand.Next(parameters.Length);
      var parameter = parameters[idx];

      //The order in 'signinfo' does not matter. 
      //'signinfo' can also be saved and reloaded.
      var signinfo = SignInfoSamples(cnt, message, parameter);
      var multiinfo = MLDSAMultiSignVerifyInfo.ToMultiInfo(signinfo);

      //Check multiinfo us null
      if (multiinfo is null)
        throw new NullReferenceException($"{nameof(multiinfo)} has failed!");

      //privkey and pubkey are in 'multiinfo'
      var sign = MLDSAMultiSignVerifyInfo.MultiSign(multiinfo, message);
      var verify = MLDSAMultiSignVerifyInfo.MultiVerify(multiinfo, sign, message);

      if (!verify)
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();

    var t = sw.ElapsedMilliseconds;
    Console.WriteLine($" rounds = {rounds}, signatories = {s / rounds}, t = {t}ms, td = {t / rounds}ms");
  }

  private static void Test_ML_DSA_Multi_Signature_File(int rounds)
  {
    Console.Write($"{nameof(Test_ML_DSA_Multi_Signature_File)}: ");

    if (rounds < 10) rounds = 10;

    var srcfile = "data";
    var s = 0; //Sum Signatories.
    var rand = new SecureRandom();
    var parameters = BcPqcServices.ToMLDsaParameters();

    var sw = Stopwatch.StartNew();
    for (var i = 0; i < rounds; i++)
    {
      //Create a rng-testfile 
      var max = (1 << 21) + 1024; //ca. 2Mb
      var size = Random.Shared.Next(max);
      SetRngFileData(srcfile, size);

      //number of signatories
      var cnt = rand.Next(5, 30); s += cnt;

      //ML-DSA Parameter select. Only the first 3 parameters
      var idx = rand.Next(parameters.Length);
      var parameter = parameters[idx];

      //The order in 'signinfo' does not matter. 
      //'signinfo' can also be saved and reloaded.
      var signinfo = SignInfoSamples(cnt, srcfile, parameter);
      var multiinfo = MLDSAMultiSignVerifyInfo.ToMultiInfo(signinfo);

      //Check multiinfo us null
      if (multiinfo is null)
        throw new NullReferenceException($"{nameof(multiinfo)} has failed!");

      //privkey and pubkey are in 'multiinfo'
      var sign = MLDSAMultiSignVerifyInfo.MultiSign(multiinfo, srcfile);
      var verify = MLDSAMultiSignVerifyInfo.MultiVerify(multiinfo, sign, srcfile);

      if (!verify)
        throw new Exception();


      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();

    var t = sw.ElapsedMilliseconds;
    Console.WriteLine($" rounds = {rounds}, signatories = {s / rounds}, t = {t}ms, td = {t / rounds}ms");
  }
  #region Utils

  private static MLDSASignInfo[] SignInfoSamples(
    int size, ReadOnlySpan<byte> message, MLDsaParameters parameter)
  {
    if (size < 2 || message.Length < 10)
      throw new ArgumentOutOfRangeException(nameof(size));

    var rand = new SecureRandom();
    var result = new MLDSASignInfo[size];
    for (int i = 0; i < size; i++)
    {
      var name = $"Name_{i}";
      var msg = Convert.ToHexString(message);

      //Create a legal ML-DSA-KeyPair 
      var (privk, pubk) = MLDSA.ToKeyPair(rand, parameter);
      var info = new MlDsaKeyPairInfo(pubk, privk, parameter);

      //Create signatur and check verification.
      var signature = MLDSA.Sign(info, message);
      if (MLDSA.Verify(info, signature, message))
      {
        result[i] = new MLDSASignInfo(
          name, parameter, msg, pubk, signature);

        continue;
      }

      throw new VerificationException(
        $"{nameof(SignInfoSamples)} has failed!");
    }

    return result;
  }

  private static MLDSASignInfo[] SignInfoSamples(
    int size, string datapath, MLDsaParameters parameter)
  {
    var rand = new SecureRandom();
    var msghash = ToFileHash(datapath);
    var result = new MLDSASignInfo[size];

    for (int i = 0; i < size; i++)
    {
      var name = $"Name_{i}";
      var msghex = Convert.ToHexString(msghash);

      //Create a legal ML-DSA-KeyPair 
      var (privk, pubk) = MLDSA.ToKeyPair(rand, parameter);
      var info = new MlDsaKeyPairInfo(pubk, privk, parameter);

      //Create signatur and check verification.
      var signature = MLDSA.Sign(info, msghash);
      if (MLDSA.Verify(info, signature, msghash))
      {
        result[i] = new MLDSASignInfo(
          name, parameter, msghex, pubk, signature);

        continue;
      }

      throw new VerificationException(
        $"{nameof(SignInfoSamples)} has failed!");
    }

    return result;
  }

  private static byte[] ToFileHash(string datapath)
  {
    if (!File.Exists(datapath))
      throw new FileNotFoundException(datapath);

    using var fsin = new FileStream(datapath, FileMode.Open, FileAccess.Read);
    return SHA512.HashData(fsin);
  }

  private static void SetRngFileData(string filename, int size)
  {
    using var fsout = new FileStream(filename, FileMode.Create, FileAccess.Write);

    var length = size < 1024 * 1024 ? size : 1024 * 1024;

    while (length > 0)
    {
      fsout.Write(BcPqcServices.RngCryptoBytes(length));
      size -= length; length = size < 1024 * 1024 ? size : 1024 * 1024;
    }
  }

  public static bool FileEquals(string leftfile, string rightfile)
  {
    using var fsleft = new FileStream(leftfile, FileMode.Open, FileAccess.Read);
    using var fsright = new FileStream(rightfile, FileMode.Open, FileAccess.Read);
    return SHA256.HashData(fsleft).SequenceEqual(SHA256.HashData(fsright));
  }

  #endregion  Utils
}
