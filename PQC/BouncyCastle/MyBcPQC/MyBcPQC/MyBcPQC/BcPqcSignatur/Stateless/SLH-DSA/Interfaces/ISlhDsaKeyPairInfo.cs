

using Org.BouncyCastle.Crypto.Parameters;

namespace michele.natale.BcPqcs;

using Pointers;

/// <summary>
/// Interfaces for the class SlhDsaKeyPairInfo
/// </summary>
public interface ISlhDsaKeyPairInfo:IDisposable
{

  /// <summary>
  /// Specifies the Id as GUID
  /// </summary>
  Guid Id
  {
    get; set;
  }

  /// <summary>
  /// Specifies the PublicKey as Bytes
  /// </summary>
  byte[] PubKey
  {
    get; set;
  }

  /// <summary>
  /// Return the SLH-DSA-Parameter.
  /// </summary>
  /// <returns>Return the SLH-DSA-Parameter.</returns>
  SlhDsaParameters ToParameter();

  /// <summary>
  /// Returns the current PrivateKey as bytes in a protected (fix) UsIPtr-instance.
  /// </summary>
  /// <returns>Returns Privatekey as bytes</returns>
  UsIPtr<byte> ToPrivKey();

  /// <summary>
  /// Clear all data.
  /// </summary>
  void Clear();

  /// <summary>
  /// Saves the KeyPair to the hardware.
  /// </summary>
  /// <param name="filename">Desired filename</param>
  /// <param name="with_privkey">True if the PrivateKey is also to be saved, otherwise False.</param>
  void SaveKeyPair(string filename, bool with_privkey);

  /// <summary>
  /// Loads the KeyPair from the hardware.
  /// </summary>
  /// <param name="filename">Desired filename.</param>
  void LoadKeyPair(string filename);

  /// <summary>
  /// Checks whether both SLHDsaKeyPairInfo-Instances are the same.
  /// </summary>
  /// <param name="info">Desired SLHDsaKeyPairInfo</param>
  /// <returns>True, if equals, ortherwise false.</returns>
  public bool Equals(SlhDsaKeyPairInfo info);

  /// <summary>
  /// Returns the SLH-DSA-Parameters.
  /// </summary>
  /// <param name="param">Desired Parameter as SLHDsaParam</param>
  /// <returns>Returns the SLH-DSA-Parameters.</returns>
  static abstract SlhDsaParameters ToSLHDsaParameters(SLHDsaParam param);

  /// <summary>
  /// Returns the SLH-DSA-Parameters as SLHDsaParam.
  /// </summary>
  /// <param name="parameter">Desired SLH-DSA-Parameter</param>
  /// <returns>Returns the SLH-DSA-Parameters as SLHDsaParam.</returns>
  static abstract SLHDsaParam FromSLHDsaParameters(SlhDsaParameters parameter);

  /// <summary>
  /// Loads the KeyPair from the hardware.
  /// </summary>
  /// <param name="filename">Desired filename.</param>
  /// <returns>Returns the KeyPair in a SLHDsaKeyPairInfo-Instance.</returns>
  static abstract SlhDsaKeyPairInfo Load_KeyPair(string filename);
}