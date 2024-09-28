
using System.Numerics;
using System.Security.Cryptography;
using System.Text.Json.Serialization;

namespace michele.natale.Schnorrs;

using michele.natale.Schnorrs.EcServices;
using static michele.natale.Schnorrs.EcServices.EcSchnorrServices;

public struct EcSchnorrParameters
{
  public byte[] PrivateKey { get; set; } = [];
  public ECPoint PublicKey { get; set; } = EcPointEmpty;
  public HashAlgorithmName HashAlgorithmName { get; set; } = DEFAULT_H_NAME;

  [JsonIgnore]
  public ECCurve ECCurve { get; set; } = new ECCurve();
  [JsonIgnore]
  public EcCurveParameters EcCurveParameters { get; set; } = EcCurveParameters.Empty;


  public EcSchnorrParameters(HashAlgorithmName hname = default)
  {
    HashAlgorithmName = hname == default ? DEFAULT_H_NAME : hname;
  }

  public EcSchnorrParameters(ECCurve curve, HashAlgorithmName hname = default)
  {
    this.ECCurve = EcSchnorrServices.Copy(curve);
    this.EcCurveParameters = new EcCurveParameters(curve);
    this.PublicKey = EcPointEmpty;
    this.PrivateKey = [];
    HashAlgorithmName = hname == default ? DEFAULT_H_NAME : hname;
  }

  public EcSchnorrParameters(
    EcSchnorrParameters ec_schnorr_param)
  {
    this.ECCurve = EcSchnorrServices.Copy(ec_schnorr_param.ECCurve);
    this.EcCurveParameters = ec_schnorr_param.EcCurveParameters.Copy();

    this.PublicKey = IsEcPointEmpty(ec_schnorr_param.PublicKey) ? EcPointEmpty : EcSchnorrServices.Copy(ec_schnorr_param.PublicKey);
    this.PrivateKey = IsNullOrEmpty(ec_schnorr_param.PrivateKey) ? [] : [.. ec_schnorr_param.PrivateKey];
    HashAlgorithmName = ec_schnorr_param.HashAlgorithmName == default ? DEFAULT_H_NAME : ec_schnorr_param.HashAlgorithmName;
  }

  public EcSchnorrParameters(
    ECCurve curve, ReadOnlySpan<byte> priv_key,
    ECPoint pub_key, HashAlgorithmName hname = default)
  {
    this.ECCurve = EcSchnorrServices.Copy(curve);
    this.PublicKey = EcSchnorrServices.Copy(pub_key);
    this.PrivateKey = priv_key.ToArray();
    this.EcCurveParameters = new EcCurveParameters(curve);

    HashAlgorithmName = hname == default ? DEFAULT_H_NAME : hname;
  }


  public readonly bool IsEmpty
  {
    get
    {
      return IsEcCurveEmpty(this.ECCurve) && EcCurveParameters.IsEmpty
      && IsEcPointEmpty(this.PublicKey) && IsNullOrEmpty(this.PrivateKey);
    }
  }

  public void Reset()
  {
    this.ECCurve = new ECCurve();
    this.EcCurveParameters.Reset();

    ResetKeyPair();

    this.HashAlgorithmName = DEFAULT_H_NAME;
  }

  public void ResetKeyPair()
  {
    ClearBytes(this.PublicKey.X!, this.PublicKey.Y!);
    ClearBytes(this.PrivateKey);
    this.PublicKey = EcPointEmpty;
    this.PrivateKey = [];
  }

  public void GenerateParameters(
    bool without_keypair = true,
    HashAlgorithmName hname = default)
  {
    if (IsEcCurveEmpty(this.ECCurve))
      throw new ArgumentNullException(
        nameof(ECCurve), $"{nameof(ECCurve)} has failed!");

    var oid = this.ECCurve.Oid;
    this.Reset();

    this.ECCurve = ECCurve.CreateFromOid(oid);
    this.EcCurveParameters = new EcCurveParameters(this.ECCurve);
    this.HashAlgorithmName = hname == default ? DEFAULT_H_NAME : hname;

    if (!without_keypair) this.GenerateKeyPair();
  }

  public void GenerateKeyPair()
  {
    if (IsEcCurveEmpty(this.ECCurve))
      throw new ArgumentNullException(
        nameof(ECCurve), $"{nameof(ECCurve)} has failed!");

    if (this.EcCurveParameters.IsEmpty)
      this.EcCurveParameters = new EcCurveParameters(this.ECCurve);

    this.ResetKeyPair();

    var priv = BigInteger.Zero;
    var size = this.EcCurveParameters.Length / 8;
    var n = ToBILE(this.EcCurveParameters.N);
    while (priv.IsZero)
      priv = RngBigIntegerBSize(size) % n;

    var pub = ECMultiply(priv, this.EcCurveParameters.G, this.EcCurveParameters);
    this.PublicKey = EcSchnorrServices.Copy(pub);
    this.PrivateKey = FromBILE(priv);
  }

  public readonly EcSchnorrParameters Copy()
  {
    return new EcSchnorrParameters(this);
  }

  public static EcSchnorrParameters[] RngEcSchnorrParameters(
    ECCurve curve, int size, HashAlgorithmName hname = default)
  {
    var result = new EcSchnorrParameters[size];
    for (int i = 0; i < result.Length; i++)
    {
      result[i] = new EcSchnorrParameters(curve, hname);
      result[i].GenerateKeyPair();
    }
    return result;
  }


  public static bool IsEcPointEmpty(ECPoint other) =>
    IsEquality(other, EcPointEmpty);

  private static ECPoint EcPointEmpty =>
    new()
    {
      X = [],
      Y = []
    };
}
