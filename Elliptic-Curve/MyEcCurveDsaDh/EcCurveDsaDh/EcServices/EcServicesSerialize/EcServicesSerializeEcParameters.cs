

using System.Security.Cryptography;

namespace michele.natale.EcCurveDsaDh;

[Serializable]
public class EcParametersInfo
{
  public ECParameters ToEcParameters
  {
    get
    {
      return this.ToEcParam();
    }
  }

  public byte EcSize = 0;
  public byte[] D { get; set; } = [];
  public byte[] Q { get; set; } = [];
  public byte[] C { get; set; } = [];

  public EcParametersInfo(ECParameters ecparam, bool _explicit = false)
  {
    var (d, q, c, s) = ToBytes(ecparam, _explicit);
    this.D = d; this.Q = q; this.C = c; this.EcSize = s;
  }

  public EcParametersInfo(byte[] d, ECPoint q, ECCurve c)
  {
    var ecp = new ECParameters { D = d, Q = q, Curve = c, };
    var (dd, qq, cc, ss) = ToBytes(ecp);
    this.D = dd; this.Q = qq; this.C = cc; this.EcSize = ss;
  }

  public EcParametersInfo(byte[] ec_serialize)
  {
    var tmp = EcService.DeserializeJson<byte[][]>(ec_serialize);
    this.D = tmp![0]; this.Q = tmp[1];
    this.C = tmp[2]; this.EcSize = tmp[3].First();
  } 
  
  public byte[] SerializeEcParam()
  {
    return EcService.SerializeJson<byte[][]>([this.D, this.Q, this.C, [this.EcSize]]);
  }

  private ECParameters ToEcParam()
  {
    return new ECParameters
    {
      D = this.D,
      Q = ToPupKeyEcPoint(this.Q, [this.EcSize, this.EcSize]),
      Curve = this.C.Length > 0 ? EcService.DeserializeEcCurve(this.C) : new ECCurve()
    };
  }

  [Obsolete("'SerializeEcParameters' is deprecated, please use Class 'EcParametersInfo' instead.")]
  public static byte[] SerializeEcParameters(ECParameters ecparam)
  {
    var ld = ecparam.D is null ? 0 : ecparam.D.Length; //PrivarKey Length
    var lqx = ecparam.Q.X is null ? 0 : ecparam.Q.X.Length; //PublicKey I Length
    var lqy = ecparam.Q.Y is null ? 0 : ecparam.Q.Y.Length; //PublicKey II Length
    var lcurve = ecparam.Curve.Oid is null ? 0 : int.MaxValue; //Elliptic Curve Length
    var lqxy = lqx + lqy + 1; //PublicKey Length + Prefix Length

    byte[] ec, priv, pup;
    ec = priv = pup = [];

    if (ld > 0) priv = ecparam.D!;
    if (lqxy > 1) pup = ToPuplicKeyConcat(ecparam.Q);
    if (lcurve > 0) ec = EcService.SerializeEcCurve(ecparam.Curve);
    lcurve = ec.Length;

    var length = lcurve + lqxy;
    if (ld > 0) length += ld;
    var result = new byte[3 + length];
    result[0] = ecparam.D == null ? (byte)0 : (byte)ld;
    result[1] = ecparam.Q.X == null ? (byte)0 : (byte)lqx;
    result[2] = ecparam.Q.Y == null ? (byte)0 : (byte)lqy;

    Array.Copy(ec, 0, result, 3, lcurve);
    Array.Copy(pup, 0, result, 3 + lcurve, lqxy);
    if (ld > 0)
      Array.Copy(priv, 0, result, 3 + lcurve + lqxy, ld);

    return result;
  }

