
using System.Security.Cryptography;
 
namespace michele.natale;

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
  /// Return the ML-DSA-Algo.
  /// </summary>
  /// <returns>Return the ML-DSA-Algo.</returns>
  MLDsaAlgorithm ToAlgo();

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
  /// Returns the ML-DSA-Algo.
  /// </summary>
  /// <param name="algo">Desired Algo as MLDsaParam</param>
  /// <returns>Returns the ML-DSA-Algo.</returns>
  static abstract MLDsaAlgorithm ToMLDsaAlgorithm(MLDsaParam algo);

  /// <summary>
  /// Returns the ML-DSA-Algo as MLDsaParam.
  /// </summary>
  /// <param name="algo">Desired ML-DSA-Algo</param>
  /// <returns>Returns the ML-DSA-Parameters as MLDsaParam.</returns>
  static abstract MLDsaParam FromMLDsaAlgorithm(MLDsaAlgorithm algo);

  /// <summary>
  /// Loads the KeyPair from the hardware.
  /// </summary>
  /// <param name="filename">Desired filename.</param>
  /// <returns>Returns the KeyPair in a MlDsaKeyPairInfo-Instance.</returns>
  static abstract MlDsaKeyPairInfo Load_KeyPair(string filename);
}