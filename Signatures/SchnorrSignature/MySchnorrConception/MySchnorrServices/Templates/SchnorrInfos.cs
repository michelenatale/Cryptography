using System.Numerics;
using System.Security.Cryptography;

namespace michele.natale.Schnorrs;

using static michele.natale.Schnorrs.Services.SchnorrServices;

public class SchnorrInfos
{
  public byte[] Message { get; set; } = [];
  public List<SchnorrParameters> Parameters { get; set; } = [];


  private SchnorrInfos()
  {
  }

  public SchnorrInfos(
      ReadOnlySpan<byte> msg, params SchnorrParameters[] param)
  {
    this.Message = msg.ToArray();
    this.Parameters = param.Select(x => x.Copy()).ToList();
  }

  public SchnorrSignature MultiSign()
  {
    AssertMultiSign();

    var (p, q, g, hn) = this.Parameters.First().PQG.ToBigIntegers();
    var qsz = (int)BigInteger.Log2(q) + 1;

    var r = RngBI(q, qsz);
    var x = BigInteger.ModPow(g, r, p);
    var mx = this.Message.Concat(x.ToByteArray()).ToArray();

    var s1 = HashData(mx, hn);
    var summul = SumProduct(this.Parameters, s1);

    var s2 = (r + summul) % q;
    return new SchnorrSignature(s1, s2);
  }

  public bool MultiVerify(SchnorrSignature signature)
  {
    AssertMultiVerify(signature);

    var (s1, s2) = signature.ToParametersBigIntegers;
    var (p, _, g, hn) = this.Parameters.First().PQG.ToBigIntegers();
    var m = BigInteger.ModPow(g, s2, p);
    var n = ModPowProduct(this.Parameters, s1, p);
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


  private void AssertMultiSign()
  {
    if (IsNullOrEmpty(this.Message))
      throw new ArgumentException($"{nameof(Message)} has failed!");

    var g = this.Parameters.First().PQG;

    if (g.IsEmpty)
      throw new ArgumentException($"group is Empty!");

    if (this.Parameters.Where(x => x.PQG.Equality(g)).Count() != this.Parameters.Count)
      throw new ArgumentException($"{nameof(Parameters)} not all group are equals!");

    if (this.Parameters.Where(x => IsNullOrEmptyOrZero(x.PrivateKey)).Any())
      throw new ArgumentException($"{nameof(Parameters)} Some PrivateKeys has failed!");
  }

  private void AssertMultiVerify(SchnorrSignature signature)
  {
    if (IsNullOrEmpty(this.Message))
      throw new ArgumentException($"{nameof(Message)} has failed!");

    var g = this.Parameters.First().PQG;

    if (g.IsEmpty)
      throw new ArgumentException($"group is Empty!");

    if (this.Parameters.Where(x => x.PQG.Equality(g)).Count() != this.Parameters.Count)
      throw new ArgumentException($"{nameof(Parameters)} not all group are equals!");

    if (this.Parameters.Where(x => IsNullOrEmptyOrZero(x.PublicKey)).Any())
      throw new ArgumentException($"{nameof(Parameters)} Some PublicKeys has failed!");

    if (IsNullOrEmpty(signature.S1) || IsNullOrEmpty(signature.S2))
      throw new ArgumentException($"{nameof(signature)} has failed!");
  }

  private static BigInteger HashData(
    BigInteger message, HashAlgorithmName hname = default) =>
      HashData(message.GetByteCount(), hname);

  private static BigInteger HashData(
    ReadOnlySpan<byte> message, HashAlgorithmName hname = default)
  {
    hname = hname == default ? HashAlgorithmName.SHA512 : hname;
    var hash = HashDataAlgo(message, hname);
    var result = new BigInteger(hash);
    if (result.Sign < 0) result = -result;
    return result;
  }

  private static BigInteger SumProduct(
    List<SchnorrParameters> param, BigInteger hash)
  {
    var result = BigInteger.Zero;
    foreach (var mul in param.Select(x => new BigInteger(x.PrivateKey)))
      result += mul * hash;
    return result;
  }

  private static BigInteger ModPowProduct(
    List<SchnorrParameters> param, BigInteger hash, BigInteger p)
  {
    var result = BigInteger.One;
    foreach (var mul in param.Select(x => new BigInteger(x.PublicKey)))
      result *= BigInteger.ModPow(mul, hash, p); ;
    return result;
  }

}
