using System.Security.Cryptography;

namespace michele.natale.Schnorrs.EcServices;



partial class EcSchnorrServices
{
  public static ECCurve RngEcCurve()
  {
    var ec = ToEcCurveList();
    return ToEcCurve(ec[NextCryptoInt32(ec.Length)]);
  }

  public static EcCurveParameters RngEcCurveParameters()
  {
    return ToEcCurveParameters(RngEcCurve());
  }


  public static EcCurveParameters ToEcCurveParameters(ECCurve curve)
  {
    return new EcCurveParameters(curve);
  }

  public static ECCurve ToEcCurve(string ecname)
  {

    return ecname.ToLower() switch
    {
      string e when e.Trim().SequenceEqual("nistP256".Trim().ToLower()) => ECCurve.NamedCurves.nistP256,
      string e when e.Trim().SequenceEqual("nistP384".Trim().ToLower()) => ECCurve.NamedCurves.nistP384,
      string e when e.Trim().SequenceEqual("nistP521".Trim().ToLower()) => ECCurve.NamedCurves.nistP521,
      string e when e.Trim().SequenceEqual("brainpoolP160r1".Trim().ToLower()) => ECCurve.NamedCurves.brainpoolP160r1,
      string e when e.Trim().SequenceEqual("brainpoolP512t1".Trim().ToLower()) => ECCurve.NamedCurves.brainpoolP512t1,
      string e when e.Trim().SequenceEqual("brainpoolP512r1".Trim().ToLower()) => ECCurve.NamedCurves.brainpoolP512r1,
      string e when e.Trim().SequenceEqual("brainpoolP384t1".Trim().ToLower()) => ECCurve.NamedCurves.brainpoolP384t1,
      string e when e.Trim().SequenceEqual("brainpoolP384r1".Trim().ToLower()) => ECCurve.NamedCurves.brainpoolP384r1,
      string e when e.Trim().SequenceEqual("brainpoolP320t1".Trim().ToLower()) => ECCurve.NamedCurves.brainpoolP320t1,
      string e when e.Trim().SequenceEqual("brainpoolP320r1".Trim().ToLower()) => ECCurve.NamedCurves.brainpoolP320r1,
      string e when e.Trim().SequenceEqual("brainpoolP256r1".Trim().ToLower()) => ECCurve.NamedCurves.brainpoolP256r1,
      string e when e.Trim().SequenceEqual("brainpoolP224t1".Trim().ToLower()) => ECCurve.NamedCurves.brainpoolP224t1,
      string e when e.Trim().SequenceEqual("brainpoolP224r1".Trim().ToLower()) => ECCurve.NamedCurves.brainpoolP224r1,
      string e when e.Trim().SequenceEqual("brainpoolP192t1".Trim().ToLower()) => ECCurve.NamedCurves.brainpoolP192t1,
      string e when e.Trim().SequenceEqual("brainpoolP192r1".Trim().ToLower()) => ECCurve.NamedCurves.brainpoolP192r1,
      string e when e.Trim().SequenceEqual("brainpoolP160t1".Trim().ToLower()) => ECCurve.NamedCurves.brainpoolP160t1,
      string e when e.Trim().SequenceEqual("brainpoolP256t1".Trim().ToLower()) => ECCurve.NamedCurves.brainpoolP256t1,
      string e when e.Trim().SequenceEqual("secp160r1".Trim().ToLower()) => ECCurve.CreateFromFriendlyName("secp160r1"),
      string e when e.Trim().SequenceEqual("secp160r2".Trim().ToLower()) => ECCurve.CreateFromFriendlyName("secp160r2"),
      string e when e.Trim().SequenceEqual("secp160k1".Trim().ToLower()) => ECCurve.CreateFromFriendlyName("secp160k1"),
      string e when e.Trim().SequenceEqual("secp192r1".Trim().ToLower()) => ECCurve.CreateFromFriendlyName("secp192r1"),
      string e when e.Trim().SequenceEqual("secp192k1".Trim().ToLower()) => ECCurve.CreateFromFriendlyName("secp192k1"),
      string e when e.Trim().SequenceEqual("secp224r1".Trim().ToLower()) => ECCurve.CreateFromFriendlyName("secp224r1"),
      string e when e.Trim().SequenceEqual("secp224k1".Trim().ToLower()) => ECCurve.CreateFromFriendlyName("secp224k1"),
      string e when e.Trim().SequenceEqual("secp256r1".Trim().ToLower()) => ECCurve.CreateFromFriendlyName("secp256r1"),
      string e when e.Trim().SequenceEqual("secp256k1".Trim().ToLower()) => ECCurve.CreateFromFriendlyName("secp256k1"),
      _ => throw new ArgumentException("Failed", nameof(ecname)),
    };

  }

  public static string[] ToEcCurveList()
  {
    return
    [
      "nistP256"       ,
      "nistP384"       ,
      "nistP521"       ,
      "brainpoolP160r1",
      "brainpoolP512t1",
      "brainpoolP512r1",
      "brainpoolP384t1",
      "brainpoolP384r1",
      "brainpoolP320t1",
      "brainpoolP320r1",
      "brainpoolP256r1",
      "brainpoolP224t1",
      "brainpoolP224r1",
      "brainpoolP192t1",
      "brainpoolP192r1",
      "brainpoolP160t1",
      "brainpoolP256t1",
      "secp160r1",
      "secp160r2",
      "secp160k1",
      "secp192r1",
      "secp192k1",
      "secp224r1",
      "secp224k1",
      "secp256r1",
      "secp256k1 ",
    ];
  }
}

