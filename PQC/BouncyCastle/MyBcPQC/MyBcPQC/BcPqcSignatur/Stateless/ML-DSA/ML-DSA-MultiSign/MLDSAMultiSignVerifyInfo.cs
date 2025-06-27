

using System.Security;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto.Parameters;


namespace michele.natale.BcPqcs;

using Services;

/// <summary>
/// Provides methods and tools related to the multi-signature.
/// </summary>
public sealed class MLDSAMultiSignVerifyInfo: IMLDSAMultiSignVerifyInfo
{

  public static MlDsaKeyPairInfo ToMultiInfo(
    ReadOnlySpan<MLDSASignInfo> mldsainfo)
  {
    var mldinfos = mldsainfo.ToArray();

    var m = mldinfos.First().Message;
    var p = mldinfos.First().Parameter;
    var ls = mldinfos.First().Sign.Length;
    var lp = mldinfos.First().PublicKey.Length;

    var istrue = mldinfos.All(x => p == x.Parameter);
    istrue &= mldinfos.All(x => ls == x.Sign.Length);
    istrue &= mldinfos.All(x => lp == x.PublicKey.Length);
    istrue &= mldinfos.All(x => m.SequenceEqual(x.Message));
    if (!istrue) throw new VerificationException(
      $"Methode: {nameof(ToMultiInfo)}: {nameof(mldsainfo)} has failde!");

    var signinfo = mldinfos.OrderBy(x => x.Sign).ToArray();
    var hashs = signinfo.Select(x => x.VerifiyHash()).ToArray();
    var common = BcPqcServices.XorSpec(hashs);

    var seed = SHA256.HashData(common);
    var param = BcPqcServices.ToMLDsaParameters(p);
    var privkey = MLDsaPrivateKeyParameters.FromSeed(param, seed);
    var pubkey = privkey.GetPublicKey();

    return new MlDsaKeyPairInfo(pubkey.GetEncoded(), privkey.GetEncoded(), param);
  }

  public static byte[] MultiSign(
    MlDsaKeyPairInfo info, ReadOnlySpan<byte> message) =>
      MLDSA.Sign(info, message);

  public static bool MultiVerify(MlDsaKeyPairInfo info,
    ReadOnlySpan<byte> signature, ReadOnlySpan<byte> message) =>
      MLDSA.Verify(info, signature, message);

  public static byte[] MultiSign(
    MlDsaKeyPairInfo info, string datapath) =>
      MLDSA.Sign(info, datapath);

  public static bool MultiVerify(MlDsaKeyPairInfo info,
    ReadOnlySpan<byte> signature, string datapath) =>
      MLDSA.Verify(info, signature, datapath);

}
