
using System.Text;
using System.Text.Json.Serialization;
using Org.BouncyCastle.Crypto.Parameters;

namespace michele.natale.BcPqcs;

using Pointers;
using Services;

/// <summary>
/// Provides keypair methods around the ML-DSA algorithm.
/// </summary>
public sealed class MlDsaKeyPairInfo : IMlDsaKeyPairInfo
{
  [JsonInclude]
  public Guid Id
  {
    get; set;
  } = Guid.NewGuid();
  [JsonInclude]
  public byte[] PubKey
  {
    get; set;
  } = [];
  [JsonInclude]
  private byte[] PrivKey
  {
    get; set;
  } = [];
  [JsonInclude]
  private MLDsaParam Parameter
  {
    get; set;
  } = MLDsaParam.Ml_Dsa_44;

  /// <summary>
  /// C-Tor
  /// </summary>
  public MlDsaKeyPairInfo()
  {
  }

  /// <summary>
  /// C-Tor
  /// </summary>
  /// <param name="info">Desired KeyPair information</param>
  public MlDsaKeyPairInfo(MlDsaKeyPairInfo info) =>
    this.SetParameters(info);

  /// <summary> 
  /// C-Tor
  /// </summary>
  /// <param name="pubkey">Desired publickey</param>
  /// <param name="privkey">Desired privatekey</param>
  /// <param name="parameter">desired ML-DSA-Parameter</param>
  public MlDsaKeyPairInfo(
    ReadOnlySpan<byte> pubkey,
    ReadOnlySpan<byte> privkey,
    MLDsaParameters parameter)
      : this(default, pubkey, privkey, parameter)
  {
  }

  /// <summary>
  /// C-Tor
  /// </summary>
  /// <param name="id">Desired id as Guid</param>
  /// <param name="pubkey">Desired publickey</param>
  /// <param name="privkey">Desired privatekey</param>
  /// <param name="parameter">desired ML-DSA-Parameter</param>
  public MlDsaKeyPairInfo(
    Guid id, ReadOnlySpan<byte> pubkey,
    ReadOnlySpan<byte> privkey,
    MLDsaParameters parameter) =>
      this.SetParameters(id, pubkey,
        privkey, parameter);

  public MLDsaParameters ToParameter() =>
    ToMLDsaParameters(this.Parameter);

  public UsIPtr<byte> ToPrivKey() =>
    new(this.PrivKey);

  public void Clear()
  {
    this.PubKey = [];
    this.PrivKey = [];
    this.Id = Guid.Empty;
    this.Parameter = MLDsaParam.Ml_Dsa_44;
  }

  public void SaveKeyPair(string filename, bool with_privkey)
  {
    var (h, f) = MLDsaHF();
    if (File.Exists(filename)) File.Delete(filename);

    var info = new MlDsaKeyPairInfo(this); //copy
    if (!with_privkey) info.PrivKey = [];

    var b64 = Convert.ToBase64String(
      BcPqcServices.SerializeJson(info));

    var result = new StringBuilder();
    result.AppendLine(h);
    result.AppendLine(b64);
    result.Append(f);

    File.WriteAllText(filename, result.ToString());
  }

  public void LoadKeyPair(string filename)
  {
    this.Clear();
    var info = Load_KeyPair(filename);
    this.SetParameters(info);
  }

  public override bool Equals(object? obj)
  {
    if (obj is MlDsaKeyPairInfo o)
      return this.Equals(o);

    return false;
  }

  public bool Equals(MlDsaKeyPairInfo info)
  {
    if (!this.Id.Equals(info.Id)) return false;
    if (this.Parameter != info.Parameter) return false;
    if (!this.PubKey.SequenceEqual(info.PubKey)) return false;

    return this.PrivKey.SequenceEqual(info.PrivKey);
  }

  public override int GetHashCode() =>
      HashCode.Combine(this.Id, this.PubKey, this.PrivKey);

  private void SetParameters(MlDsaKeyPairInfo info) =>
    this.SetParameters(info.Id, info.PubKey,
      info.PrivKey, ToMLDsaParameters(info.Parameter));

  private void SetParameters(
    Guid id, ReadOnlySpan<byte> pubkey,
    ReadOnlySpan<byte> privkey,
    MLDsaParameters parameter)
  {
    ArgumentNullException.ThrowIfNull(
      parameter, nameof(parameter));

    this.PubKey = pubkey.ToArray();
    this.PrivKey = privkey.ToArray();
    this.Parameter = FromMLDsaParameters(parameter);
    if (id == Guid.Empty) return;
    this.Id = id;
  }

  public static MLDsaParameters ToMLDsaParameters(MLDsaParam param) =>
    BcPqcServices.ToMLDsaParameters(param);

  public static MLDsaParam FromMLDsaParameters(MLDsaParameters parameter) =>
    BcPqcServices.FromMLDsaParameters(parameter);

  public static MlDsaKeyPairInfo Load_KeyPair(string filename)
  {
    if (!File.Exists(filename))
      throw new FileNotFoundException(nameof(filename));

    var n = NameMLDsa();
    var str = File.ReadAllLines(filename);
    if (!str[0].Contains(n) || !str[2].Contains(n))
      throw new FileNotFoundException(nameof(filename));

    var info = BcPqcServices.
      DeserializeJson<MlDsaKeyPairInfo>(
        Convert.FromBase64String(str[1]));

    if (info is not null) return info;

    throw new FileNotFoundException(
      $"Load_Key_Pair IsNULL has failed!", nameof(filename));
  }

  private static (string H, string F) MLDsaHF()
  {
    var tstamp = $" {DateTimeOffset.UtcNow}";
    var footer = $"-----END {NameMLDsa()}-----";
    var header = $"-----BEGIN {NameMLDsa()}-----{tstamp}";
    return (header, footer);
  }

  private static string NameMLDsa() => "ML-DSA KEYPAIR";
}