 
using System.Text;
using System.Security;
using System.Security.Cryptography;
using System.Text.Json.Serialization;
using Org.BouncyCastle.Crypto.Parameters;


namespace michele.natale.BcPqcs;

using Services;

/// <summary>
/// Provides methods and tools related to the multi-signature.
/// </summary>
public sealed class MLDSAMultiSignVerifyInfoFile : IMLDSAMultiSignVerifyInfo
{
  [JsonIgnore]
  public bool IsDisposed
  {
    get; private set;
  } = true;

  [JsonInclude]
  public MLDSASignInfo[] MultiSignInfos
  {
    get; private set;
  } = [];

  [JsonInclude]
  public MlDsaKeyPairInfo MultiKeyPairInfos
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

  public MLDSAMultiSignVerifyInfoFile()
  {
  }

  public MLDSAMultiSignVerifyInfoFile(
    MLDSAMultiSignVerifyInfoFile multisignverifiy)
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

  public MLDSAMultiSignVerifyInfoFile(
    ReadOnlySpan<MLDSASignInfo> multiinfos, string datafile)
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
      MLDSA.Sign(this.MultiKeyPairInfos, this.FileName);

  public bool MultiVerify(ReadOnlySpan<byte> signature) =>
      MLDSA.Verify(this.MultiKeyPairInfos, signature, this.FileName);

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
    var c = BcPqcServices.DeserializeJson<MlDsaKeyPairInfo>(sc?[2]!);
    var db = BcPqcServices.DeserializeJson<byte[][]>(sc?[3]!);
    var d = db?.Select(BcPqcServices.DeserializeJson<MLDSASignInfo>).ToArray();
    var e = Encoding.UTF8.GetString(sc?[4]!);

    this.Clear();
    this.FileName = e;
    this.MessageHash = b!;
    this.Signatories = a;
    this.IsDisposed = false;
    this.MultiSignInfos = d!;
    this.MultiKeyPairInfos = c!;
  }

  private static MlDsaKeyPairInfo ToMultiInfo(
    ReadOnlySpan<MLDSASignInfo> mldsainfo)
  {
    var mldinfos = mldsainfo.ToArray();
    var p = mldinfos.First().Parameter;
    var signinfo = mldinfos.OrderBy(x => x.Sign).ToArray();
    var hashs = signinfo.Select(x => x.VerifiyHash()).ToArray();
    var common = BcPqcServices.XorSpec(hashs);

    var seed = SHA256.HashData(common);
    var param = BcPqcServices.ToMLDsaParameters(p);
    var privkey = MLDsaPrivateKeyParameters.FromSeed(param, seed);
    var pubkey = privkey.GetPublicKey();

    return new MlDsaKeyPairInfo(pubkey.GetEncoded(), privkey.GetEncoded(), param);
  }

  private static void Assert(
    ReadOnlySpan<MLDSASignInfo> mldsainfo, string datafile)
  {
    if (string.IsNullOrEmpty(datafile))
      throw new ArgumentNullException(nameof(datafile));

    if (mldsainfo.Length < 3)
      throw new ArgumentOutOfRangeException(nameof(mldsainfo));

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
      $"Methode: {nameof(Assert)}: {nameof(mldsainfo)} has failde!");

  }

  private void Dispose(bool disposing)
  {
    if (!this.IsDisposed)
      if (disposing) this.Clear();
    this.IsDisposed = true;
  }

  ~MLDSAMultiSignVerifyInfoFile() => Dispose(false);

  public void Dispose()
  {
    Dispose(true);
    GC.SuppressFinalize(this);
  }
}
