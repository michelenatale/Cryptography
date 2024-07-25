

using System.Numerics;
using System.Security.Cryptography;


namespace michele.natale.Schnorrs.EcServices;


partial class EcSchnorrServices
{
  public static ECPoint EcInfinity => new();
  public static bool IsEcInfinity(ECPoint point) => IsEquality(point, EcInfinity);

  public static ECPoint ECMultiply(BigInteger scalar, ECPoint point, EcCurveParameters param)
  {
    var n = ToBILE(param.N);
    if (IsEcInfinity(point) || scalar % n == 0)
      return EcInfinity;

    EcPointCheck(point, param);

    if (scalar < 0)
      return ECMultiply(-scalar, ECNegate(point, param), param);

    var addend = point;
    var result = EcInfinity;

    while (!scalar.IsZero)
    {
      if (!scalar.IsEven)
        result = ECAddition(result, addend, param);

      addend = ECAddition(addend, addend, param);

      scalar >>= 1;
    }

    EcPointCheck(result, param);

    return result;
  }

  public static bool IsOnEcCurve(
    ECPoint point, EcCurveParameters param)
  {
    BigInteger x = ToBILE(point.X!), y = ToBILE(point.Y!),
    a = ToBILE(param.A!), b = ToBILE(param.B!), p = ToBILE(param.P!);
    return IsEcInfinity(point) || ((BigInteger.Pow(y, 2) - BigInteger.Pow(x, 3) - a * x - b) % p) == 0;
  }

  public static ECPoint ECAddition(
    ECPoint left, ECPoint right,
    EcCurveParameters param)
  {
    if (IsEcInfinity(left))
      return right;

    if (IsEcInfinity(right))
      return left;

    EcPointCheck(left, param);
    EcPointCheck(right, param);

    BigInteger tmp, lx = ToBILE(left.X!), ly = ToBILE(left.Y!),
    rx = ToBILE(right.X!), ry = ToBILE(right.Y!), a = ToBILE(param.A),
    p = ToBILE(param.P);
    if (lx == rx)
    {
      if (ly != ry) return EcInfinity;
      tmp = (3 * BigInteger.Pow(lx, 2) + a) * ModularInverse(2 * ly, p);
    }
    else tmp = (ly - ry) * ModularInverse(lx - rx, p);

    var xx = BigInteger.Pow(tmp, 2) - lx - rx;
    var yy = ly + tmp * (xx - lx);
    var result = new ECPoint()
    {
      X = FromBILE(ModuloExt(xx, p)),
      Y = FromBILE(ModuloExt(-yy, p))
    };

    return result;
  }

  public static ECPoint ECAddition(ECPoint[] points, EcCurveParameters ecparams)
  {
    if ((points.Length <= 2) /*|| (points.Length != ecparams.Length)*/)
      throw new ArgumentException("Minimum number of points is 2.");

    var result = EcInfinity;
    for (int i = 1; i < points.Length; i++)
      result = ECAddition(points[i - 1], points[i], ecparams);

    return result;
  }

  public static ECPoint ECDouble(ECPoint point, EcCurveParameters param)
  {
    return ECAddition(point, point, param);
  }

  public static ECPoint ECNegate(ECPoint point, EcCurveParameters param)
  {
    EcPointCheck(point);

    if (IsEcInfinity(point))
      return point;

    var y = ToBILE(point.Y!);
    var p = ToBILE(param.P!);
    var yy = ModuloExt(-y, p);
    ECPoint result = new ECPoint() { X = point.X, Y = FromBILE(yy) };

    EcPointCheck(result);

    return result;
  }

  public static void EcPointCheck(ECPoint point) => EcPointCheck(point);

  public static void EcPointCheck(ECPoint point, EcCurveParameters param)
  {
    if (!IsOnEcCurve(point, param))
      throw new ArgumentException(
        $"{nameof(point)} is not on the Elliptic Curve",
          nameof(point));
  }
}
