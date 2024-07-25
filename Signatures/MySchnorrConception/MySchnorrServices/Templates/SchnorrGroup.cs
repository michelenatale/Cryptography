
using System.Text;
using System.Numerics;
using System.Security.Cryptography;


namespace michele.natale.Schnorrs;

using michele.natale.Cryptography;

using static michele.natale.Schnorrs.Services.SchnorrServices;

//https://en.wikipedia.org/wiki/Schnorr_group


public struct SchnorrGroup
{
  public byte[] P { get; set; } = [];
  public byte[] Q { get; set; } = [];
  public byte[] G { get; set; } = [];
  public HashAlgorithmName HashName { get; set; } = HashAlgorithmName.SHA512;

  public readonly bool IsEmpty
  {
    get => IsNullOrEmpty(this.P, this.Q, this.G);
  }

  public SchnorrGroup()
  {
  }

  public SchnorrGroup(SchnorrGroup group)
    : this(group.P, group.Q, group.G, group.HashName)
  {
  }

  public SchnorrGroup((
    byte[] p, byte[] q, byte[] g) pqg,
    HashAlgorithmName hname = default)
    : this(pqg.p, pqg.q, pqg.g, hname)
  {
  }

  public SchnorrGroup((
    byte[] p, byte[] q, byte[] g,
    HashAlgorithmName hname) pqgh)
    : this(pqgh.p, pqgh.q, pqgh.g, pqgh.hname)
  {
  }

  public SchnorrGroup(
    ReadOnlySpan<byte> p,
    ReadOnlySpan<byte> q,
    ReadOnlySpan<byte> g,
    HashAlgorithmName hname = default)
  {
    this.P = p.ToArray();
    this.Q = q.ToArray();
    this.G = g.ToArray();
    this.HashName = hname == default ? HashAlgorithmName.SHA512 : hname;
  }

  public SchnorrGroup((
    BigInteger p,
    BigInteger q,
    BigInteger g) pqg,
    HashAlgorithmName hname = default)
    : this(pqg.p, pqg.q, pqg.g, hname)
  {
  }

  public SchnorrGroup(
    BigInteger p,
    BigInteger q,
    BigInteger g,
    HashAlgorithmName hname = default)
  {
    this.P = p.ToByteArray();
    this.Q = q.ToByteArray();
    this.G = g.ToByteArray();
    this.HashName = hname == default ? HashAlgorithmName.SHA512 : hname;
  }

  public SchnorrGroup(string group_pmei)
  {
    var (h, f) = PMEI.SchnorrPQGHPmeiHF(false);
    var (_, msg) = PMEI.FromPmei(group_pmei, h, f);
    var (p, q, g, hn) = PmeiPQG.JDeserialize(msg).ToParameters();
    this.P = p; this.Q = q; this.G = g; this.HashName = hn;
  }

  public readonly SchnorrGroup Copy()
  {
    return new SchnorrGroup(this);
  }

  public void GenerateParameters(HashAlgorithmName hname = default)
  {
    this.Reset();
    this = new SchnorrGroup();
    var pqg = GenerateParametersPQG();
    this.P = pqg.P; this.Q = pqg.Q; this.G = pqg.G;
    this.HashName = hname == default ? HashAlgorithmName.SHA512 : hname;
  }

  public void GenerateParameters(
    int psize, int qsize, HashAlgorithmName hname = default)
  {
    this.Reset();
    this = new SchnorrGroup();
    var pqg = GenerateParametersPQG(psize, qsize);
    this.P = pqg.P; this.Q = pqg.Q; this.G = pqg.G;
    this.HashName = hname == default ? HashAlgorithmName.SHA512 : hname;
  }

  public readonly (BigInteger P, BigInteger Q, BigInteger G, HashAlgorithmName HName) ToBigIntegers()
  {
    return (new BigInteger(this.P), new BigInteger(this.Q), new BigInteger(this.G), this.HashName);
  }

  public readonly (string P, string Q, string G, string HName) ToHex(bool tolower = true)
  {
    if (tolower)
      return (Convert.ToHexString(this.P).ToLower(), Convert.ToHexString(this.Q).ToLower(), Convert.ToHexString(this.G).ToLower(), Convert.ToHexString(Encoding.UTF8.GetBytes(this.HashName.Name!)).ToLower());
    return (Convert.ToHexString(this.P).ToUpper(), Convert.ToHexString(this.Q).ToUpper(), Convert.ToHexString(this.G).ToUpper(), Convert.ToHexString(Encoding.UTF8.GetBytes(this.HashName.Name!)).ToUpper());
  }

  public readonly (byte[] P, byte[] Q, byte[] G, HashAlgorithmName HName) ToBytes()
  {
    return (this.P, this.Q, this.G, this.HashName);
  }

  public readonly string ToPmei()
  {
    var (h, f) = PMEI.SchnorrPQGHPmeiHF();
    var pqgh = new PmeiPQG(this.P, this.Q, this.G, this.HashName).Serialize();
    return PMEI.ToPmei(pqgh, h, f);
  }

  public void Reset()
  {
    ClearBytes(this.P, this.Q, this.G);
    this.P = this.Q = this.G = [];
    this.HashName = HashAlgorithmName.SHA512;
  }

  public readonly bool Equality(SchnorrGroup group)
  {
    return Equality(this, group);
  }


  //Statics

  public static (byte[] P, byte[] Q, byte[] G, HashAlgorithmName HName) FromHex(
    (string p_hex, string q_hex, string g_hex, string hname) pqg_hex)
  {
    var (p, q, g, hn) = pqg_hex;
    return (Convert.FromHexString(p),
      Convert.FromHexString(q), Convert.FromHexString(g),
      new HashAlgorithmName(Encoding.UTF8.GetString(Convert.FromHexString(hn))));
  }

  public static SchnorrGroup FromPmei(string group_pmei)
  {
    var result = new SchnorrGroup();
    var (h, f) = PMEI.SchnorrPQGHPmeiHF(false);
    var (_, msg) = PMEI.FromPmei(group_pmei, h, f);
    var (p, q, g, hn) = PmeiPQG.JDeserialize(msg).ToParameters();
    result.P = p; result.Q = q; result.G = g; result.HashName = hn;
    return result;
  }

  public static bool Equality(SchnorrGroup left, SchnorrGroup right)
  {
    if (!left.P.SequenceEqual(right.P)) return false;
    if (!left.G.SequenceEqual(right.G)) return false;
    if (!left.Q.SequenceEqual(right.Q)) return false;
    return left.HashName == right.HashName;
  }
}
