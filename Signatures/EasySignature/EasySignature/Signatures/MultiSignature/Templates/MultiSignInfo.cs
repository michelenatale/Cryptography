

namespace michele.natale.Cryptography.Signatures;

using Services;

/// <summary>
/// MultiSignInfo is the customer class in exchange 
/// with the other participants. It can be used publicly 
/// and only contains the corresponding values that are 
/// intended for the public.
/// </summary>
public class MultiSignInfo
{
  /// <summary>
  /// ID as recognition of the data for the signature.
  /// </summary>
  public Guid ID { get; set; } = Guid.Empty;

  /// <summary>
  /// True if extra force, ortherwise false.
  /// </summary>
  public bool ExtraForce { get; set; } = false;

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
  public MultiSignInfo()
  {
  }

  /// <summary>
  /// C-Tor
  /// </summary>
  /// <param name="name">Desired Name</param>
  /// <param name="msg">Desired Message</param>
  /// <param name="pupkey">Desired PublicKey</param>
  /// <param name="sign">Desired Sign</param>
  public MultiSignInfo(
    string name, byte[] msg,
    byte[] pupkey, byte[] sign, bool extra_force)
  {
    this.ID = Guid.NewGuid();
    this.KeySize = pupkey.Length;
    this.ExtraForce = extra_force;
    this.Sign = Convert.ToHexString(sign);
    this.Message = Convert.ToHexString(msg);
    this.PublicKey = Convert.ToHexString(pupkey);
    this.Name = name ?? SignatureServices.ToRngName();
  }

}
