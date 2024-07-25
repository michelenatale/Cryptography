

using System.Security.Cryptography;

namespace michele.natale.EcCurveDsaDh;

partial class EcService
{


  public static (ECParameters PublicKey, ECParameters PrivateKey) GenerateEcDhKeyPair(ECCurve ecurve = default)
  {
    ecurve = ecurve.Oid == null ||
             (string.IsNullOrEmpty(ecurve.Oid.Value) && string.IsNullOrEmpty(ecurve.Oid.FriendlyName))
             ? RngEcCurve() : ecurve;
    var ecdh = ECDiffieHellman.Create(ecurve);
    return (ecdh.ExportParameters(false), ecdh.ExportParameters(true));
  }

  public static byte[] ToSharedKey(
    ECParameters ec_private_key_firstuser /*alice*/,
    ECParameters ec_public_key_lastuser /*bob*/)
  {
    //ECDiffieHellmanPublicKey wird glech wieder zurückgesetzt!!
    using var public_key = ToEcDhPublicKey(ec_public_key_lastuser);
    return ToSharedKey(ec_private_key_firstuser, public_key);
  }

  public static byte[] ToSharedKey(
    ECParameters ec_private_key_firstuser /*alice*/,
    ECDiffieHellmanPublicKey ec_public_key_lastuser /*bob*/)
  {
    //ECDiffieHellmanPublicKey wird beim Ursprungsort zurückgesetzt!!
    var ecdh_alice = ECDiffieHellman.Create(new ECParameters
    {
      Curve = ec_private_key_firstuser.Curve,
      D = [.. ec_private_key_firstuser.D!],
    });
    return ecdh_alice.DeriveKeyMaterial(ec_public_key_lastuser);
  }

  public static ECDiffieHellmanPublicKey ToEcDhPublicKey(ECParameters key)
  {
    if (key.Curve.Oid is null)
      throw new ArgumentException($"ECParameters.{nameof(key)} has failed!");
    using var ecdh = ECDiffieHellman.Create(key);
    //ecdh.ImportParameters(key);
    return ecdh.PublicKey;
  }

  public static ECParameters FromEcDhPublicKey(ECDiffieHellmanPublicKey key)
  {
    //ECDiffieHellmanPublicKey wird beim Ursprungsort zurückgesetzt!!
    return key.ExportParameters();
  }

}
