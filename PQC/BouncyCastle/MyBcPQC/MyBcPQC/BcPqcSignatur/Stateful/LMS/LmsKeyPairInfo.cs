
using System.Text;
using System.Text.Json.Serialization;

namespace michele.natale.BcPqcs;

using Pointers;
using Services;

/// <summary>
/// Provides keypair methods around the LMS algorithm.
/// </summary>
public sealed class LmsKeyPairInfo : ILmsKeyPairInfo
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
  public LmsParam Parameter
  {
    get; set;
  } = LmsParam.lms_sha256_h5_w1;

/// <summary>
/// C-Tor
/// </summary>
  public LmsKeyPairInfo()
  {
  }

  /// <summary>
  /// C-Tor
  /// </summary>
  /// <param name="info">Desired keypair information.</param>
  public LmsKeyPairInfo(LmsKeyPairInfo info) =>
    this.SetParameters(info);

  /// <summary>
  /// C-Tor
  /// </summary>
  /// <param name="pubkey">Desired PublicKey</param>
  /// <param name="privkey">Desired PrivateKey as bytes</param>
  /// <param name="parameter">Desired LMS-Param</param>
  public LmsKeyPairInfo(
    ReadOnlySpan<byte> pubkey,
    ReadOnlySpan<byte> privkey,
    LmsParam parameter)
      : this(default, pubkey, privkey, parameter)
  {
  }

  /// <summary>
  /// C-Tor
  /// </summary>
  /// <param name="id">Desired id as GUID</param>
  /// <param name="pubkey">Desired PublicKey</param>
  /// <param name="privkey">Desired PrivateKey as bytes</param>
  /// <param name="parameter">Desired LMS-Param</param>
  public LmsKeyPairInfo(
    Guid id, ReadOnlySpan<byte> pubkey,
    ReadOnlySpan<byte> privkey,
    LmsParam parameter) =>
      this.SetParameters(id, pubkey,
        privkey, parameter);

  public UsIPtr<byte> ToPrivKey() =>
    new(this.PrivKey);

  public void Clear()
  {
    this.PubKey = [];
    this.PrivKey = [];
    this.Id = Guid.Empty;
    this.Parameter = LmsParam.lms_sha256_h5_w1;
  }

  public void SaveKeyPair(string filename, bool with_privkey)
  {
    var (h, f) = LmsHF();
    if (File.Exists(filename)) File.Delete(filename);

    var info = new LmsKeyPairInfo(this); //copy
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
    if (obj is LmsKeyPairInfo o)
      return this.Equals(o);

    return false;
  }

  public bool Equals(LmsKeyPairInfo info)
  {
    if (!this.Id.Equals(info.Id)) return false;
    if (this.Parameter != info.Parameter) return false;
    if (!this.PubKey.SequenceEqual(info.PubKey)) return false;

    return this.PrivKey.SequenceEqual(info.PrivKey);
  }

  public override int GetHashCode() =>
      HashCode.Combine(this.Id, this.PubKey, this.PrivKey);

  private void SetParameters(LmsKeyPairInfo info) =>
    this.SetParameters(info.Id, info.PubKey,
      info.PrivKey, info.Parameter);

  private void SetParameters(
    Guid id, ReadOnlySpan<byte> pubkey,
    ReadOnlySpan<byte> privkey,
    LmsParam parameter)
  {
    this.PubKey = pubkey.ToArray();
    this.PrivKey = privkey.ToArray();
    this.Parameter = parameter;
    if (id == Guid.Empty) return;
    this.Id = id;
  }

  public static LmsKeyPairInfo Load_KeyPair(string filename)
  {
    if (!File.Exists(filename))
      throw new FileNotFoundException(nameof(filename));

    var n = NameLms();
    var str = File.ReadAllLines(filename);
    if (!str[0].Contains(n) || !str[2].Contains(n))
      throw new FileNotFoundException(nameof(filename));

    var info = BcPqcServices.
      DeserializeJson<LmsKeyPairInfo>(
        Convert.FromBase64String(str[1]));

    if (info is not null) return info;

    throw new FileNotFoundException(
      $"Load_Key_Pair IsNULL has failed!", nameof(filename));
  }

  private static (string H, string F) LmsHF()
  {
    var tstamp = $" {DateTimeOffset.UtcNow}";
    var footer = $"-----END {NameLms()}-----";
    var header = $"-----BEGIN {NameLms()}-----{tstamp}";
    return (header, footer);
  }

  private static string NameLms() => "LMS KEYPAIR";
}