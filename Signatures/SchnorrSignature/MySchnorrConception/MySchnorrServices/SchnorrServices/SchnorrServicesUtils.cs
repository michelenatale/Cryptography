
using System.Numerics;

namespace michele.natale.Schnorrs.Services;

partial class SchnorrServices
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

  public static bool IsNullOrEmpty<T>(T[] ints)
    where T : INumber<T>
  {
    if (ints is null) return true;
    return ints.Length == 0;
  }

  public static bool IsNullOrEmptyOrZero<T>(T[] ints)
   where T : INumber<T>
  {
    if (ints is null) return true;
    if (ints.Length == 0) return true;
    foreach (var i in ints)
      if(!T.IsZero(i)) return false; 
    return true;
  }

  public static BigInteger ModuloExt(BigInteger x, BigInteger m)
  {
    x %= m;
    return (x + m) % m;
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
}
