

using System.Text;
using System.Security;
using System.Security.Cryptography;
using System.Text.Json.Serialization;

namespace michele.natale.BcPqcs;

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
public sealed class LMSSignInfo : ILMSSignInfo
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

  /// <summary>
  /// Desired LMS-Parameter
  /// </summary>
  [JsonInclude]
  public LmsParam Parameter { get; set; } = LmsParam.lms_sha256_h5_w1;


  public LMSSignInfo()
  {
  }

  /// <summary>
  /// C-Tor
  /// </summary>
  /// <param name="input">Desired sign information.</param>
  public LMSSignInfo(LMSSignInfo input)
    : this(input.ID, input.Name, input.Parameter,
           input.Message, input.PublicKey, input.Sign)
  {
  }

  /// <summary>
  /// C-Tor
  /// </summary>
  /// <param name="id">Desired id</param>
  /// <param name="name">Desired Name</param>
  /// <param name="parameter">Desired LMS-Parameter</param>
  /// <param name="msg">Desired Message</param>
  /// <param name="pupkey">The Publickey</param>
  /// <param name="sign">The Signature</param>
  public LMSSignInfo(
    Guid id, string name, LmsParam parameter,
    string msg, byte[] pupkey, byte[] sign)
  {
    this.Message = msg;
    this.Parameter = parameter;
    this.ID = new Guid(id.ToByteArray());
    this.Sign = Convert.ToHexString(sign);
    this.PublicKey = Convert.ToHexString(pupkey);
    this.Name = name ?? BcPqcServices.ToRngName();
  }


  /// <summary>
  /// C-Tor
  /// </summary>
  /// <param name="name">Desired Name</param>
  /// <param name="parameter">Desired LMS-Param</param>
  /// <param name="msg">Desired Message</param>
  /// <param name="pupkey">The Publickey</param>
  /// <param name="sign">The Signature</param>
  public LMSSignInfo(
    string name, LmsParam parameter,
    string msg, byte[] pupkey, byte[] sign)
  {
    this.Message = msg;
    this.ID = Guid.NewGuid();
    this.Parameter = parameter;
    this.Sign = Convert.ToHexString(sign);
    this.PublicKey = Convert.ToHexString(pupkey);
    this.Name = name ?? BcPqcServices.ToRngName();
  }


  /// <summary>
  /// C-Tor
  /// </summary>
  /// <param name="id">Desired id</param>
  /// <param name="name">Desired Name</param>
  /// <param name="parameter">Desired LMS-Param</param>
  /// <param name="msg">Desired Message</param>
  /// <param name="pupkey">The Publickey</param>
  /// <param name="sign">The Signature</param>
  public LMSSignInfo(
    Guid id, string name, LmsParam parameter,
    string msg, string hexpupkey, string hexsign)
  {
    this.Message = msg;
    this.Sign = hexsign;
    this.PublicKey = hexpupkey;
    this.Parameter = parameter;
    this.ID = new Guid(id.ToByteArray());
    this.Name = name ?? BcPqcServices.ToRngName();
  }

  public LMSSignInfo Copy()
  {
    var result = new LMSSignInfo(
      this.ID, this.Name, this.Parameter, this.Message,
      Convert.FromHexString(this.PublicKey),
      Convert.FromHexString(this.Sign));
    return result;
  }

  internal byte[] Serialize()
  {
    var copy = new LMSSignInfo(this);
    return BcPqcServices.SerializeJson(copy);
  }

  public void Save(string filename)
  {
    if (File.Exists(filename))
      File.Delete(filename);

    var copy = new LMSSignInfo(this);
    var serialize = BcPqcServices.SerializeJson(copy);
    File.WriteAllBytes(filename, serialize);
  }

  public void Load(string filename)
  {
    if (File.Exists(filename))
    {
      var data = File.ReadAllBytes(filename);
      var signinfo = BcPqcServices.DeserializeJson<LMSSignInfo>(data);

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
    this.Parameter = LmsParam.lms_sha256_h5_w1;
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
    if (LMS.Verify(
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

      //return BcPqcServices.XorSpec(bytes);
      return BcPqcServices.Sha512Concat(bytes);
    }

    throw new VerificationException(
      $"Methode {nameof(VerifiyHash)}: Verify has failed!");
  }
}