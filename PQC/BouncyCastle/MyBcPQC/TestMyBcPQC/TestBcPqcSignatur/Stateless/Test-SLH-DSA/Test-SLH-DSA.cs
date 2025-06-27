
//SLH-DSA (SPHINCS+)
//Stateless Hash-Based
//FIPS PUB 205
//https://sphincs.org/
//https://nvlpubs.nist.gov/nistpubs/fips/nist.fips.205.pdf


using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System.Diagnostics;
using System.Security;
using System.Security.Cryptography;

namespace michele.natale.BcPqcs;

using Services;
using TestBcPqcs;

public class TestSLHDSA
{

  public static void Start()
  {
    var rounds = 1;
    Test_SLH_DSA_Single_Signature(rounds);
    Test_SLH_DSA_Single_Signature_File(rounds);

    Test_SLH_DSA_Alice_Bob_Signature(rounds);

    Test_SLH_DSA_Multi_Signature(rounds);
    Test_SLH_DSA_Multi_Signature_File(rounds);

    Console.WriteLine();
  }

  private static void Test_SLH_DSA_Single_Signature(int rounds)
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

    Console.Write($"{nameof(Test_SLH_DSA_Single_Signature)}: ");

    var rand = new SecureRandom();
    var parameters = BcPqcServices.ToSLHDsaParameters();

    var sw = Stopwatch.StartNew();
    for (var i = 0; i < rounds; i++)
    {
      //Create a random Message
      var size = rand.Next(10, 128);
      var message = BcPqcServices.RngCryptoBytes(size);

      //SLH-DSA Parameter select. Only the first 3 parameters
      var idx = rand.Next(parameters.Length);
      var parameter = parameters[idx];

      //Create and Save a legal SLH-DSA-KeyPair
      var kpfile = "slhdsa_keypair.key";
      var (privk, pubk) = SLHDSA.ToKeyPair(rand, parameter);

      var slhdsainfo = new SlhDsaKeyPairInfo(pubk, privk, parameter);
      slhdsainfo.SaveKeyPair(kpfile, true);

      //Load KeyPairs Again
      var info = SlhDsaKeyPairInfo.Load_KeyPair(kpfile);
      if (!slhdsainfo.Equals(info))
        throw new Exception();

      var signature = SLHDSA.Sign(info, message);
      var verify = SLHDSA.Verify(info, signature, message);

      if (!verify)
        throw new Exception();

      if (rounds > 10)
        if (i % (rounds / 10) == 0)
          Console.Write(".");
    }

    sw.Stop();

