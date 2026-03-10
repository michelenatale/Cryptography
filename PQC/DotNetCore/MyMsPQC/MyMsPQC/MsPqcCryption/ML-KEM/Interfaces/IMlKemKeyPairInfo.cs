

using System.Security.Cryptography;


namespace michele.natale.MsPqcs;

using Pointers;
using Services;


/// <summary>
/// Interfaces for the Class MlKemKeyPairInfo
/// </summary>
public interface IMlKemKeyPairInfo:IDisposable
{

  /// <summary>
  /// Indicates the ID of the current instance.
  /// </summary>
  public Guid Id
  {
    get; set;
  }

  /// <summary>
  /// Specifies the current PublicKey.
  /// </summary>
  public byte[] PubKey
  {
    get; set;
  }

  /// <summary>
  /// Specifies the current symmetric Crypto-Algorithm..
  /// </summary>
  public CryptionAlgorithm CryptAlgo
  {
    get; set;
  }

  /// <summary>
  /// Returns the current ML-KEM-Algo.
  /// </summary>
  /// <returns>Returns the ML-KEM-Algo as a MLKemAlgorithm-Instance</returns>
  MLKemAlgorithm ToAlgo();

  /// <summary>
  /// Returns the current PrivateKey as bytes in a protected (fix) UsIPtr-Instance.
  /// </summary>
  /// <returns>Returns a UsIPtr-instance</returns>
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
  /// Checks whether both MlKemKeyPairInfo-Instances are the same.
  /// </summary>
  /// <param name="info">Desired MlKemKeyPairInfo</param>
  /// <returns>True, if equals, ortherwise false.</returns>
  bool Equals(MlKemKeyPairInfo info);

  /// <summary>
  /// Returns the ML-KEM-Parameters.
  /// </summary>
  /// <param name="algo">Desired Parameter as MLKemParam (Algo)</param>
  /// <returns>Returns the ML-KEM-Parameters.</returns>
  static abstract MLKemAlgorithm ToMLKemAlgorithm(MLKemParam algo);

  /// <summary>
  /// Returns the ML-KEM-Parameters as MLKemParam (Algo).
  /// </summary>
  /// <param name="algo">Desired ML-KEM-Parameter</param>
  /// <returns>Returns the ML-KEM-Parameters as MLKemParam.</returns>
  static abstract MLKemParam FromMLKemAlgorithm(MLKemAlgorithm algo);

  /// <summary>
  /// Loads the KeyPair from the hardware.
  /// </summary>
  /// <param name="filename">Desired filename.</param>
  /// <returns>Returns the KeyPair in a MlKemKeyPairInfo-Instance.</returns>
  static abstract MlKemKeyPairInfo Load_KeyPair(string filename);


}
