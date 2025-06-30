

using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.Security;
using System.Security;
using System.Security.Cryptography;


namespace michele.natale.BcPqcs;


using Services;
using System.Text;
using System.Text.Json.Serialization;

/// <summary>
/// Provides methods and tools related to the multi-signature.
/// </summary>
public sealed class SLHDSAMultiSignVerifyInfoFile : ISLHDSAMultiSignVerifyInfo
{
  [JsonIgnore]
  public bool IsDisposed
  {
    get; private set;
  } = true;

  [JsonInclude]
  public SLHDSASignInfo[] MultiSignInfos
  {
    get; private set;
  } = [];

  [JsonInclude]
  public SlhDsaKeyPairInfo MultiKeyPairInfos
  {
    get; private set;
  } = null!;

  [JsonInclude]
  public byte[] MessageHash
  {
    get; private set;
  } = [];

  [JsonInclude]
  public int Signatories
  {
    get; private set;
  } = -1;

  [JsonInclude]
  public string FileName
  {
    get; private set;
  } = string.Empty;

  public SLHDSAMultiSignVerifyInfoFile()
  {
  }

  public SLHDSAMultiSignVerifyInfoFile(
    SLHDSAMultiSignVerifyInfoFile multisignverifiy)
  {
    Assert(multisignverifiy.MultiSignInfos, multisignverifiy.FileName);

    if (multisignverifiy.MultiSignInfos.Length != multisignverifiy.Signatories)
      throw new ArgumentOutOfRangeException(nameof(multisignverifiy));

    this.IsDisposed = false;
    this.FileName = multisignverifiy.FileName;
    this.MessageHash = multisignverifiy.MessageHash;
    this.Signatories = multisignverifiy.Signatories;
    this.MultiSignInfos = multisignverifiy.MultiSignInfos;
    this.MultiKeyPairInfos = multisignverifiy.MultiKeyPairInfos;
  }

  public SLHDSAMultiSignVerifyInfoFile(
    ReadOnlySpan<SLHDSASignInfo> multiinfos, string datafile)
  {
    Assert(multiinfos, datafile);

    var infos = multiinfos.ToArray();
    var m = infos.First().Message;

    this.IsDisposed = false;
    this.FileName = datafile;
    this.MultiSignInfos = infos;
    this.Signatories = multiinfos.Length;
    this.MessageHash = Convert.FromHexString(m);
    this.MultiKeyPairInfos = ToMultiInfo(multiinfos);
  }

  public byte[] MultiSign() =>
      SLHDSA.Sign(this.MultiKeyPairInfos, this.FileName);

  public bool MultiVerify(ReadOnlySpan<byte> signature) =>
      SLHDSA.Verify(this.MultiKeyPairInfos, signature, this.FileName);

  public void Clear()
  {
    if (this.IsDisposed) return;

    if (this.MessageHash is not null)
      Array.Clear(this.MessageHash);

    this.MultiKeyPairInfos.Dispose();

    if (this.MultiSignInfos is not null)
    {
      foreach (var itm in this.MultiSignInfos)
        itm.Clear();
      Array.Clear(this.MultiSignInfos);
    }

    this.MessageHash = [];
    this.Signatories = -1;
    this.MultiSignInfos = [];
    this.FileName = string.Empty;
    this.MultiKeyPairInfos = null!;
  }

  public void Save(string filename)
  {
    var a = BitConverter.GetBytes(this.Signatories);
    var b = this.MessageHash.ToArray();

    var mkp = this.MultiKeyPairInfos.Copy();
    var c = BcPqcServices.SerializeJson(mkp);

    var msi = this.MultiSignInfos.Select(x => x.Serialize()).ToArray();
    var d = BcPqcServices.SerializeJson(msi);

    var e = Encoding.UTF8.GetBytes(filename);

    byte[][] abcd = [a, b, c, d, e];
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
    var e = Encoding.UTF8.GetString(sc?[4]!);

    this.Clear();
    this.FileName = e;
    this.MessageHash = b!;
    this.Signatories = a;
    this.IsDisposed = false;
    this.MultiSignInfos = d!;
    this.MultiKeyPairInfos = c!;
  }


  public static SlhDsaKeyPairInfo ToMultiInfo(
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


  private static void Assert(
    ReadOnlySpan<SLHDSASignInfo> slhdsainfo, string filename)
  {
    if (string.IsNullOrEmpty(filename))
      throw new ArgumentNullException(nameof(filename));

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

  ~SLHDSAMultiSignVerifyInfoFile() => Dispose(false);

  public void Dispose()
  {
    Dispose(true);
    GC.SuppressFinalize(this);
  }

}