
using System.Text;
using System.Security.Cryptography;

namespace michele.natale.EcCurveDsaDh;

partial class EcService
{

  public static (RSAParameters PublicKey, RSAParameters PrivateKey) GenerateRsaKeyPair(int key_length)
  {
    using var rsa = RSA.Create();
    rsa.KeySize = key_length;
    return (rsa.ExportParameters(false),
            rsa.ExportParameters(true));
  }

  public static RsaSignedMessage SignRsa(ReadOnlySpan<byte> bytes, ReadOnlySpan<byte> private_key_serialize)
  {
    var rsapi = new RsaParametersInfo(private_key_serialize.ToArray());
    return SignRsa(bytes, rsapi.ToRSAParameters);
  }

  public static RsaSignedMessage SignRsa(ReadOnlySpan<byte> bytes, RSAParameters private_key)
  {
    using var rsa = RSA.Create();
    rsa.ImportParameters(private_key);
    var hash = SHA256.HashData(bytes);

    var hash_algo_name = nameof(SHA256);
    var rsa_formatter = new RSAPKCS1SignatureFormatter(rsa);
    rsa_formatter.SetHashAlgorithm(hash_algo_name);
    var signatur = Convert.ToHexString(rsa_formatter.CreateSignature(hash));
    var pub_key = new RsaPublicKey(private_key).PublicKey;
    var pub_key_bytes = RsaParametersInfo.SerializeRsaParam(pub_key);

    return new RsaSignedMessage
    {
      PublicKey = Convert.ToHexString(pub_key_bytes).ToLower(),
      Signature = signatur,
      MessageHash = Convert.ToHexString(hash).ToLower(),
    };
  }

  public static bool VerifySignatureRsa(ReadOnlySpan<byte> bytes, RsaSignedMessage sign_msg)
  {
    var tmp = RsaParametersInfo.DeserializeRsaParam(
      Convert.FromHexString(sign_msg.PublicKey));
    var pub_key = new RSAParameters
    {
      Exponent = [.. tmp.Exponent!],
      Modulus = [.. tmp.Modulus!],
    };
    using var rsa = RSA.Create(pub_key);
    var hash = SHA256.HashData(bytes);
    if (Convert.FromHexString(sign_msg.MessageHash).SequenceEqual(hash))
    {
      var hashalgoname = nameof(SHA256);
      var rsaformatter = new RSAPKCS1SignatureDeformatter(rsa);
      rsaformatter.SetHashAlgorithm(hashalgoname);
      return rsaformatter.VerifySignature(hash, Convert.FromHexString(sign_msg.Signature));
    }
    throw new CryptographicException($"{nameof(VerifySignatureRsa)} has failed!");
  }

  public static bool VerifySignatureRsa(
      string plaintext, string signaturehex, byte[] public_key_serialize)
  {
    var rsapi = new RsaParametersInfo(public_key_serialize.ToArray());
    return VerifySignatureRsa(Encoding.UTF8.GetBytes(plaintext),
      Convert.FromHexString(signaturehex), rsapi.ToRSAParameters);
  }

  public static bool VerifySignatureRsa(
    string plaintext, string signaturehex, RSAParameters publickey) =>
      VerifySignatureRsa(Encoding.UTF8.GetBytes(plaintext),
        Convert.FromHexString(signaturehex), publickey);

  public static bool VerifySignatureRsa(
  ReadOnlySpan<byte> bytes, ReadOnlySpan<byte> signature, RSAParameters publickey)
  {
    using var rsa = RSA.Create();
    rsa.ImportParameters(publickey);
    var hash = SHA256.HashData(bytes);

    var hashalgoname = nameof(SHA256);
    var rsaformatter = new RSAPKCS1SignatureDeformatter(rsa);
    rsaformatter.SetHashAlgorithm(hashalgoname);
    return rsaformatter.VerifySignature(hash, signature.ToArray());
  }
}

