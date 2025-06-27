//ML-DSA
//Module-Lattice-Based
//FIPS PUB 204 
//https://nvlpubs.nist.gov/nistpubs/fips/nist.fips.204.pdf


using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;


namespace michele.natale.BcPqcs;


using Pointers;


/// <summary>
/// Provides signature methods around the ML-DSA algorithm.
/// </summary>
public sealed class MLDSA : IMLDSA
{

  #region ML-DSA Data Sign / Verify

  #region ML-DSA Data Sign 

  public static byte[] Sign(MlDsaKeyPairInfo info,
    ReadOnlySpan<byte> message)
  {
    var msg = message.ToArray();

    //ML-DSA Parameter 
    var parameter = info.ToParameter();

    //Sign Message-Data 
    var signer = new MLDsaSigner(parameter, true);
    var privkey = MLDsaPrivateKeyParameters
      .FromEncoding(parameter, info.ToPrivKey().ToBytes());
    signer.Init(true, privkey);
    signer.BlockUpdate(msg, 0, msg.Length);
    return signer.GenerateSignature();
  }

  public static byte[] Sign(MLDsaParameters parameter,
      UsIPtr<byte> privkey, ReadOnlySpan<byte> message)
  {
    var msg = message.ToArray();

    //Sign Message-Data 
    var signer = new MLDsaSigner(parameter, true);
    var mldsa_privkey = MLDsaPrivateKeyParameters
      .FromEncoding(parameter, privkey.ToBytes());
    signer.Init(true, mldsa_privkey);
    signer.BlockUpdate(msg, 0, msg.Length);
    return signer.GenerateSignature();
  }

  public static byte[] Sign(
    MLDsaPrivateKeyParameters privkey,
    ReadOnlySpan<byte> message)
  {
    var msg = message.ToArray();

    //Sign Message-Data
    var signer = new MLDsaSigner(privkey.Parameters, true);
    signer.Init(true, privkey);
    signer.BlockUpdate(msg, 0, msg.Length);
    return signer.GenerateSignature();
  }

  #endregion ML-DSA Data Sign

  #region ML-DSA Data Verify 

  public static bool Verify(
    MlDsaKeyPairInfo info,
    ReadOnlySpan<byte> signature,
    ReadOnlySpan<byte> message)
  {
    var msg = message.ToArray();

    //Verify Signature
    var signer = new MLDsaSigner(info.ToParameter(), true);
    var pubkey = MLDsaPublicKeyParameters.FromEncoding(
      info.ToParameter(), info.PubKey);
    signer.Init(false, pubkey);
    signer.BlockUpdate(msg, 0, msg.Length);
    return signer.VerifySignature(signature.ToArray());
  }

  public static bool Verify(
    MLDsaParameters parameter,
    ReadOnlySpan<byte> pubkey,
    ReadOnlySpan<byte> signature,
    ReadOnlySpan<byte> message)
  {
    var msg = message.ToArray();

    //Verify Signature
    var signer = new MLDsaSigner(parameter, true);
    var mldsa_pubkey = MLDsaPublicKeyParameters
      .FromEncoding(parameter, pubkey.ToArray());
    signer.Init(false, mldsa_pubkey);
    signer.BlockUpdate(msg, 0, msg.Length);
    return signer.VerifySignature(signature.ToArray());
  }

  public static bool Verify(
    MLDsaPublicKeyParameters pubkey,
    ReadOnlySpan<byte> signature,
    ReadOnlySpan<byte> message)
  {
    var msg = message.ToArray();

    //Verify Signature
    var signer = new MLDsaSigner(pubkey.Parameters, true);
    signer.Init(false, pubkey);
    signer.BlockUpdate(msg, 0, msg.Length);
    return signer.VerifySignature(signature.ToArray());
  }

  #endregion ML-DSA Data Verify

  #endregion ML-DSA Data Sign / Verify

  #region ML-DSA File Sign / Verify

  #region ML-DSA File Sign

  public static byte[] Sign(
    MlDsaKeyPairInfo info, string datapath)
  {
    if (!File.Exists(datapath))
      throw new FileNotFoundException(datapath);

    //ML-DSA Parameter 
    var parameter = info.ToParameter();

    //Sign Message-Data 
    var signer = new MLDsaSigner(parameter, true);
    var privkey = MLDsaPrivateKeyParameters
      .FromEncoding(parameter, info.ToPrivKey().ToBytes());
    signer.Init(true, privkey);

    int readbytes;
    var buffer = new byte[1 << 14];
    using var fsin = new FileStream(datapath, FileMode.Open, FileAccess.Read);

    while ((readbytes = fsin.Read(buffer)) > 0)
      signer.BlockUpdate(buffer, 0, readbytes);

    return signer.GenerateSignature();
  }

  public static byte[] Sign(MLDsaParameters parameter,
      UsIPtr<byte> privkey, string datapath)
  {
    if (!File.Exists(datapath))
      throw new FileNotFoundException(datapath);

    //Sign Message-Data 
    var signer = new MLDsaSigner(parameter, true);
    var mldsa_privkey = MLDsaPrivateKeyParameters
      .FromEncoding(parameter, privkey.ToBytes());
    signer.Init(true, mldsa_privkey);

    int readbytes;
    var buffer = new byte[1 << 14];
    using var fsin = new FileStream(datapath, FileMode.Open, FileAccess.Read);

    while ((readbytes = fsin.Read(buffer)) > 0)
      signer.BlockUpdate(buffer, 0, readbytes);

    return signer.GenerateSignature();
  }

