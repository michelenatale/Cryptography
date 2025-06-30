

using System.Security;
using Org.BouncyCastle.Security;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto.Prng;
using System.Text.Json.Serialization;
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
  [JsonIgnore]
  public bool IsDisposed
  {
    get; private set;
  } = true;

  public SLHDSASignInfo[] MultiSignInfos
  {
    get; private set;
  } = [];

  public SlhDsaKeyPairInfo MultiKeyPairInfos
  {
    get; private set;
  } = null!;

  public byte[] Message
  {
    get; private set;
  } = [];

  public int Signatories
  {
    get; private set;
  } = -1;

  public SLHDSAMultiSignVerifyInfo()
  {
  }

  public SLHDSAMultiSignVerifyInfo(
    SLHDSAMultiSignVerifyInfo multisignverifiy)
  {
    Assert(multisignverifiy.MultiSignInfos);

    if (multisignverifiy.MultiSignInfos.Length != multisignverifiy.Signatories)
      throw new ArgumentOutOfRangeException(nameof(multisignverifiy));

    this.IsDisposed = false;
    this.Signatories = multisignverifiy.Signatories;
    this.Message = [.. multisignverifiy.Message];
    this.MultiSignInfos = multisignverifiy.MultiSignInfos;
    this.MultiKeyPairInfos = multisignverifiy.MultiKeyPairInfos;
  }

  public SLHDSAMultiSignVerifyInfo(
    ReadOnlySpan<SLHDSASignInfo> signinfos)
  {
    Assert(signinfos);
    var slhdinfos = signinfos.ToArray();
    var msg = slhdinfos.First().Message;

    this.IsDisposed = false;
    this.Signatories = slhdinfos.Length;
    this.Message = Convert.FromHexString(msg);
    this.MultiSignInfos = signinfos.ToArray();
    this.MultiKeyPairInfos = ToMultiInfo(signinfos);
  }

  public byte[] MultiSign() =>
      SLHDSA.Sign(this.MultiKeyPairInfos, this.Message);

  public bool MultiVerify(ReadOnlySpan<byte> signature) =>
      SLHDSA.Verify(this.MultiKeyPairInfos, signature, this.Message);


  public void Clear()
  {
    if (this.IsDisposed) return;

    if (this.Message is not null)
      Array.Clear(this.Message);

    this.MultiKeyPairInfos.Dispose();

    if (this.MultiSignInfos is not null)
    {
      foreach (var itm in this.MultiSignInfos)
        itm.Clear();
      Array.Clear(this.MultiSignInfos);
    }

    this.Message = [];
    this.Signatories = -1;
    this.MultiSignInfos = [];
    this.MultiKeyPairInfos = null!;
  }

  public void Save(string filename)
  {
    var a = BitConverter.GetBytes(this.Signatories);
    var b = this.Message.ToArray();

    var mkp = this.MultiKeyPairInfos.Copy();
    var c = BcPqcServices.SerializeJson(mkp);

    var msi = this.MultiSignInfos.Select(x => x.Serialize()).ToArray();
    var d = BcPqcServices.SerializeJson(msi);

    byte[][] abcd = [a, b, c, d];
    var serialize = BcPqcServices.SerializeJson(abcd);
    File.WriteAllBytes(filename, serialize);
  }

  public void Load(string filename)
  {
    var bytes = File.ReadAllBytes(filename);
    var sc = BcPqcServices
      .DeserializeJson<byte[][]>(bytes);

    var a = BitConverter.ToInt32(sc?[0]);
    var b = sc?[1];
    var c = BcPqcServices.DeserializeJson<SlhDsaKeyPairInfo>(sc?[2]!);
    var db = BcPqcServices.DeserializeJson<byte[][]>(sc?[3]!);
    var d = db?.Select(BcPqcServices.DeserializeJson<SLHDSASignInfo>).ToArray();

    this.Clear();
    this.IsDisposed = false;
    this.Message = b!;
    this.Signatories = a;
    this.MultiSignInfos = d!;
    this.MultiKeyPairInfos = c!;
  }

  private static SlhDsaKeyPairInfo ToMultiInfo(
    ReadOnlySpan<SLHDSASignInfo> slhdsainfo)
  {
    var slhdinfos = slhdsainfo.ToArray();

    var p = slhdinfos.First().Parameter; 
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
  public static void Assert(
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

  }
  private void Dispose(bool disposing)
  {
    if (!this.IsDisposed)
      if (disposing) this.Clear();
    this.IsDisposed = true;
  }

  ~SLHDSAMultiSignVerifyInfo() => Dispose(false);

  public void Dispose()
  {
    Dispose(true);
    GC.SuppressFinalize(this);
  }

}


