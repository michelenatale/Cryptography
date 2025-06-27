//LMS - Leighton-Micali Hash-Based Signatures
//Hash-Based Signatures
//RFC 8554
//https://www.rfc-editor.org/info/rfc8554


using Org.BouncyCastle.Security;
using System.Diagnostics;
using System.Security.Cryptography;

namespace michele.natale.BcPqcs;

using michele.natale.TestBcPqcs;
using Services;
using System.Security;

public class TestLMS
{
  //https://openquantumsafe.org/liboqs/algorithms/sig_stfl/lms.html
  public static void Start()
  {
    var rounds = 10;
    Test_LMS_Single_Signature(rounds);
    Test_LMS_Single_Signature_File(rounds);

    Test_LMS_Alice_Bob_Signature(rounds);

    Test_LMS_Multi_Signature(rounds);
    Test_LMS_Multi_Signature_File(rounds);
  }
  private static void Test_LMS_Single_Signature(int rounds)
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

    Console.Write($"{nameof(Test_LMS_Single_Signature)}: ");

    if (rounds < 10) rounds = 10;
    var rand = new SecureRandom();
    var parameters = BcPqcServices.ToLmsParam();

    var sw = Stopwatch.StartNew();
    for (var i = 0; i < rounds; i++)
    {

      //Create a random Message
      var size = rand.Next(10, 128);
      var message = BcPqcServices.RngCryptoBytes(size);

      //Lms Parameter select. 
      var idx = rand.Next(parameters.Length);
      var parameter = parameters[idx];

      //For performance reasons, only the
      //parameters 'lms_sha256_h5_x'. 
      parameter = (LmsParam)(((byte)parameter) % 4);

      //Create and Save a legal Lms-KeyPair
      var kpfile = "lms_keypair.key";
      var (privk, pubk) = LMS.ToKeyPair(rand, parameter);

      var lmsinfo = new LmsKeyPairInfo(pubk, privk, parameter);
      lmsinfo.SaveKeyPair(kpfile, true);

      //Load KeyPairs Again
      var info = LmsKeyPairInfo.Load_KeyPair(kpfile);
      if (!lmsinfo.Equals(info))
        throw new Exception();

      var signature = LMS.Sign(info, message);
      var verify = LMS.Verify(info, signature, message);

      if (!verify)
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();

    var t = sw.ElapsedMilliseconds;
    Console.WriteLine($" rounds = {rounds}, t = {t}ms, td = {t / rounds}ms");
  }

  private static void Test_LMS_Single_Signature_File(int rounds)
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

    Console.Write($"{nameof(Test_LMS_Single_Signature_File)}: ");


    var srcfile = "data";
    var rand = new SecureRandom();
    var parameters = BcPqcServices.ToLmsParam();

    var sw = Stopwatch.StartNew();
    for (var i = 0; i < rounds; i++)
    {
      //Create a rng-testfile 
      var max = (1 << 21) + 1024; //ca. 2Mb
      var size = Random.Shared.Next(max);
      SetRngFileData(srcfile, size);

      //LMS Parameter select. Only the first 3 parameters
      var idx = rand.Next(parameters.Length);
      var parameter = parameters[idx];

      //For performance reasons, only the
      //parameters 'lms_sha256_h5_x'. 
      parameter = (LmsParam)(((byte)parameter) % 4);

      //Create and Save a legal LMS-KeyPair
      var kpfile = "lms_keypair.key";
      var (privkey, pubkey) = LMS.ToKeyPair(rand, parameter);
      var lmsinfo = new LmsKeyPairInfo(pubkey, privkey, parameter);
      lmsinfo.SaveKeyPair(kpfile, true);

      //Load KeyPairs again
      var info = LmsKeyPairInfo.Load_KeyPair(kpfile);
      if (!lmsinfo.Equals(info))
        throw new Exception();

      var signature = LMS.Sign(info, srcfile);
      var verify = LMS.Verify(info, signature, srcfile);

      if (!verify)
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }
    sw.Stop();

