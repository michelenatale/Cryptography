
//SLH-DSA
//Stateless Hash-Based
//FIPS PUB 205 
//https://nvlpubs.nist.gov/nistpubs/fips/nist.fips.205.pdf


using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;


namespace michele.natale.BcPqcs;

using Pointers;
using Services;

public sealed class SLHDSA : ISLHDSA
{

  #region SLH-DSA Data Sign / Verify

  #region SLH-DSA Data Sign

  public static byte[] Sign(SlhDsaKeyPairInfo info,
    ReadOnlySpan<byte> message)
  {
    var msg = message.ToArray();

    //SLH-DSA Parameter 
    var parameter = info.ToParameter();

    //Sign Message-Data 
    var signer = new SlhDsaSigner(parameter, true);
    var privkey = SlhDsaPrivateKeyParameters
      .FromEncoding(parameter, info.ToPrivKey().ToBytes());
    signer.Init(true, privkey);
    signer.BlockUpdate(msg, 0, msg.Length);
    return signer.GenerateSignature();
  }

  public static byte[] Sign(SlhDsaParameters parameter,
      UsIPtr<byte> privkey, ReadOnlySpan<byte> message)
  {
    var msg = message.ToArray();

    //Sign Message-Data 
    var signer = new SlhDsaSigner(parameter, true);
    var SLHDsa_privkey = SlhDsaPrivateKeyParameters
      .FromEncoding(parameter, privkey.ToBytes());
    signer.Init(true, SLHDsa_privkey);
    signer.BlockUpdate(msg, 0, msg.Length);
    return signer.GenerateSignature();
  }

  public static byte[] Sign(
    SlhDsaPrivateKeyParameters privkey,
    ReadOnlySpan<byte> message)
  {
    var msg = message.ToArray();

    //Sign Message-Data
    var signer = new SlhDsaSigner(privkey.Parameters, true);
    signer.Init(true, privkey);
    signer.BlockUpdate(msg, 0, msg.Length);
    return signer.GenerateSignature();
  }

  #endregion SLH-DSA Data Sign

  #region SLH-DSA Data Verify

  public static bool Verify(
   SlhDsaKeyPairInfo info,
   ReadOnlySpan<byte> signature,
   ReadOnlySpan<byte> message)
  {
    var msg = message.ToArray();

    //Verify Signature
    var signer = new SlhDsaSigner(info.ToParameter(), true);
    var pubkey = SlhDsaPublicKeyParameters.FromEncoding(
      info.ToParameter(), info.PubKey);
    signer.Init(false, pubkey);
    signer.BlockUpdate(msg, 0, msg.Length);
    return signer.VerifySignature(signature.ToArray());
  }

  public static bool Verify(
    SlhDsaParameters parameter,
    ReadOnlySpan<byte> pubkey,
    ReadOnlySpan<byte> signature,
    ReadOnlySpan<byte> message)
  {
    var msg = message.ToArray();

    //Verify Signature
    var signer = new SlhDsaSigner(parameter, true);
    var SLHDsa_pubkey = SlhDsaPublicKeyParameters
      .FromEncoding(parameter, pubkey.ToArray());
    signer.Init(false, SLHDsa_pubkey);
    signer.BlockUpdate(msg, 0, msg.Length);
    return signer.VerifySignature(signature.ToArray());
  }

  public static bool Verify(
    SlhDsaPublicKeyParameters pubkey,
    ReadOnlySpan<byte> signature,
    ReadOnlySpan<byte> message)
  {
    var msg = message.ToArray();

    //Verify Signature
    var signer = new SlhDsaSigner(pubkey.Parameters, true);
    signer.Init(false, pubkey);
    signer.BlockUpdate(msg, 0, msg.Length);
    return signer.VerifySignature(signature.ToArray());
  }

  #endregion SLH-DSA Data Verify

  #endregion SLH-DSA Data Sign / Verify

  #region SLH-DSA File Sign / Verify

  #region SLH-DSA File Sign

  public static byte[] Sign(
    SlhDsaKeyPairInfo info, string datapath)
  {
    if (!File.Exists(datapath))
      throw new FileNotFoundException(datapath);

    //SLH-DSA Parameter 
    var parameter = info.ToParameter();

    //Sign Message-Data 
    var signer = new SlhDsaSigner(parameter, true);
    var privkey = SlhDsaPrivateKeyParameters
      .FromEncoding(parameter, info.ToPrivKey().ToBytes());
    signer.Init(true, privkey);

    int readbytes;
    var buffer = new byte[1 << 14];
    using var fsin = new FileStream(datapath, FileMode.Open, FileAccess.Read);

    while ((readbytes = fsin.Read(buffer)) > 0)
      signer.BlockUpdate(buffer, 0, readbytes);

    return signer.GenerateSignature();
  }