    var t = sw.ElapsedMilliseconds;
    Console.WriteLine($" rounds = {rounds}, t = {t}ms, td = {t / rounds}ms");
  }

  private static void Test_SLH_DSA_Single_Signature_File(int rounds)
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

    Console.Write($"{nameof(Test_SLH_DSA_Single_Signature_File)}: ");


    var srcfile = "data";
    var rand = new SecureRandom();
    var parameters = BcPqcServices.ToSLHDsaParameters();

    var sw = Stopwatch.StartNew();
    for (var i = 0; i < rounds; i++)
    {
      //Create a rng-testfile 
      var max = (1 << 21) + 1024; //ca. 2Mb
      var size = Random.Shared.Next(max);
      SetRngFileData(srcfile, size);

      //SLH-DSA Parameter select. Only the first 3 parameters
      var idx = rand.Next(parameters.Length);
      var parameter = parameters[idx];

      //Create and Save a legal SLH-DSA-KeyPair
      var kpfile = "slhdsa_keypair.key";
      var (privkey, pubkey) = SLHDSA.ToKeyPair(rand, parameter);
      var slhdsainfo = new SlhDsaKeyPairInfo(pubkey, privkey, parameter);
      slhdsainfo.SaveKeyPair(kpfile, true);

      //Load KeyPairs again
      var info = SlhDsaKeyPairInfo.Load_KeyPair(kpfile);
      if (!slhdsainfo.Equals(info))
        throw new Exception();

      var signature = SLHDSA.Sign(info, srcfile);
      var verify = SLHDSA.Verify(info, signature, srcfile);

      if (!verify)
        throw new Exception();

      if (rounds > 10)
        if (i % (rounds / 10) == 0)
          Console.Write(".");
    }
    sw.Stop();

    var t = sw.ElapsedMilliseconds;
    Console.WriteLine($" rounds = {rounds}, t = {t}ms, td = {t / rounds}ms");
  }

  private static void Test_SLH_DSA_Alice_Bob_Signature(int rounds)
  {

    Console.Write($"{nameof(Test_SLH_DSA_Alice_Bob_Signature)}: ");


    var rand = new SecureRandom();
    var parameters = BcPqcServices.ToSLHDsaParameters();

    var sw = Stopwatch.StartNew();
    for (var i = 0; i < rounds; i++)
    {

      //Create a random Message
      var size = rand.Next(10, 128);
      var message = BcPqcServices.RngCryptoBytes(size);

      //Alice select a SLH-DSA Parameter.
      var idx = rand.Next(parameters.Length);
      var parameter = parameters[idx];

      {
        //Alice <<>> Bob
        //**************

        //Alice selects an SLH-DSA-Parameter and has
        //all keys generated with one instance. 
        using var alice = new AliceSLHDSA(parameter);

        //Alice signs the message with her PrivateKey.
        var (sign, pubkey) = alice.Sign(message);

        //Bob can now use Alice's PubKey to verify the message.
        var verify = BobSLHDSA.Verify(alice.Info.PubKey, sign, message, parameter);

        if (!verify)
          throw new Exception();
      }

      idx = rand.Next(parameters.Length);
      parameter = parameters[idx];

      {
        //Bob <<>> Alice
        //**************

        //Bob selects an SLH-DSA-Parameter and has
        //all keys generated with one instance. 
        using var bob = new BobSLHDSA(parameter);

        //Bob signs the message with his PrivateKey.
        var (sign, pubkey) = bob.Sign(message);

        //Alice can now use Bob's PubKey to verify the message.
        var verify = AliceSLHDSA.Verify(bob.Info.PubKey, sign, message, parameter);

        if (!verify)
          throw new Exception();
      }

      if (rounds > 10)
        if (i % (rounds / 10) == 0)
          Console.Write(".");
    }

    sw.Stop();

    var t = sw.ElapsedMilliseconds;
    Console.WriteLine($" rounds = {rounds}, t = {t}ms, td = {t / (2 * rounds)}ms");
  }

  private static void Test_SLH_DSA_Multi_Signature(int rounds)
  {
    Console.Write($"{nameof(Test_SLH_DSA_Multi_Signature)}: ");

    var s = 0; //Sum Signatories.
    var rand = new SecureRandom();
    var parameters = BcPqcServices.ToSLHDsaParameters();

    var sw = Stopwatch.StartNew();
    for (var i = 0; i < rounds; i++)
    {
      //number of signatories
      var cnt = rand.Next(3, 10); s += cnt;

      //Create random Message
      var size = rand.Next(10, 128);
      var message = BcPqcServices.RngCryptoBytes(size);

      //SLH-DSA Parameter select. Only the first 3 parameters
      var idx = rand.Next(parameters.Length);
      var parameter = parameters[idx];

      //The order in 'signinfo' does not matter. 
      //'signinfo' can also be saved and reloaded.
      var signinfo = SignInfoSamples(cnt, message, parameter);
      var multiinfo = SLHDSAMultiSignVerifyInfo.ToMultiInfo(signinfo);

      //Check multiinfo us null
      if (multiinfo is null)
        throw new NullReferenceException($"{nameof(multiinfo)} has failed!");

      //privkey and pubkey are in 'multiinfo'
      var sign = SLHDSAMultiSignVerifyInfo.MultiSign(multiinfo, message);
      var verify = SLHDSAMultiSignVerifyInfo.MultiVerify(multiinfo, sign, message);

      if (!verify)
        throw new Exception();

      if (rounds > 10)
        if (i % (rounds / 10) == 0)
          Console.Write(".");
    }

    sw.Stop();

    var t = sw.ElapsedMilliseconds;
    Console.WriteLine($" rounds = {rounds}, signatories = {s / rounds}, t = {t}ms, td = {t / rounds}ms");
  }

  private static void Test_SLH_DSA_Multi_Signature_File(int rounds)
  {
    Console.Write($"{nameof(Test_SLH_DSA_Multi_Signature_File)}: ");


    var srcfile = "data";
    var s = 0; //Sum Signatories.
    var rand = new SecureRandom();
    var parameters = BcPqcServices.ToSLHDsaParameters();

    var sw = Stopwatch.StartNew();
    for (var i = 0; i < rounds; i++)
    {
      //Create a rng-testfile 
      var max = (1 << 21) + 1024; //
      var size = Random.Shared.Next(max);
      SetRngFileData(srcfile, size);

      //number of signatories
      var cnt = rand.Next(3, 10); s += cnt;

      //SLH-DSA Parameter select. Only the first 3 parameters
      var idx = rand.Next(parameters.Length);
      var parameter = parameters[idx];

      //The order in 'signinfo' does not matter. 
      //'signinfo' can also be saved and reloaded.
      var signinfo = SignInfoSamples(cnt, srcfile, parameter);
      var multiinfo = SLHDSAMultiSignVerifyInfo.ToMultiInfo(signinfo);

      //Check multiinfo us null
      if (multiinfo is null)
        throw new NullReferenceException($"{nameof(multiinfo)} has failed!");

      //privkey and pubkey are in 'multiinfo'
      var sign = SLHDSAMultiSignVerifyInfo.MultiSign(multiinfo, srcfile);
      var verify = SLHDSAMultiSignVerifyInfo.MultiVerify(multiinfo, sign, srcfile);

      if (!verify)
        throw new Exception();

      if (rounds > 10)
        if (i % (rounds / 10) == 0)
          Console.Write(".");
    }

    sw.Stop();

    var t = sw.ElapsedMilliseconds;
    Console.WriteLine($" rounds = {rounds}, signatories = {s / rounds}, t = {t}ms, td = {t / rounds}ms");
  }


  #region Utils

  private static SLHDSASignInfo[] SignInfoSamples(
    int size, ReadOnlySpan<byte> message, SlhDsaParameters parameter)
  {
    if (size < 2 || message.Length < 10)
      throw new ArgumentOutOfRangeException(nameof(size));

    var rand = new SecureRandom();
    var result = new SLHDSASignInfo[size];
    for (int i = 0; i < size; i++)
    {
      var name = $"Name_{i}";
      var msg = Convert.ToHexString(message);

      //Create a legal slh-DSA-KeyPair 
      var (privk, pubk) = SLHDSA.ToKeyPair(rand, parameter);
      var info = new SlhDsaKeyPairInfo(pubk, privk, parameter);

      //Create signatur and check verification.
      var signature = SLHDSA.Sign(info, message);
      if (SLHDSA.Verify(info, signature, message))
      {
        result[i] = new SLHDSASignInfo(
          name, parameter, msg, pubk, signature);

        continue;
      }

      throw new VerificationException(
        $"{nameof(SignInfoSamples)} has failed!");
    }

    return result;
  }

  private static SLHDSASignInfo[] SignInfoSamples(
    int size, string datapath, SlhDsaParameters parameter)
  {
    var rand = new SecureRandom();
    var msghash = ToFileHash(datapath);
    var result = new SLHDSASignInfo[size];

    for (int i = 0; i < size; i++)
    {
      var name = $"Name_{i}";
      var msghex = Convert.ToHexString(msghash);

      //Create a legal slh-DSA-KeyPair 
      var (privk, pubk) = SLHDSA.ToKeyPair(rand, parameter);
      var info = new SlhDsaKeyPairInfo(pubk, privk, parameter);

      //Create signatur and check verification.
      var signature = SLHDSA.Sign(info, msghash);
      if (SLHDSA.Verify(info, signature, msghash))
      {
        result[i] = new SLHDSASignInfo(
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
