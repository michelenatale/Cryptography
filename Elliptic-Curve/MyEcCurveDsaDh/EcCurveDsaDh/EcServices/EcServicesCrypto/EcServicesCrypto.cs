

using System.Security.Cryptography;

namespace michele.natale.EcCurveDsaDh;

partial class EcService
{
  public static ECCurve EC_STANDARD
  {
    get;
  } = ECCurve.NamedCurves.nistP256;

  public static EcCryptionAlgorithm ToEcCryptionAlgorithm(string ec_crypt_algo)
  {
    return ec_crypt_algo switch
    {
      string obj when obj == EcCryptionAlgorithm.AES.ToString() => EcCryptionAlgorithm.AES,
      string obj when obj == EcCryptionAlgorithm.AES_GCM.ToString() => EcCryptionAlgorithm.AES_GCM,
      string obj when obj == EcCryptionAlgorithm.CHACHA20_POLY1305.ToString() => EcCryptionAlgorithm.CHACHA20_POLY1305,
      _ => throw new ArgumentException($"{ec_crypt_algo} is failed"),
    };
  }

  public static byte[] DecryptionWithEcCryptionAlgo(
    ReadOnlySpan<byte> bytes, ReadOnlySpan<byte> key,
    ReadOnlySpan<byte> associated, EcCryptionAlgorithm ec_crypt_algo)
  {
    return ec_crypt_algo switch
    {
      var obj when obj == EcCryptionAlgorithm.AES => DecryptionAes(bytes, key, associated),
      var obj when obj == EcCryptionAlgorithm.AES_GCM => DecryptionAesGcm(bytes, key, associated),
      var obj when obj == EcCryptionAlgorithm.CHACHA20_POLY1305 => DecryptionChaCha20Poly1305(bytes, key, associated),
      _ => throw new ArgumentException(),
    };
  }

  public static (string Idx, ECParameters EcParam) GenerateEcKeyPairSavePmei(
    string username, string ext = ".priv", bool encryption = true) =>
      GenerateEcKeyPairSavePmei(RngEcCurve(), username, ext, encryption);

  public static (string Idx, ECParameters EcParam) GenerateEcKeyPairSavePmei(
    ECCurve ecurve, string username,
    string ext = ".priv", bool encryption = true)
  {
    var ecparam = GenerateEcDsaKeyPair(ecurve).PrivateKey;
    var ecbytes = EcParametersInfo.SerializeEcParam(ecparam);
    var rn = NextCryptoInt64().ToString() + ext;

    var (h, f) = PMEI.EcPrivateKeyPmeiHF();
    var fp = EcSettings.ToEcCurrentFolderUser(username);
    var fn = Path.Combine(fp, rn);

    var encbytes = ecbytes;
    if (encryption)
    {
      var mpw = SHA256.HashData(EcSettings.ToMasterKey(username));
      var sd = ToUserSystemData();
      encbytes = EncryptionChaCha20Poly1305(ecbytes, mpw, sd);
    }
    var id = PMEI.SavePmeiToFile(fn, encbytes, h, f);

    return (id, ecparam);
  }


  public static (string Idx, RSAParameters RsaParam) GenerateRsaKeyPairSavePmei(
    string username, string ext = ".priv", bool encryption = true) =>
      GenerateRsaKeyPairSavePmei(username, ext, encryption, 2048);

  public static (string Idx, RSAParameters RsaParam) GenerateRsaKeyPairSavePmei(
    string username, string ext = ".priv", bool encryption = true, int keylength = 2048)
  {
    var rsaparam = GenerateRsaKeyPair(keylength).PrivateKey;
    var rsabytes = RsaParametersInfo.SerializeRsaParam(rsaparam);
    var rn = NextCryptoInt64().ToString() + ext;

    var (h, f) = PMEI.RsaPrivateKeyPmeiHF();
    var fp = EcSettings.ToEcCurrentFolderUser(username);
    var fn = Path.Combine(fp, rn);

    var encbytes = rsabytes;
    if (encryption)
    {
      var mpw = SHA256.HashData(EcSettings.ToMasterKey(username));
      var sd = ToUserSystemData();
      encbytes = EncryptionChaCha20Poly1305(rsabytes, mpw, sd);
    }
    var id = PMEI.SavePmeiToFile(fn, encbytes, h, f);

    return (id, rsaparam);
  }

  public static ECParameters EcKeyPairLoadPmei(
    string username, string index, string ext = ".priv", bool decryption = true)
  {
    var (h, f) = PMEI.EcPrivateKeyPmeiHF();
    var fp = EcSettings.ToEcCurrentFolderUser(username);
    var fn = Path.Combine(fp, index + ext);
    var (id, msg) = PMEI.LoadPmeiFromFile(fn, h, f);

    if (id.Contains(index))
    {
      var ecbytes = msg;
      if (decryption)
      {
        var mpw = SHA256.HashData(EcSettings.ToMasterKey(username));
        var sd = ToUserSystemData();
        ecbytes = DecryptionChaCha20Poly1305(msg, mpw, sd);
      }
      var ecparam = EcParametersInfo.DeserializeEcParam(ecbytes);
      return ecparam;
    }
    throw new ArgumentException($"{nameof(EcKeyPairLoadPmei)} is failed!");
  }

  public static EcMessagePackage ToEcMessagePackage(
    string cipher, string signatur, string pub_key_original,
    EcCryptionAlgorithm ec_crypt_algo)
  {
    return new EcMessagePackage
    {
      Cipher = cipher,
      Signature = signatur,
      SenderPublicKeyPmei = pub_key_original,
      EcCryptionAlgo = ec_crypt_algo.ToString(),
    };
  }

