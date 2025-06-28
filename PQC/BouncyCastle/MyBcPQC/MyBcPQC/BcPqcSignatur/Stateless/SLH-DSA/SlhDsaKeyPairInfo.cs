
using System.Text;
using System.Text.Json.Serialization;
using Org.BouncyCastle.Crypto.Parameters;

namespace michele.natale.BcPqcs;

using Pointers;
using Services;

/// <summary>
/// Provides keypair methods around the SLH-DSA algorithm.
/// </summary>
public sealed class SlhDsaKeyPairInfo : ISlhDsaKeyPairInfo
{
  [JsonIgnore]
  public bool IsDisposed
  {
    get;
    private set;
  } = true;

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
  private SLHDsaParam Parameter
  {
    get; set;
  } = SLHDsaParam.Slh_Dsa_sha2_128s;

  /// <summary>
  /// C-Tor
  /// </summary>
  public SlhDsaKeyPairInfo()
  {
    this.IsDisposed = false;
  }

  /// <summary>
  /// C-Tor
  /// </summary>
  /// <param name="info">Desired keypair information.</param>
  public SlhDsaKeyPairInfo(SlhDsaKeyPairInfo info) =>
    this.SetParameters(info);

  /// <summary>
  /// C-Tor
  /// </summary>
  /// <param name="pubkey">desired publickey</param>
  /// <param name="privkey">desired privatekey as bytes</param>
  /// <param name="parameter">desired SLH-DSA_Parameter</param>
  public SlhDsaKeyPairInfo(
    ReadOnlySpan<byte> pubkey,
    ReadOnlySpan<byte> privkey,
    SlhDsaParameters parameter)
      : this(default, pubkey, privkey, parameter)
  {
  }

  /// <summary>
  /// C-Tor
  /// </summary>
  /// <param name="id">Desired id as GUID</param>
  /// <param name="pubkey">Desired publickey</param>
  /// <param name="privkey">Desired privatekey as bytes</param>
  /// <param name="parameter">Desired SLH-DSA-Parameter</param>
  public SlhDsaKeyPairInfo(
    Guid id, ReadOnlySpan<byte> pubkey,
    ReadOnlySpan<byte> privkey,
    SlhDsaParameters parameter) =>
      this.SetParameters(id, pubkey,
        privkey, parameter);

  public SlhDsaParameters ToParameter() =>
    ToSLHDsaParameters(this.Parameter);


  public UsIPtr<byte> ToPrivKey() =>
    new(this.PrivKey);

  public void Clear()
  {
    if (this.IsDisposed) return;

    if (this.PubKey is not null)
      Array.Clear(this.PubKey);
    if (this.PrivKey is not null)
      Array.Clear(this.PrivKey);

    this.PubKey = [];
    this.PrivKey = [];
    this.Id = Guid.Empty;
    this.Parameter = SLHDsaParam.Slh_Dsa_sha2_128s;
  }

  public void SaveKeyPair(string filename, bool with_privkey)
  {
    var (h, f) = SLHDsaHF();
    if (File.Exists(filename)) File.Delete(filename);

    using var info = new SlhDsaKeyPairInfo(this); //copy
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
    if (obj is SlhDsaKeyPairInfo o)
      return this.Equals(o);

    return false;
  }

  public bool Equals(SlhDsaKeyPairInfo info)
  {
    if (!this.Id.Equals(info.Id)) return false;
    if (this.Parameter != info.Parameter) return false;
    if (!this.PubKey.SequenceEqual(info.PubKey)) return false;

    return this.PrivKey.SequenceEqual(info.PrivKey);
  }

  public override int GetHashCode() =>
      HashCode.Combine(this.Id, this.PubKey, this.PrivKey);

  private void SetParameters(SlhDsaKeyPairInfo info) =>
    this.SetParameters(info.Id, info.PubKey,
      info.PrivKey, ToSLHDsaParameters(info.Parameter));

  private void SetParameters(
    Guid id, ReadOnlySpan<byte> pubkey,
    ReadOnlySpan<byte> privkey,
    SlhDsaParameters parameter)
  {
    ArgumentNullException.ThrowIfNull(
      parameter, nameof(parameter));

    this.IsDisposed = false;
    this.PubKey = pubkey.ToArray();
    this.PrivKey = privkey.ToArray();
    this.Parameter = FromSLHDsaParameters(parameter);
    if (id == Guid.Empty) return;
    this.Id = id;
  }

  public static SlhDsaParameters ToSLHDsaParameters(SLHDsaParam param) =>
    BcPqcServices.ToSLHDsaParameters(param);

  public static SLHDsaParam FromSLHDsaParameters(SlhDsaParameters parameter) =>
    BcPqcServices.FromSLHDsaParameters(parameter);

  public static SlhDsaKeyPairInfo Load_KeyPair(string filename)
  {
    if (!File.Exists(filename))
      throw new FileNotFoundException(nameof(filename));

    var n = NameSlhDsa();
    var str = File.ReadAllLines(filename);
    if (!str[0].Contains(n) || !str[2].Contains(n))
      throw new FileNotFoundException(nameof(filename));

    var info = BcPqcServices.
      DeserializeJson<SlhDsaKeyPairInfo>(
        Convert.FromBase64String(str[1]));

    if (info is not null) return info;

    throw new FileNotFoundException(
      $"Load_Key_Pair IsNULL has failed!", nameof(filename));
  }

  private static (string H, string F) SLHDsaHF()
  {
    var tstamp = $" {DateTimeOffset.UtcNow}";
    var footer = $"-----END {NameSlhDsa()}-----";
    var header = $"-----BEGIN {NameSlhDsa()}-----{tstamp}";
    return (header, footer);
  }

  private static string NameSlhDsa() => "SLH-DSA KEYPAIR";

  private void Dispose(bool disposing)
  {
    if (!IsDisposed)
    {
      if (disposing) this.Clear();
      IsDisposed = true;
    }
  }

  ~SlhDsaKeyPairInfo() => Dispose(false);

  public void Dispose()
  {
    Dispose(true);
    GC.SuppressFinalize(this);
  }
}