

using System.Security;
using Org.BouncyCastle.Security;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;


namespace michele.natale.BcPqcs;


using Services;

/// <summary>
/// Provides methods and tools related to the multi-signature.
/// </summary>
public sealed class SLHDSAMultiSignVerifyInfo : ISLHDSAMultiSignVerifyInfo
{

  public static SlhDsaKeyPairInfo ToMultiInfo(
    ReadOnlySpan<SLHDSASignInfo> slhdsainfo)
  {
    var slhdinfos = slhdsainfo.ToArray();

    var m = slhdinfos.First().Message;
    var p = slhdinfos.First().Parameter;
    var ls = slhdinfos.First().Sign.Length;
    var lp = slhdinfos.First().PublicKey.Length;

    var istrue = slhdinfos.All(x => p == x.Parameter);
    istrue &= slhdinfos.All(x => ls == x.Sign.Length);
    istrue &= slhdinfos.All(x => lp == x.PublicKey.Length);
    istrue &= slhdinfos.All(x => m.SequenceEqual(x.Message));
    if (!istrue) throw new VerificationException(
      $"Methode: {nameof(ToMultiInfo)}: {nameof(slhdsainfo)} has failde!");

    var signinfo = slhdinfos.OrderBy(x => x.Sign).ToArray();
    var hashs = signinfo.Select(x => x.VerifiyHash()).ToArray();
    var seed = BcPqcServices.XorSpec(hashs);

    var param = BcPqcServices.ToSLHDsaParameters(p);
    var privkey = CreatePrivateKey(seed, param);
    var pubkey = privkey.GetPublicKey();

    return new SlhDsaKeyPairInfo(pubkey.GetEncoded(), privkey.GetEncoded(), param);
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

  private static SlhDsaPrivateKeyParameters CreatePrivateKey(
    byte[] seed, SlhDsaParameters parameter)
  {
    var rand = ToSecureRandom(seed);
    var keypair_generator = new SlhDsaKeyPairGenerator();
    var generator = new SlhDsaKeyGenerationParameters(rand, parameter);
    keypair_generator.Init(generator);

    var kp = keypair_generator.GenerateKeyPair();

    return (SlhDsaPrivateKeyParameters)kp.Private;
  }

  public static byte[] MultiSign(
    SlhDsaKeyPairInfo info, ReadOnlySpan<byte> message) =>
      SLHDSA.Sign(info, message);

  public static bool MultiVerify(SlhDsaKeyPairInfo info,
    ReadOnlySpan<byte> signature, ReadOnlySpan<byte> message) =>
      SLHDSA.Verify(info, signature, message);

  public static byte[] MultiSign(
    SlhDsaKeyPairInfo info, string datapath) =>
      SLHDSA.Sign(info, datapath);

  public static bool MultiVerify(SlhDsaKeyPairInfo info,
    ReadOnlySpan<byte> signature, string datapath) =>
      SLHDSA.Verify(info, signature, datapath);

}




//public static SlhDsaPrivateKeyParameters CreatePrivateKey(byte[] seed, SlhDsaParameters parameters)
//{
//  if (seed.Length != 96)
//    throw new ArgumentException($"{nameof(seed)}.Length has failed.");

//  byte[] skseed = new byte[32], skprf = new byte[32], pkseed = new byte[32];

//  Array.Copy(seed, 0, skseed, 0, 32);
//  Array.Copy(seed, 32, skprf, 0, 32);
//  Array.Copy(seed, 64, pkseed, 0, 32);

//  var ctor = typeof(SlhDsaPrivateKeyParameters).GetConstructor(
//      BindingFlags.NonPublic | BindingFlags.Instance,
//      null,
//      new[] { typeof(byte[]), typeof(byte[]), typeof(byte[]), typeof(SlhDsaParameters) },
//      null
//  );

//  Debugger.Break();


//  var privatekey = (SlhDsaPrivateKeyParameters)ctor?.Invoke(new object[] { skseed, skprf, pkseed, parameters })!;

//  return privatekey;
//}