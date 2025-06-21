
using System.Text;
using System.Text.Json.Serialization;
using Org.BouncyCastle.Crypto.Parameters;

namespace michele.natale.BcPqcs;

using Services;

public class MlKemKeyPairInfo
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
  public byte[] PrivKey
  {
    get; set;
  } = [];
  [JsonInclude]
  private MLKemParam Parameter
  {
    get; set;
  } = MLKemParam.Ml_Kem_512;
  [JsonInclude]
  public CryptionAlgorithm CryptAlgo
  {
    get; set;
  } = CryptionAlgorithm.AES_GCM;

  public MlKemKeyPairInfo()
  {
  }

  public MlKemKeyPairInfo(MlKemKeyPairInfo info) =>
    this.SetParameters(info);

  public MlKemKeyPairInfo(
    ReadOnlySpan<byte> pubkey,
    ReadOnlySpan<byte> privkey,
    MLKemParameters parameter,
    CryptionAlgorithm cryptoalgo)
      : this(default, pubkey, privkey,
          parameter, cryptoalgo)
  {
  }

  public MlKemKeyPairInfo(
    Guid id, ReadOnlySpan<byte> pubkey,
    ReadOnlySpan<byte> privkey,
    MLKemParameters parameter,
    CryptionAlgorithm cryptoalgo) =>
      this.SetParameters(id, pubkey,
        privkey, parameter, cryptoalgo);

  public MLKemParameters ToParameter =>
    ToMLKemParameters(this.Parameter);

  public void Clear()
  {
    this.PubKey = [];
    this.PrivKey = [];
    this.Id = Guid.Empty;
    this.Parameter = MLKemParam.Ml_Kem_512;
    this.CryptAlgo = CryptionAlgorithm.AES_GCM;
  }

  public void SaveKeyPair(string filename, bool with_privkey)
  {
    var (h, f) = MlKemHF();
    if (File.Exists(filename)) File.Delete(filename);

    var info = new MlKemKeyPairInfo(this); //copy
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
    if (obj is MlKemKeyPairInfo o)
      return this.Equals(o);

    return false;
  }

  public bool Equals(MlKemKeyPairInfo info)
  {
    if (!this.Id.Equals(info.Id)) return false;
    if (this.Parameter != info.Parameter) return false;
    if (!this.PubKey.SequenceEqual(info.PubKey)) return false;
    if (!this.PrivKey.SequenceEqual(info.PrivKey)) return false;

    return this.CryptAlgo == info.CryptAlgo;
  }

  public override int GetHashCode() =>
      HashCode.Combine(this.Id, this.PubKey, this.PrivKey);

  private void SetParameters(MlKemKeyPairInfo info) =>
    this.SetParameters(info.Id, info.PubKey,
      info.PrivKey, ToMLKemParameters(info.Parameter), info.CryptAlgo);

  private void SetParameters(
    Guid id, ReadOnlySpan<byte> pubkey,
    ReadOnlySpan<byte> privkey,
    MLKemParameters parameter,
    CryptionAlgorithm cryptoalgo)
  {
    ArgumentNullException.ThrowIfNull(
      parameter, nameof(parameter));

    this.CryptAlgo = cryptoalgo;
    this.PubKey = pubkey.ToArray();
    this.PrivKey = privkey.ToArray();
    this.Parameter = FromMLKemParameters(parameter);
    if (id == Guid.Empty) return;
    this.Id = id;
  }

  public static MLKemParameters ToMLKemParameters(MLKemParam param) =>
    BcPqcServices.ToMLKemParameters(param);

  public static MLKemParam FromMLKemParameters(MLKemParameters parameter) =>
    BcPqcServices.FromMLKemParameters(parameter);

  public static MlKemKeyPairInfo Load_KeyPair(string filename)
  {
    if (!File.Exists(filename))
      throw new FileNotFoundException(nameof(filename));

    var n = NameMLKEM();
    var str = File.ReadAllLines(filename);
    if (!str[0].Contains(n) || !str[2].Contains(n))
      throw new FileNotFoundException(nameof(filename));

    var info = BcPqcServices.
      DeserializeJson<MlKemKeyPairInfo>(
        Convert.FromBase64String(str[1]));

    if (info is not null) return info;

    throw new FileNotFoundException(
      $"Load_Key_Pair IsNULL has failed!", nameof(filename));
  }

  private static (string H, string F) MlKemHF()
  {
    var tstamp = $" {DateTimeOffset.UtcNow}";
    var footer = $"-----END {NameMLKEM()}-----";
    var header = $"-----BEGIN {NameMLKEM()}-----{tstamp}";
    return (header, footer);
  }

  private static string NameMLKEM() => "ML-KEM KEYPAIR";
}