  public static byte[] Sign(SlhDsaParameters parameter,
      UsIPtr<byte> privkey, string datapath)
  {
    if (!File.Exists(datapath))
      throw new FileNotFoundException(datapath);

    //Sign Message-Data 
    var signer = new SlhDsaSigner(parameter, true);
    var slhdsa_privkey = SlhDsaPrivateKeyParameters
      .FromEncoding(parameter, privkey.ToBytes());
    signer.Init(true, slhdsa_privkey);

    int readbytes;
    var buffer = new byte[1 << 14];
    using var fsin = new FileStream(datapath, FileMode.Open, FileAccess.Read);

    while ((readbytes = fsin.Read(buffer)) > 0)
      signer.BlockUpdate(buffer, 0, readbytes);

    return signer.GenerateSignature();
  }

  public static byte[] Sign(
    SlhDsaPrivateKeyParameters privkey, string datapath)
  {
    if (!File.Exists(datapath))
      throw new FileNotFoundException(datapath);

    //Sign Message-Data
    var signer = new SlhDsaSigner(privkey.Parameters, true);
    signer.Init(true, privkey);
    int readbytes;
    var buffer = new byte[1 << 14];
    using var fsin = new FileStream(datapath, FileMode.Open, FileAccess.Read);

    while ((readbytes = fsin.Read(buffer)) > 0)
      signer.BlockUpdate(buffer, 0, readbytes);

    return signer.GenerateSignature();
  }

  #endregion SLH-DSA File Sign

  #region SLH-DSA File Verify

  public static bool Verify(
   SlhDsaKeyPairInfo info,
   ReadOnlySpan<byte> signature,
   string datapath)
  {
    if (!File.Exists(datapath))
      throw new FileNotFoundException();

    //Verify Signature
    var signer = new SlhDsaSigner(info.ToParameter(), true);
    var pubkey = SlhDsaPublicKeyParameters.FromEncoding(
      info.ToParameter(), info.PubKey);
    signer.Init(false, pubkey);

    int readbytes;
    var buffer = new byte[1 << 14];
    using var fs = new FileStream(datapath, FileMode.Open, FileAccess.Read);

    while ((readbytes = fs.Read(buffer, 0, buffer.Length)) > 0)
      signer.BlockUpdate(buffer, 0, readbytes);

    return signer.VerifySignature(signature.ToArray());
  }

  public static bool Verify(
    SlhDsaParameters parameter,
    ReadOnlySpan<byte> pubkey,
    ReadOnlySpan<byte> signature,
    string datapath)
  {
    if (!File.Exists(datapath))
      throw new FileNotFoundException();

    //Verify Signature
    var signer = new SlhDsaSigner(parameter, true);
    var slhdsa_pubkey = SlhDsaPublicKeyParameters
      .FromEncoding(parameter, pubkey.ToArray());
    signer.Init(false, slhdsa_pubkey);

    int readbytes;
    var buffer = new byte[1 << 14];
    using var fs = new FileStream(datapath, FileMode.Open, FileAccess.Read);

    while ((readbytes = fs.Read(buffer, 0, buffer.Length)) > 0)
      signer.BlockUpdate(buffer, 0, readbytes);

    return signer.VerifySignature(signature.ToArray());
  }

  public static bool Verify(
    SlhDsaPublicKeyParameters pubkey,
    ReadOnlySpan<byte> signature,
    string datapath)
  {
    if (!File.Exists(datapath))
      throw new FileNotFoundException();

    //Verify Signature
    var signer = new SlhDsaSigner(pubkey.Parameters, true);
    signer.Init(false, pubkey);

    int readbytes;
    var buffer = new byte[1 << 14];
    using var fs = new FileStream(datapath, FileMode.Open, FileAccess.Read);

    while ((readbytes = fs.Read(buffer, 0, buffer.Length)) > 0)
      signer.BlockUpdate(buffer, 0, readbytes);

    return signer.VerifySignature(signature.ToArray());
  }

  #endregion SLH-DSA File Verify

  #endregion SLH-DSA File Sign / Verify


  #region SLH-DSA KeyPair Generate 

  public static (byte[] PrivKey, byte[] PubKey) ToKeyPair(
    SecureRandom rand, SlhDsaParameters parameter)
  {
    var keypair_generator = new SlhDsaKeyPairGenerator();
    var generator = new SlhDsaKeyGenerationParameters(rand, parameter);
    keypair_generator.Init(generator);

    var keypair = keypair_generator.GenerateKeyPair();
    var slhd_pubkey = (SlhDsaPublicKeyParameters)keypair.Public;
    var slhd_privkey = (SlhDsaPrivateKeyParameters)keypair.Private;

    return (slhd_privkey.GetEncoded(), slhd_pubkey.GetEncoded());
  }



  #endregion SLH-DSA KeyPair Generate

  #region Utils

  public static SlhDsaParameters ToSLHDsaParameter(int idx)
  {
    var result = BcPqcServices.ToSLHDsaParameters();
    return result[idx];
  }

  public static int ToIndex(SlhDsaParameters parameter)
  {
    var parameters = BcPqcServices.ToSLHDsaParameters();
    var result = Array.IndexOf(parameters, parameter);
    if (result >= 0) return result;

    throw new InvalidKeyException(nameof(parameter));
  }

  #endregion  Utils
}
