
using System.Text;
using System.Numerics;
using System.Security.Cryptography;
using System.Text.Json.Serialization;


namespace michele.natale.Schnorrs;


using michele.natale.Cryptography;
using static michele.natale.Schnorrs.Services.SchnorrServices;


public struct SchnorrParameters
{

  public byte[] PublicKey { get; set; } = [];
  public byte[] PrivateKey { get; set; } = [];

  [JsonIgnore]
  public SchnorrGroup PQG { get; set; } = default;

  public readonly bool IsEmpty
  {
    get => IsNullOrEmpty(this.PublicKey)
      && IsNullOrEmpty(this.PublicKey) && this.PQG.IsEmpty;
  }

  public SchnorrParameters()
  {
  }
  public SchnorrParameters(string schnorr_param_pmei)
    : this(FromPmei(schnorr_param_pmei))
  {
  }

  public SchnorrParameters(SchnorrParameters schnorr_param)
  {
    this.PQG = schnorr_param.PQG.Copy();
    this.PublicKey = [.. schnorr_param.PublicKey];
    this.PrivateKey = [.. schnorr_param.PrivateKey];
  }

  public SchnorrParameters(SchnorrGroup group)
  {
    this.PQG = group;
    this.PublicKey = this.PrivateKey = [];
  }

  public SchnorrParameters(
    SchnorrGroup group,
    ReadOnlySpan<byte> priv_key, ReadOnlySpan<byte> pub_key)
  {
    this.PQG = group;
    this.PublicKey = pub_key.ToArray();
    this.PrivateKey = priv_key.ToArray();
  }

  public void Reset()
  {
    this.PQG.Reset();
    this.PQG = new SchnorrGroup();
    this.ResetKeyPair();
  }

  public void ResetKeyPair()
  {
    ClearBytes(this.PrivateKey, this.PublicKey);
    this.PrivateKey = this.PublicKey = [];
  }

  public void GenerateParameters(
   bool without_keypair = true,
   HashAlgorithmName hname = default) =>
    this.GenerateParameters(
      P_MIN_SIZE, Q_MIN_SIZE,
      without_keypair, hname);

  public void GenerateParameters(
    int psize, int qsize,
    bool without_keypair = true,
    HashAlgorithmName hname = default)
  {
    this.Reset();
    var tmp = new SchnorrGroup();
    tmp.GenerateParameters(psize, qsize, hname);

    this.PQG = tmp;
    if (!without_keypair) this.GenerateKeyPair();
  }

  public void GenerateKeyPair()
  {
    if (this.PQG.IsEmpty)
      throw new NotImplementedException($"{nameof(this.PQG)} is empty!");

    this.ResetKeyPair();
    var (p, q, g, _) = this.PQG.ToBigIntegers();
    var qsz = BitOfNumber(q);

    var priv = BigInteger.Zero;
    while (priv.IsZero)
    {
      priv = RngBigInteger(qsz);
      priv = ModuloExt(priv, q);
    }
    var pub = BigInteger.ModPow(g, q - priv, p);
    this.PublicKey = pub.ToByteArray();
    this.PrivateKey = priv.ToByteArray();
  }

  public readonly (BigInteger PrivateKey, BigInteger PublicKey) ToKeysBigIntegers()
  {
    return (new BigInteger(this.PrivateKey), new BigInteger(this.PublicKey));
  }

  public readonly (string PrivateKey, string PublicKey) ToKeysHex(bool tolower = true)
  {
    if (tolower)
      return (Convert.ToHexString(this.PrivateKey).ToLower(), Convert.ToHexString(this.PublicKey).ToLower());
    return (Convert.ToHexString(this.PrivateKey).ToUpper(), Convert.ToHexString(this.PublicKey).ToUpper());
  }

  public readonly (byte[] PrivateKey, byte[] PublicKey) ToKeysBytes()
  {
    return (this.PrivateKey, this.PublicKey);
  }


  public readonly byte[] Serialize(bool without_private_key = true)
  {
    return JSerialize(this, without_private_key);
  }

  public void Deserialize(byte[] data_schnorr_param)
  {
    this.Reset();
    var param = JDeserialize(data_schnorr_param);
    this.PrivateKey = param.PrivateKey;
    this.PublicKey = param.PublicKey;
    this.PQG = param.PQG;
  }

  public readonly SchnorrParameters Copy()
  {
    return new SchnorrParameters(this);
  }

  public readonly string ToPmei(bool without_private_key = true)
  {
    var (h, f) = PMEI.SchnorrParamPmeiHF();
    return PMEI.ToPmei(this.Serialize(without_private_key), h, f);
  }


  //Static

  public static byte[] JSerialize(SchnorrParameters param, bool without_private_key = true)
  {
    var bytes = new byte[3][];
    bytes[0] = param.PublicKey;
    bytes[1] = without_private_key ? [] : param.PrivateKey;
    bytes[2] = Encoding.UTF8.GetBytes(param.PQG.ToPmei());
    return SerializeJson(bytes);
  }

  public static SchnorrParameters JDeserialize(byte[] data)
  {
    var result = new SchnorrParameters();
    var bytes = DeserializeJson<byte[][]>(data);
    result.PublicKey = bytes![0];
    result.PrivateKey = bytes[1];
    result.PQG = new SchnorrGroup(Encoding.UTF8.GetString(bytes[2]));
    return result;
  }


  public static SchnorrParameters FromPmei(string param_pmei)
  {
    var (h, f) = PMEI.SchnorrParamPmeiHF(false);
    var (_, msg) = PMEI.FromPmei(param_pmei, h, f);
    return JDeserialize(msg);
  }

  public static SchnorrParameters[] RngSchnorrParameters(
    int size, int psize = P_MIN_SIZE, int qsize = Q_MAX_SIZE, 
    HashAlgorithmName hname = default)
  {
    var group = new SchnorrGroup();
    hname = hname == default ? HashAlgorithmName.SHA512 : hname;
    group.GenerateParameters(psize,qsize, hname);
    return RngSchnorrParameters(size, group);
  }

  public static SchnorrParameters[] RngSchnorrParameters(
    int size, SchnorrGroup group)
  {
    var result = new SchnorrParameters[size];
    for (int i = 0; i < size; i++)
    {
      var param = new SchnorrParameters(group);
      param.GenerateKeyPair();
      result[i] = param;
    }
    return result;
  }
}
