//LMS
//Leighton-Micali Signature hash-based signature
//SP 800-208, RFC 8554, RFC 8708  


//https://www.bouncycastle.org/documentation/specification_interoperability/

using Org.BouncyCastle.Security;
using System.Security.Cryptography;
using Org.BouncyCastle.Pqc.Crypto.Lms;


namespace michele.natale.BcPqcs;


using Pointers;
using Services;

/// <summary>
/// Provides signature methods around the LMS algorithm.
/// </summary>
public sealed class LMS : ILMS
{
  #region LMS Data Sign / Verify

  #region LMS Data Sign

  public static byte[] Sign(LmsKeyPairInfo info,
    ReadOnlySpan<byte> message)
  {
    //Sign Message-Data 
    var signer = new LmsSigner();
    var privkey = LmsPrivateKeyParameters
      .GetInstance(info.ToPrivKey().ToBytes());
    signer.Init(true, privkey);
    return signer.GenerateSignature(message.ToArray());
  }

  public static byte[] Sign(
    UsIPtr<byte> privkey, ReadOnlySpan<byte> message)
  {
    //Sign Message-Data  
    var signer = new LmsSigner();
    var lms_privkey = LmsPrivateKeyParameters
      .GetInstance(privkey.ToBytes());
    signer.Init(true, lms_privkey);
    return signer.GenerateSignature(message.ToArray());
  }

  public static byte[] Sign(
    LmsPrivateKeyParameters privkey,
    ReadOnlySpan<byte> message)
  {
    //Sign Message-Data
    var signer = new LmsSigner();
    signer.Init(true, privkey);
    return signer.GenerateSignature(message.ToArray());
  }

  #endregion LMS Data Sign

  #region LMS Data Verify

  public static bool Verify(
   LmsKeyPairInfo info,
   ReadOnlySpan<byte> signature,
   ReadOnlySpan<byte> message)
  {
    //Verify Signature
    var signer = new LmsSigner();
    var pubkey = LmsPublicKeyParameters
      .GetInstance(info.PubKey);
    signer.Init(false, pubkey);
    return signer.VerifySignature(
      message.ToArray(), signature.ToArray());
  }

  public static bool Verify(
    ReadOnlySpan<byte> pubkey,
    ReadOnlySpan<byte> signature,
    ReadOnlySpan<byte> message)
  {
    //Verify Signature
    var signer = new LmsSigner();
    var lms_pubkey = LmsPublicKeyParameters
      .GetInstance(pubkey.ToArray());
    signer.Init(false, lms_pubkey);
    return signer.VerifySignature(
      message.ToArray(), signature.ToArray());
  }

  public static bool Verify(
    LmsPublicKeyParameters pubkey,
    ReadOnlySpan<byte> signature,
    ReadOnlySpan<byte> message)
  {
    //Verify Signature
    var signer = new LmsSigner();
    signer.Init(false, pubkey);
    return signer.VerifySignature(
      message.ToArray(), signature.ToArray());
  }

  #endregion LMS Data Verify

  #endregion LMS Data Sign / Verify

  #region LMS File Sign / Verify

  #region LMS File Sign

  public static byte[] Sign(
    LmsKeyPairInfo info, string datapath)
  {
    if (!File.Exists(datapath))
      throw new FileNotFoundException(datapath);

    //LMS Parameter 
    var parameter = info.Parameter;

    //Sign Message-Data 
    var signer = new LmsSigner();
    var privkey = LmsPrivateKeyParameters
      .GetInstance(info.ToPrivKey().ToBytes());
    signer.Init(true, privkey);

    using var fsin = new FileStream(datapath, FileMode.Open, FileAccess.Read);
    var hashfile = SHA512.HashData(fsin);

    return signer.GenerateSignature(hashfile);
  }

  public static byte[] Sign(LmsParam parameter,
      UsIPtr<byte> privkey, string datapath)
  {
    if (!File.Exists(datapath))
      throw new FileNotFoundException(datapath);

    //Sign Message-Data 
    var signer = new LmsSigner();
    var lms_privkey = LmsPrivateKeyParameters
      .GetInstance(privkey.ToBytes());
    signer.Init(true, lms_privkey);

    using var fsin = new FileStream(datapath, FileMode.Open, FileAccess.Read);
    var hashfile = SHA512.HashData(fsin);

    return signer.GenerateSignature(hashfile);
  }

  public static byte[] Sign(
    LmsPrivateKeyParameters privkey, string datapath)
  {
    if (!File.Exists(datapath))
      throw new FileNotFoundException(datapath);

    //Sign Message-Data
    var signer = new LmsSigner();
    signer.Init(true, privkey);

    using var fsin = new FileStream(datapath, FileMode.Open, FileAccess.Read);
    var hashfile = SHA512.HashData(fsin);

    return signer.GenerateSignature(hashfile);
  }

  #endregion LMS File Sign

  #region LMS File Verify

  public static bool Verify(
   LmsKeyPairInfo info,
   ReadOnlySpan<byte> signature,
   string datapath)
  {
    if (!File.Exists(datapath))
      throw new FileNotFoundException();

    //Verify Signature
    var signer = new LmsSigner();
    var pubkey = LmsPublicKeyParameters
      .GetInstance(info.PubKey);
    signer.Init(false, pubkey);

    using var fsin = new FileStream(datapath, FileMode.Open, FileAccess.Read);
    var hashfile = SHA512.HashData(fsin);

    return signer.VerifySignature(hashfile, signature.ToArray());
  }

  public static bool Verify(
    LmsParam parameter,
    ReadOnlySpan<byte> pubkey,
    ReadOnlySpan<byte> signature,
    string datapath)
  {
    if (!File.Exists(datapath))
      throw new FileNotFoundException();

    //Verify Signature
    var signer = new LmsSigner();
    var lms_pubkey = LmsPublicKeyParameters
      .GetInstance(pubkey.ToArray());
    signer.Init(false, lms_pubkey);

    using var fsin = new FileStream(datapath, FileMode.Open, FileAccess.Read);
    var hashfile = SHA512.HashData(fsin);

    return signer.VerifySignature(hashfile, signature.ToArray());
  }

  public static bool Verify(
    LmsPublicKeyParameters pubkey,
    ReadOnlySpan<byte> signature,
    string datapath)
  {
    if (!File.Exists(datapath))
      throw new FileNotFoundException();

    //Verify Signature
    var signer = new LmsSigner();
    signer.Init(false, pubkey);

    using var fsin = new FileStream(datapath, FileMode.Open, FileAccess.Read);
    var hashfile = SHA512.HashData(fsin);

    return signer.VerifySignature(hashfile, signature.ToArray());
  }

  #endregion LMS File Verify

  #endregion LMS File Sign / Verify


  #region LMS KeyPair Generate 

  public static (byte[] PrivKey, byte[] PubKey) ToKeyPair(
    SecureRandom rand, LmsParam parameter)
  {
    var keypair_generator = new LmsKeyPairGenerator();
    var generator = BcPqcServices.ToLmsKeyGenerationParameter(parameter, rand);
    keypair_generator.Init(generator);

    var keypair = keypair_generator.GenerateKeyPair();
    var slhd_pubkey = (LmsPublicKeyParameters)keypair.Public;
    var slhd_privkey = (LmsPrivateKeyParameters)keypair.Private;

    return (slhd_privkey.GetEncoded(), slhd_pubkey.GetEncoded());
  }



  #endregion LMS KeyPair Generate
}