  [Obsolete("'DeserializeEcParameters' is deprecated, please use Class 'EcParametersInfo' instead.")]
  public static ECParameters DeserializeEcParameters(byte[] bytes)
  {
    int ld = bytes[0], lqx = bytes[1], lqy = bytes[2];
    var lcurve = bytes.Length - ld - lqx - lqy - 1 - 3;
    var lqxy = lqx + lqy + 1;

    //byte[] ec, priv, pup;
    var ec = lcurve > 0 ? new byte[lcurve] : [];
    var priv = ld > 0 ? new byte[ld] : [];
    var pup = new byte[lqxy];

    Array.Copy(bytes, 3, ec, 0, lcurve);  //Elliptic Curve 
    Array.Copy(bytes, lcurve + 3, pup, 0, lqxy); //PublicKey I + II + Predicat
    Array.Copy(bytes, lcurve + lqxy + 3, priv, 0, ld); //Privkey

    return new ECParameters()
    {
      Curve = lcurve > 0 ? EcService.DeserializeEcCurve(ec) : new ECCurve(),
      D = priv,
      Q = ToPupKeyEcPoint(pup, [lqx, lqy]),
    };
  }

  private static ECPoint ToPupKeyEcPoint(
    byte[] pupkey, int[] length, byte prefix = 0x04)
  {
    if (IsNullOrEmpty(pupkey) || pupkey.Length == 1)
      return new ECPoint();

    if (prefix == pupkey.First())
    {
      return new ECPoint()
      {
        X = pupkey.Skip(1).Take(length[0]).ToArray(),
        Y = pupkey.Skip(1 + length[0]).ToArray(),
      };
    }
    throw new ArgumentException($"{nameof(pupkey)} format is false");
  }

  public static byte[] ToPuplicKeyConcat(ECPoint q, byte prefix = 0x04)
  {
    var lqx = IsNullOrEmpty(q.X) ? 0 : q.X!.Length;
    var lqy = IsNullOrEmpty(q.Y) ? 0 : q.Y!.Length;
    var result = new byte[1 + lqx + lqy];
    result[0] = prefix;

    Array.Copy(q.X!, 0, result, 1, lqx);
    Array.Copy(q.Y!, 0, result, lqx + 1, lqy);
    return result;
  }

  public static byte[] SerializeEcParam(ECParameters ecparam)
  {
    var instance = new EcParametersInfo(ecparam);
    return instance.SerializeEcParam();
  }

  public static ECParameters DeserializeEcParam(byte[] ec_serialize)
  {
    var instance = new EcParametersInfo(ec_serialize);
    return instance.ToEcParam();
  }

  private static bool CheckQ(ECPoint q)
  {
    if (IsNullOrEmpty(q.X!)) return false;
    if (IsNullOrEmpty(q.Y!)) return false;

    return true;
  }

  private static bool CheckOid(Oid oid)
  {
    // Both entries must have a value here.
    if (oid is null) return false;
    if (string.IsNullOrEmpty(oid.Value)) return false;
    if (string.IsNullOrEmpty(oid.FriendlyName)) return false;

    return true;
  }

  private static bool IsNullOrEmpty(byte[]? bytes)
  {
    if (bytes == null) return true;
    return bytes.Length == 0;
  }

  private static (byte[] D, byte[] Q, byte[] C, byte S) ToBytes(
    ECParameters ecparam, bool _explicit = false)
  {
    var s = 0;
    var c = Array.Empty<byte>();
    var d = IsNullOrEmpty(ecparam.D) ? [] : ecparam.D!;
    var q = !CheckQ(ecparam.Q!) ? [] : ToPuplicKeyConcat(ecparam.Q);
    if (!_explicit)
      c = !CheckOid(ecparam.Curve.Oid)
        ? [] : EcService.SerializeEcCurve(ecparam.Curve);
    if (!IsNullOrEmpty(ecparam.D)) { s = ecparam.D!.Length; return (d, q, c, (byte)s); }
    if (!IsNullOrEmpty(ecparam.Q.X)) { s = ecparam.Q.X!.Length; return (d, q, c, (byte)s); }
    if (!IsNullOrEmpty(ecparam.Q.Y)) { s = ecparam.Q.Y!.Length; return (d, q, c, (byte)s); }
    return (d, q, c, (byte)s);
  }


}
