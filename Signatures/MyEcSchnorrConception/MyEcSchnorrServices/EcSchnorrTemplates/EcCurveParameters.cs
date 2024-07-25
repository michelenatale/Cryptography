
using System.Numerics;
using System.Security.Cryptography;
using System.Text.Json.Serialization;

namespace michele.natale.Schnorrs;

using michele.natale.Schnorrs.EcServices;
using static michele.natale.Schnorrs.EcServices.EcSchnorrServices;

public class EcCurveParameters
{
  [JsonIgnore]
  public bool IsEmpty
  {
    get => IsEccpEmpty();
  }
  public byte[] P { get; private set; } = [];
  public byte[] A { get; private set; } = [];
  public byte[] B { get; private set; } = [];
  public byte[] N { get; private set; } = [];
  public byte[] H { get; private set; } = [];
  public int Length { get; private set; } = 0;
  public Oid Oid { get; private set; } = new Oid("", "");
  public ECPoint G { get; private set; } = new ECPoint() { X = [], Y = [] };

  public ECCurve ECCurve { get; private set; } = default;

  private EcCurveParameters()
  {
  }

  public EcCurveParameters(ECCurve curve)
  {
    var ecc = ECDsa.Create(curve);
    var ecparam = ecc.ExportExplicitParameters(true);

    this.ECCurve = curve;
    this.Length = ecc.KeySize;
    this.Oid = curve.Oid is null ? new Oid("", "") : Copy(curve.Oid);
    this.Oid.Value = string.IsNullOrEmpty(this.Oid.Value) ? string.Empty : this.Oid.Value;
    this.Oid.FriendlyName = string.IsNullOrEmpty(this.Oid.FriendlyName) ? string.Empty : this.Oid.FriendlyName;

    this.A = IsNullOrEmpty(ecparam.Curve.A) ? [] : [.. ecparam.Curve.A!];
    this.B = IsNullOrEmpty(ecparam.Curve.B) ? [] : [.. ecparam.Curve.B!];
    this.N = IsNullOrEmpty(ecparam.Curve.Order) ? [] : [.. ecparam.Curve.Order!];
    this.P = IsNullOrEmpty(ecparam.Curve.Prime) ? [] : [.. ecparam.Curve.Prime!];
    this.H = IsNullOrEmpty(ecparam.Curve.Cofactor) ? [] : [.. ecparam.Curve.Cofactor!];

    var gx = IsNullOrEmpty(ecparam.Curve.G.X) ? [] : ecparam.Curve.G.X!.ToArray();
    var gy = IsNullOrEmpty(ecparam.Curve.G.Y) ? [] : ecparam.Curve.G.Y!.ToArray();
    this.G = new ECPoint() { X = gx, Y = gy };
  }


  public EcCurveParameters(EcCurveParameters eccparam)
  {
    this.Length = eccparam.Length;
    this.ECCurve = eccparam.ECCurve;
    this.Oid = eccparam.Oid is null ? new Oid("", "") : new Oid(eccparam.Oid.Value, eccparam.Oid.FriendlyName);
    this.Oid.Value = string.IsNullOrEmpty(this.Oid.Value) ? string.Empty : this.Oid.Value;
    this.Oid.FriendlyName = string.IsNullOrEmpty(this.Oid.FriendlyName) ? string.Empty : this.Oid.FriendlyName;

    this.A = IsNullOrEmpty(eccparam.A) ? [] : [.. eccparam.A!];
    this.B = IsNullOrEmpty(eccparam.B) ? [] : [.. eccparam.B!];
    this.N = IsNullOrEmpty(eccparam.N) ? [] : [.. eccparam.N!];
    this.P = IsNullOrEmpty(eccparam.P) ? [] : [.. eccparam.P!];
    this.H = IsNullOrEmpty(eccparam.H) ? [] : [.. eccparam.H!];

    var gx = IsNullOrEmpty(eccparam.G.X) ? [] : eccparam.G.X!.ToArray();
    var gy = IsNullOrEmpty(eccparam.G.Y) ? [] : eccparam.G.Y!.ToArray();
    this.G = new ECPoint() { X = gx, Y = gy };
  }

  public EcCurveParameters Copy()
  {
    return new EcCurveParameters(this);
  }

  public void Reset()
  {
    this.Length = 0;
    this.ECCurve = default;
    this.Oid = new Oid("", "");

    this.A = this.B =
    this.N = this.P =
    this.H = [];

    this.G = new ECPoint() { X = [], Y = [] };
  }

  public (int Length, Oid Oid, byte[] A, byte[] B, byte[] N, byte[] P, byte[] H, ECPoint G) ToParameters()
  {
    //Curve wird nicht zurückgegeben, sondern nur Oid
    return (this.Length, this.Oid, this.A, this.B, this.N, this.P, this.H, this.G);
  }

  public (int Length, Oid Oid, BigInteger A, BigInteger B, BigInteger N, BigInteger P, BigInteger H, (BigInteger X, BigInteger Y) G) ToParametersBigInteger()
  {
    //Curve wird nicht zurückgegeben, sondern nur Oid
    return (this.Length, this.Oid, ToBILE(this.A),
            ToBILE(this.B), ToBILE(this.N),
            ToBILE(this.P), ToBILE(this.H),
            (ToBILE(this.G.X!), ToBILE(this.G.Y!)));
  }

  public bool IsEquality(EcCurveParameters other)
  {
    //Der Check muss nur über die Kurven gemacht werden.
    if (!EcSchnorrServices.IsEquality(this.ECCurve, other.ECCurve)) return false;
    return true;
  }

  private bool IsEccpEmpty()
  {
    if (!IsEcCurveEmpty(this.ECCurve)) return false;
    if (this.Length != 0) return false;
    if (this.Oid is not null)
    {
      if (!string.IsNullOrEmpty(this.Oid.Value)) return false;
      if (!string.IsNullOrEmpty(this.Oid.FriendlyName)) return false;
    }
    if (!IsNullOrEmptyOrZero(this.A)) return false;
    if (!IsNullOrEmptyOrZero(this.B)) return false;
    if (!IsNullOrEmptyOrZero(this.N)) return false;
    if (!IsNullOrEmptyOrZero(this.P)) return false;
    if (!IsNullOrEmptyOrZero(this.H)) return false;

    if (!IsNullOrEmptyOrZero(this.G.X)) return false;
    if (!IsNullOrEmptyOrZero(this.G.Y)) return false;

    return true;
  }



  //Static


  public static EcCurveParameters Empty
  {
    get
    {
      return new EcCurveParameters();
    }
  }

  private static Oid Copy(Oid oid) => new() { Value = oid.Value, FriendlyName = oid.FriendlyName };
}
