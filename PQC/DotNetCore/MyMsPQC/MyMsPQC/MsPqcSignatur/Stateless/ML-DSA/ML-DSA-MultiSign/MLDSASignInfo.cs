 
using System.Text;
using System.Security;
using System.Security.Cryptography;
using System.Text.Json.Serialization;

namespace michele.natale.MsPqcs;

using Services;

/// <summary>
/// <para>The class for the signature creator only.</para>
/// For temporary use, the signature creator can save the 
/// data of this class and call it up again later using the ID.
/// <para>
/// The signature creator must be in possession 
/// of the basic data such as the seed.
/// </para>
/// </summary>
public sealed class MLDSASignInfo : IMLDSASignInfo
{

  [JsonInclude]
  public Guid ID { get; set; } = Guid.Empty;

  [JsonInclude]
  public string Name { get; set; } = string.Empty;

  [JsonInclude]
  public string Sign { get; set; } = string.Empty;

  [JsonInclude]
  public string Message { get; set; } = string.Empty;

  [JsonInclude]
  public string PublicKey { get; set; } = string.Empty;

  [JsonInclude]
  public MLDsaParam Parameter { get; set; } = MLDsaParam.Ml_Dsa_44;


  public MLDsaAlgorithm ToParameter() =>
    MsPqcServices.ToMLDsaAlgorithm(this.Parameter);

  /// <summary>
  /// C-Tor
  /// </summary>
  public MLDSASignInfo()
  {
  }

  /// <summary>
  /// C-Tor
  /// </summary>
  /// <param name="input">Desired sign information</param>
  public MLDSASignInfo(MLDSASignInfo input)
    : this(input.ID, input.Name, input.Parameter, input.Message, input.PublicKey, input.Sign)
  {
  }

  /// <summary>
  /// C-Tor
  /// </summary>
  /// <param name="name">Desired Name</param>
  /// <param name="parameter">Desired ML-DSA-Parameter</param>
  /// <param name="msg">Desired Message</param>
  /// <param name="pupkey">The Publickey</param>
  /// <param name="sign">The Signature</param>
  public MLDSASignInfo(
    string name, MLDsaAlgorithm parameter, string msg,
    byte[] pupkey, byte[] sign)
  {
    this.Message = msg;
    this.ID = Guid.NewGuid();
    this.Sign = Convert.ToHexString(sign);
    this.PublicKey = Convert.ToHexString(pupkey);
    this.Name = name ?? MsPqcServices.ToRngName();
    this.Parameter = MsPqcServices.FromMLDsaAlgorithm(parameter);
  }

  /// <summary>
  /// C-Tor
  /// </summary>
  /// <param name="id">Desired id</param>
  /// <param name="name">Desired Name</param>
  /// <param name="parameter">Desired ML-DSA-Parameter</param>
  /// <param name="msg">Desired Message</param>
  /// <param name="pupkey">The Publickey</param>
  /// <param name="sign">The Signature</param>
  public MLDSASignInfo(
    Guid id, string name, MLDsaAlgorithm parameter,
    string msg, byte[] pupkey, byte[] sign)
  {
    this.Message = msg;
    this.ID = new Guid(id.ToByteArray());
    this.Sign = Convert.ToHexString(sign);
    this.PublicKey = Convert.ToHexString(pupkey);
    this.Name = name ?? MsPqcServices.ToRngName();
    this.Parameter = MsPqcServices.FromMLDsaAlgorithm(parameter);
  }


  /// <summary>
  /// C-Tor
  /// </summary>
  /// <param name="name">Desired Name</param>
  /// <param name="parameter">Desired ML-DSA-Param</param>
  /// <param name="msg">Desired Message</param>
  /// <param name="pupkey">The Publickey</param>
  /// <param name="sign">The Signature</param>
  public MLDSASignInfo(
    string name, MLDsaParam parameter,
    string msg, byte[] pupkey, byte[] sign)
  {
    this.Message = msg;
    this.ID = Guid.NewGuid();
    this.Parameter = parameter;
    this.Sign = Convert.ToHexString(sign);
    this.PublicKey = Convert.ToHexString(pupkey);
    this.Name = name ?? MsPqcServices.ToRngName();
  }