    var t = sw.ElapsedMilliseconds;
    Console.WriteLine($" rounds = {rounds}, t = {t}ms, td = {t / rounds}ms");
  }

  private static void Test_LMS_Alice_Bob_Signature(int rounds)
  {

    Console.Write($"{nameof(Test_LMS_Alice_Bob_Signature)}: ");


    var rand = new SecureRandom();
    var parameters = BcPqcServices.ToLmsParam();

    var sw = Stopwatch.StartNew();
    for (var i = 0; i < rounds; i++)
    {

      //Create a random Message
      var size = rand.Next(10, 128);
      var message = BcPqcServices.RngCryptoBytes(size);

      //Alice select a LMS Parameter.
      var idx = rand.Next(parameters.Length);
      var parameter = parameters[idx];

      //For performance reasons, only the
      //parameters 'lms_sha256_h5_x'. 
      parameter = (LmsParam)(((byte)parameter) % 4);

      {
        //Alice <<>> Bob
        //**************

        //Alice selects an LMS-Parameter and has
        //all keys generated with one instance. 
        using var alice = new AliceLMS(parameter);

        //Alice signs the message with her PrivateKey.
        var (sign, pubkey) = alice.Sign(message);

        //Bob can now use Alice's PubKey to verify the message.
        var verify = BobLMS.Verify(alice.Info.PubKey, sign, message);

        if (!verify)
          throw new Exception();
      }

      idx = rand.Next(parameters.Length);
      parameter = parameters[idx];

      //For performance reasons, only the
      //parameters 'lms_sha256_h5_x'. 
      parameter = (LmsParam)(((byte)parameter) % 4);

      {
        //Bob <<>> Alice
        //**************

        //Bob selects an LMS-Parameter and has
        //all keys generated with one instance. 
        using var bob = new BobLMS(parameter);

        //Bob signs the message with his PrivateKey.
        var (sign, pubkey) = bob.Sign(message);

        //Alice can now use Bob's PubKey to verify the message.
        var verify = AliceLMS.Verify(bob.Info.PubKey, sign, message);

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

  private static void Test_LMS_Multi_Signature(int rounds)
  {
    Console.Write($"{nameof(Test_LMS_Multi_Signature)}: ");

    var s = 0; //Sum Signatories.
    var rand = new SecureRandom();
    var parameters = BcPqcServices.ToLmsParam();

    var sw = Stopwatch.StartNew();
    for (var i = 0; i < rounds; i++)
    {
      //number of signatories
      var cnt = rand.Next(3, 10); s += cnt;

      //Create random Message
      var size = rand.Next(10, 128);
      var message = BcPqcServices.RngCryptoBytes(size);

      //LMS Parameter select. Only the first 3 parameters
      var idx = rand.Next(parameters.Length);
      var parameter = parameters[idx];

      //For performance reasons, only the
      //parameters 'lms_sha256_h5_x'. 
      parameter = (LmsParam)(((byte)parameter) % 4);

      //The order in 'signinfo' does not matter. 
      //'signinfo' can also be saved and reloaded.
      var signinfo = SignInfoSamples(cnt, message, parameter);
      var multiinfo = LMSMultiSignVerifyInfo.ToMultiInfo(signinfo);

      //Check multiinfo us null
      if (multiinfo is null)
        throw new NullReferenceException($"{nameof(multiinfo)} has failed!");

      //privkey and pubkey are in 'multiinfo'
      var sign = LMSMultiSignVerifyInfo.MultiSign(multiinfo, message);
      var verify = LMSMultiSignVerifyInfo.MultiVerify(multiinfo, sign, message);

      if (!verify)
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();

    var t = sw.ElapsedMilliseconds;
    Console.WriteLine($" rounds = {rounds}, signatories = {s / rounds}, t = {t}ms, td = {t / rounds}ms");
  }

  private static void Test_LMS_Multi_Signature_File(int rounds)
  {
    Console.Write($"{nameof(Test_LMS_Multi_Signature_File)}: ");


    var srcfile = "data";
    var s = 0; //Sum Signatories.
    var rand = new SecureRandom();
    var parameters = BcPqcServices.ToLmsParam();

    var sw = Stopwatch.StartNew();
    for (var i = 0; i < rounds; i++)
    {
      //Create a rng-testfile 
      var max = (1 << 21) + 1024; //
      var size = Random.Shared.Next(max);
      SetRngFileData(srcfile, size);

      //number of signatories
      var cnt = rand.Next(3, 10); s += cnt;

      //LMS Parameter select. Only the first 3 parameters
      var idx = rand.Next(parameters.Length);
      var parameter = parameters[idx];

      //For performance reasons, only the
      //parameters 'lms_sha256_h5_x'.
      parameter = (LmsParam)(((byte)parameter) % 4);

      //The order in 'signinfo' does not matter. 
      //'signinfo' can also be saved and reloaded.
      var signinfo = SignInfoSamples(cnt, srcfile, parameter);
      var multiinfo = LMSMultiSignVerifyInfo.ToMultiInfo(signinfo);

      //Check multiinfo us null
      if (multiinfo is null)
        throw new NullReferenceException($"{nameof(multiinfo)} has failed!");

      //privkey and pubkey are in 'multiinfo'
      var sign = LMSMultiSignVerifyInfo.MultiSign(multiinfo, srcfile);
      var verify = LMSMultiSignVerifyInfo.MultiVerify(multiinfo, sign, srcfile);

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

  private static LMSSignInfo[] SignInfoSamples(
    int size, ReadOnlySpan<byte> message, LmsParam parameter)
  {
    if (size < 2 || message.Length < 10)
      throw new ArgumentOutOfRangeException(nameof(size));

    var rand = new SecureRandom();
    var result = new LMSSignInfo[size];
    var msg = Convert.ToHexString(message);
    for (int i = 0; i < size; i++)
    {
      var name = $"Name_{i}";

      //Create a legal LMS-KeyPair 
      var (privk, pubk) = LMS.ToKeyPair(rand, parameter);
      var info = new LmsKeyPairInfo(pubk, privk, parameter);

      //Create signatur and check verification.
      var signature = LMS.Sign(info, message);
      if (LMS.Verify(info, signature, message))
      {
        result[i] = new LMSSignInfo(
          name, parameter, msg, pubk, signature);

        continue;
      }

      throw new VerificationException(
        $"{nameof(SignInfoSamples)} has failed!");
    }

    return result;
  }

  private static LMSSignInfo[] SignInfoSamples(
    int size, string datapath, LmsParam parameter)
  {
    var rand = new SecureRandom();
    var msghash = ToFileHash(datapath);
    var result = new LMSSignInfo[size];

    for (int i = 0; i < size; i++)
    {
      var name = $"Name_{i}";
      var msghex = Convert.ToHexString(msghash);

      //Create a legal LMS-KeyPair 
      var (privk, pubk) = LMS.ToKeyPair(rand, parameter);
      var info = new LmsKeyPairInfo(pubk, privk, parameter);

      //Create signatur and check verification.
      var signature = LMS.Sign(info, msghash);
      if (LMS.Verify(info, signature, msghash))
      {
        result[i] = new LMSSignInfo(
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