  public static RsaMessagePackage ToRsaMessagePackage(
    string cipher_msg, string cipher_shared_key,
    string signatur, string pub_key_original,
    string rsa_index, EcCryptionAlgorithm ec_crypt_algo)
  {
    return new RsaMessagePackage
    {
      Index = rsa_index,
      Signature = signatur,
      CipherMessage = cipher_msg,
      CipherSharedKey = cipher_shared_key,
      SenderPublicKey = pub_key_original,
      RsaCryptionAlgo = ec_crypt_algo.ToString(),
    };
  }

  public static EcSignedMessage ToEcSignedMessage(
    string sender_public_key_pmei,
    string signatur,
    byte[] message_hash) =>
      ToEcSignedMessage(sender_public_key_pmei, signatur,
        Convert.ToHexString(message_hash));

  public static EcSignedMessage ToEcSignedMessage(
    string sender_public_key_pmei,
    string signatur,
    string message_hash)
  {
    return new EcSignedMessage
    {
      PublicKey = sender_public_key_pmei,
      Signature = signatur,
      MessageHash = message_hash
    };
  }

  public static RsaSignedMessage ToRsaSignedMessage(
    string sender_public_key_pmei,
    string signatur,
    byte[] message_hash) =>
      ToRsaSignedMessage(sender_public_key_pmei, signatur,
        Convert.ToHexString(message_hash));
  public static RsaSignedMessage ToRsaSignedMessage(
    string sender_public_key_pmei,
    string signatur,
    string message_hash)
  {
    return new RsaSignedMessage
    {
      PublicKey = sender_public_key_pmei,
      Signature = signatur,
      MessageHash = message_hash
    };
  }

  private static byte[] ECDH(
    ECParameters privatekey, EcPublicKey publickey)
  {
    var curve = privatekey.Curve;
    using var alice = ECDiffieHellman.Create();

    alice.ImportParameters(new ECParameters
    {
      Curve = curve,
      D = [.. privatekey.D!],
    });

    curve = publickey.PublicKey.Curve;
    using var bob = ECDiffieHellman.Create();
    bob.ImportParameters(new ECParameters
    {
      Curve = curve,
      Q = publickey.ToPublicKeyEcPoint(),
    });

    //return the sharedkey
    return alice.DeriveKeyMaterial(bob.PublicKey);
  }

  public static ECCurve RngEcCurve()
  {
    var ec = ToEcCurveList();
    return ToEcCurve(ec[NextCryptoInt32(ec.Length)]);
  }

  public static ECCurve ToEcCurve(string ecname)
  {
    //var bla = ECCurve.NamedCurves.brainpoolP160r1;

    return ecname.ToLower() switch
    {
      string e when e.SequenceEqual("nistP256".ToLower()) => ECCurve.NamedCurves.nistP256,
      string e when e.SequenceEqual("nistP384".ToLower()) => ECCurve.NamedCurves.nistP384,
      string e when e.SequenceEqual("nistP521".ToLower()) => ECCurve.NamedCurves.nistP521,
      string e when e.SequenceEqual("brainpoolP160r1".ToLower()) => ECCurve.NamedCurves.brainpoolP160r1,
      string e when e.SequenceEqual("brainpoolP512t1".ToLower()) => ECCurve.NamedCurves.brainpoolP512t1,
      string e when e.SequenceEqual("brainpoolP512r1".ToLower()) => ECCurve.NamedCurves.brainpoolP512r1,
      string e when e.SequenceEqual("brainpoolP384t1".ToLower()) => ECCurve.NamedCurves.brainpoolP384t1,
      string e when e.SequenceEqual("brainpoolP384r1".ToLower()) => ECCurve.NamedCurves.brainpoolP384r1,
      string e when e.SequenceEqual("brainpoolP320t1".ToLower()) => ECCurve.NamedCurves.brainpoolP320t1,
      string e when e.SequenceEqual("brainpoolP320r1".ToLower()) => ECCurve.NamedCurves.brainpoolP320r1,
      string e when e.SequenceEqual("brainpoolP256r1".ToLower()) => ECCurve.NamedCurves.brainpoolP256r1,
      string e when e.SequenceEqual("brainpoolP224t1".ToLower()) => ECCurve.NamedCurves.brainpoolP224t1,
      string e when e.SequenceEqual("brainpoolP224r1".ToLower()) => ECCurve.NamedCurves.brainpoolP224r1,
      string e when e.SequenceEqual("brainpoolP192t1".ToLower()) => ECCurve.NamedCurves.brainpoolP192t1,
      string e when e.SequenceEqual("brainpoolP192r1".ToLower()) => ECCurve.NamedCurves.brainpoolP192r1,
      string e when e.SequenceEqual("brainpoolP160t1".ToLower()) => ECCurve.NamedCurves.brainpoolP160t1,
      string e when e.SequenceEqual("brainpoolP256t1".ToLower()) => ECCurve.NamedCurves.brainpoolP256t1,
      string e when e.SequenceEqual("secp256k1".ToLower()) => ECCurve.CreateFromFriendlyName("SecP256K1"),
      _ => throw new ArgumentException("Failed", nameof(ecname)),
    };

  }

  public static string[] ToEcCurveList()
  {
    return
    [
      "nistP256"       ,
      "nistP384"       ,
      "nistP521"       ,
      "brainpoolP160r1",
      "brainpoolP512t1",
      "brainpoolP512r1",
      "brainpoolP384t1",
      "brainpoolP384r1",
      "brainpoolP320t1",
      "brainpoolP320r1",
      "brainpoolP256r1",
      "brainpoolP224t1",
      "brainpoolP224r1",
      "brainpoolP192t1",
      "brainpoolP192r1",
      "brainpoolP160t1",
      "brainpoolP256t1",
      "secp256k1"      ,
    ];
  }
}
