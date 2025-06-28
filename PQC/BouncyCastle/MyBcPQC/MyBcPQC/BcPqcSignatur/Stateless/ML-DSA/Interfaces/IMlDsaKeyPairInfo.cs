

using Org.BouncyCastle.Crypto.Parameters;

namespace michele.natale.BcPqcs;

using Pointers;

/// <summary>
/// Interface for the class MlDsaKeyPairInfo
/// </summary>
public interface IMlDsaKeyPairInfo:IDisposable
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
  /// Return the ML-DSA-Parameter.
  /// </summary>
  /// <returns>Return the ML-DSA-Parameter.</returns>
  MLDsaParameters ToParameter();

  /// <summary>
  /// Returns the current PrivateKey as bytes in a protected (fix) UsIPtr-instance.
  /// </summary>
  /// <returns></returns>
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
  /// Checks whether both MlDsaKeyPairInfo-Instances are the same.
  /// </summary>
  /// <param name="info">Desired MlDsaKeyPairInfo</param>
  /// <returns>True, if equals, ortherwise false.</returns>
  public bool Equals(MlDsaKeyPairInfo info);

  /// <summary>
  /// Returns the ML-DSA-Parameters.
  /// </summary>
  /// <param name="param">Desired Parameter as MLDsaParam</param>
  /// <returns>Returns the ML-DSA-Parameters.</returns>
  static abstract MLDsaParameters ToMLDsaParameters(MLDsaParam param);

  /// <summary>
  /// Returns the ML-DSA-Parameters as MLDsaParam.
  /// </summary>
  /// <param name="parameter">Desired ML-DSA-Parameter</param>
  /// <returns>Returns the ML-DSA-Parameters as MLDsaParam.</returns>
  static abstract MLDsaParam FromMLDsaParameters(MLDsaParameters parameter);

  /// <summary>
  /// Loads the KeyPair from the hardware.
  /// </summary>
  /// <param name="filename">Desired filename.</param>
  /// <returns>Returns the KeyPair in a MlDsaKeyPairInfo-Instance.</returns>
  static abstract MlDsaKeyPairInfo Load_KeyPair(string filename);
}