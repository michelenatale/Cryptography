

using System.Security;
using Org.BouncyCastle.Security;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Pqc.Crypto.Lms;
using MyBcPQC.BcPqcSignatur.Stateful.LMS.Interface;


namespace michele.natale.BcPqcs;


using Services;


/// <summary>
/// Provides methods and tools related to the multi-signature.
/// </summary>
public sealed class LMSMultiSignVerifyInfo : ILMSMultiSignVerifyInfo
{

  public static LmsKeyPairInfo ToMultiInfo(
    ReadOnlySpan<LMSSignInfo> lmsinfo)
  {
    var slhdinfos = lmsinfo.ToArray();

    var m = slhdinfos.First().Message;
    var p = slhdinfos.First().Parameter;
    var ls = slhdinfos.First().Sign.Length;
    var lp = slhdinfos.First().PublicKey.Length;

    var istrue = slhdinfos.All(x => p == x.Parameter);
    istrue &= slhdinfos.All(x => ls == x.Sign.Length);
    istrue &= slhdinfos.All(x => lp == x.PublicKey.Length);
    istrue &= slhdinfos.All(x => m.SequenceEqual(x.Message));
    if (!istrue) throw new VerificationException(
      $"Methode: {nameof(ToMultiInfo)}: {nameof(lmsinfo)} has failde!");

    var signinfo = slhdinfos.OrderBy(x => x.Sign).ToArray();
    var hashs = signinfo.Select(x => x.VerifiyHash()).ToArray();
    var seed = BcPqcServices.XorSpec(hashs);

    var privkey = CreatePrivateKey(seed, p);
    var pubkey = privkey.GetPublicKey();

    return new LmsKeyPairInfo(pubkey.GetEncoded(), privkey.GetEncoded(), p);
  }

  private static SecureRandom ToSecureRandom(byte[] seed)
  {
    var s = seed.Length / 2;
    if (seed.Length != 96)
      seed = [.. SHA512.HashData([.. seed.Take(s)]), .. SHA256.HashData([.. seed.Skip(s)])];

    var rnggenerator = new DigestRandomGenerator(new Sha256Digest());
    rnggenerator.AddSeedMaterial(seed);

    return new SecureRandom(rnggenerator);
  }

  private static LmsPrivateKeyParameters CreatePrivateKey(
    byte[] seed, LmsParam parameter)
  {
    var rand = ToSecureRandom(seed);
    var keypair_generator = new LmsKeyPairGenerator();
    var generator = BcPqcServices.ToLmsKeyGenerationParameter(parameter, rand);
    keypair_generator.Init(generator);

    var kp = keypair_generator.GenerateKeyPair();

    return (LmsPrivateKeyParameters)kp.Private;
  }

  public static byte[] MultiSign(
    LmsKeyPairInfo info, ReadOnlySpan<byte> message) =>
      LMS.Sign(info, message);

  public static bool MultiVerify(LmsKeyPairInfo info,
    ReadOnlySpan<byte> signature, ReadOnlySpan<byte> message) =>
      LMS.Verify(info, signature, message);

  public static byte[] MultiSign(
    LmsKeyPairInfo info, string datapath) =>
      LMS.Sign(info, datapath);

  public static bool MultiVerify(LmsKeyPairInfo info,
    ReadOnlySpan<byte> signature, string datapath) =>
      LMS.Verify(info, signature, datapath);

}
