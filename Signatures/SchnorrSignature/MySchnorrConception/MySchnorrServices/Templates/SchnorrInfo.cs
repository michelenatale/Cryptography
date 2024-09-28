
using System.Numerics;
using System.Security.Cryptography;

namespace michele.natale.Schnorrs;

using static michele.natale.Schnorrs.Services.SchnorrServices;

public class SchnorrInfo
{
  public byte[] Message { get; set; } = [];
  public SchnorrParameters Parameter { get; set; } = default;

  private SchnorrInfo()
  {
  }

  public SchnorrInfo(SchnorrInfo info)
  {
    this.Message = info.Message;
    this.Parameter = info.Parameter;
  }

  public SchnorrInfo(
    ReadOnlySpan<byte> msg, SchnorrParameters param)
  {
    this.Message = msg.ToArray();
    this.Parameter = param.Copy();
  }

  public SchnorrSignature Sign()
  {
    var privkey = new BigInteger(this.Parameter.PrivateKey);

    if (privkey.Sign < 1 || IsNullOrEmpty(this.Message))
      throw new ArgumentException($"{nameof(privkey)} has failed!");

    var (p, q, g, hn) = this.Parameter.PQG.ToBigIntegers();
    var qsz = (int)BigInteger.Log2(q) + 1;

    var r = RngBI(q, qsz);
    var x = BigInteger.ModPow(g, r, p);
    var mx = this.Message.Concat(x.ToByteArray()).ToArray();

    var s1 = HashData(mx, hn);
    var s2 = (r + s1 * privkey) % q;
    return new SchnorrSignature(s1, s2);
  }

  public bool Verify(SchnorrSignature signature)
  {
    var pubkey = new BigInteger(this.Parameter.PublicKey);
    var (s1, s2) = signature.ToParametersBigIntegers;
    var (p, _, g, hn) = this.Parameter.PQG.ToBigIntegers();

    var m = BigInteger.ModPow(g, s2, p);
    var n = BigInteger.ModPow(pubkey, s1, p);
    var x = (m * n) % p;

    var mx = this.Message.Concat(x.ToByteArray()).ToArray();
    var hash = HashData(mx, hn);

    return hash.ToByteArray().SequenceEqual(signature.S1);
  }


  private static BigInteger RngBI(BigInteger reference, int bits)
  {
    var result = BigInteger.Zero;
    var (min, max) = ToMinMax(bits);
    while (result.IsZero)
    {
      var k = RngBigInteger(min, max);
      result = ModuloExt(k, reference);
    }
    return result;
  }

  public static BigInteger HashData(
    BigInteger message, HashAlgorithmName hname = default) =>
      HashData(message.GetByteCount(), hname);

  public static BigInteger HashData(
    ReadOnlySpan<byte> message, HashAlgorithmName hname = default)
  {
    hname = hname == default ? HashAlgorithmName.SHA512 : hname;
    var hash = HashDataAlgo(message, hname);
    var result = new BigInteger(hash);
    if (result.Sign < 0) result = -result;
    return result;
  }

}