  public static byte[] Sign(
    MLDsaPrivateKeyParameters privkey, string datapath)
  {
    if (!File.Exists(datapath))
      throw new FileNotFoundException(datapath);

    //Sign Message-Data
    var signer = new MLDsaSigner(privkey.Parameters, true);
    signer.Init(true, privkey);
    int readbytes;
    var buffer = new byte[1 << 14];
    using var fsin = new FileStream(datapath, FileMode.Open, FileAccess.Read);

    while ((readbytes = fsin.Read(buffer)) > 0)
      signer.BlockUpdate(buffer, 0, readbytes);

    return signer.GenerateSignature();
  }


  #endregion ML-DSA File Sign

  #region ML-DSA File Verify

  public static bool Verify(
    MlDsaKeyPairInfo info,
    ReadOnlySpan<byte> signature,
    string datapath)
  {
    if (!File.Exists(datapath))
      throw new FileNotFoundException();

    //Verify Signature
    var signer = new MLDsaSigner(info.ToParameter(), true);
    var pubkey = MLDsaPublicKeyParameters.FromEncoding(
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
    MLDsaParameters parameter,
    ReadOnlySpan<byte> pubkey,
    ReadOnlySpan<byte> signature,
    string datapath)
  {
    if (!File.Exists(datapath))
      throw new FileNotFoundException();

    //Verify Signature
    var signer = new MLDsaSigner(parameter, true);
    var mldsa_pubkey = MLDsaPublicKeyParameters
      .FromEncoding(parameter, pubkey.ToArray());
    signer.Init(false, mldsa_pubkey);

    int readbytes;
    var buffer = new byte[1 << 14];
    using var fs = new FileStream(datapath, FileMode.Open, FileAccess.Read);

    while ((readbytes = fs.Read(buffer, 0, buffer.Length)) > 0)
      signer.BlockUpdate(buffer, 0, readbytes);

    return signer.VerifySignature(signature.ToArray());
  }

  public static bool Verify(
    MLDsaPublicKeyParameters pubkey,
    ReadOnlySpan<byte> signature,
    string datapath)
  {
    if (!File.Exists(datapath))
      throw new FileNotFoundException();

    //Verify Signature
    var signer = new MLDsaSigner(pubkey.Parameters, true);
    signer.Init(false, pubkey);

    int readbytes;
    var buffer = new byte[1 << 14];
    using var fs = new FileStream(datapath, FileMode.Open, FileAccess.Read);

    while ((readbytes = fs.Read(buffer, 0, buffer.Length)) > 0)
      signer.BlockUpdate(buffer, 0, readbytes);

    return signer.VerifySignature(signature.ToArray());
  }

  #endregion ML-DSA File Verify

  #endregion ML-DSA File Sign / Verify


  #region ML-DSA KeyPair Generate 

  public static (byte[] PrivKey, byte[] PubKey) ToKeyPair(
    SecureRandom rand, MLDsaParameters parameter)
  {
    var keypair_generator = new MLDsaKeyPairGenerator();
    var generator = new MLDsaKeyGenerationParameters(rand, parameter);
    keypair_generator.Init(generator);

    var keypair = keypair_generator.GenerateKeyPair();
    var mld_pubkey = (MLDsaPublicKeyParameters)keypair.Public;
    var mld_privkey = (MLDsaPrivateKeyParameters)keypair.Private;

    mld_privkey.GetPublicKey();

    return (mld_privkey.GetEncoded(), mld_pubkey.GetEncoded());
  }

  #endregion ML-DSA KeyPair Generate

  #region Utils

  public static MLDsaParameters ToMLDsaParameter(int idx)
  {
    var a = MLDsaParameters.ml_dsa_44;
    var b = MLDsaParameters.ml_dsa_65;
    var c = MLDsaParameters.ml_dsa_87;
    //var d = MLDsaParameters.ml_dsa_44_with_sha512;
    //var e = MLDsaParameters.ml_dsa_65_with_sha512;
    //var f = MLDsaParameters.ml_dsa_87_with_sha512;
    MLDsaParameters[] result = [a, b, c, /*d, e, f*/];

    return result[idx];
  }

  public static int ToIndex(MLDsaParameters parameter)
  {
    var a = MLDsaParameters.ml_dsa_44;
    var b = MLDsaParameters.ml_dsa_65;
    var c = MLDsaParameters.ml_dsa_87;
    //var d = MLDsaParameters.ml_dsa_44_with_sha512;
    //var e = MLDsaParameters.ml_dsa_65_with_sha512;
    //var f = MLDsaParameters.ml_dsa_87_with_sha512;
    MLDsaParameters[] parameters = [a, b, c, /*d, e, f*/];

    var result = Array.IndexOf(parameters, parameter);
    if (result >= 0) return result;

    throw new InvalidKeyException(nameof(parameter));
  }

  #endregion  Utils

}
