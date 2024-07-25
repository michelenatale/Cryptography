

using System.Security.Cryptography;

namespace michele.natale.EcCurveDsaDh;

partial class EcService
{
  public static (ECParameters PublicKey, ECParameters PrivateKey) GenerateEcDsaKeyPair(ECCurve ecurve = default)
  {
    ecurve = ecurve.Oid == null ||
             (string.IsNullOrEmpty(ecurve.Oid.Value) && string.IsNullOrEmpty(ecurve.Oid.FriendlyName))
             ? RngEcCurve() : ecurve;
    var ecdh = ECDsa.Create(ecurve);
    return (ecdh.ExportParameters(false), ecdh.ExportParameters(true));
  }

  public static EcSignedMessage SignEcDsa(
    ECParameters privatekey, byte[] message, byte prefix = 0x04)
  {
    using var ecdsa = ECDsa.Create(privatekey.Curve);
    ecdsa.ImportParameters(new ECParameters
    {
      Curve = CopyOrEmpty(privatekey.Curve),
      D = [.. privatekey.D],
    });

    //only with privatekey and publickey and curve
    var keyParameters = ecdsa.ExportExplicitParameters(true);
    var pubkey = EcParametersInfo.ToPuplicKeyConcat(keyParameters.Q, prefix);

    var hash = SHA256.HashData(message);
    var signature = ecdsa.SignData(hash, HashAlgorithmName.SHA256);

    var result = new EcSignedMessage
    {
      PublicKey = Convert.ToHexString(pubkey).ToLower(),
      Signature = Convert.ToHexString(signature).ToLower(),
      MessageHash = Convert.ToHexString(hash).ToLower(),
    };

    return result;
  }

  public static bool VerifyEcDsa(EcSignedMessage message)
  {
    var pubkey = EcPublicKey.FromEcPublicKeyPmei(message.PublicKey);
    using var ecdsa = ECDsa.Create(pubkey.PublicKey.Curve);
    ecdsa.ImportParameters(new ECParameters
    {
      Curve = pubkey.PublicKey.Curve,
      Q = pubkey.PublicKey.Q,
    });

    var hash = Convert.FromHexString(message.MessageHash);

    return ecdsa.VerifyData(
        hash,
        Convert.FromHexString(message.Signature),
        HashAlgorithmName.SHA256);
  }


}
