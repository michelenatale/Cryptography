
using System.Numerics;
using System.Security.Cryptography;

namespace michele.natale.Schnorrs.EcServices;

partial class EcSchnorrServices
{
  public static (ECPoint, byte[]) Sign(
    ReadOnlySpan<byte> message,
    EcSchnorrParameters ecsparam)
  {
    var k = BigInteger.Zero;
    var privkey = ecsparam.PrivateKey;
    var param = ecsparam.EcCurveParameters;
    var hname = ecsparam.HashAlgorithmName;

    var n = ToBILE(param.N);
    int size = param.Length / 8;

    while (k.IsZero)
      k = RngBigIntegerBSize(size) % n;

    var g = Copy(param.G);
    var s1 = ECMultiply(k, g, param);

    hname = hname == default ? DEFAULT_H_NAME : hname;

    var concat = message.ToArray().Concat(s1.X!).Concat(s1.Y!);
    var hash = new BigInteger(HashDataAlgo(concat.ToArray(), hname), true);
    var s2 = (k + ToBILE(privkey) * hash) % n;

    return (s1, s2.ToByteArray(true));
  }

  public static bool Verify(
    ReadOnlySpan<byte> message,
    (ECPoint s1, byte[] s2) signature,
    EcSchnorrParameters ecsparam)
  {
    ArgumentNullException.ThrowIfNull(ecsparam);

    var (s1, ss) = signature;
    var pubkey = ecsparam.PublicKey;
    var s2 = new BigInteger(ss, true);
    var hname = ecsparam.HashAlgorithmName;
    var param = ecsparam.EcCurveParameters;

    var n = ToBILE(param.N);
    if (IsEcInfinity(pubkey)) return false;
    if (!IsOnEcCurve(pubkey, param)) return false;
    if (!IsEcInfinity(ECMultiply(n, pubkey, param))) return false;

    var concat = message.ToArray().Concat(s1.X!).Concat(s1.Y!);
    var hash = new BigInteger(HashDataAlgo(concat.ToArray(), hname), true);
    var current = ECAddition(s1, ECMultiply(hash, pubkey, param), param);

    var g = Copy(param.G);
    var expected = ECMultiply(s2, g, param);
    return current.X!.SequenceEqual(expected.X!)
           && current.Y!.SequenceEqual(expected.Y!);
  }
}