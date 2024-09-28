

using System.Text;
using System.Security.Cryptography;

namespace michele.natale.Schnorrs;

using static michele.natale.Schnorrs.Services.SchnorrServices;

//Is internal 
internal class PmeiPQG
{
  public byte[] P { get; set; } = [];
  public byte[] Q { get; set; } = [];
  public byte[] G { get; set; } = [];
  public byte[] H { get; set; } = [];

  public PmeiPQG()
  {
  }

  public PmeiPQG(PmeiPQG pqgh)
    : this(pqgh.P, pqgh.Q, pqgh.G,
       new HashAlgorithmName(
          Encoding.UTF8.GetString(pqgh.H)))
  {
  }

  public PmeiPQG(
    (byte[] p, byte[] q,
     byte[] g, HashAlgorithmName hname) pqgh)
    : this(pqgh.p, pqgh.q, pqgh.g, pqgh.hname)
  {
  }

  public PmeiPQG(
    byte[] p, byte[] q, byte[] g, HashAlgorithmName hname)
  {
    this.P = p; this.Q = q; this.G = g;
    this.H = Encoding.UTF8.GetBytes(hname.Name!);
  }

  public PmeiPQG Copy()
  {
    return new PmeiPQG(this);
  }

  public (byte[] P, byte[] Q, byte[] G, HashAlgorithmName HName) ToParameters()
  {
    return (this.P, this.Q, this.G,
       new HashAlgorithmName(
        Encoding.UTF8.GetString(this.H)));
  }

  public byte[] Serialize()
  {
    return SerializeJson(this);
  }

  public void Deserialize(byte[] data)
  {
    var with = DeserializeJson<PmeiPQG>(data);
    this.P = with!.P; this.Q = with.Q;
    this.G = with.G; this.H = with.H;
  }

  //Static

  public static byte[] JSerialize(PmeiPQG pqgh)
  {
    return SerializeJson(pqgh);
  }

  public static PmeiPQG JDeserialize(ReadOnlySpan<byte> data)
  {
    return DeserializeJson<PmeiPQG>(data.ToArray())!;
  }
}