  /// <summary>
  /// C-Tor
  /// </summary>
  /// <param name="id">Desired id</param>
  /// <param name="name">Desired Name</param>
  /// <param name="parameter">Desired ML-DSA-Param</param>
  /// <param name="msg">Desired Message</param>
  /// <param name="pupkey">The Publickey</param>
  /// <param name="sign">The Signature</param>
  public MLDSASignInfo(
    Guid id, string name, MLDsaParam parameter,
    string msg, byte[] pupkey, byte[] sign)
  {
    this.Message = msg;
    this.Parameter = parameter;
    this.ID = new Guid(id.ToByteArray());
    this.Sign = Convert.ToHexString(sign);
    this.PublicKey = Convert.ToHexString(pupkey);
    this.Name = name ?? MsPqcServices.ToRngName();
  }


  /// <summary>
  /// C-Tor
  /// </summary>
  /// <param name="id">Desired id</param>
  /// <param name="name">Desired Name</param>
  /// <param name="parameter">Desired ML-DSA-Param</param>
  /// <param name="msg">Desired Message</param>
  /// <param name="pupkey">The Publickey</param>
  /// <param name="sign">The Signature</param>
  public MLDSASignInfo(
    Guid id, string name, MLDsaParam parameter,
    string msg, string hexpupkey, string hexsign)
  {
    this.Message = msg;
    this.Sign = hexsign;
    this.PublicKey = hexpupkey;
    this.Parameter = parameter;
    this.ID = new Guid(id.ToByteArray());
    this.Name = name ?? MsPqcServices.ToRngName();
  }

  public MLDSASignInfo Copy()
  {
    return new MLDSASignInfo(
      this.ID, this.Name, this.Parameter, this.Message,
      Encoding.UTF8.GetBytes(this.PublicKey),
      Encoding.UTF8.GetBytes(this.Sign));
  }

  public byte[] Serialize()
  {
    var copy = new MLDSASignInfo(this);
    return MsPqcServices.SerializeJson(copy);
  }

  public void Save(string filename)
  {
    if (File.Exists(filename))
      File.Delete(filename);

    File.WriteAllBytes(filename, this.Serialize());
  }

  public void Load(string filename)
  {
    if (File.Exists(filename))
    {
      var data = File.ReadAllBytes(filename);
      var signinfo = MsPqcServices.DeserializeJson<MLDSASignInfo>(data);

      if (signinfo is not null)
      {
        this.ID = signinfo.ID;
        this.Name = signinfo.Name;
        this.Sign = signinfo.Sign;
        this.Message = signinfo.Message;
        this.PublicKey = signinfo.PublicKey;
        this.Parameter = signinfo.Parameter;
      }
    }
  }

  public void Clear()
  {
    this.ID = Guid.Empty;
    this.Name = string.Empty;
    this.Sign = string.Empty;
    this.Message = string.Empty;
    this.PublicKey = string.Empty;
    this.Parameter = MLDsaParam.Ml_Dsa_44;
  }

  /// <summary>
  /// Creates a cryptographically secure hash derived 
  /// from SHA512 using the MLDSASignInfo class.
  /// </summary>
  /// <returns>Returns as bytes</returns>
  /// <exception cref="VerificationException"></exception>
  internal byte[] VerifiyHash()
  {
    //Check in advance whether all signatures can be verified.
    if (MlDsaEx.Verify(this.ToParameter(),
      Convert.FromHexString(this.PublicKey),
      Convert.FromHexString(this.Sign),
      Convert.FromHexString(this.Message)))
    {
      byte[][] bytes;
      var nb = Encoding.UTF8.GetBytes(this.Name);
      if (nb.Length > 1 << 13) nb = SHA512.HashData(nb);
      var sb = Convert.FromHexString(this.Sign);
      var mb = Convert.FromHexString(this.Message);
      if (mb.Length > 1 << 13) mb = SHA512.HashData(mb);
      var pb = Convert.FromHexString(this.PublicKey);
      var ib = this.ID.ToByteArray().Concat([(byte)this.Parameter]).ToArray();
      if (nb.Length > 0) bytes = [nb, sb, mb, pb, ib];
      else bytes = [sb, mb, pb, ib];

      return MsPqcServices.Sha512Concat(bytes);
      //return MsPqcservices.XorSpec(bytes);
    }

    throw new VerificationException(
      $"Methode {nameof(VerifiyHash)}: Verify has failed!");
  }
}