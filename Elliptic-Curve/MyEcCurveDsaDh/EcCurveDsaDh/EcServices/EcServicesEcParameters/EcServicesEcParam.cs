

using System.Security.Cryptography;

namespace michele.natale.EcCurveDsaDh;

partial class EcService
{
  public static ECParameters Copy(ECParameters ecparam, bool _explicit = false)
  {
    return new ECParameters
    {
      Q = new ECPoint
      {
        X = [.. ecparam.Q.X!],
        Y = [.. ecparam.Q.Y!],
      },
      Curve = CopyOrEmpty(ecparam.Curve, _explicit),
    };
  }

  public static ECCurve CopyOrEmpty(ECCurve ecurve, bool _ecplizit = false)
  {
    if (_ecplizit || ecurve.Oid is null) return new ECCurve();
    return ECCurve.CreateFromOid(ecurve.Oid);
  }

  public static ECPoint ToPublicKeyEcPoint(ECParameters ecpubkey) =>
        new()
        {
          X = [.. ecpubkey.Q.X],
          Y = [.. ecpubkey.Q.Y],
        };



}
