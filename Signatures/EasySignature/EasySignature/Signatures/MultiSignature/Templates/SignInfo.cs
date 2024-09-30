

namespace michele.natale.Cryptography.Signatures;

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
public class SignInfo
{
  /// <summary>
  /// ID as recognition of the data for the signature.
  /// </summary>
  public Guid ID { get; set; } = Guid.Empty;

  /// <summary>
  /// Desired key strength. 
  /// </summary>
  public int KeySize { get; internal set; } = 0;

  /// <summary>
  /// Name of the creator.
  /// </summary>
  public string Name { get; set; } = string.Empty;

  /// <summary>
  /// The calculated signature.
  /// </summary>
  public string Sign { get; set; } = string.Empty;

  /// <summary>
  /// The basic seed for calculating the keys for the signature.
  /// </summary>
  public string Seed { get; set; } = string.Empty;

  /// <summary>
  /// The message used for the signature.
  /// </summary>
  public string Message { get; set; } = string.Empty;

  /// <summary>
  /// The generated PublicKey.
  /// </summary>
  public string PublicKey { get; set; } = string.Empty;

  /// <summary>
  /// C-Tor
  /// </summary>
  /// <param name="name">Desired Name</param>
  /// <param name="seed">Desired Seed</param>
  /// <param name="msg">Desired Message</param>
  /// <param name="pupkey">The Publickey</param>
  /// <param name="sign">The Signature</param>
  public SignInfo(
    string name, byte[] seed, string msg,
    byte[] pupkey, byte[] sign)
  {
    this.Message = msg;
    this.ID = Guid.NewGuid();
    this.KeySize = pupkey.Length;
    this.Sign = Convert.ToHexString(sign);
    this.Seed = Convert.ToHexString(seed);
    this.PublicKey = Convert.ToHexString(pupkey);
    this.Name = name ?? SignatureServices.ToRngName();
  }

  /// <summary>
  /// Generates the data relevant for the MultiSignInfo 
  /// customer class using the basic SignInfo data.
  /// </summary>
  /// <returns>MultiSignInfo</returns>
  public MultiSignInfo ToMultiSignInfo()
  {
    return new MultiSignInfo
    {
      ID = ID,
      Name = Name,
      Sign = Sign,
      Message = Message,
      KeySize = KeySize,
      PublicKey = PublicKey,
    };
  }

}
