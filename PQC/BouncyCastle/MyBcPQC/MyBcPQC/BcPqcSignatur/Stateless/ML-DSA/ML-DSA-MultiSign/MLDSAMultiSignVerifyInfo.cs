

using System.Security;
using System.Security.Cryptography;
using System.Text.Json.Serialization;
using Org.BouncyCastle.Crypto.Parameters;


namespace michele.natale.BcPqcs;


using Services;

/// <summary>
/// Provides methods and tools related to the multi-signature.
/// </summary>
public sealed class MLDSAMultiSignVerifyInfo: IMLDSAMultiSignVerifyInfo
{
  [JsonIgnore]
  public bool IsDisposed
  {
    get; private set;
  } = true;

  public MLDSASignInfo[] MultiSignInfos
  {
    get; private set;
  } = [];

  public MlDsaKeyPairInfo MultiKeyPairInfos
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

  public MLDSAMultiSignVerifyInfo()
  {
  }

  public MLDSAMultiSignVerifyInfo(
    MLDSAMultiSignVerifyInfo multisignverifiy)
  {
    Assert(multisignverifiy.MultiSignInfos);

    if (multisignverifiy.MultiSignInfos.Length != multisignverifiy.Signatories)
      throw new ArgumentOutOfRangeException(nameof(multisignverifiy));

    this.IsDisposed = false;
    this.Message = [.. multisignverifiy.Message];
    this.Signatories = multisignverifiy.Signatories;
    this.MultiSignInfos = multisignverifiy.MultiSignInfos;
    this.MultiKeyPairInfos = multisignverifiy.MultiKeyPairInfos;
  }

  public MLDSAMultiSignVerifyInfo(
    ReadOnlySpan<MLDSASignInfo> signinfos)
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
      MLDSA.Sign(this.MultiKeyPairInfos, this.Message);

  public bool MultiVerify(ReadOnlySpan<byte> signature) =>
      MLDSA.Verify(this.MultiKeyPairInfos, signature, this.Message);


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
    var c = BcPqcServices.DeserializeJson<MlDsaKeyPairInfo>(sc?[2]!);
    var db = BcPqcServices.DeserializeJson<byte[][]>(sc?[3]!);
    var d = db?.Select(BcPqcServices.DeserializeJson<MLDSASignInfo>).ToArray();

    this.Clear();
    this.IsDisposed = false;
    this.Message = b!;
    this.Signatories = a;
    this.MultiSignInfos = d!;
    this.MultiKeyPairInfos = c!;
  }


  public static MlDsaKeyPairInfo ToMultiInfo(
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
    ReadOnlySpan<MLDSASignInfo> mldsainfo)
  {
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

  ~MLDSAMultiSignVerifyInfo() => Dispose(false);

  public void Dispose()
  {
    Dispose(true);
    GC.SuppressFinalize(this);
  }
}
