
using System.Numerics;
using System.Security.Cryptography;

namespace michele.natale.Schnorrs;

using static michele.natale.Schnorrs.EcServices.EcSchnorrServices;


public struct EcSchnorrSignature
{

  public byte[] S2 = [];
  public ECPoint S1 = new() { X = [], Y = [] };



  public EcSchnorrSignature()
  {
  }

  public EcSchnorrSignature(EcSchnorrSignature sign)
  {
    this.S2 = [.. sign.S2];
    this.S1 = Copy(sign.S1);
  }

  public EcSchnorrSignature(ECPoint s1, byte[] s2)
    : this(((s1.X!, s1.Y!), s2))
  {
  }

  public EcSchnorrSignature(byte[] s1x, byte[] s1y, byte[] s2)
    : this(((s1x, s1y), s2))
  {
  }

  public EcSchnorrSignature(((byte[] X, byte[] Y) s1, byte[] s2) sign)
  {
    var (s1, s2) = sign;
    this.S2 = [.. s2];
    this.S1 = new ECPoint() { X = [.. s1.X], Y = [.. s1.Y] };
  }

  public EcSchnorrSignature((BigInteger X, BigInteger Y) s1, BigInteger s2)
  {
    var bytes = ToBytes(s1, s2);
    this.S2 = bytes.S2;
    this.S1 = new ECPoint() { X = bytes.S1.X, Y = bytes.S1.Y };
  }

  public readonly (ECPoint S1, byte[] S2) ToParameters =>
   (Copy(this.S1), this.S2.ToArray());

  public readonly ((BigInteger X, BigInteger Y) S1, BigInteger S2) ToParametersBigIntegers =>
    ((new BigInteger(this.S1.X!), new BigInteger(this.S1.Y!)), new BigInteger(this.S2));

  public readonly bool Validation()
  {
    if (!this.CheckAllParam()) return false;
    return true;
  }

  public void Reset()
  {
    ClearBytes(this.S1.X!, this.S1.Y!, this.S2);
    this.S1 = new ECPoint() { X = [], Y = [] };
  }

  private readonly bool CheckAllParam()
  {
    if (IsNullOrEmpty(this.S1.X)) return false;
    if (IsNullOrEmpty(this.S1.Y)) return false;
    if (IsNullOrEmpty(this.S2)) return false;
    return true;
  }

  public static ((byte[] X, byte[] Y) S1, byte[] S2) ToBytes(
    (BigInteger X, BigInteger Y) s1, BigInteger s2)
  {
    return ((s1.X.ToByteArray(), s1.Y.ToByteArray()), s2.ToByteArray());
  }

  public readonly ((string X, string Y) S1, string S2) ToHex(bool tolower = true)
  {
    var hex = ToHexStr(tolower, this.S1.X!, this.S1.Y!, this.S2);
    return ((hex[0], hex[1]), hex[2]);
  }

  public static EcSchnorrSignature FromHex((string x, string y) s1, string s2)
  {
    return new EcSchnorrSignature(FromHexToBytes(s1, s2));
  }
  public static ((byte[] X, byte[] Y) S1, byte[] S2) FromHexToBytes((string x, string y) s1, string s2)
  {
    var tmp = FromHexStr(s1.x, s1.y, s2);
    return ((tmp[0], tmp[1]), tmp[2]);
  }

  public static ((BigInteger X, BigInteger Y) S1, BigInteger S2) FromHexToBigInteger((string x, string y) s1, string s2)
  {
    return new EcSchnorrSignature(FromHex(s1, s2)).ToParametersBigIntegers;
  }

}
