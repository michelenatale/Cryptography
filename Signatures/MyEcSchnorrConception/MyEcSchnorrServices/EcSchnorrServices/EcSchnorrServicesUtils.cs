
using System.Numerics;
using System.Security.Cryptography;

namespace michele.natale.Schnorrs.EcServices;

partial class EcSchnorrServices
{
  public static void ClearBytes(params byte[][] bytes)
  {
    for (var i = 0; i < bytes.Length; i++)
      if (bytes[i] is not null)
        Array.Clear(bytes[i]);
      else bytes[i] = [];
  }

  public static byte[][] ToBytes(params BigInteger[] bignumbers)
  {
    var result = new byte[bignumbers.Length][];
    for (var i = 0; i < bignumbers.Length; i++)
      result[i] = bignumbers[i].ToByteArray();
    return result;
  }

  public static bool IsNullOrEmpty<T>(params T[][] ints)
    where T : INumber<T>
  {
    for (var i = 0; i < ints.Length; i++)
      if (!IsNullOrEmpty(ints[i]))
        return false;
    return true;
  }

  public static bool IsNullOrEmpty<T>(T[]? ints)
    where T : INumber<T>
  {
    if (ints is null) return true;
    return ints.Length == 0;
  }

  public static bool IsNullOrEmptyOrZero<T>(T[]? ints)
   where T : INumber<T>
  {
    if (ints is null) return true;
    if (ints.Length == 0) return true;
    foreach (var i in ints)
      if (!T.IsZero(i)) return false;
    return true;
  }

  public static byte[][] FromHexStr(params string[] values)
  {
    var result = new byte[values.Length][];
    for (var i = 0; i < values.Length; i++)
      result[i] = FromHexStr(values[i]);
    return result;
  }

  public static byte[] FromHexStr(string value) =>
    Convert.FromHexString(value);

  public static string[] ToHexStr(bool tolowers, params byte[][] bytes)
  {
    var result = new string[bytes.Length];
    for (var i = 0; i < bytes.Length; i++)
      if (tolowers)
        result[i] = Convert.ToHexString(bytes[i]).ToLower();
      else result[i] = Convert.ToHexString(bytes[i]).ToUpper();
    return result;
  }

  public static bool IsEcCurveEmpty(ECCurve curve)
  {
    if (curve.Oid is null) return true;
    return string.IsNullOrEmpty(curve.Oid.Value)
            && string.IsNullOrEmpty(curve.Oid.FriendlyName);
  }

  public static ECPoint Copy(ECPoint ecpoint) =>
    new()
    {
      X = [.. ecpoint.X!],
      Y = [.. ecpoint.Y!],
    };

    public static ECCurve Copy(ECCurve curve)
  {
    if (curve.Oid is not null)
    {
      if (!string.IsNullOrEmpty(curve.Oid.Value))
        return ECCurve.CreateFromValue(curve.Oid.Value);
      if (!string.IsNullOrEmpty(curve.Oid.FriendlyName))
        return ECCurve.CreateFromFriendlyName(curve.Oid.FriendlyName);
    }
    return new ECCurve();
  }

  public static bool IsEquality(ECCurve left, ECCurve right)
  {
    var loid = left.Oid;
    var roid = right.Oid;
    return IsEquality(loid, roid);
  }

  public static bool IsEquality(Oid left, Oid right)
  {
    if (left is null && right is null) return true;
    if (left is null) return false;
    if (right is null) return false;
    if (left.Value != right.Value) return false;
    return left.FriendlyName == right.FriendlyName;
  }

  public static bool IsEquality(
    EcCurveParameters left, EcCurveParameters right)
  {
    return left.IsEquality(right);
  }

  public static bool IsEquality(ECPoint left, ECPoint right)
  {
    byte[] lx = left.X!, ly = left.Y!;
    byte[] rx = right.Y!, ry = right.Y!;

    if (IsNullOrEmpty(lx) && IsNullOrEmpty(rx)) return true;
    if (IsNullOrEmpty(ly) && IsNullOrEmpty(ry)) return true;
    if (IsNullOrEmpty(lx) || IsNullOrEmpty(rx)) return false;
    if (IsNullOrEmpty(ly) || IsNullOrEmpty(ry)) return false;
    if (!lx.SequenceEqual(rx)) return false;
    return ly.SequenceEqual(rx);
  }
}