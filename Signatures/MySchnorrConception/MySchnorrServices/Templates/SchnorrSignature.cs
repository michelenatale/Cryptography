
using System.Numerics;
 


namespace michele.natale.Schnorrs;


using static michele.natale.Schnorrs.Services.SchnorrServices;


public struct SchnorrSignature
{
  public byte[] S1 = [];
  public byte[] S2 = [];

  public SchnorrSignature()
  {
  }

  public SchnorrSignature(SchnorrSignature sign)
  {
    this.S1 = sign.S1; this.S2 = sign.S2;
  }

  public SchnorrSignature(byte[] s1, byte[] s2)
    : this((s1, s2))
  {
  }

  public SchnorrSignature((byte[] s1, byte[] s2) sign)
  {
    var (s1, s2) = sign;
    this.S1 = s1; this.S2 = s2;
  }

  public SchnorrSignature(BigInteger s1, BigInteger s2)
    : this(ToBytes(s1, s2))
  {
  }

  public readonly (byte[] S1, byte[] S2) ToParameters =>
    (this.S1, this.S2);

  public readonly (BigInteger S1, BigInteger S2) ToParametersBigIntegers =>
    (new BigInteger(this.S1), new BigInteger(this.S2));

  public readonly bool Validation()
  {
    if (!this.CheckAllParam()) return false;
    return true;
  }

  public readonly void Reset()
  {
    ClearBytes(this.S1, this.S2);
  }

  private readonly bool CheckAllParam()
  {
    if (IsNullOrEmpty(this.S1)) return false;
    if (IsNullOrEmpty(this.S2)) return false;
    return true;
  }

  public static (byte[] S1, byte[] S2) ToBytes(
    BigInteger s1, BigInteger s2)
  {
    return (s1.ToByteArray(), s2.ToByteArray());
  }

  public readonly (string S1, string S2) ToHex(bool tolower = true)
  {
    var hex = ToHexStr(tolower, this.S1, this.S2);
    return (hex[0], hex[1]);
  }

  public static SchnorrSignature FromHex(string s1, string s2)
  {
    var tmp = FromHexStr(s1, s2);
    var bytes = (tmp[0], tmp[1]);
    return new SchnorrSignature(bytes);
  }

  public static (byte[] S1, byte[] S2) FromHexToBytes(string s1, string s2)
  {
    var tmp = FromHexStr(s1, s2);
    return (tmp[0], tmp[1]);
  }

  public static (BigInteger S1, BigInteger S2) FromHexToBigInteger(string s1, string s2)
  {
    return new SchnorrSignature(FromHex(s1, s2)).ToParametersBigIntegers;
  }

  private static bool IsNullOrEmpty(byte[] bytes)
  {
    if (bytes is null) return true;
    return bytes.Length == 0;
  }

  private static void ClearBytes(params byte[][] bytes)
  {
    for (var i = 0; i < bytes.Length; i++)
      Array.Clear(bytes[i]);
  }
}
