

using System.Numerics;
using System.Security.Cryptography;

namespace michele.natale.Schnorrs;

using static michele.natale.Schnorrs.EcServices.EcSchnorrServices;

public class EcSchnorrInfos
{
  public byte[] Message { get; set; } = [];
  public List<EcSchnorrParameters> Parameters { get; set; } = [];


  private EcSchnorrInfos()
  {
  }

  public EcSchnorrInfos(
      ReadOnlySpan<byte> msg, params EcSchnorrParameters[] ecsparams)
  {
    this.Message = msg.ToArray();
    this.Parameters = ecsparams.Select(x => x.Copy()).ToList();
  }

  public EcSchnorrSignature MultiSign()
  {
    AssertMultiSign();

    var k = BigInteger.Zero;
    var param = this.Parameters.First().EcCurveParameters;
    var hname = this.Parameters.First().HashAlgorithmName;
    var g = Copy(this.Parameters.First().EcCurveParameters.G);
    var n = ToBILE(this.Parameters.First().EcCurveParameters.N);
    int size = this.Parameters.First().EcCurveParameters.Length / 8;

    while (k.IsZero)
      k = RngBigIntegerBSize(size) % n;

    var s1 = ECMultiply(k, g, param);

    hname = hname == default ? DEFAULT_H_NAME : hname;
    var concat = Message.ToArray().Concat(s1.X!).Concat(s1.Y!);
    var hash = new BigInteger(HashDataAlgo(concat.ToArray(), hname), true);
    
    //The order of the PrivateKeys does not matter.
    var sum_product = this.Parameters.Aggregate(BigInteger.Zero, (x, y) => ToBILE(y.PrivateKey) * hash + x);

    var s2 = (k + sum_product) % n;
    return new EcSchnorrSignature(s1, s2.ToByteArray());
  }

  public bool MultiVerify(EcSchnorrSignature signature)
  {
    AssertMultiVerify(signature);

    var (s1, ss) = signature.ToParameters;

    var s2 = new BigInteger(ss, true);
    var param = this.Parameters.First().EcCurveParameters;
    var hname = this.Parameters.First().HashAlgorithmName;
    var g = Copy(this.Parameters.First().EcCurveParameters.G);
    var n = ToBILE(this.Parameters.First().EcCurveParameters.N);

    if (this.Parameters.Where(x => IsEcInfinity(x.PublicKey)).Any()) return false;
    if (this.Parameters.Where(x => !IsOnEcCurve(x.PublicKey, param)).Any()) return false;
    if (this.Parameters.Where(x => !IsEcInfinity(ECMultiply(n, x.PublicKey, param))).Any()) return false;

    var concat = Message.ToArray().Concat(s1.X!).Concat(s1.Y!);
    var hash = new BigInteger(HashDataAlgo(concat.ToArray(), hname), true);
    var current = this.Parameters.Aggregate(s1,
        (seed, ecp) => ECAddition(seed, ECMultiply(hash, ecp.PublicKey, ecp.EcCurveParameters), ecp.EcCurveParameters));

    var expected = ECMultiply(s2, g, param);
    return current.X!.SequenceEqual(expected.X!)
           && current.Y!.SequenceEqual(expected.Y!);
  }

  private void AssertMultiSign()
  {

    if (IsNullOrEmpty(this.Message))
      throw new ArgumentException($"{nameof(Message)} has failed!");

    var curve = this.Parameters.First().ECCurve;
    var hname = this.Parameters.First().HashAlgorithmName;

    if (IsEcCurveEmpty(curve))
      throw new ArgumentException($"{nameof(this.Parameters)} has failed!");

    if (this.Parameters.Where(x => IsEquality(curve, x.ECCurve)).Count() != this.Parameters.Count)
      throw new ArgumentException($"{nameof(Parameters)} Not all curves are equals!");

    if (this.Parameters.Where(x => x.HashAlgorithmName != hname).Any())
      throw new ArgumentException($"{nameof(Parameters)} Some HashAlgorithmName has failed!");

    if (this.Parameters.Where(x => IsNullOrEmptyOrZero(x.PrivateKey)).Any())
      throw new ArgumentException($"{nameof(Parameters)} Some PrivateKeys has failed!");
  }

  private void AssertMultiVerify(EcSchnorrSignature signature)
  {
    if (IsNullOrEmpty(this.Message))
      throw new ArgumentException($"{nameof(Message)} has failed!");

    var curve = this.Parameters.First().ECCurve;
    var hname = this.Parameters.First().HashAlgorithmName;

    if (IsEcCurveEmpty(curve))
      throw new ArgumentException($"{nameof(this.Parameters)} has failed!");

    if (this.Parameters.Where(x => IsEquality(curve, x.ECCurve)).Count() != this.Parameters.Count)
      throw new ArgumentException($"{nameof(Parameters)} Not all curves are equals!");

    if (this.Parameters.Where(x => IsEmpty(x.PublicKey)).Any())
      throw new ArgumentException($"{nameof(Parameters)} Some PrivateKeys has failed!");

    if (this.Parameters.Where(x => x.HashAlgorithmName != hname).Any())
      throw new ArgumentException($"{nameof(Parameters)} Some HashAlgorithmName has failed!");

    if (IsEmpty(signature.S1) || IsNullOrEmpty(signature.S2))
      throw new ArgumentException($"{nameof(signature)} has failed!");
  }

  private static bool IsEmpty(ECPoint ecp)
  {
    return IsNullOrEmptyOrZero(ecp.X) && IsNullOrEmptyOrZero(ecp.Y);
  }
}
